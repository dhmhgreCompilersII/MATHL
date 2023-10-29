using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;


public partial class MATHLLexer : Lexer {
    private Stack<string> m_InputFiles;

    public void SetInputFiles(string[] args) {
        m_InputFiles = new Stack<string>(args.Reverse());
    }

    public override IToken EmitEOF() {
        string currentFile;
        m_InputFiles.Pop();
        if (m_InputFiles.Count ==0) {
            return base.EmitEOF();
        }
        this.HitEOF = false;
        currentFile = m_InputFiles.Peek();
        StreamReader r = new StreamReader(currentFile);
        AntlrInputStream astream = new AntlrInputStream(r);
        SetInputStream(astream);
        return this.NextToken();
    }
}

