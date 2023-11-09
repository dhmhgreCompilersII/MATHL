grammar MATHL;

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

command : expression del=(';'|NEWLINE) {  
											switch ($del.type){
												case MATHLLexer.SEMICOLON:
												
												break;
												case MATHLLexer.NEWLINE:
													Console.WriteLine($"{$expression.result}");
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

variable_declaration: type IDENTIFIER ( '=' expression )? { 
		VariableSymbol vs = new VariableSymbol($IDENTIFIER.text,$type.tsym);
		symtab.DefineSymbol(vs, SymbolType.ST_VARIABLE);
	}
					;

function_declaration : type IDENTIFIER '(' (variable_declaration (',' variable_declaration )*)? ')'
					;


expression returns [int result] 
			:  NUMBER		{ $result = Int32.Parse($NUMBER.text);}
			| IDENTIFIER				{ LSymbol symbol = symtab.SearchSymbol($IDENTIFIER.text,SymbolType.ST_VARIABLE); 
										  $result = symbol.MValue;}
			| IDENTIFIER '(' params ')' { symtab.SearchSymbol($IDENTIFIER.text,SymbolType.ST_FUNCTION); }
			| expression '=' expression
			| '(' expression ')' { $result = $expression.result; }
			| op=('+'|'-') expression
			| expression op=(<assoc=left>'*'|<assoc=left>'/'|<assoc=left>IDIV|<assoc=left>'%') expression
			| expression op=(<assoc=left>'+'|<assoc=left>'-') expression			
			;

params : (expression (',' expression)+);  

/*
Lexer Rules
*/

INT : 'int';
FLOAT : 'float';
LP : '(';
RP : ')';
PLUS : '+';
MINUS : '-';
MULT : '*';
FDIV : '/';
IDIV : '//';
MOD : '%';
ASSIGN : '=';
SEMICOLON : ';' ;
IDENTIFIER : [a-zA-Z][a-zA-Z0-9_]* ;
NUMBER : '0'|[1-9][0-9]* ;
NEWLINE :'\r'?'\n' ; 
SPACE :[ \t] ->skip;

