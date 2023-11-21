using Antlr4.Runtime.Tree;
using Antlr4.Runtime;
using MATHL.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class ANTLRExtensions {

    private static ITerminalNode GetTerminalNode<Result>(this AbstractParseTreeVisitor<Result> t,
        ParserRuleContext node, IToken terminal) {

        for (int i = 0; i < node.ChildCount; i++) {
            ITerminalNode child = node.GetChild(i) as ITerminalNode;
            if (child != null) {
                if (child.Symbol == terminal) {
                    return child;
                }
            }
        }
        return null;
    }
    public static Result VisitElementInContext<Result>(this AbstractParseTreeVisitor<Result> t,
        ParserRuleContext node, int context, Stack<int> contextsStack,
        ASTComposite parent, Stack<ASTComposite> parentStack) {
        Result res = default(Result);

        parentStack.Push(parent);
        contextsStack.Push(context);
        res = t.Visit(node);     // Visits a particular element
        contextsStack.Pop();
        parentStack.Pop();

        return res;
    }

    public static Result VisitElementsInContext<Result>(this AbstractParseTreeVisitor<Result> t,
        IEnumerable<IParseTree> nodeset, int context, Stack<int> contextsStack,
        ASTComposite parent, Stack<ASTComposite> parentsStack) {
        Result res = default(Result);

        parentsStack.Push(parent);
        contextsStack.Push(context);
        foreach (IParseTree node in nodeset) {
            res = t.Visit(node);
        }
        contextsStack.Pop();
        parentsStack.Pop();
        return res;
    }

    public static Result VisitTerminalInContext<Result>(this AbstractParseTreeVisitor<Result> t,
        ParserRuleContext tokenParent, IToken node, int context, Stack<int> s,
        ASTComposite parent, Stack<ASTComposite> parentStack) {
        parentStack.Push(parent);
        s.Push(context);
        Result res = t.Visit(GetTerminalNode<Result>(t, tokenParent, node));
        s.Pop();
        parentStack.Pop();
        return res;
    }
}

