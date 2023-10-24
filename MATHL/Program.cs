// See https://aka.ms/new-console-template for more information

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

Console.WriteLine("Hello, World!");

StreamReader aReader = new StreamReader(args[0]);
AntlrInputStream antlrstream = new AntlrInputStream(aReader);
MATHLLexer lexer  = new MATHLLexer(antlrstream);
CommonTokenStream tokens = new CommonTokenStream(lexer);
MATHLParser parser = new MATHLParser(tokens);
IParseTree tree= parser.compile_unit();
Console.WriteLine(tree.ToStringTree());