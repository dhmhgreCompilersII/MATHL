﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MATHL.TypeSystem;

namespace MATHL.Composite {


    public enum NodeType {
        NT_NA = -1, NT_COMPILEUNIT, NT_COMMAND_EXPRESSION, NT_DECLARATION_VARIABLE, NT_DECLARATOR_VARIABLE,
        NT_DECLARATION_FUNCTION, NT_COMMAND_COMMANDBLOCK, 
        NT_EXPRESSION_EQUATION, NT_EXPRESSION_ADDITION, NT_EXPRESSION_SUBTRACTION, NT_EXPRESSION_MULTIPLICATION,
        NT_EXPRESSION_FDIVISION, NT_EXPRESSION_IDIVISION, NT_EXPRESSION_MODULO,NT_EXPRESSION_UNARYPLUS,
        NT_EXPRESSION_UNARYMINUS,
        
        T_INTTYPE, T_FLOATTYPE, T_RANGETYPE,T_NUMBER,T_IDENTIFIER,
        
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

        public CCommand_Expression() :
            base(1, (int)NodeType.NT_COMMAND_EXPRESSION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitCommand_Expression(this, info);
        }
    }
    public class CExpression_Equation : ASTComposite {
        public const int LHS = 0, RHS=1;
        public readonly string[] mc_contextNames = { "LHS", "RHS" };

        public CExpression_Equation() :
            base(2, (int)NodeType.NT_EXPRESSION_EQUATION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_Equation(this, info);
        }
    }
    public class CExpression_UnaryPlus : ASTComposite {
        public const int EXPR = 0;
        public readonly string[] mc_contextNames = { "Expression" };

        public CExpression_UnaryPlus() :
            base(1, (int)NodeType.NT_EXPRESSION_UNARYPLUS) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_UnaryPlus(this, info);
        }
    }
    public class CExpression_UnaryMinus : ASTComposite {
        public const int EXPR = 0;
        public readonly string[] mc_contextNames = { "Expression" };

        public CExpression_UnaryMinus() :
            base(1, (int)NodeType.NT_EXPRESSION_UNARYMINUS) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_UnaryMinus(this, info);
        }
    }
    public class CExpression_Addition : ASTComposite {
        public const int LHS = 0, RHS = 1;
        public readonly string[] mc_contextNames = { "LHS", "RHS" };

        public CExpression_Addition() :
            base(2, (int)NodeType.NT_EXPRESSION_ADDITION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_Addition(this, info);
        }
    }
    public class CExpression_Subtraction : ASTComposite {
        public const int LHS = 0, RHS = 1;
        public readonly string[] mc_contextNames = { "LHS", "RHS" };

        public CExpression_Subtraction() :
            base(2, (int)NodeType.NT_EXPRESSION_SUBTRACTION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_Subtraction(this, info);
        }
    }
    public class CExpression_Multiplication : ASTComposite {
        public const int LHS = 0, RHS = 1;
        public readonly string[] mc_contextNames = { "LHS", "RHS" };

        public CExpression_Multiplication() :
            base(2, (int)NodeType.NT_EXPRESSION_MULTIPLICATION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_Multiplication(this, info);
        }
    }
    public class CExpression_FDivision : ASTComposite {
        public const int LHS = 0, RHS = 1;
        public readonly string[] mc_contextNames = { "LHS", "RHS" };

        public CExpression_FDivision() :
            base(2, (int)NodeType.NT_EXPRESSION_FDIVISION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_FDivision(this, info);
        }
    }
    public class CExpression_IDivision : ASTComposite {
        public const int LHS = 0, RHS = 1;
        public readonly string[] mc_contextNames = { "LHS", "RHS" };

        public CExpression_IDivision() :
            base(2, (int)NodeType.NT_EXPRESSION_IDIVISION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_IDivision(this, info);
        }
    }
    public class CExpression_Modulo : ASTComposite {
        public const int LHS = 0, RHS = 1;
        public readonly string[] mc_contextNames = { "LHS", "RHS" };

        public CExpression_Modulo() :
            base(2, (int)NodeType.NT_EXPRESSION_MODULO) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitExpression_Modulo(this, info);
        }
    }
    public class CCommand_CommandBlock : ASTComposite {
        public const int COMMAND = 0;
        public readonly string[] mc_contextNames = { "Command_CommandBlock" };

        public CCommand_CommandBlock() :
            base(1, (int)NodeType.NT_COMMAND_COMMANDBLOCK) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitCommand_CommandBlock(this, info);
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
        public const int TYPE = 0, FUNCTION_NAME=1, PARAMETERS = 2;
        public readonly string[] mc_contextNames = { "ReturnType", "FunctionName", "Parameters"};

        public CDeclarationFunction() :
            base(3, (int)NodeType.NT_DECLARATION_FUNCTION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitDeclaration_Function(this, info);
        }
    }
    public class CNUMBER : ASTLeaf {
        private LType m_type;

        public override string MNodeName => m_nodeName + "_" + MStringLiteral;

        public LType MType1 => m_type;

        public CNUMBER(string leafLiteral) :
            base(leafLiteral, (int)NodeType.T_NUMBER) {
            m_type = new IntegerType();
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v, params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitT_NUMBER(this, info);
        }
    }

    public class CIDENTIFIER : ASTLeaf {
        
        public override string MNodeName => m_nodeName + "_" + MStringLiteral;

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

        public override string MNodeName => m_nodeName + "_" + MStringLiteral;

        public LType MType1 => m_type;

        public CIntType(string leafLiteral) :
            base(leafLiteral, (int)NodeType.T_INTTYPE) {
            m_type = new IntegerType();
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v, params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitT_IntegerDataType(this, info);
        }
    }
    public class CFloatType : ASTLeaf {
        private LType m_type;

        public override string MNodeName => m_nodeName + "_" + MStringLiteral;

        public LType MType1 => m_type;

        public CFloatType(string leafLiteral) :
            base(leafLiteral, (int)NodeType.T_FLOATTYPE) {
            m_type = new FloatingType();
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v, params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitT_FloatDataType(this, info);
        }
    }
    public class CRangeType : ASTLeaf {
        private LType m_type;

        public override string MNodeName => m_nodeName + "_" + MStringLiteral;

        public LType MType1 => m_type;

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
