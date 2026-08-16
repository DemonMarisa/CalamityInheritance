using CalamityInheritance.Content.Projectiles.Armor.Magic;
using CalamityInheritance.Content.Projectiles.Armor.Melee;
using CalamityInheritance.Content.Projectiles.Armor.Ranged;
using CalamityInheritance.Content.Projectiles.Armor.Summon;
using CalamityInheritance.Content.Projectiles.Heals;
using CalamityInheritance.Core.GlobalInstance.Players;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorBonus
{
    public class GodSlayerBonus
    {
        public static void HitNPCWithProj_Magic(CIPlayer player, Projectile proj, NPC.HitInfo hit, NPC target)
        {
            if (player.Player.whoAmI != Main.myPlayer)
                return;
            if (!player.HasCount("GodSlayerMagicProj"))
            {
                int weaponDamage = hit.SourceDamage;
                int finalDamage = 400 + weaponDamage / 4;
                int projectileTypes = ProjectileType<GodSlayerOrb>();
                float randomAngleOffset = (float)(Main.rand.NextFloat(MathHelper.TwoPi));
                Vector2 direction = new((float)Math.Cos(randomAngleOffset), (float)Math.Sin(randomAngleOffset));
                float randomSpeed = Main.rand.NextFloat(12f, 16f);
                Projectile.NewProjectile(proj.GetSource_FromThis(), proj.Center, direction * randomSpeed, projectileTypes, finalDamage, proj.knockBack);
                player.AddCount("GodSlayerMagicProj", 2);
            }
            if (!player.HasCount("GodSlayerMagicHealCD"))
            {
                player.Player.SpawnLifeStealProj(target, proj.GetSource_FromThis(), ProjectileType<GodSlayerHealOrb>(), proj.Center, Vector2.UnitX.RotatedByRandom(6.29f) * 6f);
                player.AddCount("GodSlayerMagicHealCD", Main.rand.Next(10, 30));
            }
        }
        public static void HitNPCWithProj_Summon(CIPlayer player, Projectile proj, NPC.HitInfo hit, NPC target)
        {
            if (!player.HasCount("GodSlayerSummonProj"))
            {
                int weaponDamage = hit.SourceDamage;
                int finalDamage = 800 + weaponDamage / 2;
                int projectileTypes = ProjectileType<GodSlayerPhantom>();
                float randomAngleOffset = (float)(Main.rand.NextDouble() * 2 * MathHelper.Pi);
                Vector2 direction = new((float)Math.Cos(randomAngleOffset), (float)Math.Sin(randomAngleOffset));
                float randomSpeed = Main.rand.NextFloat(6f, 8f);
                Projectile.NewProjectile(proj.GetSource_FromThis(), proj.Center, direction * randomSpeed, projectileTypes, finalDamage, proj.knockBack);
                player.AddCount("GodSlayerSummonProj", 3);
            }
        }
        public static void HitNPC_Melee(CIPlayer player, NPC.HitInfo hit, NPC target)
        {
            if (player.Player.whoAmI != Main.myPlayer)
                return;
            if (!player.HasCount("GodSlayerMeleeProj"))
            {
                int finalDamage = (int)(500 + hit.SourceDamage / 2f);
                Vector2 getSpwanPos = player.Player.Center;
                Vector2 velocity = Vector2.Zero;
                Projectile.NewProjectile(player.Player.GetSource_FromThis(), getSpwanPos, velocity * 4f, ProjectileType<GodslayerDartMount>(), finalDamage, 0f, player.Player.whoAmI);
                player.AddCount("GodSlayerMeleeProj", 60);
            }
        }
        public static void PostUpdate_Ranged(Player player)
        {
            if (Main.myPlayer != player.whoAmI || !player.controlUseItem || player.HeldItem is null || player.HeldItem.damage == 0 || !player.HeldItem.DamageType.CountsAsClass(DamageClass.Ranged))
                return;
            if (!player.CI().HasCount("GodSlayerRangedProj"))
            {
                int damage = player.CalcIntDamage<RangedDamageClass>(player.ActiveItem().damage);
                int shrapnelRoundDamage = CIUtils.DamageSoftCap(damage * 2, 1500);
                Vector2 fireVel = player.GetPlayerToMouseVector2();
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, fireVel * 12.5f, ProjectileType<GodSlayerShrapnelRound>(), shrapnelRoundDamage, 2f, player.whoAmI);
                player.CI().AddCount("GodSlayerRangedProj", 120);
            }
        }
        public static void ModifyHitNPC_Ranged(Player player, ref NPC.HitModifiers modifiers)
        {
            int randomChance = (int)(player.GetTotalCritChance(DamageClass.Ranged) - 100);
            if (randomChance > 0)
            {
                if (Main.rand.Next(1, 101) <= randomChance)
                    modifiers.FinalDamage *= 2;
            }
            else if (Main.rand.NextBool(20))
                modifiers.FinalDamage *= 4;
        }
        public static void OnHurt_Magic(Player player)
        {
            SoundEngine.PlaySound(SoundID.Item73, player.Center);
            if (player.whoAmI == Main.myPlayer)
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ProjectileType<GodSlayerBlaze>(), 1200, 1f, player.whoAmI, 0f, 0f);
        }
        public static void OnHurt_Melee(Player player)
        {
            if (player.whoAmI != Main.myPlayer)
                return;
            SoundEngine.PlaySound(SoundID.Item73, player.Center);
            float spread = 45f * 0.0174f;
            double startAngle = Math.Atan2(player.velocity.X, player.velocity.Y) - spread / 2;
            double deltaAngle = spread / 8f;
            double offsetAngle;
            int shrapnelDamage = player.CalcIntDamage<MeleeDamageClass>(1500);
            if (player.whoAmI == Main.myPlayer)
            {
                for (int i = 0; i < 4; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center.X, player.Center.Y, (float)(Math.Sin(offsetAngle) * 32f), (float)(Math.Cos(offsetAngle) * 32f), ProjectileType<GodSlayerDart>(), shrapnelDamage, 5f, player.whoAmI, 1f, 0f);
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center.X, player.Center.Y, (float)(-Math.Sin(offsetAngle) * 32f), (float)(-Math.Cos(offsetAngle) * 32f), ProjectileType<GodSlayerDart>(), shrapnelDamage, 5f, player.whoAmI, 1f, 0f);
                }
            }
        }
    }
}
