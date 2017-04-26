namespace ClipperLib
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class PolyNode
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsOpen>k__BackingField;
        internal List<PolyNode> m_Childs = new List<PolyNode>();
        internal EndType m_endtype;
        internal int m_Index;
        internal JoinType m_jointype;
        internal PolyNode m_Parent;
        internal List<IntPoint> m_polygon = new List<IntPoint>();

        internal void AddChild(PolyNode Child)
        {
            int count = this.m_Childs.Count;
            this.m_Childs.Add(Child);
            Child.m_Parent = this;
            Child.m_Index = count;
        }

        public PolyNode GetNext()
        {
            if (this.m_Childs.Count > 0)
            {
                return this.m_Childs[0];
            }
            return this.GetNextSiblingUp();
        }

        internal PolyNode GetNextSiblingUp()
        {
            if (this.m_Parent == null)
            {
                return null;
            }
            if (this.m_Index == (this.m_Parent.m_Childs.Count - 1))
            {
                return this.m_Parent.GetNextSiblingUp();
            }
            return this.m_Parent.m_Childs[this.m_Index + 1];
        }

        private bool IsHoleNode()
        {
            bool flag = true;
            for (PolyNode node = this.m_Parent; node > null; node = node.m_Parent)
            {
                flag = !flag;
            }
            return flag;
        }

        public int ChildCount =>
            this.m_Childs.Count;

        public List<PolyNode> Childs =>
            this.m_Childs;

        public List<IntPoint> Contour =>
            this.m_polygon;

        public bool IsHole =>
            this.IsHoleNode();

        public bool IsOpen { get; set; }

        public PolyNode Parent =>
            this.m_Parent;
    }
}

