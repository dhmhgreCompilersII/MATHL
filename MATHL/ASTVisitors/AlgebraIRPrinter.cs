using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MATHL.Composite;

namespace MATHL.ASTVisitors {

    public class AlgebraIRPrinter_Params {
        private string m_parent;

        public string MParent {
            get => m_parent;
            set => m_parent = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    public class AlgebraIRPrinter : AlgebraBaseVisitor<int, AlgebraIRPrinter_Params> {
        private StreamWriter irFile;
        public static int m_expressionSerial = 0;
        

        public override int VisitSymbol(ALESymbol node, params AlgebraIRPrinter_Params[] args) {
            return base.VisitSymbol(node, args);
        }

        public override int VisitNumber(ALENumber node, params AlgebraIRPrinter_Params[] args) {
            return base.VisitNumber(node, args);
        }

        public override int VisitExpressionRoot(ALERoot node, params AlgebraIRPrinter_Params[] args) {
            string outputFile;

            // 1. Create output file 
            outputFile = $"expression{m_expressionSerial++}.dot";
            irFile = new StreamWriter(outputFile);

            // 2. Create prologue
            irFile.WriteLine("digraph G{");

            // X. Visit children
            base.VisitExpressionRoot(node, args);

            // X. Create epilogue
            irFile.WriteLine("}");
            irFile.Close();

            // X. Call graphviz
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "dot.exe";
            startInfo.Arguments = " -Tgif " + Path.GetFileName(outputFile) +
                                  " -o " + Path.GetFileNameWithoutExtension(outputFile) + ".gif";

            Process proc = Process.Start(startInfo);

            return 0;
        }

        public override int VisitAddition(ALEAddition node, params AlgebraIRPrinter_Params[] args) {
            string parent = args[0].MParent;
            irFile.WriteLine($"\"{parent}\"->\"+\";");
            AlgebraIRPrinter_Params param = new AlgebraIRPrinter_Params() {
                MParent = "+"
            };
            base.VisitAddition(node, param);
            return 0;
        }

        public override int VisitMultiplication(ALEMultiplication node, params AlgebraIRPrinter_Params[] args) {
            string parent = args[0].MParent;
            irFile.WriteLine($"\"{parent}\"->\"*\";");
            AlgebraIRPrinter_Params param = new AlgebraIRPrinter_Params() {
                MParent = "*"
            };
            base.VisitMultiplication(node, param);
            return 0;
        }
    }
}
