using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Shortsword
{
    public class CosmicShivBallold : CIMeleeProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public NPC target = null;
        public const float maxDistanceToTarget = 1540f;
        public bool initialized = false;

        public float startingVelocityY = 0f;
        public float startingVelocityX = 0f;

        public static Vector2 startingVelocity;

        public float randomAngleDelta = 0f;
        public const float explosionDamageMultiplier = 1.8f;
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 220;
        }
        public override void AI()
        {
            if (!initialized)
            {
                target = LAPUtilities.FindClosestTarget(Projectile.Center, maxDistanceToTarget, true);
                startingVelocity = Projectile.velocity;
                randomAngleDelta = Main.rand.NextFloat(0f, (float)Math.PI * 2f);
                initialized = true;
            }
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
            {
                for (int i = 0; i < 3; i++)
                {
                    int dustID = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, Projectile.direction * 2, 0f, 115, Color.White, 1.3f);
                    Main.dust[dustID].noGravity = true;
                    Dust obj = Main.dust[dustID];
                    obj.velocity *= 0f;
                }
            }
            if (Projectile.localAI[0] % 30 == 0) // every 0.5 seconds
            {
                target = LAPUtilities.FindClosestTarget(Projectile.Center, maxDistanceToTarget, true);
            }
            if (target != null)
            {
                float inertia = 70f;
                float homingSpeed = 48f;
                Projectile.HomingTarget(target.Center, -1, homingSpeed, inertia);
            }
            else
            {
                Projectile.ai[0] += 1f;
                Projectile.position += Projectile.velocity.RotatedBy(MathHelper.PiOver2) * (float)(Math.Cos(Projectile.ai[0] / 12d + randomAngleDelta) * 7d) * 0.08f;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // for spawning the side beams
            for (int i = 0; i < 3; i++)
            {
                int directionSign = Main.rand.NextBool(2).ToDirectionInt();
                Vector2 spawnPos = new Vector2(target.Center.X + directionSign * 650, Projectile.Center.Y + Main.rand.Next(-500, 501));
                Vector2 velocity = Vector2.Normalize(target.Center - spawnPos) * 30f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos.X, spawnPos.Y, velocity.X, velocity.Y, ProjectileType<CosmicShivBladeold>(), Projectile.damage / 2, Projectile.knockBack * 0.1f, Projectile.owner);
            }
            int starMax = Main.rand.Next(6, 11); // 6 to 10 stars
            for (int i = -starMax / 2; i < starMax / 2; i++)
            {
                int ySpawnAdditive = Main.rand.Next(-40, 41);
                Vector2 toSpawn = target.Center - new Vector2(0f, 800f + ySpawnAdditive).RotatedBy(MathHelper.ToRadians(i * 11f / starMax));
                Vector2 toTarget = Vector2.Normalize(target.Center - toSpawn) * 35f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), toSpawn, toTarget, ProjectileType<CosmicStar>(), Projectile.damage / 2, Projectile.knockBack * 0.5f, Projectile.owner);
            }
            target.AddBuff(BuffType<CIGodSlayerInferno>(), 60);
        }
        public override void OnKill(int timeLeft)
        {
            // mostly from AstralCrystal kill code
            float rand2PI = Main.rand.NextFloat(MathHelper.TwoPi);
            int petalCount = 5;
            float speed = 12f;
            float scale = Main.rand.NextFloat(1f, 1.35f);
            for (float k = 0f; k < MathHelper.TwoPi; k += 0.05f)
            {
                Vector2 velocity = k.ToRotationVector2() * (2f + (float)(Math.Sin((double)(rand2PI + k * petalCount)) + 1.0) * speed) * Main.rand.NextFloat(0.95f, 1.05f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.ShadowbeamStaff, new Vector2?(velocity), 0, default, scale);
                dust.customData = 0.025f;
            }
            // CircularDamage(80f);
        }
    }
}
