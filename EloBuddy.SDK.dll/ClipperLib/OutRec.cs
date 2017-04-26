namespace ClipperLib
{
    using System;

    internal class OutRec
    {
        internal OutPt BottomPt;
        internal OutRec FirstLeft;
        internal int Idx;
        internal bool IsHole;
        internal bool IsOpen;
        internal ClipperLib.PolyNode PolyNode;
        internal OutPt Pts;
    }
}

