using CalamityInheritance.Content.Buff.Armor.Magic;
using CalamityInheritance.Content.Buff.Armor.Melee;
using CalamityInheritance.Content.Projectiles.Armor.Melee;
using CalamityInheritance.Content.Projectiles.Armor.Ranged;
using CalamityInheritance.Content.Projectiles.Magic.Ray;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorBonus
{
    public class ReaverLegacyBonus
    {
        public static void HitNPCWithProj_Melee(Player player, Projectile proj, NPC target)
        {
            if (Main.myPlayer != player.whoAmI)
                return;
            if(!player.CI().HasCount("ReaverMeleeCount"))
            {
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, target.Center);
                Projectile.NewProjectile(player.GetSource_FromThis(), target.Center, Vector2.Zero, ProjectileType<ReaverBlast>(), CIUtils.DamageSoftCap(proj.damage, 60), 0.15f, player.whoAmI);
                player.CI().AddCount("ReaverMeleeCount", 10);
            }
        }
        public static void HitNPCWithItem_Melee(Player player, NPC target, NPC.HitInfo hit)
        {
            if (player.whoAmI != Main.myPlayer)
                return;
            if (!player.CI().HasCount("ReaverMeleeCount"))
            {
                SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, target.Center);
                Projectile.NewProjectile(player.GetSource_FromThis(), target.Center, Vector2.Zero, ProjectileType<ReaverBlast>(), CIUtils.DamageSoftCap(hit.SourceDamage, 60), 0.15f, player.whoAmI);
                player.CI().AddCount("ReaverMeleeCount", 10);
            }
        }
        public static void OnPlayerHurt_Melee(Player player)
        {
            player.AddBuff(BuffType<ReaverMeleeRage>(), 360);
        }
        public static void HitNPCWithProj_Magic(Player player, Projectile proj, NPC target)
        {
            if (Main.myPlayer != player.whoAmI)
                return;
            if (!player.CI().HasCount("ReaverMagicCount"))
            {
                int[] projectileTypes = [ProjectileID.SporeGas, ProjectileID.SporeGas2, ProjectileID.SporeGas3];
                float baseAngleIncrement = 2 * MathHelper.Pi / 16;
                float randomAngleOffset = (float)(Main.rand.NextDouble() * MathHelper.Pi / 4 - MathHelper.Pi / 8);
                //好像这样伤害还挺低的但我也不知道该不该调整了
                int newDamage = CIUtils.DamageSoftCap(15 + 0.3f * proj.damage, 30);
                for (int sporecounts = 0; sporecounts < 16; sporecounts++)
                {
                    float angle = sporecounts * baseAngleIncrement + randomAngleOffset;
                    Vector2 direction = new((float)Math.Cos(angle), (float)Math.Sin(angle));
                    int randomProjectileType = projectileTypes[Main.rand.Next(projectileTypes.Length)];
                    float randomSpeed = Main.rand.NextFloat(2f, 4f);
                    Projectile p = Projectile.NewProjectileDirect(player.GetSource_FromThis(), target.Center, direction * randomSpeed, randomProjectileType, newDamage, proj.knockBack);
                    p.usesIDStaticNPCImmunity = true;
                    p.idStaticNPCHitCooldown = 20;
                }
                player.AddBuff(BuffType<ReaverMagePower>(), 180);
                target.AddBuff(BuffID.Poisoned, 120);
                player.CI().AddCount("ReaverMagicCount", 90);
            }
        }
        public static void PostUpdate_Ranged(Player player)
        {
            if (Main.myPlayer != player.whoAmI || !player.controlUseItem || player.HeldItem is null || player.HeldItem.damage == 0 || !player.HeldItem.DamageType.CountsAsClass(DamageClass.Ranged))
                return;
            int damage = player.CalcIntDamage<RangedDamageClass>(player.ActiveItem().damage);
            if (!player.CI().HasCount("ReaverRangedCD"))
            {
                player.CI().AddCount("ReaverRangedCD", 150);
            }
            else
            {
                Vector2 velocity = player.GetPlayerToMouseVector2() * 12f;
                if (player.CI().CICD["ReaverRangedCD"] == 149)
                {
                    int RocketDamage = (int)(0.70f * damage);
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, velocity, ProjectileType<MiniRocket>(), RocketDamage, 2f, player.whoAmI, 0f, 0f);
                }
                if (player.CI().CICD["ReaverRangedCD"] == 139)
                {
                    int RocketDamage = (int)(0.70f * damage);
                    Vector2 adjustedVelocity = velocity.RotatedBy(MathHelper.ToRadians(-140));
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, adjustedVelocity, ProjectileType<MiniRocket>(), RocketDamage, 2f, player.whoAmI, 0f, 0f);
                }
                if (player.CI().CICD["ReaverRangedCD"] == 129)
                {
                    int RocketDamage = (int)(0.70f * damage);
                    Vector2 adjustedVelocity = velocity.RotatedBy(MathHelper.ToRadians(140));
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, adjustedVelocity, ProjectileType<MiniRocket>(), RocketDamage, 2f, player.whoAmI, 0f, 0f);
                }
            }
        }
        public static void ProjAI_Rogue(Player player, Projectile projectile)
        {
            if (projectile.owner == Main.myPlayer && projectile.DamageType.CountsAsClass<ThrowingDamageClass>())
            {
                if (Main.player[projectile.owner].miscCounter % 60 == 0 && projectile.FinalExtraUpdate())
                {
                    int damage = (int)player.GetTotalDamage<ThrowingDamageClass>().ApplyTo(60);
                    int newProjectileId = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, Vector2.Zero, ProjectileType<TerraShard>(), damage, 0f, projectile.owner);
                    Main.projectile[newProjectileId].DamageType = DamageClass.Generic;
                }
            }
        }
    }
}
