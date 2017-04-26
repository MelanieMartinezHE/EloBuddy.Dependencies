namespace ClipperLib
{
    using System;

    internal class LocalMinima
    {
        internal TEdge LeftBound;
        internal LocalMinima Next;
        internal TEdge RightBound;
        internal long Y;
    }
}

