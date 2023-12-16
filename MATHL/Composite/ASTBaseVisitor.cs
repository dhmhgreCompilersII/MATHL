using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATHL.Composite {
    public abstract class ASTBaseVisitor<Return, Params> : IASTBaseVisitor<Return, Params> {

        // Visit a specific node and send a variable number of
        // arguments. The responsibility of the type and sequence
        // of arguments is on the user. ( box/unboxing for scalars)
        public virtual Return Visit(IASTVisitableNode node, params Params[] info) {
            return node.Accept<Return, Params>(this, info);
        }

        // Visit the children of a specific node and summarize the 
        // results by the visiting each child 
        public virtual Return VisitChildren(IASTComposite node, params Params[] info) {
            Return result = default(Return);
            Return iResult;
            foreach (IASTVisitableNode astNode in node) {
                iResult = astNode.Accept<Return, Params>(this, info);
                result = Summarize(iResult, result);
            }
            return result;
        }

        public virtual Return Summarize(Return iresult, Return result) {
            return iresult;
        }
    }

    public class MATHLBaseVisitor<Result, Params> : ASTBaseVisitor<Result, Params> {

        public virtual Result VisitContextChildren(ASTComposite node, int context,
            params Params[] info) {
            Result result = default(Result);
            Result iResult;
            IASTIterator it = node.CreateContextIterator(context);
            ASTElement astNode;
            for (it.Init(); it.End() == false; it.Next()) {
                astNode = it.MCurNode;
                iResult = astNode.Accept<Result, Params>(this, info);
                result = Summarize(iResult, result);
            }
            return result;
        }

        public override Result VisitChildren(IASTComposite node, params Params[] info) {
            ASTComposite n = node as ASTComposite;
            Result result = default(Result);
            Result iResult;
            IASTIterator it = n.CreateIterator();
            ASTElement astNode;
            for (it.Init(); it.End() == false; it.Next()) {
                astNode = it.MCurNode;
                iResult = astNode.Accept<Result, Params>(this, info);
                result = Summarize(iResult, result);
            }
            return result;
        }

        public virtual Result VisitCompileUnit(CCompileUnit node, params Params[] args) {
            return VisitChildren(node, args);
        }

        public virtual Result VisitDeclaration_Function(CDeclarationFunction node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitCommand_Expression(CCommand_Expression node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitExpression_FunctionCall(CExpression_FunctionCall node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitExpression_Equation(CExpression_Equation node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitExpression_UnaryPlus(CExpression_UnaryPlus node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitExpression_UnaryMinus(CExpression_UnaryMinus node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitExpression_Addition(CExpression_Addition node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitExpression_Subtraction(CExpression_Subtraction node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitExpression_Multiplication(CExpression_Multiplication node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitExpression_FDivision(CExpression_FDivision node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitExpression_IDivision(CExpression_IDivision node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitExpression_Modulo(CExpression_Modulo node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitExpression_Range(CExpression_Range node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitExpression_Number(CExpression_Number node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitCommand_CommandBlock(CCommand_CommandBlock node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitCommand_Return(CCommand_Return node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitDeclaration_Variable(CDeclarationVariable node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitDeclarator_Variable(CDeclaratorVariable node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitT_IntegerDataType(CIntType node, params Params[] args) {
            return default(Result);
        }
        public virtual Result VisitT_FloatDataType(CFloatType node, params Params[] args) {
            return default(Result);
        }
        public virtual Result VisitT_RangeDataType(CRangeType node, params Params[] args) {
            return default(Result);
        }
        public virtual Result VisitT_INTEGERNUMBER(CINTEGERNUMBER node, params Params[] args) {
            return default(Result);
        }
        public virtual Result VisitT_FLOATNUMBER(CFLOATNUMBER node, params Params[] args) {
            return default(Result);
        }
        public virtual Result VisitT_IDENTIFIER(CIDENTIFIER node, params Params[] args) {
            return default(Result);
        }
    }
}
