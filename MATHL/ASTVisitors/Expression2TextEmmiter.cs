using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MATHL.Composite;

namespace MATHL.ASTVisitors {
    public class Expression2TextParams {

    }

    public class Expression2TextEmmiter : MATHLBaseVisitor<string,Expression2TextParams> {
        public override string? VisitExpression_Number(CExpression_Number node, params Expression2TextParams[] args) {
            return base.VisitExpression_Number(node, args);
        }

        public override string VisitCommand_Expression(CCommand_Expression node, params Expression2TextParams[] args) {
            string expr = Visit(node.M_Expression);
            Console.WriteLine(expr);
            return expr + ";";
        }

        public override string VisitExpression_FunctionCall(CExpression_FunctionCall node, params Expression2TextParams[] args) {
            StringBuilder exprBuilder = new StringBuilder();
            exprBuilder.Append(Visit(node.M_FunctionName));
            exprBuilder.Append("(");
            int i = 0;
            foreach (var astElement in node.M_Params) {
                if (i > 0) {
                    exprBuilder.Append(",");
                }
                exprBuilder.Append(Visit(astElement));
            }
            exprBuilder.Append(")");
            return exprBuilder.ToString();
        }

        public override string VisitExpression_Range(CExpression_Range node, params Expression2TextParams[] args) {
            StringBuilder exprBuilder = new StringBuilder();
            exprBuilder.Append("[");
            if (node.M_StartExpression != null) {
                exprBuilder.Append(Visit(node.M_StartExpression));
            }
            exprBuilder.Append(":");
            if (node.M_EndExpression != null) {
                exprBuilder.Append(Visit(node.M_EndExpression));
            }
            exprBuilder.Append(":");
            if (node.M_StepExpression != null) {
                exprBuilder.Append(Visit(node.M_StepExpression));
            }
            exprBuilder.Append("]");
            return exprBuilder.ToString();
        }

        public override string VisitExpression_Equation(CExpression_Equation node, params Expression2TextParams[] args) {

            string lhs = Visit(node.M_LHSExpression);
            string rhs = Visit(node.M_RHSExpression);
            return lhs + "=" + rhs;
        }

        public override string VisitExpression_UnaryPlus(CExpression_UnaryPlus node, params Expression2TextParams[] args) {
            string expr = Visit(node.M_Expression);
            return "+" + expr;
        }

        public override string VisitExpression_UnaryMinus(CExpression_UnaryMinus node, params Expression2TextParams[] args) {
            string expr = Visit(node.M_Expression);
            return "-" + expr;
        }

        public override string VisitExpression_ParenthesizedExpression(CExpression_ParenthesizedExpression node,
            params Expression2TextParams[] args) {
            string expr = Visit(node.M_Expression);
            return "(" + expr + ")";
        }

        public override string VisitExpression_Addition(CExpression_Addition node, params Expression2TextParams[] args) {
            string lhs = Visit(node.M_LHSExpression);
            string rhs = Visit(node.M_RHSExpression);

            return lhs + "+" + rhs;
        }

        public override string VisitExpression_Subtraction(CExpression_Subtraction node, params Expression2TextParams[] args) {
            string lhs = Visit(node.M_LHSExpression);
            string rhs = Visit(node.M_RHSExpression);

            return lhs + "+" + rhs;
        }

        public override string VisitExpression_Multiplication(CExpression_Multiplication node, params Expression2TextParams[] args) {
            string lhs = Visit(node.M_LHSExpression);
            string rhs = Visit(node.M_RHSExpression);

            return lhs + "*" + rhs;
        }

        public override string VisitExpression_FDivision(CExpression_FDivision node, params Expression2TextParams[] args) {
            string lhs = Visit(node.M_LHSExpression);
            string rhs = Visit(node.M_RHSExpression);

            return lhs + "/" + rhs;
        }

        public override string VisitExpression_IDivision(CExpression_IDivision node, params Expression2TextParams[] args) {
            string lhs = Visit(node.M_LHSExpression);
            string rhs = Visit(node.M_RHSExpression);

            return lhs + "/" + rhs;
        }

        public override string VisitExpression_Modulo(CExpression_Modulo node, params Expression2TextParams[] args) {
            string lhs = Visit(node.M_LHSExpression);
            string rhs = Visit(node.M_RHSExpression);

            return lhs + "%" + rhs;
        }

        public override string VisitT_INTEGERNUMBER(CINTEGERNUMBER node, params Expression2TextParams[] args) {
            return node.M_Value.Ivalue.ToString();
        }

        public override string VisitT_FLOATNUMBER(CFLOATNUMBER node, params Expression2TextParams[] args) {
            return node.M_Value.Fvalue.ToString();
        }

        public override string VisitT_IDENTIFIER(CIDENTIFIER node, params Expression2TextParams[] args) {
            return node.M_StringLiteral;
        }
    }
}
