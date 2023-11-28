using MATHL.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;
using MATHL.TypeSystem;

namespace MATHL.Visitors {
    public class ASTGeneration : MATHLParserBaseVisitor<ASTElement> {
        private CCompileUnit m_root;
        private Stack<ASTComposite> m_parentsStack = new Stack<ASTComposite>();
        private Stack<int> m_contextsStack = new Stack<int>();

        public override ASTElement VisitCompile_unit(MATHLParser.Compile_unitContext context) {

            CCompileUnit newNode = new CCompileUnit();
            m_root = newNode;
            
            var res =this.VisitElementsInContext(context.command(),
                CCompileUnit.COMMANDS,m_contextsStack, newNode, m_parentsStack);
            return m_root;
        }

        public override ASTElement VisitCommand_block(MATHLParser.Command_blockContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();

            CCommand_CommandBlock newNode = new CCommand_CommandBlock();
            parent.AddChild(parentContext, newNode);

            var res = this.VisitElementsInContext(context.command(),
                CCommand_Expression.COMMAND, m_contextsStack, newNode, m_parentsStack);
            return newNode;
        }

        public override ASTElement VisitCommand_expression(MATHLParser.Command_expressionContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();


            CCommand_Expression newNode = new CCommand_Expression();
            parent.AddChild(parentContext,newNode);


            var res = this.VisitElementInContext(context.expression(),
                CCommand_Expression.COMMAND,m_contextsStack, newNode, m_parentsStack);
            return newNode;
        }

        public override ASTElement VisitExpression_equationassignment(MATHLParser.Expression_equationassignmentContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();


            CExpression_Equation newNode = new CExpression_Equation();
            parent.AddChild(parentContext, newNode);


            var res = this.VisitElementInContext(context.a,
                CExpression_Equation.LHS, m_contextsStack, newNode, m_parentsStack);
            res = this.VisitElementInContext(context.b,
                CExpression_Equation.RHS, m_contextsStack, newNode, m_parentsStack);
            return newNode;

        }


        public override ASTElement VisitVariable_declaration(MATHLParser.Variable_declarationContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();

            CDeclarationVariable newNode = new CDeclarationVariable();
            parent.AddChild(parentContext, newNode);

           var res = this.VisitElementsInContext(context._ids, CDeclarationVariable.DECLARATIONS, 
                m_contextsStack, newNode, m_parentsStack);

           res = this.VisitElementInContext(context.type(), CDeclarationVariable.TYPE, m_contextsStack,
               newNode, m_parentsStack);

            return newNode;
        }

        public override ASTElement VisitVariable_declarator(MATHLParser.Variable_declaratorContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();

            CDeclaratorVariable newNode = new CDeclaratorVariable();
            parent.AddChild(parentContext, newNode);

            var res = this.VisitTerminalInContext(context,context._IDENTIFIER,
                CDeclaratorVariable.VARIABLENAME, m_contextsStack,newNode,m_parentsStack);

            res = this.VisitElementsInContext(context._pds, CDeclaratorVariable.TYPE,
                m_contextsStack, newNode, m_parentsStack);

            if (context.expression() != null) {
                res = this.VisitElementInContext(context.expression(), CDeclaratorVariable.INITIALIZATION,
                    m_contextsStack, newNode, m_parentsStack);
            }
            return newNode;
        }

        public override ASTElement VisitRange(MATHLParser.RangeContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();
            ASTComposite newNode = null;
            newNode = new CExpression_Range();
            parent.AddChild(parentContext, newNode);

            var res = this.VisitElementInContext(context.a, CExpression_Range.START,
                m_contextsStack, newNode, m_parentsStack);
            res = this.VisitElementInContext(context.b, CExpression_Range.END,
                m_contextsStack, newNode, m_parentsStack);
            res = this.VisitElementInContext(context.c, CExpression_Range.STEP,
                m_contextsStack, newNode, m_parentsStack);

            return newNode;
        }
        public override ASTElement VisitExpression_unaryprefixexpression(MATHLParser.Expression_unaryprefixexpressionContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();
            int context_ = 0;
            ASTComposite newNode = null;
            switch (context.op.Type) {
                case MATHLLexer.PLUS:
                    newNode = new CExpression_UnaryPlus();
                    break;
                case MATHLLexer.MINUS:
                    newNode = new CExpression_UnaryMinus();
                    break;
            }
            parent.AddChild(parentContext, newNode);

            var res = this.VisitElementInContext(context.expression(), context_,
                m_contextsStack,newNode,m_parentsStack);

            return newNode;
        }

        public override ASTElement VisitExpression_additionsubtraction(MATHLParser.Expression_additionsubtractionContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();
            int context_L=0, context_R=1;

            ASTComposite newNode=null;
            switch (context.op.Type) {
                case MATHLLexer.PLUS:
                    newNode = new CExpression_Addition();
                    break;
                case MATHLLexer.MINUS:
                    newNode = new CExpression_Subtraction();
                    break;
            }
            parent.AddChild(parentContext, newNode);

            var res = this.VisitElementInContext(context.a, context_L,
                m_contextsStack,newNode,m_parentsStack);

            res = this.VisitElementInContext(context.b, context_R,
                m_contextsStack, newNode, m_parentsStack);
            
            return newNode;
        }

        public override ASTElement VisitExpression_multiplicationdivision(MATHLParser.Expression_multiplicationdivisionContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();
            int context_L = 0, context_R = 1;

            ASTComposite newNode = null;
            switch (context.op.Type) {
                case MATHLLexer.MULT:
                    newNode = new CExpression_Multiplication();
                    break;
                case MATHLLexer.IDIV:
                    newNode = new CExpression_IDivision();
                    break;
                case MATHLLexer.MOD:
                    newNode = new CExpression_Addition();
                    break;
                case MATHLLexer.FDIV:
                    newNode = new CExpression_FDivision();
                    break;
            }
            parent.AddChild(parentContext, newNode);

            var res = this.VisitElementInContext(context.a, context_L,
                m_contextsStack, newNode, m_parentsStack);

            res = this.VisitElementInContext(context.b, context_R,
                m_contextsStack, newNode, m_parentsStack);
            return newNode;
        }
        public override ASTElement VisitExpression_multiplicationNoOperator(MATHLParser.Expression_multiplicationNoOperatorContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();
            int context_L = 0, context_R = 1;

            ASTComposite newNode = new CExpression_Multiplication();
            parent.AddChild(parentContext, newNode);

            var res = this.VisitElementInContext(context.a, context_L,
                m_contextsStack, newNode, m_parentsStack);

            res = this.VisitElementInContext(context.b, context_R,
                m_contextsStack, newNode, m_parentsStack);
            return newNode;
        }

        public override ASTElement VisitTerminal(ITerminalNode node) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();

            ASTElement newNode = null;

            switch (node.Symbol.Type) {
                case MATHLLexer.INT:
                    newNode = new CIntType(node.GetText());
                    parent.AddChild(parentContext, newNode);
                    break;
                case MATHLLexer.FLOAT:
                    newNode = new CFloatType(node.GetText());
                    parent.AddChild(parentContext, newNode);
                    break;
                case MATHLLexer.RANGE:
                    newNode = new CRangeType(node.GetText());
                    parent.AddChild(parentContext, newNode);
                    break;
                case MATHLLexer.NUMBER:
                    newNode = new CNUMBER(node.GetText());
                    parent.AddChild(parentContext, newNode);
                    break;
                case MATHLLexer.IDENTIFIER:
                    newNode = new CIDENTIFIER(node.GetText());
                    parent.AddChild(parentContext, newNode);
                    break;
            }
            return newNode;
        }
    }
}
