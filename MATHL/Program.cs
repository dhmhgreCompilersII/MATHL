// See https://aka.ms/new-console-template for more information

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using MATHL;
using MATHL.TypeSystem;



MATHLCommandLineProcessor mathlProcessor = new MATHLCommandLineProcessor();
mathlProcessor.ParseCommandLineArguments(args);


