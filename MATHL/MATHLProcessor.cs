using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using MATHL.TypeSystem;

namespace MATHL {
    public class MATHLProcessor {


        List<string> m_inputFilesNames = new List<string>(); // Input Files
        MATHLParser m_parser = null; //Parser
        private MATHLLexer m_lexer = null;  // Lexer
        private CommonTokenStream m_tokens = null; // TokenStream
        private Scope m_symbolTable;    // SymbolTable

        // indicates the creation of an interactive parser
        private bool m_InteractiveParser = true;

        protected void CreateInteractiveParser() {

            InitializeProcessor();

            if (m_inputFilesNames.Count != 0) {
                CreateNonInteractiveParser();
            }

            string line;
            while ( (line =Console.ReadLine()) != null) {
                StringBuilder line_ = new StringBuilder(line);
                line_.AppendLine();
                AntlrInputStream antlrstream = new AntlrInputStream(line_.ToString());
                MATHLLexer lexer = new MATHLLexer(antlrstream);
                CommonTokenStream tokens = new CommonTokenStream(lexer);
                MATHLParser parser = new MATHLParser(tokens);
                IParseTree tree = parser.compile_unit(m_symbolTable);
                //Console.WriteLine(tree.ToStringTree());
            }
            Console.WriteLine(m_symbolTable.ToString());
        }

        protected void CreateNonInteractiveParser() {
            InitializeProcessor();

            // Get the first file
            Console.WriteLine($"Opening file {m_inputFilesNames[0]}...");
            StreamReader reader = new StreamReader(m_inputFilesNames[0]);
            AntlrInputStream antlrstream = new AntlrInputStream(reader);
            m_lexer = new MATHLLexer(antlrstream);
            m_lexer.SetInputFiles(m_inputFilesNames.ToArray());        // Send the input files to lexer
            m_tokens = new CommonTokenStream(m_lexer);
            m_parser = new MATHLParser(m_tokens);

            IParseTree tree = m_parser.compile_unit(m_symbolTable);

            Console.WriteLine(m_symbolTable.ToString());
            Console.WriteLine(tree.ToStringTree());

        }

        public MATHLProcessor Start(string[] args) {
            string input_ = string.Join(" ", args);

            
            
            // Regular Expression to analyze Command line 
            var matches = Regex.Matches(input_, @"(\w[a-zA-Z0-9_]*\.\w[a-zA-Z0-9_]{1,3})|(-\p{L})");

            // Analyze input
            foreach (Match match in matches) {
                if (match.Groups[1].Length != 0) {
                    m_inputFilesNames.Add(match.Value);
                }
                if (match.Groups[2].Length != 0 && match.Value[1]=='c') {
                    m_InteractiveParser = false;
                }
            }

            // If there are no files create an interactive parser
            if (m_inputFilesNames.Count == 0) {
                CreateInteractiveParser();
            }
            else { // If there are files create an interactive parser if the -c switch is not used
                if (m_InteractiveParser) {
                    CreateInteractiveParser();
                }
                else {
                    CreateNonInteractiveParser();
                }
            }

            //Regex.
            // Analyze the input arguments and initialize
            // the parser accordingly

            // the absence of a switch indicates input file 
            // switch -i create an interactive parser

            // separate the input arguments into an array
            // determine switches from the - prefixing the letter

            return null;
        }
        
        void InitializeProcessor() {
            m_symbolTable = new Scope(null, scope => {
                scope.InitializeNamespace(SymbolType.ST_TYPENAME);
                scope.InitializeNamespace(SymbolType.ST_VARIABLE);
                scope.InitializeNamespace(SymbolType.ST_FUNCTION);
                scope.DefineSymbol(new TypenameSymbol("int", new IntegerType()), SymbolType.ST_TYPENAME);
                scope.DefineSymbol(new TypenameSymbol("float", new FloatingType()), SymbolType.ST_TYPENAME);
            },
               "global");
        }
    }
}
