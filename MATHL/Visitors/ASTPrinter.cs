using MATHL.Composite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATHL.Visitors {
    internal class ASTPrinter : MATHLBaseVisitor<int, ASTElement> {
        StreamWriter m_writer;
        private string m_outputFilename;
        private int m_clusterSerial;
        private int ms_clusterSerialCounter;

        public ASTPrinter(string outputFilename) {
            m_outputFilename = outputFilename;
            m_writer = new StreamWriter(m_outputFilename);
        }

        private void CreateContextSubgraph(ASTComposite node, int contextindex, string contextName) {
            
            m_writer.WriteLine($"\tsubgraph cluster{m_clusterSerial++} {{");
            m_writer.WriteLine("\t\tnode [style=filled,color=white];");
            m_writer.WriteLine("\t\tnode [style=filled,color=lightgrey];");
            bool first = true;
            if (node.GetNumberOfContextNodes(contextindex) != 0) {
                foreach (ASTElement child in node.GetContextChildren(contextindex)) {
                    if (first) {
                        m_writer.Write("\t\t");
                    }

                    first = false;
                    m_writer.Write("\"" + child.MNodeName + "\"");
                }

                m_writer.WriteLine($";");
            }
            m_writer.WriteLine($"\t\tlabel = \"{contextName}\";");
            m_writer.WriteLine($"\t}}");
        }

        public override int VisitCompileUnit(CCompileUnit node, params ASTElement[] args) {
            CCompileUnit n = node as CCompileUnit;
            if (n == null) {
                throw new InvalidCastException("Expected CompileUnit type");
            }

            m_writer.WriteLine("digraph G{");

            CreateContextSubgraph(n, CCompileUnit.COMMANDS,
                n.mc_contextNames[CCompileUnit.COMMANDS]);

            base.VisitCompileUnit(node, n);

            m_writer.WriteLine("}");
            m_writer.Close();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "dot.exe";
            startInfo.Arguments = " -Tgif "+Path.GetFileName(m_outputFilename) +
                                  " -o "+ Path.GetFileNameWithoutExtension(m_outputFilename) +".gif";

            Process proc = Process.Start(startInfo);

            return 0;
        }

        public override int VisitCommand_Expression(CCommand_Expression node, params ASTElement[] args) {

            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");

            CreateContextSubgraph(node, CCommand_Expression.COMMAND,
                node.mc_contextNames[CCommand_Expression.COMMAND]);

            return base.VisitCommand_Expression(node, node);
        }

        public override int VisitExpression_Equation(CExpression_Equation node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");

            CreateContextSubgraph(node, CExpression_Equation.LHS,
                node.mc_contextNames[CExpression_Equation.LHS]);
            CreateContextSubgraph(node, CExpression_Equation.RHS,
                node.mc_contextNames[CExpression_Equation.RHS]);

            return base.VisitExpression_Equation(node, node);
        }

        public override int VisitDeclaration_Variable(CDeclarationVariable node, params ASTElement[] args) {
            
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");

            CreateContextSubgraph(node, CDeclarationVariable.TYPE,
                node.mc_contextNames[CDeclarationVariable.TYPE]);
            
            CreateContextSubgraph(node, CDeclarationVariable.DECLARATIONS,
                node.mc_contextNames[CDeclarationVariable.DECLARATIONS]);

            return base.VisitDeclaration_Variable(node, node);
        }

        public override int VisitDeclarator_Variable(CDeclaratorVariable node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");

            CreateContextSubgraph(node, CDeclaratorVariable.TYPE,
                node.mc_contextNames[CDeclaratorVariable.TYPE]);

            CreateContextSubgraph(node, CDeclaratorVariable.VARIABLENAME,
                node.mc_contextNames[CDeclaratorVariable.VARIABLENAME]);


            if (node.GetNumberOfContextNodes(CDeclaratorVariable.INITIALIZATION) != 0) {
                CreateContextSubgraph(node, CDeclaratorVariable.INITIALIZATION,
                    node.mc_contextNames[CDeclaratorVariable.INITIALIZATION]);
            }

            return base.VisitDeclarator_Variable(node, node);
        }

        public override int VisitDeclaration_Function(CDeclarationFunction node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");

            CreateContextSubgraph(node, CDeclarationFunction.TYPE,
                node.mc_contextNames[CDeclarationFunction.TYPE]);

            CreateContextSubgraph(node, CDeclarationFunction.FUNCTION_NAME,
                node.mc_contextNames[CDeclarationFunction.FUNCTION_NAME]);

            CreateContextSubgraph(node, CDeclarationFunction.PARAMETERS,
                node.mc_contextNames[CDeclarationFunction.PARAMETERS]);
            CreateContextSubgraph(node, CDeclarationFunction.BODY,
                node.mc_contextNames[CDeclarationFunction.BODY]);

            return base.VisitDeclaration_Function(node, node);
        }

        public override int VisitCommand_Return(CCommand_Return node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");

            CreateContextSubgraph(node, CCommand_Return.EXPRESSION,
                node.mc_contextNames[CCommand_Return.EXPRESSION]);

            return base.VisitCommand_Return(node, node);
        }

        public override int VisitCommand_CommandBlock(CCommand_CommandBlock node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");

            CreateContextSubgraph(node, CCommand_CommandBlock.COMMAND,
                node.mc_contextNames[CCommand_CommandBlock.COMMAND]);

            return base.VisitCommand_CommandBlock(node, node);
        }

        public override int VisitExpression_Range(CExpression_Range node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");

            CreateContextSubgraph(node, CExpression_Range.START,
                node.mc_contextNames[CExpression_Range.START]);
            CreateContextSubgraph(node, CExpression_Range.END,
                node.mc_contextNames[CExpression_Range.END]);
            CreateContextSubgraph(node, CExpression_Range.STEP,
                node.mc_contextNames[CExpression_Range.STEP]);

            return base.VisitExpression_Range(node, node);
        }



        public override int VisitExpression_FunctionCall(CExpression_FunctionCall node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");

            CreateContextSubgraph(node, CExpression_FunctionCall.NAME,
                node.mc_contextNames[CExpression_FunctionCall.NAME]);
            CreateContextSubgraph(node, CExpression_FunctionCall.PARAMS,
                node.mc_contextNames[CExpression_FunctionCall.PARAMS]);

            return base.VisitExpression_FunctionCall(node, node);
        }

        public override int VisitExpression_UnaryPlus(CExpression_UnaryPlus node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");

            CreateContextSubgraph(node, CExpression_UnaryPlus.EXPR,
                node.mc_contextNames[CExpression_UnaryPlus.EXPR]);

            return base.VisitExpression_UnaryPlus(node, node);
        }

        public override int VisitExpression_UnaryMinus(CExpression_UnaryMinus node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");

            CreateContextSubgraph(node, CExpression_UnaryMinus.EXPR,
                node.mc_contextNames[CExpression_UnaryMinus.EXPR]);

            return base.VisitExpression_UnaryMinus(node, node);
        }

        public override int VisitExpression_Addition(CExpression_Addition node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");

            CreateContextSubgraph(node, CExpression_Addition.LHS,
                node.mc_contextNames[CExpression_Addition.LHS]);

            CreateContextSubgraph(node, CExpression_Addition.RHS,
                node.mc_contextNames[CExpression_Addition.RHS]);

            return base.VisitExpression_Addition(node, node);
        }

        public override int VisitExpression_Subtraction(CExpression_Subtraction node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");

            CreateContextSubgraph(node, CExpression_Subtraction.LHS,
                node.mc_contextNames[CExpression_Subtraction.LHS]);

            CreateContextSubgraph(node, CExpression_Subtraction.RHS,
                node.mc_contextNames[CExpression_Subtraction.RHS]);

            return base.VisitExpression_Subtraction(node, node);
        }

        public override int VisitExpression_Multiplication(CExpression_Multiplication node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");

            CreateContextSubgraph(node, CExpression_Multiplication.LHS,
                node.mc_contextNames[CExpression_Multiplication.LHS]);

            CreateContextSubgraph(node, CExpression_Multiplication.RHS,
                node.mc_contextNames[CExpression_Multiplication.RHS]);

            return base.VisitExpression_Multiplication(node, node);
        }

        public override int VisitExpression_FDivision(CExpression_FDivision node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");

            CreateContextSubgraph(node, CExpression_FDivision.LHS,
                node.mc_contextNames[CExpression_FDivision.LHS]);

            CreateContextSubgraph(node, CExpression_FDivision.RHS,
                node.mc_contextNames[CExpression_FDivision.RHS]);

            return base.VisitExpression_FDivision(node, node);
        }

        public override int VisitExpression_IDivision(CExpression_IDivision node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");

            CreateContextSubgraph(node, CExpression_IDivision.LHS,
                node.mc_contextNames[CExpression_IDivision.LHS]);

            CreateContextSubgraph(node, CExpression_IDivision.RHS,
                node.mc_contextNames[CExpression_IDivision.RHS]);

            return base.VisitExpression_IDivision(node, node);
        }

        public override int VisitExpression_Modulo(CExpression_Modulo node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");

            CreateContextSubgraph(node, CExpression_Modulo.LHS,
                node.mc_contextNames[CExpression_Modulo.LHS]);

            CreateContextSubgraph(node, CExpression_Modulo.RHS,
                node.mc_contextNames[CExpression_Modulo.RHS]);

            return base.VisitExpression_Modulo(node, node);
        }

        public override int VisitT_IntegerDataType(CIntType node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");
            return 0;
        }

        public override int VisitT_FloatDataType(CFloatType node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");
            return 0;
        }

        public override int VisitT_RangeDataType(CRangeType node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");
            return 0;
        }

        public override int VisitT_NUMBER(CNUMBER node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");
            return 0;
        }

        public override int VisitT_IDENTIFIER(CIDENTIFIER node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");
            return 0;
        }
    }
}
