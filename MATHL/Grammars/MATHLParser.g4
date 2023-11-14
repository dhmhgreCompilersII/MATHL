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

command : (SEMICOLON|NEWLINE)
		 | expression  {Console.WriteLine($"->{MMessage}");}
		 | block		  
		 ;

command_termination : (SEMICOLON|NEWLINE) ;
block : LB command (command_termination command)* command_termination* RB
	;
		

declaration : variable_declaration
			| function_declaration			
			;

type returns [LType tid] 
	 : INT	 { $tid = new IntegerType(); }
	 | FLOAT { $tid = new FloatingType(); }
	 | RANGE { $tid = new RangeType(); }
	 ;
	  

variable_declarator [LType t] returns [string id]
					: IDENTIFIER pds+=postfix_declarators* { $id = $IDENTIFIER.text;
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
																symtab.DefineSymbol(vs, SymbolType.ST_VARIABLE);
															
														   }	
					;

postfix_declarators : LBR RBR
					;

variable_declaration: type ( ','? ids+=variable_declarator[$type.tid] ( '=' expression )?)+ { 
		foreach ( var id in $ids ){
			VariableSymbol vs = new VariableSymbol($variable_declarator.id,$type.tid);
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
			| a=expression '=' b=expression {  if ( $a.ctx.GetChild(0) is ITerminalNode identifier ){
												LSymbol sym = symtab.SearchSymbol(identifier.Symbol.Text,SymbolType.ST_VARIABLE);		
												if ( sym != null ){
													sym.MValue = $b.result;
													MMessage = $"{sym.MName}={$b.result}";
												}
											 }
										}
			;

params : (expression (COMMA expression)+);  