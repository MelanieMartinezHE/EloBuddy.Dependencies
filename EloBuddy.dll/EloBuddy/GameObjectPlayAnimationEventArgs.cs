namespace EloBuddy
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class GameObjectPlayAnimationEventArgs : EventArgs
    {
        private unsafe sbyte modopt(IsSignUnspecifiedByte)** m_animation;
        private int m_animationHash;
        internal static Dictionary<int, string> m_animationHashDictionary = new Dictionary<int, string>();
        private bool m_process;

        static GameObjectPlayAnimationEventArgs()
        {
            m_animationHashDictionary.Add(0x2acd4eca, "Run");
            m_animationHashDictionary.Add(-1023291501, "Idle");
            m_animationHashDictionary.Add(-1038207930, "Joke");
            m_animationHashDictionary.Add(-1231696706, "Laugh");
            m_animationHashDictionary.Add(-1130593181, "Taunt");
            m_animationHashDictionary.Add(-132831076, "Dance");
            m_animationHashDictionary.Add(0x56b1e924, "Attack1");
            m_animationHashDictionary.Add(0x59b1eddd, "Attack2");
            m_animationHashDictionary.Add(0x5a81bdb0, "Recall");
            m_animationHashDictionary.Add(0x3a224d98, "Spawn");
            m_animationHashDictionary.Add(-1292486552, "Spell1");
            m_animationHashDictionary.Add(-1030080981, "Spell1a");
            m_animationHashDictionary.Add(-1013303362, "Spell1b");
            m_animationHashDictionary.Add(-996525743, "Spell1c");
            m_animationHashDictionary.Add(-1242153695, "Spell2");
            m_animationHashDictionary.Add(-1297978432, "Spell2a");
            m_animationHashDictionary.Add(-1247645575, "Spell2b");
            m_animationHashDictionary.Add(-1264423194, "Spell2c");
            m_animationHashDictionary.Add(-1258931314, "Spell3");
            m_animationHashDictionary.Add(-828352195, "Spell3a");
            m_animationHashDictionary.Add(-878685052, "Spell3b");
            m_animationHashDictionary.Add(-861907433, "Spell3c");
            m_animationHashDictionary.Add(-1208598457, "Spell4");
            m_animationHashDictionary.Add(-1096352814, "Spell4a");
            m_animationHashDictionary.Add(-1113130433, "Spell4b");
            m_animationHashDictionary.Add(-1129908052, "Spell4c");
            m_animationHashDictionary.Add(-1121403571, "Death");
            m_animationHashDictionary.Add(-1118238244, "AzirSoldierSpawn");
            m_animationHashDictionary.Add(-1900803113, "AzirSoldierIdle");
            m_animationHashDictionary.Add(0x45f47b73, "AzirSoldierActive");
            m_animationHashDictionary.Add(0x777270d5, "AzirSoldierAutoAttack1");
            m_animationHashDictionary.Add(0x74726c1c, "AzirSoldierAutoAttack2");
            m_animationHashDictionary.Add(-1772878451, "Crit");
            m_animationHashDictionary.Add(-1716546266, "TurretFirstBreak");
            m_animationHashDictionary.Add(0x233951b2, "TurretSecondBreak");
            m_animationHashDictionary.Add(-1175109554, "TurretExplosion");
            m_animationHashDictionary.Add(0x34edfa7b, "Spell4_Windup");
            m_animationHashDictionary.Add(-1450333034, "Spell4_Loop");
            m_animationHashDictionary.Add(0x4b07ae2e, "Spell4_Winddown");
            m_animationHashDictionary.Add(-1646666746, "Idle1");
            m_animationHashDictionary.Add(-285102248, "Inactive");
            m_animationHashDictionary.Add(0x1f4d30b1, "Reactivate");
            m_animationHashDictionary.Add(-1821709809, "Run_Exit");
            m_animationHashDictionary.Add(-1225376076, "Spell5");
            m_animationHashDictionary.Add(-1784503985, "Spell3withReload");
        }

        public GameObjectPlayAnimationEventArgs(int animationHash)
        {
            this.m_animationHash = animationHash;
            this.m_process = true;
        }

        public string Animation
        {
            get
            {
                if (m_animationHashDictionary.ContainsKey(this.AnimationHash))
                {
                    return m_animationHashDictionary[this.AnimationHash];
                }
                return "Unknown";
            }
            set
            {
            }
        }

        public int AnimationHash =>
            this.m_animationHash;

        public bool Process
        {
            [return: MarshalAs(UnmanagedType.U1)]
            get => 
                this.m_process;
            [param: MarshalAs(UnmanagedType.U1)]
            set
            {
                this.m_process = value;
            }
        }

        public delegate void GameObjectPlayAnimationEvent(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args);
    }
}

