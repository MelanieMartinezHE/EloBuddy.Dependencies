namespace ClipperLib
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class Clipper : ClipperBase
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <ReverseSolution>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <StrictlySimple>k__BackingField;
        public const int ioPreserveCollinear = 4;
        public const int ioReverseSolution = 1;
        public const int ioStrictlySimple = 2;
        private TEdge m_ActiveEdges = null;
        private PolyFillType m_ClipFillType;
        private ClipType m_ClipType;
        private bool m_ExecuteLocked = false;
        private List<Join> m_GhostJoins = new List<Join>();
        private List<IntersectNode> m_IntersectList = new List<IntersectNode>();
        private IComparer<IntersectNode> m_IntersectNodeComparer = new MyIntersectNodeSort();
        private List<Join> m_Joins = new List<Join>();
        private List<OutRec> m_PolyOuts = new List<OutRec>();
        private Scanbeam m_Scanbeam = null;
        private TEdge m_SortedEdges = null;
        private PolyFillType m_SubjFillType;
        private bool m_UsingPolyTree = false;

        public Clipper(int InitOptions = 0)
        {
            this.ReverseSolution = (1 & InitOptions) > 0;
            this.StrictlySimple = (2 & InitOptions) > 0;
            base.PreserveCollinear = (4 & InitOptions) > 0;
        }

        private void AddEdgeToSEL(TEdge edge)
        {
            if (this.m_SortedEdges == null)
            {
                this.m_SortedEdges = edge;
                edge.PrevInSEL = null;
                edge.NextInSEL = null;
            }
            else
            {
                edge.NextInSEL = this.m_SortedEdges;
                edge.PrevInSEL = null;
                this.m_SortedEdges.PrevInSEL = edge;
                this.m_SortedEdges = edge;
            }
        }

        private void AddGhostJoin(OutPt Op, IntPoint OffPt)
        {
            Join item = new Join {
                OutPt1 = Op,
                OffPt = OffPt
            };
            this.m_GhostJoins.Add(item);
        }

        private void AddJoin(OutPt Op1, OutPt Op2, IntPoint OffPt)
        {
            Join item = new Join {
                OutPt1 = Op1,
                OutPt2 = Op2,
                OffPt = OffPt
            };
            this.m_Joins.Add(item);
        }

        private void AddLocalMaxPoly(TEdge e1, TEdge e2, IntPoint pt)
        {
            this.AddOutPt(e1, pt);
            if (e2.WindDelta == 0)
            {
                this.AddOutPt(e2, pt);
            }
            if (e1.OutIdx == e2.OutIdx)
            {
                e1.OutIdx = -1;
                e2.OutIdx = -1;
            }
            else if (e1.OutIdx < e2.OutIdx)
            {
                this.AppendPolygon(e1, e2);
            }
            else
            {
                this.AppendPolygon(e2, e1);
            }
        }

        private OutPt AddLocalMinPoly(TEdge e1, TEdge e2, IntPoint pt)
        {
            OutPt pt2;
            TEdge edge;
            TEdge prevInAEL;
            if (ClipperBase.IsHorizontal(e2) || (e1.Dx > e2.Dx))
            {
                pt2 = this.AddOutPt(e1, pt);
                e2.OutIdx = e1.OutIdx;
                e1.Side = EdgeSide.esLeft;
                e2.Side = EdgeSide.esRight;
                edge = e1;
                if (edge.PrevInAEL == e2)
                {
                    prevInAEL = e2.PrevInAEL;
                }
                else
                {
                    prevInAEL = edge.PrevInAEL;
                }
            }
            else
            {
                pt2 = this.AddOutPt(e2, pt);
                e1.OutIdx = e2.OutIdx;
                e1.Side = EdgeSide.esRight;
                e2.Side = EdgeSide.esLeft;
                edge = e2;
                if (edge.PrevInAEL == e1)
                {
                    prevInAEL = e1.PrevInAEL;
                }
                else
                {
                    prevInAEL = edge.PrevInAEL;
                }
            }
            if (((((prevInAEL != null) && (prevInAEL.OutIdx >= 0)) && ((TopX(prevInAEL, pt.Y) == TopX(edge, pt.Y)) && ClipperBase.SlopesEqual(edge, prevInAEL, base.m_UseFullRange))) && (edge.WindDelta != 0)) && (prevInAEL.WindDelta > 0))
            {
                OutPt pt3 = this.AddOutPt(prevInAEL, pt);
                this.AddJoin(pt2, pt3, edge.Top);
            }
            return pt2;
        }

        private OutPt AddOutPt(TEdge e, IntPoint pt)
        {
            bool flag = e.Side == EdgeSide.esLeft;
            if (e.OutIdx < 0)
            {
                OutRec outRec = this.CreateOutRec();
                outRec.IsOpen = e.WindDelta == 0;
                OutPt pt2 = new OutPt();
                outRec.Pts = pt2;
                pt2.Idx = outRec.Idx;
                pt2.Pt = pt;
                pt2.Next = pt2;
                pt2.Prev = pt2;
                if (!outRec.IsOpen)
                {
                    this.SetHoleState(e, outRec);
                }
                e.OutIdx = outRec.Idx;
                return pt2;
            }
            OutRec rec2 = this.m_PolyOuts[e.OutIdx];
            OutPt pts = rec2.Pts;
            if (flag && (pt == pts.Pt))
            {
                return pts;
            }
            if (!flag && (pt == pts.Prev.Pt))
            {
                return pts.Prev;
            }
            OutPt pt5 = new OutPt {
                Idx = rec2.Idx,
                Pt = pt,
                Next = pts,
                Prev = pts.Prev
            };
            pt5.Prev.Next = pt5;
            pts.Prev = pt5;
            if (flag)
            {
                rec2.Pts = pt5;
            }
            return pt5;
        }

        internal static void AddPolyNodeToPaths(PolyNode polynode, NodeType nt, List<List<IntPoint>> paths)
        {
            bool flag = true;
            switch (nt)
            {
                case NodeType.ntOpen:
                    return;

                case NodeType.ntClosed:
                    flag = !polynode.IsOpen;
                    break;
            }
            if ((polynode.m_polygon.Count > 0) & flag)
            {
                paths.Add(polynode.m_polygon);
            }
            foreach (PolyNode node in polynode.Childs)
            {
                AddPolyNodeToPaths(node, nt, paths);
            }
        }

        private void AppendPolygon(TEdge e1, TEdge e2)
        {
            OutRec lowermostRec;
            EdgeSide esLeft;
            OutRec rec = this.m_PolyOuts[e1.OutIdx];
            OutRec rec2 = this.m_PolyOuts[e2.OutIdx];
            if (this.Param1RightOfParam2(rec, rec2))
            {
                lowermostRec = rec2;
            }
            else if (this.Param1RightOfParam2(rec2, rec))
            {
                lowermostRec = rec;
            }
            else
            {
                lowermostRec = this.GetLowermostRec(rec, rec2);
            }
            OutPt pts = rec.Pts;
            OutPt prev = pts.Prev;
            OutPt pp = rec2.Pts;
            OutPt pt4 = pp.Prev;
            if (e1.Side == EdgeSide.esLeft)
            {
                if (e2.Side == EdgeSide.esLeft)
                {
                    this.ReversePolyPtLinks(pp);
                    pp.Next = pts;
                    pts.Prev = pp;
                    prev.Next = pt4;
                    pt4.Prev = prev;
                    rec.Pts = pt4;
                }
                else
                {
                    pt4.Next = pts;
                    pts.Prev = pt4;
                    pp.Prev = prev;
                    prev.Next = pp;
                    rec.Pts = pp;
                }
                esLeft = EdgeSide.esLeft;
            }
            else
            {
                if (e2.Side == EdgeSide.esRight)
                {
                    this.ReversePolyPtLinks(pp);
                    prev.Next = pt4;
                    pt4.Prev = prev;
                    pp.Next = pts;
                    pts.Prev = pp;
                }
                else
                {
                    prev.Next = pp;
                    pp.Prev = prev;
                    pts.Prev = pt4;
                    pt4.Next = pts;
                }
                esLeft = EdgeSide.esRight;
            }
            rec.BottomPt = null;
            if (lowermostRec == rec2)
            {
                if (rec2.FirstLeft != rec)
                {
                    rec.FirstLeft = rec2.FirstLeft;
                }
                rec.IsHole = rec2.IsHole;
            }
            rec2.Pts = null;
            rec2.BottomPt = null;
            rec2.FirstLeft = rec;
            int outIdx = e1.OutIdx;
            int num2 = e2.OutIdx;
            e1.OutIdx = -1;
            e2.OutIdx = -1;
            for (TEdge edge = this.m_ActiveEdges; edge > null; edge = edge.NextInAEL)
            {
                if (edge.OutIdx == num2)
                {
                    edge.OutIdx = outIdx;
                    edge.Side = esLeft;
                    break;
                }
            }
            rec2.Idx = rec.Idx;
        }

        private double Area(OutRec outRec)
        {
            OutPt pts = outRec.Pts;
            if (pts == null)
            {
                return 0.0;
            }
            double num = 0.0;
            do
            {
                num += (pts.Prev.Pt.X + pts.Pt.X) * (pts.Prev.Pt.Y - pts.Pt.Y);
                pts = pts.Next;
            }
            while (pts != outRec.Pts);
            return (num * 0.5);
        }

        public static double Area(List<IntPoint> poly)
        {
            int count = poly.Count;
            if (count < 3)
            {
                return 0.0;
            }
            double num2 = 0.0;
            int num4 = 0;
            int num5 = count - 1;
            while (num4 < count)
            {
                num2 += (poly[num5].X + poly[num4].X) * (poly[num5].Y - poly[num4].Y);
                num5 = num4;
                num4++;
            }
            return (-num2 * 0.5);
        }

        private void BuildIntersectList(long topY)
        {
            if (this.m_ActiveEdges != null)
            {
                TEdge activeEdges = this.m_ActiveEdges;
                this.m_SortedEdges = activeEdges;
                while (activeEdges > null)
                {
                    activeEdges.PrevInSEL = activeEdges.PrevInAEL;
                    activeEdges.NextInSEL = activeEdges.NextInAEL;
                    activeEdges.Curr.X = TopX(activeEdges, topY);
                    activeEdges = activeEdges.NextInAEL;
                }
                bool flag = true;
                while (flag && (this.m_SortedEdges > null))
                {
                    flag = false;
                    activeEdges = this.m_SortedEdges;
                    while (activeEdges.NextInSEL > null)
                    {
                        TEdge nextInSEL = activeEdges.NextInSEL;
                        if (activeEdges.Curr.X > nextInSEL.Curr.X)
                        {
                            IntPoint point;
                            this.IntersectPoint(activeEdges, nextInSEL, out point);
                            IntersectNode item = new IntersectNode {
                                Edge1 = activeEdges,
                                Edge2 = nextInSEL,
                                Pt = point
                            };
                            this.m_IntersectList.Add(item);
                            this.SwapPositionsInSEL(activeEdges, nextInSEL);
                            flag = true;
                        }
                        else
                        {
                            activeEdges = nextInSEL;
                        }
                    }
                    if (activeEdges.PrevInSEL > null)
                    {
                        activeEdges.PrevInSEL.NextInSEL = null;
                    }
                    else
                    {
                        break;
                    }
                }
                this.m_SortedEdges = null;
            }
        }

        private void BuildResult(List<List<IntPoint>> polyg)
        {
            polyg.Clear();
            polyg.Capacity = this.m_PolyOuts.Count;
            for (int i = 0; i < this.m_PolyOuts.Count; i++)
            {
                OutRec rec = this.m_PolyOuts[i];
                if (rec.Pts != null)
                {
                    OutPt prev = rec.Pts.Prev;
                    int capacity = this.PointCount(prev);
                    if (capacity >= 2)
                    {
                        List<IntPoint> item = new List<IntPoint>(capacity);
                        for (int j = 0; j < capacity; j++)
                        {
                            item.Add(prev.Pt);
                            prev = prev.Prev;
                        }
                        polyg.Add(item);
                    }
                }
            }
        }

        private void BuildResult2(PolyTree polytree)
        {
            polytree.Clear();
            polytree.m_AllPolys.Capacity = this.m_PolyOuts.Count;
            for (int i = 0; i < this.m_PolyOuts.Count; i++)
            {
                OutRec outRec = this.m_PolyOuts[i];
                int num2 = this.PointCount(outRec.Pts);
                if ((!outRec.IsOpen || (num2 >= 2)) && (outRec.IsOpen || (num2 >= 3)))
                {
                    this.FixHoleLinkage(outRec);
                    PolyNode item = new PolyNode();
                    polytree.m_AllPolys.Add(item);
                    outRec.PolyNode = item;
                    item.m_polygon.Capacity = num2;
                    OutPt prev = outRec.Pts.Prev;
                    for (int k = 0; k < num2; k++)
                    {
                        item.m_polygon.Add(prev.Pt);
                        prev = prev.Prev;
                    }
                }
            }
            polytree.m_Childs.Capacity = this.m_PolyOuts.Count;
            for (int j = 0; j < this.m_PolyOuts.Count; j++)
            {
                OutRec rec2 = this.m_PolyOuts[j];
                if (rec2.PolyNode != null)
                {
                    if (rec2.IsOpen)
                    {
                        rec2.PolyNode.IsOpen = true;
                        polytree.AddChild(rec2.PolyNode);
                    }
                    else if ((rec2.FirstLeft != null) && (rec2.FirstLeft.PolyNode > null))
                    {
                        rec2.FirstLeft.PolyNode.AddChild(rec2.PolyNode);
                    }
                    else
                    {
                        polytree.AddChild(rec2.PolyNode);
                    }
                }
            }
        }

        public static List<IntPoint> CleanPolygon(List<IntPoint> path, double distance = 1.415)
        {
            int count = path.Count;
            if (count == 0)
            {
                return new List<IntPoint>();
            }
            OutPt[] ptArray = new OutPt[count];
            for (int i = 0; i < count; i++)
            {
                ptArray[i] = new OutPt();
            }
            for (int j = 0; j < count; j++)
            {
                ptArray[j].Pt = path[j];
                ptArray[j].Next = ptArray[(j + 1) % count];
                ptArray[j].Next.Prev = ptArray[j];
                ptArray[j].Idx = 0;
            }
            double distSqrd = distance * distance;
            OutPt op = ptArray[0];
            while ((op.Idx == 0) && (op.Next != op.Prev))
            {
                if (PointsAreClose(op.Pt, op.Prev.Pt, distSqrd))
                {
                    op = ExcludeOp(op);
                    count--;
                }
                else if (PointsAreClose(op.Prev.Pt, op.Next.Pt, distSqrd))
                {
                    ExcludeOp(op.Next);
                    op = ExcludeOp(op);
                    count -= 2;
                }
                else if (SlopesNearCollinear(op.Prev.Pt, op.Pt, op.Next.Pt, distSqrd))
                {
                    op = ExcludeOp(op);
                    count--;
                }
                else
                {
                    op.Idx = 1;
                    op = op.Next;
                }
            }
            if (count < 3)
            {
                count = 0;
            }
            List<IntPoint> list = new List<IntPoint>(count);
            for (int k = 0; k < count; k++)
            {
                list.Add(op.Pt);
                op = op.Next;
            }
            ptArray = null;
            return list;
        }

        public static List<List<IntPoint>> CleanPolygons(List<List<IntPoint>> polys, double distance = 1.415)
        {
            List<List<IntPoint>> list = new List<List<IntPoint>>(polys.Count);
            for (int i = 0; i < polys.Count; i++)
            {
                list.Add(CleanPolygon(polys[i], distance));
            }
            return list;
        }

        public static List<List<IntPoint>> ClosedPathsFromPolyTree(PolyTree polytree)
        {
            List<List<IntPoint>> paths = new List<List<IntPoint>> {
                Capacity = polytree.Total
            };
            AddPolyNodeToPaths(polytree, NodeType.ntClosed, paths);
            return paths;
        }

        private void CopyAELToSEL()
        {
            TEdge activeEdges = this.m_ActiveEdges;
            this.m_SortedEdges = activeEdges;
            while (activeEdges > null)
            {
                activeEdges.PrevInSEL = activeEdges.PrevInAEL;
                activeEdges.NextInSEL = activeEdges.NextInAEL;
                activeEdges = activeEdges.NextInAEL;
            }
        }

        private OutRec CreateOutRec()
        {
            OutRec item = new OutRec {
                Idx = -1,
                IsHole = false,
                IsOpen = false,
                FirstLeft = null,
                Pts = null,
                BottomPt = null,
                PolyNode = null
            };
            this.m_PolyOuts.Add(item);
            item.Idx = this.m_PolyOuts.Count - 1;
            return item;
        }

        private void DeleteFromAEL(TEdge e)
        {
            TEdge prevInAEL = e.PrevInAEL;
            TEdge nextInAEL = e.NextInAEL;
            if (((prevInAEL != null) || (nextInAEL != null)) || (e == this.m_ActiveEdges))
            {
                if (prevInAEL > null)
                {
                    prevInAEL.NextInAEL = nextInAEL;
                }
                else
                {
                    this.m_ActiveEdges = nextInAEL;
                }
                if (nextInAEL > null)
                {
                    nextInAEL.PrevInAEL = prevInAEL;
                }
                e.NextInAEL = null;
                e.PrevInAEL = null;
            }
        }

        private void DeleteFromSEL(TEdge e)
        {
            TEdge prevInSEL = e.PrevInSEL;
            TEdge nextInSEL = e.NextInSEL;
            if (((prevInSEL != null) || (nextInSEL != null)) || (e == this.m_SortedEdges))
            {
                if (prevInSEL > null)
                {
                    prevInSEL.NextInSEL = nextInSEL;
                }
                else
                {
                    this.m_SortedEdges = nextInSEL;
                }
                if (nextInSEL > null)
                {
                    nextInSEL.PrevInSEL = prevInSEL;
                }
                e.NextInSEL = null;
                e.PrevInSEL = null;
            }
        }

        private void DisposeAllPolyPts()
        {
            for (int i = 0; i < this.m_PolyOuts.Count; i++)
            {
                this.DisposeOutRec(i);
            }
            this.m_PolyOuts.Clear();
        }

        private void DisposeOutRec(int index)
        {
            OutRec rec = this.m_PolyOuts[index];
            rec.Pts = null;
            rec = null;
            this.m_PolyOuts[index] = null;
        }

        private void DisposeScanbeamList()
        {
            while (this.m_Scanbeam > null)
            {
                Scanbeam next = this.m_Scanbeam.Next;
                this.m_Scanbeam = null;
                this.m_Scanbeam = next;
            }
        }

        private static double DistanceFromLineSqrd(IntPoint pt, IntPoint ln1, IntPoint ln2)
        {
            double num = ln1.Y - ln2.Y;
            double num2 = ln2.X - ln1.X;
            double num3 = (num * ln1.X) + (num2 * ln1.Y);
            num3 = ((num * pt.X) + (num2 * pt.Y)) - num3;
            return ((num3 * num3) / ((num * num) + (num2 * num2)));
        }

        private static double DistanceSqrd(IntPoint pt1, IntPoint pt2)
        {
            double num = pt1.X - pt2.X;
            double num2 = pt1.Y - pt2.Y;
            return ((num * num) + (num2 * num2));
        }

        private void DoMaxima(TEdge e)
        {
            TEdge maximaPair = this.GetMaximaPair(e);
            if (maximaPair == null)
            {
                if (e.OutIdx >= 0)
                {
                    this.AddOutPt(e, e.Top);
                }
                this.DeleteFromAEL(e);
            }
            else
            {
                for (TEdge edge2 = e.NextInAEL; (edge2 != null) && (edge2 != maximaPair); edge2 = e.NextInAEL)
                {
                    this.IntersectEdges(e, edge2, e.Top);
                    this.SwapPositionsInAEL(e, edge2);
                }
                if ((e.OutIdx == -1) && (maximaPair.OutIdx == -1))
                {
                    this.DeleteFromAEL(e);
                    this.DeleteFromAEL(maximaPair);
                }
                else
                {
                    if ((e.OutIdx < 0) || (maximaPair.OutIdx < 0))
                    {
                        throw new ClipperException("DoMaxima error");
                    }
                    if (e.OutIdx >= 0)
                    {
                        this.AddLocalMaxPoly(e, maximaPair, e.Top);
                    }
                    this.DeleteFromAEL(e);
                    this.DeleteFromAEL(maximaPair);
                }
            }
        }

        private void DoSimplePolygons()
        {
            int num = 0;
            while (num < this.m_PolyOuts.Count)
            {
                OutRec newOutRec = this.m_PolyOuts[num++];
                OutPt pts = newOutRec.Pts;
                if ((pts != null) && !newOutRec.IsOpen)
                {
                    do
                    {
                        for (OutPt pt2 = pts.Next; pt2 != newOutRec.Pts; pt2 = pt2.Next)
                        {
                            if (((pts.Pt == pt2.Pt) && (pt2.Next != pts)) && (pt2.Prev != pts))
                            {
                                OutPt prev = pts.Prev;
                                OutPt pt4 = pt2.Prev;
                                pts.Prev = pt4;
                                pt4.Next = pts;
                                pt2.Prev = prev;
                                prev.Next = pt2;
                                newOutRec.Pts = pts;
                                OutRec outrec = this.CreateOutRec();
                                outrec.Pts = pt2;
                                this.UpdateOutPtIdxs(outrec);
                                if (Poly2ContainsPoly1(outrec.Pts, newOutRec.Pts))
                                {
                                    outrec.IsHole = !newOutRec.IsHole;
                                    outrec.FirstLeft = newOutRec;
                                    if (this.m_UsingPolyTree)
                                    {
                                        this.FixupFirstLefts2(outrec, newOutRec);
                                    }
                                }
                                else if (Poly2ContainsPoly1(newOutRec.Pts, outrec.Pts))
                                {
                                    outrec.IsHole = newOutRec.IsHole;
                                    newOutRec.IsHole = !outrec.IsHole;
                                    outrec.FirstLeft = newOutRec.FirstLeft;
                                    newOutRec.FirstLeft = outrec;
                                    if (this.m_UsingPolyTree)
                                    {
                                        this.FixupFirstLefts2(newOutRec, outrec);
                                    }
                                }
                                else
                                {
                                    outrec.IsHole = newOutRec.IsHole;
                                    outrec.FirstLeft = newOutRec.FirstLeft;
                                    if (this.m_UsingPolyTree)
                                    {
                                        this.FixupFirstLefts1(newOutRec, outrec);
                                    }
                                }
                                pt2 = pts;
                            }
                        }
                        pts = pts.Next;
                    }
                    while (pts != newOutRec.Pts);
                }
            }
        }

        private OutPt DupOutPt(OutPt outPt, bool InsertAfter)
        {
            OutPt pt = new OutPt {
                Pt = outPt.Pt,
                Idx = outPt.Idx
            };
            if (InsertAfter)
            {
                pt.Next = outPt.Next;
                pt.Prev = outPt;
                outPt.Next.Prev = pt;
                outPt.Next = pt;
                return pt;
            }
            pt.Prev = outPt.Prev;
            pt.Next = outPt;
            outPt.Prev.Next = pt;
            outPt.Prev = pt;
            return pt;
        }

        private bool E2InsertsBeforeE1(TEdge e1, TEdge e2)
        {
            if (e2.Curr.X == e1.Curr.X)
            {
                if (e2.Top.Y > e1.Top.Y)
                {
                    return (e2.Top.X < TopX(e1, e2.Top.Y));
                }
                return (e1.Top.X > TopX(e2, e1.Top.Y));
            }
            return (e2.Curr.X < e1.Curr.X);
        }

        private bool EdgesAdjacent(IntersectNode inode) => 
            ((inode.Edge1.NextInSEL == inode.Edge2) || (inode.Edge1.PrevInSEL == inode.Edge2));

        private static OutPt ExcludeOp(OutPt op)
        {
            OutPt prev = op.Prev;
            prev.Next = op.Next;
            op.Next.Prev = prev;
            prev.Idx = 0;
            return prev;
        }

        public bool Execute(ClipType clipType, PolyTree polytree) => 
            this.Execute(clipType, polytree, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);

        public bool Execute(ClipType clipType, List<List<IntPoint>> solution) => 
            this.Execute(clipType, solution, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);

        public bool Execute(ClipType clipType, PolyTree polytree, PolyFillType subjFillType, PolyFillType clipFillType)
        {
            bool flag;
            if (this.m_ExecuteLocked)
            {
                return false;
            }
            this.m_ExecuteLocked = true;
            this.m_SubjFillType = subjFillType;
            this.m_ClipFillType = clipFillType;
            this.m_ClipType = clipType;
            this.m_UsingPolyTree = true;
            try
            {
                flag = this.ExecuteInternal();
                if (flag)
                {
                    this.BuildResult2(polytree);
                }
            }
            finally
            {
                this.DisposeAllPolyPts();
                this.m_ExecuteLocked = false;
            }
            return flag;
        }

        public bool Execute(ClipType clipType, List<List<IntPoint>> solution, PolyFillType subjFillType, PolyFillType clipFillType)
        {
            bool flag;
            if (this.m_ExecuteLocked)
            {
                return false;
            }
            if (base.m_HasOpenPaths)
            {
                throw new ClipperException("Error: PolyTree struct is need for open path clipping.");
            }
            this.m_ExecuteLocked = true;
            solution.Clear();
            this.m_SubjFillType = subjFillType;
            this.m_ClipFillType = clipFillType;
            this.m_ClipType = clipType;
            this.m_UsingPolyTree = false;
            try
            {
                flag = this.ExecuteInternal();
                if (flag)
                {
                    this.BuildResult(solution);
                }
            }
            finally
            {
                this.DisposeAllPolyPts();
                this.m_ExecuteLocked = false;
            }
            return flag;
        }

        private bool ExecuteInternal()
        {
            bool flag2;
            try
            {
                this.Reset();
                if (base.m_CurrentLM == null)
                {
                    return false;
                }
                long botY = this.PopScanbeam();
                do
                {
                    this.InsertLocalMinimaIntoAEL(botY);
                    this.m_GhostJoins.Clear();
                    this.ProcessHorizontals(false);
                    if (this.m_Scanbeam == null)
                    {
                        break;
                    }
                    long topY = this.PopScanbeam();
                    if (!this.ProcessIntersections(topY))
                    {
                        return false;
                    }
                    this.ProcessEdgesAtTopOfScanbeam(topY);
                    botY = topY;
                }
                while ((this.m_Scanbeam != null) || (base.m_CurrentLM > null));
                for (int i = 0; i < this.m_PolyOuts.Count; i++)
                {
                    OutRec outRec = this.m_PolyOuts[i];
                    if (((outRec.Pts != null) && !outRec.IsOpen) && ((outRec.IsHole ^ this.ReverseSolution) == (this.Area(outRec) > 0.0)))
                    {
                        this.ReversePolyPtLinks(outRec.Pts);
                    }
                }
                this.JoinCommonEdges();
                for (int j = 0; j < this.m_PolyOuts.Count; j++)
                {
                    OutRec rec2 = this.m_PolyOuts[j];
                    if ((rec2.Pts != null) && !rec2.IsOpen)
                    {
                        this.FixupOutPolygon(rec2);
                    }
                }
                if (this.StrictlySimple)
                {
                    this.DoSimplePolygons();
                }
                flag2 = true;
            }
            finally
            {
                this.m_Joins.Clear();
                this.m_GhostJoins.Clear();
            }
            return flag2;
        }

        private bool FirstIsBottomPt(OutPt btmPt1, OutPt btmPt2)
        {
            OutPt prev = btmPt1.Prev;
            while ((prev.Pt == btmPt1.Pt) && (prev != btmPt1))
            {
                prev = prev.Prev;
            }
            double num = Math.Abs(this.GetDx(btmPt1.Pt, prev.Pt));
            prev = btmPt1.Next;
            while ((prev.Pt == btmPt1.Pt) && (prev != btmPt1))
            {
                prev = prev.Next;
            }
            double num2 = Math.Abs(this.GetDx(btmPt1.Pt, prev.Pt));
            prev = btmPt2.Prev;
            while ((prev.Pt == btmPt2.Pt) && (prev != btmPt2))
            {
                prev = prev.Prev;
            }
            double num3 = Math.Abs(this.GetDx(btmPt2.Pt, prev.Pt));
            prev = btmPt2.Next;
            while ((prev.Pt == btmPt2.Pt) && (prev != btmPt2))
            {
                prev = prev.Next;
            }
            double num4 = Math.Abs(this.GetDx(btmPt2.Pt, prev.Pt));
            return (((num >= num3) && (num >= num4)) || ((num2 >= num3) && (num2 >= num4)));
        }

        internal void FixHoleLinkage(OutRec outRec)
        {
            if ((outRec.FirstLeft != null) && ((outRec.IsHole == outRec.FirstLeft.IsHole) || (outRec.FirstLeft.Pts <= null)))
            {
                OutRec firstLeft = outRec.FirstLeft;
                while ((firstLeft != null) && ((firstLeft.IsHole == outRec.IsHole) || (firstLeft.Pts == null)))
                {
                    firstLeft = firstLeft.FirstLeft;
                }
                outRec.FirstLeft = firstLeft;
            }
        }

        private void FixupFirstLefts1(OutRec OldOutRec, OutRec NewOutRec)
        {
            for (int i = 0; i < this.m_PolyOuts.Count; i++)
            {
                OutRec rec = this.m_PolyOuts[i];
                if ((((rec.Pts != null) && (rec.FirstLeft != null)) && (ParseFirstLeft(rec.FirstLeft) == OldOutRec)) && Poly2ContainsPoly1(rec.Pts, NewOutRec.Pts))
                {
                    rec.FirstLeft = NewOutRec;
                }
            }
        }

        private void FixupFirstLefts2(OutRec OldOutRec, OutRec NewOutRec)
        {
            foreach (OutRec rec in this.m_PolyOuts)
            {
                if (rec.FirstLeft == OldOutRec)
                {
                    rec.FirstLeft = NewOutRec;
                }
            }
        }

        private bool FixupIntersectionOrder()
        {
            this.m_IntersectList.Sort(this.m_IntersectNodeComparer);
            this.CopyAELToSEL();
            int count = this.m_IntersectList.Count;
            for (int i = 0; i < count; i++)
            {
                if (!this.EdgesAdjacent(this.m_IntersectList[i]))
                {
                    int num3 = i + 1;
                    while ((num3 < count) && !this.EdgesAdjacent(this.m_IntersectList[num3]))
                    {
                        num3++;
                    }
                    if (num3 == count)
                    {
                        return false;
                    }
                    IntersectNode node = this.m_IntersectList[i];
                    this.m_IntersectList[i] = this.m_IntersectList[num3];
                    this.m_IntersectList[num3] = node;
                }
                this.SwapPositionsInSEL(this.m_IntersectList[i].Edge1, this.m_IntersectList[i].Edge2);
            }
            return true;
        }

        private void FixupOutPolygon(OutRec outRec)
        {
            OutPt pt = null;
            outRec.BottomPt = null;
            OutPt pts = outRec.Pts;
        Label_0016:
            if ((pts.Prev == pts) || (pts.Prev == pts.Next))
            {
                outRec.Pts = null;
            }
            else
            {
                if (((pts.Pt == pts.Next.Pt) || (pts.Pt == pts.Prev.Pt)) || (ClipperBase.SlopesEqual(pts.Prev.Pt, pts.Pt, pts.Next.Pt, base.m_UseFullRange) && (!base.PreserveCollinear || !base.Pt2IsBetweenPt1AndPt3(pts.Prev.Pt, pts.Pt, pts.Next.Pt))))
                {
                    pt = null;
                    pts.Prev.Next = pts.Next;
                    pts.Next.Prev = pts.Prev;
                    pts = pts.Prev;
                    goto Label_0016;
                }
                if (pts != pt)
                {
                    if (pt == null)
                    {
                        pt = pts;
                    }
                    pts = pts.Next;
                    goto Label_0016;
                }
                outRec.Pts = pts;
            }
        }

        private OutPt GetBottomPt(OutPt pp)
        {
            OutPt pt = null;
            OutPt next = pp.Next;
            while (next != pp)
            {
                if (next.Pt.Y > pp.Pt.Y)
                {
                    pp = next;
                    pt = null;
                }
                else if ((next.Pt.Y == pp.Pt.Y) && (next.Pt.X <= pp.Pt.X))
                {
                    if (next.Pt.X < pp.Pt.X)
                    {
                        pt = null;
                        pp = next;
                    }
                    else if ((next.Next != pp) && (next.Prev != pp))
                    {
                        pt = next;
                    }
                }
                next = next.Next;
            }
            if (pt > null)
            {
                while (pt != next)
                {
                    if (!this.FirstIsBottomPt(next, pt))
                    {
                        pp = pt;
                    }
                    for (pt = pt.Next; pt.Pt != pp.Pt; pt = pt.Next)
                    {
                    }
                }
            }
            return pp;
        }

        private double GetDx(IntPoint pt1, IntPoint pt2)
        {
            if (pt1.Y == pt2.Y)
            {
                return -3.4E+38;
            }
            return (((double) (pt2.X - pt1.X)) / ((double) (pt2.Y - pt1.Y)));
        }

        private void GetHorzDirection(TEdge HorzEdge, out Direction Dir, out long Left, out long Right)
        {
            if (HorzEdge.Bot.X < HorzEdge.Top.X)
            {
                Left = HorzEdge.Bot.X;
                Right = HorzEdge.Top.X;
                Dir = Direction.dLeftToRight;
            }
            else
            {
                Left = HorzEdge.Top.X;
                Right = HorzEdge.Bot.X;
                Dir = Direction.dRightToLeft;
            }
        }

        private OutRec GetLowermostRec(OutRec outRec1, OutRec outRec2)
        {
            if (outRec1.BottomPt == null)
            {
                outRec1.BottomPt = this.GetBottomPt(outRec1.Pts);
            }
            if (outRec2.BottomPt == null)
            {
                outRec2.BottomPt = this.GetBottomPt(outRec2.Pts);
            }
            OutPt bottomPt = outRec1.BottomPt;
            OutPt pt2 = outRec2.BottomPt;
            if (bottomPt.Pt.Y > pt2.Pt.Y)
            {
                return outRec1;
            }
            if (bottomPt.Pt.Y >= pt2.Pt.Y)
            {
                if (bottomPt.Pt.X < pt2.Pt.X)
                {
                    return outRec1;
                }
                if (bottomPt.Pt.X > pt2.Pt.X)
                {
                    return outRec2;
                }
                if (bottomPt.Next == bottomPt)
                {
                    return outRec2;
                }
                if (pt2.Next == pt2)
                {
                    return outRec1;
                }
                if (this.FirstIsBottomPt(bottomPt, pt2))
                {
                    return outRec1;
                }
            }
            return outRec2;
        }

        private TEdge GetMaximaPair(TEdge e)
        {
            TEdge next = null;
            if ((e.Next.Top == e.Top) && (e.Next.NextInLML == null))
            {
                next = e.Next;
            }
            else if ((e.Prev.Top == e.Top) && (e.Prev.NextInLML == null))
            {
                next = e.Prev;
            }
            if ((next != null) && ((next.OutIdx == -2) || ((next.NextInAEL == next.PrevInAEL) && !ClipperBase.IsHorizontal(next))))
            {
                return null;
            }
            return next;
        }

        private TEdge GetNextInAEL(TEdge e, Direction Direction) => 
            ((Direction == Direction.dLeftToRight) ? e.NextInAEL : e.PrevInAEL);

        private OutRec GetOutRec(int idx)
        {
            OutRec rec = this.m_PolyOuts[idx];
            while (rec != this.m_PolyOuts[rec.Idx])
            {
                rec = this.m_PolyOuts[rec.Idx];
            }
            return rec;
        }

        private bool GetOverlap(long a1, long a2, long b1, long b2, out long Left, out long Right)
        {
            if (a1 < a2)
            {
                if (b1 < b2)
                {
                    Left = Math.Max(a1, b1);
                    Right = Math.Min(a2, b2);
                }
                else
                {
                    Left = Math.Max(a1, b2);
                    Right = Math.Min(a2, b1);
                }
            }
            else if (b1 < b2)
            {
                Left = Math.Max(a2, b1);
                Right = Math.Min(a1, b2);
            }
            else
            {
                Left = Math.Max(a2, b2);
                Right = Math.Min(a1, b1);
            }
            return (Left < Right);
        }

        private bool HorzSegmentsOverlap(long seg1a, long seg1b, long seg2a, long seg2b)
        {
            if (seg1a > seg1b)
            {
                base.Swap(ref seg1a, ref seg1b);
            }
            if (seg2a > seg2b)
            {
                base.Swap(ref seg2a, ref seg2b);
            }
            return ((seg1a < seg2b) && (seg2a < seg1b));
        }

        private void InsertEdgeIntoAEL(TEdge edge, TEdge startEdge)
        {
            if (this.m_ActiveEdges == null)
            {
                edge.PrevInAEL = null;
                edge.NextInAEL = null;
                this.m_ActiveEdges = edge;
            }
            else if ((startEdge == null) && this.E2InsertsBeforeE1(this.m_ActiveEdges, edge))
            {
                edge.PrevInAEL = null;
                edge.NextInAEL = this.m_ActiveEdges;
                this.m_ActiveEdges.PrevInAEL = edge;
                this.m_ActiveEdges = edge;
            }
            else
            {
                if (startEdge == null)
                {
                    startEdge = this.m_ActiveEdges;
                }
                while ((startEdge.NextInAEL != null) && !this.E2InsertsBeforeE1(startEdge.NextInAEL, edge))
                {
                    startEdge = startEdge.NextInAEL;
                }
                edge.NextInAEL = startEdge.NextInAEL;
                if (startEdge.NextInAEL > null)
                {
                    startEdge.NextInAEL.PrevInAEL = edge;
                }
                edge.PrevInAEL = startEdge;
                startEdge.NextInAEL = edge;
            }
        }

        private void InsertLocalMinimaIntoAEL(long botY)
        {
            while ((base.m_CurrentLM != null) && (base.m_CurrentLM.Y == botY))
            {
                TEdge leftBound = base.m_CurrentLM.LeftBound;
                TEdge rightBound = base.m_CurrentLM.RightBound;
                base.PopLocalMinima();
                OutPt pt = null;
                if (leftBound == null)
                {
                    this.InsertEdgeIntoAEL(rightBound, null);
                    this.SetWindingCount(rightBound);
                    if (this.IsContributing(rightBound))
                    {
                        pt = this.AddOutPt(rightBound, rightBound.Bot);
                    }
                }
                else if (rightBound == null)
                {
                    this.InsertEdgeIntoAEL(leftBound, null);
                    this.SetWindingCount(leftBound);
                    if (this.IsContributing(leftBound))
                    {
                        pt = this.AddOutPt(leftBound, leftBound.Bot);
                    }
                    this.InsertScanbeam(leftBound.Top.Y);
                }
                else
                {
                    this.InsertEdgeIntoAEL(leftBound, null);
                    this.InsertEdgeIntoAEL(rightBound, leftBound);
                    this.SetWindingCount(leftBound);
                    rightBound.WindCnt = leftBound.WindCnt;
                    rightBound.WindCnt2 = leftBound.WindCnt2;
                    if (this.IsContributing(leftBound))
                    {
                        pt = this.AddLocalMinPoly(leftBound, rightBound, leftBound.Bot);
                    }
                    this.InsertScanbeam(leftBound.Top.Y);
                }
                if (rightBound > null)
                {
                    if (ClipperBase.IsHorizontal(rightBound))
                    {
                        this.AddEdgeToSEL(rightBound);
                    }
                    else
                    {
                        this.InsertScanbeam(rightBound.Top.Y);
                    }
                }
                if ((leftBound != null) && (rightBound != null))
                {
                    if ((((pt != null) && ClipperBase.IsHorizontal(rightBound)) && (this.m_GhostJoins.Count > 0)) && (rightBound.WindDelta > 0))
                    {
                        for (int i = 0; i < this.m_GhostJoins.Count; i++)
                        {
                            Join join = this.m_GhostJoins[i];
                            if (this.HorzSegmentsOverlap(join.OutPt1.Pt.X, join.OffPt.X, rightBound.Bot.X, rightBound.Top.X))
                            {
                                this.AddJoin(join.OutPt1, pt, join.OffPt);
                            }
                        }
                    }
                    if (((((leftBound.OutIdx >= 0) && (leftBound.PrevInAEL != null)) && ((leftBound.PrevInAEL.Curr.X == leftBound.Bot.X) && (leftBound.PrevInAEL.OutIdx >= 0))) && (ClipperBase.SlopesEqual(leftBound.PrevInAEL, leftBound, base.m_UseFullRange) && (leftBound.WindDelta != 0))) && (leftBound.PrevInAEL.WindDelta > 0))
                    {
                        OutPt pt2 = this.AddOutPt(leftBound.PrevInAEL, leftBound.Bot);
                        this.AddJoin(pt, pt2, leftBound.Top);
                    }
                    if (leftBound.NextInAEL != rightBound)
                    {
                        if ((((rightBound.OutIdx >= 0) && (rightBound.PrevInAEL.OutIdx >= 0)) && (ClipperBase.SlopesEqual(rightBound.PrevInAEL, rightBound, base.m_UseFullRange) && (rightBound.WindDelta != 0))) && (rightBound.PrevInAEL.WindDelta > 0))
                        {
                            OutPt pt3 = this.AddOutPt(rightBound.PrevInAEL, rightBound.Bot);
                            this.AddJoin(pt, pt3, rightBound.Top);
                        }
                        TEdge nextInAEL = leftBound.NextInAEL;
                        if (nextInAEL > null)
                        {
                            while (nextInAEL != rightBound)
                            {
                                this.IntersectEdges(rightBound, nextInAEL, leftBound.Curr);
                                nextInAEL = nextInAEL.NextInAEL;
                            }
                        }
                    }
                }
            }
        }

        private void InsertScanbeam(long Y)
        {
            if (this.m_Scanbeam == null)
            {
                this.m_Scanbeam = new Scanbeam();
                this.m_Scanbeam.Next = null;
                this.m_Scanbeam.Y = Y;
            }
            else if (Y > this.m_Scanbeam.Y)
            {
                Scanbeam scanbeam = new Scanbeam {
                    Y = Y,
                    Next = this.m_Scanbeam
                };
                this.m_Scanbeam = scanbeam;
            }
            else
            {
                Scanbeam next = this.m_Scanbeam;
                while ((next.Next != null) && (Y <= next.Next.Y))
                {
                    next = next.Next;
                }
                if (Y != next.Y)
                {
                    Scanbeam scanbeam3 = new Scanbeam {
                        Y = Y,
                        Next = next.Next
                    };
                    next.Next = scanbeam3;
                }
            }
        }

        private void IntersectEdges(TEdge e1, TEdge e2, IntPoint pt)
        {
            PolyFillType subjFillType;
            PolyFillType type2;
            PolyFillType clipFillType;
            PolyFillType type4;
            int num;
            int num2;
            bool flag = e1.OutIdx >= 0;
            bool flag2 = e2.OutIdx >= 0;
            if (e1.PolyTyp == e2.PolyTyp)
            {
                if (this.IsEvenOddFillType(e1))
                {
                    int windCnt = e1.WindCnt;
                    e1.WindCnt = e2.WindCnt;
                    e2.WindCnt = windCnt;
                }
                else
                {
                    if ((e1.WindCnt + e2.WindDelta) == 0)
                    {
                        e1.WindCnt = -e1.WindCnt;
                    }
                    else
                    {
                        e1.WindCnt += e2.WindDelta;
                    }
                    if ((e2.WindCnt - e1.WindDelta) == 0)
                    {
                        e2.WindCnt = -e2.WindCnt;
                    }
                    else
                    {
                        e2.WindCnt -= e1.WindDelta;
                    }
                }
            }
            else
            {
                if (!this.IsEvenOddFillType(e2))
                {
                    e1.WindCnt2 += e2.WindDelta;
                }
                else
                {
                    e1.WindCnt2 = (e1.WindCnt2 == 0) ? 1 : 0;
                }
                if (!this.IsEvenOddFillType(e1))
                {
                    e2.WindCnt2 -= e1.WindDelta;
                }
                else
                {
                    e2.WindCnt2 = (e2.WindCnt2 == 0) ? 1 : 0;
                }
            }
            if (e1.PolyTyp == PolyType.ptSubject)
            {
                subjFillType = this.m_SubjFillType;
                clipFillType = this.m_ClipFillType;
            }
            else
            {
                subjFillType = this.m_ClipFillType;
                clipFillType = this.m_SubjFillType;
            }
            if (e2.PolyTyp == PolyType.ptSubject)
            {
                type2 = this.m_SubjFillType;
                type4 = this.m_ClipFillType;
            }
            else
            {
                type2 = this.m_ClipFillType;
                type4 = this.m_SubjFillType;
            }
            switch (subjFillType)
            {
                case PolyFillType.pftPositive:
                    num = e1.WindCnt;
                    break;

                case PolyFillType.pftNegative:
                    num = -e1.WindCnt;
                    break;

                default:
                    num = Math.Abs(e1.WindCnt);
                    break;
            }
            switch (type2)
            {
                case PolyFillType.pftPositive:
                    num2 = e2.WindCnt;
                    break;

                case PolyFillType.pftNegative:
                    num2 = -e2.WindCnt;
                    break;

                default:
                    num2 = Math.Abs(e2.WindCnt);
                    break;
            }
            if (flag & flag2)
            {
                if ((((num != 0) && (num != 1)) || ((num2 != 0) && (num2 != 1))) || ((e1.PolyTyp != e2.PolyTyp) && (this.m_ClipType != ClipType.ctXor)))
                {
                    this.AddLocalMaxPoly(e1, e2, pt);
                }
                else
                {
                    this.AddOutPt(e1, pt);
                    this.AddOutPt(e2, pt);
                    SwapSides(e1, e2);
                    SwapPolyIndexes(e1, e2);
                }
            }
            else if (flag)
            {
                switch (num2)
                {
                    case 0:
                    case 1:
                        this.AddOutPt(e1, pt);
                        SwapSides(e1, e2);
                        SwapPolyIndexes(e1, e2);
                        break;
                }
            }
            else if (flag2)
            {
                switch (num)
                {
                    case 0:
                    case 1:
                        this.AddOutPt(e2, pt);
                        SwapSides(e1, e2);
                        SwapPolyIndexes(e1, e2);
                        break;
                }
            }
            else if (((num == 0) || (num == 1)) && ((num2 == 0) || (num2 == 1)))
            {
                long num4;
                long num5;
                switch (clipFillType)
                {
                    case PolyFillType.pftPositive:
                        num4 = e1.WindCnt2;
                        break;

                    case PolyFillType.pftNegative:
                        num4 = -e1.WindCnt2;
                        break;

                    default:
                        num4 = Math.Abs(e1.WindCnt2);
                        break;
                }
                switch (type4)
                {
                    case PolyFillType.pftPositive:
                        num5 = e2.WindCnt2;
                        break;

                    case PolyFillType.pftNegative:
                        num5 = -e2.WindCnt2;
                        break;

                    default:
                        num5 = Math.Abs(e2.WindCnt2);
                        break;
                }
                if (e1.PolyTyp != e2.PolyTyp)
                {
                    this.AddLocalMinPoly(e1, e2, pt);
                }
                else if ((num == 1) && (num2 == 1))
                {
                    switch (this.m_ClipType)
                    {
                        case ClipType.ctIntersection:
                            if ((num4 > 0L) && (num5 > 0L))
                            {
                                this.AddLocalMinPoly(e1, e2, pt);
                            }
                            break;

                        case ClipType.ctUnion:
                            if ((num4 <= 0L) && (num5 <= 0L))
                            {
                                this.AddLocalMinPoly(e1, e2, pt);
                            }
                            break;

                        case ClipType.ctDifference:
                            if ((((e1.PolyTyp == PolyType.ptClip) && (num4 > 0L)) && (num5 > 0L)) || (((e1.PolyTyp == PolyType.ptSubject) && (num4 <= 0L)) && (num5 <= 0L)))
                            {
                                this.AddLocalMinPoly(e1, e2, pt);
                            }
                            break;

                        case ClipType.ctXor:
                            this.AddLocalMinPoly(e1, e2, pt);
                            break;
                    }
                }
                else
                {
                    SwapSides(e1, e2);
                }
            }
        }

        private static int IntersectNodeSort(IntersectNode node1, IntersectNode node2) => 
            ((int) (node2.Pt.Y - node1.Pt.Y));

        private void IntersectPoint(TEdge edge1, TEdge edge2, out IntPoint ip)
        {
            ip = new IntPoint();
            if (edge1.Dx == edge2.Dx)
            {
                ip.Y = edge1.Curr.Y;
                ip.X = TopX(edge1, ip.Y);
            }
            else
            {
                double num2;
                if (edge1.Delta.X == 0L)
                {
                    ip.X = edge1.Bot.X;
                    if (ClipperBase.IsHorizontal(edge2))
                    {
                        ip.Y = edge2.Bot.Y;
                    }
                    else
                    {
                        num2 = edge2.Bot.Y - (((double) edge2.Bot.X) / edge2.Dx);
                        ip.Y = Round((((double) ip.X) / edge2.Dx) + num2);
                    }
                }
                else
                {
                    double num;
                    if (edge2.Delta.X == 0L)
                    {
                        ip.X = edge2.Bot.X;
                        if (ClipperBase.IsHorizontal(edge1))
                        {
                            ip.Y = edge1.Bot.Y;
                        }
                        else
                        {
                            num = edge1.Bot.Y - (((double) edge1.Bot.X) / edge1.Dx);
                            ip.Y = Round((((double) ip.X) / edge1.Dx) + num);
                        }
                    }
                    else
                    {
                        num = edge1.Bot.X - (edge1.Bot.Y * edge1.Dx);
                        num2 = edge2.Bot.X - (edge2.Bot.Y * edge2.Dx);
                        double num3 = (num2 - num) / (edge1.Dx - edge2.Dx);
                        ip.Y = Round(num3);
                        if (Math.Abs(edge1.Dx) < Math.Abs(edge2.Dx))
                        {
                            ip.X = Round((edge1.Dx * num3) + num);
                        }
                        else
                        {
                            ip.X = Round((edge2.Dx * num3) + num2);
                        }
                    }
                }
                if ((ip.Y < edge1.Top.Y) || (ip.Y < edge2.Top.Y))
                {
                    if (edge1.Top.Y > edge2.Top.Y)
                    {
                        ip.Y = edge1.Top.Y;
                    }
                    else
                    {
                        ip.Y = edge2.Top.Y;
                    }
                    if (Math.Abs(edge1.Dx) < Math.Abs(edge2.Dx))
                    {
                        ip.X = TopX(edge1, ip.Y);
                    }
                    else
                    {
                        ip.X = TopX(edge2, ip.Y);
                    }
                }
                if (ip.Y > edge1.Curr.Y)
                {
                    ip.Y = edge1.Curr.Y;
                    if (Math.Abs(edge1.Dx) > Math.Abs(edge2.Dx))
                    {
                        ip.X = TopX(edge2, ip.Y);
                    }
                    else
                    {
                        ip.X = TopX(edge1, ip.Y);
                    }
                }
            }
        }

        private bool IsContributing(TEdge edge)
        {
            PolyFillType subjFillType;
            PolyFillType clipFillType;
            if (edge.PolyTyp == PolyType.ptSubject)
            {
                subjFillType = this.m_SubjFillType;
                clipFillType = this.m_ClipFillType;
            }
            else
            {
                subjFillType = this.m_ClipFillType;
                clipFillType = this.m_SubjFillType;
            }
            switch (subjFillType)
            {
                case PolyFillType.pftEvenOdd:
                    if ((edge.WindDelta != 0) || (edge.WindCnt == 1))
                    {
                        break;
                    }
                    return false;

                case PolyFillType.pftNonZero:
                    if (Math.Abs(edge.WindCnt) == 1)
                    {
                        break;
                    }
                    return false;

                case PolyFillType.pftPositive:
                    if (edge.WindCnt == 1)
                    {
                        break;
                    }
                    return false;

                default:
                    if (edge.WindCnt != -1)
                    {
                        return false;
                    }
                    break;
            }
            switch (this.m_ClipType)
            {
                case ClipType.ctIntersection:
                    switch (clipFillType)
                    {
                        case PolyFillType.pftEvenOdd:
                        case PolyFillType.pftNonZero:
                            return (edge.WindCnt2 > 0);

                        case PolyFillType.pftPositive:
                            return (edge.WindCnt2 > 0);
                    }
                    return (edge.WindCnt2 < 0);

                case ClipType.ctUnion:
                    switch (clipFillType)
                    {
                        case PolyFillType.pftEvenOdd:
                        case PolyFillType.pftNonZero:
                            return (edge.WindCnt2 == 0);

                        case PolyFillType.pftPositive:
                            return (edge.WindCnt2 <= 0);
                    }
                    return (edge.WindCnt2 >= 0);

                case ClipType.ctDifference:
                    if (edge.PolyTyp != PolyType.ptSubject)
                    {
                        switch (clipFillType)
                        {
                            case PolyFillType.pftEvenOdd:
                            case PolyFillType.pftNonZero:
                                return (edge.WindCnt2 > 0);

                            case PolyFillType.pftPositive:
                                return (edge.WindCnt2 > 0);
                        }
                        return (edge.WindCnt2 < 0);
                    }
                    switch (clipFillType)
                    {
                        case PolyFillType.pftEvenOdd:
                        case PolyFillType.pftNonZero:
                            return (edge.WindCnt2 == 0);

                        case PolyFillType.pftPositive:
                            return (edge.WindCnt2 <= 0);
                    }
                    return (edge.WindCnt2 >= 0);

                case ClipType.ctXor:
                    if (edge.WindDelta != 0)
                    {
                        return true;
                    }
                    switch (clipFillType)
                    {
                        case PolyFillType.pftEvenOdd:
                        case PolyFillType.pftNonZero:
                            return (edge.WindCnt2 == 0);

                        case PolyFillType.pftPositive:
                            return (edge.WindCnt2 <= 0);
                    }
                    return (edge.WindCnt2 >= 0);
            }
            return true;
        }

        private bool IsEvenOddAltFillType(TEdge edge)
        {
            if (edge.PolyTyp == PolyType.ptSubject)
            {
                return (this.m_ClipFillType == PolyFillType.pftEvenOdd);
            }
            return (this.m_SubjFillType == PolyFillType.pftEvenOdd);
        }

        private bool IsEvenOddFillType(TEdge edge)
        {
            if (edge.PolyTyp == PolyType.ptSubject)
            {
                return (this.m_SubjFillType == PolyFillType.pftEvenOdd);
            }
            return (this.m_ClipFillType == PolyFillType.pftEvenOdd);
        }

        private bool IsIntermediate(TEdge e, double Y) => 
            ((e.Top.Y == Y) && (e.NextInLML > null));

        private bool IsMaxima(TEdge e, double Y) => 
            (((e != null) && (e.Top.Y == Y)) && (e.NextInLML == null));

        private bool IsMinima(TEdge e) => 
            (((e != null) && (e.Prev.NextInLML != e)) && (e.Next.NextInLML != e));

        private void JoinCommonEdges()
        {
            for (int i = 0; i < this.m_Joins.Count; i++)
            {
                Join j = this.m_Joins[i];
                OutRec outRec = this.GetOutRec(j.OutPt1.Idx);
                OutRec rec2 = this.GetOutRec(j.OutPt2.Idx);
                if ((outRec.Pts != null) && (rec2.Pts != null))
                {
                    OutRec lowermostRec;
                    if (outRec == rec2)
                    {
                        lowermostRec = outRec;
                    }
                    else if (this.Param1RightOfParam2(outRec, rec2))
                    {
                        lowermostRec = rec2;
                    }
                    else if (this.Param1RightOfParam2(rec2, outRec))
                    {
                        lowermostRec = outRec;
                    }
                    else
                    {
                        lowermostRec = this.GetLowermostRec(outRec, rec2);
                    }
                    if (this.JoinPoints(j, outRec, rec2))
                    {
                        if (outRec == rec2)
                        {
                            outRec.Pts = j.OutPt1;
                            outRec.BottomPt = null;
                            rec2 = this.CreateOutRec();
                            rec2.Pts = j.OutPt2;
                            this.UpdateOutPtIdxs(rec2);
                            if (this.m_UsingPolyTree)
                            {
                                for (int k = 0; k < (this.m_PolyOuts.Count - 1); k++)
                                {
                                    OutRec rec4 = this.m_PolyOuts[k];
                                    if ((((rec4.Pts != null) && (ParseFirstLeft(rec4.FirstLeft) == outRec)) && (rec4.IsHole != outRec.IsHole)) && Poly2ContainsPoly1(rec4.Pts, j.OutPt2))
                                    {
                                        rec4.FirstLeft = rec2;
                                    }
                                }
                            }
                            if (Poly2ContainsPoly1(rec2.Pts, outRec.Pts))
                            {
                                rec2.IsHole = !outRec.IsHole;
                                rec2.FirstLeft = outRec;
                                if (this.m_UsingPolyTree)
                                {
                                    this.FixupFirstLefts2(rec2, outRec);
                                }
                                if ((rec2.IsHole ^ this.ReverseSolution) == (this.Area(rec2) > 0.0))
                                {
                                    this.ReversePolyPtLinks(rec2.Pts);
                                }
                            }
                            else if (Poly2ContainsPoly1(outRec.Pts, rec2.Pts))
                            {
                                rec2.IsHole = outRec.IsHole;
                                outRec.IsHole = !rec2.IsHole;
                                rec2.FirstLeft = outRec.FirstLeft;
                                outRec.FirstLeft = rec2;
                                if (this.m_UsingPolyTree)
                                {
                                    this.FixupFirstLefts2(outRec, rec2);
                                }
                                if ((outRec.IsHole ^ this.ReverseSolution) == (this.Area(outRec) > 0.0))
                                {
                                    this.ReversePolyPtLinks(outRec.Pts);
                                }
                            }
                            else
                            {
                                rec2.IsHole = outRec.IsHole;
                                rec2.FirstLeft = outRec.FirstLeft;
                                if (this.m_UsingPolyTree)
                                {
                                    this.FixupFirstLefts1(outRec, rec2);
                                }
                            }
                        }
                        else
                        {
                            rec2.Pts = null;
                            rec2.BottomPt = null;
                            rec2.Idx = outRec.Idx;
                            outRec.IsHole = lowermostRec.IsHole;
                            if (lowermostRec == rec2)
                            {
                                outRec.FirstLeft = rec2.FirstLeft;
                            }
                            rec2.FirstLeft = outRec;
                            if (this.m_UsingPolyTree)
                            {
                                this.FixupFirstLefts2(rec2, outRec);
                            }
                        }
                    }
                }
            }
        }

        private bool JoinHorz(OutPt op1, OutPt op1b, OutPt op2, OutPt op2b, IntPoint Pt, bool DiscardLeft)
        {
            Direction direction = (op1.Pt.X > op1b.Pt.X) ? Direction.dRightToLeft : Direction.dLeftToRight;
            Direction direction2 = (op2.Pt.X > op2b.Pt.X) ? Direction.dRightToLeft : Direction.dLeftToRight;
            if (direction == direction2)
            {
                return false;
            }
            if (direction != Direction.dLeftToRight)
            {
                while (((op1.Next.Pt.X >= Pt.X) && (op1.Next.Pt.X <= op1.Pt.X)) && (op1.Next.Pt.Y == Pt.Y))
                {
                    op1 = op1.Next;
                }
                if (!DiscardLeft && (op1.Pt.X != Pt.X))
                {
                    op1 = op1.Next;
                }
                op1b = this.DupOutPt(op1, DiscardLeft);
                if (op1b.Pt != Pt)
                {
                    op1 = op1b;
                    op1.Pt = Pt;
                    op1b = this.DupOutPt(op1, DiscardLeft);
                }
            }
            else
            {
                while (((op1.Next.Pt.X <= Pt.X) && (op1.Next.Pt.X >= op1.Pt.X)) && (op1.Next.Pt.Y == Pt.Y))
                {
                    op1 = op1.Next;
                }
                if (DiscardLeft && (op1.Pt.X != Pt.X))
                {
                    op1 = op1.Next;
                }
                op1b = this.DupOutPt(op1, !DiscardLeft);
                if (op1b.Pt != Pt)
                {
                    op1 = op1b;
                    op1.Pt = Pt;
                    op1b = this.DupOutPt(op1, !DiscardLeft);
                }
            }
            if (direction2 == Direction.dLeftToRight)
            {
                while (((op2.Next.Pt.X <= Pt.X) && (op2.Next.Pt.X >= op2.Pt.X)) && (op2.Next.Pt.Y == Pt.Y))
                {
                    op2 = op2.Next;
                }
                if (DiscardLeft && (op2.Pt.X != Pt.X))
                {
                    op2 = op2.Next;
                }
                op2b = this.DupOutPt(op2, !DiscardLeft);
                if (op2b.Pt != Pt)
                {
                    op2 = op2b;
                    op2.Pt = Pt;
                    op2b = this.DupOutPt(op2, !DiscardLeft);
                }
            }
            else
            {
                while (((op2.Next.Pt.X >= Pt.X) && (op2.Next.Pt.X <= op2.Pt.X)) && (op2.Next.Pt.Y == Pt.Y))
                {
                    op2 = op2.Next;
                }
                if (!DiscardLeft && (op2.Pt.X != Pt.X))
                {
                    op2 = op2.Next;
                }
                op2b = this.DupOutPt(op2, DiscardLeft);
                if (op2b.Pt != Pt)
                {
                    op2 = op2b;
                    op2.Pt = Pt;
                    op2b = this.DupOutPt(op2, DiscardLeft);
                }
            }
            if ((direction == Direction.dLeftToRight) == DiscardLeft)
            {
                op1.Prev = op2;
                op2.Next = op1;
                op1b.Next = op2b;
                op2b.Prev = op1b;
            }
            else
            {
                op1.Next = op2;
                op2.Prev = op1;
                op1b.Prev = op2b;
                op2b.Next = op1b;
            }
            return true;
        }

        private bool JoinPoints(Join j, OutRec outRec1, OutRec outRec2)
        {
            OutPt next;
            OutPt pt4;
            OutPt outPt = j.OutPt1;
            OutPt prev = j.OutPt2;
            bool flag = j.OutPt1.Pt.Y == j.OffPt.Y;
            if ((flag && (j.OffPt == j.OutPt1.Pt)) && (j.OffPt == j.OutPt2.Pt))
            {
                if (outRec1 != outRec2)
                {
                    return false;
                }
                next = j.OutPt1.Next;
                while ((next != outPt) && (next.Pt == j.OffPt))
                {
                    next = next.Next;
                }
                bool flag3 = next.Pt.Y > j.OffPt.Y;
                pt4 = j.OutPt2.Next;
                while ((pt4 != prev) && (pt4.Pt == j.OffPt))
                {
                    pt4 = pt4.Next;
                }
                bool flag4 = pt4.Pt.Y > j.OffPt.Y;
                if (flag3 == flag4)
                {
                    return false;
                }
                if (flag3)
                {
                    next = this.DupOutPt(outPt, false);
                    pt4 = this.DupOutPt(prev, true);
                    outPt.Prev = prev;
                    prev.Next = outPt;
                    next.Next = pt4;
                    pt4.Prev = next;
                    j.OutPt1 = outPt;
                    j.OutPt2 = next;
                    return true;
                }
                next = this.DupOutPt(outPt, true);
                pt4 = this.DupOutPt(prev, false);
                outPt.Next = prev;
                prev.Prev = outPt;
                next.Prev = pt4;
                pt4.Next = next;
                j.OutPt1 = outPt;
                j.OutPt2 = next;
                return true;
            }
            if (flag)
            {
                long num;
                long num2;
                IntPoint pt;
                bool flag12;
                next = outPt;
                while (((outPt.Prev.Pt.Y == outPt.Pt.Y) && (outPt.Prev != next)) && (outPt.Prev != prev))
                {
                    outPt = outPt.Prev;
                }
                while (((next.Next.Pt.Y == next.Pt.Y) && (next.Next != outPt)) && (next.Next != prev))
                {
                    next = next.Next;
                }
                if ((next.Next == outPt) || (next.Next == prev))
                {
                    return false;
                }
                pt4 = prev;
                while (((prev.Prev.Pt.Y == prev.Pt.Y) && (prev.Prev != pt4)) && (prev.Prev != next))
                {
                    prev = prev.Prev;
                }
                while (((pt4.Next.Pt.Y == pt4.Pt.Y) && (pt4.Next != prev)) && (pt4.Next != outPt))
                {
                    pt4 = pt4.Next;
                }
                if ((pt4.Next == prev) || (pt4.Next == outPt))
                {
                    return false;
                }
                if (!this.GetOverlap(outPt.Pt.X, next.Pt.X, prev.Pt.X, pt4.Pt.X, out num, out num2))
                {
                    return false;
                }
                if ((outPt.Pt.X >= num) && (outPt.Pt.X <= num2))
                {
                    pt = outPt.Pt;
                    flag12 = outPt.Pt.X > next.Pt.X;
                }
                else if ((prev.Pt.X >= num) && (prev.Pt.X <= num2))
                {
                    pt = prev.Pt;
                    flag12 = prev.Pt.X > pt4.Pt.X;
                }
                else if ((next.Pt.X >= num) && (next.Pt.X <= num2))
                {
                    pt = next.Pt;
                    flag12 = next.Pt.X > outPt.Pt.X;
                }
                else
                {
                    pt = pt4.Pt;
                    flag12 = pt4.Pt.X > prev.Pt.X;
                }
                j.OutPt1 = outPt;
                j.OutPt2 = prev;
                return this.JoinHorz(outPt, next, prev, pt4, pt, flag12);
            }
            next = outPt.Next;
            while ((next.Pt == outPt.Pt) && (next != outPt))
            {
                next = next.Next;
            }
            bool flag23 = (next.Pt.Y > outPt.Pt.Y) || !ClipperBase.SlopesEqual(outPt.Pt, next.Pt, j.OffPt, base.m_UseFullRange);
            if (flag23)
            {
                next = outPt.Prev;
                while ((next.Pt == outPt.Pt) && (next != outPt))
                {
                    next = next.Prev;
                }
                if ((next.Pt.Y > outPt.Pt.Y) || !ClipperBase.SlopesEqual(outPt.Pt, next.Pt, j.OffPt, base.m_UseFullRange))
                {
                    return false;
                }
            }
            pt4 = prev.Next;
            while ((pt4.Pt == prev.Pt) && (pt4 != prev))
            {
                pt4 = pt4.Next;
            }
            bool flag24 = (pt4.Pt.Y > prev.Pt.Y) || !ClipperBase.SlopesEqual(prev.Pt, pt4.Pt, j.OffPt, base.m_UseFullRange);
            if (flag24)
            {
                pt4 = prev.Prev;
                while ((pt4.Pt == prev.Pt) && (pt4 != prev))
                {
                    pt4 = pt4.Prev;
                }
                if ((pt4.Pt.Y > prev.Pt.Y) || !ClipperBase.SlopesEqual(prev.Pt, pt4.Pt, j.OffPt, base.m_UseFullRange))
                {
                    return false;
                }
            }
            if ((((next == outPt) || (pt4 == prev)) || (next == pt4)) || ((outRec1 == outRec2) && (flag23 == flag24)))
            {
                return false;
            }
            if (flag23)
            {
                next = this.DupOutPt(outPt, false);
                pt4 = this.DupOutPt(prev, true);
                outPt.Prev = prev;
                prev.Next = outPt;
                next.Next = pt4;
                pt4.Prev = next;
                j.OutPt1 = outPt;
                j.OutPt2 = next;
                return true;
            }
            next = this.DupOutPt(outPt, true);
            pt4 = this.DupOutPt(prev, false);
            outPt.Next = prev;
            prev.Prev = outPt;
            next.Prev = pt4;
            pt4.Next = next;
            j.OutPt1 = outPt;
            j.OutPt2 = next;
            return true;
        }

        internal static List<List<IntPoint>> Minkowski(List<IntPoint> pattern, List<IntPoint> path, bool IsSum, bool IsClosed)
        {
            int num = IsClosed ? 1 : 0;
            int count = pattern.Count;
            int capacity = path.Count;
            List<List<IntPoint>> list = new List<List<IntPoint>>(capacity);
            if (IsSum)
            {
                for (int j = 0; j < capacity; j++)
                {
                    List<IntPoint> item = new List<IntPoint>(count);
                    foreach (IntPoint point in pattern)
                    {
                        item.Add(new IntPoint(path[j].X + point.X, path[j].Y + point.Y));
                    }
                    list.Add(item);
                }
            }
            else
            {
                for (int k = 0; k < capacity; k++)
                {
                    List<IntPoint> list4 = new List<IntPoint>(count);
                    foreach (IntPoint point2 in pattern)
                    {
                        list4.Add(new IntPoint(path[k].X - point2.X, path[k].Y - point2.Y));
                    }
                    list.Add(list4);
                }
            }
            List<List<IntPoint>> list2 = new List<List<IntPoint>>((capacity + num) * (count + 1));
            for (int i = 0; i < ((capacity - 1) + num); i++)
            {
                for (int m = 0; m < count; m++)
                {
                    List<IntPoint> poly = new List<IntPoint>(4) {
                        list[i % capacity][m % count],
                        list[(i + 1) % capacity][m % count],
                        list[(i + 1) % capacity][(m + 1) % count],
                        list[i % capacity][(m + 1) % count]
                    };
                    if (!Orientation(poly))
                    {
                        poly.Reverse();
                    }
                    list2.Add(poly);
                }
            }
            return list2;
        }

        public static List<List<IntPoint>> MinkowskiDiff(List<IntPoint> poly1, List<IntPoint> poly2)
        {
            List<List<IntPoint>> ppg = Minkowski(poly1, poly2, false, true);
            Clipper clipper = new Clipper(0);
            clipper.AddPaths(ppg, PolyType.ptSubject, true);
            clipper.Execute(ClipType.ctUnion, ppg, PolyFillType.pftNonZero, PolyFillType.pftNonZero);
            return ppg;
        }

        public static List<List<IntPoint>> MinkowskiSum(List<IntPoint> pattern, List<IntPoint> path, bool pathIsClosed)
        {
            List<List<IntPoint>> ppg = Minkowski(pattern, path, true, pathIsClosed);
            Clipper clipper = new Clipper(0);
            clipper.AddPaths(ppg, PolyType.ptSubject, true);
            clipper.Execute(ClipType.ctUnion, ppg, PolyFillType.pftNonZero, PolyFillType.pftNonZero);
            return ppg;
        }

        public static List<List<IntPoint>> MinkowskiSum(List<IntPoint> pattern, List<List<IntPoint>> paths, bool pathIsClosed)
        {
            List<List<IntPoint>> solution = new List<List<IntPoint>>();
            Clipper clipper = new Clipper(0);
            for (int i = 0; i < paths.Count; i++)
            {
                List<List<IntPoint>> ppg = Minkowski(pattern, paths[i], true, pathIsClosed);
                clipper.AddPaths(ppg, PolyType.ptSubject, true);
                if (pathIsClosed)
                {
                    List<IntPoint> pg = TranslatePath(paths[i], pattern[0]);
                    clipper.AddPath(pg, PolyType.ptClip, true);
                }
            }
            clipper.Execute(ClipType.ctUnion, solution, PolyFillType.pftNonZero, PolyFillType.pftNonZero);
            return solution;
        }

        public static List<List<IntPoint>> OpenPathsFromPolyTree(PolyTree polytree)
        {
            List<List<IntPoint>> list = new List<List<IntPoint>> {
                Capacity = polytree.ChildCount
            };
            for (int i = 0; i < polytree.ChildCount; i++)
            {
                if (polytree.Childs[i].IsOpen)
                {
                    list.Add(polytree.Childs[i].m_polygon);
                }
            }
            return list;
        }

        public static bool Orientation(List<IntPoint> poly) => 
            (Area(poly) >= 0.0);

        private bool Param1RightOfParam2(OutRec outRec1, OutRec outRec2)
        {
            do
            {
                outRec1 = outRec1.FirstLeft;
                if (outRec1 == outRec2)
                {
                    return true;
                }
            }
            while (outRec1 > null);
            return false;
        }

        private static OutRec ParseFirstLeft(OutRec FirstLeft)
        {
            while ((FirstLeft != null) && (FirstLeft.Pts == null))
            {
                FirstLeft = FirstLeft.FirstLeft;
            }
            return FirstLeft;
        }

        private int PointCount(OutPt pts)
        {
            if (pts == null)
            {
                return 0;
            }
            int num = 0;
            OutPt next = pts;
            do
            {
                num++;
                next = next.Next;
            }
            while (next != pts);
            return num;
        }

        private static int PointInPolygon(IntPoint pt, OutPt op)
        {
            int num = 0;
            OutPt pt2 = op;
            long x = pt.X;
            long y = pt.Y;
            long num4 = op.Pt.X;
            long num5 = op.Pt.Y;
            do
            {
                op = op.Next;
                long num6 = op.Pt.X;
                long num7 = op.Pt.Y;
                if ((num7 == y) && ((num6 == x) || ((num5 == y) && ((num6 > x) == (num4 < x)))))
                {
                    return -1;
                }
                if ((num5 < y) != (num7 < y))
                {
                    if (num4 >= x)
                    {
                        if (num6 > x)
                        {
                            num = 1 - num;
                        }
                        else
                        {
                            double num9 = ((num4 - x) * (num7 - y)) - ((num6 - x) * (num5 - y));
                            if (num9 == 0.0)
                            {
                                return -1;
                            }
                            if ((num9 > 0.0) == (num7 > num5))
                            {
                                num = 1 - num;
                            }
                        }
                    }
                    else if (num6 > x)
                    {
                        double num10 = ((num4 - x) * (num7 - y)) - ((num6 - x) * (num5 - y));
                        if (num10 == 0.0)
                        {
                            return -1;
                        }
                        if ((num10 > 0.0) == (num7 > num5))
                        {
                            num = 1 - num;
                        }
                    }
                }
                num4 = num6;
                num5 = num7;
            }
            while (pt2 != op);
            return num;
        }

        public static int PointInPolygon(IntPoint pt, List<IntPoint> path)
        {
            int num = 0;
            int count = path.Count;
            if (count < 3)
            {
                return 0;
            }
            IntPoint point = path[0];
            for (int i = 1; i <= count; i++)
            {
                IntPoint point2 = (i == count) ? path[0] : path[i];
                if ((point2.Y == pt.Y) && ((point2.X == pt.X) || ((point.Y == pt.Y) && ((point2.X > pt.X) == (point.X < pt.X)))))
                {
                    return -1;
                }
                if ((point.Y < pt.Y) != (point2.Y < pt.Y))
                {
                    if (point.X >= pt.X)
                    {
                        if (point2.X > pt.X)
                        {
                            num = 1 - num;
                        }
                        else
                        {
                            double num5 = ((point.X - pt.X) * (point2.Y - pt.Y)) - ((point2.X - pt.X) * (point.Y - pt.Y));
                            if (num5 == 0.0)
                            {
                                return -1;
                            }
                            if ((num5 > 0.0) == (point2.Y > point.Y))
                            {
                                num = 1 - num;
                            }
                        }
                    }
                    else if (point2.X > pt.X)
                    {
                        double num6 = ((point.X - pt.X) * (point2.Y - pt.Y)) - ((point2.X - pt.X) * (point.Y - pt.Y));
                        if (num6 == 0.0)
                        {
                            return -1;
                        }
                        if ((num6 > 0.0) == (point2.Y > point.Y))
                        {
                            num = 1 - num;
                        }
                    }
                }
                point = point2;
            }
            return num;
        }

        private static bool PointsAreClose(IntPoint pt1, IntPoint pt2, double distSqrd)
        {
            double num = pt1.X - pt2.X;
            double num2 = pt1.Y - pt2.Y;
            return (((num * num) + (num2 * num2)) <= distSqrd);
        }

        private static bool Poly2ContainsPoly1(OutPt outPt1, OutPt outPt2)
        {
            OutPt next = outPt1;
            do
            {
                int num = PointInPolygon(next.Pt, outPt2);
                if (num >= 0)
                {
                    return (num > 0);
                }
                next = next.Next;
            }
            while (next != outPt1);
            return true;
        }

        public static List<List<IntPoint>> PolyTreeToPaths(PolyTree polytree)
        {
            List<List<IntPoint>> paths = new List<List<IntPoint>> {
                Capacity = polytree.Total
            };
            AddPolyNodeToPaths(polytree, NodeType.ntAny, paths);
            return paths;
        }

        private long PopScanbeam()
        {
            long y = this.m_Scanbeam.Y;
            this.m_Scanbeam = this.m_Scanbeam.Next;
            return y;
        }

        private void ProcessEdgesAtTopOfScanbeam(long topY)
        {
            TEdge activeEdges = this.m_ActiveEdges;
            while (activeEdges > null)
            {
                bool flag = this.IsMaxima(activeEdges, (double) topY);
                if (flag)
                {
                    TEdge maximaPair = this.GetMaximaPair(activeEdges);
                    flag = (maximaPair == null) || !ClipperBase.IsHorizontal(maximaPair);
                }
                if (flag)
                {
                    TEdge prevInAEL = activeEdges.PrevInAEL;
                    this.DoMaxima(activeEdges);
                    if (prevInAEL == null)
                    {
                        activeEdges = this.m_ActiveEdges;
                    }
                    else
                    {
                        activeEdges = prevInAEL.NextInAEL;
                    }
                }
                else
                {
                    if (this.IsIntermediate(activeEdges, (double) topY) && ClipperBase.IsHorizontal(activeEdges.NextInLML))
                    {
                        this.UpdateEdgeIntoAEL(ref activeEdges);
                        if (activeEdges.OutIdx >= 0)
                        {
                            this.AddOutPt(activeEdges, activeEdges.Bot);
                        }
                        this.AddEdgeToSEL(activeEdges);
                    }
                    else
                    {
                        activeEdges.Curr.X = TopX(activeEdges, topY);
                        activeEdges.Curr.Y = topY;
                    }
                    if (this.StrictlySimple)
                    {
                        TEdge e = activeEdges.PrevInAEL;
                        if (((((activeEdges.OutIdx >= 0) && (activeEdges.WindDelta != 0)) && ((e != null) && (e.OutIdx >= 0))) && (e.Curr.X == activeEdges.Curr.X)) && (e.WindDelta > 0))
                        {
                            IntPoint point = new IntPoint(activeEdges.Curr);
                            OutPt pt = this.AddOutPt(e, point);
                            OutPt pt2 = this.AddOutPt(activeEdges, point);
                            this.AddJoin(pt, pt2, point);
                        }
                    }
                    activeEdges = activeEdges.NextInAEL;
                }
            }
            this.ProcessHorizontals(true);
            for (activeEdges = this.m_ActiveEdges; activeEdges > null; activeEdges = activeEdges.NextInAEL)
            {
                if (this.IsIntermediate(activeEdges, (double) topY))
                {
                    OutPt pt3 = null;
                    if (activeEdges.OutIdx >= 0)
                    {
                        pt3 = this.AddOutPt(activeEdges, activeEdges.Top);
                    }
                    this.UpdateEdgeIntoAEL(ref activeEdges);
                    TEdge edge5 = activeEdges.PrevInAEL;
                    TEdge nextInAEL = activeEdges.NextInAEL;
                    if (((((edge5 != null) && (edge5.Curr.X == activeEdges.Bot.X)) && ((edge5.Curr.Y == activeEdges.Bot.Y) && (pt3 != null))) && (((edge5.OutIdx >= 0) && (edge5.Curr.Y > edge5.Top.Y)) && (ClipperBase.SlopesEqual(activeEdges, edge5, base.m_UseFullRange) && (activeEdges.WindDelta != 0)))) && (edge5.WindDelta > 0))
                    {
                        OutPt pt4 = this.AddOutPt(edge5, activeEdges.Bot);
                        this.AddJoin(pt3, pt4, activeEdges.Top);
                    }
                    else if (((((nextInAEL != null) && (nextInAEL.Curr.X == activeEdges.Bot.X)) && ((nextInAEL.Curr.Y == activeEdges.Bot.Y) && (pt3 != null))) && (((nextInAEL.OutIdx >= 0) && (nextInAEL.Curr.Y > nextInAEL.Top.Y)) && (ClipperBase.SlopesEqual(activeEdges, nextInAEL, base.m_UseFullRange) && (activeEdges.WindDelta != 0)))) && (nextInAEL.WindDelta > 0))
                    {
                        OutPt pt5 = this.AddOutPt(nextInAEL, activeEdges.Bot);
                        this.AddJoin(pt3, pt5, activeEdges.Top);
                    }
                }
            }
        }

        private void ProcessHorizontal(TEdge horzEdge, bool isTopOfScanbeam)
        {
            Direction direction;
            long num;
            long num2;
            bool flag3;
            TEdge nextInAEL;
            this.GetHorzDirection(horzEdge, out direction, out num, out num2);
            TEdge e = horzEdge;
            TEdge maximaPair = null;
            while ((e.NextInLML != null) && ClipperBase.IsHorizontal(e.NextInLML))
            {
                e = e.NextInLML;
            }
            if (e.NextInLML == null)
            {
                maximaPair = this.GetMaximaPair(e);
            }
        Label_005A:
            flag3 = horzEdge == e;
            for (TEdge edge3 = this.GetNextInAEL(horzEdge, direction); edge3 > null; edge3 = nextInAEL)
            {
                if (((edge3.Curr.X == horzEdge.Top.X) && (horzEdge.NextInLML != null)) && (edge3.Dx < horzEdge.NextInLML.Dx))
                {
                    break;
                }
                nextInAEL = this.GetNextInAEL(edge3, direction);
                if (((direction == Direction.dLeftToRight) && (edge3.Curr.X <= num2)) || ((direction == Direction.dRightToLeft) && (edge3.Curr.X >= num)))
                {
                    if ((edge3 == maximaPair) & flag3)
                    {
                        if (horzEdge.OutIdx >= 0)
                        {
                            OutPt pt = this.AddOutPt(horzEdge, horzEdge.Top);
                            for (TEdge edge5 = this.m_SortedEdges; edge5 > null; edge5 = edge5.NextInSEL)
                            {
                                if ((edge5.OutIdx >= 0) && this.HorzSegmentsOverlap(horzEdge.Bot.X, horzEdge.Top.X, edge5.Bot.X, edge5.Top.X))
                                {
                                    OutPt pt2 = this.AddOutPt(edge5, edge5.Bot);
                                    this.AddJoin(pt2, pt, edge5.Top);
                                }
                            }
                            this.AddGhostJoin(pt, horzEdge.Bot);
                            this.AddLocalMaxPoly(horzEdge, maximaPair, horzEdge.Top);
                        }
                        this.DeleteFromAEL(horzEdge);
                        this.DeleteFromAEL(maximaPair);
                        return;
                    }
                    if (direction == Direction.dLeftToRight)
                    {
                        IntPoint point = new IntPoint(edge3.Curr.X, horzEdge.Curr.Y);
                        this.IntersectEdges(horzEdge, edge3, point);
                    }
                    else
                    {
                        IntPoint point2 = new IntPoint(edge3.Curr.X, horzEdge.Curr.Y);
                        this.IntersectEdges(edge3, horzEdge, point2);
                    }
                    this.SwapPositionsInAEL(horzEdge, edge3);
                }
                else if (((direction == Direction.dLeftToRight) && (edge3.Curr.X >= num2)) || ((direction == Direction.dRightToLeft) && (edge3.Curr.X <= num)))
                {
                    break;
                }
            }
            if ((horzEdge.NextInLML != null) && ClipperBase.IsHorizontal(horzEdge.NextInLML))
            {
                this.UpdateEdgeIntoAEL(ref horzEdge);
                if (horzEdge.OutIdx >= 0)
                {
                    this.AddOutPt(horzEdge, horzEdge.Bot);
                }
                this.GetHorzDirection(horzEdge, out direction, out num, out num2);
                goto Label_005A;
            }
            if (horzEdge.NextInLML > null)
            {
                if (horzEdge.OutIdx >= 0)
                {
                    OutPt op = this.AddOutPt(horzEdge, horzEdge.Top);
                    if (isTopOfScanbeam)
                    {
                        this.AddGhostJoin(op, horzEdge.Bot);
                    }
                    this.UpdateEdgeIntoAEL(ref horzEdge);
                    if (horzEdge.WindDelta != 0)
                    {
                        TEdge prevInAEL = horzEdge.PrevInAEL;
                        TEdge edge7 = horzEdge.NextInAEL;
                        if ((((prevInAEL != null) && (prevInAEL.Curr.X == horzEdge.Bot.X)) && ((prevInAEL.Curr.Y == horzEdge.Bot.Y) && (prevInAEL.WindDelta != 0))) && (((prevInAEL.OutIdx >= 0) && (prevInAEL.Curr.Y > prevInAEL.Top.Y)) && ClipperBase.SlopesEqual(horzEdge, prevInAEL, base.m_UseFullRange)))
                        {
                            OutPt pt4 = this.AddOutPt(prevInAEL, horzEdge.Bot);
                            this.AddJoin(op, pt4, horzEdge.Top);
                        }
                        else if (((((edge7 != null) && (edge7.Curr.X == horzEdge.Bot.X)) && ((edge7.Curr.Y == horzEdge.Bot.Y) && (edge7.WindDelta != 0))) && ((edge7.OutIdx >= 0) && (edge7.Curr.Y > edge7.Top.Y))) && ClipperBase.SlopesEqual(horzEdge, edge7, base.m_UseFullRange))
                        {
                            OutPt pt5 = this.AddOutPt(edge7, horzEdge.Bot);
                            this.AddJoin(op, pt5, horzEdge.Top);
                        }
                    }
                }
                else
                {
                    this.UpdateEdgeIntoAEL(ref horzEdge);
                }
            }
            else
            {
                if (horzEdge.OutIdx >= 0)
                {
                    this.AddOutPt(horzEdge, horzEdge.Top);
                }
                this.DeleteFromAEL(horzEdge);
            }
        }

        private void ProcessHorizontals(bool isTopOfScanbeam)
        {
            for (TEdge edge = this.m_SortedEdges; edge > null; edge = this.m_SortedEdges)
            {
                this.DeleteFromSEL(edge);
                this.ProcessHorizontal(edge, isTopOfScanbeam);
            }
        }

        private bool ProcessIntersections(long topY)
        {
            if (this.m_ActiveEdges != null)
            {
                try
                {
                    this.BuildIntersectList(topY);
                    if (this.m_IntersectList.Count == 0)
                    {
                        return true;
                    }
                    if ((this.m_IntersectList.Count == 1) || this.FixupIntersectionOrder())
                    {
                        this.ProcessIntersectList();
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    this.m_SortedEdges = null;
                    this.m_IntersectList.Clear();
                    throw new ClipperException("ProcessIntersections error");
                }
                this.m_SortedEdges = null;
            }
            return true;
        }

        private void ProcessIntersectList()
        {
            for (int i = 0; i < this.m_IntersectList.Count; i++)
            {
                IntersectNode node = this.m_IntersectList[i];
                this.IntersectEdges(node.Edge1, node.Edge2, node.Pt);
                this.SwapPositionsInAEL(node.Edge1, node.Edge2);
            }
            this.m_IntersectList.Clear();
        }

        protected override void Reset()
        {
            base.Reset();
            this.m_Scanbeam = null;
            this.m_ActiveEdges = null;
            this.m_SortedEdges = null;
            for (LocalMinima minima = base.m_MinimaList; minima > null; minima = minima.Next)
            {
                this.InsertScanbeam(minima.Y);
            }
        }

        public static void ReversePaths(List<List<IntPoint>> polys)
        {
            foreach (List<IntPoint> list in polys)
            {
                list.Reverse();
            }
        }

        private void ReversePolyPtLinks(OutPt pp)
        {
            if (pp != null)
            {
                OutPt pt = pp;
                do
                {
                    OutPt next = pt.Next;
                    pt.Next = pt.Prev;
                    pt.Prev = next;
                    pt = next;
                }
                while (pt != pp);
            }
        }

        internal static long Round(double value) => 
            ((value < 0.0) ? ((long) (value - 0.5)) : ((long) (value + 0.5)));

        private void SetHoleState(TEdge e, OutRec outRec)
        {
            bool flag = false;
            for (TEdge edge = e.PrevInAEL; edge > null; edge = edge.PrevInAEL)
            {
                if ((edge.OutIdx >= 0) && (edge.WindDelta > 0))
                {
                    flag = !flag;
                    if (outRec.FirstLeft == null)
                    {
                        outRec.FirstLeft = this.m_PolyOuts[edge.OutIdx];
                    }
                }
            }
            if (flag)
            {
                outRec.IsHole = true;
            }
        }

        private void SetWindingCount(TEdge edge)
        {
            TEdge prevInAEL = edge.PrevInAEL;
            while ((prevInAEL != null) && ((prevInAEL.PolyTyp != edge.PolyTyp) || (prevInAEL.WindDelta == 0)))
            {
                prevInAEL = prevInAEL.PrevInAEL;
            }
            if (prevInAEL == null)
            {
                edge.WindCnt = (edge.WindDelta == 0) ? 1 : edge.WindDelta;
                edge.WindCnt2 = 0;
                prevInAEL = this.m_ActiveEdges;
            }
            else if ((edge.WindDelta == 0) && (this.m_ClipType != ClipType.ctUnion))
            {
                edge.WindCnt = 1;
                edge.WindCnt2 = prevInAEL.WindCnt2;
                prevInAEL = prevInAEL.NextInAEL;
            }
            else if (this.IsEvenOddFillType(edge))
            {
                if (edge.WindDelta == 0)
                {
                    bool flag6 = true;
                    for (TEdge edge3 = prevInAEL.PrevInAEL; edge3 > null; edge3 = edge3.PrevInAEL)
                    {
                        if ((edge3.PolyTyp == prevInAEL.PolyTyp) && (edge3.WindDelta > 0))
                        {
                            flag6 = !flag6;
                        }
                    }
                    edge.WindCnt = flag6 ? 0 : 1;
                }
                else
                {
                    edge.WindCnt = edge.WindDelta;
                }
                edge.WindCnt2 = prevInAEL.WindCnt2;
                prevInAEL = prevInAEL.NextInAEL;
            }
            else
            {
                if ((prevInAEL.WindCnt * prevInAEL.WindDelta) < 0)
                {
                    if (Math.Abs(prevInAEL.WindCnt) > 1)
                    {
                        if ((prevInAEL.WindDelta * edge.WindDelta) < 0)
                        {
                            edge.WindCnt = prevInAEL.WindCnt;
                        }
                        else
                        {
                            edge.WindCnt = prevInAEL.WindCnt + edge.WindDelta;
                        }
                    }
                    else
                    {
                        edge.WindCnt = (edge.WindDelta == 0) ? 1 : edge.WindDelta;
                    }
                }
                else if (edge.WindDelta == 0)
                {
                    edge.WindCnt = (prevInAEL.WindCnt < 0) ? (prevInAEL.WindCnt - 1) : (prevInAEL.WindCnt + 1);
                }
                else if ((prevInAEL.WindDelta * edge.WindDelta) < 0)
                {
                    edge.WindCnt = prevInAEL.WindCnt;
                }
                else
                {
                    edge.WindCnt = prevInAEL.WindCnt + edge.WindDelta;
                }
                edge.WindCnt2 = prevInAEL.WindCnt2;
                prevInAEL = prevInAEL.NextInAEL;
            }
            if (!this.IsEvenOddAltFillType(edge))
            {
                while (prevInAEL != edge)
                {
                    edge.WindCnt2 += prevInAEL.WindDelta;
                    prevInAEL = prevInAEL.NextInAEL;
                }
            }
            else
            {
                while (prevInAEL != edge)
                {
                    if (prevInAEL.WindDelta > 0)
                    {
                        edge.WindCnt2 = (edge.WindCnt2 == 0) ? 1 : 0;
                    }
                    prevInAEL = prevInAEL.NextInAEL;
                }
            }
        }

        public static List<List<IntPoint>> SimplifyPolygon(List<IntPoint> poly, PolyFillType fillType = 0)
        {
            List<List<IntPoint>> solution = new List<List<IntPoint>>();
            Clipper clipper = new Clipper(0) {
                StrictlySimple = true
            };
            clipper.AddPath(poly, PolyType.ptSubject, true);
            clipper.Execute(ClipType.ctUnion, solution, fillType, fillType);
            return solution;
        }

        public static List<List<IntPoint>> SimplifyPolygons(List<List<IntPoint>> polys, PolyFillType fillType = 0)
        {
            List<List<IntPoint>> solution = new List<List<IntPoint>>();
            Clipper clipper = new Clipper(0) {
                StrictlySimple = true
            };
            clipper.AddPaths(polys, PolyType.ptSubject, true);
            clipper.Execute(ClipType.ctUnion, solution, fillType, fillType);
            return solution;
        }

        private static bool SlopesNearCollinear(IntPoint pt1, IntPoint pt2, IntPoint pt3, double distSqrd)
        {
            if (Math.Abs((long) (pt1.X - pt2.X)) > Math.Abs((long) (pt1.Y - pt2.Y)))
            {
                if ((pt1.X > pt2.X) == (pt1.X < pt3.X))
                {
                    return (DistanceFromLineSqrd(pt1, pt2, pt3) < distSqrd);
                }
                if ((pt2.X > pt1.X) == (pt2.X < pt3.X))
                {
                    return (DistanceFromLineSqrd(pt2, pt1, pt3) < distSqrd);
                }
                return (DistanceFromLineSqrd(pt3, pt1, pt2) < distSqrd);
            }
            if ((pt1.Y > pt2.Y) == (pt1.Y < pt3.Y))
            {
                return (DistanceFromLineSqrd(pt1, pt2, pt3) < distSqrd);
            }
            if ((pt2.Y > pt1.Y) == (pt2.Y < pt3.Y))
            {
                return (DistanceFromLineSqrd(pt2, pt1, pt3) < distSqrd);
            }
            return (DistanceFromLineSqrd(pt3, pt1, pt2) < distSqrd);
        }

        internal void SwapPoints(ref IntPoint pt1, ref IntPoint pt2)
        {
            IntPoint point = new IntPoint(pt1);
            pt1 = pt2;
            pt2 = point;
        }

        private static void SwapPolyIndexes(TEdge edge1, TEdge edge2)
        {
            int outIdx = edge1.OutIdx;
            edge1.OutIdx = edge2.OutIdx;
            edge2.OutIdx = outIdx;
        }

        private void SwapPositionsInAEL(TEdge edge1, TEdge edge2)
        {
            if ((edge1.NextInAEL != edge1.PrevInAEL) && (edge2.NextInAEL != edge2.PrevInAEL))
            {
                if (edge1.NextInAEL == edge2)
                {
                    TEdge nextInAEL = edge2.NextInAEL;
                    if (nextInAEL > null)
                    {
                        nextInAEL.PrevInAEL = edge1;
                    }
                    TEdge prevInAEL = edge1.PrevInAEL;
                    if (prevInAEL > null)
                    {
                        prevInAEL.NextInAEL = edge2;
                    }
                    edge2.PrevInAEL = prevInAEL;
                    edge2.NextInAEL = edge1;
                    edge1.PrevInAEL = edge2;
                    edge1.NextInAEL = nextInAEL;
                }
                else if (edge2.NextInAEL == edge1)
                {
                    TEdge edge4 = edge1.NextInAEL;
                    if (edge4 > null)
                    {
                        edge4.PrevInAEL = edge2;
                    }
                    TEdge edge5 = edge2.PrevInAEL;
                    if (edge5 > null)
                    {
                        edge5.NextInAEL = edge1;
                    }
                    edge1.PrevInAEL = edge5;
                    edge1.NextInAEL = edge2;
                    edge2.PrevInAEL = edge1;
                    edge2.NextInAEL = edge4;
                }
                else
                {
                    TEdge edge6 = edge1.NextInAEL;
                    TEdge edge7 = edge1.PrevInAEL;
                    edge1.NextInAEL = edge2.NextInAEL;
                    if (edge1.NextInAEL > null)
                    {
                        edge1.NextInAEL.PrevInAEL = edge1;
                    }
                    edge1.PrevInAEL = edge2.PrevInAEL;
                    if (edge1.PrevInAEL > null)
                    {
                        edge1.PrevInAEL.NextInAEL = edge1;
                    }
                    edge2.NextInAEL = edge6;
                    if (edge2.NextInAEL > null)
                    {
                        edge2.NextInAEL.PrevInAEL = edge2;
                    }
                    edge2.PrevInAEL = edge7;
                    if (edge2.PrevInAEL > null)
                    {
                        edge2.PrevInAEL.NextInAEL = edge2;
                    }
                }
                if (edge1.PrevInAEL == null)
                {
                    this.m_ActiveEdges = edge1;
                }
                else if (edge2.PrevInAEL == null)
                {
                    this.m_ActiveEdges = edge2;
                }
            }
        }

        private void SwapPositionsInSEL(TEdge edge1, TEdge edge2)
        {
            if (((edge1.NextInSEL != null) || (edge1.PrevInSEL != null)) && ((edge2.NextInSEL != null) || (edge2.PrevInSEL != null)))
            {
                if (edge1.NextInSEL == edge2)
                {
                    TEdge nextInSEL = edge2.NextInSEL;
                    if (nextInSEL > null)
                    {
                        nextInSEL.PrevInSEL = edge1;
                    }
                    TEdge prevInSEL = edge1.PrevInSEL;
                    if (prevInSEL > null)
                    {
                        prevInSEL.NextInSEL = edge2;
                    }
                    edge2.PrevInSEL = prevInSEL;
                    edge2.NextInSEL = edge1;
                    edge1.PrevInSEL = edge2;
                    edge1.NextInSEL = nextInSEL;
                }
                else if (edge2.NextInSEL == edge1)
                {
                    TEdge edge4 = edge1.NextInSEL;
                    if (edge4 > null)
                    {
                        edge4.PrevInSEL = edge2;
                    }
                    TEdge edge5 = edge2.PrevInSEL;
                    if (edge5 > null)
                    {
                        edge5.NextInSEL = edge1;
                    }
                    edge1.PrevInSEL = edge5;
                    edge1.NextInSEL = edge2;
                    edge2.PrevInSEL = edge1;
                    edge2.NextInSEL = edge4;
                }
                else
                {
                    TEdge edge6 = edge1.NextInSEL;
                    TEdge edge7 = edge1.PrevInSEL;
                    edge1.NextInSEL = edge2.NextInSEL;
                    if (edge1.NextInSEL > null)
                    {
                        edge1.NextInSEL.PrevInSEL = edge1;
                    }
                    edge1.PrevInSEL = edge2.PrevInSEL;
                    if (edge1.PrevInSEL > null)
                    {
                        edge1.PrevInSEL.NextInSEL = edge1;
                    }
                    edge2.NextInSEL = edge6;
                    if (edge2.NextInSEL > null)
                    {
                        edge2.NextInSEL.PrevInSEL = edge2;
                    }
                    edge2.PrevInSEL = edge7;
                    if (edge2.PrevInSEL > null)
                    {
                        edge2.PrevInSEL.NextInSEL = edge2;
                    }
                }
                if (edge1.PrevInSEL == null)
                {
                    this.m_SortedEdges = edge1;
                }
                else if (edge2.PrevInSEL == null)
                {
                    this.m_SortedEdges = edge2;
                }
            }
        }

        private static void SwapSides(TEdge edge1, TEdge edge2)
        {
            EdgeSide side = edge1.Side;
            edge1.Side = edge2.Side;
            edge2.Side = side;
        }

        private static long TopX(TEdge edge, long currentY)
        {
            if (currentY == edge.Top.Y)
            {
                return edge.Top.X;
            }
            return (edge.Bot.X + Round(edge.Dx * (currentY - edge.Bot.Y)));
        }

        private static List<IntPoint> TranslatePath(List<IntPoint> path, IntPoint delta)
        {
            List<IntPoint> list = new List<IntPoint>(path.Count);
            for (int i = 0; i < path.Count; i++)
            {
                list.Add(new IntPoint(path[i].X + delta.X, path[i].Y + delta.Y));
            }
            return list;
        }

        private void UpdateEdgeIntoAEL(ref TEdge e)
        {
            if (e.NextInLML == null)
            {
                throw new ClipperException("UpdateEdgeIntoAEL: invalid call");
            }
            TEdge prevInAEL = e.PrevInAEL;
            TEdge nextInAEL = e.NextInAEL;
            e.NextInLML.OutIdx = e.OutIdx;
            if (prevInAEL > null)
            {
                prevInAEL.NextInAEL = e.NextInLML;
            }
            else
            {
                this.m_ActiveEdges = e.NextInLML;
            }
            if (nextInAEL > null)
            {
                nextInAEL.PrevInAEL = e.NextInLML;
            }
            e.NextInLML.Side = e.Side;
            e.NextInLML.WindDelta = e.WindDelta;
            e.NextInLML.WindCnt = e.WindCnt;
            e.NextInLML.WindCnt2 = e.WindCnt2;
            e = e.NextInLML;
            e.Curr = e.Bot;
            e.PrevInAEL = prevInAEL;
            e.NextInAEL = nextInAEL;
            if (!ClipperBase.IsHorizontal(e))
            {
                this.InsertScanbeam(e.Top.Y);
            }
        }

        private void UpdateOutPtIdxs(OutRec outrec)
        {
            OutPt pts = outrec.Pts;
            do
            {
                pts.Idx = outrec.Idx;
                pts = pts.Prev;
            }
            while (pts != outrec.Pts);
        }

        public bool ReverseSolution { get; set; }

        public bool StrictlySimple { get; set; }

        internal enum NodeType
        {
            ntAny,
            ntOpen,
            ntClosed
        }
    }
}

