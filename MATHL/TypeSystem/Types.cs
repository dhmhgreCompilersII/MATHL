using System.Text;

namespace MATHL.TypeSystem {

    public enum TypeID {
        TID_INTEGER, TID_FLOAT, TID_RANGE, TID_FUNCTION
    }

    public class LType {
        protected string m_typename;
        TypeID m_typeID;

        public string MTypename => m_typename;

        public TypeID MTypeId => m_typeID;

        public LType(TypeID tid, string mTypename) {
            m_typename = mTypename;
            m_typeID = tid;
        }
    }

    public class IntegerType : LType {
        public IntegerType() : base(TypeID.TID_INTEGER ,"IntegerType") { }

        public override string ToString() {
            return m_typename;
        }
    }

    public class FloatingType : LType {
        public FloatingType() : base(TypeID.TID_FLOAT,"FloatingType") { }

        public override string ToString() {
            return "FloatingType";
        }
    }
    public class FunctionType : LType {
        private List<LType> m_parameters = new List<LType>();
        private LType m_returnType;
        public FunctionType(LType returnType, List<LType> parameters) :
            base(TypeID.TID_FUNCTION, "Function") {
            m_returnType = returnType;
            m_typename += m_returnType + ",function";
            foreach (LType parameter in parameters) {
                AddParameter(parameter);
            }
        }

        public override string ToString() {
            StringBuilder typename = new StringBuilder($"{m_returnType} <f>(");
            int i = 0;
            foreach (LType type in m_parameters) {
                if (i > 0) {
                    typename.Append(", ");
                }
                typename.Append(type.ToString());
                i++;
            }
            typename.Append(")");
            return typename.ToString();
        }

        private void AddParameter(LType param) {
            m_parameters.Add(param);
            m_typename += "," + param.MTypename;
        }
    }

    public record CRange {
        private int? m_StartIndex;
        private int? m_EndIndex;
        private int? m_Step;
        
        public int? MStartIndex {
            get => m_StartIndex;
            set => m_StartIndex = value;
        }

        public int? MEndIndex {
            get => m_EndIndex;
            set => m_EndIndex = value;
        }

        public int? MStep {
            get => m_Step;
            set => m_Step = value;
        }

        public override string ToString() {
            return $"[{MStartIndex}:{MEndIndex}:{MStep}]";
        }
    }

    public class RangeType : LType {
        private int m_dimensions=1;

        public int MDimensions {
            get => m_dimensions;
            set => m_dimensions = value;
        }

        public override bool Equals(object? obj) {
            if ((obj as RangeType) == null) {
                throw new Exception("Inconsistent type comparison");
            }
            return (RangeType)this==(RangeType)obj;
        }

        public static bool operator ==(RangeType this_,RangeType other) {
            if (this_.m_dimensions != other.m_dimensions) return false;
            return true;
        }

        public void AddDimension() {
            m_dimensions++;
        }

        public static bool operator !=(RangeType this_, RangeType other) {
            return !(this_==other);
        }
        public RangeType() :
            base(TypeID.TID_RANGE,"RangeType") { }
        
        public override string ToString() {
            return "RangeType";
        }
    }
}
