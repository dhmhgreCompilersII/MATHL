using MATHL.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override ASTElement VisitVariable_declaration(MATHLParser.Variable_declarationContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();

            CDeclarationVariable newNode = new CDeclarationVariable();
            parent.AddChild(parentContext, newNode);

           var res = this.VisitElementsInContext(context._ids, CDeclarationVariable.DECLARATIONS, 
                m_contextsStack, newNode, m_parentsStack);

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
    }
}
