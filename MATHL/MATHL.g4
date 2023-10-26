grammar MATHL;

/* 
Parser Rules
*/

compile_unit: ((command|declaration) ';')+
;

command : expression
		;

declaration : ;

expression :  NUMBER
			| IDENTIFIER
			| IDENTIFIER '(' params ')'
			| expression '=' expression
			| '(' expression ')'
			| op=('+'|'-') expression
			| expression op=('*'|'/'|IDIV|'%') expression
			| expression op=('+'|'-') expression			
			;

params : (expression (',' expression)+);  

/*
Lexer Rules
*/


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
NEWLINE :[ \r\n\t] ->skip;

