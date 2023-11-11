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
using static MATHL.MATHLInteractiveInterpreter;

namespace MATHL {

    public class MATHLCommandLineProcessor {
        List<string> m_inputFilesNames = new List<string>(); // Input Files
        // indicates the creation of an interactive parser
        private bool m_InteractiveParser = true;

        public void ParseCommandLineArguments(string[] args) {
            // Join arguments to single string to parse the contents using REs
            string input_ = string.Join(" ", args);

            // Regular Expression to analyze Command line identifying files and switches 
            var matches = Regex.Matches(input_, @"(\w[a-zA-Z0-9_]*\.\w[a-zA-Z0-9_]{1,3})|(-\p{L})");

            // Analyze input
            foreach (Match match in matches) {
                if (match.Groups[1].Length != 0) {
                    m_inputFilesNames.Add(match.Value);
                }
                if (match.Groups[2].Length != 0 && match.Value[1] == 'c') {
                    m_InteractiveParser = false;
                }
            }

            // If there are no files create an interactive parser
            if (m_inputFilesNames.Count == 0) {
                MATHLInteractiveInterpreter ii =
                    new MATHLInteractiveInterpreter(MATHLExecutionEnvironment.GetInstance());
                ii.Start();
            } else { // If there are files, create an interactive parser, if the -c switch is not used
                if (m_InteractiveParser) {
                    // Read and execute any give files from the command line
                    // before passing to interactive mode 
                    MATHLInterpreter fi =
                        new MATHLFileInterpreter(MATHLExecutionEnvironment.GetInstance(), m_inputFilesNames);
                    fi.Start();
                    MATHLInteractiveInterpreter ii =
                        new MATHLInteractiveInterpreter(MATHLExecutionEnvironment.GetInstance());
                    ii.Start();
                } else {
                    // If there are files, create an Non-interactive parser, if the -c switch is used
                    MATHLInterpreter fi =
                        new MATHLFileInterpreter(MATHLExecutionEnvironment.GetInstance(), m_inputFilesNames);
                    fi.Start();
                }
            }
        }
    }

    public abstract class MATHLInterpreter {
        protected MATHLExecutionEnvironment m_environment;
        public abstract void Start();

        protected MATHLInterpreter(MATHLExecutionEnvironment mEnvironment) {
            m_environment = mEnvironment;
        }

        protected void StartMATHLParser(string input) {
            AntlrInputStream antlrstream = new AntlrInputStream(input);
            MATHLLexer lexer = new MATHLLexer(antlrstream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            MATHLParser parser = new MATHLParser(tokens);
            IParseTree tree = parser.compile_unit(m_environment.MSymbolTable);
        }
    }

    public class MATHLInteractiveInterpreter : MATHLInterpreter {
        public MATHLInteractiveInterpreter(MATHLExecutionEnvironment environment) :
            base(environment) { }

        public override void Start() {
            MatchCollection enterBlockPredicate;
            MatchCollection leaveBlockPredicate;

            // Update nesting variable for each input line
            int UpdateNestingFromCurrentLine(string line, int i) {
                //INPUT
                // Match open curly braces per line
                enterBlockPredicate = Regex.Matches(line, "[^{]*{");
                i += enterBlockPredicate.Count;
                // Match closing curly braces per line
                leaveBlockPredicate = Regex.Matches(line, "[^}]*}");
                i -= leaveBlockPredicate.Count;
                return i;
            }

            string line = "";
            int nesting = 0;
            Console.Write("->");
            string lineBuffer = "";
            // Read lines until the end of file is given
            while ((lineBuffer = Console.ReadLine()) != null) {
                // INPUT
                // Append newline to recover it from Readline method
                line = lineBuffer + "\n"; // INPUT

                // update nesting level from the given line
                nesting = UpdateNestingFromCurrentLine(line, nesting);

                while (nesting > 0) {
                    lineBuffer = Console.ReadLine(); // INPUT
                    // Append newline to recover it from Readline method
                    line += lineBuffer + "\n"; // INPUT

                    // update nesting level from the given line
                    nesting = UpdateNestingFromCurrentLine(lineBuffer, nesting);
                }

                StringBuilder line_ = new StringBuilder(line); // INPUT

                StartMATHLParser(line_.ToString());
                Console.Write("->");
            }
        }
    }

    public class MATHLFileInterpreter : MATHLInterpreter {
        List<string> m_inputFilesNames; // Input Files

        public MATHLFileInterpreter(MATHLExecutionEnvironment environment,
            List<string> inputFiles) :base(environment) {
            m_inputFilesNames = inputFiles;
        }

        public override void Start() {
            // Get the first file
            foreach (string fileName in m_inputFilesNames) {
                Console.WriteLine($"Opening file {fileName}...");
                StreamReader reader = new StreamReader(fileName); // INPUT
                StartMATHLParser(reader.ReadToEnd());
            }
        }
    }

    // Singleton
    public class MATHLExecutionEnvironment {
        private Scope m_symbolTable; // SymbolTable
        private static MATHLExecutionEnvironment m_instance=null;

        public Scope MSymbolTable {
            get => m_symbolTable;
        }

        public static MATHLExecutionEnvironment GetInstance() {
            if (m_instance == null) {
                m_instance = new MATHLExecutionEnvironment();
            }
            return m_instance;
        }

        private MATHLExecutionEnvironment() {
            // Initialize build in types 
            InitializeProcessor();
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
