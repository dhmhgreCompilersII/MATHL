using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using MATHL.TypeSystem;

public partial class MATHLParser : Parser {
    private StringBuilder message= new StringBuilder();

    public string MMessage {
        get { return message.ToString(); }
        set {
            message = new StringBuilder();
            message.Append(value);
        }
    }

    /// <summary>
    /// Checks if the next token is an IDENTIFIER referring to a function and set
    /// appropriately the isFunction predicate. If the token is not an IDENTIFIER
    /// invalidate the flag to alleviate side effects with other uses of IDENTIFIER
    /// </summary>
    public void SetPredicate_isFunction() {
        // Check if the following token is an IDENTIFIER of a function
        // and set the isFunction predicate
        IToken x = TokenStream.LT(1);
        if (x.Type == MATHLParser.IDENTIFIER) {
            if (symtab.ContainsKey(x.Text)) {
                if (symtab[x.Text] == SymbolCategory.ST_FUNCTION) {
                    isFunction = true;
                    return;
                }
            }
            isFunction = false;
        } else {
            // Need to invalidate the isFunction flag after passing a function IDENTIFIER symbol
            // to a next symbol that isn't an IDENTIFIER. 
            // Bug if ommitted: When expression is IDENTIFIER and isFunction is true (meaning that the previous
            // match is a function name ) it enables functioncall rule
            // {isFunction}? IDENTIFIER LP params? RP and disables IDENTIFIER rule thus the parser
            // is unable to match for example an assignment after a functioncall 
            isFunction = false;
        }
    }

}
