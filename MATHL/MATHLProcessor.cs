using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using MATHL.ASTVisitors;
using MATHL.STVisitors;
using MATHL.TypeSystem;
using MATHL.Visitors;
using static MATHL.MATHLInteractiveInterpreter;

namespace MATHL
{

    public class MATHLCommandLineProcessor {
        private MATHLExecutionEnvironment m_environment;
        List<string> m_inputFilesNames = new List<string>(); // Input Files
        // indicates the creation of an interactive parser
        private bool m_InteractiveParser = true;

        public MATHLCommandLineProcessor() {
            m_environment = MATHLExecutionEnvironment.GetInstance();
        }

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
                    new MATHLInteractiveInterpreter();
                ii.Start();
            } else { // If there are files, create an interactive parser, if the -c switch is not used
                if (m_InteractiveParser) {
                    // Read and execute any give files from the command line
                    // before passing to interactive mode 
                    MATHLInterpreter fi =
                        new MATHLFileInterpreter(m_inputFilesNames);
                    fi.Start();
                    MATHLInteractiveInterpreter ii =
                        new MATHLInteractiveInterpreter();
                    ii.Start();
                } else {
                    // If there are files, create an Non-interactive parser, if the -c switch is used
                    MATHLInterpreter fi =
                        new MATHLFileInterpreter( m_inputFilesNames);
                    fi.Start();
                }
            }
        }
    }

    public abstract class MATHLInterpreter {
        protected MATHLExecutionEnvironment m_environment;
        public abstract void Start();

        protected MATHLInterpreter() {
            m_environment = MATHLExecutionEnvironment.Instance;
        }

        protected void StartMATHLParser(string input) {
            AntlrInputStream antlrstream = new AntlrInputStream(input);
            MATHLLexer lexer = new MATHLLexer(antlrstream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            MATHLParser parser = new MATHLParser(tokens);
            IParseTree tree = parser.compile_unit();
            
            // ST Generated
            
            SyntaxTreePrinter stPrinter = new SyntaxTreePrinter("St");
            stPrinter.Visit(tree);
            
            DeclarationProcessor declarationProcessor = new DeclarationProcessor(m_environment.M_ScopeSystem);
            declarationProcessor.Visit(tree);
            
            ASTGeneration astgen = new ASTGeneration(m_environment.M_ScopeSystem);
            var asttree = astgen.Visit(tree);
            
            // AST Generated

            ASTPrinter astprinter = new ASTPrinter("AST.dot");
            astprinter.Visit(asttree);

            InitializationsProcessor initializationsProcessor =
                new InitializationsProcessor(m_environment.M_ScopeSystem);
            initializationsProcessor.Visit(asttree);

            EvaluationProcessor evaluator = new EvaluationProcessor();
            evaluator.Visit(asttree);


            // Reports
            m_environment.M_ScopeSystem.Report("SymbolTable.txt");

        }
    }

    public class MATHLInteractiveInterpreter : MATHLInterpreter {
        public const string m_terminalCommand = "COMMAND";
        public MATHLInteractiveInterpreter() { }

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
                // Record command to history
                m_environment.RecordInput(line_.ToString(),m_terminalCommand);

                // Call MATHL parser
                StartMATHLParser(line_.ToString());
                Console.Write("->");
            }
        }
    }

    public class MATHLFileInterpreter : MATHLInterpreter {
        List<string> m_inputFilesNames; // Input Files

        public MATHLFileInterpreter(List<string> inputFiles) {
            m_inputFilesNames = inputFiles;
        }

        public override void Start() {
            StringBuilder temp = new StringBuilder();
            // Get the first file
            foreach (string fileName in m_inputFilesNames) {
                Console.WriteLine($"Opening file {fileName}...");
                StreamReader reader = new StreamReader(fileName); // INPUT
                temp.Append(reader.ReadToEnd());

                // Record command to history
                m_environment.RecordInput(temp.ToString(), fileName);

                /*reader.DiscardBufferedData();
                reader.BaseStream.Position = 0;*/

                StartMATHLParser(temp.ToString());
            }
        }
    }

    // Singleton
    public class MATHLExecutionEnvironment {
        // SymbolTable
        private ScopeSystem m_scopeSystem;

        public ScopeSystem M_ScopeSystem => m_scopeSystem;

        // History of given commands
        private Dictionary<string, StringBuilder> m_history = new Dictionary<string, StringBuilder>();

        // Singleton instance
        private static MATHLExecutionEnvironment m_instance = null;

        public static MATHLExecutionEnvironment Instance => m_instance;

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

        public void RecordInput(string input, string streamname) {
            if (!m_history.ContainsKey(streamname)) {
                m_history[streamname] = new StringBuilder();
            }
            m_history[streamname].Append(input);
        }

        public string GetStreamCommandRecord(string streamname) {
            
            if (m_history.ContainsKey(streamname)) {
                return m_history[streamname].ToString();
            }

            throw new FileNotFoundException($"The given stream ({streamname}) does not exist");
        }

        void InitializeProcessor() {
            m_scopeSystem = new ScopeSystem();
        }
    }
}
