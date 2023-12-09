parser grammar MATHLParser;

options { tokenVocab = MATHLLexer; }

/* 
Parser Rules
*/

@header {
  using MATHL.TypeSystem;
}
@parser::members {Scope symtab;
				  
				  bool isFunction; /* predicate */
					}
@lexer::members { Scope symtab;
				   }

compile_unit:  (command command_termination)* (command  command_termination?)?
;

command : expression			#command_expression	 		  // AST OK
		| declaration			#command_declaration		  // AST OMMIT
		| command_block			#command_commandblock	      // AST OMMIT		
		| RETURN expression		#command_return	
		;

command_termination : (SEMICOLON|NEWLINE)+ ;													// AST OMMIT
command_block : LB command_termination* command (command_termination+ command)* command_termination* RB			// AST OK
			 |  LB command_termination* RB
	;
		

declaration : variable_declaration	#declaration_variable		// AST OMMIT
			| function_declaration	#declaration_function		// AST OMMIT
			;

type 
	 : INT	 
	 | FLOAT 
	 | RANGE 
	 ;

variable_declaration : type variable_declarator ( ',' variable_declarator )* ;   

variable_declarator : IDENTIFIER pds+=postfix_declarators* ( '=' expression )? 	;

postfix_declarators : LBR RBR;

function_declaration : type IDENTIFIER '(' (variable_declaration (COMMA variable_declaration )*)? ')' command_block ;





expression 
			:  number														  #expression_NUMBER
			|  IDENTIFIER 												 	  #expression_IDENTIFIER
			| range	 														  #expression_range												  
			| LP expression RP												  #expression_parenthesizedexpression
			| op=('+'|'-') expression										  #expression_unaryprefixexpression
			| a=expression op=(<assoc=left>'*'|
							 <assoc=left>'/'|
							 <assoc=left>IDIV|
							 <assoc=left>'%') b=expression					  #expression_multiplicationdivision
			| a=expression op=(<assoc=left>'+'|<assoc=left>'-') b=expression  #expression_additionsubtraction			
			| a=expression '=' b=expression									  #expression_equationassignment
			| a=expression  b=expression									  #expression_context
			;

number :  INTEGER
		| FLOATING
		;

params : expression (COMMA expression)*
;  

range  : LBR a=expression? COLON b=expression? COLON c=expression? RBR 
	;