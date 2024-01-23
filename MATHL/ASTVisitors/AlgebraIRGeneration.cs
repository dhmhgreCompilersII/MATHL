using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MATHL.Composite;

namespace MATHL.ASTVisitors {

    public class AlgebraIRGeneration_Params {
        private AlgebraElement m_parent;
        private int m_parentContext;

        public AlgebraElement M_Parent {
            get => m_parent;
            set => m_parent = value ?? throw new ArgumentNullException(nameof(value));
        }
        public int M_ParentContext {
            get => m_parentContext;
            set => m_parentContext = value;
        }
    }

    public class AlgebraIRGeneration : MATHLBaseVisitor<AlgebraElement, AlgebraIRGeneration_Params> {
        private ALERoot m_ALERoot;

        public ALERoot M_AleRoot => m_ALERoot;

        public AlgebraIRGeneration() { }

        public override AlgebraElement VisitDeclaration_Function(CDeclarationFunction node, params AlgebraIRGeneration_Params[] args) {
            return null;
        }

        public override AlgebraElement VisitDeclaration_Variable(CDeclarationVariable node, params AlgebraIRGeneration_Params[] args) {
            return null;
        }


        public override AlgebraElement VisitCommand_Expression(CCommand_Expression node, params AlgebraIRGeneration_Params[] args) {
            m_ALERoot= new ALERoot();

            // 3. Visit 
            AlgebraIRGeneration_Params param = new AlgebraIRGeneration_Params() {
                M_Parent = m_ALERoot, M_ParentContext = ALERoot.EXPRESSION
            };
            Visit(node.M_Expression, param);

            return m_ALERoot;
        }

        public override AlgebraElement VisitExpression_Addition(CExpression_Addition node, params AlgebraIRGeneration_Params[] args) {
            AlgebraElement parent = args[0].M_Parent;
            int parentContext = args[0].M_ParentContext;
            AlgebraElement newNode;

            if (parent.MType != (int)AlgebraElements.ALE_ADDITION) {
                // 1. Create ADDITION node
                newNode = new ALEAddition();
                parentContext = ALEAddition.TERMS;
                
                // 2. Connect to parent
                parent.AddChild(parentContext, newNode);

                // 2a. Connect to AST node 
                node.M_AlgebraElement = newNode;
            } else {
                newNode = parent;
            }
            // 3. Visit 
            AlgebraIRGeneration_Params param = new AlgebraIRGeneration_Params() { M_Parent = newNode, M_ParentContext = parentContext };
            Visit(node.M_LHSExpression, param);
            Visit(node.M_RHSExpression, param);

            return newNode;
        }

        public override AlgebraElement VisitExpression_Multiplication(CExpression_Multiplication node, params AlgebraIRGeneration_Params[] args) {
            AlgebraElement parent = args[0].M_Parent;
            int parentContext = args[0].M_ParentContext;
            AlgebraElement newNode;

            if (parent.MType != (int)AlgebraElements.ALE_MULTIPLICATION) {
                // 1. Create ADDITION node
                newNode = new ALEMultiplication();
                parentContext = ALEMultiplication.TERMS;

                // 2. Connect to parent
                parent.AddChild(parentContext, newNode);

                // 2a. Connect to AST node 
                node.M_AlgebraElement = newNode;
            } else {
                newNode = parent;
            }
            // 3. Visit 
            AlgebraIRGeneration_Params param = new AlgebraIRGeneration_Params() { M_Parent = newNode, M_ParentContext = parentContext };
            Visit(node.M_LHSExpression, param);
            Visit(node.M_RHSExpression, param);

            return newNode;
        }

        public override AlgebraElement VisitExpression_ParenthesizedExpression(CExpression_ParenthesizedExpression node,
            params AlgebraIRGeneration_Params[] args) {
            return base.VisitExpression_ParenthesizedExpression(node, args);
        }

        public override AlgebraElement VisitExpression_Number(CExpression_Number node, params AlgebraIRGeneration_Params[] args) {
            AlgebraElement number = base.VisitExpression_Number(node, args);
            node.M_AlgebraElement = number;
            return number;
        }

        public override AlgebraElement VisitT_INTEGERNUMBER(CINTEGERNUMBER node, params AlgebraIRGeneration_Params[] args) {
            AlgebraElement parent = args[0].M_Parent;
            int parentContext = args[0].M_ParentContext;

            // 1. Create IDENTIFIER node
            ALENumber newNode = new ALENumber(node.M_Value);

            // 2. Connect to parent
            parent.AddChild(parentContext, newNode);
            
            return newNode;
        }

        public override AlgebraElement VisitT_FLOATNUMBER(CFLOATNUMBER node, params AlgebraIRGeneration_Params[] args) {
            AlgebraElement parent = args[0].M_Parent;
            int parentContext = args[0].M_ParentContext;

            // 1. Create NUMBER node
            ALENumber newNode = new ALENumber(node.M_Value);

            // 2. Connect to parent
            parent.AddChild(parentContext, newNode);

            return newNode;
        }

        public override AlgebraElement VisitT_IDENTIFIER(CIDENTIFIER node, params AlgebraIRGeneration_Params[] args) {
            AlgebraElement parent = args[0].M_Parent;
            int parentContext = args[0].M_ParentContext;

            // 1. Create NUMBER node
            ALESymbol newNode = new ALESymbol(node.M_StringLiteral);

            // 2. Connect to parent
            parent.AddChild(parentContext, newNode);

            return newNode;
        }

    }
}
