using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Items.Weapons.ExoWeapons;
using LAP.Core.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoWeapons
{
    public class ExoGunBlastsplit : CIExoProj
    {
        public override string Texture => GetInstance<ExoGunBlast>().Texture;

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
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.CI().CanSplit = false;
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
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0f).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : (float)Math.PI) + MathHelper.ToRadians(90f) * Projectile.direction;
            Dust.NewDustPerfect(Projectile.Center + new Vector2(-16f, 0f).RotatedBy((double)(Projectile.rotation + MathHelper.ToRadians(-90f)), default), DustID.HallowSpray, (Vector2?)Vector2.Zero, 0, default, 1f).noGravity = true;
            Lighting.AddLight(Projectile.Center, 1f, 1f, 1f);

            if (Projectile.timeLeft > 500)
            {
                Projectile.velocity *= 0.97f;
            }
            if (Projectile.timeLeft < 500)
            {
                float maxSpeed = 20f;
                float acceleration = 0.02f * 12f;
                float homeInSpeed = MathHelper.Clamp(Projectile.ai[0] += acceleration, 0f, maxSpeed);
                Projectile.extraUpdates = 3;
                Projectile.HomeInNPC(1500f, homeInSpeed, 15f, 10f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ProjectileType<ExoboomoldRanged>(), Projectile.damage / 2, 0, Projectile.owner, 1f, 0f);
                Main.projectile[p].DamageType = DamageClass.Ranged;
                Main.projectile[p].netUpdate = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 5);
            return false;
        }
    }
}
