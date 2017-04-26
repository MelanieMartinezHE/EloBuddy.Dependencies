namespace EloBuddy
{
    using EloBuddy.Native;
    using System;

    public class Experience
    {
        internal unsafe EloBuddy.Native.AIHeroClient* self;

        public unsafe Experience(EloBuddy.Native.AIHeroClient* hero)
        {
            this.self = hero;
        }

        public int Level
        {
            get
            {
                EloBuddy.Native.AIHeroClient* self = this.self;
                if (self != null)
                {
                    EloBuddy.Native.Experience* experiencePtr = EloBuddy.Native.AIHeroClient.GetExperience(self);
                    if (experiencePtr != null)
                    {
                        return *(EloBuddy.Native.Experience.GetLevel(experiencePtr));
                    }
                }
                return 0;
            }
        }

        public int SpellTrainingPoints
        {
            get
            {
                EloBuddy.Native.AIHeroClient* self = this.self;
                if (self != null)
                {
                    EloBuddy.Native.Experience* experiencePtr = EloBuddy.Native.AIHeroClient.GetExperience(self);
                    if (experiencePtr != null)
                    {
                        return *(EloBuddy.Native.Experience.GetSpellTrainingPoints(experiencePtr));
                    }
                }
                return 0;
            }
        }

        public float XP
        {
            get
            {
                EloBuddy.Native.AIHeroClient* self = this.self;
                if (self != null)
                {
                    EloBuddy.Native.Experience* experiencePtr = EloBuddy.Native.AIHeroClient.GetExperience(self);
                    if (experiencePtr != null)
                    {
                        return *(EloBuddy.Native.Experience.GetExperience(experiencePtr));
                    }
                }
                return 0f;
            }
        }

        public float XPNextLevelVisual
        {
            get
            {
                double xPToNextLevel = this.XPToNextLevel;
                return (((float) xPToNextLevel) - this.XPToCurrentLevel);
            }
        }

        public float XPPercentage
        {
            get
            {
                if (this.Level == 0x12)
                {
                    return 100f;
                }
                double xP = this.XP;
                double num2 = (xP - this.XPToCurrentLevel) * 100.0;
                double xPToNextLevel = this.XPToNextLevel;
                return (float) (num2 / (xPToNextLevel - this.XPToCurrentLevel));
            }
        }

        public float XPToCurrentLevel
        {
            get
            {
                EloBuddy.Native.AIHeroClient* self = this.self;
                if (self != null)
                {
                    EloBuddy.Native.Experience* experiencePtr = EloBuddy.Native.AIHeroClient.GetExperience(self);
                    if (experiencePtr != null)
                    {
                        return EloBuddy.Native.Experience.GetExpToCurrentLevel(experiencePtr);
                    }
                }
                return 0f;
            }
        }

        public float XPToNextLevel
        {
            get
            {
                EloBuddy.Native.AIHeroClient* self = this.self;
                if (self != null)
                {
                    EloBuddy.Native.Experience* experiencePtr = EloBuddy.Native.AIHeroClient.GetExperience(self);
                    if (experiencePtr != null)
                    {
                        return EloBuddy.Native.Experience.GetExpToNextLevel(experiencePtr);
                    }
                }
                return 0f;
            }
        }
    }
}

