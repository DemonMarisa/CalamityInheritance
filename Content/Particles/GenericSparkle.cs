using LAP.Assets.TextureRegister;
using LAP.Core.Enums;
using LAP.Core.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Particles
{
    public class GenericSparkle : BaseParticle
    {
        public override int UseBlendStateID => BlendStateID.Additive;

        public bool imporant;
        private float Spin;
        private float opacity;
        private Color Bloom;
        private Color LightColor => Bloom * opacity;
        private float BloomScale;

        public GenericSparkle(Vector2 position, Vector2 velocity, Color color, Color bloom, float scale, int lifeTime, float rotationSpeed = 1f, float bloomScale = 1f, bool needed = false)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = color;
            Bloom = bloom;
            Scale = scale;
            Lifetime = lifeTime;
            Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            Spin = rotationSpeed;
            BloomScale = bloomScale;
            imporant = needed;
        }

        public override void Update()
        {
            opacity = (float)Math.Sin(LifetimeRatio * MathHelper.Pi);
            Lighting.AddLight(Position, LightColor.R / 255f, LightColor.G / 255f, LightColor.B / 255f);
            Velocity *= 0.95f;
            Rotation += Spin * ((Velocity.X > 0) ? 1f : -1f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D starTexture = LAPTextureRegister.Sparkle.Value;
            Texture2D bloomTexture = LAPTextureRegister.BloomCircle.Value;
            //Ajust the bloom's texture to be the same size as the star's
            float properBloomSize = (float)starTexture.Height / (float)bloomTexture.Height;

            spriteBatch.Draw(bloomTexture, Position - Main.screenPosition, null, Bloom * opacity * 0.5f, 0, bloomTexture.Size() / 2f, Scale * BloomScale * properBloomSize, SpriteEffects.None, 0);
            spriteBatch.Draw(starTexture, Position - Main.screenPosition, null, DrawColor * opacity * 0.5f, Rotation + MathHelper.PiOver4, starTexture.Size() / 2f, Scale * 0.75f, SpriteEffects.None, 0);
            spriteBatch.Draw(starTexture, Position - Main.screenPosition, null, DrawColor * opacity, Rotation, starTexture.Size() / 2f, Scale, SpriteEffects.None, 0);
        }
    }
}
