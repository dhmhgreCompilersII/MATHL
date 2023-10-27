using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATHL.TypeSystem {
    public class Scope: IScope {

        private Dictionary<SymbolType, Dictionary<string, LSymbol>> m_symbolTable =
            new Dictionary<SymbolType, Dictionary<string, LSymbol>>();

        private string m_scopename;
        private IScope m_parentScope;

        public Scope(IScope parentScope, string scopeName) {
            m_scopename = scopeName;
            m_parentScope = parentScope;
        }

        public bool DefineSymbol(LSymbol symbol, SymbolType symbolType) {
            return false;
        }
        public LSymbol SearchSymbol(string name, SymbolType symbolType) {
            return null;
        }

        public IScope M_EnclosingScope {
            get { return m_parentScope; }
        }

        public string M_ScopeName {
            get => m_scopename;
        }

        public override string ToString() {
            StringBuilder str = new StringBuilder();
            str.Append("Name:");
            if (m_scopename != null) {
                str.Append(m_scopename);
                str.AppendLine();
            }
            str.Append("Parent Scope:");
            if (M_EnclosingScope != null) {
                str.Append(m_parentScope.M_ScopeName);
                str.AppendLine();
            }
            str.Append(m_symbolTable.ToString());
            return base.ToString();
        }
    }
}
