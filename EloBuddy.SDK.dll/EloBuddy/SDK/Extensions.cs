namespace EloBuddy.SDK
{
    using EloBuddy;
    using EloBuddy.SDK.Constants;
    using EloBuddy.SDK.Rendering;
    using mscorlib;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class Extensions
    {
        private static readonly HashSet<BuffType> BlockedMovementBuffTypes;
        private static readonly List<string> CloneList;
        private static readonly List<string> PetList;
        private static readonly HashSet<BuffType> ReducedMovementBuffTypes;

        static Extensions()
        {
            HashSet<BuffType> set1 = new HashSet<BuffType> {
                BuffType.Knockup,
                BuffType.Knockback,
                BuffType.Charm,
                BuffType.Fear,
                BuffType.Flee,
                BuffType.Taunt,
                BuffType.Snare,
                BuffType.Stun,
                BuffType.Suppression,
                BuffType.Grounded
            };
            BlockedMovementBuffTypes = set1;
            HashSet<BuffType> set2 = new HashSet<BuffType> {
                BuffType.Slow,
                BuffType.Polymorph
            };
            ReducedMovementBuffTypes = set2;
            List<string> list1 = new List<string> { 
                "annietibbers",
                "elisespiderling",
                "heimertyellow",
                "heimertblue",
                "malzaharvoidling",
                "shacobox",
                "yorickspectralghoul",
                "yorickdecayedghoul",
                "yorickravenousghoul",
                "zyrathornplant",
                "zyragraspingplant"
            };
            PetList = list1;
            List<string> list2 = new List<string> { 
                "leblanc",
                "shaco",
                "monkeyking"
            };
            CloneList = list2;
        }

        public static SharpDX.Rectangle Add(this SharpDX.Rectangle rectangle1, SharpDX.Rectangle rectangle2) => 
            new SharpDX.Rectangle(rectangle1.X + rectangle2.X, rectangle1.Y + rectangle2.Y, rectangle1.Width + rectangle2.Width, rectangle1.Height + rectangle2.Height);

        public static float AngleBetween(this Vector3 vector3, Vector3 toVector3)
        {
            double num = Math.Sqrt((double) (((vector3.X * vector3.X) + (vector3.Y * vector3.Y)) + (vector3.Z * vector3.Z)));
            double num2 = Math.Sqrt((double) (((toVector3.X * toVector3.X) + (toVector3.Y * toVector3.Y)) + (toVector3.Z * toVector3.Z)));
            float num3 = ((vector3.X * toVector3.X) + (vector3.Y * toVector3.Y)) + (vector3.Z + toVector3.Z);
            return (float) Math.Acos((((double) num3) / num) * num2);
        }

        public static Vector2[] CirclesIntersection(this Geometry.Polygon.Circle circle1, Geometry.Polygon.Circle circle2) => 
            circle1.Center.CirclesIntersection(circle1.Radius, circle2.Center, circle2.Radius);

        public static Vector2[] CirclesIntersection(this Vector2 center1, float radius1, Vector2 center2, float radius2)
        {
            float num = center1.Distance(center2, false);
            if ((num > (radius1 + radius2)) || (num <= Math.Abs((float) (radius1 - radius2))))
            {
                return new Vector2[0];
            }
            float num2 = (((radius1 * radius1) - (radius2 * radius2)) + (num * num)) / (2f * num);
            float num3 = (float) Math.Sqrt((double) ((radius1 * radius1) - (num2 * num2)));
            Vector2 v = (center2 - center1).Normalized();
            Vector2 vector2 = center1 + ((Vector2) (num2 * v));
            Vector2 vector3 = vector2 + ((Vector2) (num3 * v.Perpendicular()));
            Vector2 vector4 = vector2 - ((Vector2) (num3 * v.Perpendicular()));
            return new Vector2[] { vector3, vector4 };
        }

        public static Vector2[] CirclesIntersection(this Vector3 center1, float radius1, Vector3 center2, float radius2)
        {
            Vector2 vector = center1.To2D();
            Vector2 vector2 = center2.To2D();
            return vector.CirclesIntersection(radius1, vector2, radius2);
        }

        [Obsolete("Use CountAllyChampionsInRange")]
        public static int CountAlliesInRange(this GameObject target, float range)
        {
            Obj_AI_Base base2 = target as Obj_AI_Base;
            return ((base2 != null) ? base2.ServerPosition : target.Position).CountAlliesInRange(range);
        }

        [Obsolete("Use CountAllyChampionsInRange")]
        public static int CountAlliesInRange(this Vector2 position, float range)
        {
            float rangeSqr = range.Pow();
            return EntityManager.Heroes.Allies.Count<AIHeroClient>(o => (o.IsValidTarget(null, false, null) && (((Obj_AI_Base) o).Distance(position, true) < rangeSqr)));
        }

        [Obsolete("Use CountAllyChampionsInRange")]
        public static int CountAlliesInRange(this Vector3 position, float range) => 
            position.To2D().CountAlliesInRange(range);

        public static int CountAllyChampionsInRange(this GameObject target, float range)
        {
            Obj_AI_Base base2 = target as Obj_AI_Base;
            return ((base2 != null) ? base2.ServerPosition : target.Position).CountAllyChampionsInRange(range);
        }

        public static int CountAllyChampionsInRange(this Vector2 position, float range) => 
            EntityManager.Heroes.Allies.Count<AIHeroClient>(o => (o.IsValidTarget(null, false, null) && ((Obj_AI_Base) o).IsInRange(position, range)));

        public static int CountAllyChampionsInRange(this Vector3 position, float range) => 
            position.To2D().CountAllyChampionsInRange(range);

        public static int CountAllyChampionsInRangeWithPrediction(this GameObject target, int range, int delay = 250) => 
            EntityManager.Heroes.Allies.Count<AIHeroClient>(e => (e.IsValidTarget(null, false, null) && Prediction.Position.PredictUnitPosition(e, delay).IsInRange(target, ((float) range))));

        public static int CountAllyChampionsInRangeWithPrediction(this Vector2 position, int range, int delay = 250) => 
            EntityManager.Heroes.Allies.Count<AIHeroClient>(e => (e.IsValidTarget(null, false, null) && Prediction.Position.PredictUnitPosition(e, delay).IsInRange(position, ((float) range))));

        public static int CountAllyChampionsInRangeWithPrediction(this Vector3 position, int range, int delay = 250) => 
            EntityManager.Heroes.Allies.Count<AIHeroClient>(e => (e.IsValidTarget(null, false, null) && Prediction.Position.PredictUnitPosition(e, delay).IsInRange(position, ((float) range))));

        public static int CountAllyMinionsInRange(this GameObject target, float range)
        {
            Obj_AI_Base base2 = target as Obj_AI_Base;
            return ((base2 != null) ? base2.ServerPosition : target.Position).CountAllyMinionsInRange(range);
        }

        public static int CountAllyMinionsInRange(this Vector2 position, float range) => 
            EntityManager.MinionsAndMonsters.AlliedMinions.Count<Obj_AI_Minion>(o => (o.IsValidTarget(null, false, null) && ((Obj_AI_Base) o).IsInRange(position, range)));

        public static int CountAllyMinionsInRange(this Vector3 position, float range) => 
            position.To2D().CountAllyMinionsInRange(range);

        public static int CountAllyMinionsInRangeWithPrediction(this GameObject target, int range, int delay = 250) => 
            EntityManager.MinionsAndMonsters.AlliedMinions.Count<Obj_AI_Minion>(e => (e.IsValidTarget(null, false, null) && Prediction.Position.PredictUnitPosition(e, delay).IsInRange(target.Position, ((float) range))));

        public static int CountAllyMinionsInRangeWithPrediction(this Vector2 position, int range, int delay = 250) => 
            EntityManager.MinionsAndMonsters.AlliedMinions.Count<Obj_AI_Minion>(e => (e.IsValidTarget(null, false, null) && Prediction.Position.PredictUnitPosition(e, delay).IsInRange(position, ((float) range))));

        public static int CountAllyMinionsInRangeWithPrediction(this Vector3 position, int range, int delay = 250) => 
            EntityManager.MinionsAndMonsters.AlliedMinions.Count<Obj_AI_Minion>(e => (e.IsValidTarget(null, false, null) && Prediction.Position.PredictUnitPosition(e, delay).IsInRange(position, ((float) range))));

        [Obsolete("Use CountEnemyChampionsInRange")]
        public static int CountEnemiesInRange(this GameObject target, float range)
        {
            Obj_AI_Base base2 = target as Obj_AI_Base;
            return ((base2 != null) ? base2.ServerPosition : target.Position).CountEnemiesInRange(range);
        }

        [Obsolete("Use CountEnemyChampionsInRange")]
        public static int CountEnemiesInRange(this Vector2 position, float range)
        {
            float rangeSqr = range.Pow();
            return EntityManager.Heroes.Enemies.Count<AIHeroClient>(o => (o.IsValidTarget(null, false, null) && (((Obj_AI_Base) o).Distance(position, true) < rangeSqr)));
        }

        [Obsolete("Use CountEnemyChampionsInRange")]
        public static int CountEnemiesInRange(this Vector3 position, float range) => 
            position.To2D().CountEnemiesInRange(range);

        [Obsolete("Use CountAllyChampionsInRangeWithPrediction")]
        public static int CountEnemyAlliesInRangeWithPrediction(this GameObject target, int range, int delay = 250) => 
            EntityManager.Heroes.Allies.Count<AIHeroClient>(e => (e.IsValidTarget(null, false, null) && Prediction.Position.PredictUnitPosition(e, delay).IsInRange(target, ((float) range))));

        [Obsolete("Use CountAllyChampionsInRangeWithPrediction")]
        public static int CountEnemyAlliesInRangeWithPrediction(this Vector2 position, int range, int delay = 250) => 
            EntityManager.Heroes.Allies.Count<AIHeroClient>(e => (e.IsValidTarget(null, false, null) && Prediction.Position.PredictUnitPosition(e, delay).IsInRange(position, ((float) range))));

        [Obsolete("Use CountAllyChampionsInRangeWithPrediction")]
        public static int CountEnemyAlliesInRangeWithPrediction(this Vector3 position, int range, int delay = 250) => 
            EntityManager.Heroes.Allies.Count<AIHeroClient>(e => (e.IsValidTarget(null, false, null) && Prediction.Position.PredictUnitPosition(e, delay).IsInRange(position, ((float) range))));

        public static int CountEnemyChampionsInRange(this GameObject target, float range)
        {
            Obj_AI_Base base2 = target as Obj_AI_Base;
            return ((base2 != null) ? base2.ServerPosition : target.Position).CountEnemyChampionsInRange(range);
        }

        public static int CountEnemyChampionsInRange(this Vector2 position, float range) => 
            EntityManager.Heroes.Enemies.Count<AIHeroClient>(o => (o.IsValidTarget(null, false, null) && ((Obj_AI_Base) o).IsInRange(position, range)));

        public static int CountEnemyChampionsInRange(this Vector3 position, float range) => 
            position.To2D().CountEnemyChampionsInRange(range);

        public static int CountEnemyHeroesInRangeWithPrediction(this GameObject target, int range, int delay = 250) => 
            EntityManager.Heroes.Enemies.Count<AIHeroClient>(e => (e.IsValidTarget(null, false, null) && Prediction.Position.PredictUnitPosition(e, delay).IsInRange(target, ((float) range))));

        public static int CountEnemyHeroesInRangeWithPrediction(this Vector2 position, int range, int delay = 250) => 
            EntityManager.Heroes.Enemies.Count<AIHeroClient>(e => (e.IsValidTarget(null, false, null) && Prediction.Position.PredictUnitPosition(e, delay).IsInRange(position, ((float) range))));

        public static int CountEnemyHeroesInRangeWithPrediction(this Vector3 position, int range, int delay = 250) => 
            EntityManager.Heroes.Enemies.Count<AIHeroClient>(e => (e.IsValidTarget(null, false, null) && Prediction.Position.PredictUnitPosition(e, delay).IsInRange(position, ((float) range))));

        public static int CountEnemyMinionsInRange(this GameObject target, float range)
        {
            Obj_AI_Base base2 = target as Obj_AI_Base;
            return ((base2 != null) ? base2.ServerPosition : target.Position).CountEnemyMinionsInRange(range);
        }

        public static int CountEnemyMinionsInRange(this Vector2 position, float range) => 
            EntityManager.MinionsAndMonsters.EnemyMinions.Count<Obj_AI_Minion>(o => (o.IsValidTarget(null, false, null) && ((Obj_AI_Base) o).IsInRange(position, range)));

        public static int CountEnemyMinionsInRange(this Vector3 position, float range) => 
            position.To2D().CountEnemyMinionsInRange(range);

        public static int CountEnemyMinionsInRangeWithPrediction(this GameObject target, int range, int delay = 250) => 
            EntityManager.MinionsAndMonsters.EnemyMinions.Count<Obj_AI_Minion>(e => (e.IsValidTarget(null, false, null) && Prediction.Position.PredictUnitPosition(e, delay).IsInRange(target.Position, ((float) range))));

        public static int CountEnemyMinionsInRangeWithPrediction(this Vector2 position, int range, int delay = 250) => 
            EntityManager.MinionsAndMonsters.EnemyMinions.Count<Obj_AI_Minion>(e => (e.IsValidTarget(null, false, null) && Prediction.Position.PredictUnitPosition(e, delay).IsInRange(position, ((float) range))));

        public static int CountEnemyMinionsInRangeWithPrediction(this Vector3 position, int range, int delay = 250) => 
            EntityManager.MinionsAndMonsters.EnemyMinions.Count<Obj_AI_Minion>(e => (e.IsValidTarget(null, false, null) && Prediction.Position.PredictUnitPosition(e, delay).IsInRange(position, ((float) range))));

        public static List<Vector2> CutPath(this List<Vector2> path, float distance)
        {
            List<Vector2> list1;
            System.Boolean ReflectorVariable0;
            List<Vector2> list = new List<Vector2>();
            float num = distance;
            if (distance < 0f)
            {
                path[0] += (Vector2) (distance * (path[1] - path[0]).Normalized());
                return path;
            }
            for (int i = 0; i < (path.Count - 1); i++)
            {
                float num3 = path[i].Distance(path[i + 1], false);
                if (num3 > num)
                {
                    list.Add(path[i] + ((Vector2) (num * (path[i + 1] - path[i]).Normalized())));
                    for (int j = i + 1; j < path.Count; j++)
                    {
                        list.Add(path[j]);
                    }
                    break;
                }
                num -= num3;
            }
            if (list.Count <= 0)
            {
                list1 = new List<Vector2> {
                    path.Last<Vector2>()
                };
                ReflectorVariable0 = true;
            }
            else
            {
                ReflectorVariable0 = false;
            }
            return (ReflectorVariable0 ? list1 : list);
        }

        public static Vector2 Direction(this Obj_AI_Base source) => 
            source.Direction.To2D().Normalized();

        public static float Distance(this GameObject target1, GameObject target2, bool squared = false) => 
            target1.Position.To2D().Distance(target2.Position.To2D(), squared);

        public static float Distance(this GameObject target1, Obj_AI_Base target2, bool squared = false) => 
            target1.Position.To2D().Distance(target2.ServerPosition.To2D(), squared);

        public static float Distance(this GameObject target, Vector2 pos, bool squared = false) => 
            target.Position.To2D().Distance(pos, squared);

        public static float Distance(this GameObject target, Vector3 pos, bool squared = false) => 
            target.Position.To2D().Distance(pos.To2D(), squared);

        public static float Distance(this Obj_AI_Base target1, GameObject target2, bool squared = false) => 
            target1.ServerPosition.To2D().Distance(target2.Position.To2D(), squared);

        public static float Distance(this Obj_AI_Base target1, Obj_AI_Base target2, bool squared = false) => 
            target1.ServerPosition.To2D().Distance(target2.ServerPosition.To2D(), squared);

        public static float Distance(this Obj_AI_Base target, Vector2 pos, bool squared = false) => 
            target.ServerPosition.To2D().Distance(pos, squared);

        public static float Distance(this Obj_AI_Base target, Vector3 pos, bool squared = false) => 
            target.ServerPosition.To2D().Distance(pos.To2D(), squared);

        public static float Distance(this Vector2 pos, GameObject target, bool squared = false) => 
            pos.Distance(target.Position.To2D(), squared);

        public static float Distance(this Vector2 pos, Obj_AI_Base target, bool squared = false) => 
            pos.Distance(target.ServerPosition.To2D(), squared);

        public static float Distance(this Vector2 pos1, Vector2 pos2, bool squared = false)
        {
            if (squared)
            {
                return Vector2.DistanceSquared(pos1, pos2);
            }
            return Vector2.Distance(pos1, pos2);
        }

        public static float Distance(this Vector2 pos1, Vector3 pos2, bool squared = false) => 
            pos1.Distance(pos2.To2D(), squared);

        public static float Distance(this Vector3 pos, GameObject target, bool squared = false) => 
            pos.To2D().Distance(target.Position.To2D(), squared);

        public static float Distance(this Vector3 pos, Obj_AI_Base target, bool squared = false) => 
            pos.To2D().Distance(target.ServerPosition.To2D(), squared);

        public static float Distance(this Vector3 pos1, Vector2 pos2, bool squared = false) => 
            pos1.To2D().Distance(pos2, squared);

        public static float Distance(this Vector3 pos1, Vector3 pos2, bool squared = false) => 
            pos1.To2D().Distance(pos2.To2D(), squared);

        public static float Distance(this Vector2 point, Vector2 segmentStart, Vector2 segmentEnd, bool squared = false)
        {
            float number = Math.Abs((float) (((((segmentEnd.Y - segmentStart.Y) * point.X) - ((segmentEnd.X - segmentStart.X) * point.Y)) + (segmentEnd.X * segmentStart.Y)) - (segmentEnd.Y * segmentStart.X)));
            return ((squared ? number.Pow() : number) / segmentStart.Distance(segmentEnd, squared));
        }

        public static float Distance(this Vector3 point, Vector2 segmentStart, Vector2 segmentEnd, bool squared = false) => 
            point.To2D().Distance(segmentStart, segmentEnd, squared);

        public static float DistanceSquared(this Vector2 pos1, Vector2 pos2) => 
            Vector2.DistanceSquared(pos1, pos2);

        public static float DistanceSquared(this Vector2 pos1, Vector3 pos2) => 
            pos1.DistanceSquared(pos2.To2D());

        public static float DistanceSquared(this Vector3 pos1, Vector2 pos2) => 
            pos1.To2D().DistanceSquared(pos2);

        public static float DistanceSquared(this Vector3 pos1, Vector3 pos2) => 
            pos1.To2D().DistanceSquared(pos2.To2D());

        public static SharpDX.Rectangle Divide(this SharpDX.Rectangle rectangle1, SharpDX.Rectangle rectangle2) => 
            new SharpDX.Rectangle(rectangle1.X / rectangle2.X, rectangle1.Y / rectangle2.Y, rectangle1.Width / rectangle2.Width, rectangle1.Height / rectangle2.Height);

        public static void DrawArrow(this Geometry.Polygon.Line line, System.Drawing.Color color, int width = 3)
        {
            Vector2 lineStart = line.LineStart;
            Vector2 lineEnd = line.LineEnd;
            lineStart.DrawArrow(lineEnd, color, width);
        }

        public static void DrawArrow(this Vector2 start, Vector2 end, System.Drawing.Color color, int width = 3)
        {
            Vector2 vector = end + ((Vector2) ((start - end).Rotated(0.7853982f) / 5f));
            Vector2 vector2 = end + ((Vector2) ((start - end).Rotated(-0.7853982f) / 5f));
            new Geometry.Polygon.Line(start.To3D(0), end.To3D(0), -1f).Draw(color, width);
            new Geometry.Polygon.Line(end.To3D(0), vector.To3D(0), -1f).Draw(color, width);
            new Geometry.Polygon.Line(end.To3D(0), vector2.To3D(0), -1f).Draw(color, width);
        }

        public static void DrawArrow(this Vector3 start, Vector3 end, System.Drawing.Color color, int width = 3)
        {
            Vector2 vector = end.To2D() + ((Vector2) ((start - end).To2D().Rotated(0.7853982f) / 5f));
            Vector2 vector2 = end.To2D() + ((Vector2) ((start - end).To2D().Rotated(-0.7853982f) / 5f));
            new Geometry.Polygon.Line(start, end, -1f).Draw(color, width);
            new Geometry.Polygon.Line(end, vector.To3D(0), -1f).Draw(color, width);
            new Geometry.Polygon.Line(end, vector2.To3D(0), -1f).Draw(color, width);
        }

        public static void DrawCircle(this GameObject target, int radius, SharpDX.Color color, float lineWidth = 3f)
        {
            GameObject[] objects = new GameObject[] { target };
            EloBuddy.SDK.Rendering.Circle.Draw(color, (float) radius, lineWidth, objects);
        }

        public static void DrawCircle(this GameObject target, int radius, System.Drawing.Color color, float lineWidth = 3f)
        {
            GameObject[] objects = new GameObject[] { target };
            EloBuddy.SDK.Rendering.Circle.Draw(color.ToSharpDX(), (float) radius, lineWidth, objects);
        }

        public static void DrawCircle(this Vector2 position, int radius, SharpDX.Color color, float lineWidth = 3f)
        {
            Vector3[] positions = new Vector3[] { position.To3D(0) };
            EloBuddy.SDK.Rendering.Circle.Draw(color, (float) radius, lineWidth, positions);
        }

        public static void DrawCircle(this Vector3 position, int radius, SharpDX.Color color, float lineWidth = 3f)
        {
            Vector3[] positions = new Vector3[] { position };
            EloBuddy.SDK.Rendering.Circle.Draw(color, (float) radius, lineWidth, positions);
        }

        public static Vector2 Extend(this Vector2 source, GameObject target, float range) => 
            source.Extend(target.Position.To2D(), range);

        public static Vector2 Extend(this Vector2 source, Obj_AI_Base target, float range) => 
            source.Extend(target.ServerPosition.To2D(), range);

        public static Vector2 Extend(this Vector2 source, Vector2 target, float range) => 
            (source + ((Vector2) (range * (target - source).Normalized())));

        public static Vector2 Extend(this Vector2 source, Vector3 target, float range) => 
            source.Extend(target.To2D(), range);

        public static Vector2 Extend(this Vector3 source, GameObject target, float range) => 
            source.To2D().Extend(target.Position.To2D(), range);

        public static Vector2 Extend(this Vector3 source, Obj_AI_Base target, float range) => 
            source.To2D().Extend(target.ServerPosition.To2D(), range);

        public static Vector2 Extend(this Vector3 source, Vector2 target, float range) => 
            source.To2D().Extend(target, range);

        public static Vector2 Extend(this Vector3 source, Vector3 target, float range) => 
            source.To2D().Extend(target.To2D(), range);

        public static SpellSlot FindSummonerSpellSlotFromName(this AIHeroClient target, string spellName)
        {
            Func<SpellDataInst, bool> <>9__0;
            foreach (SpellDataInst inst in target.Spellbook.Spells.Where<SpellDataInst>((Func<SpellDataInst, bool>) (<>9__0 ?? (<>9__0 = spell => ((spell.Slot == SpellSlot.Summoner1) || (spell.Slot == SpellSlot.Summoner2)) && spell.Name.ToLower().Contains(spellName.ToLower())))))
            {
                return inst.Slot;
            }
            return SpellSlot.Unknown;
        }

        public static float GetAutoAttackRange(this Obj_AI_Base source, AttackableUnit target = null)
        {
            float num = (source.AttackRange + source.BoundingRadius) + ((target != null) ? (target.BoundingRadius - 35f) : 35f);
            AIHeroClient client = source as AIHeroClient;
            if ((client != null) && (target > null))
            {
                if (client.Hero == Champion.Caitlyn)
                {
                    Obj_AI_Base base2 = target as Obj_AI_Base;
                    if ((base2 != null) && base2.HasBuff("caitlynyordletrapinternal"))
                    {
                        num += 650f;
                    }
                }
                return num;
            }
            if (source is Obj_AI_Turret)
            {
                return (750f + source.BoundingRadius);
            }
            if (source.BaseSkinName == "AzirSoldier")
            {
                num += 275f - source.BoundingRadius;
            }
            return num;
        }

        public static CollisionFlags GetCollisionFlags(this Vector2 vector) => 
            NavMesh.GetCollisionFlags(vector.X, vector.Y);

        public static CollisionFlags GetCollisionFlags(this Vector3 vector) => 
            vector.To2D().GetCollisionFlags();

        public static int GetHighestSpellRange(this AIHeroClient target)
        {
            SpellDataInst inst = target.Spellbook.GetSpell(SpellSlot.Q);
            SpellDataInst inst2 = target.Spellbook.GetSpell(SpellSlot.W);
            SpellDataInst inst3 = target.Spellbook.GetSpell(SpellSlot.E);
            SpellDataInst inst4 = target.Spellbook.GetSpell(SpellSlot.R);
            if ((((inst != null) && (inst2 != null)) && (inst3 != null)) && (inst4 != null))
            {
                List<SpellDataInst> list = new List<SpellDataInst> {
                    inst,
                    inst2,
                    inst3,
                    inst4
                };
                SpellDataInst inst5 = (from spell in list
                    orderby (spell.SData.CastRangeDisplayOverride > 0f) ? ((IEnumerable<SpellDataInst>) spell.SData.CastRangeDisplayOverride) : ((IEnumerable<SpellDataInst>) spell.SData.CastRange) descending
                    select spell).FirstOrDefault<SpellDataInst>();
                if (inst5 > null)
                {
                    return ((inst5.SData.CastRangeDisplayOverride > 0f) ? ((int) inst5.SData.CastRangeDisplayOverride) : ((int) inst5.SData.CastRange));
                }
            }
            return 0;
        }

        public static float GetItemsFlatArmorPenetrationMod(this Obj_AI_Base target) => 
            target.InventoryItems.Sum<InventorySlot>(((Func<InventorySlot, float>) (i => new Item(i.Id, 0f).ItemInfo.Stats.rFlatArmorPenetrationMod)));

        public static float GetItemsFlatMagicPenetrationMod(this Obj_AI_Base target) => 
            target.InventoryItems.Sum<InventorySlot>(((Func<InventorySlot, float>) (i => new Item(i.Id, 0f).ItemInfo.Stats.rFlatMagicPenetrationMod)));

        public static InventorySlot GetItemSlot(this Obj_AI_Base target, params ItemId[] itemIds) => 
            target.InventoryItems.GetItemSlot(itemIds);

        public static InventorySlot GetItemSlot(this Obj_AI_Base target, params int[] itemIds) => 
            target.InventoryItems.GetItemSlot(itemIds);

        public static InventorySlot GetItemSlot(this IEnumerable<InventorySlot> inventory, params ItemId[] items) => 
            inventory.FirstOrDefault<InventorySlot>(itemId => items.Contains<ItemId>(itemId.Id));

        public static InventorySlot GetItemSlot(this IEnumerable<InventorySlot> inventory, params int[] itemIds) => 
            inventory.FirstOrDefault<InventorySlot>(itemId => itemIds.Contains<int>(((int) itemId.Id)));

        public static float GetItemsPercentArmorPenetrationMod(this Obj_AI_Base target) => 
            target.InventoryItems.Sum<InventorySlot>(((Func<InventorySlot, float>) (i => new Item(i.Id, 0f).ItemInfo.Stats.rPercentArmorPenetrationMod)));

        public static int GetLowestSpellRange(this AIHeroClient target)
        {
            SpellDataInst inst = target.Spellbook.GetSpell(SpellSlot.Q);
            SpellDataInst inst2 = target.Spellbook.GetSpell(SpellSlot.W);
            SpellDataInst inst3 = target.Spellbook.GetSpell(SpellSlot.E);
            SpellDataInst inst4 = target.Spellbook.GetSpell(SpellSlot.R);
            if ((((inst != null) && (inst2 != null)) && (inst3 != null)) && (inst4 != null))
            {
                List<SpellDataInst> list = new List<SpellDataInst> {
                    inst,
                    inst2,
                    inst3,
                    inst4
                };
                SpellDataInst inst5 = (from spell in list
                    orderby (spell.SData.CastRangeDisplayOverride > 0f) ? ((IEnumerable<SpellDataInst>) spell.SData.CastRangeDisplayOverride) : ((IEnumerable<SpellDataInst>) spell.SData.CastRange)
                    select spell).FirstOrDefault<SpellDataInst>();
                if (inst5 > null)
                {
                    return ((inst5.SData.CastRangeDisplayOverride > 0f) ? ((int) inst5.SData.CastRangeDisplayOverride) : ((int) inst5.SData.CastRange));
                }
            }
            return 0;
        }

        public static Vector3 GetMissileFixedYPosition(this MissileClient target)
        {
            Vector3 position = target.Position;
            return new Vector3(position.X, position.Y, position.Z - 100f);
        }

        public static float GetMovementBlockedDebuffDuration(this Obj_AI_Base target) => 
            ((from b in target.Buffs
                where (b.IsActive && (Game.Time < b.EndTime)) && BlockedMovementBuffTypes.Contains(b.Type)
                select b).Aggregate<BuffInstance, float>(0f, (current, buff) => Math.Max(current, buff.EndTime)) - Game.Time);

        [Obsolete("GetMovementDebuffDuration is deprecated, please use GetMovementBlockedDebuffDuration instead.")]
        public static float GetMovementDebuffDuration(this Obj_AI_Base target) => 
            ((from b in target.Buffs
                where (b.IsActive && (Game.Time < b.EndTime)) && BlockedMovementBuffTypes.Contains(b.Type)
                select b).Aggregate<BuffInstance, float>(0f, (current, buff) => Math.Max(current, buff.EndTime)) - Game.Time);

        public static float GetMovementReducedDebuffDuration(this Obj_AI_Base target) => 
            ((from b in target.Buffs
                where (b.IsActive && (Game.Time < b.EndTime)) && ReducedMovementBuffTypes.Contains(b.Type)
                select b).Aggregate<BuffInstance, float>(0f, (current, buff) => Math.Max(current, buff.EndTime)) - Game.Time);

        public static SpellDataInst GetSpellDataFromName(this AIHeroClient target, string spellName) => 
            target.Spellbook.Spells.FirstOrDefault<SpellDataInst>(spell => string.Equals(spell.Name, spellName, StringComparison.CurrentCultureIgnoreCase));

        public static SpellSlot GetSpellSlotFromName(this AIHeroClient target, string spellName)
        {
            Func<SpellDataInst, bool> <>9__0;
            foreach (SpellDataInst inst in target.Spellbook.Spells.Where<SpellDataInst>((Func<SpellDataInst, bool>) (<>9__0 ?? (<>9__0 = spell => string.Equals(spell.Name, spellName, StringComparison.CurrentCultureIgnoreCase)))))
            {
                return inst.Slot;
            }
            return SpellSlot.Unknown;
        }

        internal static List<Vector2> GetWaypoints(this Obj_AI_Base unit)
        {
            List<Vector2> list = new List<Vector2>();
            if (unit.IsVisible)
            {
                list.Add(unit.ServerPosition.To2D());
                Vector3[] path = unit.Path;
                if (path.Length <= 0)
                {
                    return list;
                }
                Vector2 vector = path[0].To2D();
                if (vector.Distance(list[0], true) > 40f)
                {
                    list.Add(vector);
                }
                for (int i = 1; i < path.Length; i++)
                {
                    list.Add(path[i].To2D());
                }
            }
            return list;
        }

        public static Vector3 GetYasuoWallCollision(this Obj_AI_Base start, Obj_AI_Base end, bool OnlyEnemy = true) => 
            start.ServerPosition.GetYasuoWallCollision(end.ServerPosition, OnlyEnemy);

        public static Vector3 GetYasuoWallCollision(this Obj_AI_Base start, Vector2 end, bool OnlyEnemy = true) => 
            start.ServerPosition.GetYasuoWallCollision(end.To3DWorld(), OnlyEnemy);

        public static Vector3 GetYasuoWallCollision(this Obj_AI_Base start, Vector3 end, bool OnlyEnemy = true) => 
            start.ServerPosition.GetYasuoWallCollision(end, OnlyEnemy);

        public static Vector3 GetYasuoWallCollision(this Vector2 start, Obj_AI_Base end, bool OnlyEnemy = true) => 
            start.To3DWorld().GetYasuoWallCollision(end.ServerPosition, OnlyEnemy);

        public static Vector3 GetYasuoWallCollision(this Vector2 start, Vector2 end, bool OnlyEnemy = true) => 
            start.To3DWorld().GetYasuoWallCollision(end.To3DWorld(), OnlyEnemy);

        public static Vector3 GetYasuoWallCollision(this Vector3 start, Obj_AI_Base end, bool OnlyEnemy = true) => 
            start.GetYasuoWallCollision(end.ServerPosition, OnlyEnemy);

        public static Vector3 GetYasuoWallCollision(this Vector3 start, Vector3 end, bool OnlyEnemy = true) => 
            Prediction.Position.Collision.GetYasuoWallCollision(start, end, OnlyEnemy);

        public static Vector3 GridToWorld(this Vector2 vector) => 
            NavMesh.GridToWorld((short) vector.X, (short) vector.Y);

        public static Vector3 GridToWorld(this Vector3 vector) => 
            vector.To2D().GridToWorld();

        public static bool HasItem(this Obj_AI_Base target, params ItemId[] itemIds) => 
            target.InventoryItems.HasItem(itemIds);

        public static bool HasItem(this Obj_AI_Base target, params int[] itemIds) => 
            target.InventoryItems.HasItem(itemIds);

        public static bool HasItem(this IEnumerable<InventorySlot> inventory, params ItemId[] items) => 
            inventory.Any<InventorySlot>(itemId => items.Contains<ItemId>(itemId.Id));

        public static bool HasItem(this IEnumerable<InventorySlot> inventory, params int[] itemIds) => 
            inventory.Any<InventorySlot>(itemId => itemIds.Contains<int>(((int) itemId.Id)));

        public static bool HasUndyingBuff(this AIHeroClient target, bool addHealthCheck = false)
        {
            switch (target.Hero)
            {
                case Champion.Aatrox:
                    if (!target.HasBuff("aatroxpassivedeath"))
                    {
                        break;
                    }
                    return true;

                case Champion.Fiora:
                    if (!target.HasBuff("FioraW"))
                    {
                        break;
                    }
                    return true;

                case Champion.Tryndamere:
                    if (!(target.HasBuff("UndyingRage") && (!addHealthCheck || (target.Health <= 30f))))
                    {
                        break;
                    }
                    return true;

                case Champion.Vladimir:
                    if (target.HasBuff("VladimirSanguinePool"))
                    {
                        return true;
                    }
                    break;
            }
            return ((EntityManager.Heroes.ContainsKayle && target.HasBuff("JudicatorIntervention")) || (((EntityManager.Heroes.ContainsKindred && target.HasBuff("kindredrnodeathbuff")) && (!addHealthCheck || (target.HealthPercent <= 10f))) || ((EntityManager.Heroes.ContainsZilean && (target.HasBuff("ChronoShift") || target.HasBuff("chronorevive"))) && (!addHealthCheck || (target.HealthPercent <= 10f)))));
        }

        public static bool IdEquals(this GameObject source, GameObject target)
        {
            if ((source == null) || (target == null))
            {
                return false;
            }
            return (source.NetworkId == target.NetworkId);
        }

        public static bool IsAlive(this GameObject unit) => 
            !unit.IsDead;

        public static bool IsAlly(this GameObjectTeam team) => 
            (team == Player.Instance.Team);

        public static bool IsBothFacing(this Obj_AI_Base source, Obj_AI_Base target) => 
            (source.IsFacing(target) && target.IsFacing(source));

        public static bool IsBuilding(this Vector2 vector) => 
            NavMesh.GetCollisionFlags(vector.X, vector.Y).HasFlag(CollisionFlags.Building);

        public static bool IsBuilding(this Vector3 vector) => 
            vector.To2D().IsBuilding();

        public static bool IsClone(this Obj_AI_Minion minion)
        {
            string item = minion.CharData.BaseSkinName.ToLower();
            return CloneList.Contains(item);
        }

        public static bool IsCompletlyInside(this SharpDX.Rectangle rectangle1, SharpDX.Rectangle rectangle2) => 
            ((((rectangle2.X >= rectangle1.X) && (rectangle2.Y >= rectangle1.Y)) && (rectangle2.BottomRight.X <= rectangle1.BottomRight.X)) && (rectangle2.BottomRight.Y <= rectangle1.BottomRight.Y));

        public static bool IsEnemy(this GameObjectTeam team) => 
            (team != Player.Instance.Team);

        public static bool IsFacing(this Obj_AI_Base source, Obj_AI_Base target) => 
            source.IsFacing(target.Position);

        public static bool IsFacing(this Obj_AI_Base source, Vector3 position) => 
            (((source != null) && source.IsValid) && (source.Direction().AngleBetween((position - source.Position).To2D()) < 90f));

        public static bool IsFacing(this Vector3 position1, Vector3 position2) => 
            (position1.To2D().Perpendicular().AngleBetween((position2 - position1).To2D()) < 90f);

        public static bool IsGrass(this Vector2 vector) => 
            NavMesh.GetCollisionFlags(vector.X, vector.Y).HasFlag(CollisionFlags.Grass);

        public static bool IsGrass(this Vector3 vector) => 
            vector.To2D().IsGrass();

        public static bool IsInAutoAttackRange(this Obj_AI_Base source, AttackableUnit target)
        {
            AIHeroClient client = source as AIHeroClient;
            return (((client > null) && ((client.Hero == Champion.Azir) && (client.IsMe && Orbwalker.ValidAzirSoldiers.Any<Obj_AI_Minion>(i => i.IsInAutoAttackRange(target))))) || source.IsInRange(target, source.GetAutoAttackRange(target)));
        }

        public static bool IsInFountainRange(this Obj_AI_Base hero, bool enemyFountain = false) => 
            (hero.IsHPBarRendered && ObjectManager.Get<Obj_SpawnPoint>().Any<Obj_SpawnPoint>(s => ((enemyFountain ? (s.Team != hero.Team) : (s.Team == hero.Team)) && hero.IsInRange(((GameObject) s), 1250f))));

        public static bool IsInRange(this GameObject source, GameObject target, float range) => 
            source.Position.To2D().IsInRange(target, range);

        public static bool IsInRange(this GameObject source, Obj_AI_Base target, float range) => 
            source.Position.To2D().IsInRange(target, range);

        public static bool IsInRange(this GameObject source, Vector2 target, float range) => 
            source.Position.To2D().IsInRange(target, range);

        public static bool IsInRange(this GameObject source, Vector3 target, float range) => 
            source.Position.To2D().IsInRange(target, range);

        public static bool IsInRange(this Obj_AI_Base source, GameObject target, float range) => 
            source.ServerPosition.To2D().IsInRange(target, range);

        public static bool IsInRange(this Obj_AI_Base source, Obj_AI_Base target, float range) => 
            source.ServerPosition.To2D().IsInRange(target, range);

        public static bool IsInRange(this Obj_AI_Base source, Vector2 target, float range) => 
            source.ServerPosition.To2D().IsInRange(target, range);

        public static bool IsInRange(this Obj_AI_Base source, Vector3 target, float range) => 
            source.ServerPosition.To2D().IsInRange(target, range);

        public static bool IsInRange(this Vector2 source, GameObject target, float range) => 
            source.IsInRange(target.Position.To2D(), range);

        public static bool IsInRange(this Vector2 source, Obj_AI_Base target, float range) => 
            source.IsInRange(target.ServerPosition.To2D(), range);

        public static bool IsInRange(this Vector2 source, Vector2 target, float range) => 
            (source.Distance(target, true) < range.Pow());

        public static bool IsInRange(this Vector2 source, Vector3 target, float range) => 
            source.IsInRange(target.To2D(), range);

        public static bool IsInRange(this Vector3 source, GameObject target, float range) => 
            source.To2D().IsInRange(target, range);

        public static bool IsInRange(this Vector3 source, Obj_AI_Base target, float range) => 
            source.To2D().IsInRange(target, range);

        public static bool IsInRange(this Vector3 source, Vector2 target, float range) => 
            source.To2D().IsInRange(target, range);

        public static bool IsInRange(this Vector3 source, Vector3 target, float range) => 
            source.To2D().IsInRange(target, range);

        public static bool IsInShopRange(this AIHeroClient hero) => 
            (hero.IsVisible && ObjectManager.Get<Obj_Shop>().Any<Obj_Shop>(s => ((Obj_AI_Base) hero).IsInRange(((GameObject) s), 1250f)));

        public static bool IsInside(this SharpDX.Rectangle rectangle, Vector2 position) => 
            ((((position.X >= rectangle.X) && (position.Y >= rectangle.Y)) && (position.X < rectangle.BottomRight.X)) && (position.Y < rectangle.BottomRight.Y));

        public static bool IsMinion(this Obj_AI_Base target) => 
            ObjectNames.Minions.Contains<string>(target.BaseSkinName);

        public static bool IsNear(this SharpDX.Rectangle rectangle, Vector2 position, int distance) => 
            new SharpDX.Rectangle(rectangle.X - distance, rectangle.Y - distance, rectangle.Width + distance, rectangle.Height + distance).IsInside(position);

        public static bool IsNeutral(this GameObjectTeam team) => 
            (team == GameObjectTeam.Neutral);

        public static bool IsOnScreen(this Vector2 p) => 
            ((((p.X <= Drawing.Width) && (p.X >= 0f)) && (p.Y <= Drawing.Height)) && (p.Y >= 0f));

        public static bool IsOnScreen(this Vector3 p) => 
            p.WorldToScreen().IsOnScreen();

        public static bool IsPartialInside(this SharpDX.Rectangle rectangle1, SharpDX.Rectangle rectangle2) => 
            (((rectangle2.X >= rectangle1.X) && (rectangle2.X <= rectangle1.BottomRight.X)) || ((rectangle2.Y >= rectangle1.Y) && (rectangle2.Y <= rectangle1.BottomRight.Y)));

        public static bool IsPet(this Obj_AI_Minion minion)
        {
            string item = minion.CharData.BaseSkinName.ToLower();
            return PetList.Contains(item);
        }

        public static bool IsRecalling(this AIHeroClient unit) => 
            unit.Buffs.Any<BuffInstance>(buff => ((buff.Type == BuffType.Aura) && buff.Name.ToLower().Contains("recall")));

        public static bool IsStructure(this GameObject target)
        {
            GameObjectType type = target.Type;
            return (((type == GameObjectType.obj_AI_Turret) || (type == GameObjectType.obj_BarracksDampener)) || (type == GameObjectType.obj_HQ));
        }

        public static bool IsUnderEnemyturret(this Obj_AI_Base target) => 
            EntityManager.Turrets.AllTurrets.Any<Obj_AI_Turret>(turret => ((target.Team != turret.Team) && turret.IsInAutoAttackRange(target)));

        public static bool IsUnderHisturret(this Obj_AI_Base target) => 
            EntityManager.Turrets.AllTurrets.Any<Obj_AI_Turret>(turret => ((target.Team == turret.Team) && turret.IsInAutoAttackRange(target)));

        public static bool IsUnderTurret(this Obj_AI_Base target) => 
            target.ServerPosition.IsUnderTurret();

        public static bool IsUnderTurret(this Vector3 position) => 
            position.To2D().IsUnderTurret(false);

        public static bool IsUnderTurret(this Vector2 position, bool EnemyTurretsOnly = false) => 
            EntityManager.Turrets.AllTurrets.Any<Obj_AI_Turret>(turret => (((Obj_AI_Base) turret).IsInRange(position, turret.GetAutoAttackRange(null)) && (!EnemyTurretsOnly || turret.IsEnemy)));

        public static bool IsValid(this BuffInstance buffInstance) => 
            (((buffInstance != null) && buffInstance.IsValid) && ((buffInstance.EndTime - Game.Time) > 0f));

        public static bool IsValid(this Obj_AI_Base unit) => 
            ((unit != null) && unit.IsValid);

        public static bool IsValid(this Vector2 vector, bool checkWorldCoords = false)
        {
            if (vector.IsZero)
            {
                return false;
            }
            if (checkWorldCoords)
            {
                Vector2 vector2 = vector.WorldToGrid();
                return ((((vector2.X >= 0f) && (vector2.X <= NavMesh.Width)) && (vector2.Y >= 0f)) && (vector2.Y <= NavMesh.Height));
            }
            return true;
        }

        public static bool IsValid(this Vector3 vector, bool checkWorldCoords = false) => 
            vector.To2D().IsValid(checkWorldCoords);

        public static bool IsValidMissile(this MissileClient source, bool checkTarget = true) => 
            (((ObjectManager.Get<MissileClient>().Count<MissileClient>(w => (w.MemoryAddress == source.MemoryAddress)) == 1) && source.IsValid) && ((source.Target == null) || (source.Target.IsValid && (!checkTarget || !source.IsInRange(source.Target, source.Target.BoundingRadius)))));

        public static bool IsValidTarget(this AttackableUnit target, float? range = new float?(), bool onlyEnemyTeam = false, Vector3? rangeCheckFrom = new Vector3?())
        {
            if (((((target == null) || !target.IsValid) || (target.IsDead || !target.IsVisible)) || !target.IsTargetable) || target.IsInvulnerable)
            {
                return false;
            }
            if (onlyEnemyTeam && (Player.Instance.Team == target.Team))
            {
                return false;
            }
            Obj_AI_Base base2 = target as Obj_AI_Base;
            if ((base2 != null) && !base2.IsHPBarRendered)
            {
                return false;
            }
            if (range.HasValue)
            {
                float? nullable;
                range = new float?(range.Value.Pow());
                Vector3 vector = (base2 != null) ? base2.ServerPosition : target.Position;
                return (rangeCheckFrom.HasValue ? ((rangeCheckFrom.Value.Distance(vector, true) < (nullable = range).GetValueOrDefault()) ? nullable.HasValue : false) : ((Player.Instance.ServerPosition.DistanceSquared(vector) < (nullable = range).GetValueOrDefault()) ? nullable.HasValue : false));
            }
            return true;
        }

        public static bool IsWall(this Vector2 vector) => 
            NavMesh.GetCollisionFlags(vector.X, vector.Y).HasFlag(CollisionFlags.None | CollisionFlags.Wall);

        public static bool IsWall(this Vector3 vector) => 
            vector.To2D().IsWall();

        public static bool IsWard(this GameObject unit) => 
            ((unit.Type == GameObjectType.obj_Ward) || (((unit.Type == GameObjectType.obj_AI_Minion) && unit.Name.ToLower().Contains("ward")) && !unit.Name.ToLower().Contains("corpse")));

        public static Obj_AI_Base LastTarget(this Obj_AI_Turret turret)
        {
            if (!Orbwalker.LastTargetTurrets.ContainsKey(turret.NetworkId))
            {
                Orbwalker.LastTargetTurrets[turret.NetworkId] = null;
            }
            else if ((Orbwalker.LastTargetTurrets[turret.NetworkId] != null) && !Orbwalker.LastTargetTurrets[turret.NetworkId].IsValidTarget(null, false, null))
            {
                Orbwalker.LastTargetTurrets[turret.NetworkId] = null;
            }
            return Orbwalker.LastTargetTurrets[turret.NetworkId];
        }

        public static Vector3 MinimapToWorld(this Vector2 vector) => 
            TacticalMap.MinimapToWorld(vector.X, vector.Y);

        public static float MissingHealthPercent(this Obj_AI_Base target) => 
            (target.TotalHealth() / target.TotalMaxHealth());

        public static SharpDX.Rectangle Multiply(this SharpDX.Rectangle rectangle1, SharpDX.Rectangle rectangle2) => 
            new SharpDX.Rectangle(rectangle1.X * rectangle2.X, rectangle1.Y * rectangle2.Y, rectangle1.Width * rectangle2.Width, rectangle1.Height * rectangle2.Height);

        public static SharpDX.Rectangle Negate(this SharpDX.Rectangle rectangle) => 
            new SharpDX.Rectangle(-rectangle.X, -rectangle.Y, -rectangle.Width, -rectangle.Height);

        public static Vector2 Normalized(this Vector2 vector) => 
            Vector2.Normalize(vector);

        public static Vector3 Normalized(this Vector3 vector) => 
            Vector3.Normalize(vector);

        public static double Pow(this double number) => 
            (number * number);

        public static int Pow(this int number) => 
            (number * number);

        public static float Pow(this float number) => 
            (number * number);

        public static uint Pow(this uint number) => 
            (number * number);

        public static Vector3[] RealPath(this Obj_AI_Base unit) => 
            Prediction.Position.GetRealPath(unit);

        public static Vector3 ScreenToWorld(this Vector2 vector) => 
            Drawing.ScreenToWorld(vector.X, vector.Y);

        public static double Sqrt(this double number) => 
            Math.Sqrt(number);

        public static double Sqrt(this int number) => 
            Math.Sqrt((double) number);

        public static double Sqrt(this float number) => 
            Math.Sqrt((double) number);

        public static double Sqrt(this uint number) => 
            Math.Sqrt((double) number);

        public static SharpDX.Rectangle Substract(this SharpDX.Rectangle rectangle1, SharpDX.Rectangle rectangle2) => 
            new SharpDX.Rectangle(rectangle1.X - rectangle2.X, rectangle1.Y - rectangle2.Y, rectangle1.Width - rectangle2.Width, rectangle1.Height - rectangle2.Height);

        public static Vector2 To2D(this Vector3 vector) => 
            new Vector2(vector.X, vector.Y);

        public static List<Vector2> To2D(this List<Vector3> points)
        {
            List<Vector2> list = new List<Vector2>();
            list.AddRange(from v in points select v.To2D());
            return list;
        }

        public static Vector3 To3D(this Vector2 vector, int height = 0) => 
            new Vector3(vector.X, vector.Y, (float) height);

        public static Vector3 To3DWorld(this Vector2 vector) => 
            new Vector3(vector.X, vector.Y, NavMesh.GetHeightForPosition(vector.X, vector.Y));

        public static NavMeshCell ToNavMeshCell(this Vector2 vector)
        {
            Vector2 vector2 = vector.WorldToGrid();
            return NavMesh.GetCell((short) vector2.X, (short) vector2.Y);
        }

        public static NavMeshCell ToNavMeshCell(this Vector3 vector) => 
            vector.To2D().ToNavMeshCell();

        public static IEnumerable<Obj_AI_Base> ToObj_AI_BaseEnumerable<T>(this IEnumerable<T> list) where T: Obj_AI_Base => 
            list.Cast<Obj_AI_Base>();

        public static List<Obj_AI_Base> ToObj_AI_BaseList<T>(this IEnumerable<T> list) where T: Obj_AI_Base => 
            list.Cast<Obj_AI_Base>().ToList<Obj_AI_Base>();

        public static List<Obj_AI_Base> ToObj_AI_BaseList<T>(this List<T> list) where T: Obj_AI_Base => 
            list.Cast<Obj_AI_Base>().ToList<Obj_AI_Base>();

        public static SharpDX.Color ToSharpDX(this System.Drawing.Color color) => 
            new SharpDX.Color(color.R, color.G, color.B, color.A);

        public static System.Drawing.Color ToSystem(this SharpDX.Color color) => 
            System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);

        public static float TotalHealth(this Obj_AI_Base target)
        {
            float health = target.Health;
            AIHeroClient client = target as AIHeroClient;
            if ((client > null) && (client.Hero == Champion.Kled))
            {
                health += target.KledSkaarlHP;
            }
            return health;
        }

        public static float TotalMaxHealth(this Obj_AI_Base target)
        {
            float maxHealth = target.MaxHealth;
            AIHeroClient client = target as AIHeroClient;
            if ((client > null) && (client.Hero == Champion.Kled))
            {
                maxHealth += target.MaxKledSkaarlHP;
            }
            return maxHealth;
        }

        public static float TotalMissingHealth(this Obj_AI_Base target) => 
            (target.TotalMaxHealth() - target.TotalHealth());

        public static float TotalShield(this Obj_AI_Base target)
        {
            float num = (target.AllShield + target.AttackShield) + target.MagicShield;
            AIHeroClient client = target as AIHeroClient;
            if (client > null)
            {
                switch (client.Hero)
                {
                    case Champion.Blitzcrank:
                        if (!target.HasBuff("BlitzcrankManaBarrierCD") && !target.HasBuff("ManaBarrier"))
                        {
                            num += target.Mana / 2f;
                        }
                        return num;

                    case Champion.Yasuo:
                        if (client.ManaPercent.Equals((float) 100f))
                        {
                            int num3 = new int[] { 
                                100, 0x69, 110, 0x73, 120, 130, 140, 150, 0xa5, 180, 200, 0xe1, 0xff, 330, 380, 440,
                                510
                            }[Math.Min(0x11, client.Level - 1)];
                            num += num3;
                        }
                        return num;
                }
            }
            return num;
        }

        public static float TotalShieldHealth(this Obj_AI_Base target) => 
            (target.TotalHealth() + target.TotalShield());

        public static float TotalShieldMaxHealth(this Obj_AI_Base target) => 
            (target.TotalMaxHealth() + target.TotalShield());

        public static bool WillCollideWithYasuoWall(this Obj_AI_Base start, Obj_AI_Base end, bool OnlyEnemy = true) => 
            start.ServerPosition.WillCollideWithYasuoWall(end.ServerPosition, OnlyEnemy);

        public static bool WillCollideWithYasuoWall(this Obj_AI_Base start, Vector2 end, bool OnlyEnemy = true) => 
            start.ServerPosition.WillCollideWithYasuoWall(end.To3DWorld(), OnlyEnemy);

        public static bool WillCollideWithYasuoWall(this Obj_AI_Base start, Vector3 end, bool OnlyEnemy = true) => 
            start.ServerPosition.WillCollideWithYasuoWall(end, OnlyEnemy);

        public static bool WillCollideWithYasuoWall(this Vector2 start, Obj_AI_Base end, bool OnlyEnemy = true) => 
            start.To3DWorld().WillCollideWithYasuoWall(end.ServerPosition, OnlyEnemy);

        public static bool WillCollideWithYasuoWall(this Vector2 start, Vector2 end, bool OnlyEnemy = true) => 
            start.To3DWorld().WillCollideWithYasuoWall(end.To3DWorld(), OnlyEnemy);

        public static bool WillCollideWithYasuoWall(this Vector3 start, Obj_AI_Base end, bool OnlyEnemy = true) => 
            start.WillCollideWithYasuoWall(end.ServerPosition, OnlyEnemy);

        public static bool WillCollideWithYasuoWall(this Vector3 start, Vector3 end, bool OnlyEnemy = true) => 
            Prediction.Position.Collision.GetYasuoWallCollision(start, end, OnlyEnemy).IsValid(false);

        public static Vector2 WorldToGrid(this Vector2 vector) => 
            NavMesh.WorldToGrid(vector.X, vector.Y);

        public static Vector2 WorldToGrid(this Vector3 vector) => 
            vector.To2D().WorldToGrid();

        public static Vector2 WorldToMinimap(this Vector3 vector) => 
            TacticalMap.WorldToMinimap(vector);

        public static Vector2 WorldToScreen(this Vector3 vector) => 
            Drawing.WorldToScreen(vector);

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly Extensions.<>c <>9 = new Extensions.<>c();
            public static Func<Vector3, Vector2> <>9__122_0;
            public static Func<BuffInstance, bool> <>9__29_0;
            public static Func<float, BuffInstance, float> <>9__29_1;
            public static Func<BuffInstance, bool> <>9__30_0;
            public static Func<float, BuffInstance, float> <>9__30_1;
            public static Func<BuffInstance, bool> <>9__32_0;
            public static Func<float, BuffInstance, float> <>9__32_1;
            public static Func<BuffInstance, bool> <>9__41_0;
            public static Func<SpellDataInst, float> <>9__59_0;
            public static Func<SpellDataInst, float> <>9__60_0;
            public static Func<InventorySlot, float> <>9__72_0;
            public static Func<InventorySlot, float> <>9__73_0;
            public static Func<InventorySlot, float> <>9__74_0;

            internal float <GetHighestSpellRange>b__59_0(SpellDataInst spell) => 
                ((spell.SData.CastRangeDisplayOverride > 0f) ? spell.SData.CastRangeDisplayOverride : spell.SData.CastRange);

            internal float <GetItemsFlatArmorPenetrationMod>b__72_0(InventorySlot i) => 
                new Item(i.Id, 0f).ItemInfo.Stats.rFlatArmorPenetrationMod;

            internal float <GetItemsFlatMagicPenetrationMod>b__74_0(InventorySlot i) => 
                new Item(i.Id, 0f).ItemInfo.Stats.rFlatMagicPenetrationMod;

            internal float <GetItemsPercentArmorPenetrationMod>b__73_0(InventorySlot i) => 
                new Item(i.Id, 0f).ItemInfo.Stats.rPercentArmorPenetrationMod;

            internal float <GetLowestSpellRange>b__60_0(SpellDataInst spell) => 
                ((spell.SData.CastRangeDisplayOverride > 0f) ? spell.SData.CastRangeDisplayOverride : spell.SData.CastRange);

            internal bool <GetMovementBlockedDebuffDuration>b__30_0(BuffInstance b) => 
                ((b.IsActive && (Game.Time < b.EndTime)) && Extensions.BlockedMovementBuffTypes.Contains(b.Type));

            internal float <GetMovementBlockedDebuffDuration>b__30_1(float current, BuffInstance buff) => 
                Math.Max(current, buff.EndTime);

            internal bool <GetMovementDebuffDuration>b__29_0(BuffInstance b) => 
                ((b.IsActive && (Game.Time < b.EndTime)) && Extensions.BlockedMovementBuffTypes.Contains(b.Type));

            internal float <GetMovementDebuffDuration>b__29_1(float current, BuffInstance buff) => 
                Math.Max(current, buff.EndTime);

            internal bool <GetMovementReducedDebuffDuration>b__32_0(BuffInstance b) => 
                ((b.IsActive && (Game.Time < b.EndTime)) && Extensions.ReducedMovementBuffTypes.Contains(b.Type));

            internal float <GetMovementReducedDebuffDuration>b__32_1(float current, BuffInstance buff) => 
                Math.Max(current, buff.EndTime);

            internal bool <IsRecalling>b__41_0(BuffInstance buff) => 
                ((buff.Type == BuffType.Aura) && buff.Name.ToLower().Contains("recall"));

            internal Vector2 <To2D>b__122_0(Vector3 v) => 
                v.To2D();
        }
    }
}

