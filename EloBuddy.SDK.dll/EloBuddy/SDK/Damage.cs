namespace EloBuddy.SDK
{
    using EloBuddy;
    using EloBuddy.SDK.Constants;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class Damage
    {
        private const bool Broken = true;

        public static float CalculateDamageOnUnit(this Obj_AI_Base from, Obj_AI_Base target, DamageType damageType, float rawDamage, bool isAbility = true, bool isAutoAttackOrTargetted = false)
        {
            if (target == null)
            {
                return 0f;
            }
            float num = 1f;
            float armor = 0f;
            float num3 = 0f;
            float num4 = 0f;
            float num5 = 0f;
            float num6 = 0f;
            float num7 = 0f;
            float num8 = 0f;
            switch (damageType)
            {
                case DamageType.Physical:
                    armor = target.CharData.Armor;
                    num3 = target.Armor - target.CharData.Armor;
                    if (from.Type == GameObjectType.obj_AI_Minion)
                    {
                        num6 = 0f;
                        num7 = 0f;
                        num8 = 0f;
                    }
                    else if (from.Type == GameObjectType.obj_AI_Turret)
                    {
                        num6 = 0f;
                        num7 = from.IsLaneTurret() ? 0.75f : 0.25f;
                        num8 = 0f;
                    }
                    break;

                case DamageType.Magical:
                    armor = target.CharData.SpellBlock;
                    num3 = target.SpellBlock - target.CharData.SpellBlock;
                    break;

                case DamageType.True:
                    return rawDamage;
            }
            float num9 = armor + num3;
            if (num9 > 0f)
            {
                float num16 = armor / num9;
                float num17 = 1f - num16;
                armor -= num4 * num16;
                num3 -= num4 * num17;
                num9 = armor + num3;
                if (num9 > 0f)
                {
                    armor *= 1f - num5;
                    num3 *= 1f - num5;
                    armor *= 1f - num7;
                    num3 *= 1f - num7;
                    num3 *= 1f - num8;
                    num9 = armor + num3;
                    num9 -= num6;
                }
            }
            if (num9 >= 0f)
            {
                num *= 100f / (100f + num9);
            }
            else
            {
                num *= 2f - (100f / (100f - num9));
            }
            AIHeroClient fromHero = from as AIHeroClient;
            Obj_AI_Minion minion = from as Obj_AI_Minion;
            AIHeroClient hero = target as AIHeroClient;
            Obj_AI_Minion minion2 = target as Obj_AI_Minion;
            float num10 = 1f;
            if (from.Type == GameObjectType.obj_AI_Turret)
            {
                if (target.Type == GameObjectType.obj_AI_Minion)
                {
                    num10 *= 1.25f;
                    if (target.IsSiegeMinion())
                    {
                        num10 *= 0.7f;
                    }
                }
                else if (target.Type == GameObjectType.AIHeroClient)
                {
                    num10 *= 1.375f;
                }
            }
            if (((minion != null) && (minion2 != null)) && (Game.MapId == GameMapId.SummonersRift))
            {
                num10 *= 1f + minion.PercentDamageToBarracksMinionMod;
            }
            if (fromHero > null)
            {
                if (fromHero.HasDoubleEdgedSword())
                {
                    num10 *= 1.03f;
                }
                if (fromHero.HasAssassin() && !EntityManager.Heroes.AllHeroes.Any<AIHeroClient>(h => (((h.IsValidTarget(null, false, null) && (fromHero.Team == h.Team)) && !h.Equals(fromHero)) && ((Obj_AI_Base) h).IsInRange(((Obj_AI_Base) fromHero), 800f))))
                {
                    num10 *= 1.02f;
                }
                if ((target.HealthPercent <= 40f) && (target.Type == GameObjectType.AIHeroClient))
                {
                    num10 *= 1f + (0.06f * fromHero.GetMercilessCount());
                }
                if (hero > null)
                {
                    if (fromHero.HasBuff("summonerexhaust"))
                    {
                        num10 *= 0.6f;
                    }
                    if (target.HasBuff("sonapassivedebuff"))
                    {
                        AIHeroClient caster = target.GetBuff("sonapassivedebuff").Caster as AIHeroClient;
                        num10 *= 1f - (0.2f + ((caster != null) ? (0.02f * ((int) (caster.FlatMagicDamageMod / 100f))) : 0f));
                    }
                    if (target.HasBuff("urgotcorrosivedebuff"))
                    {
                        num10 *= 0.85f;
                    }
                }
                if ((fromHero.Hero == Champion.Jayce) && (isAutoAttackOrTargetted && fromHero.HasBuff("jaycehypercharge")))
                {
                    num10 *= 0.62f + (0.08f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level);
                }
                if ((hero > null) && ((damageType == DamageType.Physical) && (fromHero.MaxHealth < hero.MaxHealth)))
                {
                    int[] itemIds = new int[] { 0xbda };
                    if (fromHero.HasItem(itemIds))
                    {
                        num10 *= 1f + ((Math.Min((float) (hero.MaxHealth - fromHero.MaxHealth), (float) 500f) / 50f) * 0.01f);
                    }
                    int[] numArray2 = new int[] { 0xbdc };
                    if (fromHero.HasItem(numArray2))
                    {
                        num10 *= 1f + ((Math.Min((float) (hero.MaxHealth - fromHero.MaxHealth), (float) 500f) / 50f) * 0.015f);
                    }
                }
            }
            if ((hero > null) && hero.HasDoubleEdgedSword())
            {
                num10 *= 1.025f;
            }
            float num11 = 0f;
            if (fromHero > null)
            {
                if (isAutoAttackOrTargetted)
                {
                    if (fromHero.HasBuff("MasteryOnHitDamageStacker"))
                    {
                    }
                    ItemId[] idArray1 = new ItemId[] { ItemId.Muramana };
                    if ((fromHero.HasItem(idArray1) && fromHero.HasBuff("Muramana")) && (fromHero.ManaPercent >= 20f))
                    {
                        num11 += 0.06f * fromHero.Mana;
                    }
                }
                if (isAutoAttackOrTargetted | isAbility)
                {
                }
            }
            float num12 = 1f;
            if (hero > null)
            {
                if (hero.HasBuff("vladimirhemoplaguedebuff"))
                {
                    num12 *= 1.1f;
                }
                Champion champion2 = hero.Hero;
                if (champion2 <= Champion.Katarina)
                {
                    switch (champion2)
                    {
                        case Champion.Alistar:
                            if (hero.HasBuff("FerociousHowl"))
                            {
                                num12 *= 0.4f + (0.1f * hero.Spellbook.GetSpell(SpellSlot.R).Level);
                            }
                            break;

                        case Champion.Braum:
                            if (hero.HasBuff("braumeshieldbuff"))
                            {
                                num12 *= 1f - (0.275f + (0.025f * hero.Spellbook.GetSpell(SpellSlot.E).Level));
                            }
                            break;

                        case Champion.Galio:
                            if (hero.HasBuff("GalioIdolOfDurand"))
                            {
                                num12 *= 0.5f;
                            }
                            break;

                        case Champion.Garen:
                            if (hero.HasBuff("GarenW"))
                            {
                                num12 *= 0.7f;
                            }
                            break;

                        case Champion.Gragas:
                            if (hero.HasBuff("gragaswself"))
                            {
                                num12 *= 1f - (0.08f + (0.02f * hero.Spellbook.GetSpell(SpellSlot.W).Level));
                            }
                            break;

                        case Champion.Kassadin:
                            if (hero.HasBuff("voidstone") && (damageType == DamageType.Magical))
                            {
                                num12 *= 0.15f;
                            }
                            break;
                    }
                }
                else if (champion2 <= Champion.MasterYi)
                {
                    switch (champion2)
                    {
                        case Champion.Maokai:
                            if (hero.HasBuff("maokaidrain3defense") && (from.Type != GameObjectType.obj_AI_Turret))
                            {
                                num12 *= 0.8f;
                            }
                            break;

                        case Champion.MasterYi:
                            if (hero.HasBuff("Meditate"))
                            {
                                num12 *= 1f - ((0.45f + (0.05f * hero.Spellbook.GetSpell(SpellSlot.W).Level)) / ((from.Type == GameObjectType.obj_AI_Turret) ? 2f : 1f));
                            }
                            break;
                    }
                }
                else if (champion2 != Champion.Shen)
                {
                    if (champion2 == Champion.Urgot)
                    {
                        if (hero.HasBuff("urgotswapdef"))
                        {
                            num12 *= 1f - (0.2f + (0.1f * hero.Spellbook.GetSpell(SpellSlot.R).Level));
                        }
                    }
                    else if (champion2 == Champion.Yorick)
                    {
                    }
                }
                if (isAutoAttackOrTargetted && hero.HasItem(new ItemId[] { ItemId.Ninja_Tabi }))
                {
                    num12 *= 0.88f;
                }
                if (((fromHero != null) && fromHero.HasBuff("itemphantomdancerdebuff")) && hero.HasItem(new int[] { 0xbe6 }))
                {
                    num12 *= 0.88f;
                }
                if (target.HasBuff("Mastery6263"))
                {
                    num12 *= 0.96f;
                }
                if (target.HasBuff("MasteryWardenOfTheDawn"))
                {
                    num12 *= 0.94f;
                }
            }
            if (((minion2 != null) && minion2.IsMelee) && minion2.HasBuff("exaltedwithbaronnashorminion"))
            {
                if (fromHero > null)
                {
                    num12 *= 0.25f;
                }
                else if (from.Type == GameObjectType.obj_AI_Turret)
                {
                    num12 *= 0.7f;
                }
            }
            float num13 = 0f;
            if (hero <= null)
            {
                if (minion2 > null)
                {
                    if (fromHero > null)
                    {
                        if (isAutoAttackOrTargetted)
                        {
                            num13 += fromHero.GetSavageryCount();
                        }
                    }
                    else if (((minion != null) && (Game.MapId == GameMapId.SummonersRift)) && (Game.Time >= 240f))
                    {
                        num13 -= minion2.FlatDamageReductionFromBarracksMinionMod;
                    }
                }
            }
            else
            {
                switch (hero.Hero)
                {
                    case Champion.Amumu:
                        if (damageType == DamageType.Physical)
                        {
                            num13 -= 2f * hero.Spellbook.GetSpell(SpellSlot.E).Level;
                        }
                        break;

                    case Champion.Fizz:
                        if (isAutoAttackOrTargetted)
                        {
                            num13 -= (2f * ((int) ((hero.Level + 2f) / 3f))) + 2f;
                        }
                        break;
                }
            }
            float num14 = 0f;
            if (((isAutoAttackOrTargetted && (damageType == DamageType.Physical)) && (target.Type == GameObjectType.AIHeroClient)) && ((AIHeroClient) target).HasItem(new ItemId[] { ItemId.Dorans_Shield }))
            {
                num14 -= (8f * num10) * num;
            }
            if (((isAbility && (fromHero != null)) && fromHero.HasItem(new int[] { 0x57a, 0x582, 0x586, 0xe59 })) && (fromHero.GetBuffCount("itemmagicshankcharge") == 100))
            {
                num14 += fromHero.CalculateDamageOnUnit(target, DamageType.Magical, (((minion2 != null) && minion2.IsMonster) ? 2.5f : 1f) * (100f + (0.1f * fromHero.FlatMagicDamageMod)), false, false);
            }
            return Math.Max((float) (((((num12 * num10) * num) * (rawDamage + num11)) + num13) + num14), (float) 0f);
        }

        public static Calculator CreateCalculator(Obj_AI_Base sourceUnit) => 
            new Calculator(sourceUnit);

        internal static float GetAutoAttackDamage(this AIHeroClient fromHero, Obj_AI_Base target, PrecalculatedAutoAttackDamage precalculated)
        {
            Func<KeyValuePair<ItemId, float>, bool> <>9__1;
            Func<float, bool> <>9__3;
            int num = Math.Min(0x12, fromHero.Level);
            bool flag = target.Type == GameObjectType.obj_AI_Minion;
            float num2 = precalculated._calculatedPhysical;
            float num3 = precalculated._calculatedMagical;
            float num4 = precalculated._calculatedTrue;
            float rawDamage = precalculated._rawPhysical;
            float num6 = precalculated._rawMagical;
            float num7 = precalculated._rawTotal;
            bool flag2 = false;
            if ((flag && (target.MaxHealth >= 0f)) && (target.MaxHealth <= 6f))
            {
                return 1f;
            }
            if (((((fromHero.IsMelee && target.IsEnemy) && !target.Team.IsNeutral()) & flag) && (fromHero.GetBuffCount("talentreaperdisplay") > 0)) && EntityManager.Heroes.AllHeroes.Any<AIHeroClient>(h => (((h.IsValidTarget(null, false, null) && (h.Team == fromHero.Team)) && !h.IdEquals(fromHero)) && ((Obj_AI_Base) h).IsInRange(((Obj_AI_Base) fromHero), 1050f))))
            {
                Dictionary<ItemId, float> source = new Dictionary<ItemId, float> {
                    { 
                        ItemId.Relic_Shield,
                        195f + (5f * num)
                    },
                    { 
                        ItemId.Targons_Brace,
                        200f + (10f * num)
                    },
                    { 
                        ItemId.Face_of_the_Mountain,
                        320f + (20f * num)
                    },
                    { 
                        ItemId.Eye_of_the_Equinox,
                        320f + (20f * num)
                    }
                };
                using (IEnumerator<float> enumerator = (from pair in source.Where<KeyValuePair<ItemId, float>>((Func<KeyValuePair<ItemId, float>, bool>) (<>9__1 ?? (<>9__1 = pair => fromHero.HasItem(new ItemId[] { pair.Key })))) select pair.Value).Where<float>(((Func<float, bool>) (<>9__3 ?? (<>9__3 = relicDamage => target.Health <= relicDamage)))).GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        return enumerator.Current;
                    }
                }
            }
            ItemId[] itemIds = new ItemId[] { ItemId.Blade_of_the_Ruined_King };
            if (fromHero.HasItem(itemIds))
            {
                float num12 = 0.06f * target.Health;
                if (flag)
                {
                    num12 = Math.Min(num12, 60f);
                }
                rawDamage += Math.Max(num12, 15f);
            }
            ItemId[] idArray2 = new ItemId[] { ItemId.Hunters_Machete };
            if (fromHero.HasItem(idArray2) && target.IsMonster)
            {
                num4 += 25f;
            }
            switch (fromHero.Hero)
            {
                case Champion.Braum:
                    if (target.GetBuffCount("braummarkcounter") == 3)
                    {
                        num6 += 16f + (10f * num);
                    }
                    if (target.HasBuff("braummarkstunreduction"))
                    {
                        num6 += 6.4f + (1.6f * num);
                    }
                    break;

                case Champion.Ekko:
                    if (target.GetBuffCount("EkkoStacks") == 3)
                    {
                        int num13 = new int[] { 
                            30, 40, 50, 60, 70, 80, 0x55, 90, 0x5f, 100, 0x69, 110, 0x73, 120, 0x7d, 130,
                            0x87, 140
                        }[num - 1];
                        num6 += num13 + (0.8f * fromHero.FlatMagicDamageMod);
                    }
                    break;

                case Champion.Fizz:
                    if (fromHero.HasBuff("FizzWActive"))
                    {
                        float num14 = (10f + (15f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level)) + (fromHero.TotalMagicalDamage * 0.333f);
                        if (target.HasBuff("fizzwdot"))
                        {
                            num14 *= 3f;
                        }
                        num6 += num14;
                    }
                    break;

                case Champion.Akali:
                    if (target.HasBuff("AkaliMota"))
                    {
                        num6 += (20f + (25f * fromHero.Spellbook.GetSpell(SpellSlot.Q).Level)) + (0.5f * fromHero.FlatMagicDamageMod);
                    }
                    if (fromHero.HasBuff("akalishadowstate"))
                    {
                        num6 += ((8 + (((num < 11) ? 2 : 10) * num)) + (0.5f * fromHero.FlatPhysicalDamageMod)) + (0.65f * fromHero.TotalMagicalDamage);
                    }
                    break;

                case Champion.Ashe:
                    if (target.HasBuff("ashepassiveslow"))
                    {
                        rawDamage += fromHero.TotalAttackDamage * (0.1f + (fromHero.FlatCritChanceMod * (1f + fromHero.FlatCritDamageMod)));
                    }
                    break;

                case Champion.Gnar:
                    if (target.GetBuffCount("gnarwproc") == 2)
                    {
                        precalculated._autoAttackDamageType = DamageType.Magical;
                        num7 += (10f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level) + Math.Min((float) ((target.MaxHealth * (4f + (2f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level))) / 100f), (float) ((50f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level) + 50f));
                    }
                    break;

                case Champion.Gragas:
                    if (fromHero.HasBuff("gragaswattackbuff"))
                    {
                        num6 += ((-10f + (30f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level)) + (0.3f * fromHero.FlatMagicDamageMod)) + (0.08f * target.MaxHealth);
                    }
                    break;

                case Champion.JarvanIV:
                    if (!target.HasBuff("jarvanivmartialcadencecheck"))
                    {
                        rawDamage += Math.Min((float) 400f, (float) (0.1f * target.Health));
                    }
                    break;

                case Champion.Jhin:
                    if (fromHero.HasBuff("jhinpassiveattackbuff"))
                    {
                        rawDamage += ((target.MaxHealth - target.Health) * ((((int) Math.Truncate((double) (((float) num) / 6f))) * 5f) + 10f)) / 100f;
                        flag2 = true;
                    }
                    break;

                case Champion.Katarina:
                    if (target.HasBuff("katarinaqmark"))
                    {
                        num6 += ((((((float) num) / 1.75f) + (3f * num)) + 71.5f) + fromHero.FlatPhysicalDamageMod) + ((0.55f + (0.15f * ((num - 1) / 5))) * fromHero.FlatMagicDamageMod);
                    }
                    break;

                case Champion.Khazix:
                    if (fromHero.HasBuff("KhazixPDamage") && (target.Type == GameObjectType.AIHeroClient))
                    {
                        num6 += (2 + (num * 8)) + (0.4f * fromHero.FlatPhysicalDamageMod);
                    }
                    break;

                case Champion.KogMaw:
                    if (fromHero.HasBuff("KogMawBioArcaneBarrage"))
                    {
                        float num15 = fromHero.CalculateDamageOnUnit(target, DamageType.Magical, target.MaxHealth * (((float) ((1 + fromHero.Spellbook.GetSpell(SpellSlot.W).Level) + ((int) (fromHero.FlatMagicDamageMod / 100f)))) / 100f), false, false);
                        if (flag)
                        {
                            num15 = Math.Min(num15, 100f);
                        }
                        num3 += num15;
                    }
                    break;

                case Champion.Lulu:
                    if (!ObjectManager.Get<Obj_AI_Base>().Any<Obj_AI_Base>(o => (((!o.IsWard() && !o.IdEquals(target)) && o.IsInRange(target, 100f)) && (o.Distance(((Obj_AI_Base) fromHero), false) < target.Distance(((Obj_AI_Base) fromHero), false)))))
                    {
                        num6 += (-1f + (4f * ((num + 1f) / 2f))) + (0.05f * fromHero.FlatMagicDamageMod);
                    }
                    break;

                case Champion.Lux:
                    if (target.HasBuff("LuxIlluminatingFraulein"))
                    {
                        num6 += (10f + (10f * num)) + (0.2f * fromHero.FlatMagicDamageMod);
                    }
                    break;

                case Champion.Nidalee:
                    if (fromHero.HasBuff("Takedown"))
                    {
                        int level = fromHero.Spellbook.GetSpell(SpellSlot.R).Level;
                        if (level > 0)
                        {
                            float num17 = 0.85f + (0.25f * level);
                            float num18 = ((-20f + (25f * level)) + (0.75f * fromHero.TotalAttackDamage)) + (0.4f * fromHero.TotalMagicalDamage);
                            float num19 = (new float[] { 10f, 67.5f, 137.5f, 220f }[level - 1] + (new float[] { 1.5f, 1.6875f, 1.875f, 2.0625f }[level - 1] * fromHero.TotalAttackDamage)) + ((0.7f + (0.1f * level)) * fromHero.TotalMagicalDamage);
                            float num20 = num18 + (num19 / 2f);
                            float num21 = 0f;
                            if (target.HasBuff("NidaleePassiveHunted"))
                            {
                                num20 *= 1.4f;
                                float num22 = (new float[] { 7f, 42f, 77f, 136f }[level - 1] + (1.05f * fromHero.TotalAttackDamage)) + (0.56f * fromHero.TotalMagicalDamage);
                                float num23 = (new float[] { 14f, 94.5f, 192.5f, 308f }[level - 1] + (new float[] { 2.1f, 2.3625f, 2.625f, 2.8875f }[level - 1] * fromHero.TotalAttackDamage)) + ((1.12f + (0.14f * level)) * fromHero.TotalMagicalDamage);
                                num21 = num22 + (num23 / 2f);
                            }
                            num6 += ((num20 + num21) + (target.MaxHealth - (target.Health * 0.1f))) * num17;
                        }
                    }
                    break;

                case Champion.Orianna:
                {
                    int num8 = (((fromHero.GetBuffCount("orianapowerdaggerdisplay") > 0) && fromHero.IsMe) && ((Orbwalker.LastTarget != null) && (Orbwalker.LastTarget.NetworkId == target.NetworkId))) ? fromHero.GetBuffCount("orianapowerdaggerdisplay") : 0;
                    num6 += (((8f * ((int) Math.Truncate((double) (((float) (num + 2)) / 3f)))) + 2f) + (num8 * ((1.6f * ((int) Math.Truncate((double) (((float) (num + 2)) / 3f)))) + 0.4f))) + (((15f + (3f * num8)) / 100f) * fromHero.FlatMagicDamageMod);
                    break;
                }
                case Champion.Pantheon:
                    if (target.HealthPercent < 15f)
                    {
                        SpellDataInst spell = fromHero.Spellbook.GetSpell(SpellSlot.E);
                        if (spell.IsLearned && (spell.Level > 0))
                        {
                            flag2 = true;
                        }
                    }
                    break;

                case Champion.Quinn:
                    if (target.HasBuff("QuinnW"))
                    {
                        rawDamage += 0.5f * fromHero.TotalAttackDamage;
                    }
                    break;

                case Champion.Renekton:
                    if (fromHero.HasBuff("RenektonExecuteReady"))
                    {
                        rawDamage += (-10f + (20f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level)) + (1.5f * fromHero.TotalAttackDamage);
                    }
                    break;

                case Champion.Shyvana:
                    if (target.BaseSkinName.Contains("SRU_Dragon") || target.BaseSkinName.Contains("TT_Spiderboss"))
                    {
                        num6 += 0.1f * fromHero.TotalMagicalDamage;
                        rawDamage += 0.1f * fromHero.TotalAttackDamage;
                    }
                    if (target.HasBuff("ShyvanaFireballMissile"))
                    {
                        num6 += Math.Min(target.MaxHealth * 0.025f, target.IsMonster ? 100f : target.MaxHealth);
                    }
                    break;

                case Champion.Vayne:
                    if (target.GetBuffCount("vaynesilvereddebuff") == 2)
                    {
                        num4 += Math.Max((float) ((0.045f + (0.015f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level)) * target.MaxHealth), (float) ((20f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level) + 20f));
                    }
                    break;

                case Champion.Yorick:
                    if (fromHero.HasBuff("YorickQBuff"))
                    {
                        rawDamage += (5f + (25f * fromHero.Spellbook.GetSpell(SpellSlot.Q).Level)) + (0.4f * fromHero.TotalAttackDamage);
                    }
                    break;

                case Champion.Zed:
                    if ((target.HealthPercent <= 50f) && !target.HasBuff("zedpassivecd"))
                    {
                        num6 += (target.MaxHealth * ((((int) Math.Truncate((double) ((num - 1f) / 6f))) * 2f) + 6f)) / 100f;
                    }
                    break;
            }
            if (fromHero.Hero == Champion.Kalista)
            {
                if (target.Buffs.Any<BuffInstance>(o => (o.IsValid() && (o.Caster == fromHero)) && (o.Name == "kalistacoopstrikemarkally")))
                {
                    if (flag && (target.Health <= 125f))
                    {
                        return target.Health;
                    }
                    num6 += Math.Min(flag ? new float[] { 75f, 125f, 150f, 175f, 200f }[Math.Min(fromHero.Spellbook.GetSpell(SpellSlot.W).Level, 5) - 1] : target.MaxHealth, new float[] { 0.1f, 0.125f, 0.15f, 0.175f, 0.2f }[Math.Min(fromHero.Spellbook.GetSpell(SpellSlot.W).Level, 5) - 1] * target.MaxHealth);
                }
            }
            else if (EntityManager.Heroes.ContainsKalista && fromHero.HasBuff("KalistaPassiveCoopStrike"))
            {
                AIHeroClient client = EntityManager.Heroes.AllHeroes.FirstOrDefault<AIHeroClient>(o => (o.Team == fromHero.Team) && (o.Hero == Champion.Kalista));
                if ((client != null) && target.Buffs.Any<BuffInstance>(o => (o.IsValid() && (o.Name == "kalistacoopstrikemarkself"))))
                {
                    if (flag && (target.Health <= 125f))
                    {
                        return target.Health;
                    }
                    num6 += Math.Min(flag ? new float[] { 75f, 125f, 150f, 175f, 200f }[Math.Min(client.Spellbook.GetSpell(SpellSlot.W).Level, 5) - 1] : target.MaxHealth, new float[] { 0.1f, 0.125f, 0.15f, 0.175f, 0.2f }[Math.Min(client.Spellbook.GetSpell(SpellSlot.W).Level, 5) - 1] * target.MaxHealth);
                }
            }
            if (fromHero.IsMe && (fromHero.Hero == Champion.Azir))
            {
                int num24 = Orbwalker.AzirSoldiers.Count<Obj_AI_Minion>(i => i.IsInAutoAttackRange(target));
                if (num24 > 0)
                {
                    num7 = (new int[] { 
                        50, 0x34, 0x36, 0x38, 0x3a, 60, 0x3f, 0x42, 70, 80, 90, 100, 110, 120, 130, 140,
                        150, 160, 170
                    }[num - 1] + (fromHero.FlatMagicDamageMod * 0.6f)) * ((num24 * 0.25f) + 0.75f);
                    precalculated._autoAttackDamageType = DamageType.Magical;
                }
            }
            if (fromHero.HasBuff("BlessingoftheLizardElder") && !target.HasBuff("Bruning"))
            {
                num4 += 2 + (2 * num);
            }
            switch (precalculated._autoAttackDamageType)
            {
                case DamageType.Physical:
                    rawDamage += num7;
                    break;

                case DamageType.Magical:
                    num6 += num7;
                    break;

                case DamageType.True:
                    num4 += num7;
                    break;
            }
            if (rawDamage > 0f)
            {
                num2 += fromHero.CalculateDamageOnUnit(target, DamageType.Physical, rawDamage, false, precalculated._autoAttackDamageType == DamageType.Physical);
            }
            if (num6 > 0f)
            {
                num3 += fromHero.CalculateDamageOnUnit(target, DamageType.Magical, num6, false, precalculated._autoAttackDamageType == DamageType.Magical);
            }
            float num9 = 1f;
            if ((Math.Abs((float) (fromHero.FlatCritChanceMod - 1f)) < float.Epsilon) | flag2)
            {
                num9 *= fromHero.GetCriticalStrikePercentMod();
            }
            return (((num9 * num2) + num3) + num4);
        }

        public static float GetAutoAttackDamage(this Obj_AI_Base from, Obj_AI_Base target, bool respectPassives = false)
        {
            if (from == null)
            {
                return 0f;
            }
            if (target == null)
            {
                return 0f;
            }
            AIHeroClient fromHero = from as AIHeroClient;
            if (respectPassives && (fromHero > null))
            {
                return fromHero.GetAutoAttackDamage(target, fromHero.GetStaticAutoAttackDamage((target.Type == GameObjectType.obj_AI_Minion)));
            }
            return from.CalculateDamageOnUnit(target, DamageType.Physical, from.TotalAttackDamage, false, true);
        }

        public static float GetCriticalStrikePercentMod(this AIHeroClient fromHero)
        {
            ItemId[] itemIds = new ItemId[] { ItemId.Infinity_Edge };
            float num = (fromHero.HasItem(itemIds) ? 0.5f : 0f) + 2f;
            switch (fromHero.Hero)
            {
                case Champion.Jhin:
                    return (num * 0.75f);

                case Champion.XinZhao:
                    return (num - (0.875f - (0.125f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level)));

                case Champion.Yasuo:
                    return (num * 0.9f);
            }
            return num;
        }

        internal static int GetMercilessCount(this AIHeroClient hero) => 
            (hero.IsMe ? Orbwalker.FarmingMenu["merciless"].Cast<Slider>().CurrentValue : 0);

        internal static int GetSavageryCount(this AIHeroClient hero) => 
            (hero.IsMe ? Orbwalker.FarmingMenu["savagery"].Cast<Slider>().CurrentValue : 0);

        internal static PrecalculatedAutoAttackDamage GetStaticAutoAttackDamage(this AIHeroClient fromHero, bool targetIsMinion)
        {
            PrecalculatedAutoAttackDamage damage = new PrecalculatedAutoAttackDamage {
                _rawTotal = fromHero.TotalAttackDamage
            };
            int num = Math.Min(fromHero.Level, 0x12);
            ItemId[] itemIds = new ItemId[] { ItemId.Nashors_Tooth };
            if (fromHero.HasItem(itemIds))
            {
                damage._rawMagical += 15f + (0.15f * fromHero.FlatMagicDamageMod);
            }
            ItemId[] idArray2 = new ItemId[] { ItemId.Runaans_Hurricane, ItemId.Recurve_Bow };
            if (fromHero.HasItem(idArray2))
            {
                damage._rawPhysical += 15f;
            }
            ItemId[] idArray3 = new ItemId[] { ItemId.Wits_End };
            if (fromHero.HasItem(idArray3))
            {
                damage._rawMagical += 40f;
            }
            ItemId[] idArray4 = new ItemId[] { ItemId.Sheen };
            if (fromHero.HasItem(idArray4) && fromHero.HasBuff("sheen"))
            {
                damage._rawPhysical += fromHero.BaseAttackDamage;
            }
            ItemId[] idArray5 = new ItemId[] { ItemId.Lich_Bane };
            if (fromHero.HasItem(idArray5) && fromHero.HasBuff("lichbane"))
            {
                damage._rawMagical += (0.75f * fromHero.BaseAttackDamage) + (0.5f * fromHero.FlatMagicDamageMod);
            }
            ItemId[] idArray6 = new ItemId[] { ItemId.Trinity_Force };
            if (fromHero.HasItem(idArray6) && fromHero.HasBuff("sheen"))
            {
                damage._rawPhysical += 2f * fromHero.BaseAttackDamage;
            }
            if (fromHero.GetBuffCount("itemstatikshankcharge") == 100)
            {
                float num2 = 0f;
                float num3 = 0f;
                float num4 = 0f;
                ItemId[] idArray7 = new ItemId[] { ItemId.Statikk_Shiv };
                if (fromHero.HasItem(idArray7))
                {
                    num2 += (targetIsMinion ? 2.2f : 1f) * new int[] { 
                        50, 50, 50, 50, 50, 0x38, 0x3d, 0x43, 0x48, 0x4d, 0x53, 0x58, 0x5e, 0x63, 0x68, 110,
                        0x73, 120
                    }[num - 1];
                }
                ItemId[] idArray8 = new ItemId[] { ItemId.Kircheis_Shard };
                if (fromHero.HasItem(idArray8))
                {
                    num3 += 40f;
                }
                ItemId[] idArray9 = new ItemId[] { ItemId.Rapid_Firecannon };
                if (fromHero.HasItem(idArray9))
                {
                    num4 += new int[] { 
                        50, 50, 50, 50, 50, 0x3a, 0x42, 0x4b, 0x53, 0x5c, 100, 0x6d, 0x75, 0x7e, 0x86, 0x8f,
                        0x97, 160
                    }[num - 1];
                }
                damage._rawMagical = new float[] { num2, num3, num4 }.Max();
            }
            ItemId[] idArray10 = new ItemId[] { ItemId.Guinsoos_Rageblade };
            if (fromHero.HasItem(idArray10))
            {
                damage._calculatedMagical += 15f;
            }
            if (fromHero.IsMelee)
            {
                int[] numArray1 = new int[] { 0xe9e };
                if (fromHero.HasItem(numArray1))
                {
                    int buffCount = fromHero.GetBuffCount("DreadnoughtMomentumBuff");
                    if (buffCount > 0)
                    {
                        damage._rawPhysical += ((float) (((buffCount == 100) ? 2 : 1) * buffCount)) / 2f;
                    }
                }
                int[] numArray2 = new int[] { 0xea4 };
                if (fromHero.HasItem(numArray2))
                {
                    damage._rawPhysical += fromHero.HasBuff("itemtitanichydracleavebuff") ? (40f + (0.1f * fromHero.MaxHealth)) : (5f + (0.01f * fromHero.MaxHealth));
                }
            }
            Champion hero = fromHero.Hero;
            if (hero <= Champion.Alistar)
            {
                switch (hero)
                {
                    case Champion.Aatrox:
                        if (fromHero.HasBuff("AatroxWONHPowerBuff"))
                        {
                            damage._rawPhysical += (10f + (35f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level)) + (fromHero.FlatPhysicalDamageMod * 0.75f);
                        }
                        return damage;

                    case Champion.Alistar:
                        if (fromHero.HasBuff("alistartrample"))
                        {
                            damage._rawMagical += 40f + (10 * num);
                        }
                        return damage;
                }
                return damage;
            }
            switch (hero)
            {
                case Champion.Blitzcrank:
                    if (fromHero.HasBuff("PowerFist"))
                    {
                        damage._rawPhysical += fromHero.TotalAttackDamage;
                    }
                    return damage;

                case Champion.Brand:
                case Champion.Braum:
                case Champion.Camille:
                case Champion.Cassiopeia:
                case Champion.DrMundo:
                    return damage;

                case Champion.Caitlyn:
                    if (fromHero.HasBuff("caitlynheadshot"))
                    {
                        if (!targetIsMinion)
                        {
                            damage._rawPhysical += fromHero.TotalAttackDamage * (0.5f + (fromHero.FlatCritChanceMod * (1f + fromHero.FlatCritDamageMod)));
                            return damage;
                        }
                        damage._rawTotal *= 2.5f;
                    }
                    return damage;

                case Champion.Chogath:
                    if (fromHero.HasBuff("VorpalSpikes"))
                    {
                        damage._rawMagical += (5f + (15f * fromHero.Spellbook.GetSpell(SpellSlot.E).Level)) + (0.3f * fromHero.FlatMagicDamageMod);
                    }
                    return damage;

                case Champion.Corki:
                    damage._rawTotal *= 0.5f;
                    damage._rawMagical = damage._rawTotal;
                    return damage;

                case Champion.Darius:
                    if (fromHero.HasBuff("DariusNoxianTacticsONH"))
                    {
                        damage._rawPhysical += 0.4f * fromHero.TotalAttackDamage;
                    }
                    return damage;

                case Champion.Diana:
                    if (fromHero.GetBuffCount("dianapassivemarker") == 2)
                    {
                        damage._rawMagical += new int[] { 
                            20, 0x19, 30, 0x23, 40, 50, 60, 70, 80, 90, 0x69, 120, 0x87, 0x9b, 0xaf, 200,
                            0xe1, 250
                        }[num - 1] + (0.8f * fromHero.FlatMagicDamageMod);
                    }
                    return damage;

                case Champion.Draven:
                    if (fromHero.HasBuff("dravenspinningattack"))
                    {
                        int level = fromHero.Spellbook.GetSpell(SpellSlot.Q).Level;
                        damage._rawPhysical += (0x19 + (5 * level)) + (fromHero.FlatPhysicalDamageMod * (0.55f + (level * 0.1f)));
                    }
                    return damage;

                case Champion.Ashe:
                    if (fromHero.HasBuff("asheqbuff"))
                    {
                        damage._rawPhysical += ((100f + (5f * fromHero.Spellbook.GetSpell(SpellSlot.Q).Level)) / 100f) * fromHero.TotalAttackDamage;
                    }
                    return damage;

                case Champion.Fiora:
                    return damage;

                case Champion.Fizz:
                    if (fromHero.HasBuff("FizzSeastonePassive"))
                    {
                        damage._rawMagical += (10f + (15f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level)) + (0.333f * fromHero.FlatMagicDamageMod);
                    }
                    return damage;

                case Champion.Galio:
                    if (fromHero.HasBuff("GalioPassiveBuff"))
                    {
                        damage._rawPhysical += ((10f + (5f * num)) + (0.4f * fromHero.TotalMagicalDamage)) + (0.4f * fromHero.CharData.SpellBlock);
                    }
                    return damage;

                case Champion.Gangplank:
                case Champion.Gnar:
                case Champion.Gragas:
                case Champion.Heimerdinger:
                case Champion.Illaoi:
                case Champion.Ivern:
                case Champion.Janna:
                case Champion.JarvanIV:
                case Champion.Jhin:
                case Champion.Karma:
                case Champion.Karthus:
                case Champion.Katarina:
                case Champion.Kindred:
                case Champion.Kled:
                case Champion.KogMaw:
                case Champion.Leblanc:
                case Champion.LeeSin:
                case Champion.Lissandra:
                case Champion.Lulu:
                case Champion.Lux:
                case Champion.Malzahar:
                case Champion.Maokai:
                case Champion.Morgana:
                case Champion.Nidalee:
                case Champion.Olaf:
                case Champion.Orianna:
                case Champion.Pantheon:
                case Champion.PracticeTool_TargetDummy:
                case Champion.Quinn:
                case Champion.Rakan:
                case Champion.Rammus:
                case Champion.Renekton:
                case Champion.Ryze:
                case Champion.Singed:
                case Champion.Sivir:
                case Champion.Skarner:
                case Champion.Soraka:
                case Champion.Swain:
                case Champion.Syndra:
                case Champion.Taliyah:
                case Champion.Talon:
                case Champion.Tristana:
                case Champion.Tryndamere:
                case Champion.Twitch:
                case Champion.Urgot:
                case Champion.Veigar:
                case Champion.Velkoz:
                case Champion.Vladimir:
                case Champion.Xayah:
                case Champion.Xerath:
                case Champion.Yasuo:
                case Champion.Zac:
                case Champion.Zed:
                    return damage;

                case Champion.Garen:
                    if (fromHero.HasBuff("GarenQ"))
                    {
                        damage._rawPhysical += (5f + (25f * fromHero.Spellbook.GetSpell(SpellSlot.Q).Level)) + (0.4f * fromHero.TotalAttackDamage);
                    }
                    return damage;

                case Champion.Graves:
                    damage._rawTotal *= ((float) new int[] { 
                        70, 0x47, 0x48, 0x4a, 0x4b, 0x4c, 0x4e, 80, 0x51, 0x53, 0x55, 0x57, 0x59, 0x5b, 0x5f, 0x60,
                        0x61, 100
                    }[num - 1]) / 100f;
                    return damage;

                case Champion.Hecarim:
                    if (fromHero.HasBuff("HecarimRamp"))
                    {
                        damage._rawPhysical += (5f + (35f * fromHero.Spellbook.GetSpell(SpellSlot.E).Level)) + (0.5f * fromHero.FlatPhysicalDamageMod);
                    }
                    return damage;

                case Champion.Irelia:
                    if (fromHero.HasBuff("ireliahitenstylecharged"))
                    {
                        damage._calculatedTrue += 15f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level;
                    }
                    return damage;

                case Champion.Jax:
                    if (fromHero.HasBuff("JaxEmpowerTwo"))
                    {
                        damage._rawMagical += (5f + (35f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level)) + (0.6f * fromHero.FlatMagicDamageMod);
                    }
                    return damage;

                case Champion.Jayce:
                    if (fromHero.HasBuff("jaycepassivemeleeattack"))
                    {
                        damage._rawMagical += (-20f + (40f * fromHero.Spellbook.GetSpell(SpellSlot.R).Level)) + (0.4f * fromHero.FlatMagicDamageMod);
                    }
                    return damage;

                case Champion.Jinx:
                    if (fromHero.Spellbook.GetSpell(SpellSlot.Q).ToggleState == 2)
                    {
                        damage._rawTotal *= 1.1f;
                    }
                    return damage;

                case Champion.Kalista:
                    damage._rawTotal *= 0.9f;
                    return damage;

                case Champion.Kassadin:
                    if (fromHero.Spellbook.GetSpell(SpellSlot.W).Level > 0)
                    {
                        damage._rawMagical += 20f + (0.1f * fromHero.FlatMagicDamageMod);
                    }
                    if (fromHero.HasBuff("NetherBladeArmorPen"))
                    {
                        damage._rawMagical += (-5f + (25f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level)) + (0.6f * fromHero.FlatMagicDamageMod);
                    }
                    return damage;

                case Champion.Kayle:
                    if (fromHero.Spellbook.GetSpell(SpellSlot.E).Level > 0)
                    {
                        damage._rawMagical += (5f + (5f * fromHero.Spellbook.GetSpell(SpellSlot.E).Level)) + (0.15f * fromHero.FlatMagicDamageMod);
                        if (fromHero.HasBuff("JudicatorRighteousFury"))
                        {
                            damage._rawMagical += (5f + (5f * fromHero.Spellbook.GetSpell(SpellSlot.E).Level)) + (0.15f * fromHero.FlatMagicDamageMod);
                        }
                    }
                    return damage;

                case Champion.Kennen:
                    if (fromHero.HasBuff("kennendoublestrikelive"))
                    {
                        damage._rawMagical += (0.3f + (0.1f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level)) * fromHero.TotalAttackDamage;
                    }
                    return damage;

                case Champion.Khazix:
                    return damage;

                case Champion.Leona:
                    if (fromHero.HasBuff("LeonaShieldOfDaybreak"))
                    {
                        damage._rawMagical += (10f + (30f * fromHero.Spellbook.GetSpell(SpellSlot.Q).Level)) + (0.3f * fromHero.FlatMagicDamageMod);
                    }
                    return damage;

                case Champion.Lucian:
                    if (fromHero.HasBuff("lucianpassivebuff"))
                    {
                        float num7 = targetIsMinion ? 1f : ((num >= 13) ? 0.6f : ((num >= 7) ? 0.5f : 0.4f));
                        damage._rawTotal += num7 * fromHero.TotalAttackDamage;
                    }
                    return damage;

                case Champion.Malphite:
                    if (fromHero.HasBuff("MalphiteCleave"))
                    {
                        damage._rawPhysical += 15f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level;
                    }
                    return damage;

                case Champion.MasterYi:
                    if (fromHero.HasBuff("doublestrike"))
                    {
                        damage._rawTotal *= 1.5f;
                    }
                    if (fromHero.HasBuff("wujustylesuperchargedvisual"))
                    {
                        damage._calculatedTrue += ((9f * fromHero.Spellbook.GetSpell(SpellSlot.E).Level) + 5f) + (0.25f * fromHero.FlatPhysicalDamageMod);
                    }
                    return damage;

                case Champion.MissFortune:
                    return damage;

                case Champion.Mordekaiser:
                {
                    float num8 = ((10f * fromHero.Spellbook.GetSpell(SpellSlot.Q).Level) + ((0.4f + (0.1f * fromHero.Spellbook.GetSpell(SpellSlot.Q).Level)) * fromHero.TotalAttackDamage)) + (0.6f * fromHero.TotalMagicalDamage);
                    if (fromHero.HasBuff("mordekaisermaceofspades1") || fromHero.HasBuff("mordekaisermaceofspades15"))
                    {
                        damage._rawMagical += num8;
                    }
                    if (fromHero.HasBuff("mordekaisermaceofspades2"))
                    {
                        damage._rawMagical += num8 * 2f;
                    }
                    return damage;
                }
                case Champion.Nami:
                    return damage;

                case Champion.Nasus:
                    if (fromHero.HasBuff("NasusQ"))
                    {
                        damage._rawPhysical += (Math.Max((float) fromHero.GetBuffCount("nasusqstacks"), 0f) + 10f) + (20f * fromHero.Spellbook.GetSpell(SpellSlot.Q).Level);
                    }
                    return damage;

                case Champion.Nautilus:
                    return damage;

                case Champion.Nocturne:
                    if (fromHero.HasBuff("nocturneumbrablades"))
                    {
                        damage._rawPhysical += 1.2f * fromHero.TotalAttackDamage;
                    }
                    return damage;

                case Champion.Nunu:
                    return damage;

                case Champion.Poppy:
                    return damage;

                case Champion.RekSai:
                    return damage;

                case Champion.Rengar:
                    return damage;

                case Champion.Riven:
                    if (fromHero.GetBuffCount("rivenpassiveaaboost") > 0)
                    {
                        damage._rawPhysical += ((20f + (5f * ((int) Math.Truncate((double) ((num + 2f) / 3f))))) * fromHero.TotalAttackDamage) / 100f;
                    }
                    return damage;

                case Champion.Rumble:
                    return damage;

                case Champion.Sejuani:
                    return damage;

                case Champion.Shaco:
                    if (fromHero.HasBuff("Deceive"))
                    {
                        damage._rawPhysical += (5f + (15f * fromHero.Spellbook.GetSpell(SpellSlot.Q).Level)) + (0.4f * fromHero.FlatMagicDamageMod);
                    }
                    return damage;

                case Champion.Shen:
                    return damage;

                case Champion.Shyvana:
                    if (fromHero.HasBuff("ShyvanaDoubleAttack"))
                    {
                        damage._rawPhysical += 0.25f + ((0.15f * fromHero.Spellbook.GetSpell(SpellSlot.Q).Level) * fromHero.TotalAttackDamage);
                    }
                    return damage;

                case Champion.Sion:
                    return damage;

                case Champion.Sona:
                    if (fromHero.HasBuff("sonapassiveattack"))
                    {
                        damage._rawMagical += new int[] { 
                            13, 20, 0x1b, 0x23, 0x2b, 0x34, 0x3e, 0x48, 0x52, 0x5c, 0x66, 0x70, 0x7a, 0x84, 0x93, 0xa2,
                            0xb1, 0xc0
                        }[num - 1] + (0.2f * fromHero.FlatMagicDamageMod);
                    }
                    return damage;

                case Champion.TahmKench:
                    return damage;

                case Champion.Taric:
                    return damage;

                case Champion.Teemo:
                    if (fromHero.HasBuff("ToxicShot"))
                    {
                        damage._rawMagical += (0.3f * fromHero.FlatMagicDamageMod) + (10f * fromHero.Spellbook.GetSpell(SpellSlot.E).Level);
                    }
                    return damage;

                case Champion.Thresh:
                    if (fromHero.Spellbook.GetSpell(SpellSlot.E).Level > 0)
                    {
                        float num9 = Math.Max((float) fromHero.GetBuffCount("threshpassivesouls"), 0f) + ((0.5f + (0.3f * fromHero.Spellbook.GetSpell(SpellSlot.E).Level)) * fromHero.TotalAttackDamage);
                        if (fromHero.HasBuff("threshqpassive4"))
                        {
                            num9 /= 1f;
                        }
                        else if (fromHero.HasBuff("threshqpassive3"))
                        {
                            num9 /= 2f;
                        }
                        else if (fromHero.HasBuff("threshqpassive2"))
                        {
                            num9 /= 3f;
                        }
                        else
                        {
                            num9 /= 4f;
                        }
                        damage._rawMagical += num9;
                    }
                    return damage;

                case Champion.Trundle:
                    return damage;

                case Champion.TwistedFate:
                    if (fromHero.HasBuff("CardMasterStackParticle"))
                    {
                        damage._rawMagical += ((25f * fromHero.Spellbook.GetSpell(SpellSlot.E).Level) + 30f) + (0.5f * fromHero.FlatMagicDamageMod);
                    }
                    if (fromHero.HasBuff("BlueCardPreAttack"))
                    {
                        damage._autoAttackDamageType = DamageType.Magical;
                        damage._rawTotal += ((20f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level) + 20f) + (0.5f * fromHero.FlatMagicDamageMod);
                        return damage;
                    }
                    if (fromHero.HasBuff("RedCardPreAttack"))
                    {
                        damage._autoAttackDamageType = DamageType.Magical;
                        damage._rawTotal += ((15f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level) + 15f) + (0.5f * fromHero.FlatMagicDamageMod);
                        return damage;
                    }
                    if (fromHero.HasBuff("GoldCardPreAttack"))
                    {
                        damage._autoAttackDamageType = DamageType.Magical;
                        damage._rawTotal += ((7.5f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level) + 7.5f) + (0.5f * fromHero.FlatMagicDamageMod);
                    }
                    return damage;

                case Champion.Udyr:
                    if (fromHero.HasBuff("UdyrTigerStance"))
                    {
                        damage._rawPhysical += 1.15f * fromHero.TotalAttackDamage;
                    }
                    if (fromHero.HasBuff("UdyrTigerPunch"))
                    {
                        damage._rawPhysical += (-10f + (25f * fromHero.Spellbook.GetSpell(SpellSlot.Q).Level)) + ((0.55f + (0.05f * fromHero.Spellbook.GetSpell(SpellSlot.Q).Level)) * fromHero.TotalAttackDamage);
                    }
                    if (fromHero.GetBuffCount("UdyrPhoenixStance") > 2)
                    {
                        damage._rawMagical += (40f * fromHero.Spellbook.GetSpell(SpellSlot.R).Level) + (0.45f * fromHero.TotalMagicalDamage);
                    }
                    return damage;

                case Champion.Varus:
                    if (fromHero.Spellbook.GetSpell(SpellSlot.W).Level > 0)
                    {
                        damage._rawMagical += (6f + (4f * fromHero.Spellbook.GetSpell(SpellSlot.W).Level)) + (0.25f * fromHero.FlatMagicDamageMod);
                    }
                    return damage;

                case Champion.Vayne:
                    if (fromHero.HasBuff("vaynetumblebonus"))
                    {
                        damage._rawPhysical += (0.25f + (0.05f * fromHero.Spellbook.GetSpell(SpellSlot.Q).Level)) * fromHero.TotalAttackDamage;
                    }
                    return damage;

                case Champion.Vi:
                    return damage;

                case Champion.Viktor:
                    if (fromHero.HasBuff("viktorpowertransferreturn"))
                    {
                        damage._autoAttackDamageType = DamageType.Magical;
                        damage._rawTotal += new int[] { 
                            20, 0x19, 30, 0x23, 40, 0x2d, 50, 0x37, 60, 70, 80, 90, 110, 130, 150, 170,
                            190, 210
                        }[num - 1] + (0.5f * fromHero.FlatMagicDamageMod);
                    }
                    return damage;

                case Champion.Volibear:
                    if (fromHero.HasBuff("VolibearQ"))
                    {
                        damage._rawPhysical += 30f * fromHero.Spellbook.GetSpell(SpellSlot.Q).Level;
                    }
                    return damage;

                case Champion.Warwick:
                    damage._rawMagical += 8 + (2 * num);
                    return damage;

                case Champion.MonkeyKing:
                    if (fromHero.HasBuff("MonkeyKingDoubleAttack"))
                    {
                        damage._rawPhysical += (30f * fromHero.Spellbook.GetSpell(SpellSlot.Q).Level) + (0.1f * fromHero.TotalAttackDamage);
                    }
                    return damage;

                case Champion.XinZhao:
                    if (fromHero.HasBuff("XenZhaoComboTarget"))
                    {
                        damage._rawPhysical += (15f * fromHero.Spellbook.GetSpell(SpellSlot.Q).Level) + (0.2f * fromHero.TotalAttackDamage);
                    }
                    return damage;

                case Champion.Yorick:
                    return damage;

                case Champion.Ziggs:
                    if (fromHero.HasBuff("ziggsshortfuse"))
                    {
                        damage._rawMagical += (1f * new int[] { 
                            20, 0x18, 0x1c, 0x20, 0x24, 40, 0x30, 0x38, 0x40, 0x48, 80, 0x58, 100, 0x70, 0x7c, 0x88,
                            0x94, 160
                        }[num - 1]) + ((fromHero.FlatMagicDamageMod * new int[] { 
                            0x19, 0x19, 0x19, 0x19, 0x19, 0x19, 30, 30, 30, 30, 30, 30, 0x23, 0x23, 0x23, 0x23,
                            0x23, 0x23
                        }[num - 1]) / 100f);
                    }
                    return damage;
            }
            return damage;
        }

        internal static bool HasAssassin(this AIHeroClient hero) => 
            (!hero.IsMe ? false : Orbwalker.FarmingMenu["assassin"].Cast<CheckBox>().CurrentValue);

        internal static bool HasDoubleEdgedSword(this AIHeroClient hero) => 
            (!hero.IsMe ? false : Orbwalker.FarmingMenu["doubleEdgedSword"].Cast<CheckBox>().CurrentValue);

        internal static bool IsLaneTurret(this Obj_AI_Base turret) => 
            ObjectNames.LaneTurrets.Contains(turret.BaseSkinName);

        internal static bool IsSiegeMinion(this Obj_AI_Base minion) => 
            minion.BaseSkinName.Contains("Siege");

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Damage.<>c <>9 = new Damage.<>c();
            public static Func<KeyValuePair<ItemId, float>, float> <>9__8_2;
            public static Func<BuffInstance, bool> <>9__8_7;

            internal float <GetAutoAttackDamage>b__8_2(KeyValuePair<ItemId, float> pair) => 
                pair.Value;

            internal bool <GetAutoAttackDamage>b__8_7(BuffInstance o) => 
                (o.IsValid() && (o.Name == "kalistacoopstrikemarkself"));
        }

        public class BonusDamageSource : Damage.DamageSourceBase
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Func<Obj_AI_Base, bool> <Condition>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private float[] <DamagePercentages>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.DamageType <DamageType>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.SDK.Damage.ScalingTarget <ScalingTarget>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.SDK.Damage.ScalingType <ScalingType>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private SpellSlot <Slot>k__BackingField;

            public BonusDamageSource(SpellSlot slot, EloBuddy.DamageType damageType)
            {
                this.Slot = slot;
                this.DamageType = damageType;
            }

            public override float GetDamage(Obj_AI_Base source, Obj_AI_Base target)
            {
                SpellDataInst spell = source.Spellbook.GetSpell(this.Slot);
                if ((((spell == null) || (spell.Level == 0)) || ((this.Condition != null) && !this.Condition(target))) || (spell.Level > this.DamagePercentages.Length))
                {
                    return 0f;
                }
                float flatMagicDamageMod = 0f;
                Obj_AI_Base base2 = (this.ScalingTarget == EloBuddy.SDK.Damage.ScalingTarget.Source) ? source : target;
                switch (this.ScalingType)
                {
                    case EloBuddy.SDK.Damage.ScalingType.AbilityPoints:
                        flatMagicDamageMod = base2.FlatMagicDamageMod;
                        break;

                    case EloBuddy.SDK.Damage.ScalingType.AttackPoints:
                        flatMagicDamageMod = base2.TotalAttackDamage;
                        break;

                    case EloBuddy.SDK.Damage.ScalingType.BonusAbilityPoints:
                        flatMagicDamageMod = base2.FlatMagicDamageMod;
                        break;

                    case EloBuddy.SDK.Damage.ScalingType.BonusAttackPoints:
                        flatMagicDamageMod = base2.FlatPhysicalDamageMod;
                        break;

                    case EloBuddy.SDK.Damage.ScalingType.Armor:
                        flatMagicDamageMod = base2.Armor + base2.FlatArmorMod;
                        break;

                    case EloBuddy.SDK.Damage.ScalingType.MagicResist:
                        flatMagicDamageMod = base2.SpellBlock;
                        break;

                    case EloBuddy.SDK.Damage.ScalingType.CurrentHealth:
                        flatMagicDamageMod = base2.Health;
                        break;

                    case EloBuddy.SDK.Damage.ScalingType.CurrentMana:
                        flatMagicDamageMod = base2.Mana;
                        break;

                    case EloBuddy.SDK.Damage.ScalingType.MaxHealth:
                        flatMagicDamageMod = base2.MaxHealth;
                        break;

                    case EloBuddy.SDK.Damage.ScalingType.MaxMana:
                        flatMagicDamageMod = base2.MaxMana;
                        break;

                    case EloBuddy.SDK.Damage.ScalingType.MissingHealth:
                        flatMagicDamageMod = base2.MaxHealth - base2.Health;
                        break;
                }
                return source.CalculateDamageOnUnit(target, this.DamageType, (flatMagicDamageMod * this.DamagePercentages[spell.Level - 1]), true, false);
            }

            public Func<Obj_AI_Base, bool> Condition { get; set; }

            public float[] DamagePercentages { get; set; }

            public EloBuddy.DamageType DamageType { get; set; }

            public EloBuddy.SDK.Damage.ScalingTarget ScalingTarget { get; set; }

            public EloBuddy.SDK.Damage.ScalingType ScalingType { get; set; }

            public SpellSlot Slot { get; set; }
        }

        public class Calculator
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Obj_AI_Base <SourceUnit>k__BackingField;
            protected readonly Dictionary<Guid, Damage.DamageSourceBase> DamageSources = new Dictionary<Guid, Damage.DamageSourceBase>();

            internal Calculator(Obj_AI_Base sourceUnit)
            {
                this.SourceUnit = sourceUnit;
            }

            public Damage.Calculator AddBonusDamageSource(SpellSlot slot, DamageType damageType, float[] damagePercentages, Damage.ScalingType scalingType, Damage.ScalingTarget scalingTarget = 0, Func<Obj_AI_Base, bool> condition = null)
            {
                Guid guid;
                return this.AddBonusDamageSource(out guid, slot, damageType, damagePercentages, scalingType, scalingTarget, null);
            }

            public Damage.Calculator AddBonusDamageSource(out Guid damageId, SpellSlot slot, DamageType damageType, float[] damagePercentages, Damage.ScalingType scalingType, Damage.ScalingTarget scalingTarget = 0, Func<Obj_AI_Base, bool> condition = null)
            {
                damageId = Guid.NewGuid();
                Damage.BonusDamageSource source1 = new Damage.BonusDamageSource(slot, damageType) {
                    ScalingType = scalingType,
                    ScalingTarget = scalingTarget,
                    DamagePercentages = damagePercentages,
                    Condition = condition
                };
                this.DamageSources[damageId] = source1;
                return this;
            }

            public Damage.Calculator AddDamageSource(Damage.DamageSourceBase damageSource)
            {
                this.DamageSources[Guid.NewGuid()] = damageSource;
                return this;
            }

            public Damage.Calculator AddDamageSource(SpellSlot slot, DamageType damageType, float[] damages, Func<Obj_AI_Base, bool> condition = null)
            {
                Guid guid;
                return this.AddDamageSource(out guid, slot, damageType, damages, null);
            }

            public Damage.Calculator AddDamageSource(out Guid damageId, SpellSlot slot, DamageType damageType, float[] damages, Func<Obj_AI_Base, bool> condition = null)
            {
                damageId = Guid.NewGuid();
                Damage.DamageSource source1 = new Damage.DamageSource(slot, damageType) {
                    Damages = damages,
                    Condition = condition
                };
                this.DamageSources[damageId] = source1;
                return this;
            }

            public float GetDamage(Obj_AI_Base target) => 
                this.DamageSources.Sum<KeyValuePair<Guid, Damage.DamageSourceBase>>(((Func<KeyValuePair<Guid, Damage.DamageSourceBase>, float>) (o => o.Value.GetDamage(this.SourceUnit, target))));

            public void RemoveDamageSource(Damage.DamageSourceBase damageSource)
            {
                Func<KeyValuePair<Guid, Damage.DamageSourceBase>, bool> <>9__0;
                foreach (KeyValuePair<Guid, Damage.DamageSourceBase> pair in this.DamageSources.ToArray<KeyValuePair<Guid, Damage.DamageSourceBase>>().Where<KeyValuePair<Guid, Damage.DamageSourceBase>>((Func<KeyValuePair<Guid, Damage.DamageSourceBase>, bool>) (<>9__0 ?? (<>9__0 = entry => entry.Value == damageSource))))
                {
                    this.DamageSources.Remove(pair.Key);
                }
            }

            public void RemoveDamageSource(Guid damageId)
            {
                this.DamageSources.Remove(damageId);
            }

            public Obj_AI_Base SourceUnit { get; protected set; }
        }

        public class DamageSource : Damage.DamageSourceBase
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Func<Obj_AI_Base, bool> <Condition>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private float[] <Damages>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.DamageType <DamageType>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private SpellSlot <Slot>k__BackingField;

            public DamageSource(SpellSlot slot, EloBuddy.DamageType damageType)
            {
                this.Slot = slot;
                this.DamageType = damageType;
            }

            public override float GetDamage(Obj_AI_Base source, Obj_AI_Base target)
            {
                SpellDataInst spell = source.Spellbook.GetSpell(this.Slot);
                if ((((spell == null) || (spell.Level == 0)) || ((this.Condition != null) && !this.Condition(target))) || (spell.Level > this.Damages.Length))
                {
                    return 0f;
                }
                return source.CalculateDamageOnUnit(target, this.DamageType, this.Damages[spell.Level - 1], true, false);
            }

            public Func<Obj_AI_Base, bool> Condition { get; set; }

            public float[] Damages { get; set; }

            public EloBuddy.DamageType DamageType { get; set; }

            public SpellSlot Slot { get; set; }
        }

        public abstract class DamageSourceBase
        {
            protected DamageSourceBase()
            {
            }

            public abstract float GetDamage(Obj_AI_Base source, Obj_AI_Base target);
        }

        public class DamageSourceBoundle : Damage.DamageSourceBase
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Func<Obj_AI_Base, bool> <Condition>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private List<Damage.DamageSourceExpression> <DamageSourceExpresions>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private List<Damage.DamageSourceBase> <DamageSources>k__BackingField;

            public DamageSourceBoundle()
            {
                this.DamageSources = new List<Damage.DamageSourceBase>();
                this.DamageSourceExpresions = new List<Damage.DamageSourceExpression>();
            }

            public void Add(Damage.DamageSourceBase damageSource)
            {
                this.DamageSources.Add(damageSource);
            }

            public void AddExpresion(Damage.DamageSourceExpression damageSource)
            {
                this.DamageSourceExpresions.Add(damageSource);
            }

            public float GetDamage(Obj_AI_Base target) => 
                this.GetDamage(Player.Instance, target);

            public override float GetDamage(Obj_AI_Base source, Obj_AI_Base target)
            {
                if ((this.Condition == null) ? false : !this.Condition(target))
                {
                    return 0f;
                }
                float baseDamage = this.DamageSources.Sum<Damage.DamageSourceBase>((Func<Damage.DamageSourceBase, float>) (o => o.GetDamage(source, target)));
                return ((this.DamageSourceExpresions.Count == 0) ? baseDamage : this.DamageSourceExpresions.Sum<Damage.DamageSourceExpression>(((Func<Damage.DamageSourceExpression, float>) (x => x.GetDamage(source, target, baseDamage)))));
            }

            public void Remove(Damage.DamageSourceBase damageSource)
            {
                this.DamageSources.RemoveAll(o => o == damageSource);
            }

            public void RemoveExpresion(Damage.DamageSourceExpression damageSource)
            {
                this.DamageSourceExpresions.RemoveAll(o => o == damageSource);
            }

            public Func<Obj_AI_Base, bool> Condition { get; set; }

            public List<Damage.DamageSourceExpression> DamageSourceExpresions { get; protected set; }

            public List<Damage.DamageSourceBase> DamageSources { get; protected set; }
        }

        public abstract class DamageSourceExpression
        {
            protected DamageSourceExpression()
            {
            }

            public abstract float GetDamage(Obj_AI_Base source, Obj_AI_Base target, float baseDamage);
        }

        internal class ExpresionBasicVarible : Damage.IVariable
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <Key>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private System.Func<Obj_AI_Base, Obj_AI_Base, float> <Value>k__BackingField;

            public ExpresionBasicVarible(string key, System.Func<Obj_AI_Base, Obj_AI_Base, float> value)
            {
                this.Key = key;
                this.Value = value;
            }

            public float GetValue(Obj_AI_Base source, Obj_AI_Base target) => 
                this.Value(source, target);

            public string Key { get; set; }

            public System.Func<Obj_AI_Base, Obj_AI_Base, float> Value { get; set; }
        }

        internal class ExpresionDamageSource : Damage.DamageSourceExpression
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <Condition>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private float[] <DamagePercentages>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.DamageType <DamageType>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <Expression>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private EloBuddy.SDK.Damage.ExpressionCalculator <ExpressionCalculator>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private float <RequriedValue>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private SpellSlot <Slot>k__BackingField;

            public ExpresionDamageSource(string expression, SpellSlot slot, EloBuddy.DamageType damageType)
            {
                this.ExpressionCalculator = new EloBuddy.SDK.Damage.ExpressionCalculator();
                this.Expression = expression;
                this.Slot = slot;
                this.DamageType = damageType;
            }

            public bool CheckCondition(string condition, Obj_AI_Base source, Obj_AI_Base target)
            {
                if (!string.IsNullOrEmpty(condition))
                {
                    char[] separator = new char[] { ' ' };
                    string[] strArray = this.ExpressionCalculator.SetVaribles(condition, source, target, null).Split(separator);
                    if (strArray.Length != 3)
                    {
                        return true;
                    }
                    switch (strArray[1])
                    {
                        case ">":
                            return (float.Parse(strArray[0]) > float.Parse(strArray[2]));

                        case "<":
                            return (float.Parse(strArray[0]) < float.Parse(strArray[2]));

                        case "==":
                            return (Math.Abs((float) (float.Parse(strArray[0]) - float.Parse(strArray[2]))) < float.Epsilon);

                        case ">=":
                            return (float.Parse(strArray[0]) >= float.Parse(strArray[2]));

                        case "<=":
                            return (float.Parse(strArray[0]) <= float.Parse(strArray[2]));
                    }
                }
                return true;
            }

            private static Damage.IVariable[] GetCustomVaribales(float baseDamage) => 
                new Damage.IVariable[] { new Damage.ExpresionBasicVarible("BaseDamage", (source, target) => baseDamage) };

            public override float GetDamage(Obj_AI_Base source, Obj_AI_Base target, float baseDamage)
            {
                SpellDataInst spell = source.Spellbook.GetSpell(this.Slot);
                if ((spell == null) || (spell.Level == 0))
                {
                    return 0f;
                }
                return (this.CheckCondition(this.Condition, source, target) ? source.CalculateDamageOnUnit(target, this.DamageType, this.ExpressionCalculator.Calculate(this.Expression, source, target, GetCustomVaribales(baseDamage)), true, false) : baseDamage);
            }

            public string Condition { get; set; }

            public float[] DamagePercentages { get; set; }

            public EloBuddy.DamageType DamageType { get; set; }

            public string Expression { get; set; }

            public EloBuddy.SDK.Damage.ExpressionCalculator ExpressionCalculator { get; set; }

            public float RequriedValue { get; set; }

            public SpellSlot Slot { get; set; }

            public IEnumerable<Damage.IVariable> Variables
            {
                set
                {
                    this.ExpressionCalculator.Variables = value.ToList<Damage.IVariable>();
                }
            }
        }

        internal class ExpresionLevelVarible : Damage.IVariable
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private float[] <Damages>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <Key>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private SpellSlot <Slot>k__BackingField;

            public ExpresionLevelVarible(string key, SpellSlot slot, float[] damages)
            {
                this.Key = key;
                this.Slot = slot;
                this.Damages = damages;
            }

            public float GetValue(Obj_AI_Base source, Obj_AI_Base target)
            {
                SpellDataInst spell = source.Spellbook.GetSpell(this.Slot);
                if (spell != null)
                {
                    int index = spell.Level - 1;
                    if ((index > 0) && (index <= this.Damages.Length))
                    {
                        return this.Damages[index];
                    }
                }
                return 0f;
            }

            public float[] Damages { get; set; }

            public string Key { get; set; }

            public SpellSlot Slot { get; set; }
        }

        internal class ExpresionStaticVarible : Damage.IVariable
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <Key>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Damage.ScalingTarget <Target>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Damage.ScalingType <Type>k__BackingField;

            public ExpresionStaticVarible(string key, Damage.ScalingTarget target, Damage.ScalingType type)
            {
                this.Key = key;
                this.Target = target;
                this.Type = type;
            }

            public float GetValue(Obj_AI_Base source, Obj_AI_Base target)
            {
                Obj_AI_Base base2 = (this.Target == Damage.ScalingTarget.Source) ? source : target;
                switch (this.Type)
                {
                    case Damage.ScalingType.AbilityPoints:
                        return base2.FlatMagicDamageMod;

                    case Damage.ScalingType.AttackPoints:
                        return base2.TotalAttackDamage;

                    case Damage.ScalingType.BonusAbilityPoints:
                        return base2.FlatMagicDamageMod;

                    case Damage.ScalingType.BonusAttackPoints:
                        return base2.FlatPhysicalDamageMod;

                    case Damage.ScalingType.Armor:
                        return (base2.Armor + base2.FlatArmorMod);

                    case Damage.ScalingType.MagicResist:
                        return base2.SpellBlock;

                    case Damage.ScalingType.CurrentHealth:
                        return base2.Health;

                    case Damage.ScalingType.CurrentMana:
                        return base2.Mana;

                    case Damage.ScalingType.MaxHealth:
                        return base2.MaxHealth;

                    case Damage.ScalingType.MaxMana:
                        return base2.MaxMana;
                }
                return 0f;
            }

            public string Key { get; set; }

            public Damage.ScalingTarget Target { get; set; }

            public Damage.ScalingType Type { get; set; }
        }

        internal class ExpresionTypeVarible : Damage.IVariable
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <Key>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private MethodInfo <Method>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private ParameterInfo[] <MethodParameters>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <Name>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private System.Type <ObjectType>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string[] <Parameters>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private PropertyInfo <Property>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Damage.ScalingTarget <Target>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private DamageType <Type>k__BackingField;

            public ExpresionTypeVarible(string key, DamageType type, Damage.ScalingTarget target, string name, params string[] param)
            {
                this.Key = key;
                this.Type = type;
                this.Target = target;
                this.Name = name;
                this.Parameters = param;
                this.ObjectType = typeof(Obj_AI_Base);
                if (this.IsMethod)
                {
                    this.Method = this.ObjectType.GetMethod(name);
                    if (this.Method != null)
                    {
                        this.MethodParameters = this.Method.GetParameters();
                        return;
                    }
                }
                else
                {
                    this.Property = this.ObjectType.GetProperty(name);
                    if (this.Property != null)
                    {
                        return;
                    }
                }
                throw new ArgumentException((this.IsMethod ? "Method" : "Property") + " (" + name + ") not found!");
            }

            public float GetValue(Obj_AI_Base source, Obj_AI_Base target)
            {
                Obj_AI_Base base2 = (this.Target == Damage.ScalingTarget.Source) ? source : target;
                if (this.IsMethod && (this.Method != null))
                {
                    object[] parameters = new object[this.MethodParameters.Length];
                    for (int i = 0; i < this.MethodParameters.Length; i++)
                    {
                        if ((i > (this.Parameters.Length - 1)) && this.MethodParameters[i].HasDefaultValue)
                        {
                            parameters[i] = this.MethodParameters[i].DefaultValue;
                        }
                        else
                        {
                            if (!this.MethodParameters[i].HasDefaultValue && (this.Parameters.Length < i))
                            {
                                object[] objArray1 = new object[] { "Not Enough Arguments For Method Expected: ", this.MethodParameters.Length, ",  Collected: ", this.Parameters.Length };
                                throw new ArgumentException(string.Concat(objArray1));
                            }
                            if (this.MethodParameters[i].ParameterType == typeof(float))
                            {
                                parameters[i] = float.Parse(this.Parameters[i]);
                            }
                            else if (this.MethodParameters[i].ParameterType == typeof(int))
                            {
                                parameters[i] = int.Parse(this.Parameters[i]);
                            }
                            else if (this.MethodParameters[i].ParameterType == typeof(string))
                            {
                                parameters[i] = this.Parameters[i];
                            }
                        }
                    }
                    object obj2 = this.Method.Invoke(base2, parameters);
                    if (this.Method.ReturnType == typeof(float))
                    {
                        return (float) obj2;
                    }
                    if (this.Method.ReturnType == typeof(int))
                    {
                        return (float) ((int) obj2);
                    }
                }
                else
                {
                    return (float) this.Property.GetValue(base2);
                }
                return 0f;
            }

            public bool IsMethod =>
                (this.Parameters.Length > 0);

            public string Key { get; set; }

            private MethodInfo Method { get; set; }

            private ParameterInfo[] MethodParameters { get; set; }

            public string Name { get; set; }

            private System.Type ObjectType { get; set; }

            public string[] Parameters { get; set; }

            private PropertyInfo Property { get; set; }

            public Damage.ScalingTarget Target { get; set; }

            public DamageType Type { get; set; }
        }

        public class ExpressionCalculator
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private System.Data.DataTable <DataTable>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private List<Damage.IVariable> <Variables>k__BackingField;

            public ExpressionCalculator()
            {
                this.DataTable = new System.Data.DataTable();
                this.Variables = new List<Damage.IVariable>();
            }

            public float Calculate(string expresion, Obj_AI_Base source, Obj_AI_Base target, Damage.IVariable[] customVariables = null)
            {
                expresion = this.SetVaribles(expresion, source, target, customVariables);
                try
                {
                    return float.Parse(this.DataTable.Compute(expresion, null).ToString());
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    return 0f;
                }
            }

            public string SetVaribles(string expresion, Obj_AI_Base source, Obj_AI_Base target, Damage.IVariable[] customVariables)
            {
                expresion = this.Variables.Aggregate<Damage.IVariable, string>(expresion, (current, variable) => current.Replace("{" + variable.Key + "}", variable.GetValue(source, target).ToString("F")));
                if (customVariables > null)
                {
                    expresion = customVariables.Aggregate<Damage.IVariable, string>(expresion, (current, variable) => current.Replace("{" + variable.Key + "}", variable.GetValue(source, target).ToString("F")));
                }
                return expresion;
            }

            private System.Data.DataTable DataTable { get; set; }

            public List<Damage.IVariable> Variables { get; set; }
        }

        public interface IVariable
        {
            float GetValue(Obj_AI_Base source, Obj_AI_Base target);

            string Key { get; }
        }

        public enum ScalingTarget
        {
            Source,
            Target
        }

        public enum ScalingType
        {
            AbilityPoints,
            AttackPoints,
            BonusAbilityPoints,
            BonusAttackPoints,
            Armor,
            MagicResist,
            CurrentHealth,
            CurrentMana,
            MaxHealth,
            MaxMana,
            MissingHealth
        }
    }
}

