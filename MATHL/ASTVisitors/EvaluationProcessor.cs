using MATHL.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MATHL;
using MATHL.TypeSystem;

public struct EvalParams {

}

public class EvaluatorLog : LogRecord {
    private string m_method;
    private string m_resultName;
    private string m_resultScope;
    LValue m_result;

    public string MMethod {
        get => m_method;
        set => m_method = value ?? throw new ArgumentNullException(nameof(value));
    }
    public string MResultName {
        get => m_resultName;
        set => m_resultName = value ?? throw new ArgumentNullException(nameof(value));
    }
    public LValue MResult {
        get => m_result;
        set => m_result = value;
    }
    public string MResultScope {
        get => m_resultScope;
        set => m_resultScope = value ?? throw new ArgumentNullException(nameof(value));
    }
    public override string LogResult(int verbocity) {
        string message;
        switch (verbocity) {
            case 0:
                message = $"{m_resultName}={m_result}";
                break;
            case 1:
                message = $"{m_resultScope}.{m_resultName}={m_result}";
                break;
            case 2:
                message = $"{m_resultScope}.{m_method}.{m_resultName}={m_result}";
                break;
            default:
                message = "";
                break;
        }
        return message;
    }
}

namespace MATHL.ASTVisitors {

    public class EvaluationProcessor : MATHLBaseVisitor<LValue, EvalParams> {
        private ScopeSystem m_scopeSystem;
        private Logging m_logger;


        private void Log(EvaluatorLog logRecord) {
            m_logger.LogStdOut(this,logRecord);
        }

        public EvaluationProcessor(ScopeSystem mScopeSystem, Logging logger) {
            m_scopeSystem = mScopeSystem;
            m_logger = logger;
        }

        public override LValue VisitCompileUnit(CCompileUnit node, params EvalParams[] args) {

            m_scopeSystem.EnterScope("global");

            base.VisitCompileUnit(node, args);

            return default(LValue);
        }

        public override LValue VisitDeclaration_Function(CDeclarationFunction node, params EvalParams[] args) {

            m_scopeSystem.EnterScope(node.MFunctionName);

            base.VisitDeclaration_Function(node, args);

            m_scopeSystem.ExitScope();

            return default(LValue);
        }

        public override LValue VisitCommand_CommandBlock(CCommand_CommandBlock node, params EvalParams[] args) {
            Scope commandBlockScope = node[typeof(Scope)] as Scope;

            m_scopeSystem.EnterScope(commandBlockScope.M_ScopeName);

            base.VisitCommand_CommandBlock(node, args);

            m_scopeSystem.ExitScope();

            return default(LValue);
        }

        public override LValue VisitExpression_Equation(CExpression_Equation node, params EvalParams[] args) {
            CIDENTIFIER lhs;

            // Grammar Rule  expression = expression;
            // Assignment can only be accomplished if lhs is an IDENTIFIER that's 
            // why it is checked before proceed in execution

            // 1. Validate the Identifier on LHS
            lhs = node.GetChild(CExpression_Equation.LHS) as CIDENTIFIER;
            if (lhs == null) {
                throw new Exception("Evaluator Exception: lhs must be Identifier");
            }

            // 2. Evaluate expression
            CExpression rhs = node.GetChild(CExpression_Equation.RHS) as CExpression;
            LValue result = Visit(rhs);

            // 3. Search Identifier in SymbolTable in Variables' namespace
            LSymbol IDsymbol = m_scopeSystem.SearchSymbol(lhs.M_StringLiteral, SymbolCategory.ST_VARIABLE);

            // 4. Assign expression result to identifier value
            IDsymbol.MValue = result;

            // 5. Log result
            EvaluatorLog log = new EvaluatorLog() {
                MResultScope = m_scopeSystem.M_CurrentScope.M_ScopeName,
                MResultName = IDsymbol.MName,
                MResult = result,
                MMethod = "VisitExpression_Equation"
            };
            Log(log);

            // 6. Return result
            return result;
        }




        public override LValue VisitExpression_Number(CExpression_Number node, params EvalParams[] args) {
            // 1. Evaluate Number
            ASTElement number = node.GetChild(CExpression_Number.NUMBER);
            LValue result = Visit(number);

            // 2. Return result
            return result;
        }

        public override LValue VisitT_INTEGERNUMBER(CINTEGERNUMBER node, params EvalParams[] args) {
            Expression<Func<LValue>> number = () => node.M_Value;
            return node.M_Value;
        }

        public override LValue VisitT_FLOATNUMBER(CFLOATNUMBER node, params EvalParams[] args) {
            return node.MValue;
        }
    }
}
