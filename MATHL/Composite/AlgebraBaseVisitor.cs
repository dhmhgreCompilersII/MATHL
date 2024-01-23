using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATHL.Composite {
    public abstract class AlgebraBaseVisitor<Result,Params> :
        IASTBaseVisitor<Result, Params> {
        public Result Visit(IASTVisitableNode node, params Params[] info) {
            return node.Accept<Result, Params>(this, info);
        }

        public Result VisitChildren(IASTComposite node, params Params[] info) {
            Result result = default(Result);
            Result iResult;
            foreach (IASTVisitableNode astNode in node) {
                iResult = astNode.Accept<Result, Params>(this, info);
                result = Summarize(iResult, result);
            }
            return result;
        }

        public virtual Result Summarize(Result iresult, Result result) {
            return iresult;
        }


        public virtual Result VisitSymbol(ALESymbol node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitNumber(ALENumber node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitExpressionRoot(ALERoot node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitAddition(ALEAddition node, params Params[] args) {
            return VisitChildren(node, args);
        }
        public virtual Result VisitMultiplication(ALEMultiplication node, params Params[] args) {
            return VisitChildren(node, args);
        }
    }
}
