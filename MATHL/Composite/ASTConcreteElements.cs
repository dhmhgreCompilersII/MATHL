using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MATHL.TypeSystem;

namespace MATHL.Composite {


    public enum NodeType {
        NT_NA = -1, NT_COMPILEUNIT, NT_COMMAND_EXPRESSION, NT_DECLARATION_VARIABLE, NT_DECLARATOR_VARIABLE,
        NT_DECLARATION_FUNCTION, NT_COMMAND_COMMANDBLOCK, NT_COMMAND_RETURN,
        NT_EXPRESSION_EQUATION, NT_EXPRESSION_ADDITION, NT_EXPRESSION_SUBTRACTION, NT_EXPRESSION_MULTIPLICATION,
        NT_EXPRESSION_FDIVISION, NT_EXPRESSION_IDIVISION, NT_EXPRESSION_MODULO,NT_EXPRESSION_UNARYPLUS,
        NT_EXPRESSION_UNARYMINUS, NT_EXPRESSION_RANGE,NT_EXPRESSION_FUNCTIONCALL, NT_EXPRESSION_NUMBER,
        NT_EXPRESSION_PARENTHESIS,

        T_INTTYPE, T_FLOATTYPE, T_RANGETYPE,T_IDENTIFIER, T_FLOATNUMBER, T_INTEGERNUMBER
    }

    public class CCompileUnit : ASTComposite {
        public const int COMMANDS = 0;
        public readonly string[] mc_contextNames = { "Commands" };

        public CCompileUnit() :
            base(1, (int)NodeType.NT_COMPILEUNIT) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitCompileUnit(this, info);
        }
    }
    
    public class CCommand_Expression : ASTComposite {
        public const int COMMAND = 0;
        public readonly string[] mc_contextNames = { "Command_Expression" };

        public ASTElement M_Expression => GetChild(COMMAND, 0);
        public CCommand_Expression() :
            base(1, (int)NodeType.NT_COMMAND_EXPRESSION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitCommand_Expression(this, info);
        }
    }

    public abstract class CExpression : ASTComposite {
        private LType m_type=null;
        private bool m_isConstantExpression=false;

        private AlgebraElement m_algebraElement;

        public AlgebraElement M_AlgebraElement {
            get => m_algebraElement;
            set => m_algebraElement = value ?? throw new ArgumentNullException(nameof(value));
        }

        public bool M_IsConstantExpression {
            get => m_isConstantExpression;
            set => m_isConstantExpression = value;
        }

        public LType M_ExpressionType {
            get => m_type;
            set => m_type = value ?? throw new ArgumentNullException(nameof(value));
        }

        public CExpression(int contexts, int mType) : base(contexts, mType) { }
    }

    public class CExpression_FunctionCall : CExpression {
        public const int NAME = 0, PARAMS= 1;
        public readonly string[] mc_contextNames = { "FUNCTIONNAME", "ARGUMENTS" };

        public ASTElement M_FunctionName => GetChild(NAME, 0);
        public IEnumerable<ASTElement> M_Params => GetContextChildren(PARAMS);

        public CExpression_FunctionCall() :
            base(2, (int)NodeType.NT_EXPRESSION_FUNCTIONCALL) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_FunctionCall(this, info);
        }
    }
    public class CExpression_Equation : CExpression {
        public const int LHS = 0, RHS=1;
        public readonly string[] mc_contextNames = { "LHS", "RHS" };

        public ASTElement M_LHSExpression => GetChild(LHS, 0);
        public ASTElement M_RHSExpression => GetChild(RHS, 0);

        public CExpression_Equation() :
            base(2, (int)NodeType.NT_EXPRESSION_EQUATION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_Equation(this, info);
        }
    }
    public class CExpression_UnaryPlus : CExpression {
        public const int EXPR = 0;
        public readonly string[] mc_contextNames = { "Expression" };

        public ASTElement M_Expression => GetChild(EXPR, 0);

        public CExpression_UnaryPlus() :
            base(1, (int)NodeType.NT_EXPRESSION_UNARYPLUS) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_UnaryPlus(this, info);
        }
    }
    public class CExpression_UnaryMinus : CExpression {
        public const int EXPR = 0;
        public readonly string[] mc_contextNames = { "Expression" };

        public ASTElement M_Expression => GetChild(EXPR, 0);

        public CExpression_UnaryMinus() :
            base(1, (int)NodeType.NT_EXPRESSION_UNARYMINUS) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_UnaryMinus(this, info);
        }
    }
    public class CExpression_Addition : CExpression {
        public const int LHS = 0, RHS = 1;
        public readonly string[] mc_contextNames = { "LHS", "RHS" };

        public ASTElement M_LHSExpression => GetChild(LHS, 0);
        public ASTElement M_RHSExpression => GetChild(RHS, 0);

        public CExpression_Addition() :
            base(2, (int)NodeType.NT_EXPRESSION_ADDITION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_Addition(this, info);
        }
    }
    public class CExpression_Subtraction : CExpression {
        public const int LHS = 0, RHS = 1;
        public readonly string[] mc_contextNames = { "LHS", "RHS" };

        public ASTElement M_LHSExpression => GetChild(LHS, 0);
        public ASTElement M_RHSExpression => GetChild(RHS, 0);

        public CExpression_Subtraction() :
            base(2, (int)NodeType.NT_EXPRESSION_SUBTRACTION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_Subtraction(this, info);
        }
    }
    public class CExpression_Multiplication : CExpression {
        public const int LHS = 0, RHS = 1;
        public readonly string[] mc_contextNames = { "LHS", "RHS" };

        public ASTElement M_LHSExpression => GetChild(LHS, 0);
        public ASTElement M_RHSExpression => GetChild(RHS, 0);
        public CExpression_Multiplication() :
            base(2, (int)NodeType.NT_EXPRESSION_MULTIPLICATION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_Multiplication(this, info);
        }
    }
    public class CExpression_FDivision : CExpression {
        public const int LHS = 0, RHS = 1;
        public readonly string[] mc_contextNames = { "NUMERATOR", "DENOMINATOR" };

        public ASTElement M_LHSExpression => GetChild(LHS, 0);
        public ASTElement M_RHSExpression => GetChild(RHS, 0);
        public CExpression_FDivision() :
            base(2, (int)NodeType.NT_EXPRESSION_FDIVISION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_FDivision(this, info);
        }
    }
    public class CExpression_IDivision : CExpression {
        public const int LHS = 0, RHS = 1;
        public readonly string[] mc_contextNames = { "NUMERATOR", "DENOMINATOR" };

        public ASTElement M_LHSExpression => GetChild(LHS, 0);
        public ASTElement M_RHSExpression => GetChild(RHS, 0);
        public CExpression_IDivision() :
            base(2, (int)NodeType.NT_EXPRESSION_IDIVISION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_IDivision(this, info);
        }
    }
    public class CExpression_Modulo : CExpression {
        public const int LHS = 0, RHS = 1;
        public readonly string[] mc_contextNames = { "NUMERATOR", "DENOMINATOR" };

        public ASTElement M_LHSExpression => GetChild(LHS, 0);
        public ASTElement M_RHSExpression => GetChild(RHS, 0);
        public CExpression_Modulo() :
            base(2, (int)NodeType.NT_EXPRESSION_MODULO) {
        }
        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_Modulo(this, info);
        }
    }
    public class CExpression_ParenthesizedExpression : CExpression {
        public const int EXPR = 0;
        public readonly string[] mc_contextNames = { "Expression" };

        public ASTElement M_Expression => GetChild(EXPR, 0);
        public CExpression_ParenthesizedExpression() : 
            base(1, (int)NodeType.NT_EXPRESSION_PARENTHESIS) { }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v, params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_ParenthesizedExpression(this, info);
        }
    }
    public class CExpression_Number : CExpression {
        public const int NUMBER = 0;
        public readonly string[] mc_contextNames = { "NUMBER" };

        public CExpression_Number() :
            base(1, (int)NodeType.NT_EXPRESSION_NUMBER) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_Number(this, info);
        }
    }
    public class CExpression_Range : CExpression {
        public const int START = 0, END = 1 , STEP=2;
        public readonly string[] mc_contextNames = { "Start", "End", "Step" };

        public ASTElement M_StartExpression => GetChild(START, 0);
        public ASTElement M_EndExpression => GetChild(END, 0);
        public ASTElement M_StepExpression => GetChild(STEP, 0);
        
        public CExpression_Range() :
            base(3, (int)NodeType.NT_EXPRESSION_RANGE) {
        }
        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_Range(this, info);
        }
    }
    public class CCommand_CommandBlock : ASTComposite {
        public const int COMMAND = 0;
        public readonly string[] mc_contextNames = { "Command_CommandBlock" };
        private string m_scopeName;
        
        public CCommand_CommandBlock() :
            base(1, (int)NodeType.NT_COMMAND_COMMANDBLOCK) {
        }
        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitCommand_CommandBlock(this, info);
        }
    }

    public class CCommand_Return : ASTComposite {
        public const int EXPRESSION = 0;
        public readonly string[] mc_contextNames = { "CommandReturn_Expression" };

        public CCommand_Return() :
            base(1, (int)NodeType.NT_COMMAND_RETURN) {
        }
        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitCommand_Return(this, info);
        }
    }
    public class CDeclarationVariable : ASTComposite {
        public const int TYPE=0, DECLARATIONS = 1;
        public readonly string[] mc_contextNames = { "Type","Declaration_Declarators" };
        
        public CDeclarationVariable() :
            base(2, (int)NodeType.NT_DECLARATION_VARIABLE) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitDeclaration_Variable(this, info);
        }
    }
    public class CDeclaratorVariable : ASTComposite {
        public const int TYPE = 0, VARIABLENAME = 1, INITIALIZATION=2;
        public readonly string[] mc_contextNames = { "Type", "VariableName", "Initialization" };

        public CDeclaratorVariable() :
            base(3, (int)NodeType.NT_DECLARATOR_VARIABLE) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitDeclarator_Variable(this, info);
        }
    }
    public class CDeclarationFunction : ASTComposite {
        public const int TYPE = 0, FUNCTION_NAME=1, PARAMETERS = 2, BODY=3;
        public readonly string[] mc_contextNames = { "ReturnType", "FunctionName", "Parameters", "FunctionBody"};
        private string m_functionName;

        public string MFunctionName => m_functionName;

        public CDeclarationFunction(string functionName) :
            base(4, (int)NodeType.NT_DECLARATION_FUNCTION) {
            m_functionName = functionName;
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitDeclaration_Function(this, info);
        }
    }
    
    public class CINTEGERNUMBER : ASTLeaf {
        private LType m_type;

        private LValue m_value;

        public override string MNodeName => m_nodeName + "_" + M_StringLiteral;

        public LType M_Type => m_type;

        public LValue M_Value => m_value;
        
        public CINTEGERNUMBER(string leafLiteral) :
            base(leafLiteral, (int)NodeType.T_INTEGERNUMBER) {
            m_type = MATHLExecutionEnvironment.GetInstance().M_ScopeSystem.M_GlobalScope.
                SearchSymbol(IntegerType.mc_typename,SymbolCategory.ST_TYPENAME).MType;
            m_value.MType = TypeID.TID_INTEGER;
            m_value.Ivalue = Int32.Parse(leafLiteral);
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v, params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitT_INTEGERNUMBER(this, info);
        }
    }

    public class CFLOATNUMBER : ASTLeaf {
        private LType m_type;

        private LValue m_value;

        public override string MNodeName => m_nodeName + "_" + M_StringLiteral;

        public LType M_Type => m_type;

        public LValue M_Value  => m_value;
        

        public CFLOATNUMBER(string leafLiteral) :
            base(leafLiteral, (int)NodeType.T_FLOATNUMBER) {
            m_type = m_type = MATHLExecutionEnvironment.GetInstance().M_ScopeSystem.M_GlobalScope.
                SearchSymbol(FloatingType.mc_typename, SymbolCategory.ST_TYPENAME).MType; ;
            m_value.MType = TypeID.TID_FLOAT;
            m_value.Fvalue = float.Parse(leafLiteral,CultureInfo.InvariantCulture);
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v, params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitT_FLOATNUMBER(this, info);
        }
    }

    public class CIDENTIFIER : ASTLeaf {
        
        public override string MNodeName => m_nodeName + "_" + M_StringLiteral;
        public Scope M_CurrentScope => this[typeof(Scope)] as Scope;
        
        public LSymbol MSymbol => this[typeof(LSymbol)] as LSymbol;

        public CIDENTIFIER(string leafLiteral) :
            base(leafLiteral, (int)NodeType.T_IDENTIFIER) {
        }
        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v, params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitT_IDENTIFIER(this, info);
        }
    }
    public class CIntType : ASTLeaf {
        private LType m_type;

        public override string MNodeName => m_nodeName + "_" + M_StringLiteral;

        public LType MType1 => m_type;

        public CIntType(string leafLiteral) :
            base(leafLiteral, (int)NodeType.T_INTTYPE) {
            m_type = MATHLExecutionEnvironment.GetInstance().M_ScopeSystem.M_GlobalScope
                .SearchSymbol(M_StringLiteral, SymbolCategory.ST_TYPENAME).MType;
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v, params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitT_IntegerDataType(this, info);
        }
    }
    public class CFloatType : ASTLeaf {
        private LType m_type;

        public override string MNodeName => m_nodeName + "_" + M_StringLiteral;

        public LType MType1 => m_type;

        public CFloatType(string leafLiteral) :
            base(leafLiteral, (int)NodeType.T_FLOATTYPE) {
            m_type = MATHLExecutionEnvironment.GetInstance().M_ScopeSystem.M_GlobalScope
                .SearchSymbol(M_StringLiteral, SymbolCategory.ST_TYPENAME).MType;
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v, params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitT_FloatDataType(this, info);
        }
    }
    public class CRangeType : ASTLeaf {
        private LType m_type;

        public override string MNodeName => m_nodeName + "_" + M_StringLiteral;

        public LType M_Type => m_type;

        public CRangeType(string leafLiteral) :
            base(leafLiteral, (int)NodeType.T_RANGETYPE) {
            m_type = new RangeType();
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v, params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitT_RangeDataType(this, info);
        }
    }
}
