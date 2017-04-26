namespace EloBuddy
{
    using EloBuddy.Native;
    using std;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class SpellData
    {
        private unsafe EloBuddy.Native.SpellData* self;

        public SpellData(string name)
        {
        }

        public SpellData(uint hash)
        {
        }

        public unsafe SpellData(EloBuddy.Native.SpellData* spelldata)
        {
            this.self = spelldata;
        }

        internal unsafe EloBuddy.Native.SpellData* GetPtr() => 
            this.self;

        public static unsafe EloBuddy.SpellData GetSpellData(string name)
        {
            IntPtr hglobal = Marshal.StringToHGlobalAnsi(name);
            EloBuddy.Native.SpellData* spelldata = EloBuddy.Native.SpellData.FindSpell((sbyte modopt(IsSignUnspecifiedByte)*) hglobal.ToPointer());
            if (spelldata == null)
            {
            }
            Marshal.FreeHGlobal(hglobal);
            return new EloBuddy.SpellData(spelldata);
        }

        public int AffectsStatusFlags
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[12];
                }
                return 0;
            }
        }

        public int AffectsTypeFlags
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[8];
                }
                return 0;
            }
        }

        public string AfterEffectName
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x504] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x504]);
                }
                return "Unknown";
            }
        }

        public int AIData
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x5ac];
                }
                return 0;
            }
        }

        public string AlternateName
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[100] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[100]);
                }
                return "Unknown";
            }
        }

        public bool AlwaysSnapFacing
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[920];
                }
                return false;
            }
        }

        public float AmmoCountHiddenInUI
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[890];
                }
                return 0f;
            }
        }

        public float AmmoNotAffectedByCDR
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x379];
                }
                return 0f;
            }
        }

        public float AmmoRechargeTime
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[860];
                }
                return 0f;
            }
        }

        public float[] AmmoRechargeTimeArray
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    float* numPtr = EloBuddy.Native.SpellData.GetSDataArray(self) + 0x358;
                    float[] numArray = new float[6];
                    int index = 0;
                    do
                    {
                        numArray[index] = (index * 4)[(int) numPtr];
                        index++;
                    }
                    while (index < 6);
                    return numArray;
                }
                return null;
            }
        }

        public int AmmoUsed
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x340];
                }
                return 0;
            }
        }

        public int[] AmmoUsedArray
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    int* numPtr = EloBuddy.Native.SpellData.GetSDataArray(self) + 0x57c;
                    int[] numArray = new int[6];
                    int index = 0;
                    do
                    {
                        numArray[index] = (index * 4)[(int) numPtr];
                        index++;
                    }
                    while (index < 6);
                    return numArray;
                }
                return null;
            }
        }

        public string AnimationLeadOutName
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x200] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x200]);
                }
                return "Unknown";
            }
        }

        public string AnimationLoopName
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x1e8] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x1e8]);
                }
                return "Unknown";
            }
        }

        public string AnimationName
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x1dc] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x1dc]);
                }
                return "Unknown";
            }
        }

        public string AnimationWinddownName
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[500] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[500]);
                }
                return "Unknown";
            }
        }

        public bool ApplyAttackDamage
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x383];
                }
                return false;
            }
        }

        public bool ApplyAttackEffect
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[900];
                }
                return false;
            }
        }

        public bool ApplyMaterialOnHitSound
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x385];
                }
                return false;
            }
        }

        public bool BelongsToAvatar
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x387];
                }
                return false;
            }
        }

        public float BounceRadius
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x42c];
                }
                return 0f;
            }
        }

        public bool CanCastOrQueueWhileCasting
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x37e];
                }
                return false;
            }
        }

        public bool CanCastWhileDisabled
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x37d];
                }
                return false;
            }
        }

        public float CancelChargeOnRecastTime
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[800];
                }
                return 0f;
            }
        }

        public bool CanMoveWhileChanneling
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x395];
                }
                return false;
            }
        }

        public float CannotBeSuppressed
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x37c];
                }
                return 0f;
            }
        }

        public bool CanOnlyCastWhileDead
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x389];
                }
                return false;
            }
        }

        public bool CanOnlyCastWhileDisabled
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x37f];
                }
                return false;
            }
        }

        public bool CantCancelWhileChanneling
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x381];
                }
                return false;
            }
        }

        public bool CantCancelWhileWindingUp
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x380];
                }
                return false;
            }
        }

        public bool CantCastWhileRooted
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x382];
                }
                return false;
            }
        }

        public float CastConeAngle
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x420];
                }
                return 0f;
            }
        }

        public float CastConeDistance
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x424];
                }
                return 0f;
            }
        }

        public float CastFrame
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x474];
                }
                return 0f;
            }
        }

        public float CastRadius
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x3e8];
                }
                return 0f;
            }
        }

        public float[] CastRadiusArray
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    float* numPtr = EloBuddy.Native.SpellData.GetSDataArray(self) + 0x3e0;
                    float[] numArray = new float[6];
                    int index = 0;
                    do
                    {
                        numArray[index] = (index * 4)[(int) numPtr];
                        index++;
                    }
                    while (index < 6);
                    return numArray;
                }
                return null;
            }
        }

        public float CastRadiusSecondary
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x404];
                }
                return 0f;
            }
        }

        public float[] CastRadiusSecondaryArray
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    float* numPtr = EloBuddy.Native.SpellData.GetSDataArray(self) + 0x3fc;
                    float[] numArray = new float[6];
                    int index = 0;
                    do
                    {
                        numArray[index] = (index * 4)[(int) numPtr];
                        index++;
                    }
                    while (index < 6);
                    return numArray;
                }
                return null;
            }
        }

        public float CastRange
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x3b0];
                }
                return 0f;
            }
        }

        public float[] CastRangeArray
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    float* numPtr = EloBuddy.Native.SpellData.GetSDataArray(self) + 0x3b0;
                    float[] numArray = new float[6];
                    int index = 0;
                    do
                    {
                        numArray[index] = (index * 4)[(int) numPtr];
                        index++;
                    }
                    while (index < 6);
                    return numArray;
                }
                return null;
            }
        }

        public float CastRangeDisplayOverride
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x3cc];
                }
                return 0f;
            }
        }

        public float[] CastRangeDisplayOverrideArray
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    float* numPtr = EloBuddy.Native.SpellData.GetSDataArray(self) + 0x3c4;
                    float[] numArray = new float[6];
                    int index = 0;
                    do
                    {
                        numArray[index] = (index * 4)[(int) numPtr];
                        index++;
                    }
                    while (index < 6);
                    return numArray;
                }
                return null;
            }
        }

        public float CastRangeGrowthDuration
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x300];
                }
                return 0f;
            }
        }

        public float[] CastRangeGrowthDurationArray
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    float* numPtr = EloBuddy.Native.SpellData.GetSDataArray(self) + 0x2fc;
                    float[] numArray = new float[6];
                    int index = 0;
                    do
                    {
                        numArray[index] = (index * 4)[(int) numPtr];
                        index++;
                    }
                    while (index < 6);
                    return numArray;
                }
                return null;
            }
        }

        public float CastRangeGrowthMax
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x2c8];
                }
                return 0f;
            }
        }

        public float[] CastRangeGrowthMaxArray
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    float* numPtr = EloBuddy.Native.SpellData.GetSDataArray(self) + 0x2c4;
                    float[] numArray = new float[6];
                    int index = 0;
                    do
                    {
                        numArray[index] = (index * 4)[(int) numPtr];
                        index++;
                    }
                    while (index < 6);
                    return numArray;
                }
                return null;
            }
        }

        public float CastRangeGrowthStartTime
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[740];
                }
                return 0f;
            }
        }

        public bool CastRangeUseBoundingBoxes
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x391];
                }
                return false;
            }
        }

        public float CastTargetAdditionalUnitsRadius
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x428];
                }
                return 0f;
            }
        }

        public float CastTime
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x274];
                }
                return 0f;
            }
        }

        public int CastType
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x470];
                }
                return 0;
            }
        }

        public float ChannelDuration
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x278];
                }
                return 0f;
            }
        }

        public float[] ChannelDurationArray
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    float* numPtr = EloBuddy.Native.SpellData.GetSDataArray(self) + 0x27c;
                    float[] numArray = new float[6];
                    int index = 0;
                    do
                    {
                        numArray[index] = (index * 4)[(int) numPtr];
                        index++;
                    }
                    while (index < 6);
                    return numArray;
                }
                return null;
            }
        }

        public float ChargeUpdateInterval
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x31c];
                }
                return 0f;
            }
        }

        public float CircleMissileAngularVelocity
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x4bc];
                }
                return 0f;
            }
        }

        public float CircleMissileRadialVelocity
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x4b8];
                }
                return 0f;
            }
        }

        public int ClientData
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x5c4];
                }
                return 0;
            }
        }

        public float Coefficient
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x1d0];
                }
                return 0f;
            }
        }

        public float Coefficient2
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x1d4];
                }
                return 0f;
            }
        }

        public bool ConsideredAsAutoAttack
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x3ab];
                }
                return false;
            }
        }

        public float[] CooldownArray
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    float* numPtr = EloBuddy.Native.SpellData.GetSDataArray(self) + 0x29c;
                    float[] numArray = new float[6];
                    int index = 0;
                    do
                    {
                        numArray[index] = (index * 4)[(int) numPtr];
                        index++;
                    }
                    while (index < 6);
                    return numArray;
                }
                return null;
            }
        }

        public float CooldownTime
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x298];
                }
                return 0f;
            }
        }

        public bool CostAlwaysShownInUI
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x37b];
                }
                return false;
            }
        }

        public bool CursorChangesInGrass
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x38a];
                }
                return false;
            }
        }

        public bool CursorChangesInTerrain
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x38b];
                }
                return false;
            }
        }

        public float DeathRecapPriority
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x51c];
                }
                return 0f;
            }
        }

        public float DelayCastOffsetPercent
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x2b4];
                }
                return 0f;
            }
        }

        public float DelayTotalTimePercent
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x2b8];
                }
                return 0f;
            }
        }

        public string Description
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x7c] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x7c]);
                }
                return "Unknown";
            }
        }

        public string DescriptionTranslated =>
            RiotString.Translate(this.Description);

        public bool DisableCastBar
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x396];
                }
                return false;
            }
        }

        public string DisplayName
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x70] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x70]);
                }
                return "Unknown";
            }
        }

        public string DisplayNameTranslated =>
            RiotString.Translate(this.DisplayName);

        public bool DoesntBreakChannels
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x386];
                }
                return false;
            }
        }

        public bool DoNotNeedToFaceTarget
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[930];
                }
                return false;
            }
        }

        public string DynamicExtended
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0xac] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0xac]);
                }
                return "Unknown";
            }
        }

        public string DynamicExtendedTranslated =>
            RiotString.Translate(this.DynamicExtended);

        public string DynamicTooltip
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[160] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[160]);
                }
                return "Unknown";
            }
        }

        public string DynamicTooltipTranslated =>
            RiotString.Translate(this.DynamicTooltip);

        public int EffectAmount
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0xb8];
                }
                return 0;
            }
        }

        public int ExcludedUnitTags
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[40];
                }
                return 0;
            }
        }

        public int Flags
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[4];
                }
                return 0;
            }
        }

        public float FloatVarsDecimals
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x52c];
                }
                return 0f;
            }
        }

        public float[] FloatVarsDecimalsArray
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    float* numPtr = EloBuddy.Native.SpellData.GetSDataArray(self) + 0x5a4;
                    float[] numArray = new float[0x10];
                    int index = 0;
                    do
                    {
                        numArray[index] = (index * 4)[(int) numPtr];
                        index++;
                    }
                    while (index < 0x10);
                    return numArray;
                }
                return null;
            }
        }

        public bool HaveAfterEffect
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x39c];
                }
                return false;
            }
        }

        public bool HaveHitBone
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x39b];
                }
                return false;
            }
        }

        public bool HaveHitEffect
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x39a];
                }
                return false;
            }
        }

        public bool HavePointEffect
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x39d];
                }
                return false;
            }
        }

        public bool HideRangeIndicatorWhenCasting
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x3a8];
                }
                return false;
            }
        }

        public string HitBoneName
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x4e0] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x4e0]);
                }
                return "Unknown";
            }
        }

        public string HitEffectName
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x4ec] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x4ec]);
                }
                return "Unknown";
            }
        }

        public int HitEffectOrientType
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x4dc];
                }
                return 0;
            }
        }

        public string HitEffectPlayerName
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x4f8] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x4f8]);
                }
                return "Unknown";
            }
        }

        public bool IgnoreAnimContinueUntilCastFrame
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x3a7];
                }
                return false;
            }
        }

        public bool IgnoreRangeCheck
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x3a4];
                }
                return false;
            }
        }

        public string ImgIconName
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x20c] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x20c]);
                }
                return "Unknown";
            }
        }

        public bool IsDisabledWhileDead
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x388];
                }
                return false;
            }
        }

        public bool IsToggleSpell
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x39e];
                }
                return false;
            }
        }

        public string KeywordWhenAcquired
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x224] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x224]);
                }
                return "Unknown";
            }
        }

        public float LineDragLength
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x504];
                }
                return 0f;
            }
        }

        public bool LineMissileBounces
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x39f];
                }
                return false;
            }
        }

        public float LineMissileDelayDestroyAtEndSeconds
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x438];
                }
                return 0f;
            }
        }

        public bool LineMissileEndsAtTargetPoint
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x38c];
                }
                return false;
            }
        }

        public float LineMissileTargetHeightAugment
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x434];
                }
                return 0f;
            }
        }

        public float LineMissileTimePulseBetweenCollisionSpellHits
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x43c];
                }
                return 0f;
            }
        }

        public bool LineMissileTrackUnits
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[910];
                }
                return false;
            }
        }

        public bool LineMissileTrackUnitsAndContinues
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x38f];
                }
                return false;
            }
        }

        public bool LineMissileUsesAccelerationForBounce
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x3a0];
                }
                return false;
            }
        }

        public float LineWidth
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x360];
                }
                return 0f;
            }
        }

        public float[] LocationTargettingLengthArray
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    float* numPtr = EloBuddy.Native.SpellData.GetSDataArray(self) + 0x628;
                    float[] numArray = new float[6];
                    int index = 0;
                    do
                    {
                        numArray[index] = (index * 4)[(int) numPtr];
                        index++;
                    }
                    while (index < 6);
                    return numArray;
                }
                return null;
            }
        }

        public float[] LocationTargettingWidthArray
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    float* numPtr = EloBuddy.Native.SpellData.GetSDataArray(self) + 0x628;
                    float[] numArray = new float[6];
                    int index = 0;
                    do
                    {
                        numArray[index] = (index * 4)[(int) numPtr];
                        index++;
                    }
                    while (index < 6);
                    return numArray;
                }
                return null;
            }
        }

        public int LookAtPolicy
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x4d8];
                }
                return 0;
            }
        }

        public float LuaOnMissileUpdateDistanceInterval
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x440];
                }
                return 0f;
            }
        }

        public float Mana
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x56c];
                }
                return 0f;
            }
        }

        public float[] ManaCostArray
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    float* numPtr = EloBuddy.Native.SpellData.GetSDataArray(self) + 0x90;
                    float[] numArray = new float[6];
                    int index = 0;
                    do
                    {
                        numArray[index] = (index * 4)[(int) numPtr];
                        index++;
                    }
                    while (index < 6);
                    return numArray;
                }
                return null;
            }
        }

        public bool ManaUiOverride
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x584];
                }
                return false;
            }
        }

        public int MaxAmmo
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x324];
                }
                return 0;
            }
        }

        public int[] MaxAmmoArray
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    int* numPtr = EloBuddy.Native.SpellData.GetSDataArray(self) + 0x57c;
                    int[] numArray = new int[6];
                    int index = 0;
                    do
                    {
                        numArray[index] = (index * 4)[(int) numPtr];
                        index++;
                    }
                    while (index < 6);
                    return numArray;
                }
                return null;
            }
        }

        public int MaxHighlightTargets
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x1d8];
                }
                return 0;
            }
        }

        public bool MinimapIconDisplayFlag
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[940];
                }
                return false;
            }
        }

        public string MinimapIconName
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x218] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x218]);
                }
                return "Unknown";
            }
        }

        public bool MinimapIconRotation
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x392];
                }
                return false;
            }
        }

        public float MissileAccel
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x484];
                }
                return 0f;
            }
        }

        public bool MissileBlockTriggersOnDestroy
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x4af];
                }
                return false;
            }
        }

        public string MissileBoneName
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x490] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x490]);
                }
                return "Unknown";
            }
        }

        public bool MissileClientExitFOWPrediction
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x4ac];
                }
                return false;
            }
        }

        public string MissileEffectEnemyName
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x484] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x484]);
                }
                return "Unknown";
            }
        }

        public string MissileEffectName
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x46c] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x46c]);
                }
                return "Unknown";
            }
        }

        public string MissileEffectPlayerName
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x478] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x478]);
                }
                return "Unknown";
            }
        }

        public float MissileFixedTravelTime
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x494];
                }
                return 0f;
            }
        }

        public bool MissileFollowsTerrainHeight
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x3a1];
                }
                return false;
            }
        }

        public float MissileGravity
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x674];
                }
                return 0f;
            }
        }

        public float MissileLifetime
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x45c];
                }
                return 0f;
            }
        }

        public float MissileMaxSpeed
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x45c];
                }
                return 0f;
            }
        }

        public float MissileMinSpeed
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x484];
                }
                return 0f;
            }
        }

        public float MissilePerceptionBubbleRadius
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x4b0];
                }
                return 0f;
            }
        }

        public bool MissilePerceptionBubbleRevealsStealth
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x4b4];
                }
                return false;
            }
        }

        public float MissileSpeed
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x484];
                }
                return 0f;
            }
        }

        public float MissileTargetHeightAugment
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x430];
                }
                return 0f;
            }
        }

        public bool MissileUnblockable
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x4ae];
                }
                return false;
            }
        }

        public string Name
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetName(self) != null))
                {
                    basic_string<char,std::char_traits<char>,std::allocator<char> > modopt(IsConst)* modopt(IsConst) modopt(IsConst) localPtr = EloBuddy.Native.SpellData.GetName(this.self);
                    return new string((0x10 > localPtr[20]) ? ((sbyte*) localPtr) : ((sbyte*) localPtr[0]));
                }
                return "Unknown";
            }
        }

        public bool NoWinddownIfCancelled
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x3a3];
                }
                return false;
            }
        }

        public bool OrientRadiusTextureFromPlayer
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x3a5];
                }
                return false;
            }
        }

        public float PhysicalDamageRatio
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x450];
                }
                return 0f;
            }
        }

        public int PlatformSpellInfo
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x4c];
                }
                return 0;
            }
        }

        public string PointEffectName
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x510] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x510]);
                }
                return "Unknown";
            }
        }

        public int RequiredUnitTags
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x10];
                }
                return 0;
            }
        }

        public int SelectionPriority
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x59c];
                }
                return 0;
            }
        }

        public bool ShowChannelBar
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x397];
                }
                return false;
            }
        }

        public float SpellCastTime
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x41c];
                }
                return 0f;
            }
        }

        public float SpellDamageRatio
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x44c];
                }
                return 0f;
            }
        }

        public bool SpellRevealsChampion
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x38d];
                }
                return false;
            }
        }

        public string SpellTags
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x94] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x94]);
                }
                return "Unknown";
            }
        }

        public float SpellTotalTime
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x41c];
                }
                return 0f;
            }
        }

        public float StartCooldown
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x2c4];
                }
                return 0f;
            }
        }

        public string SummonerSpellUpgradeDescription
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[560] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[560]);
                }
                return "Unknown";
            }
        }

        public string TargetBoneName
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x49c] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x49c]);
                }
                return "Unknown";
            }
        }

        public SpellDataTargetType TargettingType
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self == null)
                {
                    return SpellDataTargetType.Self;
                }
                return EloBuddy.Native.SpellData.GetSDataArray(self)[0x698];
            }
        }

        public bool UpdateRotationWhenCasting
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x3a9];
                }
                return false;
            }
        }

        public bool UseAnimatorFramerate
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x399];
                }
                return false;
            }
        }

        public float UseAutoattackCastTime
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x3a6];
                }
                return 0f;
            }
        }

        public bool UseChargeChanneling
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x393];
                }
                return false;
            }
        }

        public bool UseChargeTargeting
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x394];
                }
                return false;
            }
        }

        public bool UseMinimapTargeting
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if (self != null)
                {
                    return EloBuddy.Native.SpellData.GetSDataArray(self)[0x390];
                }
                return false;
            }
        }

        public string VOEventCategory
        {
            get
            {
                EloBuddy.Native.SpellData* self = this.self;
                if ((self != null) && (EloBuddy.Native.SpellData.GetSDataArray(self)[0x5a0] != null))
                {
                    return new string(EloBuddy.Native.SpellData.GetSDataArray(this.self)[0x5a0]);
                }
                return "Unknown";
            }
        }
    }
}

