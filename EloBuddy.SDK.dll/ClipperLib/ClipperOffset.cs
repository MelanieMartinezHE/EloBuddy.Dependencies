namespace ClipperLib
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class ClipperOffset
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double <ArcTolerance>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double <MiterLimit>k__BackingField;
        private const double def_arc_tolerance = 0.25;
        private double m_cos;
        private double m_delta;
        private List<IntPoint> m_destPoly;
        private List<List<IntPoint>> m_destPolys;
        private IntPoint m_lowest;
        private double m_miterLim;
        private List<DoublePoint> m_normals = new List<DoublePoint>();
        private PolyNode m_polyNodes = new PolyNode();
        private double m_sin;
        private double m_sinA;
        private List<IntPoint> m_srcPoly;
        private double m_StepsPerRad;
        private const double two_pi = 6.2831853071795862;

        public ClipperOffset(double miterLimit = 2.0, double arcTolerance = 0.25)
        {
            this.MiterLimit = miterLimit;
            this.ArcTolerance = arcTolerance;
            this.m_lowest.X = -1L;
        }

        public void AddPath(List<IntPoint> path, JoinType joinType, EndType endType)
        {
            int num = path.Count - 1;
            if (num >= 0)
            {
                PolyNode child = new PolyNode {
                    m_jointype = joinType,
                    m_endtype = endType
                };
                if ((endType == EndType.etClosedLine) || (endType == EndType.etClosedPolygon))
                {
                    while ((num > 0) && (path[0] == path[num]))
                    {
                        num--;
                    }
                }
                child.m_polygon.Capacity = num + 1;
                child.m_polygon.Add(path[0]);
                int num2 = 0;
                int num3 = 0;
                for (int i = 1; i <= num; i++)
                {
                    if (child.m_polygon[num2] != path[i])
                    {
                        num2++;
                        child.m_polygon.Add(path[i]);
                        if ((path[i].Y > child.m_polygon[num3].Y) || ((path[i].Y == child.m_polygon[num3].Y) && (path[i].X < child.m_polygon[num3].X)))
                        {
                            num3 = num2;
                        }
                    }
                }
                if ((endType != EndType.etClosedPolygon) || (num2 >= 2))
                {
                    this.m_polyNodes.AddChild(child);
                    if (endType <= EndType.etClosedPolygon)
                    {
                        if (this.m_lowest.X < 0L)
                        {
                            this.m_lowest = new IntPoint((long) (this.m_polyNodes.ChildCount - 1), (long) num3);
                        }
                        else
                        {
                            IntPoint point = this.m_polyNodes.Childs[(int) this.m_lowest.X].m_polygon[(int) this.m_lowest.Y];
                            if ((child.m_polygon[num3].Y > point.Y) || ((child.m_polygon[num3].Y == point.Y) && (child.m_polygon[num3].X < point.X)))
                            {
                                this.m_lowest = new IntPoint((long) (this.m_polyNodes.ChildCount - 1), (long) num3);
                            }
                        }
                    }
                }
            }
        }

        public void AddPaths(List<List<IntPoint>> paths, JoinType joinType, EndType endType)
        {
            foreach (List<IntPoint> list in paths)
            {
                this.AddPath(list, joinType, endType);
            }
        }

        public void Clear()
        {
            this.m_polyNodes.Childs.Clear();
            this.m_lowest.X = -1L;
        }

        internal void DoMiter(int j, int k, double r)
        {
            double num = this.m_delta / r;
            this.m_destPoly.Add(new IntPoint(Round(this.m_srcPoly[j].X + ((this.m_normals[k].X + this.m_normals[j].X) * num)), Round(this.m_srcPoly[j].Y + ((this.m_normals[k].Y + this.m_normals[j].Y) * num))));
        }

        private void DoOffset(double delta)
        {
            this.m_destPolys = new List<List<IntPoint>>();
            this.m_delta = delta;
            if (ClipperBase.near_zero(delta))
            {
                this.m_destPolys.Capacity = this.m_polyNodes.ChildCount;
                for (int i = 0; i < this.m_polyNodes.ChildCount; i++)
                {
                    PolyNode node = this.m_polyNodes.Childs[i];
                    if (node.m_endtype == EndType.etClosedPolygon)
                    {
                        this.m_destPolys.Add(node.m_polygon);
                    }
                }
            }
            else
            {
                double arcTolerance;
                if (this.MiterLimit > 2.0)
                {
                    this.m_miterLim = 2.0 / (this.MiterLimit * this.MiterLimit);
                }
                else
                {
                    this.m_miterLim = 0.5;
                }
                if (this.ArcTolerance <= 0.0)
                {
                    arcTolerance = 0.25;
                }
                else if (this.ArcTolerance > (Math.Abs(delta) * 0.25))
                {
                    arcTolerance = Math.Abs(delta) * 0.25;
                }
                else
                {
                    arcTolerance = this.ArcTolerance;
                }
                double num2 = 3.1415926535897931 / Math.Acos(1.0 - (arcTolerance / Math.Abs(delta)));
                this.m_sin = Math.Sin(6.2831853071795862 / num2);
                this.m_cos = Math.Cos(6.2831853071795862 / num2);
                this.m_StepsPerRad = num2 / 6.2831853071795862;
                if (delta < 0.0)
                {
                    this.m_sin = -this.m_sin;
                }
                this.m_destPolys.Capacity = this.m_polyNodes.ChildCount * 2;
                for (int j = 0; j < this.m_polyNodes.ChildCount; j++)
                {
                    PolyNode node2 = this.m_polyNodes.Childs[j];
                    this.m_srcPoly = node2.m_polygon;
                    int count = this.m_srcPoly.Count;
                    if ((count != 0) && ((delta > 0.0) || ((count >= 3) && (node2.m_endtype <= EndType.etClosedPolygon))))
                    {
                        this.m_destPoly = new List<IntPoint>();
                        if (count == 1)
                        {
                            if (node2.m_jointype == JoinType.jtRound)
                            {
                                double num6 = 1.0;
                                double num7 = 0.0;
                                for (int k = 1; k <= num2; k++)
                                {
                                    this.m_destPoly.Add(new IntPoint(Round(this.m_srcPoly[0].X + (num6 * delta)), Round(this.m_srcPoly[0].Y + (num7 * delta))));
                                    double num9 = num6;
                                    num6 = (num6 * this.m_cos) - (this.m_sin * num7);
                                    num7 = (num9 * this.m_sin) + (num7 * this.m_cos);
                                }
                            }
                            else
                            {
                                double num10 = -1.0;
                                double num11 = -1.0;
                                for (int m = 0; m < 4; m++)
                                {
                                    this.m_destPoly.Add(new IntPoint(Round(this.m_srcPoly[0].X + (num10 * delta)), Round(this.m_srcPoly[0].Y + (num11 * delta))));
                                    if (num10 < 0.0)
                                    {
                                        num10 = 1.0;
                                    }
                                    else if (num11 < 0.0)
                                    {
                                        num11 = 1.0;
                                    }
                                    else
                                    {
                                        num10 = -1.0;
                                    }
                                }
                            }
                            this.m_destPolys.Add(this.m_destPoly);
                        }
                        else
                        {
                            this.m_normals.Clear();
                            this.m_normals.Capacity = count;
                            for (int n = 0; n < (count - 1); n++)
                            {
                                this.m_normals.Add(GetUnitNormal(this.m_srcPoly[n], this.m_srcPoly[n + 1]));
                            }
                            if ((node2.m_endtype == EndType.etClosedLine) || (node2.m_endtype == EndType.etClosedPolygon))
                            {
                                this.m_normals.Add(GetUnitNormal(this.m_srcPoly[count - 1], this.m_srcPoly[0]));
                            }
                            else
                            {
                                this.m_normals.Add(new DoublePoint(this.m_normals[count - 2]));
                            }
                            if (node2.m_endtype == EndType.etClosedPolygon)
                            {
                                int num14 = count - 1;
                                for (int num15 = 0; num15 < count; num15++)
                                {
                                    this.OffsetPoint(num15, ref num14, node2.m_jointype);
                                }
                                this.m_destPolys.Add(this.m_destPoly);
                            }
                            else if (node2.m_endtype == EndType.etClosedLine)
                            {
                                int num16 = count - 1;
                                for (int num17 = 0; num17 < count; num17++)
                                {
                                    this.OffsetPoint(num17, ref num16, node2.m_jointype);
                                }
                                this.m_destPolys.Add(this.m_destPoly);
                                this.m_destPoly = new List<IntPoint>();
                                DoublePoint point = this.m_normals[count - 1];
                                for (int num18 = count - 1; num18 > 0; num18--)
                                {
                                    this.m_normals[num18] = new DoublePoint(-this.m_normals[num18 - 1].X, -this.m_normals[num18 - 1].Y);
                                }
                                this.m_normals[0] = new DoublePoint(-point.X, -point.Y);
                                num16 = 0;
                                for (int num19 = count - 1; num19 >= 0; num19--)
                                {
                                    this.OffsetPoint(num19, ref num16, node2.m_jointype);
                                }
                                this.m_destPolys.Add(this.m_destPoly);
                            }
                            else
                            {
                                IntPoint point2;
                                int num20 = 0;
                                for (int num21 = 1; num21 < (count - 1); num21++)
                                {
                                    this.OffsetPoint(num21, ref num20, node2.m_jointype);
                                }
                                if (node2.m_endtype == EndType.etOpenButt)
                                {
                                    int num22 = count - 1;
                                    point2 = new IntPoint(Round(this.m_srcPoly[num22].X + (this.m_normals[num22].X * delta)), Round(this.m_srcPoly[num22].Y + (this.m_normals[num22].Y * delta)));
                                    this.m_destPoly.Add(point2);
                                    point2 = new IntPoint(Round(this.m_srcPoly[num22].X - (this.m_normals[num22].X * delta)), Round(this.m_srcPoly[num22].Y - (this.m_normals[num22].Y * delta)));
                                    this.m_destPoly.Add(point2);
                                }
                                else
                                {
                                    int num23 = count - 1;
                                    num20 = count - 2;
                                    this.m_sinA = 0.0;
                                    this.m_normals[num23] = new DoublePoint(-this.m_normals[num23].X, -this.m_normals[num23].Y);
                                    if (node2.m_endtype == EndType.etOpenSquare)
                                    {
                                        this.DoSquare(num23, num20);
                                    }
                                    else
                                    {
                                        this.DoRound(num23, num20);
                                    }
                                }
                                for (int num24 = count - 1; num24 > 0; num24--)
                                {
                                    this.m_normals[num24] = new DoublePoint(-this.m_normals[num24 - 1].X, -this.m_normals[num24 - 1].Y);
                                }
                                this.m_normals[0] = new DoublePoint(-this.m_normals[1].X, -this.m_normals[1].Y);
                                num20 = count - 1;
                                for (int num25 = num20 - 1; num25 > 0; num25--)
                                {
                                    this.OffsetPoint(num25, ref num20, node2.m_jointype);
                                }
                                if (node2.m_endtype == EndType.etOpenButt)
                                {
                                    point2 = new IntPoint(Round(this.m_srcPoly[0].X - (this.m_normals[0].X * delta)), Round(this.m_srcPoly[0].Y - (this.m_normals[0].Y * delta)));
                                    this.m_destPoly.Add(point2);
                                    point2 = new IntPoint(Round(this.m_srcPoly[0].X + (this.m_normals[0].X * delta)), Round(this.m_srcPoly[0].Y + (this.m_normals[0].Y * delta)));
                                    this.m_destPoly.Add(point2);
                                }
                                else
                                {
                                    num20 = 1;
                                    this.m_sinA = 0.0;
                                    if (node2.m_endtype == EndType.etOpenSquare)
                                    {
                                        this.DoSquare(0, 1);
                                    }
                                    else
                                    {
                                        this.DoRound(0, 1);
                                    }
                                }
                                this.m_destPolys.Add(this.m_destPoly);
                            }
                        }
                    }
                }
            }
        }

        internal void DoRound(int j, int k)
        {
            double num = Math.Atan2(this.m_sinA, (this.m_normals[k].X * this.m_normals[j].X) + (this.m_normals[k].Y * this.m_normals[j].Y));
            int num2 = Math.Max((int) Round(this.m_StepsPerRad * Math.Abs(num)), 1);
            double x = this.m_normals[k].X;
            double y = this.m_normals[k].Y;
            for (int i = 0; i < num2; i++)
            {
                this.m_destPoly.Add(new IntPoint(Round(this.m_srcPoly[j].X + (x * this.m_delta)), Round(this.m_srcPoly[j].Y + (y * this.m_delta))));
                double num5 = x;
                x = (x * this.m_cos) - (this.m_sin * y);
                y = (num5 * this.m_sin) + (y * this.m_cos);
            }
            this.m_destPoly.Add(new IntPoint(Round(this.m_srcPoly[j].X + (this.m_normals[j].X * this.m_delta)), Round(this.m_srcPoly[j].Y + (this.m_normals[j].Y * this.m_delta))));
        }

        internal void DoSquare(int j, int k)
        {
            double num = Math.Tan(Math.Atan2(this.m_sinA, (this.m_normals[k].X * this.m_normals[j].X) + (this.m_normals[k].Y * this.m_normals[j].Y)) / 4.0);
            this.m_destPoly.Add(new IntPoint(Round(this.m_srcPoly[j].X + (this.m_delta * (this.m_normals[k].X - (this.m_normals[k].Y * num)))), Round(this.m_srcPoly[j].Y + (this.m_delta * (this.m_normals[k].Y + (this.m_normals[k].X * num))))));
            this.m_destPoly.Add(new IntPoint(Round(this.m_srcPoly[j].X + (this.m_delta * (this.m_normals[j].X + (this.m_normals[j].Y * num)))), Round(this.m_srcPoly[j].Y + (this.m_delta * (this.m_normals[j].Y - (this.m_normals[j].X * num))))));
        }

        public void Execute(ref PolyTree solution, double delta)
        {
            solution.Clear();
            this.FixOrientations();
            this.DoOffset(delta);
            Clipper clipper = new Clipper(0);
            clipper.AddPaths(this.m_destPolys, PolyType.ptSubject, true);
            if (delta > 0.0)
            {
                clipper.Execute(ClipType.ctUnion, solution, PolyFillType.pftPositive, PolyFillType.pftPositive);
            }
            else
            {
                IntRect bounds = ClipperBase.GetBounds(this.m_destPolys);
                List<IntPoint> pg = new List<IntPoint>(4) {
                    new IntPoint(bounds.left - 10L, bounds.bottom + 10L),
                    new IntPoint(bounds.right + 10L, bounds.bottom + 10L),
                    new IntPoint(bounds.right + 10L, bounds.top - 10L),
                    new IntPoint(bounds.left - 10L, bounds.top - 10L)
                };
                clipper.AddPath(pg, PolyType.ptSubject, true);
                clipper.ReverseSolution = true;
                clipper.Execute(ClipType.ctUnion, solution, PolyFillType.pftNegative, PolyFillType.pftNegative);
                if ((solution.ChildCount == 1) && (solution.Childs[0].ChildCount > 0))
                {
                    PolyNode node = solution.Childs[0];
                    solution.Childs.Capacity = node.ChildCount;
                    solution.Childs[0] = node.Childs[0];
                    solution.Childs[0].m_Parent = solution;
                    for (int i = 1; i < node.ChildCount; i++)
                    {
                        solution.AddChild(node.Childs[i]);
                    }
                }
                else
                {
                    solution.Clear();
                }
            }
        }

        public void Execute(ref List<List<IntPoint>> solution, double delta)
        {
            solution.Clear();
            this.FixOrientations();
            this.DoOffset(delta);
            Clipper clipper = new Clipper(0);
            clipper.AddPaths(this.m_destPolys, PolyType.ptSubject, true);
            if (delta > 0.0)
            {
                clipper.Execute(ClipType.ctUnion, solution, PolyFillType.pftPositive, PolyFillType.pftPositive);
            }
            else
            {
                IntRect bounds = ClipperBase.GetBounds(this.m_destPolys);
                List<IntPoint> pg = new List<IntPoint>(4) {
                    new IntPoint(bounds.left - 10L, bounds.bottom + 10L),
                    new IntPoint(bounds.right + 10L, bounds.bottom + 10L),
                    new IntPoint(bounds.right + 10L, bounds.top - 10L),
                    new IntPoint(bounds.left - 10L, bounds.top - 10L)
                };
                clipper.AddPath(pg, PolyType.ptSubject, true);
                clipper.ReverseSolution = true;
                clipper.Execute(ClipType.ctUnion, solution, PolyFillType.pftNegative, PolyFillType.pftNegative);
                if (solution.Count > 0)
                {
                    solution.RemoveAt(0);
                }
            }
        }

        private void FixOrientations()
        {
            if ((this.m_lowest.X >= 0L) && !Clipper.Orientation(this.m_polyNodes.Childs[(int) this.m_lowest.X].m_polygon))
            {
                for (int i = 0; i < this.m_polyNodes.ChildCount; i++)
                {
                    PolyNode node = this.m_polyNodes.Childs[i];
                    if ((node.m_endtype == EndType.etClosedPolygon) || ((node.m_endtype == EndType.etClosedLine) && Clipper.Orientation(node.m_polygon)))
                    {
                        node.m_polygon.Reverse();
                    }
                }
            }
            else
            {
                for (int j = 0; j < this.m_polyNodes.ChildCount; j++)
                {
                    PolyNode node2 = this.m_polyNodes.Childs[j];
                    if ((node2.m_endtype == EndType.etClosedLine) && !Clipper.Orientation(node2.m_polygon))
                    {
                        node2.m_polygon.Reverse();
                    }
                }
            }
        }

        internal static DoublePoint GetUnitNormal(IntPoint pt1, IntPoint pt2)
        {
            double num = pt2.X - pt1.X;
            double num2 = pt2.Y - pt1.Y;
            if ((num == 0.0) && (num2 == 0.0))
            {
                return new DoublePoint();
            }
            double num3 = 1.0 / Math.Sqrt((num * num) + (num2 * num2));
            num *= num3;
            return new DoublePoint(num2 * num3, -num);
        }

        private void OffsetPoint(int j, ref int k, JoinType jointype)
        {
            this.m_sinA = (this.m_normals[k].X * this.m_normals[j].Y) - (this.m_normals[j].X * this.m_normals[k].Y);
            if (Math.Abs((double) (this.m_sinA * this.m_delta)) < 1.0)
            {
                double num = (this.m_normals[k].X * this.m_normals[j].X) + (this.m_normals[j].Y * this.m_normals[k].Y);
                if (num > 0.0)
                {
                    this.m_destPoly.Add(new IntPoint(Round(this.m_srcPoly[j].X + (this.m_normals[k].X * this.m_delta)), Round(this.m_srcPoly[j].Y + (this.m_normals[k].Y * this.m_delta))));
                    return;
                }
            }
            else if (this.m_sinA > 1.0)
            {
                this.m_sinA = 1.0;
            }
            else if (this.m_sinA < -1.0)
            {
                this.m_sinA = -1.0;
            }
            if ((this.m_sinA * this.m_delta) < 0.0)
            {
                this.m_destPoly.Add(new IntPoint(Round(this.m_srcPoly[j].X + (this.m_normals[k].X * this.m_delta)), Round(this.m_srcPoly[j].Y + (this.m_normals[k].Y * this.m_delta))));
                this.m_destPoly.Add(this.m_srcPoly[j]);
                this.m_destPoly.Add(new IntPoint(Round(this.m_srcPoly[j].X + (this.m_normals[j].X * this.m_delta)), Round(this.m_srcPoly[j].Y + (this.m_normals[j].Y * this.m_delta))));
            }
            else
            {
                switch (jointype)
                {
                    case JoinType.jtSquare:
                        this.DoSquare(j, k);
                        break;

                    case JoinType.jtRound:
                        this.DoRound(j, k);
                        break;

                    case JoinType.jtMiter:
                    {
                        double r = 1.0 + ((this.m_normals[j].X * this.m_normals[k].X) + (this.m_normals[j].Y * this.m_normals[k].Y));
                        if (r < this.m_miterLim)
                        {
                            this.DoSquare(j, k);
                            break;
                        }
                        this.DoMiter(j, k, r);
                        break;
                    }
                }
            }
            k = j;
        }

        internal static long Round(double value) => 
            ((value < 0.0) ? ((long) (value - 0.5)) : ((long) (value + 0.5)));

        public double ArcTolerance { get; set; }

        public double MiterLimit { get; set; }
    }
}

