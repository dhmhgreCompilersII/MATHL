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

        
    }
}
