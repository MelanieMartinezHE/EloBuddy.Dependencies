namespace ClipperLib
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct Int128
    {
        private long hi;
        private ulong lo;
        public Int128(long _lo)
        {
            this.lo = (ulong) _lo;
            if (_lo < 0L)
            {
                this.hi = -1L;
            }
            else
            {
                this.hi = 0L;
            }
        }

        public Int128(long _hi, ulong _lo)
        {
            this.lo = _lo;
            this.hi = _hi;
        }

        public Int128(Int128 val)
        {
            this.hi = val.hi;
            this.lo = val.lo;
        }

        public bool IsNegative() => 
            (this.hi < 0L);

        public static bool operator ==(Int128 val1, Int128 val2)
        {
            if (val1 == val2)
            {
                return true;
            }
            if ((val1 == 0) || (val2 == 0))
            {
                return false;
            }
            return ((val1.hi == val2.hi) && (val1.lo == val2.lo));
        }

        public static bool operator !=(Int128 val1, Int128 val2) => 
            !(val1 == val2);

        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is Int128))
            {
                return false;
            }
            Int128 num = (Int128) obj;
            return ((num.hi == this.hi) && (num.lo == this.lo));
        }

        public override int GetHashCode() => 
            (this.hi.GetHashCode() ^ this.lo.GetHashCode());

        public static bool operator >(Int128 val1, Int128 val2)
        {
            if (val1.hi != val2.hi)
            {
                return (val1.hi > val2.hi);
            }
            return (val1.lo > val2.lo);
        }

        public static bool operator <(Int128 val1, Int128 val2)
        {
            if (val1.hi != val2.hi)
            {
                return (val1.hi < val2.hi);
            }
            return (val1.lo < val2.lo);
        }

        public static Int128 operator +(Int128 lhs, Int128 rhs)
        {
            lhs.hi += rhs.hi;
            lhs.lo += rhs.lo;
            if (lhs.lo < rhs.lo)
            {
                lhs.hi += 1L;
            }
            return lhs;
        }

        public static Int128 operator -(Int128 lhs, Int128 rhs) => 
            (lhs + -rhs);

        public static Int128 operator -(Int128 val)
        {
            if (val.lo == 0L)
            {
                return new Int128(-val.hi, 0L);
            }
            return new Int128(~val.hi, ~val.lo + ((ulong) 1L));
        }

        public static explicit operator double(Int128 val)
        {
            if (val.hi < 0L)
            {
                if (val.lo == 0L)
                {
                    return (val.hi * 1.8446744073709552E+19);
                }
                return -(~val.lo + (~val.hi * 1.8446744073709552E+19));
            }
            return (val.lo + (val.hi * 1.8446744073709552E+19));
        }

        public static Int128 Int128Mul(long lhs, long rhs)
        {
            bool flag = (lhs < 0L) != (rhs < 0L);
            if (lhs < 0L)
            {
                lhs = -lhs;
            }
            if (rhs < 0L)
            {
                rhs = -rhs;
            }
            ulong num = (ulong) (lhs >> 0x20);
            ulong num2 = ((ulong) lhs) & 0xffffffffL;
            ulong num3 = (ulong) (rhs >> 0x20);
            ulong num4 = ((ulong) rhs) & 0xffffffffL;
            ulong num5 = num * num3;
            ulong num6 = num2 * num4;
            ulong num7 = (num * num4) + (num2 * num3);
            long num9 = (long) (num5 + (num7 >> 0x20));
            ulong num8 = (num7 << 0x20) + num6;
            if (num8 < num6)
            {
                num9 += 1L;
            }
            Int128 num10 = new Int128(num9, num8);
            return (flag ? -num10 : num10);
        }
    }
}

