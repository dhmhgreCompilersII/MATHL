using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MATHL.TypeSystem;

namespace MATHL.Composite {

    public enum AlgebraElements{
        ALE_ROOT, ALE_ADDITION, ALE_MULTIPLICATION,
        ALE_SYMBOL, ALE_VALUE
    }

    public abstract class AlgebraElement :ASTComposite{
        public AlgebraElement(int contexts, int mType) : base(contexts, mType) { }
    }

    public class ALERoot : AlgebraElement {
        public const int EXPRESSION = 0;

        public AlgebraElement M_Expression => (AlgebraElement)GetChild(EXPRESSION, 0); 

        public ALERoot() : 
            base(1, (int)AlgebraElements.ALE_ROOT) { }

        public override Result Accept<Result, Params>(IASTBaseVisitor<Result, Params> v,
            params Params[] info) {
            AlgebraBaseVisitor<Result, Params> visitor = v as AlgebraBaseVisitor<Result, Params>;
            return visitor.VisitExpressionRoot(this, info);
        }
    }

    public class ALESymbol : AlgebraElement {
        private string m_symbol;

        public string MSymbol => m_symbol;

        public ALESymbol(string id) : 
            base(0, (int)AlgebraElements.ALE_SYMBOL) { }
        public override Result Accept<Result, Params>(IASTBaseVisitor<Result, Params> v,
            params Params[] info) {
            AlgebraBaseVisitor<Result, Params> visitor = v as AlgebraBaseVisitor<Result, Params>;
            return visitor.VisitSymbol(this, info);
        }
    }

    public class ALENumber : AlgebraElement {
        private LValue m_value;

        public ALENumber(LValue value) :
            base(0, (int)AlgebraElements.ALE_VALUE) {
            m_value = value;
        }
        public override Result Accept<Result, Params>(IASTBaseVisitor<Result, Params> v,
            params Params[] info) {
            AlgebraBaseVisitor<Result, Params> visitor = v as AlgebraBaseVisitor<Result, Params>;
            return visitor.VisitNumber(this, info);
        }
    }

    public class ALEAddition : AlgebraElement {
        public const int TERMS = 0;

        private IEnumerable<AlgebraElement> M_Terms {
            get {
                IEnumerable<AlgebraElement>? terms;
                IEnumerable<ASTElement> terms_ = GetContextChildren(TERMS);
                terms = terms_ as IEnumerable<AlgebraElement>;  // covariance enabled casting
                return terms;
            }
        }
        public ALEAddition() : 
            base(1, (int)AlgebraElements.ALE_ADDITION) { }

        public override Result Accept<Result, Params>(IASTBaseVisitor<Result, Params> v,
            params Params[] info) {
            AlgebraBaseVisitor<Result, Params>? visitor = v as AlgebraBaseVisitor<Result, Params>;
            return visitor.VisitAddition(this, info);
        }
    }

    public class ALEMultiplication : AlgebraElement {
        public const int TERMS = 0;

        private IEnumerable<AlgebraElement> M_Terms {
            get {
                IEnumerable<AlgebraElement>? terms;
                IEnumerable<ASTElement> terms_ = GetContextChildren(TERMS);
                terms = terms_ as IEnumerable<AlgebraElement>;  // covariance enabled casting
                return terms;
            }
        }
        public ALEMultiplication() :
            base(1, (int)AlgebraElements.ALE_MULTIPLICATION) { }

        public override Result Accept<Result, Params>(IASTBaseVisitor<Result, Params> v,
            params Params[] info) {
            AlgebraBaseVisitor<Result, Params> visitor = v as AlgebraBaseVisitor<Result, Params>;
            return visitor.VisitMultiplication(this, info);
        }
    }
}
