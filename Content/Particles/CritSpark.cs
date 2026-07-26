using CalamityInheritance.Assets;
using LAP.Assets.TextureRegister;
using LAP.Core.Enums;
using LAP.Core.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace CalamityInheritance.Content.Particles
{
    public class CritSpark : BaseParticle
    {
        public override int UseBlendStateID => BlendStateID.Additive;
        private float Spin;
        private float opacity;
        private Color Bloom;
        private Color LightColor => Bloom * opacity;
        private float BloomScale;
        private float HueShift;

        public CritSpark(Vector2 position, Vector2 velocity, Color color, Color bloom, float scale, int lifeTime, float rotationSpeed = 1f, float bloomScale = 1f, float hueShift = 0f)
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
            HueShift = hueShift;
        }

        public override void Update()
        {
            opacity = (float)Math.Sin(MathHelper.PiOver2 + LifetimeRatio * MathHelper.PiOver2);
            Velocity *= 0.80f;
            Rotation += Spin * ((Velocity.X > 0) ? 1f : -1f) * (LifetimeRatio > 0.5 ? 1f : 0.5f);

            DrawColor = Main.hslToRgb((Main.rgbToHsl(DrawColor).X + HueShift) % 1, Main.rgbToHsl(DrawColor).Y, Main.rgbToHsl(DrawColor).Z);
            Bloom = Main.hslToRgb((Main.rgbToHsl(Bloom).X + HueShift) % 1, Main.rgbToHsl(Bloom).Y, Main.rgbToHsl(Bloom).Z);


            Lighting.AddLight(Position, LightColor.R / 255f, LightColor.G / 255f, LightColor.B / 255f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D sparkTexture = CIParticleTexture.ThinSparkle.Value;
            Texture2D bloomTexture = LAPTextureRegister.BloomCircle.Value;
            //Ajust the bloom's texture to be the same size as the star's
            float properBloomSize = (float)sparkTexture.Height / (float)bloomTexture.Height;

            spriteBatch.Draw(bloomTexture, Position - Main.screenPosition, null, Bloom * opacity * 0.5f, 0, bloomTexture.Size() / 2f, Scale * BloomScale * properBloomSize, SpriteEffects.None, 0);
            spriteBatch.Draw(sparkTexture, Position - Main.screenPosition, null, DrawColor * opacity, Rotation, sparkTexture.Size() / 2f, Scale, SpriteEffects.None, 0);
        }
    }
}
