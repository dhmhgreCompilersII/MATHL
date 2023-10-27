using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATHL.TypeSystem {
    public interface IScope {
        public bool DefineSymbol(LSymbol symbol, SymbolType symbolType);
        public LSymbol SearchSymbol(string name, SymbolType symbolType);

        public IScope M_EnclosingScope { get; }
        public string M_ScopeName { get; }
    }
}
