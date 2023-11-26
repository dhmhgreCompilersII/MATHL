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
            
            return base.VisitDeclaration_Function(node, node);
        }

        public override int VisitCommand_CommandBlock(CCommand_CommandBlock node, params ASTElement[] args) {
            m_writer.WriteLine($"\"{args[0].MNodeName}\"->\"{node.MNodeName}\";");

            CreateContextSubgraph(node, CCommand_CommandBlock.COMMAND,
                node.mc_contextNames[CCommand_CommandBlock.COMMAND]);

            return base.VisitCommand_CommandBlock(node, node);
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
