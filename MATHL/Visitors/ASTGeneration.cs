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
                CCompileUnit.COMMANDS,
                m_contextsStack, newNode, m_parentsStack);
            
            return m_root;
        }

    }
}
