
using MATHL.Composite;

namespace MATHL.TypeSystem {
    // The base class of symbols of various categories
    public enum SymbolCategory {
        ST_NA, ST_VARIABLE, ST_FUNCTION, ST_TYPENAME
    }

    public abstract class LSymbol {
        string m_name;
        private LType m_type;
        private int value;
        private SymbolCategory m_symbolCategory;

        public LSymbol(string mName, SymbolCategory mSymbolType,LType mType) {
            m_name = mName;
            m_type = mType;
            m_symbolCategory = mSymbolType;
        }

        public string MName => m_name;

        public LType MType => m_type;

        public int MValue {
            get => value;
            set => this.value = value;
        }

        public SymbolCategory MSymbolCategory => m_symbolCategory;

        public override string ToString() {
            return MName + ":" + m_type;
        }
    }

    public class VariableSymbol : LSymbol {
        public VariableSymbol(string mName, LType mType) :
            base(mName, SymbolCategory.ST_VARIABLE, mType) { }
    }

    public class FunctionSymbol : LSymbol {
        private List<VariableSymbol> m_ParameterSymbols;
        ASTElement m_FunctionBody;

        public FunctionSymbol(string mName, LType mType) :
            base(mName, SymbolCategory.ST_FUNCTION, mType) {
            m_ParameterSymbols = new List<VariableSymbol>();
        }
        public void AddParameter(VariableSymbol p) {
            m_ParameterSymbols.Add(p);
        }
        public void AddFunctionRoot(ASTElement r) {
            m_FunctionBody = r;
        }
    }

    public class TypenameSymbol : LSymbol {
        public TypenameSymbol(string mName, LType mType) :
            base(mName, SymbolCategory.ST_TYPENAME, mType) { }
    }
}