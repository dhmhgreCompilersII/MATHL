using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;

namespace MATHL.Visitors {
    public class Evaluator : MATHLParserBaseVisitor<int> {

        public override int VisitCompile_unit(MATHLParser.Compile_unitContext context) {
            return base.VisitCompile_unit(context);
        }

        public override int VisitCommand_expression(MATHLParser.Command_expressionContext context) {
            return base.VisitCommand_expression(context);
        }

        public override int VisitCommand_declaration(MATHLParser.Command_declarationContext context) {
            return base.VisitCommand_declaration(context);
        }

        public override int VisitCommand_commandblock(MATHLParser.Command_commandblockContext context) {
            return base.VisitCommand_commandblock(context);
        }
        
        public override int VisitCommand_termination(MATHLParser.Command_terminationContext context) {
            return base.VisitCommand_termination(context);
        }

        public override int VisitCommand_block(MATHLParser.Command_blockContext context) {
            return base.VisitCommand_block(context);
        }

        public override int VisitDeclaration_variable(MATHLParser.Declaration_variableContext context) {
            return base.VisitDeclaration_variable(context);
        }

        public override int VisitDeclaration_function(MATHLParser.Declaration_functionContext context) {
            return base.VisitDeclaration_function(context);
        }
        
        public override int VisitType(MATHLParser.TypeContext context) {
            return base.VisitType(context);
        }

        public override int VisitVariable_declarator(MATHLParser.Variable_declaratorContext context) {
            return base.VisitVariable_declarator(context);
        }

        public override int VisitPostfix_declarators(MATHLParser.Postfix_declaratorsContext context) {
            return base.VisitPostfix_declarators(context);
        }

        public override int VisitVariable_declaration(MATHLParser.Variable_declarationContext context) {
            return base.VisitVariable_declaration(context);
        }

        public override int VisitFunction_declaration(MATHLParser.Function_declarationContext context) {
            return base.VisitFunction_declaration(context);
        }

        public override int VisitExpression_IDENTIFIER(MATHLParser.Expression_IDENTIFIERContext context) {
            return base.VisitExpression_IDENTIFIER(context);
        }

        public override int VisitExpression_parenthesizedexpression(MATHLParser.Expression_parenthesizedexpressionContext context) {
            return base.VisitExpression_parenthesizedexpression(context);
        }

        public override int VisitExpression_multiplicationdivision(MATHLParser.Expression_multiplicationdivisionContext context) {
            return base.VisitExpression_multiplicationdivision(context);
        }

        public override int VisitExpression_equationassignment(MATHLParser.Expression_equationassignmentContext context) {
            return base.VisitExpression_equationassignment(context);
        }

        public override int VisitExpression_NUMBER(MATHLParser.Expression_NUMBERContext context) {
            return base.VisitExpression_NUMBER(context);
        }

        public override int VisitExpression_functioncall(MATHLParser.Expression_functioncallContext context) {
            return base.VisitExpression_functioncall(context);
        }

        public override int VisitExpression_unaryprefixexpression(MATHLParser.Expression_unaryprefixexpressionContext context) {
            return base.VisitExpression_unaryprefixexpression(context);
        }

        public override int VisitExpression_additionsubtraction(MATHLParser.Expression_additionsubtractionContext context) {
            return base.VisitExpression_additionsubtraction(context);
        }

        public override int VisitExpression_range(MATHLParser.Expression_rangeContext context) {
            return base.VisitExpression_range(context);
        }

        public override int VisitParams(MATHLParser.ParamsContext context) {
            return base.VisitParams(context);
        }

        public override int VisitRange(MATHLParser.RangeContext context) {
            return base.VisitRange(context);
        }

        public override int VisitTerminal(ITerminalNode node) {

            switch (node.Symbol.Type) {
                case MATHLLexer.NUMBER:

                    break;
                case MATHLLexer.IDENTIFIER:

                    break;
                default:
                    break;
            }

            return base.VisitTerminal(node);
        }
    }
}
