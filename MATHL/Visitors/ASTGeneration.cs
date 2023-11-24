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

        public override ASTElement VisitCommand_expression(MATHLParser.Command_expressionContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();


            CCommand_Expression newNode = new CCommand_Expression();
            parent.AddChild(parentContext,newNode);


            var res = this.VisitElementInContext(context.expression(),
                CCommand_Expression.COMMAND,m_contextsStack, newNode, m_parentsStack);
            return newNode;
        }

        public override ASTElement VisitCommand_declaration(MATHLParser.Command_declarationContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();

            CCommand_Declaration newNode = new CCommand_Declaration();
            parent.AddChild(parentContext, newNode);

            var res = this.VisitElementInContext(context.declaration(),
                CCommand_Expression.COMMAND, m_contextsStack, newNode, m_parentsStack);
            return newNode;
        }

        public override ASTElement VisitCommand_commandblock(MATHLParser.Command_commandblockContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();

            CCommand_CommandBlock newNode = new CCommand_CommandBlock();
            parent.AddChild(parentContext, newNode);

            var res = this.VisitElementInContext(context.command_block(),
                CCommand_Expression.COMMAND, m_contextsStack, newNode, m_parentsStack);
            return newNode;
        }
    }
}
