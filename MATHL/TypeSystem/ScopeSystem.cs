using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATHL.TypeSystem {
    public class ScopeSystem {
        private Dictionary<string, Scope> m_scopes = new Dictionary<string, Scope>();
        Stack<Scope> m_scopesStack = new Stack<Scope>();
        private Scope m_currentScope = null;

        public Scope M_CurrentScope => m_currentScope;

        /// <summary>
        /// Creates and enters a scope ( symboltable ). The first scope is considered
        /// global with 3 namespaces (typenames, variables and functions ).
        /// The others have only variable namespaces. 
        /// </summary>
        /// <param name="scopename">if global scopename must be null and non-null otherwise</param>
        /// <returns></returns>
        public Scope EnterScope(string scopename = null) {
            string scopeName = m_currentScope == null ? "global" : scopename;
            m_currentScope = new Scope(m_currentScope,
                scope => {
                    if (scopename == null) {
                        scope.InitializeNamespace(SymbolCategory.ST_TYPENAME);
                        scope.InitializeNamespace(SymbolCategory.ST_VARIABLE);
                        scope.InitializeNamespace(SymbolCategory.ST_FUNCTION);
                    } else {
                        scope.InitializeNamespace(SymbolCategory.ST_VARIABLE);
                    }
                },
                scopeName);
            m_scopes[scopeName] = m_currentScope;
            m_scopesStack.Push(m_currentScope);
            return m_currentScope;
        }
        public Scope ExitScope() {
            m_currentScope = m_currentScope.M_EnclosingScope;
            m_scopesStack.Pop();
            return m_currentScope;
        }

        public bool DefineSymbol(LSymbol symbol, SymbolCategory symbolType) {
            return m_currentScope.DefineSymbol(symbol, symbolType);
        }

        public LSymbol SearchSymbol(string name, SymbolCategory symbolType) {
            return m_currentScope.SearchSymbol(name, symbolType);
        }

        public void Report(string filename) {
            StreamWriter reportFile = new StreamWriter(filename);
            reportFile.WriteLine("---- REPORTING SCOPESYSTEM CONTENTS -----\n");

            foreach (KeyValuePair<string, Scope> keyValuePair in m_scopes) {
                reportFile.WriteLine($"---- SCOPE {keyValuePair.Key} ------");
                reportFile.WriteLine(keyValuePair.Value);
                reportFile.WriteLine();
            }
            reportFile.Close();
        }
    }
}
