using LAP.Assets.TextureRegister;
using LAP.Core.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using LAP.Core.Enums;

namespace CalamityInheritance.Particles
{
    public class SparkParticle2 : BaseParticle
    {
        public SparkParticle2(Vector2 position, Color color, Color bloomcolor, float scale, float bloomScale, int lifeTime)
        {
            Position = position;
            DrawColor = color;
            BloomColor = bloomcolor;
            Scale = scale;
            BloomScale = bloomScale;
            Lifetime = lifeTime;
            Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        }
        public override int UseBlendStateID => BlendStateID.Additive;
        public Color LightColor => BloomColor * Opacity;
        public Color BloomColor;
        public float BloomScale;
        public float RotSpeed;
        public override void OnSpawn()
        {
            RotSpeed = 0f;
        }
        public override void Update()
        {
            Opacity = (float)Math.Sin(LifetimeRatio * MathHelper.Pi);
            Lighting.AddLight(Position, LightColor.R / 255f, LightColor.G / 255f, LightColor.B / 255f);
            Velocity *= 0.95f;
            Rotation += RotSpeed * ((Velocity.X > 0) ? 1f : -1f);
        }
        public override void OnKill()
        {
            base.OnKill();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D starTexture = LAPTextureRegister.Sparkle.Value;
            Texture2D bloomTexture = LAPTextureRegister.BloomCircle.Value;
            //Ajust the bloom's texture to be the same size as the star's
            float properBloomSize = (float)starTexture.Height / (float)bloomTexture.Height;

            spriteBatch.Draw(bloomTexture, Position - Main.screenPosition, null, BloomColor * Opacity * 0.5f, 0, bloomTexture.Size() / 2f, Scale * BloomScale * properBloomSize, SpriteEffects.None, 0);
            spriteBatch.Draw(starTexture, Position - Main.screenPosition, null, DrawColor * Opacity * 0.5f, Rotation + MathHelper.PiOver4, starTexture.Size() / 2f, Scale * 0.75f, SpriteEffects.None, 0);
            spriteBatch.Draw(starTexture, Position - Main.screenPosition, null, DrawColor * Opacity, Rotation, starTexture.Size() / 2f, Scale, SpriteEffects.None, 0);
        }
    }
}
