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
    public class SquishyLightParticle : BaseParticle
    {
        public override int UseBlendStateID => BlendStateID.Additive;

        public float SquishStrenght;
        public float MaxSquish;
        public float HueShift;

        public SquishyLightParticle(Vector2 position, Vector2 velocity, float scale, Color color, int lifetime, float opacity = 1f, float squishStrenght = 1f, float maxSquish = 3f, float hueShift = 0f)
        {
            Position = position;
            Velocity = velocity;
            Scale = scale;
            DrawColor = color;
            Opacity = opacity;
            Rotation = 0;
            Lifetime = lifetime;
            SquishStrenght = squishStrenght;
            MaxSquish = maxSquish;
            HueShift = hueShift;
        }

        public override void Update()
        {
            Velocity *= (LifetimeRatio >= 0.34f) ? 0.93f : 1.02f;

            Opacity = LifetimeRatio > 0.5f ? (float)Math.Sin(LifetimeRatio * MathHelper.Pi) * 0.2f + 0.8f : (float)Math.Sin(LifetimeRatio * MathHelper.Pi);
            Scale *= 0.95f;

            DrawColor = Main.hslToRgb(Main.rgbToHsl(DrawColor).X + HueShift, Main.rgbToHsl(DrawColor).Y, Main.rgbToHsl(DrawColor).Z);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D tex = CIParticleTexture.Light.Value;
            Texture2D bloomTex = LAPTextureRegister.BloomCircle.Value;

            float squish = MathHelper.Clamp(Velocity.Length() / 10f * SquishStrenght, 1f, MaxSquish);

            float rot = Velocity.ToRotation() + MathHelper.PiOver2;
            Vector2 origin = tex.Size() / 2f;
            Vector2 scale = new Vector2(Scale - Scale * squish * 0.3f, Scale * squish);
            float properBloomSize = (float)tex.Height / (float)bloomTex.Height;

            Vector2 drawPosition = Position - Main.screenPosition;

            Main.spriteBatch.Draw(bloomTex, drawPosition, null, DrawColor * Opacity * 0.8f, rot, bloomTex.Size() / 2f, scale * 2 * properBloomSize, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(tex, drawPosition, null, DrawColor * Opacity * 0.8f, rot, origin, scale * 1.1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(tex, drawPosition, null, Color.White * Opacity * 0.9f, rot, origin, scale, SpriteEffects.None, 0f);

        }
    }
}
