using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Enums;
using CalamityMod.Items.Accessories;
using CalamityMod.Projectiles.Typeless;
using System;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace CalamityInheritance.CIPlayer.Dash
{
    public class AsgardsValorDashold : PlayerDashEffect
    {
        public static new string ID => "Asgard's Valor old";

        public override DashCollisionType CollisionType => DashCollisionType.ShieldSlam;

        public override bool IsOmnidirectional => false;

        public override float CalculateDashSpeed(Player player) => 16.9f;

        public override void OnDashEffects(Player player)
        {
            // Spawn fire dust around the player's body.
            for (int d = 0; d < 20; d++)
            {
                Dust holyFireDashDust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.GoldCoin, 0f, 0f, 100, default, 3f);
                holyFireDashDust.position += Main.rand.NextVector2Square(-5f, 5f);
                holyFireDashDust.velocity *= 0.2f;
                holyFireDashDust.scale *= Main.rand.NextFloat(1f, 1.2f);
                holyFireDashDust.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                holyFireDashDust.noGravity = true;
                holyFireDashDust.fadeIn = 0.5f;
            }
        }

        public override void MidDashEffects(Player player, ref float dashSpeed, ref float dashSpeedDecelerationFactor, ref float runSpeedDecelerationFactor)
        {
            // Spawn fire dust around the player's body.
            for (int d = 0; d < 4; d++)
            {
                Dust holyFireDashDust = Dust.NewDustDirect(player.position + Vector2.UnitY * 4f, player.width, player.height - 8, Main.rand.NextBool() ? 296 : 158, 0f, 0f, 0, default, 1.2f);
                holyFireDashDust.velocity = -player.velocity * Main.rand.NextFloat(0.1f, 0.75f);
                holyFireDashDust.scale *= Main.rand.NextFloat(1f, 1.2f);
                holyFireDashDust.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                holyFireDashDust.noGravity = true;
                if (Main.rand.NextBool())
                    holyFireDashDust.fadeIn = 0.1f;
            }
            if (Main.rand.NextBool(3))
            {
                Vector2 dustPosition = player.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-15f, 15f)) - (player.velocity * 1.7f);
                Dust dust = Dust.NewDustPerfect(dustPosition, 222, -player.velocity * Main.rand.NextFloat(0.15f, 0.4f), 0, default, 0.5f);
                dust.noGravity = false;
                dust.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
            }
        }

        public override void OnHitEffects(Player player, NPC npc, IEntitySource source, ref DashHitContext hitContext)
        {
            // Define hit context variables.
            int hitDirection = player.direction;
            if (player.velocity.X != 0f)
                hitDirection = Math.Sign(player.velocity.X);
            hitContext.HitDirection = hitDirection;
            hitContext.PlayerImmunityFrames = AsgardsValor.ShieldSlamIFrames;

            // Define damage parameters.
            int dashDamage = AsgardsValor.ShieldSlamDamage;
            hitContext.damageClass = DamageClass.Melee;
            hitContext.BaseDamage = player.ApplyArmorAccDamageBonusesTo(dashDamage);
            hitContext.BaseKnockback = AsgardsValor.ShieldSlamKnockback;

            int Dusts = 12;
            float radians = MathHelper.TwoPi / Dusts;
            Vector2 spinningPoint = Vector2.Normalize(new Vector2(-1f, -1f));
            for (int k = 0; k < Dusts; k++)
            {
                Vector2 velocity = spinningPoint.RotatedBy(radians * k);
                Dust dust = Dust.NewDustPerfect(npc.Center, 296, velocity * 6f, 0, default, 2.5f);
                dust.noGravity = true;
                dust.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);

                Dust dust2 = Dust.NewDustPerfect(npc.Center, 158, velocity * 10f, 0, default, 2.2f);
                dust2.noGravity = true;
                dust2.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                dust2.color = Color.Salmon;

                Dust dust3 = Dust.NewDustPerfect(npc.Center, 169, velocity * 14f, 0, default, 1.9f);
                dust3.noGravity = true;
                dust3.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                dust3.color = Color.SandyBrown;

            }

            int holyExplosionDamage = (int)player.GetBestClassDamage().ApplyTo(60);
            Projectile.NewProjectile(source, player.Center, Vector2.Zero, ModContent.ProjectileType<HolyExplosion>(), holyExplosionDamage, 15f, Main.myPlayer, 0f, 0f);
            npc.AddBuff(ModContent.BuffType<HolyFlames>(), 180);
        }
    }
}
