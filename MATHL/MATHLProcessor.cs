﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using MATHL.TypeSystem;

namespace MATHL {
    public class MATHLProcessor {
        private List<string> m_inputFilesNames = new List<string>();
        private StreamReader m_currentInputStream;

        MATHLParser m_parser = null;
        private MATHLLexer m_lexer = null;
        private CommonTokenStream m_tokens = null;
        private Scope m_symbolTable;

        public static void Start(string[] args) {
            // Analyze the input arguments and initialize
            // the parser accordingly

            // the absence of a switch indicates input file 
            // switch -i create an interactive parser
            
            // separate the input arguments into an array
            // determine switches from the - prefixing the letter

        }

        protected MATHLProcessor() {

            InitializeProcessor();

            AntlrInputStream antlrstream = new AntlrInputStream(Console.In);
            MATHLLexer lexer = new MATHLLexer(antlrstream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            MATHLParser parser = new MATHLParser(tokens);

            IParseTree tree = parser.compile_unit(m_symbolTable);
            Console.WriteLine(m_symbolTable.ToString());
            Console.WriteLine(tree.ToStringTree());
        }

        protected MATHLProcessor(string[] args) {
            int inputFiles = args.Length;
            InitializeProcessor();

            // Get the first file
            Console.WriteLine($"Opening file {args[0]}...");
            StreamReader reader = new StreamReader(args[0]);
            AntlrInputStream antlrstream = new AntlrInputStream(reader);
            m_lexer = new MATHLLexer(antlrstream);
            m_lexer.SetInputFiles(args);        // Send the input files to lexer
            m_tokens = new CommonTokenStream(m_lexer);
            m_parser = new MATHLParser(m_tokens);
            
            IParseTree tree = m_parser.compile_unit(m_symbolTable);

            Console.WriteLine(m_symbolTable.ToString());
            Console.WriteLine(tree.ToStringTree());
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
