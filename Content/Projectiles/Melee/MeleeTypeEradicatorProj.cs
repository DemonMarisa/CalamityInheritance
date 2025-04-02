using System;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Content.Items.Weapons;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    [LegacyName("EradicatorProjectileLegacyMelee")]
    public class MeleeTypeEradicatorProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => $"{Generic.WeaponRoute}/Melee/MeleeTypeEradicator";
        public static readonly float ChasingRange = 60000f;
        public static readonly float ChasingSpeed = 24f;
        private static float RotationIncrement = 0.15f;
        private static int Lifetime = 350;
        private static int ReboundTime = 60;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 62;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 2;
            Projectile.timeLeft = Lifetime;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 3;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == Lifetime - ReboundTime)
                Projectile.netUpdate = true;

            if (Projectile.timeLeft <= Lifetime - ReboundTime)
            {
                float returnSpeed = Eradicator.Speed * 1.3f;
                float acceleration = 0.25f;
                Player owner = Main.player[Projectile.owner];
                CIFunction.BoomerangReturningAI(owner, Projectile, returnSpeed, acceleration);
                if (Main.myPlayer == Projectile.owner)
                    if (Projectile.Hitbox.Intersects(owner.Hitbox))
                        Projectile.Kill();
            }

            Lighting.AddLight(Projectile.Center, 0.35f, 0f, 0.25f);

            float spin = Projectile.direction <= 0 ? -1f : 1f;
            Projectile.rotation += spin * RotationIncrement;
            
            double laserDamageRatio = 0.8D;
            float laserFrames = Projectile.MaxUpdates * 6f;
            CalamityUtils.MagnetSphereHitscan(Projectile,
                                                ChasingRange,
                                                ChasingSpeed,
                                                laserFrames,
                                                2,
                                                ModContent.ProjectileType<NebulaShot>(),
                                                laserDamageRatio,
                                                true);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 180);

            // Spawn sparks; taken from Despair stone then adapted to a projectile
            Vector2 particleSpawnDisplacement;
            Vector2 splatterDirection;

            particleSpawnDisplacement = new Vector2(2f * -Projectile.ai[2], 2f * -Projectile.ai[2]);
            splatterDirection = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);

            Vector2 SparkSpawnPosition = target.Center + particleSpawnDisplacement;

            if (Projectile.ai[1] % 4 == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    int sparkLifetime = Main.rand.Next(14, 21);
                    float sparkScale = Main.rand.NextFloat(0.8f, 1f) + 1f * 0.05f;
                    Color sparkColor = Color.Lerp(Color.Fuchsia, Color.AliceBlue, Main.rand.NextFloat(0.5f));
                    sparkColor = Color.Lerp(sparkColor, Color.Cyan, Main.rand.NextFloat());

                    if (Main.rand.NextBool(5))
                        sparkScale *= 1.4f;

                    Vector2 sparkVelocity = splatterDirection.RotatedByRandom(MathHelper.TwoPi);
                    sparkVelocity.Y -= 6f;
                    SparkParticle spark = new SparkParticle(SparkSpawnPosition, sparkVelocity, true, sparkLifetime, sparkScale, sparkColor);
                    GeneralParticleHandler.SpawnParticle(spark);
                }
            }

        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 180);

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Vector2 origin = new Vector2(31f, 29f);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>($"{Generic.WeaponRoute}/Melee/MeleeTypeEradicatorGlow").Value,
                                  Projectile.Center - Main.screenPosition,
                                  null,
                                  Color.White,
                                  Projectile.rotation,
                                  origin,
                                  1f,
                                  SpriteEffects.None,
                                  0);
        }
    }
}
