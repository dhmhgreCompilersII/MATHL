using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using MATHL.TypeSystem;

namespace MATHL.Visitors {
    public class Evaluator : MATHLParserBaseVisitor<int> {
        private MATHLExecutionEnvironment m_executionEnvironment;
        public Evaluator() {
            m_executionEnvironment = MATHLExecutionEnvironment.GetInstance();
        }

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
            IToken identifier = context.IDENTIFIER().Symbol;
            LSymbol idSymbol =
                m_executionEnvironment.MSymbolTable.SearchSymbol(identifier.Text, SymbolCategory.ST_VARIABLE);
            return idSymbol.MValue;
        }
        public override int VisitExpression_NUMBER(MATHLParser.Expression_NUMBERContext context) {
            IToken number = context.NUMBER().Symbol;
            return Int32.Parse(number.Text);
        }

        public override int VisitExpression_parenthesizedexpression(MATHLParser.Expression_parenthesizedexpressionContext context) {
            return Visit(context.expression());
        }

        public override int VisitExpression_multiplicationdivision(MATHLParser.Expression_multiplicationdivisionContext context) {

            int lhv = Visit(context.a);
            int rhv = Visit(context.b);

            switch (context.op.Type) {
                case MATHLLexer.MULT:
                    return lhv + rhv;
                case MATHLLexer.IDIV:
                    return lhv / rhv;
                case MATHLLexer.FDIV:
                    return lhv / rhv;
                case MATHLLexer.MOD:
                    return lhv % rhv;
                default:
                    throw new NotImplementedException();
            }
        }

        public override int VisitExpression_equationassignment(MATHLParser.Expression_equationassignmentContext context) {

            var lhs = context.a;
            var rhs = context.b;
            int result=Visit(rhs) ;

            if (lhs is MATHLParser.Expression_IDENTIFIERContext expression_id) {
                IToken identifier = expression_id.IDENTIFIER().Symbol;
                LSymbol idLSymbol =
                    m_executionEnvironment.MSymbolTable.SearchSymbol(identifier.Text, SymbolCategory.ST_VARIABLE);
                result = Visit(rhs);
                idLSymbol.MValue = result;
            }
            else {

            }
            return result;
        }
        

        public override int VisitExpression_unaryprefixexpression(MATHLParser.Expression_unaryprefixexpressionContext context) {
            int uv = Visit(context.expression());

            switch (context.op.Type) {
                case MATHLLexer.PLUS:
                    return uv;
                case MATHLLexer.MINUS:
                    return -uv;
                default:
                    throw new NotImplementedException();
            }
        }

        public override int VisitExpression_additionsubtraction(MATHLParser.Expression_additionsubtractionContext context) {

            int lhv = Visit(context.a);
            int rhv = Visit(context.b);

            switch (context.op.Type) {
                case MATHLLexer.PLUS:
                    return lhv + rhv;
                case MATHLLexer.MINUS:
                    return lhv - rhv;
                default:
                    throw new NotImplementedException();
            }
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
            return base.VisitTerminal(node);
        }
    }
}
