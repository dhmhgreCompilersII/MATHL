namespace MATHL.TypeSystem {
    public class LType {
        protected string m_typename;

        public LType(string mTypename) {
            m_typename = mTypename;
        }
    }

    public class IntegerType : LType {
        public IntegerType() : base("IntegerType") { }

        public override string ToString() {
            return m_typename;
        }
    }

    public class FloatingType : LType {
        public FloatingType() : base("FloatingType") { }

        public override string ToString() {
            return "FloatingType";
        }
    }
}
