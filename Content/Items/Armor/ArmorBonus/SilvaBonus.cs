using CalamityInheritance.Common.Blance;
using CalamityInheritance.Content.Buff.SummonBuff.Armor;
using CalamityInheritance.Content.CDs;
using CalamityInheritance.Content.Items.Armor.ArmorItems.Silva;
using CalamityInheritance.Content.Projectiles.Armor.Magic;
using CalamityInheritance.Content.Projectiles.Armor.Summon.Header;
using CalamityInheritance.Content.Projectiles.Heals;
using CalamityInheritance.Core.GlobalInstance.Players;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorBonus
{
    public class SilvaBonus
    {
        public static void Main_PostUpdate(CIPlayer ciplayer, Player player)
        {
            if (ciplayer.SilvaMagic)
            {
                if (player.HasCD<SilvaReviveCD>())
                    player.GetDamage<MagicDamageClass>() += 0.60f;
            }
            if (ciplayer.SilvaMelee)
            {
                if (player.HasCD<SilvaReviveCD>())
                    player.CI().ContactDamageReduction *= 0.6f;
            }
            if (ciplayer.SilvaRanged)
            {
                if (player.HeldItem.useTime > 3 && player.HeldItem.DamageType.CountsAsClass<RangedDamageClass>())
                    player.GetAttackSpeed<RangedDamageClass>() += CIArmorBlance.SilvaRangedAttackSpeed(player);
                if (player.HasCD<SilvaReviveCD>())
                    player.GetDamage<RangedDamageClass>() += 0.40f;
            }
            if (ciplayer.SilvaRogue)
            {
                if (player.statLife > (int)(player.statLifeMax2 * 0.5) && player.HeldItem.DamageType.CountsAsClass<ThrowingDamageClass>())
                    player.GetAttackSpeed<ThrowingDamageClass>() += CIArmorBlance.SilvaRogueAttackSpeed(player);
                if (player.HasCD<SilvaReviveCD>())
                    player.GetDamage<ThrowingDamageClass>() += 0.40f;
            }
            if (ciplayer.SilvaSummon)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    var source = player.GetSource_FromThis();
                    if (player.FindBuffIndex(BuffType<SilvaCrystalBuff>()) == -1)
                    {
                        player.AddBuff(BuffType<SilvaCrystalBuff>(), 2, true);
                    }
                    if (player.ownedProjectileCounts[ProjectileType<SilvaCrystal>()] < 1)
                    {
                        var damage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(1000);

                        var p = Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0f, -1f, ProjectileType<SilvaCrystal>(), damage, 0f, Main.myPlayer, -20f, 0f);
                        if (Main.projectile.IndexInRange(p))
                            Main.projectile[p].originalDamage = 1000;
                    }
                }
                if (player.HasCD<SilvaReviveCD>())
                    player.GetDamage<ThrowingDamageClass>() *= 1.10f;
            }
        }
        public static void HitNPCWithProj_All(CIPlayer player, Projectile proj, NPC.HitInfo hit, NPC target)
        {
            if (player.Player.whoAmI != Main.myPlayer)
                return;
            if (!player.HasCount("SilvaHealCD"))
            {
                player.Player.SpawnLifeStealProj(target, proj.GetSource_FromThis(), ProjectileType<SilvaOrbLegacy>(), proj.Center, Vector2.UnitX.RotatedByRandom(6.29f) * 6f);
                player.AddCount("SilvaHealCD", Main.rand.Next(10, 30));
            }
        }
        public static void HitNPC_Magic(CIPlayer player, NPC.HitInfo hit, NPC target)
        {
            SoundEngine.PlaySound(SoundID.Zombie103, target.Center);
            if (!player.HasCount("SilvaMagicHitCD"))
            {
                int silvaBurstDamage = player.Player.GetIntDamage<MagicDamageClass>(CIArmorBlance.SilvaMagicHit(player.Player));
                Projectile.NewProjectile(player.Player.GetSource_FromThis(), target.Center, Vector2.Zero, ProjectileType<SilvaBurst>(), silvaBurstDamage, 8f, player.Player.whoAmI);
                player.AddCount("SilvaMagicHitCD", 120);
            }
        }
        public static void PostUpdate_Melee(Player player)
        {
            float multiplier = player.statLife / (float)player.statLifeMax2;
            player.GetDamage<MeleeDamageClass>() += (float)(multiplier * 0.2f);
        }
        public static void ModifyHitNPC_Melee(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Main.rand.NextBool(4))
                modifiers.SourceDamage *= 5;
        }
    }
}
