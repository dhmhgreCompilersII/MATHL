﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATHL.TypeSystem {
    public class Scope: IScope {

        private Dictionary<SymbolCategory, Dictionary<string, LSymbol>> m_symbolTable =
            new Dictionary<SymbolCategory, Dictionary<string, LSymbol>>();

        private string m_scopename;
        private IScope m_parentScope;

        public Scope(IScope parentScope,Action<Scope> init, string scopeName) {
            m_scopename = scopeName;
            m_parentScope = parentScope;
            init(this);
        }

        public void InitializeNamespace(SymbolCategory type) {
            if (!m_symbolTable.ContainsKey(type)) {
                m_symbolTable[type] = new Dictionary<string, LSymbol>();
            }
        }

        public bool DefineSymbol(LSymbol symbol, SymbolCategory symbolType) {
            if (!m_symbolTable[symbolType].ContainsKey(symbol.MName)) {
                m_symbolTable[symbolType][symbol.MName] = symbol;
                return true;
            }
            return false;

        }
        public LSymbol SearchSymbol(string name, SymbolCategory symbolType) {
            if (m_symbolTable[symbolType].ContainsKey(name)) {
                return m_symbolTable[symbolType][name];
            }
            else {
                return null;
            }
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
            if (M_ScopeName != null) {
                str.Append(m_scopename);
                str.AppendLine();
            }
            else {
                str.AppendLine();
            }

            str.Append("Parent Scope:");
            if (M_EnclosingScope != null) {
                str.Append(m_parentScope.M_ScopeName);
                str.AppendLine();
            }
            else {
                str.Append("NONE. Its the root scope");
                str.AppendLine();
            }

            foreach (KeyValuePair<SymbolCategory, Dictionary<string, LSymbol>> keyValuePair in m_symbolTable) {
                str.Append("Namespace :" + keyValuePair.Key);
                str.AppendLine();
                foreach (var lSymbol in m_symbolTable[keyValuePair.Key]) {
                    str.Append("\t"+lSymbol.Value);
                    str.AppendLine();
                }
            }
            return str.ToString();
        }
    }
}
