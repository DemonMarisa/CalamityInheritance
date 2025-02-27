using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class ExoGunBlast : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Climax");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.arrow = false;
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.extraUpdates = 2;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            Projectile projectile = Projectile;
            projectile.frameCounter++;
            if (Projectile.frameCounter > 7)
            {
                Projectile projectile2 = Projectile;
                projectile2.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 4)
            {
                Projectile.frame = 0;
            }
            _ = Main.player[Projectile.owner];
            Projectile.spriteDirection = Projectile.direction = Utils.ToDirectionInt(Projectile.velocity.X > 0f);
            Projectile.rotation = Utils.ToRotation(Projectile.velocity) + ((Projectile.spriteDirection == 1) ? 0f : ((float)Math.PI)) + MathHelper.ToRadians(90f) * Projectile.direction;
            Dust.NewDustPerfect(Projectile.Center + Utils.RotatedBy(new Vector2(-16f, 0f), (double)(Projectile.rotation + MathHelper.ToRadians(-90f)), default(Vector2)), 111, (Vector2?)Vector2.Zero, 0, default(Color), 1f).noGravity = true;
            Lighting.AddLight(Projectile.Center, 1f, 1f, 1f);

            if(Projectile.timeLeft > 500)
            {
                Projectile.velocity *= 0.97f;
            }

            if (Projectile.timeLeft < 500)
            {
                float maxSpeed = 20f;
                float acceleration = 0.1f * 2f;
                float homeInSpeed = MathHelper.Clamp(Projectile.ai[0] += acceleration, 0f, maxSpeed);

                CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, 3500f, homeInSpeed, 15f);
            }
            if (Projectile.timeLeft == 500)
            {
                float spread = 180f * 0.0174f;
                double startAngle = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - (double)(spread / 2f);
                if (projectile.owner == Main.myPlayer && Main.player[projectile.owner].ownedProjectileCounts[projectile.type] < 200)
                {
                    int projectile2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, (float)(Math.Sin(startAngle) * 8.0), (float)(Math.Cos(startAngle) * 8.0), ModContent.ProjectileType<ExoGunBlastsplit>(), (int)(projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                    int projectile3 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, (float)((double)(0f - (float)Math.Sin(startAngle)) * 8.0), (float)((double)(0f - (float)Math.Cos(startAngle)) * 8.0), ModContent.ProjectileType<ExoGunBlastsplit>(), (int)(projectile.damage * 0.5), projectile.knockBack, projectile.owner, 0f, 0f, 0f);
                    Main.projectile[projectile2].DamageType = DamageClass.Default;
                    Main.projectile[projectile3].DamageType = DamageClass.Default;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<ExoboomoldRanged>(), Projectile.damage / 2, 0, Projectile.owner, 0f, 0f);
                }
            }
            target.immune[Projectile.owner] = 0;
            Projectile.Kill();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<ExoboomoldRanged>(), Projectile.damage / 2, 0, Projectile.owner, 0f, 0f);
            }
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 3; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<ExoboomoldRanged>(), Projectile.damage / 2, 0, Projectile.owner, 0f, 0f);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 5);
            return false;
        }
    }
}
