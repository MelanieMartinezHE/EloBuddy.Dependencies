namespace ClipperLib
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct IntPoint
    {
        public long X;
        public long Y;
        public IntPoint(long X, long Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public IntPoint(double x, double y)
        {
            this.X = (long) x;
            this.Y = (long) y;
        }

        public IntPoint(IntPoint pt)
        {
            this.X = pt.X;
            this.Y = pt.Y;
        }

        public static bool operator ==(IntPoint a, IntPoint b) => 
            ((a.X == b.X) && (a.Y == b.Y));

        public static bool operator !=(IntPoint a, IntPoint b) => 
            ((a.X != b.X) || (a.Y != b.Y));

        public override bool Equals(object obj)
        {
            if ((obj != null) && (obj is IntPoint))
            {
                IntPoint point = (IntPoint) obj;
                return ((this.X == point.X) && (this.Y == point.Y));
            }
            return false;
        }

        public override int GetHashCode() => 
            base.GetHashCode();
    }
}

