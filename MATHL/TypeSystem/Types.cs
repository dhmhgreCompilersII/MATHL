namespace MATHL.TypeSystem {
    public class LType {
        private string m_typename;

    }

    public class IntegerType : LType {
        public override string ToString() {
            return "IntegerType";
        }
    }

    public class FloatingType : LType {
        public override string ToString() {
            return "FloatingType";
        }
    }
}
