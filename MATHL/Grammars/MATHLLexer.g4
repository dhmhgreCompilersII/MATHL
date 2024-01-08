lexer grammar MATHLLexer;


/*
Lexer Rules
*/

fragment NATURAL: [1-9][0-9]*;
fragment SIGN: [+-];
fragment EXPONENT : ([EeXx]|'10^')SIGN? NATURAL;
fragment LOWERCASELETTER : [a-z];

INT : 'int';
FLOAT : 'float';
RANGE : 'range';
RETURN : 'return';
LP : '(';
RP : ')';
LB : '{';
RB : '}';
LBR : '[';
RBR : ']';
PLUS : '+';
MINUS : '-';
MULT : '*';
FDIV : '/';
IDIV : '//';
MOD : '%';
ASSIGN : '=';
SEMICOLON : ';' ;
COLON : ':';
COMMA : ',';
IDENTIFIER : [a-zA-Z][a-zA-Z0-9_]* ;
INTEGER : '0'|NATURAL;
FLOATING :  (NATURAL?'.'NATURAL EXPONENT?)|(NATURAL'.'NATURAL? EXPONENT?);


NEWLINE :'\r'?'\n' ; 
LINECOMMENTS :'//'.*'\n' ->skip;
BLOCKCOMMENTS:'/*' .*? '*/' ->skip;
SPACE :[ \t] ->skip;