
using MATHL.Composite;
using System.Runtime.InteropServices;

namespace MATHL.TypeSystem {
    // The base class of symbols of various categories
    public enum SymbolCategory {
        ST_NA, ST_VARIABLE, ST_FUNCTION, ST_TYPENAME
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct LValue {
        [FieldOffset(0)]
        public int ivalue;
        [FieldOffset(0)]
        public float fvalue;
    }

    public abstract class LSymbol {
        string m_name;
        private LType m_type;
        private LValue m_value;
        private SymbolCategory m_symbolCategory;
        
        public LSymbol(string mName, SymbolCategory mSymbolType,LType mType) {
            m_name = mName;
            m_type = mType;
            m_symbolCategory = mSymbolType;
        }

        public string MName => m_name;

        public LType MType => m_type;

        public LValue MValue  => m_value;
        

        public SymbolCategory MSymbolCategory => m_symbolCategory;

        public override string ToString() {
            return MName + ":" + m_type;
        }
    }

    public class VariableSymbol : LSymbol {
        CIDENTIFIER m_element;

        // Linked AST Element ( Set during ASTGeneration phase )
        public CIDENTIFIER M_VariableIdentifier {
            get => m_element;
            set => m_element = value ?? throw new ArgumentNullException(nameof(value));
        }
        public VariableSymbol(string mName, LType mType) :
            base(mName, SymbolCategory.ST_VARIABLE, mType) { }
    }

    public class FunctionSymbol : LSymbol {
        private List<VariableSymbol> m_ParameterSymbols;
        CDeclarationFunction m_FunctionDeclaration;

        // Linked AST Element ( Set during ASTGeneration phase )
        public CDeclarationFunction M_FunctionDeclaration {
            get => m_FunctionDeclaration;
            set => m_FunctionDeclaration = value ?? throw new ArgumentNullException(nameof(value));
        }

        public CIDENTIFIER M_FunctionIdentifier {
            get => M_FunctionDeclaration.GetChild(CDeclarationFunction.FUNCTION_NAME, 0) as CIDENTIFIER;
        }

        public FunctionSymbol(string mName, LType mType,
            List<VariableSymbol> parameters) :
            base(mName, SymbolCategory.ST_FUNCTION, mType) {
            m_ParameterSymbols = new List<VariableSymbol>();
            foreach (VariableSymbol parameter in parameters) {
                AddParameter(parameter);
            }
        }
        private void AddParameter(VariableSymbol p) {
            m_ParameterSymbols.Add(p);
        }
        
    }

    public class TypenameSymbol : LSymbol {
        public TypenameSymbol(string mName, LType mType) :
            base(mName, SymbolCategory.ST_TYPENAME, mType) { }
    }
}