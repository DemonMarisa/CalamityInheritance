using CalamityInheritance.Assets.Effects;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoWeapons
{
    public class SubsumingTentacle : CIExoProj
    {
        public int AlphaFade
        {
            get => (int)Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }
        public Vector2 OffsetAcceleration
        {
            get => new Vector2(Projectile.ai[0], Projectile.ai[1]);
            set
            {
                Projectile.ai[0] = value.X;
                Projectile.ai[1] = value.Y;
            }
        }
        public const float SegmentOffset = 5f;
        public const float MaxArcingSpeed = 16f;
        public const float MaxHomingSpeed = 18f;
        public const float MaxEnemyDistance = 1450f;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 75;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.MaxUpdates = 2;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override void AI()
        {
            if (!Projectile.tileCollide)
            {
                AlphaFade++;
                Projectile.alpha = AlphaFade;
                if (Projectile.alpha >= 255)
                {
                    Projectile.Kill();
                    return;
                }
            }
            // Here, the old positions act more like "control points" than old postions, and will be referred as such henceforth.
            // Each control point should have a set offset, to give a "chain" effect.
            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                Projectile.oldPos[i] = Projectile.oldPos[i - 1] + Vector2.Normalize(Projectile.oldPos[i] - Projectile.oldPos[i - 1]) * SegmentOffset;
            }
            Projectile.Resize((int)(20f * Projectile.scale), (int)(20f * Projectile.scale));
            if (Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height) && Projectile.tileCollide)
            {
                Projectile.tileCollide = false;
            }

            NPC closestTarget = LAPUtilities.FindClosestTarget(Projectile.Center, MaxEnemyDistance);
            if (closestTarget != null)
            {
                HomingMovement(closestTarget);
            }
            else
            {
                ArcingMovement();
            }
            Projectile.scale -= closestTarget is null ? 0.014f : 0.008f;
            if (Projectile.scale <= 0.05f)
            {
                Projectile.Kill();
            }
        }
        public void ArcingMovement()
        {
            // Cause the tentacle to arc around at an increasingly fast rate.
            Projectile.velocity += OffsetAcceleration;
            if (Projectile.velocity.Length() > MaxArcingSpeed)
            {
                Projectile.velocity.Normalize();
                Projectile.velocity *= MaxArcingSpeed;
            }

            // Accelerate the arc.
            OffsetAcceleration *= 1.035f;
        }
        public void HomingMovement(NPC closestTarget)
        {
            float angleOffset = MathHelper.WrapAngle(Projectile.AngleTo(closestTarget.Center) - Projectile.velocity.ToRotation());
            angleOffset = MathHelper.Clamp(angleOffset, -0.2f, 0.2f);

            if (Projectile.Distance(closestTarget.Center) > 65f)
            {
                Projectile.velocity = Projectile.velocity.RotatedBy(angleOffset);
                Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitY) * MaxHomingSpeed;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            if (Projectile.scale < 1f)
            {
                for (int i = 10; i < Projectile.oldPos.Length; i++)
                {
                    Effect tentacleShader = CIShaders.TentacleShader.Value;
                    LAPUtilities.SetTexture(LAPTextureRegister.Noise.Value, 1);

                    Vector2 drawPos = Projectile.oldPos[i] + Request<Texture2D>(Texture).Size() / 2f - Main.screenPosition + Projectile.gfxOffY * Vector2.UnitY;
                    float scale = MathHelper.Lerp(0.05f, 1.3f, i / (float)Projectile.oldPos.Length) * Projectile.scale;
                    scale = MathHelper.Clamp(scale, 0f, 2f);
                    Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / Projectile.oldPos.Length);

                    Main.EntitySpriteDraw((Texture2D)Request<Texture2D>(Texture), drawPos, null, color, Projectile.rotation, Request<Texture2D>(Texture).Size() / 2f, scale, SpriteEffects.None, 0);

                    tentacleShader.Parameters["uColor"]?.SetValue(Vector4.One);
                    tentacleShader.Parameters["uSaturation"]?.SetValue(i / (float)Projectile.oldPos.Length);
                    tentacleShader.Parameters["uSecondaryColor"]?.SetValue(Vector3.One);
                    tentacleShader.Parameters["uTime"]?.SetValue(Main.GlobalTimeWrappedHourly);
                    tentacleShader.Parameters["uOpacity"]?.SetValue(1f / Projectile.oldPos.Length);
                    tentacleShader.Parameters["uShaderSpecificData"]?.SetValue(Vector4.Zero);
                    tentacleShader.Parameters["uSourceRect"]?.SetValue(new Vector4(0f, 0f, 4f, 4f));
                    tentacleShader.CurrentTechnique.Passes[0].Apply();
                }
            }
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                float scale = MathHelper.Lerp(0.05f, 1f, i / (float)Projectile.oldPos.Length) * Projectile.scale * 0.85f;
                if (targetHitbox.Intersects(new Rectangle((int)Projectile.oldPos[i].X, (int)Projectile.oldPos[i].Y, (int)(Projectile.width * scale), (int)(Projectile.height * scale))))
                    return true;
            }
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity;
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIMiracleBlight>(), 300);
        }
    }
}
