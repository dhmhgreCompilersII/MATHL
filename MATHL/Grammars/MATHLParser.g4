parser grammar MATHLParser;

options { tokenVocab = MATHLLexer; }


/* 
Parser Rules
*/

@header {
  using MATHL.TypeSystem;
}
@parser::members {Scope symtab;
				  bool isFunction;
					}
@lexer::members { Scope symtab;
				   }

compile_unit[Scope symtab]																	// AST OK
@init { this.symtab = symtab; }
:  (command command_termination)* (command  command_termination?)?
;

command : expression  {Console.WriteLine($"->{MMessage}");}	#command_expression	 		  // AST OK
		| declaration										#command_declaration		  // AST OMMIT
		| command_block										#command_commandblock	      // AST OMMIT
		 ;

command_termination : (SEMICOLON|NEWLINE)+ ;													// AST OMMIT
command_block : LB command (command_termination command)* command_termination* RB			// AST OK
	;
		

declaration : variable_declaration	#declaration_variable		// AST OMMIT
			| function_declaration	#declaration_function		// AST OMMIT
			;

type returns [LType tid] 
	 : INT	 { $tid = new IntegerType(); }
	 | FLOAT { $tid = new FloatingType(); }
	 | RANGE { $tid = new RangeType(); }
	 ;
	  

variable_declarator [LType t] returns [string id]
					: IDENTIFIER pds+=postfix_declarators* ( '=' expression )? { $id = $IDENTIFIER.text;
															 // Declare symbol for new variable
															 VariableSymbol vs=null;
														     switch ( $t.MTypeId ){
																case TypeID.TID_INTEGER:															
																case TypeID.TID_FLOAT:
																    // Scalar variable symbol creation
																	vs = new VariableSymbol($IDENTIFIER.text,$t);
																break;
																case TypeID.TID_RANGE:
																	vs = new VariableSymbol($IDENTIFIER.text,$t);
																	if ($pds is List<Postfix_declaratorsContext> x){
																		if ( $t is RangeType r){
																			r.MDimensions = x.Count;
																		}
																	};
																break;
																default:
																break;
															 }
																symtab.DefineSymbol(vs, SymbolCategory.ST_VARIABLE);
															
														   }	
					;

postfix_declarators : LBR RBR
					;

variable_declaration: type (  ids+=variable_declarator[$type.tid] ','? )+ { 
		foreach ( var id in $ids ){
			VariableSymbol vs = new VariableSymbol($variable_declarator.id,$type.tid);
			symtab.DefineSymbol(vs, SymbolCategory.ST_VARIABLE);
		}
	}
					;

function_declaration : type IDENTIFIER '(' (variable_declaration (COMMA variable_declaration )*)? ')' { 
																FunctionSymbol vs = new FunctionSymbol($IDENTIFIER.text,$type.tid);																
																symtab.DefineSymbol(vs,SymbolCategory.ST_FUNCTION); 
																									  }
					;





expression returns [int result]
@init{
	IToken x = TokenStream.LT(1); /*****/
	if ( x.Type == MATHLParser.IDENTIFIER ){
		LSymbol identifierSymbol_ = symtab.SearchSymbol(x.Text,SymbolCategory.ST_FUNCTION);	
		isFunction = (identifierSymbol_==null)?false:true;

	}
	   				  
}

			:  NUMBER		{ $result = Int32.Parse($NUMBER.text);
								MMessage = $"={$result}";	
							} #expression_NUMBER
			|  {!isFunction}? IDENTIFIER 	{ LSymbol identifierSymbol = symtab.SearchSymbol($IDENTIFIER.text,SymbolCategory.ST_VARIABLE); 													  
							  if (identifierSymbol!= null ){
								$result = identifierSymbol.MValue;
								MMessage = $"={$result}";}
							  }						  

							  #expression_IDENTIFIER
			| range	 		{ MMessage = $"={$range.text}"; } #expression_range
			| {isFunction}? IDENTIFIER LP params? RP { symtab.SearchSymbol($IDENTIFIER.text,SymbolCategory.ST_FUNCTION); }  #expression_functioncall
			
										  
			| LP expression RP  { $result = $expression.result; } #expression_parenthesizedexpression
			| op=('+'|'-') expression   { switch ($op.type) {
											case MATHLLexer.PLUS:
												$result = $expression.result;
											break;
											case MATHLLexer.MINUS:
												$result = -$expression.result;
											break;
										}
										MMessage = $"{$result}";
									   } #expression_unaryprefixexpression
			| a=expression op=(<assoc=left>'*'|
							 <assoc=left>'/'|
							 <assoc=left>IDIV|
							 <assoc=left>'%') b=expression  {												
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
											} #expression_multiplicationdivision
			| a=expression op=(<assoc=left>'+'|<assoc=left>'-') b=expression  {
												switch ($op.type) {
													case MATHLLexer.PLUS:
														$result = $a.result + $b.result;
													break;
													case MATHLLexer.MINUS:
														$result = $a.result - $b.result;
													break;													
												}
												MMessage = $"{$result}";
											} #expression_additionsubtraction			
			| a=expression '=' b=expression  {  if ( $a.ctx.GetChild(0) is ITerminalNode identifier ){
												LSymbol sym = symtab.SearchSymbol(identifier.Symbol.Text,SymbolCategory.ST_VARIABLE);		
												if ( sym != null ){
													sym.MValue = $b.result;
													MMessage = $"{sym.MName}={$b.result}";
												}
											 }
										} #expression_equationassignment
			|  a=expression  b=expression  {
											$result = $a.result * $b.result;
											MMessage = $"{$result}";
										} #expression_multiplicationNoOperator
			;

params : expression (COMMA expression)*;  

range returns [CRange r] : 
	LBR a=expression? COLON b=expression? COLON c=expression? RBR {
																				int? start_tmp = $a.ctx==null ? null : Int32.Parse($a.text);
																				int? end_tmp = $b.ctx==null ? null : Int32.Parse($b.text);
																				int? step_tmp = $c.ctx==null ? null : Int32.Parse($c.text);
																				$r = new CRange(){
																									MStartIndex=start_tmp,
																									MEndIndex=end_tmp,
																									MStep=step_tmp 
																								};

																			}
	;