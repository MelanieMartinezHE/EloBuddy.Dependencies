namespace EloBuddy
{
    using System;
    using System.Runtime.CompilerServices;

    public class OnHeroApplyCoolDownEventArgs : EventArgs
    {
        private SpellDataInst m_sdata;
        private AIHeroClient m_sender;
        private SpellSlot m_slot;

        public OnHeroApplyCoolDownEventArgs(AIHeroClient sender, SpellDataInst sdata, SpellSlot slot)
        {
            this.m_sender = sender;
            this.m_sdata = sdata;
            this.m_slot = slot;
        }

        public float End
        {
            get
            {
                SpellDataInst modopt(IsConst) sdata = this.m_sdata;
                if (sdata.GetSpellDataInst() != null)
                {
                    return *(EloBuddy.Native.SpellDataInst.GetCooldownExpires(sdata.GetSpellDataInst()));
                }
                return 0f;
            }
        }

        public SpellDataInst SData =>
            this.m_sdata;

        public AIHeroClient Sender =>
            this.m_sender;

        public SpellSlot Slot =>
            this.m_slot;

        public float Start
        {
            get
            {
                SpellDataInst modopt(IsConst) sdata = this.m_sdata;
                if (sdata.GetSpellDataInst() != null)
                {
                    return *(EloBuddy.Native.SpellDataInst.GetCooldown(sdata.GetSpellDataInst()));
                }
                return 0f;
            }
        }

        public delegate void OnHeroApplyCoolDownEvent(OnHeroApplyCoolDownEventArgs args);
    }
}

