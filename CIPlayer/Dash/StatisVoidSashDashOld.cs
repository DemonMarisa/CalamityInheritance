using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Enums;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer.Dash
{
    public class StatisVoidSashDashOld : PlayerDashEffect
    {
        public static new string ID => "Statis' Void Sash Old";

        public override DashCollisionType CollisionType => DashCollisionType.NoCollision;

        public override bool IsOmnidirectional => false;
        public int Time = 0;
        public bool AngleSwap = true;

        public override float CalculateDashSpeed(Player player) => 25.4f;

        public override void OnDashEffects(Player player)
        {
            Time = 0;
        }

        public override void MidDashEffects(Player player, ref float dashSpeed, ref float dashSpeedDecelerationFactor, ref float runSpeedDecelerationFactor)
        {
            Time++;
            if (Time % 3 == 0)
            {
                Vector2 trailPos = player.Center - (player.velocity * 2) + Main.rand.NextVector2Circular(10, 20);
                float trailScale = player.velocity.X * player.direction * 0.08f;
                Color trailColor = Main.rand.NextBool(3) ? Color.Indigo : Color.DarkOrchid;
                Particle Trail = new SparkParticle(trailPos, player.velocity * 0.2f, false, 35, trailScale, trailColor);
                GeneralParticleHandler.SpawnParticle(Trail);
            }

            // Periodically release scythes.
            player.CalamityInheritance().statisTimerOld++;
            if (Main.myPlayer == player.whoAmI && player.CalamityInheritance().statisTimerOld % 5 == 0)
            {
                int scytheDamage = (int)player.GetBestClassDamage().ApplyTo(250);
                scytheDamage = player.ApplyArmorAccDamageBonusesTo(scytheDamage);

                int scythe = Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<CosmicScytheOld>(), scytheDamage, 5f, player.whoAmI); ;
                if (scythe.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[scythe].DamageType = DamageClass.Generic;
                    Main.projectile[scythe].usesIDStaticNPCImmunity = true;
                    Main.projectile[scythe].idStaticNPCHitCooldown = 10;
                }

                AngleSwap = !AngleSwap;

            }

            // Dash at a faster speed than the default value.
            dashSpeed = 14f;
        }
    }
}
