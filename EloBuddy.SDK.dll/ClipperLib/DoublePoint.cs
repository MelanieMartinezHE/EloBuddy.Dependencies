﻿namespace ClipperLib
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DoublePoint
    {
        public double X;
        public double Y;
        public DoublePoint(double x = 0.0, double y = 0.0)
        {
            this.X = x;
            this.Y = y;
        }

        public DoublePoint(DoublePoint dp)
        {
            this.X = dp.X;
            this.Y = dp.Y;
        }

        public DoublePoint(IntPoint ip)
        {
            this.X = ip.X;
            this.Y = ip.Y;
        }
    }
}

