// See https://aka.ms/new-console-template for more information

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using MATHL;
using MATHL.TypeSystem;



//MATHLProcessor mathlProcessor = new MATHLProcessor();
MATHLProcessor mathlProcessor = new MATHLProcessor(args);

/*Scope symtab = new Scope(null, scope => {
        scope.InitializeNamespace(SymbolType.ST_TYPENAME);
        scope.InitializeNamespace(SymbolType.ST_VARIABLE);
        scope.InitializeNamespace(SymbolType.ST_FUNCTION);
        scope.DefineSymbol(new TypenameSymbol("int", new IntegerType()), SymbolType.ST_TYPENAME);
        scope.DefineSymbol(new TypenameSymbol("float", new FloatingType()), SymbolType.ST_TYPENAME);
    }, 
    "global");

StreamReader aReader = new StreamReader(args[0]);
AntlrInputStream antlrstream = new AntlrInputStream(aReader);
MATHLLexer lexer  = new MATHLLexer(antlrstream);
CommonTokenStream tokens = new CommonTokenStream(lexer);
MATHLParser parser = new MATHLParser(tokens);

IParseTree tree= parser.compile_unit(symtab);
Console.WriteLine(symtab.ToString());
Console.WriteLine(tree.ToStringTree());*/