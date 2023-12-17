using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MATHL.TypeSystem {
    public class ScopeSystem {
        public static readonly string m_globalScopeName = "global";
        private Dictionary<string, Scope> m_scopes = new Dictionary<string, Scope>();
        private Dictionary<object, Scope> m_scopesAssociation = new Dictionary<object, Scope>();
        Stack<Scope> m_scopesStack = new Stack<Scope>();
        private Scope m_currentScope = null;
        private Scope m_globalScope=null;
        public Scope M_CurrentScope => m_currentScope;
        public Scope M_GlobalScope => m_globalScope;
        public static string M_GlobalScopeName => m_globalScopeName;

        public ScopeSystem() {
            InitializeTypes();
        }

        public void AssociateSyntaxObjectWithScope(object syntaxObject, Scope scope) {
            if (!m_scopesAssociation.ContainsKey(syntaxObject)) {
                m_scopesAssociation[syntaxObject] = scope;
            }
            else {
                throw new Exception();
            }
        }

        public Scope GetScopeAssociatedWithSyntaxObject(object syntaxObject) {
            return m_scopesAssociation[syntaxObject];
        }

        public void InitializeTypes() {
            // Create global scope 
            m_globalScope = new Scope(null,
                scope => {
                        scope.InitializeNamespace(SymbolCategory.ST_TYPENAME);
                        scope.InitializeNamespace(SymbolCategory.ST_VARIABLE);
                        scope.InitializeNamespace(SymbolCategory.ST_FUNCTION);
                    
                },
                m_globalScopeName);
            m_scopes[M_GlobalScopeName] = m_globalScope;

            // Initialize global scope typanames namespace
            M_GlobalScope.DefineSymbol(new TypenameSymbol(IntegerType.mc_typename, IntegerType.Instance),SymbolCategory.ST_TYPENAME);
            M_GlobalScope.DefineSymbol(new TypenameSymbol(FloatingType.mc_typename,FloatingType.Instance), SymbolCategory.ST_TYPENAME);
        }
        
        /// <summary>
        /// Creates and enters a scope ( symboltable ). The first scope is considered
        /// global with 3 namespaces (typenames, variables and functions ).
        /// The others have only variable namespaces. 
        /// </summary>
        /// <param name="scopename">if global scopename must be null and non-null otherwise</param>
        /// <returns></returns>
        public Scope EnterScope(string scopename) {
            // Check if the scope already exists and if not create it
            // Global scope is initialized upon construction and it is 
            // the only existing scope
            if (scopename==null || !m_scopes.ContainsKey(scopename)) {
                m_currentScope = new Scope(m_currentScope,
                    scope => {
                        scope.InitializeNamespace(SymbolCategory.ST_VARIABLE);
                    },
                    scopename);
                // Store it to m_scopes
                m_scopes[m_currentScope.M_ScopeName] = m_currentScope;
            }
            else {
                // If the scope already exists take it from m_scopes
                m_currentScope = m_scopes[scopename];
            }
            
            // Push scope
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
                reportFile.WriteLine($"------------------------------------");
                reportFile.WriteLine($"---- SCOPE {keyValuePair.Key} ------");
                reportFile.WriteLine(keyValuePair.Value);
                reportFile.WriteLine();
            }
            reportFile.Close();
        }
    }
}
