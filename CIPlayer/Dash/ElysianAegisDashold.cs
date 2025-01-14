using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Enums;
using CalamityMod.Items.Accessories;
using CalamityMod.Projectiles.Typeless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod;
using CalamityMod.Particles;
using Terraria.ID;

namespace CalamityInheritance.CIPlayer.Dash
{
    public class ElysianAegisDashold : PlayerDashEffect
    {
        public static new string ID => "Elysian Aegis old";

        public override DashCollisionType CollisionType => DashCollisionType.ShieldSlam;

        public override bool IsOmnidirectional => false;

        public override float CalculateDashSpeed(Player player) => 21.5f;

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
                Dust hFlameDust = Dust.NewDustPerfect(player.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-15f, 15f)) - (player.velocity * 1.2f), Main.rand.NextBool(8) ? 222 : 162, -player.velocity.RotatedByRandom(MathHelper.ToRadians(10f)) * Main.rand.NextFloat(0.1f, 0.8f), 0, default, Main.rand.NextFloat(1.8f, 2.8f));
                hFlameDust.shader = GameShaders.Armor.GetSecondaryShader(player.cShield, player);
                hFlameDust.noGravity = hFlameDust.type == 222 ? false : true;
                hFlameDust.fadeIn = 0.5f;
                if (hFlameDust.type == 222)
                {
                    hFlameDust.scale = Main.rand.NextFloat(0.8f, 1.2f);
                    hFlameDust.velocity += new Vector2(0, -2.5f) * Main.rand.NextFloat(0.8f, 1.2f);
                }
                if (hFlameDust.type == 180)
                    hFlameDust.scale = Main.rand.NextFloat(1.6f, 2.2f);

                Dust dust = Dust.NewDustPerfect(player.Center + Main.rand.NextVector2Circular(6, 6) - player.velocity * 2, 228);
                dust.velocity = -player.velocity * Main.rand.NextFloat(0.6f, 1.4f);
                dust.scale = Main.rand.NextFloat(0.9f, 1.4f);
                dust.noGravity = true;
            }

            // Dash at a faster speed than the default value.
            dashSpeed = 14f;
        }

        public override void OnHitEffects(Player player, NPC npc, IEntitySource source, ref DashHitContext hitContext)
        {
            float particleScale = Main.rand.NextFloat(0.45f, 0.55f);

            float kbFactor = 12f;
            bool crit = Main.rand.Next(100) < player.GetCritChance<MeleeDamageClass>();
            if (player.kbGlove)
                kbFactor *= 2f;
            if (player.kbBuff)
                kbFactor *= 1.5f;

            int hitDirection = player.direction;

            if (player.velocity.X != 0f)
                hitDirection = Math.Sign(player.velocity.X);
            hitContext.HitDirection = hitDirection;

            hitContext.PlayerImmunityFrames = ElysianAegis.ShieldSlamIFrames;

            // Define damage parameters.
            int dashDamage = ElysianAegis.ShieldSlamDamage;
            hitContext.damageClass = DamageClass.Melee;
            hitContext.BaseDamage = player.ApplyArmorAccDamageBonusesTo(dashDamage);
            hitContext.BaseKnockback = ElysianAegis.ShieldSlamKnockback;

            int supremeExplosionDamage = (int)player.GetBestClassDamage().ApplyTo(ElysianAegis.RamExplosionDamage);
            supremeExplosionDamage = player.ApplyArmorAccDamageBonusesTo(supremeExplosionDamage);
            Projectile.NewProjectile(source, player.Center, Vector2.Zero, ModContent.ProjectileType<HolyExplosionSupreme>(), supremeExplosionDamage, ElysianAegis.RamExplosionKnockback, Main.myPlayer, 1f, 0f);
            npc.AddBuff(ModContent.BuffType<HolyFlames>(), 300);
        }
    }
}
