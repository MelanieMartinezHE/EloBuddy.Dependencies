namespace EloBuddy.SDK.Spells
{
    using EloBuddy;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract]
    public class SpellInfo
    {
        [DataMember]
        public bool Acceleration;
        [DataMember]
        public float CastRangeGrowthDuration;
        [DataMember]
        public int CastRangeGrowthMax;
        [DataMember]
        public int CastRangeGrowthMin;
        [DataMember]
        public float CastRangeGrowthStartTime;
        [DataMember]
        public bool Chargeable;
        [DataMember, JsonProperty(ItemConverterType=typeof(StringEnumConverter))]
        public CollisionType[] Collisions = new CollisionType[0];
        [DataMember]
        public float Delay;
        [DataMember]
        public float MissileAccel;
        [DataMember]
        public float MissileFixedTravelTime;
        [DataMember]
        public float MissileMaxSpeed;
        [DataMember]
        public float MissileMinSpeed;
        [DataMember]
        public string MissileName;
        [DataMember, JsonConverter(typeof(StringEnumConverter)), DefaultValue(-1)]
        public SpellSlot MissileSlot = SpellSlot.Unknown;
        [DataMember]
        public float MissileSpeed = float.MaxValue;
        [DataMember]
        public string[] OtherMissileNames = new string[0];
        [DataMember]
        public string[] OtherSpellNames = new string[0];
        [DataMember]
        public float Radius;
        [DataMember(IsRequired=true)]
        public float Range;
        [DataMember(IsRequired=true), JsonConverter(typeof(StringEnumConverter)), DefaultValue(-1)]
        public SpellSlot RealSlot;
        [DataMember(IsRequired=true), JsonConverter(typeof(StringEnumConverter)), DefaultValue(-1)]
        public SpellSlot Slot;
        [DataMember]
        public string SpellName;
        [DataMember(IsRequired=true), JsonConverter(typeof(StringEnumConverter)), DefaultValue(-1)]
        public SpellType Type;

        public bool IsCorrect(GameObjectProcessSpellCastEventArgs args)
        {
            if (this.RealSlot == args.Slot)
            {
                if (!string.IsNullOrEmpty(this.SpellName))
                {
                    return (string.Equals(this.SpellName, args.SData.Name, StringComparison.CurrentCultureIgnoreCase) || this.OtherSpellNames.Contains<string>(args.SData.Name, StringComparer.CurrentCultureIgnoreCase));
                }
                return true;
            }
            return false;
        }

        public bool IsCorrect(MissileClient missile)
        {
            if (!string.IsNullOrEmpty(this.MissileName))
            {
                return (string.Equals(this.MissileName, missile.SData.Name, StringComparison.CurrentCultureIgnoreCase) || this.OtherMissileNames.Contains<string>(missile.SData.Name, StringComparer.CurrentCultureIgnoreCase));
            }
            return ((this.MissileSlot != SpellSlot.Unknown) && (missile.Slot == this.MissileSlot));
        }
    }
}

