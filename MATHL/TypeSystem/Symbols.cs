
namespace MATHL.TypeSystem {
    // The base class of symbols of various categories
    public enum SymbolType {
        ST_NA, ST_VARIABLE, ST_FUNCTION, ST_TYPENAME
    }

    public abstract class LSymbol {
        string m_name;
        private LType m_type;
        private SymbolType m_symbolType;

        public LSymbol(string mName, SymbolType mSymbolType,LType mType) {
            m_name = mName;
            m_type = mType;
            m_symbolType = mSymbolType;
        }

        public string MName => m_name;

        public LType MType => m_type;

        public SymbolType MSymbolType => m_symbolType;

        public override string ToString() {
            return MName + ":" + m_type;
        }
    }

    public class VariableSymbol : LSymbol {
        public VariableSymbol(string mName, LType mType) :
            base(mName, SymbolType.ST_VARIABLE, mType) { }
    }

    public class FunctionSymbol : LSymbol {
        public FunctionSymbol(string mName, LType mType) :
            base(mName, SymbolType.ST_FUNCTION, mType) { }
    }

    public class TypenameSymbol : LSymbol {
        public TypenameSymbol(string mName, LType mType) :
            base(mName, SymbolType.ST_TYPENAME, mType) { }
    }
}