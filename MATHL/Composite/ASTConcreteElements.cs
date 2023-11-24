using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATHL.Composite {


    public enum NodeType {
        NT_NA = -1, NT_COMPILEUNIT, NT_COMMAND_EXPRESSION, NT_COMMAND_DECLARATION, NT_COMMAND_COMMANDBLOCK,
        NT_ADDITION, NT_SUBTRACTION, NT_MULTIPLICATION, NT_DIVISION,
        NT_IDENTIFIER, NT_NUMBER, NT_ASSIGNMENT
    }

    public class CCompileUnit : ASTComposite {
        public const int COMMANDS = 0;
        public readonly string[] mc_contextNames = { "Commands" };

        public CCompileUnit() :
            base(1, (int)NodeType.NT_COMPILEUNIT) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitCompileUnit(this, info);
        }
    }

    public class CCommand_Expression : ASTComposite {
        public const int COMMAND = 0;
        public readonly string[] mc_contextNames = { "Command_Expression" };

        public CCommand_Expression() :
            base(1, (int)NodeType.NT_COMMAND_EXPRESSION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitCommand_Expression(this, info);
        }
    }

    public class CCommand_Declaration : ASTComposite {
        public const int COMMAND = 0;
        public readonly string[] mc_contextNames = { "Command_Declaration" };

        public CCommand_Declaration() :
            base(1, (int)NodeType.NT_COMMAND_DECLARATION) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitCommand_Declaration(this, info);
        }
    }

    public class CCommand_CommandBlock : ASTComposite {
        public const int COMMAND = 0;
        public readonly string[] mc_contextNames = { "Command_CommandBlock" };

        public CCommand_CommandBlock() :
            base(1, (int)NodeType.NT_COMMAND_COMMANDBLOCK) {
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            MATHLBaseVisitor<Return, Params> visitor = v as MATHLBaseVisitor<Return, Params>;
            return visitor.VisitCommand_CommandBlock(this, info);
        }
    }

}
