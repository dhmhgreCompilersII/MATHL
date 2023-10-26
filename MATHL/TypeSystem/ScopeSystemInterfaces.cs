using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATHL.TypeSystem {
    public interface IScope {
        public bool DefineSymbol(LSymbol symbol);
        public LSymbol SearchSymbol(string name);

        public Scope M_EnclosingScope { get; }
        public string M_ScopeName { get; }
    }
}
