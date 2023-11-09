parser grammar MATHLParser;

options { tokenVocab = MATHLLexer; }


/* 
Parser Rules
*/

@header {
  using MATHL.TypeSystem;
}
@parser::members {Scope symtab;}
@lexer::members { Scope symtab;}

compile_unit[Scope symtab]
@init { this.symtab = symtab; }
: (command|declaration)+
;

command : expression del=(SEMICOLON|NEWLINE) {  
											switch ($del.type){
												case MATHLLexer.SEMICOLON:
												
												break;
												case MATHLLexer.NEWLINE:													
													Console.WriteLine($"->{MMessage}");
												break;
										    }
										
										
										};
		

declaration : variable_declaration
			| function_declaration
			;

type returns [LType tsym] 
	 : INT	 { $tsym = symtab.SearchSymbol("int", SymbolType.ST_TYPENAME).MType; }
	 | FLOAT { $tsym = symtab.SearchSymbol("float", SymbolType.ST_TYPENAME).MType; }
	 ;

variable_declaration: type ( ','? ids+=IDENTIFIER ( '=' expression )?)+ { 
		foreach ( var id in $ids ){
			VariableSymbol vs = new VariableSymbol(id.Text,$type.tsym);
			symtab.DefineSymbol(vs, SymbolType.ST_VARIABLE);
		}
	}
					;

function_declaration : type IDENTIFIER '(' (variable_declaration (COMMA variable_declaration )*)? ')'
					;


expression returns [int result] 
			:  NUMBER		{ $result = Int32.Parse($NUMBER.text);
								MMessage = $"={$result}";	
							}
			| IDENTIFIER	{ LSymbol symbol = symtab.SearchSymbol($IDENTIFIER.text,SymbolType.ST_VARIABLE); 
										  $result = symbol.MValue;
										  MMessage = $"={$result}";}
			| IDENTIFIER '(' params ')' { symtab.SearchSymbol($IDENTIFIER.text,SymbolType.ST_FUNCTION); }
			| a=expression '=' b=expression {  if ( $a.ctx.GetChild(0) is ITerminalNode identifier ){
												LSymbol sym = symtab.SearchSymbol(identifier.Symbol.Text,SymbolType.ST_VARIABLE);		
												if ( sym != null ){
													sym.MValue = $b.result;
													MMessage = $"{sym.MName}={$b.result}";
												}
											 }
										}
			| '(' expression ')' { $result = $expression.result; }
			| op=('+'|'-') expression { switch ($op.type) {
											case MATHLLexer.PLUS:
												$result = $expression.result;
											break;
											case MATHLLexer.MINUS:
												$result = -$expression.result;
											break;
										}
										MMessage = $"{$result}";
									   }
			| a=expression op=(<assoc=left>'*'|
							 <assoc=left>'/'|
							 <assoc=left>IDIV|
							 <assoc=left>'%') b=expression {
												switch ($op.type) {
													case MATHLLexer.MULT:
														$result = $a.result * $b.result;
													break;
													case MATHLLexer.FDIV:
														$result = $a.result / $b.result;
													break;
													case MATHLLexer.IDIV:
														$result = $a.result / $b.result;
													break;
													case MATHLLexer.MOD:
														$result = $a.result % $b.result;
													break;
												}
												MMessage = $"{$result}";
											}
			| a=expression op=(<assoc=left>'+'|<assoc=left>'-') b=expression {
												switch ($op.type) {
													case MATHLLexer.PLUS:
														$result = $a.result + $b.result;
													break;
													case MATHLLexer.MINUS:
														$result = $a.result - $b.result;
													break;													
												}
												MMessage = $"{$result}";
											}			

			;

params : (expression (COMMA expression)+);  