using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATHL.TypeSystem {
    public class Scope: IScope {

        private Dictionary<SymbolType, Dictionary<string, LSymbol>> m_symbolTable =
            new Dictionary<SymbolType, Dictionary<string, LSymbol>>();

        public bool DefineSymbol(LSymbol symbol) {
            return false;
        }
        public LSymbol SearchSymbol(string name) {
            return null;
        }

        public Scope M_EnclosingScope {
            get { return null; }
        }

        public string M_ScopeName {
            get => null;
        }
    }
}
