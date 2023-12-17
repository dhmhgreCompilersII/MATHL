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

postfix_declarators : LBR INTEGER RBR;

function_declaration : type IDENTIFIER '(' (variable_declaration (COMMA variable_declaration )*)? ')' command_block ;

expression :  a=expression  b=expression1			  #expression_context 
             | expression1							  #expression_expression1
			 ;

expression1: a=expression1 '=' b=expression2		 #expression_equationassignment
		     | expression2						     #expression1_expression2
			 ;

expression2 : a=expression2 op=('+'|'-') b=expression3  #expression_additionsubtraction
			| expression3								#expression2_expression3
			;

expression3: a=expression3 op=(<assoc=left>'*'|
							 <assoc=left>'/'|
							 <assoc=left>IDIV|
							 <assoc=left>'%') b=expression4	  #expression_multiplicationdivision
			| expression4									#expression3_expression4
			;

expression4: op=('+'|'-') expression	 #expression_unaryprefixexpression 
			| expression5				#expression4_expression5
			;

expression5: number														  #expression5_NUMBER
			| IDENTIFIER 												 	  #expression5_IDENTIFIER
			| range	 														  #expression5_range												  
			| LP expression RP												  #expression5_parenthesizedexpression
			;

number :  INTEGER		#numberINTEGER
		| FLOATING		#numberFLOAT
		;

params : expression (COMMA expression)*
;  

range  : LBR a=expression? COLON b=expression? COLON c=expression? RBR 
	;