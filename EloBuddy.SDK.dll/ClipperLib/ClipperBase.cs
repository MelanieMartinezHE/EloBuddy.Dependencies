namespace ClipperLib
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class ClipperBase
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <PreserveCollinear>k__BackingField;
        public const long hiRange = 0x3fffffffffffffffL;
        protected const double horizontal = -3.4E+38;
        public const long loRange = 0x3fffffffL;
        internal LocalMinima m_CurrentLM = null;
        internal List<List<TEdge>> m_edges = new List<List<TEdge>>();
        internal bool m_HasOpenPaths = false;
        internal LocalMinima m_MinimaList = null;
        internal bool m_UseFullRange = false;
        protected const int Skip = -2;
        protected const double tolerance = 1E-20;
        protected const int Unassigned = -1;

        internal ClipperBase()
        {
        }

        public bool AddPath(List<IntPoint> pg, PolyType polyType, bool Closed)
        {
            if (!Closed)
            {
                throw new ClipperException("AddPath: Open paths have been disabled.");
            }
            int num = pg.Count - 1;
            if (Closed)
            {
                while ((num > 0) && (pg[num] == pg[0]))
                {
                    num--;
                }
            }
            while ((num > 0) && (pg[num] == pg[num - 1]))
            {
                num--;
            }
            if ((Closed && (num < 2)) || (!Closed && (num < 1)))
            {
                return false;
            }
            List<TEdge> item = new List<TEdge>(num + 1);
            for (int i = 0; i <= num; i++)
            {
                item.Add(new TEdge());
            }
            bool flag = true;
            item[1].Curr = pg[1];
            this.RangeTest(pg[0], ref this.m_UseFullRange);
            this.RangeTest(pg[num], ref this.m_UseFullRange);
            this.InitEdge(item[0], item[1], item[num], pg[0]);
            this.InitEdge(item[num], item[0], item[num - 1], pg[num]);
            for (int j = num - 1; j >= 1; j--)
            {
                this.RangeTest(pg[j], ref this.m_UseFullRange);
                this.InitEdge(item[j], item[j + 1], item[j - 1], pg[j]);
            }
            TEdge next = item[0];
            TEdge e = next;
            TEdge edge3 = next;
        Label_01C3:
            while ((e.Curr == e.Next.Curr) && (Closed || (e.Next != next)))
            {
                if (e == e.Next)
                {
                    goto Label_0321;
                }
                if (e == next)
                {
                    next = e.Next;
                }
                e = this.RemoveEdge(e);
                edge3 = e;
            }
            if (e.Prev != e.Next)
            {
                if ((Closed && SlopesEqual(e.Prev.Curr, e.Curr, e.Next.Curr, this.m_UseFullRange)) && (!this.PreserveCollinear || !this.Pt2IsBetweenPt1AndPt3(e.Prev.Curr, e.Curr, e.Next.Curr)))
                {
                    if (e == next)
                    {
                        next = e.Next;
                    }
                    e = this.RemoveEdge(e).Prev;
                    edge3 = e;
                    goto Label_01C3;
                }
                e = e.Next;
                if ((e != edge3) && (Closed || (e.Next != next)))
                {
                    goto Label_01C3;
                }
            }
        Label_0321:
            if ((!Closed && (e == e.Next)) || (Closed && (e.Prev == e.Next)))
            {
                return false;
            }
            if (!Closed)
            {
                this.m_HasOpenPaths = true;
                next.Prev.OutIdx = -2;
            }
            e = next;
            do
            {
                this.InitEdge2(e, polyType);
                e = e.Next;
                if (flag && (e.Curr.Y != next.Curr.Y))
                {
                    flag = false;
                }
            }
            while (e != next);
            if (flag)
            {
                if (Closed)
                {
                    return false;
                }
                e.Prev.OutIdx = -2;
                if (e.Prev.Bot.X < e.Prev.Top.X)
                {
                    this.ReverseHorizontal(e.Prev);
                }
                LocalMinima newLm = new LocalMinima {
                    Next = null,
                    Y = e.Bot.Y,
                    LeftBound = null,
                    RightBound = e
                };
                newLm.RightBound.Side = EdgeSide.esRight;
                newLm.RightBound.WindDelta = 0;
                while (e.Next.OutIdx != -2)
                {
                    e.NextInLML = e.Next;
                    if (e.Bot.X != e.Prev.Top.X)
                    {
                        this.ReverseHorizontal(e);
                    }
                    e = e.Next;
                }
                this.InsertLocalMinima(newLm);
                this.m_edges.Add(item);
                return true;
            }
            this.m_edges.Add(item);
            TEdge edge4 = null;
            if (e.Prev.Bot == e.Prev.Top)
            {
                e = e.Next;
            }
            while (true)
            {
                bool flag2;
                e = this.FindNextLocMin(e);
                if (e == edge4)
                {
                    break;
                }
                if (edge4 == null)
                {
                    edge4 = e;
                }
                LocalMinima minima2 = new LocalMinima {
                    Next = null,
                    Y = e.Bot.Y
                };
                if (e.Dx < e.Prev.Dx)
                {
                    minima2.LeftBound = e.Prev;
                    minima2.RightBound = e;
                    flag2 = false;
                }
                else
                {
                    minima2.LeftBound = e;
                    minima2.RightBound = e.Prev;
                    flag2 = true;
                }
                minima2.LeftBound.Side = EdgeSide.esLeft;
                minima2.RightBound.Side = EdgeSide.esRight;
                if (!Closed)
                {
                    minima2.LeftBound.WindDelta = 0;
                }
                else if (minima2.LeftBound.Next == minima2.RightBound)
                {
                    minima2.LeftBound.WindDelta = -1;
                }
                else
                {
                    minima2.LeftBound.WindDelta = 1;
                }
                minima2.RightBound.WindDelta = -minima2.LeftBound.WindDelta;
                e = this.ProcessBound(minima2.LeftBound, flag2);
                if (e.OutIdx == -2)
                {
                    e = this.ProcessBound(e, flag2);
                }
                TEdge edge5 = this.ProcessBound(minima2.RightBound, !flag2);
                if (edge5.OutIdx == -2)
                {
                    edge5 = this.ProcessBound(edge5, !flag2);
                }
                if (minima2.LeftBound.OutIdx == -2)
                {
                    minima2.LeftBound = null;
                }
                else if (minima2.RightBound.OutIdx == -2)
                {
                    minima2.RightBound = null;
                }
                this.InsertLocalMinima(minima2);
                if (!flag2)
                {
                    e = edge5;
                }
            }
            return true;
        }

        public bool AddPaths(List<List<IntPoint>> ppg, PolyType polyType, bool closed)
        {
            bool flag = false;
            for (int i = 0; i < ppg.Count; i++)
            {
                if (this.AddPath(ppg[i], polyType, closed))
                {
                    flag = true;
                }
            }
            return flag;
        }

        public virtual void Clear()
        {
            this.DisposeLocalMinimaList();
            for (int i = 0; i < this.m_edges.Count; i++)
            {
                for (int j = 0; j < this.m_edges[i].Count; j++)
                {
                    this.m_edges[i][j] = null;
                }
                this.m_edges[i].Clear();
            }
            this.m_edges.Clear();
            this.m_UseFullRange = false;
            this.m_HasOpenPaths = false;
        }

        private void DisposeLocalMinimaList()
        {
            while (this.m_MinimaList > null)
            {
                LocalMinima next = this.m_MinimaList.Next;
                this.m_MinimaList = null;
                this.m_MinimaList = next;
            }
            this.m_CurrentLM = null;
        }

        private TEdge FindNextLocMin(TEdge E)
        {
            TEdge edge;
            do
            {
                while ((E.Bot != E.Prev.Bot) || (E.Curr == E.Top))
                {
                    E = E.Next;
                }
                if ((E.Dx != -3.4E+38) && !(E.Prev.Dx == -3.4E+38))
                {
                    return E;
                }
                while (E.Prev.Dx == -3.4E+38)
                {
                    E = E.Prev;
                }
                edge = E;
                while (E.Dx == -3.4E+38)
                {
                    E = E.Next;
                }
            }
            while (E.Top.Y == E.Prev.Bot.Y);
            if (edge.Prev.Bot.X < E.Bot.X)
            {
                E = edge;
            }
            return E;
        }

        public static IntRect GetBounds(List<List<IntPoint>> paths)
        {
            int num = 0;
            int count = paths.Count;
            while ((num < count) && (paths[num].Count == 0))
            {
                num++;
            }
            if (num == count)
            {
                return new IntRect(0L, 0L, 0L, 0L);
            }
            IntRect rect = new IntRect {
                left = paths[num][0].X
            };
            rect.right = rect.left;
            rect.top = paths[num][0].Y;
            rect.bottom = rect.top;
            while (num < count)
            {
                for (int i = 0; i < paths[num].Count; i++)
                {
                    if (paths[num][i].X < rect.left)
                    {
                        rect.left = paths[num][i].X;
                    }
                    else if (paths[num][i].X > rect.right)
                    {
                        rect.right = paths[num][i].X;
                    }
                    if (paths[num][i].Y < rect.top)
                    {
                        rect.top = paths[num][i].Y;
                    }
                    else if (paths[num][i].Y > rect.bottom)
                    {
                        rect.bottom = paths[num][i].Y;
                    }
                }
                num++;
            }
            return rect;
        }

        private void InitEdge(TEdge e, TEdge eNext, TEdge ePrev, IntPoint pt)
        {
            e.Next = eNext;
            e.Prev = ePrev;
            e.Curr = pt;
            e.OutIdx = -1;
        }

        private void InitEdge2(TEdge e, PolyType polyType)
        {
            if (e.Curr.Y >= e.Next.Curr.Y)
            {
                e.Bot = e.Curr;
                e.Top = e.Next.Curr;
            }
            else
            {
                e.Top = e.Curr;
                e.Bot = e.Next.Curr;
            }
            this.SetDx(e);
            e.PolyTyp = polyType;
        }

        private void InsertLocalMinima(LocalMinima newLm)
        {
            if (this.m_MinimaList == null)
            {
                this.m_MinimaList = newLm;
            }
            else if (newLm.Y >= this.m_MinimaList.Y)
            {
                newLm.Next = this.m_MinimaList;
                this.m_MinimaList = newLm;
            }
            else
            {
                LocalMinima minimaList = this.m_MinimaList;
                while ((minimaList.Next != null) && (newLm.Y < minimaList.Next.Y))
                {
                    minimaList = minimaList.Next;
                }
                newLm.Next = minimaList.Next;
                minimaList.Next = newLm;
            }
        }

        internal static bool IsHorizontal(TEdge e) => 
            (e.Delta.Y == 0L);

        internal static bool near_zero(double val) => 
            ((val > -1E-20) && (val < 1E-20));

        internal bool PointIsVertex(IntPoint pt, OutPt pp)
        {
            OutPt next = pp;
            do
            {
                if (next.Pt == pt)
                {
                    return true;
                }
                next = next.Next;
            }
            while (next != pp);
            return false;
        }

        internal bool PointOnLineSegment(IntPoint pt, IntPoint linePt1, IntPoint linePt2, bool UseFullRange)
        {
            if (UseFullRange)
            {
                return ((((pt.X == linePt1.X) && (pt.Y == linePt1.Y)) || ((pt.X == linePt2.X) && (pt.Y == linePt2.Y))) || ((((pt.X > linePt1.X) == (pt.X < linePt2.X)) && ((pt.Y > linePt1.Y) == (pt.Y < linePt2.Y))) && (Int128.Int128Mul(pt.X - linePt1.X, linePt2.Y - linePt1.Y) == Int128.Int128Mul(linePt2.X - linePt1.X, pt.Y - linePt1.Y))));
            }
            return ((((pt.X == linePt1.X) && (pt.Y == linePt1.Y)) || ((pt.X == linePt2.X) && (pt.Y == linePt2.Y))) || ((((pt.X > linePt1.X) == (pt.X < linePt2.X)) && ((pt.Y > linePt1.Y) == (pt.Y < linePt2.Y))) && (((pt.X - linePt1.X) * (linePt2.Y - linePt1.Y)) == ((linePt2.X - linePt1.X) * (pt.Y - linePt1.Y)))));
        }

        internal bool PointOnPolygon(IntPoint pt, OutPt pp, bool UseFullRange)
        {
            OutPt next = pp;
            while (true)
            {
                if (this.PointOnLineSegment(pt, next.Pt, next.Next.Pt, UseFullRange))
                {
                    return true;
                }
                next = next.Next;
                if (next == pp)
                {
                    return false;
                }
            }
        }

        protected void PopLocalMinima()
        {
            if (this.m_CurrentLM != null)
            {
                this.m_CurrentLM = this.m_CurrentLM.Next;
            }
        }

        private TEdge ProcessBound(TEdge E, bool LeftBoundIsForward)
        {
            TEdge next = E;
            if (next.OutIdx != -2)
            {
                TEdge prev;
                TEdge edge3;
                if (E.Dx == -3.4E+38)
                {
                    if (LeftBoundIsForward)
                    {
                        prev = E.Prev;
                    }
                    else
                    {
                        prev = E.Next;
                    }
                    if (prev.OutIdx != -2)
                    {
                        if (prev.Dx == -3.4E+38)
                        {
                            if ((prev.Bot.X != E.Bot.X) && (prev.Top.X != E.Bot.X))
                            {
                                this.ReverseHorizontal(E);
                            }
                        }
                        else if (prev.Bot.X != E.Bot.X)
                        {
                            this.ReverseHorizontal(E);
                        }
                    }
                }
                prev = E;
                if (LeftBoundIsForward)
                {
                    while ((next.Top.Y == next.Next.Bot.Y) && (next.Next.OutIdx != -2))
                    {
                        next = next.Next;
                    }
                    if ((next.Dx == -3.4E+38) && (next.Next.OutIdx != -2))
                    {
                        edge3 = next;
                        while (edge3.Prev.Dx == -3.4E+38)
                        {
                            edge3 = edge3.Prev;
                        }
                        if (edge3.Prev.Top.X == next.Next.Top.X)
                        {
                            if (!LeftBoundIsForward)
                            {
                                next = edge3.Prev;
                            }
                        }
                        else if (edge3.Prev.Top.X > next.Next.Top.X)
                        {
                            next = edge3.Prev;
                        }
                    }
                    while (E != next)
                    {
                        E.NextInLML = E.Next;
                        if (((E.Dx == -3.4E+38) && (E != prev)) && (E.Bot.X != E.Prev.Top.X))
                        {
                            this.ReverseHorizontal(E);
                        }
                        E = E.Next;
                    }
                    if (((E.Dx == -3.4E+38) && (E != prev)) && (E.Bot.X != E.Prev.Top.X))
                    {
                        this.ReverseHorizontal(E);
                    }
                    return next.Next;
                }
                while ((next.Top.Y == next.Prev.Bot.Y) && (next.Prev.OutIdx != -2))
                {
                    next = next.Prev;
                }
                if ((next.Dx == -3.4E+38) && (next.Prev.OutIdx != -2))
                {
                    edge3 = next;
                    while (edge3.Next.Dx == -3.4E+38)
                    {
                        edge3 = edge3.Next;
                    }
                    if (edge3.Next.Top.X == next.Prev.Top.X)
                    {
                        if (!LeftBoundIsForward)
                        {
                            next = edge3.Next;
                        }
                    }
                    else if (edge3.Next.Top.X > next.Prev.Top.X)
                    {
                        next = edge3.Next;
                    }
                }
                while (E != next)
                {
                    E.NextInLML = E.Prev;
                    if (((E.Dx == -3.4E+38) && (E != prev)) && (E.Bot.X != E.Next.Top.X))
                    {
                        this.ReverseHorizontal(E);
                    }
                    E = E.Prev;
                }
                if (((E.Dx == -3.4E+38) && (E != prev)) && (E.Bot.X != E.Next.Top.X))
                {
                    this.ReverseHorizontal(E);
                }
                return next.Prev;
            }
            E = next;
            if (!LeftBoundIsForward)
            {
                while (E.Top.Y == E.Prev.Bot.Y)
                {
                    E = E.Prev;
                }
                while ((E != next) && (E.Dx == -3.4E+38))
                {
                    E = E.Next;
                }
            }
            else
            {
                while (E.Top.Y == E.Next.Bot.Y)
                {
                    E = E.Next;
                }
                while ((E != next) && (E.Dx == -3.4E+38))
                {
                    E = E.Prev;
                }
            }
            if (E == next)
            {
                if (LeftBoundIsForward)
                {
                    return E.Next;
                }
                return E.Prev;
            }
            if (LeftBoundIsForward)
            {
                E = next.Next;
            }
            else
            {
                E = next.Prev;
            }
            LocalMinima newLm = new LocalMinima {
                Next = null,
                Y = E.Bot.Y,
                LeftBound = null,
                RightBound = E
            };
            E.WindDelta = 0;
            next = this.ProcessBound(E, LeftBoundIsForward);
            this.InsertLocalMinima(newLm);
            return next;
        }

        internal bool Pt2IsBetweenPt1AndPt3(IntPoint pt1, IntPoint pt2, IntPoint pt3)
        {
            if (((pt1 == pt3) || (pt1 == pt2)) || (pt3 == pt2))
            {
                return false;
            }
            if (pt1.X != pt3.X)
            {
                return ((pt2.X > pt1.X) == (pt2.X < pt3.X));
            }
            return ((pt2.Y > pt1.Y) == (pt2.Y < pt3.Y));
        }

        private void RangeTest(IntPoint Pt, ref bool useFullRange)
        {
            if (useFullRange)
            {
                if ((((Pt.X > 0x3fffffffffffffffL) || (Pt.Y > 0x3fffffffffffffffL)) || (-Pt.X > 0x3fffffffffffffffL)) || (-Pt.Y > 0x3fffffffffffffffL))
                {
                    throw new ClipperException("Coordinate outside allowed range");
                }
            }
            else if ((((Pt.X > 0x3fffffffL) || (Pt.Y > 0x3fffffffL)) || (-Pt.X > 0x3fffffffL)) || (-Pt.Y > 0x3fffffffL))
            {
                useFullRange = true;
                this.RangeTest(Pt, ref useFullRange);
            }
        }

        private TEdge RemoveEdge(TEdge e)
        {
            e.Prev.Next = e.Next;
            e.Next.Prev = e.Prev;
            TEdge next = e.Next;
            e.Prev = null;
            return next;
        }

        protected virtual void Reset()
        {
            this.m_CurrentLM = this.m_MinimaList;
            if (this.m_CurrentLM != null)
            {
                for (LocalMinima minima = this.m_MinimaList; minima > null; minima = minima.Next)
                {
                    TEdge leftBound = minima.LeftBound;
                    if (leftBound > null)
                    {
                        leftBound.Curr = leftBound.Bot;
                        leftBound.Side = EdgeSide.esLeft;
                        leftBound.OutIdx = -1;
                    }
                    leftBound = minima.RightBound;
                    if (leftBound > null)
                    {
                        leftBound.Curr = leftBound.Bot;
                        leftBound.Side = EdgeSide.esRight;
                        leftBound.OutIdx = -1;
                    }
                }
            }
        }

        private void ReverseHorizontal(TEdge e)
        {
            this.Swap(ref e.Top.X, ref e.Bot.X);
        }

        private void SetDx(TEdge e)
        {
            e.Delta.X = e.Top.X - e.Bot.X;
            e.Delta.Y = e.Top.Y - e.Bot.Y;
            if (e.Delta.Y == 0L)
            {
                e.Dx = -3.4E+38;
            }
            else
            {
                e.Dx = ((double) e.Delta.X) / ((double) e.Delta.Y);
            }
        }

        internal static bool SlopesEqual(TEdge e1, TEdge e2, bool UseFullRange)
        {
            if (UseFullRange)
            {
                Int128 introduced2 = Int128.Int128Mul(e1.Delta.Y, e2.Delta.X);
                return (introduced2 == Int128.Int128Mul(e1.Delta.X, e2.Delta.Y));
            }
            return ((e1.Delta.Y * e2.Delta.X) == (e1.Delta.X * e2.Delta.Y));
        }

        protected static bool SlopesEqual(IntPoint pt1, IntPoint pt2, IntPoint pt3, bool UseFullRange)
        {
            if (UseFullRange)
            {
                return (Int128.Int128Mul(pt1.Y - pt2.Y, pt2.X - pt3.X) == Int128.Int128Mul(pt1.X - pt2.X, pt2.Y - pt3.Y));
            }
            return ((((pt1.Y - pt2.Y) * (pt2.X - pt3.X)) - ((pt1.X - pt2.X) * (pt2.Y - pt3.Y))) == 0L);
        }

        protected static bool SlopesEqual(IntPoint pt1, IntPoint pt2, IntPoint pt3, IntPoint pt4, bool UseFullRange)
        {
            if (UseFullRange)
            {
                return (Int128.Int128Mul(pt1.Y - pt2.Y, pt3.X - pt4.X) == Int128.Int128Mul(pt1.X - pt2.X, pt3.Y - pt4.Y));
            }
            return ((((pt1.Y - pt2.Y) * (pt3.X - pt4.X)) - ((pt1.X - pt2.X) * (pt3.Y - pt4.Y))) == 0L);
        }

        public void Swap(ref long val1, ref long val2)
        {
            long num = val1;
            val1 = val2;
            val2 = num;
        }

        public bool PreserveCollinear { get; set; }
    }
}

