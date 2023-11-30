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
    /// <summary>
    /// This method executes the boilerplate code that is necessary to visit successor
    /// in a specific context.
    /// </summary>
    /// <typeparam name="Result"></typeparam>
    /// <param name="t">class that is extended</param>
    /// <param name="node">the successor that is to visit</param>
    /// <param name="context">the context of the current node where the successor resides</param>
    /// <param name="contextsStack">transfers the context of the current node to the successor node</param>
    /// <param name="parent">the node which is considered parent for the successor</param>
    /// <param name="parentStack">transfers the parent to the successor node</param>
    /// <returns></returns>
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

    /// <summary>
    /// This method executes the boilerplate code that is necessary to visit successors
    /// in a specific context.
    /// </summary>
    /// <typeparam name="Result"></typeparam>
    /// <param name="t">class that is extended</param>
    /// <param name="nodeset">the list of nodes that is to visit</param>
    /// <param name="context">the context of the current node where the successors reside</param>
    /// <param name="contextsStack">transfers the context of the current node to the successor nodes</param>
    /// <param name="parent">the node which is considered parent for the successors</param>
    /// <param name="parentsStack">transfers the parent to the successor node</param>
    /// <returns></returns>
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
    /// <summary>
    /// This method visits a token of a ParserRuleContext node passing the appropriate information
    /// from parent node
    /// </summary>
    /// <typeparam name="Result"></typeparam>
    /// <param name="t"></param>
    /// <param name="tokenParent">ParserRuleContext parent node of the terminal node</param>
    /// <param name="node">IToken node child of tokenParent</param>
    /// <param name="context">context of parent that is to place the child node</param>
    /// <param name="s">stack of parent contexts</param>
    /// <param name="parent">ASTElement parent node of the Terminal child node</param>
    /// <param name="parentStack">stack of ASTElement parents</param>
    /// <returns></returns>
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

