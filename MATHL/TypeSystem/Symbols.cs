
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
        private int ivalue;
        [FieldOffset(0)]
        private float fvalue;
        [FieldOffset(4)]
        TypeID m_type;

        public int Ivalue {
            get => ivalue;
            set => ivalue = value;
        }

        public float Fvalue {
            get => fvalue;
            set => fvalue = value;
        }

        public TypeID MType {
            get => m_type;
            set => m_type = value;
        }

        public override string ToString() {
            switch (MType) {
                case TypeID.TID_INTEGER:
                    return Ivalue.ToString();
                case TypeID.TID_FLOAT:
                    return Fvalue.ToString();
                default:
                    return "";
            }
        }
    }

    public abstract class LSymbol {
        string m_name;
        private LType m_type;
        private LValue m_value;
        private SymbolCategory m_symbolCategory;

        public LSymbol(string mName, SymbolCategory mSymbolType, LType mType) {
            m_name = mName;
            m_type = mType;
            m_symbolCategory = mSymbolType;
        }

        public string MName => m_name;

        public LType MType => m_type;

        public LValue MValue {
            get => m_value;
            set => m_value = value;
        }


        public SymbolCategory MSymbolCategory => m_symbolCategory;

        public override string ToString() {
            string symbol;
            if (MSymbolCategory == SymbolCategory.ST_VARIABLE) {
                switch (m_value.MType) {
                    case TypeID.TID_INTEGER:
                        return MName + ":" + m_type +$"= {m_value.Ivalue}";
                        break;
                    case TypeID.TID_FLOAT:
                        return MName + ":" + m_type + $"= {m_value.Fvalue}";
                        break;
                    default:
                        return MName + ":" + m_type;
                }
            }
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