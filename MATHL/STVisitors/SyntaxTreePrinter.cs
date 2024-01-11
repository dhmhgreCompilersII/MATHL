using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;


public class SyntaxTreePrinter : MATHLParserBaseVisitor<int> {
    private Stack<string> m_parentsLabel = new Stack<string>();
    int m_NodeCounter = 0;
    StreamWriter m_writer;
    private string m_outputFile;

    public SyntaxTreePrinter(string filename) {
        m_writer = new StreamWriter(filename + ".dot");
        m_outputFile = filename;
    }

    private void Visit_Prologue(Func<string> nodelabel_, bool root = false) {
        // Create node label
        string nodelabel = nodelabel_() + "_" + m_NodeCounter++;

        // Print content
        if (!root) {
            string parent = m_parentsLabel.Peek();
            m_writer.WriteLine($"\t\"{parent}\"->\"{nodelabel}\";");
        } else {
            m_writer.WriteLine("digraph G {");
        }

        // Save node label to stack
        m_parentsLabel.Push(nodelabel);
    }

    private void Visit_Epilogue(bool root = false) {
        // Pop parent as returning from children
        m_parentsLabel.Pop();

        // Print content
        if (root) {
            m_writer.WriteLine("}");
            m_writer.Close();
        }
    }

    public override int VisitCompile_unit(MATHLParser.Compile_unitContext context) {

        Visit_Prologue(() => "CompileUnit", true);

        base.VisitCompile_unit(context);

        Visit_Epilogue(true);

        // Prepare the process dot to run
        ProcessStartInfo start = new ProcessStartInfo();
        // Enter in the command line arguments, everything you would enter after the executable name itself
        start.Arguments = "-Tgif " +
                          m_outputFile + ".dot" + " -o " +
                          m_outputFile + ".gif";
        // Enter the executable to run, including the complete path
        start.FileName = "dot";
        // Do you want to show a console window?
        start.WindowStyle = ProcessWindowStyle.Hidden;
        start.CreateNoWindow = true;
        int exitCode;

        // Run the external process & wait for it to finish
        using (Process proc = Process.Start(start)) {
            proc.WaitForExit();

            // Retrieve the app's exit code
            exitCode = proc.ExitCode;
        }

        return m_NodeCounter;

    }

    public override int VisitCommand_expression(MATHLParser.Command_expressionContext context) {
        Visit_Prologue(() => "Command_Expression");

        base.VisitCommand_expression(context);

        Visit_Epilogue();

        return m_NodeCounter;
    }

    public override int VisitCommand_declaration(MATHLParser.Command_declarationContext context) {
        Visit_Prologue(() => "Command_Declaration");

        base.VisitCommand_declaration(context);

        Visit_Epilogue();

        return m_NodeCounter;
    }

    public override int VisitCommand_commandblock(MATHLParser.Command_commandblockContext context) {
        Visit_Prologue(() => "Command_CommandBlock");

        base.VisitCommand_commandblock(context);

        Visit_Epilogue();

        return m_NodeCounter;
    }
    public override int VisitCommand_return(MATHLParser.Command_returnContext context) {
        Visit_Prologue(() => "Command_Return");

        base.VisitCommand_return(context);

        Visit_Epilogue();

        return m_NodeCounter;
    }
    public override int VisitCommand_termination(MATHLParser.Command_terminationContext context) {
        Visit_Prologue(() => "Command_termination");

        base.VisitCommand_termination(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }

    public override int VisitCommand_block(MATHLParser.Command_blockContext context) {
        Visit_Prologue(() => "CommandBlock");

        base.VisitCommand_block(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }

    public override int VisitDeclaration_variable(MATHLParser.Declaration_variableContext context) {
        Visit_Prologue(() => "Declaration");

        base.VisitDeclaration_variable(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }

    public override int VisitDeclaration_function(MATHLParser.Declaration_functionContext context) {
        Visit_Prologue(() => "Declaration");

        base.VisitDeclaration_function(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }
    
    public override int VisitType(MATHLParser.TypeContext context) {
        Visit_Prologue(() => "Type");

        base.VisitType(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }

    public override int VisitVariable_declarator(MATHLParser.Variable_declaratorContext context) {
        Visit_Prologue(() => "Variable_declarator");

        base.VisitVariable_declarator(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }

    public override int VisitPostfix_declarators(MATHLParser.Postfix_declaratorsContext context) {
        Visit_Prologue(() => "Postfix_declarators");

        base.VisitPostfix_declarators(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }

    public override int VisitVariable_declaration(MATHLParser.Variable_declarationContext context) {
        Visit_Prologue(() => "Variable_declaration");

        base.VisitVariable_declaration(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }

    public override int VisitFunction_declaration(MATHLParser.Function_declarationContext context) {
        Visit_Prologue(() => "Function_declaration");

        base.VisitFunction_declaration(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }

    public override int VisitExpression_IDENTIFIER(MATHLParser.Expression_IDENTIFIERContext context) {
        Visit_Prologue(() => "Expression_IDENTIFIER");

        base.VisitExpression_IDENTIFIER(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }

    public override int VisitExpression_parenthesizedexpression(MATHLParser.Expression_parenthesizedexpressionContext context) {
        Visit_Prologue(() => "ParenthesizedExpression");

        base.VisitExpression_parenthesizedexpression(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }

    public override int VisitExpression_context(MATHLParser.Expression_contextContext context) {
        Visit_Prologue(() => "ImpliedMultiplication");

        base.VisitExpression_context(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }

    public override int VisitExpression_multiplicationdivision(MATHLParser.Expression_multiplicationdivisionContext context) {
        Visit_Prologue(() => {
            switch (context.op.Type) {
                case MATHLLexer.MULT:
                    return "multiplication";
                case MATHLLexer.IDIV:
                    return "idivision";
                case MATHLLexer.FDIV:
                    return "fdivision";
                case MATHLLexer.MOD:
                    return "modulo";
                default:
                    throw new Exception("invalid operator");
            }
        });

        base.VisitExpression_multiplicationdivision(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }
    


    public override int VisitExpression_equationassignment(MATHLParser.Expression_equationassignmentContext context) {
        Visit_Prologue(() => {
            if (context.a.GetChild(0) is MATHLParser.Expression_IDENTIFIERContext) {
                return "assignment";
            } else {
                return "equation";
            }
        });

        base.VisitExpression_equationassignment(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }

    public override int VisitExpression_NUMBER(MATHLParser.Expression_NUMBERContext context) {
        Visit_Prologue(() => "Expression_NUMBER");

        base.VisitExpression_NUMBER(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }
    
    public override int VisitExpression_unaryprefixexpression(MATHLParser.Expression_unaryprefixexpressionContext context) {
        Visit_Prologue(() => {
            switch (context.op.Type) {
                case MATHLLexer.PLUS:
                    return "unaryplus";
                case MATHLLexer.MINUS:
                    return "unaryminus";
                default:
                    throw new Exception("invalid operator");
            }
        });

        base.VisitExpression_unaryprefixexpression(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }

    public override int VisitExpression_additionsubtraction(MATHLParser.Expression_additionsubtractionContext context) {
        Visit_Prologue(() => {
            switch (context.op.Type) {
                case MATHLLexer.PLUS:
                    return "addition";
                case MATHLLexer.MINUS:
                    return "subtraction";
                default:
                    throw new Exception("invalid operator");
            }
        });

        base.VisitExpression_additionsubtraction(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }

    public override int VisitExpression_range(MATHLParser.Expression_rangeContext context) {
        Visit_Prologue(() => "Expression_range");

        base.VisitExpression_range(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }

    public override int VisitParams(MATHLParser.ParamsContext context) {
        Visit_Prologue(() => "params");

        base.VisitParams(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }

    public override int VisitRange(MATHLParser.RangeContext context) {
        Visit_Prologue(() => "range");

        base.VisitRange(context);

        Visit_Epilogue();
        return m_NodeCounter;
    }

    public override int VisitTerminal(ITerminalNode node) {

        string nodelabel;

        switch (node.Symbol.Type) {
            case MATHLLexer.IDENTIFIER:
                nodelabel = "IDENTIFIER_" + node.Symbol.Text;
                Visit_Prologue(() => {
                    return nodelabel;
                });
                break;
            case MATHLLexer.INTEGER:
                nodelabel = "INTEGER_" + node.Symbol.Text;
                Visit_Prologue(() => {
                    return nodelabel;
                });
                break;
            case MATHLLexer.FLOATING:
                nodelabel = "FLOATING_" + node.Symbol.Text;
                Visit_Prologue(() => {
                    return nodelabel;
                });
                break;
            default:
                break;
        }

        base.VisitTerminal(node);

        switch (node.Symbol.Type) {
            case MATHLLexer.IDENTIFIER:
            case MATHLLexer.INTEGER:
            case MATHLLexer.FLOATING:
                Visit_Epilogue();
                break;
            default:
                break;
        }

        return m_NodeCounter;
    }
}

