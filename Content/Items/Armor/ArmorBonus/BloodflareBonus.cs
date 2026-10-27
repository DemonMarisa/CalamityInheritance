using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.Blance;
using CalamityInheritance.Content.Buff.Armor.Melee;
using CalamityInheritance.Content.CDs;
using CalamityInheritance.Content.Projectiles.Armor.Magic;
using CalamityInheritance.Content.Projectiles.Armor.Ranged;
using CalamityInheritance.Content.Projectiles.Armor.Summon;
using CalamityInheritance.Content.Projectiles.Armor.Typeless;
using CalamityInheritance.Core.GlobalInstance.Players;
using CalamityInheritance.Core.Misc;
using CalamityInheritance.Core.Utils;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorBonus
{
    public class BloodflareBonus
    {
        public static void Main_OnHitNPC(CIPlayer player, NPC.HitInfo hit, NPC target)
        {
            if (!player.HasCount("BloodflareDropHeartCD"))
            {
                Item.NewItem(target.GetSource_Loot(), target.Hitbox, ItemID.Heart);
                player.AddCount("BloodflareDropHeartCD", 180);
            }
        }
        public static void MagicShoot_PostUpdateMisc(Player player)
        {
            if (Main.myPlayer != player.whoAmI || player.ItemTimeIsZero || !player.ItemAnimationActive || player.HeldItem is null || player.HeldItem.damage == 0 || !player.HeldItem.DamageType.CountsAsClass(DamageClass.Magic))
                return;
            if (!player.CI().HasCount("BloodflareMagicFireEffectCD"))
            {
                int damage = player.GetIntDamage<MagicDamageClass>(CIArmorBlance.BloodflareMagicGhostlyBolt(player));
                Vector2 fireVel = player.GetPlayerToMouseVector2();
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, fireVel * 12.5f, ProjectileType<GhostlyBolt>(), damage, 2f, player.whoAmI);
                player.CI().AddCount("BloodflareMagicFireEffectCD", 100);
            }
        }
        public static void MagicHit_OnHitNPC(CIPlayer player, NPC.HitInfo hit, NPC target)
        {
            if (!player.HasCount("BloodflareMagicHitEffectCD"))
            {
                for (int i = 0; i < 3; i++)
                {
                    int damage = player.Player.GetIntDamage<MagicDamageClass>(CIArmorBlance.BloodflareMagicBlast(player.Player));
                    Vector2 spawnPos = target.position + new Vector2(Main.rand.NextFloat(target.width), Main.rand.NextFloat(target.height)) + Main.rand.NextVector2CircularEdge(100, 100);
                    int fire = Projectile.NewProjectile(player.Player.GetSource_FromThis(), spawnPos, Vector2.Zero, ProjectileType<BloodflareFireBallBlast>(), damage, 0f, player.Player.whoAmI);
                    Main.projectile[fire].DamageType = DamageClass.Generic;
                    Main.projectile[fire].netUpdate = true;
                }
                player.AddCount("BloodflareMagicHitEffectCD", 120);
            }
        }
        public static void MeleeHit_OnHitNPC(CIPlayer player, NPC.HitInfo hit, NPC target)
        {
            int healAmount = Main.rand.Next(2) + 1;
            player.Player.NCHeal(healAmount);
            if (!player.Player.HasBuff<BloodflareBloodFrenzy>())
                player.BloodflareMeleeHitCount++;
            if (player.BloodflareMeleeHitCount >= 15)
            {
                player.Player.AddBuff(BuffType<BloodflareBloodFrenzy>(), 300);
                player.BloodflareMeleeHitCount = 0;
            }
        }
        public static void RangedArmorSetBonus_KeyPress(CIPlayer player, Player player1)
        {
            if (player1.whoAmI != Main.myPlayer)
                return;
            if (!player.Player.HasCD<BloodflareRanged>())
            {
                SoundEngine.PlaySound(CISounds.BloodflareRangerActivation, player1.Center);
                for (int d = 0; d < 64; d++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(player1.position.X, player1.position.Y + 16f), player1.width, player1.height - 16, CIDustID.DustNecroplasm, 0f, 0f, 0, default, 1f);
                    dust.velocity *= 3f;
                    dust.scale *= 1.15f;
                }
                int dustAmt = 36;
                for (int d = 0; d < dustAmt; d++)
                {
                    Vector2 source = Vector2.Normalize(player1.velocity) * new Vector2((float)player1.width / 2f, (float)player1.height) * 0.75f;
                    source = source.RotatedBy((double)((float)(d - (dustAmt / 2 - 1)) * MathHelper.TwoPi / (float)dustAmt), default) + player1.Center;
                    Vector2 dustVel = source - player1.Center;
                    Dust phanto = Dust.NewDustDirect(source + dustVel, 0, 0, CIDustID.DustNecroplasm, dustVel.X * 1.5f, dustVel.Y * 1.5f, 100, default, 1.4f);
                    phanto.noGravity = true;
                    phanto.noLight = true;
                    phanto.velocity = dustVel;
                }
                var Projsource = player1.GetSource_FromThis();
                int damage = (int)(player1.GetTotalDamage<RangedDamageClass>().ApplyTo(CIArmorBlance.BloodFlareRangedSetBonus(player1)));
                for (int i = 0; i < 16; i++)
                {
                    float ai1 = Main.rand.NextFloat() + 0.5f;
                    Vector2 circleVel = (MathHelper.TwoPi * i / 16f).ToRotationVector2() * Main.rand.NextFloat(5f, 8f);
                    int soul = Projectile.NewProjectile(Projsource, player1.Center, circleVel, ProjectileType<BloodflareSoul>(), damage, 0f, player1.whoAmI, 0f, ai1);
                    if (soul.InBounds(Main.maxProjectiles))
                        Main.projectile[soul].DamageType = DamageClass.Generic;
                }
                player.Player.AddCD(LAPContent.CDType<BloodflareRanged>(), 1800);
            }
        }
        public static void RangedShoot_PostUpdateMisc(Player player)
        {
            if (Main.myPlayer != player.whoAmI || player.ItemTimeIsZero || !player.ItemAnimationActive || player.HeldItem is null || player.HeldItem.damage == 0 || !player.HeldItem.DamageType.CountsAsClass(DamageClass.Ranged))
                return;
            if (!player.CI().HasCount("BloodflareRangedFireEffectCD"))
            {
                int damage = player.GetIntDamage<MagicDamageClass>(CIArmorBlance.BloodFlareRangedProj(player));
                Vector2 fireVel = player.GetPlayerToMouseVector2();
                Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, fireVel * 12.5f, ProjectileType<BloodBomb>(), damage, 2f, player.whoAmI);
                player.CI().AddCount("BloodflareRangedFireEffectCD", 120);
            }
        }
        public static void RogueHit_OnHitNPC(CIPlayer player)
        {
            if (!player.HasCount("BloodflareRogueHealCD"))
            {
                if (Main.rand.NextBool(2))
                {
                    int healAmount = Main.rand.Next(2) + 1;
                    player.Player.NCHeal(healAmount);
                    player.AddCount("BloodflareRogueHealCD", 30);
                }
            }
        }
        public static void Rogue_PostUpdate(Player player)
        {
            float lifeprecent = (float)player.statLife / player.statLifeMax2;
            if (lifeprecent > 0.8f)
            {
                player.statDefense += 30;
                player.GetCritChance<ThrowingDamageClass>() += 5;
            }
            else
            {
                player.GetDamage<ThrowingDamageClass>() += 0.15f;
            }
        }
        public static void Summon_PostUpdate(Player player)
        {
            if (player.statLife >= (int)(player.statLifeMax2 * 0.9))
                player.GetDamage<SummonDamageClass>() += 0.1f;
            else if (player.statLife <= (int)(player.statLifeMax2 * 0.5))
            {
                player.statDefense += 20;
                player.LAP().LifeRegen += 4;
            }

            if (player.whoAmI == Main.myPlayer && !player.CI().HasCount("BloodflareSummonCD"))
            {
                for (int I = 0; I < 3; I++)
                {
                    float ai1 = I * 120;
                    int damage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(3000);
                    int projectile = Projectile.NewProjectile(player.GetSource_FromThis(), player.Center.X + (float)(Math.Sin(I * 120) * 550), player.Center.Y + (float)(Math.Cos(I * 120) * 550), 0f, 0f,
                        ModContent.ProjectileType<GhostlyMine>(), damage, 1f, player.whoAmI, ai1, 0f);
                    if (projectile.InBounds(Main.maxProjectiles))
                    {
                        Main.projectile[projectile].originalDamage = 3000;
                        Main.projectile[projectile].DamageType = DamageClass.Generic;
                    }
                }
                player.CI().AddCount("BloodflareSummonCD", 900);
            }
        }
    }
}
