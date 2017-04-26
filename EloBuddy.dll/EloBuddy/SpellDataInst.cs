namespace EloBuddy
{
    using EloBuddy.Native;
    using std;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class SpellDataInst
    {
        private EloBuddy.SpellSlot m_slot;
        private unsafe EloBuddy.Native.Spellbook* m_spellbook;
        private unsafe EloBuddy.Native.SpellDataInst* m_spellDataInst;

        public unsafe SpellDataInst(EloBuddy.Native.SpellDataInst* spellDataInst, EloBuddy.SpellSlot slot, EloBuddy.Native.Spellbook* spellbook)
        {
            this.m_spellDataInst = spellDataInst;
            this.m_slot = slot;
            this.m_spellbook = spellbook;
        }

        public unsafe EloBuddy.Native.SpellDataInst* GetSpellDataInst() => 
            this.m_spellDataInst;

        public int Ammo
        {
            get
            {
                EloBuddy.Native.SpellDataInst* spellDataInst = this.m_spellDataInst;
                if (spellDataInst != null)
                {
                    return *(EloBuddy.Native.SpellDataInst.GetAmmo(spellDataInst));
                }
                return 0;
            }
        }

        public float AmmoRechargeStart
        {
            get
            {
                EloBuddy.Native.SpellDataInst* spellDataInst = this.m_spellDataInst;
                if (spellDataInst != null)
                {
                    return (float) *(EloBuddy.Native.SpellDataInst.GetAmmoRechargeStart(spellDataInst));
                }
                return 0f;
            }
        }

        public float Cooldown
        {
            get
            {
                EloBuddy.Native.SpellDataInst* spellDataInst = this.m_spellDataInst;
                if (spellDataInst != null)
                {
                    return *(EloBuddy.Native.SpellDataInst.GetCooldown(spellDataInst));
                }
                return 0f;
            }
        }

        public float CooldownExpires
        {
            get
            {
                EloBuddy.Native.SpellDataInst* spellDataInst = this.m_spellDataInst;
                if (spellDataInst != null)
                {
                    return *(EloBuddy.Native.SpellDataInst.GetCooldownExpires(spellDataInst));
                }
                return 0f;
            }
        }

        public bool IsLearned =>
            ((bool) ((byte) (this.Level > 0)));

        public bool IsOnCooldown =>
            ((bool) ((byte) (this.State == EloBuddy.SpellState.Cooldown)));

        public bool IsReady =>
            ((bool) ((byte) (this.State == EloBuddy.SpellState.Ready)));

        public bool IsUpgradable
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get
            {
                EloBuddy.Native.Spellbook* spellbook = this.m_spellbook;
                return ((spellbook != null) && EloBuddy.Native.Spellbook.SpellSlotCanBeUpgraded((EloBuddy.Native.Spellbook modopt(IsConst)* modopt(IsConst) modopt(IsConst)) spellbook, (EloBuddy.Native.SpellSlot) this.m_slot));
            }
        }

        public int Level
        {
            get
            {
                EloBuddy.Native.SpellDataInst* spellDataInst = this.m_spellDataInst;
                if (spellDataInst != null)
                {
                    return *(EloBuddy.Native.SpellDataInst.GetLevel(spellDataInst));
                }
                return 0;
            }
        }

        public string Name
        {
            get
            {
                EloBuddy.Native.SpellDataInst* spellDataInst = this.m_spellDataInst;
                if ((spellDataInst != null) && (EloBuddy.Native.SpellDataInst.GetSData(spellDataInst) != null))
                {
                    basic_string<char,std::char_traits<char>,std::allocator<char> > modopt(IsConst)* modopt(IsConst) modopt(IsConst) localPtr = EloBuddy.Native.SpellDataInst.GetName(this.m_spellDataInst);
                    return new string((0x10 > localPtr[20]) ? ((sbyte*) localPtr) : ((sbyte*) localPtr[0]));
                }
                return "Unknown";
            }
        }

        public EloBuddy.SpellData SData =>
            new EloBuddy.SpellData(EloBuddy.Native.SpellDataInst.GetSData(this.m_spellDataInst));

        public EloBuddy.SpellSlot Slot =>
            this.m_slot;

        public EloBuddy.SpellState State
        {
            get
            {
                EloBuddy.Native.Spellbook* spellbook = this.m_spellbook;
                if (spellbook != null)
                {
                    EloBuddy.SpellSlot slot = this.m_slot;
                    if (*(((int*) &slot)) < 60)
                    {
                        EloBuddy.SpellState state = EloBuddy.Native.Spellbook.CanUseSpell(spellbook, *((EloBuddy.Native.SpellSlot*) &slot));
                        if (!Enum.IsDefined(typeof(EloBuddy.SpellState), state))
                        {
                            return (state & ((EloBuddy.SpellState) 0x1a6));
                        }
                        return state;
                    }
                }
                return EloBuddy.SpellState.Unknown;
            }
        }

        public int ToggleState
        {
            get
            {
                EloBuddy.Native.SpellDataInst* spellDataInst = this.m_spellDataInst;
                if (spellDataInst != null)
                {
                    return *(EloBuddy.Native.SpellDataInst.GetToggleState(spellDataInst));
                }
                return 0;
            }
        }
    }
}

