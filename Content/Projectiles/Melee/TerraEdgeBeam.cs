using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using CalamityMod.Balancing;
using CalamityMod.Projectiles.Healing;
using CalamityMod.Projectiles;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class TerraEdgeBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 27;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 1;
            AIType = 132;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, (255 - Projectile.alpha) * 1.3f / 255f, 0f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 200, 0, 0);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[base.Projectile.owner];
            target.AddBuff(BuffID.Ichor, 60);
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TerraBlade,
            new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) },
            Projectile.owner);

            // You could also spawn dusts at the enemy position. Here is simple an example:
            int dustIndex = Terraria.Dust.NewDust(Main.rand.NextVector2FromRectangle(target.Hitbox), 0, 0, DustID.Terra);
            Main.dust[dustIndex].noGravity = true;

            // Set the target's hit direction to away from the player so the knockback is in the correct direction.
            hit.HitDirection = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);
            
            if (target.type != NPCID.TargetDummy && target.canGhostHeal && !player.moonLeech)
            {
                int healAmount = Main.rand.Next(2) + 2;
                player.statLife += healAmount;
                player.HealEffect(healAmount);
            }
        }

        [Obsolete]
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            int num3;
            for (int num795 = 4; num795 < 31; num795 = num3 + 1)
            {
                float num796 = Projectile.oldVelocity.X * (30f / (float)num795);
                float num797 = Projectile.oldVelocity.Y * (30f / (float)num795);
                int num798 = Terraria.Dust.NewDust(new Vector2(Projectile.oldPosition.X - num796, Projectile.oldPosition.Y - num797), 8, 8, DustID.TerraBlade, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.8f);
                Main.dust[num798].noGravity = true;
                Terraria.Dust dust = Main.dust[num798];
                dust.velocity *= 0.5f;
                num798 = Terraria.Dust.NewDust(new Vector2(Projectile.oldPosition.X - num796, Projectile.oldPosition.Y - num797), 8, 8, DustID.TerraBlade, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.4f);
                dust = Main.dust[num798];
                dust.velocity *= 0.05f;
                num3 = num795;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft == 600)
            {
                return false;
            }
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
