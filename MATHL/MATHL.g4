grammar MATHL;

/* 
Parser Rules
*/

compile_unit: ((command|declaration) ';')+
;

command : expression
		;

declaration : variable_declaration
			| function_declaration
			;

type : INT
	 | FLOAT
	 ;

variable_declaration: type IDENTIFIER ( '=' expression )?
					;

function_declaration : type IDENTIFIER '(' (variable_declaration (',' variable_declaration )*)? ')'
					;


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
NEWLINE :[ \r\n\t] ->skip;

