﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.Audio;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items;
using CalamityInheritance.System.Configs;
using CalamityInheritance.CIPlayer;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class ExoSpearProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public static readonly SoundStyle Hitsound = new("CalamityInheritance/Sounds/Custom/ExoApostleStealthHit") { Volume = 1.2f, PitchVariance = 0.3f };

        public override string Texture => $"{GenericProjRoute.ProjRoute}/Rogue/ExoSpearStealthProj";

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
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.extraUpdates = 4;
            Projectile.timeLeft = 720;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 16;
        }

        public void Explode(float StartAngle, int Streams, float ProjSpeed)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < Streams; i++)
                {
                    Vector2 vector = Utils.RotatedBy(Vector2.Normalize(new Vector2(1f, 1f)), (double)MathHelper.ToRadians(360 / Streams * i + StartAngle), default(Vector2));
                    vector.X *= ProjSpeed;
                    vector.Y *= ProjSpeed;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, vector.X, vector.Y, ModContent.ProjectileType<ExoSpearTrail>(), Projectile.damage / 12, 0, Main.myPlayer, 0f, 0f);
                }
            }
        }
        public override void AI()
        {
            Projectile.rotation = Utils.ToRotation(Projectile.velocity) + MathHelper.ToRadians(135f);
            if (phase == 1)
            {
                teleportticks -= 2;
                if (teleportticks < 1 && Projectile.owner == Main.myPlayer)
                {
                    Vector2 vector = new Vector2(400f, 400f);
                    vector = Utils.RotatedByRandom(vector, (double)MathHelper.ToRadians(360f));
                    Vector2 vector2 = teleportTarget.position + vector;
                    Vector2 vector3 = teleportTarget.position - vector2;
                    vector3.Normalize();
                    vector3.X *= 12f;
                    vector3.Y *= 12f;
                    for (int i = 0; i < 40; i++)
                    {
                        Dust.NewDust(vector2, 20, 20, DustID.PlatinumCoin, vector3.X / 2f, vector3.Y / 2f, 0, default(Color), 1f);
                    }
                    Projectile.position = vector2;
                    Projectile.velocity = vector3;
                    phase = 0;
                    if (Projectile.ai[1] != 0f)
                    {
                        Explode(Utils.NextFloat(Main.rand, (float)Math.PI * 2f), 4, 16f);
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
                for (int i = 0; i < 9; i++)
                {
                    float offsetRotationAngle = Projectile.velocity.ToRotation() + Time / 20f;
                    float radius = (20f + (float)Math.Cos(Time / 3f) * 12f) * radiusFactor;
                    Vector2 dustPosition = Projectile.Center;
                    dustPosition += offsetRotationAngle.ToRotationVector2().RotatedBy(i / 5f * MathHelper.TwoPi) * radius;
                    Dust dust = Dust.NewDustPerfect(dustPosition, Main.rand.NextBool() ? CIDustID.DustSandnado : 107);
                    dust.noGravity = true;
                    dust.velocity = Projectile.velocity * 0.8f;
                    dust.scale = Main.rand.NextFloat(1.1f, 1.7f);
                }
                Dust.NewDustPerfect(Projectile.Center, 247, (Vector2?)new Vector2(0f, 0f), 0, default(Color), 1f);
            }

            if (phase == 0 && Projectile.ai[1] == 1f && penetrates < 5)
            {
                Vector2 velocity = teleportTarget.position - Projectile.Center;
                velocity.Normalize();
                velocity.X *= 12f;
                velocity.Y *= 12f;
                Projectile.velocity = velocity;
                if (!teleportTarget.active)
                {
                    Projectile.Kill();
                }
            }
            Projectile.alpha = 255 - teleportticks * 8;
            Lighting.AddLight(Projectile.Center, 1f, 1f, 1f);
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>($"{GenericProjRoute.ProjRoute}/Rogue/ExoSpearStealthProjGlow").Value;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), Projectile.scale, spriteEffects, 0);
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
            Vector2 vector = Utils.Size(rectangle) / 2f;
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
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 3);
            return false;
        }
        private int hitCount = 0;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            target.immune[Projectile.owner] = 0;
            if (penetrates == 5)
            {
                teleportTarget = target;
            }

            if (hitsDust > 1)
            {
                hitsDust--;
            }

            for (int i = 0; i <= hitsDust; i++)
            {
                Vector2 sparkVelocity = Projectile.velocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.6f, 6f);
                Dust dust = Dust.NewDustPerfect(Projectile.Center + Projectile.velocity, Main.rand.NextBool(3) ? CIDustID.DustSandnado : 133, sparkVelocity, 0, default, Main.rand.NextFloat(1.2f, 1.5f));
                dust.noGravity = true;
            }
            SoundEngine.PlaySound(Hitsound, Projectile.Center);

            if (penetrates == 0)
            {
                Projectile.Kill();
            }

            Main.player[Projectile.owner].Calamity().GeneralScreenShakePower = 12;

            int numberOfProjectiles = Main.rand.Next(6, 8);
            float spreadAngle = MathHelper.ToRadians(Main.rand.Next(25, 30));
            float baseAngle = Projectile.velocity.ToRotation();

            float angleStep = spreadAngle / (numberOfProjectiles - 1);

            for (int i = 0; i < numberOfProjectiles; i++)
            {
                float randomOffset = Main.rand.NextFloat(-MathHelper.ToRadians(2), MathHelper.ToRadians(1));
                float currentAngle = baseAngle - spreadAngle / 2 + (angleStep * i) + randomOffset;
                Vector2 direction = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));

                float angleFromBase = Math.Abs(MathHelper.ToDegrees(currentAngle - baseAngle));
                float randomSpeed;
                if (angleFromBase < 1f)
                {
                    randomSpeed = Main.rand.NextFloat(55f);
                }
                if (angleFromBase < 8f)
                {
                    randomSpeed = Main.rand.NextFloat(30f,45f);
                }
                else
                {
                    randomSpeed = Main.rand.NextFloat(15f,25f);
                }

                Vector2 randomizedVelocity = direction * randomSpeed;

                int newProjectileId = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, randomizedVelocity, ModContent.ProjectileType<ExoJet>(), (int)(Projectile.damage * 0.15f), Projectile.knockBack, Projectile.owner);
            }
            target.ExoDebuffs();
            OnHitEffects(target.Center);
        }

        private void OnHitEffects(Vector2 targetPos)
        {
            hitCount++;

            if (Projectile.owner == Main.myPlayer && hitCount < 4)
            {
                var source = Projectile.GetSource_FromThis();
                float swordKB = Projectile.knockBack;
                int swordDmg = (int)(Projectile.damage * 0.25);

                int numberOfProjectiles = Main.rand.Next(6, 8);
                float spreadAngle = MathHelper.ToRadians(Main.rand.Next(25, 30));
                float baseAngle = Projectile.velocity.ToRotation();

                float angleStep = spreadAngle / (numberOfProjectiles - 1);

                for (int i = 0; i < numberOfProjectiles; i++)
                {
                    float randomOffset = Main.rand.NextFloat(-MathHelper.ToRadians(2), MathHelper.ToRadians(1));
                    float currentAngle = baseAngle - spreadAngle / 2 + (angleStep * i) + randomOffset;
                    Vector2 direction = new Vector2((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));

                    float angleFromBase = Math.Abs(MathHelper.ToDegrees(currentAngle - baseAngle));
                    float randomSpeed;
                    if (angleFromBase < 1f)
                    {
                        randomSpeed = Main.rand.NextFloat(55f);
                    }
                    if (angleFromBase < 8f)
                    {
                        randomSpeed = Main.rand.NextFloat(30f, 45f);
                    }
                    else
                    {
                        randomSpeed = Main.rand.NextFloat(15f, 25f);
                    }

                    Vector2 randomizedVelocity = direction * randomSpeed;

                    int newProjectileId = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, -randomizedVelocity, ModContent.ProjectileType<ExoSpearTrail>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner);

                }

                int randomChoice = Main.rand.Next(3);

                switch (randomChoice)
                {
                    case 0:
                        CalamityUtils.ProjectileBarrage(source, Projectile.Center, targetPos, Main.rand.NextBool(), 1000f, 1400f, 80f, 1400f, Main.rand.NextFloat(24f, 30f), ModContent.ProjectileType<ExoSpearextraProj>(), swordDmg, swordKB, Projectile.owner);
                        break;

                    case 1:
                        CalamityUtils.ProjectileRain(source, targetPos, 400f, 0f, -1500f, -800f, 25f, ModContent.ProjectileType<ExoSpearextraProj>(), swordDmg, swordKB, Projectile.owner);
                        break;

                    case 2:
                        CalamityUtils.ProjectileRain(source, targetPos, 400f, 0f, 800f, 1500f, 25f, ModContent.ProjectileType<ExoSpearextraProj>(), swordDmg, swordKB, Projectile.owner);
                        break;
                }
            }
        }
    }
}
