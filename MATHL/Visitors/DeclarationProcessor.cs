using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using MATHL.TypeSystem;

namespace MATHL.Visitors {

    internal class DeclProcInfo {
        private LType decLType;

        public LType DecLType {
            get => decLType;
            set => decLType = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    internal class DeclarationProcessor : MATHLParserBaseVisitor<LType> {
        private Stack<DeclProcInfo> mDeclProcInfos = new Stack<DeclProcInfo>();
        private ScopeSystem m_scopeSystem;
        private Scope M_CurrentScope => m_scopeSystem.M_CurrentScope;

        public DeclarationProcessor() {
            m_scopeSystem = MATHLExecutionEnvironment.GetInstance().M_ScopeSystem;
        }

        
        public override LType VisitCompile_unit(MATHLParser.Compile_unitContext context) {
            // Create global scope and visit descentant nodes
            m_scopeSystem.EnterScope();
            base.VisitCompile_unit(context);
            m_scopeSystem.ExitScope();
            m_scopeSystem.Report("SymbolTable.txt");
            return null;
        }

        public override LType VisitVariable_declaration(MATHLParser.Variable_declarationContext context) {

            // Visit type to acquire type specifier
            LType decLType = Visit(context.type());

            // Visit declarator to complete the identifier type for each declarator
            DeclProcInfo info = new DeclProcInfo() { DecLType = decLType };
            this.VisitElementsInContext(context.variable_declarator(),mDeclProcInfos,info);
            
            return null;
        }

        public override LType VisitVariable_declarator(MATHLParser.Variable_declaratorContext context) {
            DeclProcInfo parentInfo = mDeclProcInfos.Peek();
            LType resutLType = parentInfo.DecLType;

            // Get variable identifier
            IToken identifier = (context.IDENTIFIER() as ITerminalNode).Symbol;
            string variableName = identifier.Text;

            // Store variable into the symbol table
            VariableSymbol newVariableSymbol = new VariableSymbol(variableName, resutLType);
            M_CurrentScope.DefineSymbol(newVariableSymbol, SymbolCategory.ST_VARIABLE);
            
            // Visit postfix declarators to complete the type
            // TODO

            return resutLType;
        }

        public override LType VisitType(MATHLParser.TypeContext context) {
            IToken typeSpecifier = (context.GetChild(0) as ITerminalNode).Symbol;
            LType declaredtype = typeSpecifier.Type switch {
                MATHLLexer.INT => new IntegerType(),
                MATHLLexer.FLOAT => new FloatingType(),
                MATHLLexer.RANGE => new RangeType()
            };
            return declaredtype;
        }

    }
}
