using MATHL.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;
using MATHL.TypeSystem;

namespace MATHL.STVisitors {

    public class ASTGenerationInfo {
        private ASTComposite m_ContextParent;

        public ASTComposite MContextParent {
            get => m_ContextParent;
            set => m_ContextParent = value ?? throw new ArgumentNullException(nameof(value));
        }
    }


    public class ASTGeneration : MATHLParserBaseVisitor<ASTElement> {
        
        // External Dependency with composition
        private ScopeSystem m_scopesystem;

        // State
        private CCompileUnit m_root;
        Stack<ASTGenerationInfo> m_infoStack = new Stack<ASTGenerationInfo>();
        private Stack<ASTComposite> m_parentsStack = new Stack<ASTComposite>();
        private Stack<int> m_contextsStack = new Stack<int>();
        private Scope m_currentScope;

        public ASTGeneration(ScopeSystem scopesystem) {
            m_scopesystem = scopesystem;
        }

        public override ASTElement VisitCompile_unit(MATHLParser.Compile_unitContext context) {

            m_currentScope =  m_scopesystem.EnterScope(m_scopesystem.M_GlobalScopeName);

            CCompileUnit newNode = new CCompileUnit();
            m_root = newNode;
            newNode[typeof(Scope)]= m_currentScope;


            ASTGenerationInfo info = new ASTGenerationInfo() { MContextParent = newNode };
            var res = this.VisitElementsInContext(context.command(),
                CCompileUnit.COMMANDS, m_contextsStack, newNode, m_parentsStack,
                info,m_infoStack);

            m_scopesystem.ExitScope();
            return m_root;
        }

        public override ASTElement VisitCommand_expression(MATHLParser.Command_expressionContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();

            CCommand_Expression newNode = new CCommand_Expression();
            parent.AddChild(parentContext, newNode);
            newNode[typeof(Scope)] = m_currentScope;

            ASTGenerationInfo info = new ASTGenerationInfo() { MContextParent = newNode };
            var res = this.VisitElementInContext(context.expression(),
                CCommand_Expression.COMMAND, m_contextsStack, newNode, m_parentsStack,
                info,m_infoStack);
            return newNode;
        }

        public override ASTElement VisitCommand_return(MATHLParser.Command_returnContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();

            CCommand_Return newNode = new CCommand_Return();
            parent.AddChild(parentContext, newNode);
            newNode[typeof(Scope)] = m_currentScope;

            ASTGenerationInfo info = new ASTGenerationInfo() { MContextParent = newNode };
            var res = this.VisitElementInContext(context.expression(),
                CCommand_Return.EXPRESSION, m_contextsStack, newNode, m_parentsStack,info,m_infoStack);
            return newNode;
        }


        public override ASTElement VisitCommand_block(MATHLParser.Command_blockContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();
            
            // 1. Enter block scope
            if (!(context.Parent is MATHLParser.Function_declarationContext)) {
                Scope scope = m_scopesystem.GetScopeAssociatedWithSyntaxObject(context);
                m_scopesystem.EnterScope(scope.M_ScopeName);
            }

            // 2. Create command block code 
            CCommand_CommandBlock newNode = new CCommand_CommandBlock();
            parent.AddChild(parentContext, newNode);
            newNode[typeof(Scope)] = m_currentScope;

            // 3. Visit children
            ASTGenerationInfo info = new ASTGenerationInfo() { MContextParent = newNode };
            var res = this.VisitElementsInContext(context.command(),
                CCommand_Expression.COMMAND, m_contextsStack, newNode, m_parentsStack,info,m_infoStack);

            // 4. Leave block scope
            if (!(context.Parent is MATHLParser.Function_declarationContext)) {
                m_scopesystem.ExitScope();
            }
            return newNode;
        }

        public override ASTElement VisitVariable_declaration(MATHLParser.Variable_declarationContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();

            CDeclarationVariable newNode = new CDeclarationVariable();
            parent.AddChild(parentContext, newNode);
            newNode[typeof(Scope)] = m_currentScope;

            ASTGenerationInfo info = new ASTGenerationInfo() { MContextParent = newNode };
            var res = this.VisitElementsInContext(context.variable_declarator(), CDeclarationVariable.DECLARATIONS,
                m_contextsStack, newNode, m_parentsStack,info,m_infoStack);

            res = this.VisitElementInContext(context.type(), CDeclarationVariable.TYPE, m_contextsStack,
                newNode, m_parentsStack,info,m_infoStack);

            return newNode;
        }

        public override ASTElement VisitVariable_declarator(MATHLParser.Variable_declaratorContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();

            // Create a variable declarator and link to the parent
            CDeclaratorVariable newNode = new CDeclaratorVariable();
            parent.AddChild(parentContext, newNode);
            newNode[typeof(Scope)] = m_currentScope;

            ASTGenerationInfo info = new ASTGenerationInfo() { MContextParent = newNode };
            var res = this.VisitTerminalInContext(context, context.IDENTIFIER().Symbol,
                CDeclaratorVariable.VARIABLENAME, m_contextsStack, newNode, m_parentsStack,info,m_infoStack);

            CIDENTIFIER variableIdentifier = res as CIDENTIFIER;
            VariableSymbol variableSymbol= 
                m_scopesystem.SearchSymbol(variableIdentifier.M_StringLiteral, SymbolCategory.ST_VARIABLE)
                as VariableSymbol;
            variableSymbol.M_VariableIdentifier =variableIdentifier;
            variableIdentifier[typeof(LSymbol)] = variableSymbol;

            res = this.VisitElementsInContext(context._pds, CDeclaratorVariable.TYPE,
                m_contextsStack, newNode, m_parentsStack,info,m_infoStack);

            if (context.expression() != null) {
                res = this.VisitElementInContext(context.expression(), CDeclaratorVariable.INITIALIZATION,
                    m_contextsStack, newNode, m_parentsStack,info,m_infoStack);
            }
            return newNode;
        }

        public override ASTElement VisitFunction_declaration(MATHLParser.Function_declarationContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();
            // Get function name
            string functionName = context.IDENTIFIER().Symbol.Text;

            // Get FunctionSymbol from the symbol table. ( Declaration Processor has precedes this pass)
            FunctionSymbol functionSymbol = 
                (FunctionSymbol)m_scopesystem.SearchSymbol(functionName, SymbolCategory.ST_FUNCTION);
            
            // Enter function scope
            m_currentScope= m_scopesystem.EnterScope(functionName);

            // Create function definition node
            CDeclarationFunction newNode = new CDeclarationFunction(functionName);
            parent.AddChild(parentContext, newNode);
            newNode[typeof(Scope)] = m_currentScope;

            // Update info for FunctionSymbol in the symbol table and CDeclarationFunction 
            functionSymbol.M_FunctionDeclaration = newNode;
            newNode[typeof(LSymbol)] = functionSymbol;

            // Visit Children
            ASTGenerationInfo info = new ASTGenerationInfo() { MContextParent = newNode };
            var res = this.VisitElementInContext(context.type(), CDeclarationFunction.TYPE,
                m_contextsStack, newNode, m_parentsStack,info,m_infoStack);
            res = this.VisitTerminalInContext(context, context.IDENTIFIER().Symbol, CDeclarationFunction.FUNCTION_NAME,
                m_contextsStack, newNode, m_parentsStack,info,m_infoStack);

            // Update the information for the CIDENTIFIER AST node representing the function name
            CIDENTIFIER functionId = res as CIDENTIFIER;
            functionId[functionName]= functionSymbol;

            res = this.VisitElementsInContext(context.variable_declaration(), CDeclarationFunction.PARAMETERS,
                m_contextsStack, newNode, m_parentsStack,info,m_infoStack);
            res = this.VisitElementInContext(context.command_block(), CDeclarationFunction.BODY,
                m_contextsStack, newNode, m_parentsStack,info,m_infoStack);

            // Exit function scope
            m_scopesystem.ExitScope();
            return newNode;
        }

        public override ASTElement VisitExpression_equationassignment(MATHLParser.Expression_equationassignmentContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();

            CExpression_Equation newNode = new CExpression_Equation();
            parent.AddChild(parentContext, newNode);
            newNode[typeof(Scope)] = m_currentScope;

            ASTGenerationInfo info = new ASTGenerationInfo() { MContextParent = newNode };
            var res = this.VisitElementInContext(context.a,
                CExpression_Equation.LHS, m_contextsStack, newNode, m_parentsStack,info,m_infoStack);
            res = this.VisitElementInContext(context.b,
                CExpression_Equation.RHS, m_contextsStack, newNode, m_parentsStack,info,m_infoStack);
            return newNode;

        }

        public override ASTElement VisitRange(MATHLParser.RangeContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();
            ASTComposite newNode = null;
            newNode = new CExpression_Range();
            parent.AddChild(parentContext, newNode);
            newNode[typeof(Scope)] = m_currentScope;

            ASTGenerationInfo info = new ASTGenerationInfo() { MContextParent = newNode };
            var res = this.VisitElementInContext(context.a, CExpression_Range.START,
                m_contextsStack, newNode, m_parentsStack,info,m_infoStack);
            res = this.VisitElementInContext(context.b, CExpression_Range.END,
                m_contextsStack, newNode, m_parentsStack,info,m_infoStack);
            res = this.VisitElementInContext(context.c, CExpression_Range.STEP,
                m_contextsStack, newNode, m_parentsStack,info,m_infoStack);

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
            newNode[typeof(Scope)] = m_currentScope;

            ASTGenerationInfo info = new ASTGenerationInfo() { MContextParent = newNode };
            var res = this.VisitElementInContext(context.expression(), context_,
                m_contextsStack, newNode, m_parentsStack,info,m_infoStack);

            return newNode;
        }

        public override ASTElement VisitExpression_additionsubtraction(MATHLParser.Expression_additionsubtractionContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();
            int context_L = 0, context_R = 1;

            ASTComposite newNode = null;
            switch (context.op.Type) {
                case MATHLLexer.PLUS:
                    newNode = new CExpression_Addition();
                    break;
                case MATHLLexer.MINUS:
                    newNode = new CExpression_Subtraction();
                    break;
            }
            parent.AddChild(parentContext, newNode);
            newNode[typeof(Scope)] = m_currentScope;

            ASTGenerationInfo info = new ASTGenerationInfo() { MContextParent = newNode };
            var res = this.VisitElementInContext(context.a, context_L,
                m_contextsStack, newNode, m_parentsStack,info,m_infoStack);

            res = this.VisitElementInContext(context.b, context_R,
                m_contextsStack, newNode, m_parentsStack,info,m_infoStack);

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
            newNode[typeof(Scope)] = m_currentScope;

            ASTGenerationInfo info = new ASTGenerationInfo() { MContextParent = newNode };
            var res = this.VisitElementInContext(context.a, context_L,
                m_contextsStack, newNode, m_parentsStack,info,m_infoStack);

            res = this.VisitElementInContext(context.b, context_R,
                m_contextsStack, newNode, m_parentsStack,info,m_infoStack);
            return newNode;
        }
        
        public override ASTElement VisitNumberINTEGER(MATHLParser.NumberINTEGERContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();

            CINTEGERNUMBER newNode = new CINTEGERNUMBER(context.GetText());
            parent.AddChild(parentContext, newNode);
            newNode[typeof(Scope)] = m_currentScope;
            if (parent is CExpression p) {
                p.M_IsConstantExpression = true;
                p.M_ExpressionType = newNode.M_Type;
            }
            return newNode;
        }

        public override ASTElement VisitNumberFLOAT(MATHLParser.NumberFLOATContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();

            CFLOATNUMBER newNode = new CFLOATNUMBER(context.GetText());
            parent.AddChild(parentContext, newNode);
            newNode[typeof(Scope)] = m_currentScope;
            if (parent is CExpression p) {
                p.M_IsConstantExpression = true;
                p.M_ExpressionType = newNode.M_Type;
            }
            return newNode;
        }

        public override ASTElement VisitExpression_NUMBER(MATHLParser.Expression_NUMBERContext context) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();

            CExpression newNode = new CExpression_Number();
            parent.AddChild(parentContext, newNode);
            newNode[typeof(Scope)] = m_currentScope;

            ASTGenerationInfo info = new ASTGenerationInfo() { MContextParent = newNode };
            this.VisitElementInContext(context.number(), CExpression_Number.NUMBER,
                m_contextsStack, newNode, m_parentsStack,info,m_infoStack);
            return newNode;
        }

        public override ASTElement VisitTerminal(ITerminalNode node) {
            ASTComposite parent = m_parentsStack.Peek();
            int parentContext = m_contextsStack.Peek();
            ASTGenerationInfo info = m_infoStack.Peek();
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
                case MATHLLexer.IDENTIFIER:
                    switch (info.MContextParent.MType) {
                        case (int)NodeType.NT_EXPRESSION_FUNCTIONCALL:
                            FunctionSymbol functionSymbol;
                            functionSymbol = m_scopesystem.SearchSymbol(node.GetText(), SymbolCategory.ST_FUNCTION) as FunctionSymbol;
                            newNode = functionSymbol.M_FunctionIdentifier;
                            newNode[typeof(LSymbol)] = functionSymbol;
                            break;
                        case (int)NodeType.NT_EXPRESSION_EQUATION:
                        case (int)NodeType.NT_EXPRESSION_ADDITION:
                        case (int)NodeType.NT_EXPRESSION_SUBTRACTION:
                        case (int)NodeType.NT_EXPRESSION_MULTIPLICATION:
                        case (int)NodeType.NT_EXPRESSION_FDIVISION:
                        case (int)NodeType.NT_EXPRESSION_IDIVISION:
                        case (int)NodeType.NT_EXPRESSION_MODULO:
                        case (int)NodeType.NT_EXPRESSION_UNARYPLUS:
                        case (int)NodeType.NT_EXPRESSION_UNARYMINUS:
                        case (int)NodeType.NT_EXPRESSION_RANGE:
                            VariableSymbol variableSymbol;
                            variableSymbol = m_scopesystem.SearchSymbol(node.GetText(), SymbolCategory.ST_VARIABLE) as VariableSymbol;
                            if (variableSymbol == null) {
                                throw new Exception("Undeclared Identifier");
                            }
                            else {
                                newNode = variableSymbol.M_VariableIdentifier;
                                newNode[typeof(LSymbol)] = variableSymbol;
                            }

                            break;
                        default:
                            newNode = new CIDENTIFIER(node.GetText());
                            break;
                    }
                    parent.AddChild(parentContext, newNode);
                    newNode[typeof(Scope)] = m_currentScope;
                    break;
            }
            return newNode;
        }
    }
}
