using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MATHL.Composite {


    public class ASTChildrenIterator : IASTIterator {
        private ASTComposite m_sourceNode;

        private int mi_curContext;
        private int mi_curNode;
        private ASTElement m_curNode;
        private bool m_endFlag;

        public ASTElement MCurNode => m_curNode;

        public ASTChildrenIterator(ASTComposite sourceNode) {
            m_sourceNode = sourceNode;
            mi_curContext = 0;
            mi_curNode = 0;
            m_endFlag = false;
        }

        public void Init() {
            mi_curContext = 0;
            mi_curNode = 0;

            mi_curContext = 0;
            while (mi_curContext < m_sourceNode.MContexts &&
                   m_sourceNode.GetNumberOfContextNodes(mi_curContext) == 0) {
                mi_curContext++;
            }

            if (mi_curContext == m_sourceNode.MContexts) {
                m_endFlag = true;
            }
            else {
                m_curNode = m_sourceNode.GetChild(mi_curContext, mi_curNode);
                m_endFlag = false;
            }

            /*
            if (m_sourceNode.MContexts != 0 &&
                m_sourceNode.GetNumberOfContextNodes(mi_curContext) != 0) {
                m_curNode = m_sourceNode.GetChild(mi_curContext, mi_curNode);
                m_endFlag = false;
            } else {
                m_endFlag = true;
            }*/

        }

        public bool End() {
            return m_endFlag;
        }

        public void Next() {
            mi_curNode++;
            if (mi_curNode < m_sourceNode.GetNumberOfContextNodes(mi_curContext)) {
                m_curNode = m_sourceNode.GetChild(mi_curContext, mi_curNode);
            } else {
                mi_curContext++;
                if (mi_curContext < m_sourceNode.MContexts) {
                    mi_curNode = 0;
                    m_curNode = m_sourceNode.GetChild(mi_curContext, mi_curNode);
                } else {
                    m_endFlag = true;
                }
            }
        }
    }

    public class ASTContextIterator : IASTIterator {
        private ASTComposite m_sourceNode;
        private int m_Context;

        private int mi_curNode;
        private ASTElement m_curNode;
        private bool m_endFlag;

        public ASTElement MCurNode => m_curNode;

        public ASTContextIterator(ASTComposite sourceNode, int context) {
            m_sourceNode = sourceNode;
            m_Context = context;
            m_curNode = m_sourceNode.GetChild(m_Context, mi_curNode);
        }

        public void Init() {
            mi_curNode = 0;
            m_endFlag = false;
        }

        public bool End() {
            return m_endFlag;
        }

        public void Next() {
            mi_curNode++; // 
            if (mi_curNode < m_sourceNode.GetNumberOfContextNodes(m_Context)) {
                m_curNode = m_sourceNode.GetChild(m_Context, mi_curNode);
            } else {
                m_endFlag = true;
            }
        }
    }

    public class ASTCompositeEnumerator : IEnumerator<IASTVisitableNode> {
        private ASTComposite m_sourceNode;

        private int mi_curContext;
        private int mi_curNode;
        private ASTElement m_curNode;

        public ASTCompositeEnumerator(ASTComposite sourceNode) {
            m_sourceNode = sourceNode;
            mi_curNode = -1;  // must be initialized here
            mi_curContext = 0;
        }

        public IASTVisitableNode Current => m_curNode;

        public bool MoveNext() {
            mi_curNode++;
            if (mi_curNode < m_sourceNode.GetNumberOfContextNodes(mi_curContext)) {
                m_curNode = m_sourceNode.GetChild(mi_curContext, mi_curNode);
            } else {
                mi_curContext++;
                if (mi_curContext < m_sourceNode.MContexts) {
                    mi_curNode = 0;
                    m_curNode = m_sourceNode.GetChild(mi_curContext, mi_curNode);
                } else {
                    return false;
                }
            }
            return true;
        }

        public void Reset() {  // This is does not actually called by the foreach
                               // Thats why initialization is mandatory in the 
                               // constructor
            mi_curContext = 0;
            mi_curNode = -1;
        }

        object IEnumerator.Current => Current;

        public void Dispose() { }
    }


    public interface IASTIterator {
        ASTElement MCurNode { get; }
        void Init();
        bool End();
        void Next();
    }

    public interface ILabelled {
        string MNodeName { get; }
    }

    public interface IASTVisitableNode {
        Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v, params Params[] info);
    }

    public interface IASTComposite : IEnumerable<IASTVisitableNode> {

    }

    public interface IASTBaseVisitor<Return, Params> {
        Return Visit(IASTVisitableNode node, params Params[] info);
        Return VisitChildren(IASTComposite node, params Params[] info);
    }


    public class ASTElement : IASTVisitableNode, ILabelled {
        private int m_type;
        private int m_serialNumber;
        // open for modification by subclasses
        protected string m_nodeName;
        private ASTComposite m_parent;
        private static int ms_serialCounter;
        // Either will use this member or there has to be
        // a static dictionary to map ASTNode to the corresponding
        // link hierarchy node. Trees have many applications 
        // most of the times they extend to an application domain
        // So this link is necessary.
        private object m_hierarchyBridgeLink;

        public object HierarchyBridgeLink {
            get => m_hierarchyBridgeLink;
            set => m_hierarchyBridgeLink = value ??
                throw new ArgumentNullException(nameof(value));
        }

        public int MType => m_type;

        // Node label is open for changes as is virtual
        public virtual int MSerialNumber => m_serialNumber;

        // Node label is open for changes as is virtual
        public virtual string MNodeName => m_nodeName;

        public ASTComposite MParent => m_parent;

        public static int MsSerialCounter => ms_serialCounter;

        public ASTElement(int mType) {
            m_type = mType;
            m_serialNumber = ms_serialCounter++;
            m_nodeName = "Node" + GetType().Name + m_serialNumber;
        }

        public virtual void SetParent(ASTComposite parent) {
            m_parent = parent;
        }

        public virtual Return Accept<Return, Params>(IASTBaseVisitor<Return, Params> v,
            params Params[] info) {
            return v.Visit(this);
        }
    }

    public class ASTComposite : ASTElement, IASTComposite {

        List<ASTElement>[] m_children;

        public int MContexts => m_children.Length;

        public ASTComposite(int contexts, int mType) :
            base(mType) {
            m_children = new List<ASTElement>[contexts];
            for (int i = 0; i < contexts; i++) {
                m_children[i] = new List<ASTElement>();
            }
        }

        public int GetNumberOfContextNodes(int context) {
            if (context < m_children.Length) {
                return m_children[context].Count;
            } else {
                throw new ArgumentOutOfRangeException("context index out of range");
            }
        }

        public IEnumerable<ASTElement> GetContextChildren(int context) {
            if (context < m_children.Length) {
                foreach (ASTElement node in m_children[context]) {
                    yield return node;
                }
            } else {
                throw new ArgumentOutOfRangeException("node index out of range");
            }
        }

        public IEnumerable<ASTElement> GetChildren() {
            foreach (ASTElement node in this) {
                yield return node;
            }
        }

        public ASTElement GetChild(int context, int index = 0) {
            if (context < m_children.Length) {
                if (index < m_children[context].Count) {
                    return m_children[context][index];
                } else {
                    throw new ArgumentOutOfRangeException("node index out of range");
                }
            } else {
                throw new ArgumentOutOfRangeException("context index out of range");
            }
        }

        public void AddChild(int context, ASTElement child) {
            if (context < m_children.Length) {
                m_children[context].Add(child);
                child.SetParent(this);
            } else {
                throw new ArgumentOutOfRangeException("context index out of range");
            }
        }

        public IEnumerator<IASTVisitableNode> GetEnumerator() {
            return new ASTCompositeEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public IASTIterator CreateIterator() {
            return new ASTChildrenIterator(this);
        }

        public IASTIterator CreateContextIterator(int context) {
            return new ASTContextIterator(this, context);
        }

        public override Return Accept<Return, Params>(IASTBaseVisitor<Return,
            Params> v, params Params[] info) {
            return v.Visit(this, info);
        }
    }

    public class ASTLeaf : ASTElement {
        private string m_stringLiteral;

        public string MStringLiteral => m_stringLiteral;

        public ASTLeaf(string leafLiteral, int mType) :
            base(mType) {
            m_stringLiteral = leafLiteral;
        }
        public override Return Accept<Return, Params>(IASTBaseVisitor<Return,
            Params> v, params Params[] info) {
            return v.Visit(this, info);
        }
    }

}
