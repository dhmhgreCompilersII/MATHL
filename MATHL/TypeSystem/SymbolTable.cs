using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATHL.TypeSystem {
    public class SymbolTable {
        public bool DefineSymbol(LSymbol symbol) {
            return false;
        }
        public LSymbol SearchSymbol(string name) {
            return null;
        }

        public SymbolTable M_EnclosingScope {
            get { return null; }
        }

        public string M_ScopeName {
            get => null;
        }



    }
}
