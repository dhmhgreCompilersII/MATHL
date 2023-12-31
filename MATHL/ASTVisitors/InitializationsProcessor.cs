﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MATHL.Composite;
using MATHL.TypeSystem;
using SharpCompress.Common;

namespace MATHL.ASTVisitors {

    internal struct InitializationsInfo {
        private LValue m_value;
        private VariableSymbol m_Variable;

        public LValue M_Value {
            get => m_value;
            set => m_value = value;
        }

        public VariableSymbol M_VariableSymbol {
            get => m_Variable;
            set => m_Variable = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    internal class InitializationsProcessor : MATHLBaseVisitor<InitializationsInfo, InitializationsInfo> {
        private ScopeSystem m_scopeSystem;

        public InitializationsProcessor(ScopeSystem mScopeSystem) {
            m_scopeSystem = mScopeSystem;
        }

        public override InitializationsInfo VisitCompileUnit(CCompileUnit node, params InitializationsInfo[] args) {
            m_scopeSystem.EnterScope("global");
            base.VisitCompileUnit(node, args);
            m_scopeSystem.ExitScope();
            return default(InitializationsInfo);
        }

        public override InitializationsInfo VisitCommand_CommandBlock(CCommand_CommandBlock node, params InitializationsInfo[] args) {
            Scope commandBlockScope = node[typeof(Scope)] as Scope;
            m_scopeSystem.EnterScope(commandBlockScope.M_ScopeName);
            base.VisitCommand_CommandBlock(node, args);
            m_scopeSystem.ExitScope();
            return default(InitializationsInfo);
        }

        public override InitializationsInfo VisitCommand_Expression(CCommand_Expression node, params InitializationsInfo[] args) {
            return default(InitializationsInfo);
        }

        public override InitializationsInfo VisitDeclaration_Function(CDeclarationFunction node, params InitializationsInfo[] args) {
            m_scopeSystem.EnterScope(node.MFunctionName);
            base.VisitContextChildren(node, CDeclarationFunction.BODY, args);
            m_scopeSystem.ExitScope();
            return default(InitializationsInfo);
        }

        public override InitializationsInfo VisitDeclarator_Variable(CDeclaratorVariable node, params InitializationsInfo[] args) {
            args = new InitializationsInfo[1];
            InitializationsInfo newInfo = new InitializationsInfo();
            args[0] = newInfo;
            base.VisitDeclarator_Variable(node, args);
            args[0].M_VariableSymbol.MValue = args[0].M_Value;
            return default(InitializationsInfo);
        }


        public override InitializationsInfo VisitT_INTEGERNUMBER(CINTEGERNUMBER node, params InitializationsInfo[] args) {
            int value = Int32.Parse(node.MStringLiteral);
            args[0].M_Value = new LValue() { MType = TypeID.TID_INTEGER, Ivalue = value };
            return args[0];
        }

        public override InitializationsInfo VisitT_FLOATNUMBER(CFLOATNUMBER node, params InitializationsInfo[] args) {
            float value = float.Parse(node.MStringLiteral);
            args[0].M_Value = new LValue() { MType = TypeID.TID_FLOAT, Fvalue = value };
            return args[0];
        }

        public override InitializationsInfo VisitT_IDENTIFIER(CIDENTIFIER node, params InitializationsInfo[] args) {
            args[0].M_VariableSymbol = node.MSymbol as VariableSymbol;
            return args[0];
        }
    }
}
