
namespace MATHL.TypeSystem {
    // The base class of symbols of various categories
    public class LSymbol {
        string m_name;
        private LType m_type;

        public string MName => m_name;

        public LType MType => m_type;
    }

    public class VariableSymbol : LSymbol {

    }

    public class FunctionSymbol : LSymbol {

    }
}