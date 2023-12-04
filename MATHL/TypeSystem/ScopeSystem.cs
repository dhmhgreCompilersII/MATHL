using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATHL.TypeSystem {
    public class ScopeSystem :IScopeSystem{
        public IScope M_CurrentScope { get; }
        public IScope EnterScope(string scopename) {
            throw new NotImplementedException();
        }

        public IScope ExitScope() {
            throw new NotImplementedException();
        }
    }
}
