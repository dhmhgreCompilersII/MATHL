
namespace MATHL.TypeSystem {
    // The base class of symbols of various categories
    public enum SymbolType {
        ST_NA, ST_VARIABLE, ST_FUNCTION
    }

    public class LSymbol {
        string m_name;
        private LType m_type;
        private SymbolType m_symbolType;

        public LSymbol(string mName, LType mType, SymbolType mSymbolType) {
            m_name = mName;
            m_type = mType;
            m_symbolType = mSymbolType;
        }

        public string MName => m_name;

        public LType MType => m_type;

        public SymbolType MSymbolType => m_symbolType;
    }

    public class VariableSymbol : LSymbol {
        public VariableSymbol(string mName, LType mType) :
            base(mName, mType, SymbolType.ST_VARIABLE) { }
    }

    public class FunctionSymbol : LSymbol {
        public FunctionSymbol(string mName, LType mType) :
            base(mName, mType, SymbolType.ST_FUNCTION) { }
    }
}