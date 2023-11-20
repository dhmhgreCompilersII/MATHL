using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATHL.Composite {


    public enum NodeType {
        NT_NA = -1, NT_COMPILEUNIT, NT_ADDITION, NT_SUBTRACTION,
        NT_MULTIPLICATION, NT_DIVISION, NT_IDENTIFIER, NT_NUMBER,
        NT_ASSIGNMENT
    }

    public class CompileUnit : ASTComposite {
        public const int EXPRESSIONS = 0;
        public readonly string[] mc_contextNames = { "Expressions" };

        public CompileUnit() :
            base(1, (int)NodeType.NT_COMPILEUNIT) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitCompileUnit(this, info);
        }
    }
}
