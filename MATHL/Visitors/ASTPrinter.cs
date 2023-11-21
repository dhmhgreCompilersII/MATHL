using MATHL.Composite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATHL.Visitors {
    internal class ASTPrinter : MATHLBaseVisitor<int, ASTElement> {
        StreamWriter m_writer;
        private string m_outputFilename;
        private int m_clusterSerial;
        private int ms_clusterSerialCounter;

        public ASTPrinter(string outputFilename) {
            m_outputFilename = outputFilename;
            m_writer = new StreamWriter(m_outputFilename);
        }

        private void CreateContextSubgraph(ASTComposite node, int contextindex, string contextName) {
            
            m_writer.WriteLine($"\tsubgraph cluster{m_clusterSerial++} {{");
            m_writer.WriteLine("\t\tnode [style=filled,color=white];");
            m_writer.WriteLine("\t\tnode [style=filled,color=lightgrey];");
            bool first = true;
            if (node.GetNumberOfContextNodes(contextindex) != 0) {
                foreach (ASTElement child in node.GetContextChildren(contextindex)) {
                    if (first) {
                        m_writer.Write("\t\t");
                    }

                    first = false;
                    m_writer.Write("\"" + child.MNodeName + "\"");
                }

                m_writer.WriteLine($";");
            }
            m_writer.WriteLine($"\t\tlabel = \"{contextName}\";");
            m_writer.WriteLine($"\t}}");
        }

        public override int VisitCompileUnit(CCompileUnit node, params ASTElement[] args) {
            CCompileUnit n = node as CCompileUnit;
            if (n == null) {
                throw new InvalidCastException("Expected CompileUnit type");
            }

            m_writer.WriteLine("digraph G{");

            CreateContextSubgraph(n, CCompileUnit.COMMANDS,
                n.mc_contextNames[CCompileUnit.COMMANDS]);

            base.VisitCompileUnit(node, n);

            m_writer.WriteLine("}");
            m_writer.Close();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "dot.exe";
            startInfo.Arguments = " -Tgif "+Path.GetFileName(m_outputFilename) +
                                  " -o "+ Path.GetFileNameWithoutExtension(m_outputFilename) +".gif";

            Process proc = Process.Start(startInfo);

            return 0;
        }
    }
}
