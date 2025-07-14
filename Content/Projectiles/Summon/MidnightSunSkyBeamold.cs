using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Summon
{
    public class MidnightSunSkyBeamold : ModProjectile, ILocalizedModType 
    {
        public new string LocalizationCategory => "Content.Projectiles.Summon";
        // How long this laser can exist before it is deleted.
        public const int TrueTimeLeft = 180;

        // Pretty self explanatory
        private const float maximumLength = 2000f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 10000;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = TrueTimeLeft;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 30;
            Projectile.DamageType = DamageClass.Summon;
        }

        // Netcode for sending and receiving shit
        // localAI[0] is the timer, localAI[1] is the laser length

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
        }

        public override void AI()
        {
            Projectile body = Main.projectile[(int)Projectile.ai[0]];
            if (body.type != ModContent.ProjectileType<MidnightSunBeaconProjold>() || !body.active)
                Projectile.Kill();

            if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
            {
                Projectile.velocity = -Vector2.UnitY;
            }

            if (body.active)
            {
                float angleVarianceRatio = Projectile.localAI[0] / TrueTimeLeft;
                float angleVariance = MathHelper.TwoPi * (Projectile.ai[1] - 2f) / 2f * angleVarianceRatio * 1.25f;
                float xDelta = 45f * (float)Math.Sin(angleVariance) * angleVarianceRatio;
                Projectile.Center = body.Center - new Vector2(xDelta, maximumLength / 2f);
            }

            if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
            {
                Projectile.velocity = -Vector2.UnitY;
            }

            // How fat the laser is
            float laserSize = 1f;

            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] >= TrueTimeLeft)
            {
                Projectile.Kill();
                return;
            }

            // Causes the effect where the laser appears to expand/contract at the beginning and end of its life
            Projectile.scale = (float)Math.Sin(Projectile.localAI[0] * MathHelper.Pi / TrueTimeLeft) * 10f * laserSize;
            if (Projectile.scale > laserSize)
            {
                Projectile.scale = laserSize;
            }

            Projectile.rotation = 0f;
            Projectile.velocity = Vector2.UnitY;

            Vector2 samplingPoint = Projectile.Center;

            float[] samples = new float[3];

            float determinedLength = 0f;
            Collision.LaserScan(samplingPoint, Projectile.velocity, Projectile.width * Projectile.scale, maximumLength, samples);
            for (int i = 0; i < samples.Length; i++)
            {
                determinedLength += samples[i];
            }
            determinedLength /= samples.Length;

            float lerpDelta = 0.5f;
            Projectile.localAI[1] = MathHelper.Lerp(Projectile.localAI[1], determinedLength, lerpDelta);
            Vector2 beamEndPosiiton = Projectile.Center + Projectile.velocity * (Projectile.localAI[1] - 6f);
            for (int i = 0; i < 4; i++)
            {
                float theta = Projectile.velocity.ToRotation() + Main.rand.NextBool(2).ToDirectionInt() * MathHelper.PiOver2;
                float speed = (float)Main.rand.NextDouble() * 2f + 2f;
                Vector2 velocity = theta.ToRotationVector2() * speed;
                Dust dust = Dust.NewDustDirect(beamEndPosiiton, 0, 0, DustID.FrostHydra, velocity.X, velocity.Y, 0, default, 1f);
                dust.noGravity = true;
                dust.scale = 1.7f;
            }

            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustPerfect(beamEndPosiiton, 185);
                dust.velocity = Vector2.UnitY.RotatedByRandom(MathHelper.ToRadians(55f)).RotatedBy(Projectile.rotation);
                dust.noGravity = true;
                dust.scale = 1.2f;
            }

            // Draw light blue light across the laser
            DelegateMethods.v3_1 = new Vector3(0.52f, 0.93f, 0.97f);
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.localAI[1], Projectile.width * Projectile.scale, DelegateMethods.CastLight);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.localAI[1] == 0f)
            {
                Projectile.localAI[1] = 2000f;
            }

            Texture2D laserTailTexture = ModContent.Request<Texture2D>("CalamityInheritance/ExtraTextures/Laser/MidnightSunBeamBegin").Value;
            Texture2D laserBodyTexture = ModContent.Request<Texture2D>("CalamityInheritance/ExtraTextures/Laser/MidnightSunBeamMid").Value;
            Texture2D laserHeadTexture = ModContent.Request<Texture2D>("CalamityInheritance/ExtraTextures/Laser/MidnightSunBeamEnd").Value;

            float laserLength = Projectile.localAI[1];
            Color drawColor = Color.White * 0.9f;

            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(laserTailTexture, drawPosition, null, drawColor, Projectile.rotation, laserTailTexture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);

            laserLength -= (laserTailTexture.Height / 2 + laserHeadTexture.Height) * Projectile.scale;
            Vector2 centerDelta = Projectile.Center + Projectile.velocity * Projectile.scale * laserTailTexture.Height / 2f;
            centerDelta -= Main.screenPosition;

            if (laserLength > 0f)
            {
                float laserLengthDelta = 0f;
                Rectangle sourceRectangle = new Rectangle(0, 16 * (Projectile.timeLeft / 3 % 5), laserBodyTexture.Width, 16);

                while (laserLengthDelta + 1f < laserLength)
                {
                    if (laserLength - laserLengthDelta < sourceRectangle.Height)
                    {
                        sourceRectangle.Height = (int)(laserLength - laserLengthDelta);
                    }

                    Main.EntitySpriteDraw(laserBodyTexture, centerDelta, new Rectangle?(sourceRectangle),drawColor,Projectile.rotation,new Vector2(sourceRectangle.Width / 2f, 0f), Projectile.scale, SpriteEffects.None,0f);

                    laserLengthDelta += sourceRectangle.Height * Projectile.scale;
                    centerDelta += Projectile.velocity * sourceRectangle.Height * Projectile.scale;
                    sourceRectangle.Y += 16;

                    if (sourceRectangle.Y + sourceRectangle.Height > laserBodyTexture.Height)
                    {
                        sourceRectangle.Y = 0;
                    }
                }
            }

            Main.EntitySpriteDraw(laserHeadTexture, centerDelta, null,drawColor,Projectile.rotation,laserHeadTexture.Frame(1, 1, 0, 0).Top(), Projectile.scale,SpriteEffects.None,0f);

            return false;
        }
        
        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = Projectile.velocity;
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + unit * Projectile.localAI[1], Projectile.width * Projectile.scale, DelegateMethods.CutTiles);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }
            float value = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.localAI[1], 22f * Projectile.scale, ref value))
            {
                return true;
            }
            return false;
        }
        public override bool ShouldUpdatePosition() => false;
    }
}
