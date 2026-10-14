using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Core.Misc;
using LAP.Core.SpecificEffectManagers;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoWeapons.ExoTheApostle
{
    public class ExoSpearextraProj : CIExoProj
    {
        public static readonly SoundStyle Hitsound = CISounds.RealityRuptureStealthHit;
        public override string Texture => GetInstance<ExoSpearProj>().Texture;

        private int increment;

        private int phase;

        private NPC teleportTarget;

        private int penetrates = 5;

        private int teleportticks = 32;

        public int Time = 0;

        public int hitsDust = 50;
        public ref float Timer => ref Projectile.ai[0];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.arrow = false;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = RogueDamage.Instance;
            Projectile.extraUpdates = 4;
            Projectile.timeLeft = 180;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 16;
        }

        public void Explode(float StartAngle, int Streams, float ProjSpeed)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < Streams; i++)
                {
                    Vector2 vector = Vector2.Normalize(new Vector2(1f, 1f)).RotatedBy((double)MathHelper.ToRadians(360 / Streams * i + StartAngle), default);
                    vector.X *= ProjSpeed;
                    vector.Y *= ProjSpeed;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, vector.X, vector.Y, ProjectileType<ExoSpearTrail>(), Projectile.damage / 12, 0, Main.myPlayer, 0f, 0f);
                }
            }
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
            if (phase == 1)
            {
                teleportticks -= 2;
                if (teleportticks < 1 && Projectile.owner == Main.myPlayer)
                {
                    Vector2 vector = new Vector2(400f, 400f);
                    vector = vector.RotatedByRandom((double)MathHelper.ToRadians(360f));
                    Vector2 vector2 = teleportTarget.position + vector;
                    Vector2 vector3 = teleportTarget.position - vector2;
                    vector3.Normalize();
                    vector3.X *= 12f;
                    vector3.Y *= 12f;
                    for (int i = 0; i < 40; i++)
                    {
                        Dust.NewDust(vector2, 20, 20, DustID.PlatinumCoin, vector3.X / 2f, vector3.Y / 2f, 0, default, 1f);
                    }
                    Projectile.position = vector2;
                    Projectile.velocity = vector3;
                    phase = 0;
                    if (Projectile.ai[1] != 0f)
                    {
                        Explode(Main.rand.NextFloat((float)Math.PI * 2f), 4, 16f);
                    }
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                increment++;
                teleportticks++;
            }

            if (penetrates >= 5)
            {
                Timer++;
                Time++;
                Lighting.AddLight(Projectile.Center + Projectile.velocity * 0.6f, 0.6f, 0.2f, 0.9f);
                float radiusFactor = MathHelper.Lerp(0f, 1f, Utils.GetLerpValue(10f, 50f, Time, true));
                for (int i = 0; i < 4; i++)
                {
                    float offsetRotationAngle = Projectile.velocity.ToRotation() + Time / 20f;
                    float radius = (5f + (float)Math.Cos(Time / 3f) * 12f) * radiusFactor;
                    Vector2 dustPosition = Projectile.Center;
                    dustPosition += offsetRotationAngle.ToRotationVector2().RotatedBy(i / 5f * MathHelper.TwoPi) * radius;
                    Dust dust = Dust.NewDustPerfect(dustPosition, Main.rand.NextBool() ? CIDustID.DustSandnado : 107);
                    dust.noGravity = true;
                    dust.velocity = Projectile.velocity * 0.8f;
                    dust.scale = Main.rand.NextFloat(1.1f, 1.7f);
                }
                Dust.NewDustPerfect(Projectile.Center, DustID.PlatinumCoin, (Vector2?)new Vector2(0f, 0f), 0, default, 1f);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
            _ = TextureAssets.Projectile[Projectile.type].Value.Height;
            Rectangle rectangle = new Rectangle(0, 0, texture2D.Width, texture2D.Height);
            Vector2 vector = rectangle.Size() / 2f;
            vector.X = 28f;
            vector.Y = 35f;
            Color alpha = Projectile.GetAlpha(lightColor);
            Main.spriteBatch.Draw(texture2D, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), rectangle, alpha, Projectile.rotation, vector, Projectile.scale, effects, 0f);
            Vector2 origin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, TextureAssets.Projectile[Projectile.type].Value.Height * 0.5f);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 position = Projectile.oldPos[i] - Main.screenPosition + vector;
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, position, default(Rectangle), color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
            }
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 3);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 0;
            if (penetrates == 5)
            {
                teleportTarget = target;
            }
            /*
            //将穿透改为击中消失弹幕
            if (target == teleportTarget)
            {
                penetrates--;//取消弹幕本体的粒子
                teleportticks = 32;
                phase = 1;
                for (int i = 0; i < 80; i++)
                {
                    Dust.NewDust(Projectile.position, 20, 20, DustID.PlatinumCoin, Projectile.velocity.X, Projectile.velocity.Y / 4f, 0, default(Color), 1f);
                }
            }
            */
            if (hitsDust > 1)
            {
                hitsDust--;
            }

            for (int i = 0; i <= hitsDust; i++)
            {
                Vector2 sparkVelocity = Projectile.velocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.3f, 3f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center + Projectile.velocity, Main.rand.NextBool(3) ? CIDustID.DustSandnado : 133, sparkVelocity, 0, default, Main.rand.NextFloat(1.2f, 1.5f));
                dust.noGravity = true;
            }

            if (penetrates == 0)
            {
                Projectile.Kill();
            }

            ScreenShakeSystem.AddScreenShakes(Projectile.Center, 6, 6, 0);

            int numberOfProjectiles = Main.rand.Next(8, 12);
            float spreadAngle = MathHelper.ToRadians(Main.rand.Next(25, 30));
            float baseAngle = Projectile.velocity.ToRotation();

            float angleStep = spreadAngle / (numberOfProjectiles - 1);

            for (int i = 0; i < numberOfProjectiles; i++)
            {
                float randomOffset = Main.rand.NextFloat(-MathHelper.ToRadians(2), MathHelper.ToRadians(1));
                float currentAngle = baseAngle - spreadAngle / 2 + angleStep * i + randomOffset;
                Vector2 direction = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));

                float angleFromBase = Math.Abs(MathHelper.ToDegrees(currentAngle - baseAngle));
                float randomSpeed;
                if (angleFromBase < 1f)
                {
                    randomSpeed = Main.rand.NextFloat(45f);
                }
                if (angleFromBase < 8f)
                {
                    randomSpeed = Main.rand.NextFloat(30f, 40f);
                }
                else
                {
                    randomSpeed = Main.rand.NextFloat(15f, 25f);
                }

                Vector2 randomizedVelocity = direction * randomSpeed;

                int newProjectileId = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, randomizedVelocity, ProjectileType<ExoJet>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner);
            }
        }
    }
}
