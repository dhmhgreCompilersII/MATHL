using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

public partial class MATHLParser : Parser {
    private StringBuilder message= new StringBuilder();

    public string MMessage {
        get { return message.ToString(); }
        set {
            message = new StringBuilder();
            message.Append(value);
        }
    }

}
