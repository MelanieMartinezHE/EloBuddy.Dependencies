namespace EloBuddy
{
    using EloBuddy.Native;
    using SharpDX;
    using System;
    using System.Runtime.InteropServices;

    public class CharData
    {
        private unsafe CharDataInfo* m_charInfo;

        public unsafe CharData(CharDataInfo* charInfo)
        {
            this.m_charInfo = charInfo;
        }

        public float AcquisitionRange =>
            *(EloBuddy.Native.CharDataInfo.GetAcquisitionRange(this.m_charInfo));

        public float Armor =>
            *(EloBuddy.Native.CharDataInfo.GetArmor(this.m_charInfo));

        public float AttackAutoInterruptPercent =>
            *(EloBuddy.Native.CharDataInfo.GetAttackAutoInterruptPercent(this.m_charInfo));

        public float AttackDelayCastOffsetPercent =>
            *(EloBuddy.Native.CharDataInfo.GetAttackDelayCastOffsetPercent(this.m_charInfo));

        public float AttackDelayCastOffsetPercentAttackSpeedRatio =>
            *(EloBuddy.Native.CharDataInfo.GetAttackDelayCastOffsetPercentAttackSpeedRatio(this.m_charInfo));

        public float AttackDelayOffsetPercent =>
            *(EloBuddy.Native.CharDataInfo.GetAttackDelayOffsetPercent(this.m_charInfo));

        public float AttackRange =>
            *(EloBuddy.Native.CharDataInfo.GetAttackRange(this.m_charInfo));

        public float AttaGameplayCollisionRadiusckRange =>
            *(EloBuddy.Native.CharDataInfo.GetGameplayCollisionRadius(this.m_charInfo));

        public float BaseAbilityPower =>
            *(EloBuddy.Native.CharDataInfo.GetBaseAbilityPower(this.m_charInfo));

        public float BaseCritChance =>
            *(EloBuddy.Native.CharDataInfo.GetBaseCritChance(this.m_charInfo));

        public float BaseDodge =>
            *(EloBuddy.Native.CharDataInfo.GetBaseDodge(this.m_charInfo));

        public float BaseFactorHPRegen =>
            *(EloBuddy.Native.CharDataInfo.GetBaseFactorHPRegen(this.m_charInfo));

        public float BaseHP =>
            *(EloBuddy.Native.CharDataInfo.GetBaseHP(this.m_charInfo));

        public float BaseMissChance =>
            *(EloBuddy.Native.CharDataInfo.GetBaseMissChance(this.m_charInfo));

        public float BaseMP =>
            *(EloBuddy.Native.CharDataInfo.GetBaseMP(this.m_charInfo));

        public string BaseSkinName =>
            new string(*(EloBuddy.Native.CharDataInfo.GetBaseSkinName(this.m_charInfo)));

        public float BaseStaticHPRegen =>
            *(EloBuddy.Native.CharDataInfo.GetBaseStaticHPRegen(this.m_charInfo));

        public float BaseStaticMPRegen =>
            *(EloBuddy.Native.CharDataInfo.GetBaseStaticMPRegen(this.m_charInfo));

        public string BasicAttack1 =>
            new string(EloBuddy.Native.CharDataInfo.GetBasicAttack1(this.m_charInfo));

        public string BasicAttack2 =>
            new string(EloBuddy.Native.CharDataInfo.GetBasicAttack2(this.m_charInfo));

        public string BasicAttack3 =>
            new string(EloBuddy.Native.CharDataInfo.GetBasicAttack3(this.m_charInfo));

        public string BasicAttack4 =>
            new string(EloBuddy.Native.CharDataInfo.GetBasicAttack4(this.m_charInfo));

        public string BasicAttack5 =>
            new string(EloBuddy.Native.CharDataInfo.GetBasicAttack5(this.m_charInfo));

        public string BasicAttack6 =>
            new string(EloBuddy.Native.CharDataInfo.GetBasicAttack6(this.m_charInfo));

        public string BasicAttack7 =>
            new string(EloBuddy.Native.CharDataInfo.GetBasicAttack7(this.m_charInfo));

        public string BasicAttack8 =>
            new string(EloBuddy.Native.CharDataInfo.GetBasicAttack8(this.m_charInfo));

        public string BasicAttack9 =>
            new string(EloBuddy.Native.CharDataInfo.GetBasicAttack9(this.m_charInfo));

        public string CritAttack1 =>
            new string(EloBuddy.Native.CharDataInfo.GetCritAttack1(this.m_charInfo));

        public float CritDamageBonus =>
            *(EloBuddy.Native.CharDataInfo.GetCritDamageBonus(this.m_charInfo));

        public string CriticalAttack =>
            new string(*(EloBuddy.Native.CharDataInfo.GetCriticalAttack(this.m_charInfo)));

        public float DeathEventListeningRadius =>
            *(EloBuddy.Native.CharDataInfo.GetDeathEventListeningRadius(this.m_charInfo));

        public float ExperienceRadius =>
            *(EloBuddy.Native.CharDataInfo.GetExperienceRadius(this.m_charInfo));

        public float ExpGivenOnDeath =>
            *(EloBuddy.Native.CharDataInfo.GetExpGivenOnDeath(this.m_charInfo));

        public string ExtraSpell1 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetExtraSpell1(this.m_charInfo)));

        public string ExtraSpell10 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetExtraSpell10(this.m_charInfo)));

        public string ExtraSpell112 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetExtraSpell11(this.m_charInfo)));

        public string ExtraSpell12 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetExtraSpell12(this.m_charInfo)));

        public string ExtraSpell13 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetExtraSpell13(this.m_charInfo)));

        public string ExtraSpell14 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetExtraSpell14(this.m_charInfo)));

        public string ExtraSpell15 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetExtraSpell15(this.m_charInfo)));

        public string ExtraSpell16 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetExtraSpell16(this.m_charInfo)));

        public string ExtraSpell2 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetExtraSpell2(this.m_charInfo)));

        public string ExtraSpell3 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetExtraSpell3(this.m_charInfo)));

        public string ExtraSpell4 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetExtraSpell4(this.m_charInfo)));

        public string ExtraSpell5 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetExtraSpell5(this.m_charInfo)));

        public string ExtraSpell6 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetExtraSpell6(this.m_charInfo)));

        public string ExtraSpell7 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetExtraSpell7(this.m_charInfo)));

        public string ExtraSpell8 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetExtraSpell8(this.m_charInfo)));

        public string ExtraSpell9 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetExtraSpell9(this.m_charInfo)));

        public float FirstAcquisitionRange =>
            *(EloBuddy.Native.CharDataInfo.GetFirstAcquisitionRange(this.m_charInfo));

        public float GlobalExpGivenOnDeath =>
            *(EloBuddy.Native.CharDataInfo.GetGlobalExpGivenOnDeath(this.m_charInfo));

        public float GlobalGoldGivenOnDeath =>
            *(EloBuddy.Native.CharDataInfo.GetGlobalGoldGivenOnDeath(this.m_charInfo));

        public float GoldGivenOnDeath =>
            *(EloBuddy.Native.CharDataInfo.GetGoldGivenOnDeath(this.m_charInfo));

        public float GoldRadius =>
            *(EloBuddy.Native.CharDataInfo.GetGoldRadius(this.m_charInfo));

        public float HitFxScale =>
            *(EloBuddy.Native.CharDataInfo.GetHitFxScale(this.m_charInfo));

        public float HoverIndicatorRadius =>
            *(EloBuddy.Native.CharDataInfo.GetHoverIndicatorRadius(this.m_charInfo));

        public float HoverIndicatorWidth =>
            *(EloBuddy.Native.CharDataInfo.GetHoverIndicatorWidth(this.m_charInfo));

        public float HPPerLevel =>
            *(EloBuddy.Native.CharDataInfo.GetHPPerLevel(this.m_charInfo));

        public float HPPerTick =>
            *(EloBuddy.Native.CharDataInfo.GetHPPerTick(this.m_charInfo));

        public float LocalExpGivenOnDeath =>
            *(EloBuddy.Native.CharDataInfo.GetLocalExpGivenOnDeath(this.m_charInfo));

        public float LocalGoldGivenOnDeath =>
            *(EloBuddy.Native.CharDataInfo.GetLocalGoldGivenOnDeath(this.m_charInfo));

        public float LocalGoldSplitWithLastHitter =>
            *(EloBuddy.Native.CharDataInfo.GetLocalGoldSplitWithLastHitter(this.m_charInfo));

        public string Lore1 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetLore1(this.m_charInfo)));

        public int MonsterDataTableID =>
            *(EloBuddy.Native.CharDataInfo.GetMonsterDataTableID(this.m_charInfo));

        public float MoveSpeed =>
            *(EloBuddy.Native.CharDataInfo.GetMoveSpeed(this.m_charInfo));

        public float MPPerLevel =>
            *(EloBuddy.Native.CharDataInfo.GetMPPerLevel(this.m_charInfo));

        public string Passive1Desc =>
            new string(*(EloBuddy.Native.CharDataInfo.GetPassive1Desc(this.m_charInfo)));

        public string Passive1Desc1 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetPassive1Desc1(this.m_charInfo)));

        public string Passive1LuaName =>
            new string(*(EloBuddy.Native.CharDataInfo.GetPassive1LuaName(this.m_charInfo)));

        public string Passive1Name =>
            new string(*(EloBuddy.Native.CharDataInfo.GetPassive1Name(this.m_charInfo)));

        public float Passive1Range =>
            *(EloBuddy.Native.CharDataInfo.GetPassive1Range(this.m_charInfo));

        public string PassiveSpell =>
            new string(*(EloBuddy.Native.CharDataInfo.GetPassiveSpell(this.m_charInfo)));

        public float PathfindingCollisionRadius =>
            *(EloBuddy.Native.CharDataInfo.GetPathfindingCollisionRadius(this.m_charInfo));

        public float PerceptionBubbleRadius =>
            *(EloBuddy.Native.CharDataInfo.GetPerceptionBubbleRadius(this.m_charInfo));

        public float SelectionHeight =>
            *(EloBuddy.Native.CharDataInfo.GetSelectionHeight(this.m_charInfo));

        public float SelectionRadius =>
            *(EloBuddy.Native.CharDataInfo.GetSelectionRadius(this.m_charInfo));

        public bool ShowWhileUntargetable =>
            *(EloBuddy.Native.CharDataInfo.GetShowWhileUntargetable(this.m_charInfo));

        public float Significance =>
            *(EloBuddy.Native.CharDataInfo.GetSignificance(this.m_charInfo));

        public string Spell1 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetSpell1(this.m_charInfo)));

        public string Spell2 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetSpell2(this.m_charInfo)));

        public string Spell3 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetSpell3(this.m_charInfo)));

        public string Spell4 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetSpell4(this.m_charInfo)));

        public float SpellBlock =>
            *(EloBuddy.Native.CharDataInfo.GetSpellBlock(this.m_charInfo));

        public string Tips1 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetTips1(this.m_charInfo)));

        public string Tips2 =>
            new string(*(EloBuddy.Native.CharDataInfo.GetTips2(this.m_charInfo)));

        public float TowerTargetingPriorityBoost =>
            *(EloBuddy.Native.CharDataInfo.GetTowerTargetingPriorityBoost(this.m_charInfo));

        public int UnitTags =>
            *(EloBuddy.Native.CharDataInfo.GetUnitTags(this.m_charInfo));

        public float UntargetableSpawnTime =>
            *(EloBuddy.Native.CharDataInfo.GetUntargetableSpawnTime(this.m_charInfo));

        public Vector3 WorldOffset =>
            Vector3.Zero;

        public float XOffset =>
            *(EloBuddy.Native.CharDataInfo.GetXOffset(this.m_charInfo));

        public float YOffset =>
            *(EloBuddy.Native.CharDataInfo.GetYOffset(this.m_charInfo));
    }
}

