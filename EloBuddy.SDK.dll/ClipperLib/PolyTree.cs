namespace ClipperLib
{
    using System;
    using System.Collections.Generic;

    public class PolyTree : PolyNode
    {
        internal List<PolyNode> m_AllPolys = new List<PolyNode>();

        public void Clear()
        {
            for (int i = 0; i < this.m_AllPolys.Count; i++)
            {
                this.m_AllPolys[i] = null;
            }
            this.m_AllPolys.Clear();
            base.m_Childs.Clear();
        }

        ~PolyTree()
        {
            this.Clear();
        }

        public PolyNode GetFirst()
        {
            if (base.m_Childs.Count > 0)
            {
                return base.m_Childs[0];
            }
            return null;
        }

        public int Total
        {
            get
            {
                int count = this.m_AllPolys.Count;
                if ((count > 0) && (base.m_Childs[0] != this.m_AllPolys[0]))
                {
                    count--;
                }
                return count;
            }
        }
    }
}

