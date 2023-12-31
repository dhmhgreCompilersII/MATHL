﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATHL.TypeSystem {
    public interface IScope {
        public bool DefineSymbol(LSymbol symbol, SymbolCategory symbolType);
        public LSymbol SearchSymbol(string name, SymbolCategory symbolType);

        public IScope M_EnclosingScope { get; }
        public string M_ScopeName { get; }
    }

    public interface IScopeSystem {

        IScope M_CurrentScope { get; }

        public IScope EnterScope(string scopename);
        public IScope ExitScope();

    }
}
