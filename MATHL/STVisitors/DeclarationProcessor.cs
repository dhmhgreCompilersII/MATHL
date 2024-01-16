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

    internal class DeclarationInfo {
        private LType m_declarationType = null;
        List<VariableSymbol> m_parameters = new List<VariableSymbol>();
        private List<LType> m_parameterTypes = new List<LType>();
        private List<int> m_dimensions = new List<int>();

        public LType DecLType {
            get => m_declarationType;
            set => m_declarationType = value ?? throw new ArgumentNullException(nameof(value));
        }

        public List<VariableSymbol> MParameters => m_parameters;
        public List<LType> MParameterTypes => m_parameterTypes;
        public List<int> MDimensions => m_dimensions;
    }

    internal class DeclarationProcessor : MATHLParserBaseVisitor<LType> {
        private Stack<DeclarationInfo> m_DeclProcInfos = new Stack<DeclarationInfo>();
        private ScopeSystem m_scopeSystem;
        private Scope M_CurrentScope => m_scopeSystem.M_CurrentScope;

        public DeclarationProcessor(ScopeSystem scopesystem) {
            m_scopeSystem = scopesystem;
        }

        public override LType VisitCompile_unit(MATHLParser.Compile_unitContext context) {
            // Create GLOBAL SCOPE and visit descentant nodes
            Scope scope = m_scopeSystem.CreateScope(m_scopeSystem.M_GlobalScopeName);
            // Associate the current ST node (Compile_unitContext) with the global scope
            m_scopeSystem.AssociateSyntaxObjectWithScope(context, scope);
            // Visit Children
            base.VisitCompile_unit(context);
            // Leave global scope
            m_scopeSystem.ExitScope();
            return null;
        }

        public override LType VisitCommand_block(MATHLParser.Command_blockContext context) {
            Scope currentScope = M_CurrentScope;
            // 1. Enter BLOCK SCOPE if it doesn't refer to FunctionDeclaration
            // Function Declaration has already its own scope
            if (!(context.Parent is MATHLParser.Function_declarationContext)) {
                currentScope = m_scopeSystem.CreateScope(null);
                m_scopeSystem.AssociateSyntaxObjectWithScope(context, currentScope);
            }

            DeclarationInfo dummy = new DeclarationInfo();
            this.VisitElementsInContext(context.command(), m_DeclProcInfos, dummy);

            // 2. Exit block scope if it doesn't refer to FunctionDeclaration
            // Function Declaration has already its own scope
            if (!(context.Parent is MATHLParser.Function_declarationContext)) {
                m_scopeSystem.ExitScope();
            }
            return null;
        }
        // DECLARED VARIABLES
        public override LType VisitFunction_declaration(MATHLParser.Function_declarationContext context) {

            // Visit type to acquire TYPE SPECIFIER
            LType decLType = Visit(context.type());

            // Get FUNCTION NAME
            IToken identifier = (context.IDENTIFIER() as ITerminalNode).Symbol;
            string functionName = identifier.Text;

            // Enter the FUNCTION SCOPE before visiting the parameters and the command block
            Scope scope = m_scopeSystem.CreateScope(functionName);
            m_scopeSystem.AssociateSyntaxObjectWithScope(context, scope);

            // Visit FUNCTION PARAMETERS section and complete the function type
            DeclarationInfo info = new DeclarationInfo() { DecLType = decLType };
            this.VisitElementsInContext(context.variable_declaration(), m_DeclProcInfos, info);

            // Create Function type
            FunctionType functiontype = new FunctionType(decLType, info.MParameterTypes);

            // Create Function Symbol
            FunctionSymbol fs = new FunctionSymbol(functionName, functiontype, info.MParameters);

            // Visit COMMAND BLOCK
            this.VisitElementInContext(context.command_block(), m_DeclProcInfos, info);

            // Leave Function Scope
            m_scopeSystem.ExitScope();

            // Store function into the symbol table
            M_CurrentScope.DefineSymbol(fs, SymbolCategory.ST_FUNCTION);

            return functiontype;
        }
        // DECLARED VARIABLES
        public override LType VisitVariable_declaration(MATHLParser.Variable_declarationContext context) {
            // RULE : type variable_declarator ( ',' variable_declarator )* ;

            // parentInfo could come from a function declaration whose parameters'
            // declarations are inspected. Parent info must be filled with the 
            // symbols declared in the current declaration.
            DeclarationInfo parentInfo = null;
            if (context.Parent is MATHLParser.Function_declarationContext) {
                parentInfo = m_DeclProcInfos.Peek();
            }

            // Visit type to acquire TYPE SPECIFIER of the variable declaration
            LType decLType = Visit(context.type());

            // Visit DECLARATORS to complete the identifier type for each declarator
            // The variable symbol will be created into the declarator node thus,
            // the type should be sent. Info will bring the variable symbols declared
            DeclarationInfo info = new DeclarationInfo() { DecLType = decLType };
            this.VisitElementsInContext(context.variable_declarator(), m_DeclProcInfos, info);


            if (context.Parent is MATHLParser.Function_declarationContext) {
                // info has been updated with the declared variables
                // by the visit to the declarator
                foreach (VariableSymbol variableSymbol in info.MParameters) {
                    parentInfo.MParameters.Add(variableSymbol);
                    parentInfo.MParameterTypes.Add(decLType);
                }
            }
            return null;
        }
        // DECLARED VARIABLES
        public override LType VisitVariable_declarator(MATHLParser.Variable_declaratorContext context) {
            DeclarationInfo parentInfo = m_DeclProcInfos.Peek();

            // Get variable identifier and then its text
            IToken identifier = (context.IDENTIFIER() as ITerminalNode).Symbol;
            string variableName = identifier.Text;

            // Visit POSTFIX DECLARATORS to complete the type
            LType resutLType;
            if (context.postfix_declarators().Length > 0) {
                // uses from DeclarationInfo 
                // 1. MDimensions
                DeclarationInfo info = new DeclarationInfo() { DecLType = parentInfo.DecLType };
                this.VisitElementsInContext(context.postfix_declarators(), m_DeclProcInfos, info);
                resutLType = new ArrayType(parentInfo.DecLType, info.MDimensions.ToArray());
            } else {
                // If there are no postfix declarators return the type specifier
                resutLType = parentInfo.DecLType;
            }

            // Create the Variable Symbol with the appropriate type
            // Store variable into the symbol table
            VariableSymbol newVariableSymbol = new VariableSymbol(variableName, resutLType);
            m_scopeSystem.DefineSymbol(newVariableSymbol, SymbolCategory.ST_VARIABLE);
            parentInfo.MParameters.Add(newVariableSymbol);

            return resutLType;
        }
        // DECLARED VARIABLES
        public override LType VisitPostfix_declarators(MATHLParser.Postfix_declaratorsContext context) {
            DeclarationInfo parentInfo = m_DeclProcInfos.Peek();

            int dimensionSize = Int32.Parse(context.INTEGER().GetText());

            // Complete the declarator type from the postfix declarators
            parentInfo.MDimensions.Add(dimensionSize);
            return null;
        }
        // DECLARED VARIABLES
        public override LType VisitType(MATHLParser.TypeContext context) {
            IToken typeSpecifier = (context.GetChild(0) as ITerminalNode).Symbol;
            LType declaredtype = typeSpecifier.Type switch {
                MATHLLexer.INT => m_scopeSystem.M_GlobalScope.
                    SearchSymbol(typeSpecifier.Text, SymbolCategory.ST_TYPENAME).MType,
                MATHLLexer.FLOAT => m_scopeSystem.M_GlobalScope.
                    SearchSymbol(typeSpecifier.Text, SymbolCategory.ST_TYPENAME).MType,
                MATHLLexer.RANGE => new RangeType()
            };
            return declaredtype;
        }

        public override LType VisitExpression_equationassignment(MATHLParser.Expression_equationassignmentContext context) {
            LType declaredtype = null;

            // 1. Check if an IDENTIFIER exists in LHS
            if (context.a is MATHLParser.Expression_IDENTIFIERContext identifierExpression) {

                // 2. Visit LHS and acquire the IDENTIFIER 
                IToken idToken = identifierExpression.IDENTIFIER().Symbol;
                string identifier = idToken.Text;

                // 3. Visit RHS to acquire the type of the expression
                LType RHSType =Visit(context.b);

                // 4. Search symbol table for the IDENTIFIER
                LSymbol identifierSymbol= m_scopeSystem.SearchSymbol(identifier, SymbolCategory.ST_VARIABLE);

                // 5. Validate type of IDENTIFIER or create its type 
                if (identifierSymbol != null) { // Check if the symbol has already been defined
                    if (identifierSymbol.MType != RHSType) {
                        throw new Exception("Incompatible assignment type");
                    }
                }
                else { // If the symbol is not declared, declare it using the the 
                       // assignment's RHS type
                    identifierSymbol = new VariableSymbol(identifier, RHSType);
                    m_scopeSystem.DefineSymbol(identifierSymbol, SymbolCategory.ST_VARIABLE);
                }
                return identifierSymbol.MType;
            }
            return null;
        }

        public override LType VisitExpression_IDENTIFIER(MATHLParser.Expression_IDENTIFIERContext context) {

            IToken identifierToken = context.IDENTIFIER().Symbol;
            string identifier = identifierToken.Text;

            LSymbol idSymbol = m_scopeSystem.SearchSymbol(identifier, SymbolCategory.ST_VARIABLE);
            if (idSymbol == null) {
                throw new Exception($"Undeclared Identifier " +
                       $"row({context.Start.Line}-{context.Stop.Line}):column({context.Start.Column}-{context.Stop.Column}) " +
                       $": {identifier} is used without declared or defined");
            }
            return idSymbol.MType;
        }

        public override LType VisitExpression_FunctionCall(MATHLParser.Expression_FunctionCallContext context) {
            IToken identifierToken = context.IDENTIFIER().Symbol;
            string identifier = identifierToken.Text;

            LSymbol idSymbol = m_scopeSystem.SearchSymbol(identifier, SymbolCategory.ST_FUNCTION);
            if (idSymbol == null) {
                throw new Exception($"Undefined Function " +
                                    $"row({context.Start.Line}-{context.Stop.Line}):column({context.Start.Column}-{context.Stop.Column}) " +
                                    $": {identifier} is used without being defined");
            }
            return idSymbol.MType;
        }

        public override LType VisitExpression_NUMBER(MATHLParser.Expression_NUMBERContext context) {
            return base.VisitExpression_NUMBER(context);
        }

        public override LType VisitNumberINTEGER(MATHLParser.NumberINTEGERContext context) {
            return IntegerType.Instance;
        }

        public override LType VisitNumberFLOAT(MATHLParser.NumberFLOATContext context) {
            return FloatingType.Instance;
        }
    }
}
