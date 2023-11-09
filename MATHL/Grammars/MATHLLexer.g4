lexer grammar MATHLLexer;


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
COMMA : ',';
IDENTIFIER : [a-zA-Z][a-zA-Z0-9_]* ;
NUMBER : '0'|[1-9][0-9]* ;
NEWLINE :'\r'?'\n' ; 
SPACE :[ \t] ->skip;

