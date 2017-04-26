namespace EloBuddy.SDK
{
    using ClipperLib;
    using EloBuddy;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class Geometry
    {
        public static float AngleBetween(this Vector2 p1, Vector2 p2)
        {
            float num = p1.Polar() - p2.Polar();
            if (num < 0f)
            {
                num += 360f;
            }
            if (num > 180f)
            {
                num = 360f - num;
            }
            return num;
        }

        public static Vector2 CenterOfPolygon(this Polygon p)
        {
            float num = 0f;
            float num2 = 0f;
            int count = p.Points.Count;
            foreach (Vector2 vector in p.Points)
            {
                num += vector.X;
                num2 += vector.Y;
            }
            return new Vector2(num / ((float) count), num2 / ((float) count));
        }

        public static Vector2 CenterPoint(this Vector2[] points)
        {
            if (points.Length == 0)
            {
                return Vector2.Zero;
            }
            return new Vector2(points.Sum<Vector2>(((Func<Vector2, float>) (v => v.X))) / ((float) points.Length), points.Sum<Vector2>(((Func<Vector2, float>) (v => v.Y))) / ((float) points.Length));
        }

        public static List<List<IntPoint>> ClipPolygons(IEnumerable<Polygon> polygons)
        {
            Polygon[] polygonArray = (polygons as Polygon[]) ?? polygons.ToArray<Polygon>();
            List<List<IntPoint>> ppg = new List<List<IntPoint>>(polygonArray.Length);
            List<List<IntPoint>> list2 = new List<List<IntPoint>>(polygonArray.Length);
            foreach (Polygon polygon in polygonArray)
            {
                ppg.Add(polygon.ToClipperPath());
                list2.Add(polygon.ToClipperPath());
            }
            List<List<IntPoint>> solution = new List<List<IntPoint>>();
            Clipper clipper = new Clipper(0);
            clipper.AddPaths(ppg, PolyType.ptSubject, true);
            clipper.AddPaths(list2, PolyType.ptClip, true);
            clipper.Execute(ClipType.ctUnion, solution, PolyFillType.pftPositive, PolyFillType.pftEvenOdd);
            return solution;
        }

        public static bool Close(float a, float b, float eps)
        {
            if (Math.Abs(eps) < float.Epsilon)
            {
                eps = 1E-09f;
            }
            return (Math.Abs((float) (a - b)) <= eps);
        }

        public static Vector2 Closest(this Vector2 v, IEnumerable<Vector2> vList)
        {
            Vector2 vector = new Vector2();
            float maxValue = float.MaxValue;
            foreach (Vector2 vector2 in vList)
            {
                float num2 = v.Distance(vector2, true);
                if (num2 < maxValue)
                {
                    maxValue = num2;
                    vector = vector2;
                }
            }
            return vector;
        }

        public static float CrossProduct(this Vector2 self, Vector2 other) => 
            ((other.Y * self.X) - (other.X * self.Y));

        public static float DegreeToRadian(double angle) => 
            ((float) ((3.1415926535897931 * angle) / 180.0));

        public static float Distance(this Vector2 point, Vector2 segmentStart, Vector2 segmentEnd, bool onlyIfOnSegment = false, bool squared = false)
        {
            ProjectionInfo info = point.ProjectOn(segmentStart, segmentEnd);
            if (info.IsOnSegment || !onlyIfOnSegment)
            {
                return (squared ? Vector2.DistanceSquared(info.SegmentPoint, point) : Vector2.Distance(info.SegmentPoint, point));
            }
            return float.MaxValue;
        }

        public static float DotProduct(this Vector2 self, Vector2 other) => 
            ((self.X * other.X) + (self.Y * other.Y));

        public static IntersectionResult Intersection(this Vector2 lineSegment1Start, Vector2 lineSegment1End, Vector2 lineSegment2Start, Vector2 lineSegment2End)
        {
            double num = lineSegment1Start.Y - lineSegment2Start.Y;
            double num2 = lineSegment2End.X - lineSegment2Start.X;
            double num3 = lineSegment1Start.X - lineSegment2Start.X;
            double num4 = lineSegment2End.Y - lineSegment2Start.Y;
            double num5 = lineSegment1End.X - lineSegment1Start.X;
            double num6 = lineSegment1End.Y - lineSegment1Start.Y;
            double num7 = (num5 * num4) - (num6 * num2);
            double num8 = (num * num2) - (num3 * num4);
            if (Math.Abs(num7) < 1.4012984643248171E-45)
            {
                if (Math.Abs(num8) < 1.4012984643248171E-45)
                {
                    if ((lineSegment1Start.X >= lineSegment2Start.X) && (lineSegment1Start.X <= lineSegment2End.X))
                    {
                        return new IntersectionResult(true, lineSegment1Start);
                    }
                    if ((lineSegment2Start.X >= lineSegment1Start.X) && (lineSegment2Start.X <= lineSegment1End.X))
                    {
                        return new IntersectionResult(true, lineSegment2Start);
                    }
                    return new IntersectionResult();
                }
                return new IntersectionResult();
            }
            double num9 = num8 / num7;
            if ((num9 < 0.0) || (num9 > 1.0))
            {
                return new IntersectionResult();
            }
            double num10 = ((num * num5) - (num3 * num6)) / num7;
            if ((num10 < 0.0) || (num10 > 1.0))
            {
                return new IntersectionResult();
            }
            return new IntersectionResult(true, new Vector2(lineSegment1Start.X + ((float) (num9 * num5)), lineSegment1Start.Y + ((float) (num9 * num6))));
        }

        public static List<Polygon> JoinPolygons(this List<Polygon> sList)
        {
            List<List<IntPoint>> ppg = ClipPolygons(sList);
            List<List<IntPoint>> solution = new List<List<IntPoint>>();
            Clipper clipper = new Clipper(0);
            clipper.AddPaths(ppg, PolyType.ptClip, true);
            clipper.Execute(ClipType.ctUnion, solution, PolyFillType.pftNonZero, PolyFillType.pftNonZero);
            return solution.ToPolygons();
        }

        public static List<Polygon> JoinPolygons(this List<Polygon> sList, ClipType cType, PolyType pType = 1, PolyFillType pFType1 = 1, PolyFillType pFType2 = 1)
        {
            List<List<IntPoint>> ppg = ClipPolygons(sList);
            List<List<IntPoint>> solution = new List<List<IntPoint>>();
            Clipper clipper = new Clipper(0);
            clipper.AddPaths(ppg, pType, true);
            clipper.Execute(cType, solution, pFType1, pFType2);
            return solution.ToPolygons();
        }

        public static int LineCircleIntersection(float cX, float cY, float radius, Vector2 segmentStart, Vector2 segmentEnd, out Vector2 intersection1, out Vector2 intersection2)
        {
            float num;
            float num2 = segmentEnd.X - segmentStart.X;
            float num3 = segmentEnd.Y - segmentStart.Y;
            float num4 = (num2 * num2) + (num3 * num3);
            float num5 = 2f * ((num2 * (segmentStart.X - cX)) + (num3 * (segmentStart.Y - cY)));
            float num6 = (((segmentStart.X - cX) * (segmentStart.X - cX)) + ((segmentStart.Y - cY) * (segmentStart.Y - cY))) - (radius * radius);
            float num7 = (num5 * num5) - ((4f * num4) * num6);
            if ((Math.Abs(num4) <= float.Epsilon) || (num7 < 0f))
            {
                intersection1 = new Vector2(float.NaN, float.NaN);
                intersection2 = new Vector2(float.NaN, float.NaN);
                return 0;
            }
            if (num7 == 0f)
            {
                num = -num5 / (2f * num4);
                intersection1 = new Vector2(segmentStart.X + (num * num2), segmentStart.Y + (num * num3));
                intersection2 = new Vector2(float.NaN, float.NaN);
                return 1;
            }
            num = (float) ((-num5 + Math.Sqrt((double) num7)) / ((double) (2f * num4)));
            intersection1 = new Vector2(segmentStart.X + (num * num2), segmentStart.Y + (num * num3));
            num = (float) ((-num5 - Math.Sqrt((double) num7)) / ((double) (2f * num4)));
            intersection2 = new Vector2(segmentStart.X + (num * num2), segmentStart.Y + (num * num3));
            return 2;
        }

        public static int LineSegmentCircleIntersection(float cX, float cY, float radius, Vector2 segmentStart, Vector2 segmentEnd, out Vector2 intersection1, out Vector2 intersection2)
        {
            int num = LineCircleIntersection(cX, cY, radius, segmentStart, segmentEnd, out intersection1, out intersection2);
            Vector2 vector = new Vector2(float.NaN, float.NaN);
            if (!PointInLineSegment(segmentStart, segmentEnd, intersection2))
            {
                intersection2 = vector;
                num--;
            }
            if (!PointInLineSegment(segmentStart, segmentEnd, intersection1))
            {
                intersection1 = intersection2;
                num--;
            }
            return num;
        }

        public static float Magnitude(this Vector2 self, bool squared = false)
        {
            if (squared)
            {
                return (self.X.Pow() + self.Y.Pow());
            }
            return (float) Math.Sqrt((double) (self.X.Pow() + self.Y.Pow()));
        }

        public static Polygon MovePolygon(this Polygon polygon, Vector2 moveTo)
        {
            Polygon polygon2 = new Polygon();
            polygon2.Add(moveTo);
            int count = polygon.Points.Count;
            Vector2 vector = polygon.Points[0];
            for (int i = 1; i < count; i++)
            {
                Vector2 vector2 = polygon.Points[i];
                polygon2.Add(new Vector2(moveTo.X + (vector2.X - vector.X), moveTo.Y + (vector2.Y - vector.Y)));
            }
            return polygon2;
        }

        public static float PathLength(this List<Vector2> path)
        {
            float num = 0f;
            for (int i = 0; i < (path.Count - 1); i++)
            {
                num += path[i].Distance(path[i + 1], false);
            }
            return num;
        }

        public static float PathLength(this List<Vector3> path)
        {
            float num = 0f;
            for (int i = 0; i < (path.Count - 1); i++)
            {
                num += path[i].Distance(path[i + 1], false);
            }
            return num;
        }

        public static Vector2 Perpendicular(this Vector2 v) => 
            new Vector2(-v.Y, v.X);

        public static Vector2 Perpendicular2(this Vector2 v) => 
            new Vector2(v.Y, -v.X);

        public static bool PointInLineSegment(Vector2 segmentStart, Vector2 segmentEnd, Vector2 point)
        {
            float num = ((point.Y - segmentStart.Y) * (segmentEnd.X - segmentStart.X)) - ((point.X - segmentStart.X) * (segmentEnd.Y - segmentStart.Y));
            if (Math.Abs(num) > 2f)
            {
                return false;
            }
            float num2 = ((point.X - segmentStart.X) * (segmentEnd.X - segmentStart.X)) + ((point.Y - segmentStart.Y) * (segmentEnd.Y - segmentStart.Y));
            if (num2 < 0f)
            {
                return false;
            }
            float num3 = ((segmentEnd.X - segmentStart.X) * (segmentEnd.X - segmentStart.X)) + ((segmentEnd.Y - segmentStart.Y) * (segmentEnd.Y - segmentStart.Y));
            if (num2 > num3)
            {
                return false;
            }
            return true;
        }

        public static float Polar(this Vector2 v1)
        {
            if (Close(v1.X, 0f, 0f))
            {
                if (v1.Y > 0f)
                {
                    return 90f;
                }
                return ((v1.Y < 0f) ? ((float) 270) : ((float) 0));
            }
            float num = RadianToDegree(Math.Atan((double) (v1.Y / v1.X)));
            if (v1.X < 0f)
            {
                num += 180f;
            }
            if (num < 0f)
            {
                num += 360f;
            }
            return num;
        }

        public static Vector2 PositionAfter(this IEnumerable<Vector2> self, int t, int s, int delay = 0)
        {
            List<Vector2> list = self.ToList<Vector2>();
            int num = (Math.Max(0, t - delay) * s) / 0x3e8;
            for (int i = 0; i <= (list.Count - 2); i++)
            {
                Vector2 vector = list[i];
                Vector2 vector2 = list[i + 1];
                int num3 = (int) vector2.Distance(vector, false);
                if (num3 > num)
                {
                    return (vector + ((Vector2) (num * (vector2 - vector).Normalized())));
                }
                num -= num3;
            }
            return list[list.Count - 1];
        }

        public static ProjectionInfo ProjectOn(this Vector2 point, Vector2 segmentStart, Vector2 segmentEnd)
        {
            float num8;
            float x = point.X;
            float y = point.Y;
            float num3 = segmentStart.X;
            float num4 = segmentStart.Y;
            float num5 = segmentEnd.X;
            float num6 = segmentEnd.Y;
            float num7 = (((x - num3) * (num5 - num3)) + ((y - num4) * (num6 - num4))) / (((float) (num5 - num3)).Pow() + ((float) (num6 - num4)).Pow());
            Vector2 linePoint = new Vector2(num3 + (num7 * (num5 - num3)), num4 + (num7 * (num6 - num4)));
            if (num7 < 0f)
            {
                num8 = 0f;
            }
            else if (num7 > 1f)
            {
                num8 = 1f;
            }
            else
            {
                num8 = num7;
            }
            bool isOnSegment = num8.CompareTo(num7) == 0;
            return new ProjectionInfo(isOnSegment, isOnSegment ? linePoint : new Vector2(num3 + (num8 * (num5 - num3)), num4 + (num8 * (num6 - num4))), linePoint);
        }

        public static float RadianToDegree(double angle) => 
            ((float) (angle * 57.295779513082323));

        public static Vector2 RotateAroundPoint(this Vector2 rotated, Vector2 around, float angle)
        {
            double num = Math.Sin((double) angle);
            double num2 = Math.Cos((double) angle);
            double num3 = (((rotated.X - around.X) * num2) - ((around.Y - rotated.Y) * num)) + around.X;
            double num4 = (((around.Y - rotated.Y) * num2) + ((rotated.X - around.X) * num)) + around.Y;
            return new Vector2((float) num3, (float) num4);
        }

        public static Vector2 Rotated(this Vector2 v, float angle)
        {
            double num = Math.Cos((double) angle);
            double num2 = Math.Sin((double) angle);
            return new Vector2((float) ((v.X * num) - (v.Y * num2)), (float) ((v.Y * num) + (v.X * num2)));
        }

        public static Polygon RotatePolygon(this Polygon polygon, Vector2 around, Vector2 direction)
        {
            float num = around.X - direction.X;
            float num2 = around.Y - direction.Y;
            float num3 = (float) Math.Atan2((double) num2, (double) num);
            return polygon.RotatePolygon(around, ((float) (num3 - DegreeToRadian(90.0))));
        }

        public static Polygon RotatePolygon(this Polygon polygon, Vector2 around, float angle)
        {
            Func<Vector2, Vector2> <>9__0;
            Polygon polygon2 = new Polygon();
            foreach (Vector2 vector in polygon.Points.Select<Vector2, Vector2>((Func<Vector2, Vector2>) (<>9__0 ?? (<>9__0 = poinit => poinit.RotateAroundPoint(around, angle)))))
            {
                polygon2.Add(vector);
            }
            return polygon2;
        }

        public static int SegmentCircleIntersectionPriority(Vector2 segmentStart, Vector2 segmentEnd, Vector2 circlePos1, float radius1, Vector2 circlePos2, float radius2)
        {
            Vector2 vector;
            Vector2 vector2;
            Vector2 vector3;
            Vector2 vector4;
            int num = LineSegmentCircleIntersection(circlePos1.X, circlePos1.Y, radius1, segmentStart, segmentEnd, out vector, out vector2);
            int num2 = LineSegmentCircleIntersection(circlePos2.X, circlePos2.Y, radius2, segmentStart, segmentEnd, out vector3, out vector4);
            if ((num == 0) && (num2 == 0))
            {
                return 0;
            }
            if ((num2 == 0) && (num > 0))
            {
                return 1;
            }
            if ((num == 0) && (num2 > 0))
            {
                return 2;
            }
            Vector2 vector5 = (num == 1) ? vector : ((segmentEnd.Distance(vector, true) > segmentEnd.Distance(vector2, true)) ? vector : vector2);
            Vector2 vector6 = (num2 == 1) ? vector3 : ((segmentEnd.Distance(vector3, true) > segmentEnd.Distance(vector4, true)) ? vector3 : vector4);
            return ((segmentEnd.Distance(vector5, true) > segmentEnd.Distance(vector6, true)) ? 1 : 2);
        }

        public static Vector3 SetZ(this Vector3 v, float? value = new float?())
        {
            if (!value.HasValue)
            {
                v.Z = Game.CursorPos.Z;
                return v;
            }
            v.Z = value.Value;
            return v;
        }

        public static Vector2 Shorten(this Vector2 v, Vector2 to, float distance) => 
            (v - ((Vector2) (distance * (to - v).Normalized())));

        public static Vector3 Shorten(this Vector3 v, Vector3 to, float distance) => 
            (v - ((Vector3) (distance * (to - v).Normalized())));

        public static Vector3 SwitchYZ(this Vector3 v) => 
            new Vector3(v.X, v.Z, v.Y);

        public static Polygon ToPolygon(this IEnumerable<IntPoint> v)
        {
            Polygon polygon = new Polygon();
            foreach (IntPoint point in v)
            {
                polygon.Add(new Vector2((float) point.X, (float) point.Y));
            }
            return polygon;
        }

        public static List<Polygon> ToPolygons(this IEnumerable<List<IntPoint>> v) => 
            (from path in v select path.ToPolygon()).ToList<Polygon>();

        public static object[] VectorMovementCollision(Vector2 startPoint1, Vector2 endPoint1, float v1, Vector2 startPoint2, float v2, float delay = 0f)
        {
            float x = startPoint1.X;
            float y = startPoint1.Y;
            float num3 = endPoint1.X;
            float num4 = endPoint1.Y;
            float num5 = startPoint2.X;
            float num6 = startPoint2.Y;
            float num7 = num3 - x;
            float num8 = num4 - y;
            float num9 = (float) Math.Sqrt((double) ((num7 * num7) + (num8 * num8)));
            float naN = float.NaN;
            float num11 = (Math.Abs(num9) > float.Epsilon) ? ((v1 * num7) / num9) : 0f;
            float num12 = (Math.Abs(num9) > float.Epsilon) ? ((v1 * num8) / num9) : 0f;
            float num13 = num5 - x;
            float num14 = num6 - y;
            float num15 = (num13 * num13) + (num14 * num14);
            if (num9 > 0f)
            {
                if (Math.Abs((float) (v1 - float.MaxValue)) < float.Epsilon)
                {
                    float num16 = num9 / v1;
                    naN = ((v2 * num16) >= 0f) ? num16 : float.NaN;
                }
                else if (Math.Abs((float) (v2 - float.MaxValue)) < float.Epsilon)
                {
                    naN = 0f;
                }
                else
                {
                    float num17 = ((num11 * num11) + (num12 * num12)) - (v2 * v2);
                    float num18 = (-num13 * num11) - (num14 * num12);
                    if (Math.Abs(num17) < float.Epsilon)
                    {
                        if (Math.Abs(num18) < float.Epsilon)
                        {
                            naN = (Math.Abs(num15) < float.Epsilon) ? 0f : float.NaN;
                        }
                        else
                        {
                            float num19 = -num15 / (2f * num18);
                            naN = ((v2 * num19) >= 0f) ? num19 : float.NaN;
                        }
                    }
                    else
                    {
                        float num20 = (num18 * num18) - (num17 * num15);
                        if (num20 >= 0f)
                        {
                            float num21 = (float) Math.Sqrt((double) num20);
                            float num22 = (-num21 - num18) / num17;
                            naN = ((v2 * num22) >= 0f) ? num22 : float.NaN;
                            num22 = (num21 - num18) / num17;
                            float f = ((v2 * num22) >= 0f) ? num22 : float.NaN;
                            if (!float.IsNaN(f) && !float.IsNaN(naN))
                            {
                                if ((naN >= delay) && (f >= delay))
                                {
                                    naN = Math.Min(naN, f);
                                }
                                else if (f >= delay)
                                {
                                    naN = f;
                                }
                            }
                        }
                    }
                }
            }
            else if (Math.Abs(num9) < float.Epsilon)
            {
                naN = 0f;
            }
            return new object[] { naN, (!float.IsNaN(naN) ? new Vector2(x + (num11 * naN), y + (num12 * naN)) : new Vector2()) };
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Geometry.<>c <>9 = new Geometry.<>c();
            public static Func<Vector2, float> <>9__25_0;
            public static Func<Vector2, float> <>9__25_1;
            public static Func<List<IntPoint>, Geometry.Polygon> <>9__34_0;

            internal float <CenterPoint>b__25_0(Vector2 v) => 
                v.X;

            internal float <CenterPoint>b__25_1(Vector2 v) => 
                v.Y;

            internal Geometry.Polygon <ToPolygons>b__34_0(List<IntPoint> path) => 
                path.ToPolygon();
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IntersectionResult
        {
            public bool Intersects;
            public Vector2 Point;
            public IntersectionResult(bool intersects = false, Vector2 point = new Vector2())
            {
                this.Intersects = intersects;
                this.Point = point;
            }
        }

        public class Polygon
        {
            public readonly List<Vector2> Points = new List<Vector2>();

            public void Add(Geometry.Polygon polygon)
            {
                foreach (Vector2 vector in polygon.Points)
                {
                    this.Points.Add(vector);
                }
            }

            public void Add(Vector2 point)
            {
                this.Points.Add(point);
            }

            public void Add(Vector3 point)
            {
                this.Points.Add(point.To2D());
            }

            public virtual void Draw(System.Drawing.Color color, int width = 1)
            {
                for (int i = 0; i <= (this.Points.Count - 1); i++)
                {
                    int num2 = ((this.Points.Count - 1) == i) ? 0 : (i + 1);
                    Vector2 vector = Drawing.WorldToScreen(this.Points[i].To3DWorld());
                    Vector2 vector2 = Drawing.WorldToScreen(this.Points[num2].To3DWorld());
                    Drawing.DrawLine(vector[0], vector[1], vector2[0], vector2[1], (float) width, color);
                }
            }

            public bool IsInside(GameObject point) => 
                !this.IsOutside(point.Position.To2D());

            public bool IsInside(Vector2 point) => 
                !this.IsOutside(point);

            public bool IsInside(Vector3 point) => 
                !this.IsOutside(point.To2D());

            public bool IsOutside(Vector2 point)
            {
                IntPoint pt = new IntPoint((double) point.X, (double) point.Y);
                return (Clipper.PointInPolygon(pt, this.ToClipperPath()) != 1);
            }

            public List<IntPoint> ToClipperPath()
            {
                List<IntPoint> list = new List<IntPoint>(this.Points.Count);
                list.AddRange(from point in this.Points select new IntPoint((double) point.X, (double) point.Y));
                return list;
            }

            [Serializable, CompilerGenerated]
            private sealed class <>c
            {
                public static readonly Geometry.Polygon.<>c <>9 = new Geometry.Polygon.<>c();
                public static Func<Vector2, IntPoint> <>9__4_0;

                internal IntPoint <ToClipperPath>b__4_0(Vector2 point) => 
                    new IntPoint((double) point.X, (double) point.Y);
            }

            public class Arc : Geometry.Polygon
            {
                private readonly int _quality;
                public float Angle;
                public Vector2 EndPos;
                public float Radius;
                public Vector2 StartPos;

                public Arc(Vector2 start, Vector2 direction, float angle, float radius, int quality = 20)
                {
                    this.StartPos = start;
                    this.EndPos = (direction - start).Normalized();
                    this.Angle = angle;
                    this.Radius = radius;
                    this._quality = quality;
                    this.UpdatePolygon(0);
                }

                public Arc(Vector3 start, Vector3 direction, float angle, float radius, int quality = 20) : this(start.To2D(), direction.To2D(), angle, radius, quality)
                {
                }

                public void UpdatePolygon(int offset = 0)
                {
                    base.Points.Clear();
                    float num = (this.Radius + offset) / ((float) Math.Cos(6.2831853071795862 / ((double) this._quality)));
                    Vector2 v = this.EndPos.Rotated(-this.Angle * 0.5f);
                    for (int i = 0; i <= this._quality; i++)
                    {
                        Vector2 vector2 = v.Rotated(((i * this.Angle) / ((float) this._quality))).Normalized();
                        base.Points.Add(new Vector2(this.StartPos.X + (num * vector2.X), this.StartPos.Y + (num * vector2.Y)));
                    }
                }
            }

            public class Circle : Geometry.Polygon
            {
                private readonly int _quality;
                public Vector2 Center;
                public float Radius;

                public Circle(Vector2 center, float radius, int quality = 20)
                {
                    this.Center = center;
                    this.Radius = radius;
                    this._quality = quality;
                    this.UpdatePolygon(0, -1f);
                }

                public Circle(Vector3 center, float radius, int quality = 20) : this(center.To2D(), radius, quality)
                {
                }

                public void UpdatePolygon(int offset = 0, float overrideWidth = -1f)
                {
                    base.Points.Clear();
                    float num = (overrideWidth > 0f) ? overrideWidth : ((offset + this.Radius) / ((float) Math.Cos(6.2831853071795862 / ((double) this._quality))));
                    for (int i = 1; i <= this._quality; i++)
                    {
                        double d = ((i * 2) * 3.1415926535897931) / ((double) this._quality);
                        Vector2 item = new Vector2(this.Center.X + (num * ((float) Math.Cos(d))), this.Center.Y + (num * ((float) Math.Sin(d))));
                        base.Points.Add(item);
                    }
                }
            }

            public class Line : Geometry.Polygon
            {
                public Vector2 LineEnd;
                public Vector2 LineStart;

                public Line(Vector2 start, Vector2 end, float length = -1f)
                {
                    this.LineStart = start;
                    this.LineEnd = end;
                    if (length > 0f)
                    {
                        this.Length = length;
                    }
                    this.UpdatePolygon();
                }

                public Line(Vector3 start, Vector3 end, float length = -1f) : this(start.To2D(), end.To2D(), length)
                {
                }

                public void UpdatePolygon()
                {
                    base.Points.Clear();
                    base.Points.Add(this.LineStart);
                    base.Points.Add(this.LineEnd);
                }

                public float Length
                {
                    get => 
                        this.LineStart.Distance(this.LineEnd, false);
                    set
                    {
                        this.LineEnd = ((Vector2) ((this.LineEnd - this.LineStart).Normalized() * value)) + this.LineStart;
                    }
                }
            }

            public class Rectangle : Geometry.Polygon
            {
                public Vector2 End;
                public Vector2 Start;
                public float Width;

                public Rectangle(Vector2 start, Vector2 end, float width)
                {
                    this.Start = start;
                    this.End = end;
                    this.Width = width;
                    this.UpdatePolygon(0, -1f);
                }

                public Rectangle(Vector3 start, Vector3 end, float width) : this(start.To2D(), end.To2D(), width)
                {
                }

                public void UpdatePolygon(int offset = 0, float overrideWidth = -1f)
                {
                    base.Points.Clear();
                    base.Points.Add((Vector2) ((this.Start + (((overrideWidth > 0f) ? overrideWidth : (this.Width + offset)) * this.Perpendicular)) - (offset * this.Direction)));
                    base.Points.Add((Vector2) ((this.Start - (((overrideWidth > 0f) ? overrideWidth : (this.Width + offset)) * this.Perpendicular)) - (offset * this.Direction)));
                    base.Points.Add((Vector2) ((this.End - (((overrideWidth > 0f) ? overrideWidth : (this.Width + offset)) * this.Perpendicular)) + (offset * this.Direction)));
                    base.Points.Add((Vector2) ((this.End + (((overrideWidth > 0f) ? overrideWidth : (this.Width + offset)) * this.Perpendicular)) + (offset * this.Direction)));
                }

                public Vector2 Direction =>
                    (this.End - this.Start).Normalized();

                public Vector2 Perpendicular =>
                    this.Direction.Perpendicular();
            }

            public class Ring : Geometry.Polygon
            {
                private readonly int _quality;
                public Vector2 Center;
                public float InnerRadius;
                public float OuterRadius;

                public Ring(Vector2 center, float innerRadius, float outerRadius, int quality = 20)
                {
                    this.Center = center;
                    this.InnerRadius = innerRadius;
                    this.OuterRadius = outerRadius;
                    this._quality = quality;
                    this.UpdatePolygon(0);
                }

                public Ring(Vector3 center, float innerRadius, float outerRadius, int quality = 20) : this(center.To2D(), innerRadius, outerRadius, quality)
                {
                }

                public void UpdatePolygon(int offset = 0)
                {
                    base.Points.Clear();
                    float num = ((offset + this.InnerRadius) + this.OuterRadius) / ((float) Math.Cos(6.2831853071795862 / ((double) this._quality)));
                    float num2 = (this.InnerRadius - this.OuterRadius) - offset;
                    for (int i = 0; i <= this._quality; i++)
                    {
                        double d = ((i * 2) * 3.1415926535897931) / ((double) this._quality);
                        Vector2 item = new Vector2(this.Center.X - (num * ((float) Math.Cos(d))), this.Center.Y - (num * ((float) Math.Sin(d))));
                        base.Points.Add(item);
                    }
                    for (int j = 0; j <= this._quality; j++)
                    {
                        double num6 = ((j * 2) * 3.1415926535897931) / ((double) this._quality);
                        Vector2 vector2 = new Vector2(this.Center.X + (num2 * ((float) Math.Cos(num6))), this.Center.Y - (num2 * ((float) Math.Sin(num6))));
                        base.Points.Add(vector2);
                    }
                }
            }

            public class Sector : Geometry.Polygon
            {
                private readonly int _quality;
                public float Angle;
                public Vector2 Center;
                public Vector2 Direction;
                public float Radius;

                public Sector(Vector2 center, Vector2 direction, float angle, float radius, int quality = 20)
                {
                    this.Center = center;
                    this.Direction = (direction - center).Normalized();
                    this.Angle = angle;
                    this.Radius = radius;
                    this._quality = quality;
                    this.UpdatePolygon(0);
                }

                public Sector(Vector3 center, Vector3 direction, float angle, float radius, int quality = 20) : this(center.To2D(), direction.To2D(), angle, radius, quality)
                {
                }

                public void UpdatePolygon(int offset = 0)
                {
                    base.Points.Clear();
                    float num = (this.Radius + offset) / ((float) Math.Cos(6.2831853071795862 / ((double) this._quality)));
                    base.Points.Add(this.Center);
                    Vector2 v = this.Direction.Rotated(-this.Angle * 0.5f);
                    for (int i = 0; i <= this._quality; i++)
                    {
                        Vector2 vector2 = v.Rotated(((i * this.Angle) / ((float) this._quality))).Normalized();
                        base.Points.Add(new Vector2(this.Center.X + (num * vector2.X), this.Center.Y + (num * vector2.Y)));
                    }
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ProjectionInfo
        {
            public bool IsOnSegment;
            public Vector2 LinePoint;
            public Vector2 SegmentPoint;
            public ProjectionInfo(bool isOnSegment, Vector2 segmentPoint, Vector2 linePoint)
            {
                this.IsOnSegment = isOnSegment;
                this.SegmentPoint = segmentPoint;
                this.LinePoint = linePoint;
            }
        }
    }
}

