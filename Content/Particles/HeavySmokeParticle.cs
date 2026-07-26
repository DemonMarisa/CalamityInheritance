using CalamityInheritance.Assets;
using LAP.Core.Enums;
using LAP.Core.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Particles
{
    public class HeavySmokeParticle : BaseParticle
    {
        public override int UseBlendStateID => Glowing ? BlendStateID.Additive : BlendStateID.NonPremult;

        public bool AffectedByLight;
        private float Spin;
        private bool Glowing;
        private float HueShift;
        static int FrameAmount = 6;
        public int Variant;
        public HeavySmokeParticle(Vector2 position, Vector2 velocity, Color color, int lifetime, float scale, float opacity, float rotationSpeed = 0f, bool glowing = false, float hueshift = 0f, bool required = false, bool affectedByLight = false)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = color;
            Scale = scale;
            Variant = Main.rand.Next(7);
            Lifetime = lifetime;
            Opacity = opacity;
            Spin = rotationSpeed;
            Important = required;
            Glowing = glowing;
            HueShift = hueshift;
            AffectedByLight = affectedByLight;
        }

        public override void Update()
        {
            if (Time / (float)Lifetime < 0.2f)
                Scale += 0.01f;
            else
                Scale *= 0.975f;

            DrawColor = Main.hslToRgb((Main.rgbToHsl(DrawColor).X + HueShift) % 1, Main.rgbToHsl(DrawColor).Y, Main.rgbToHsl(DrawColor).Z);
            Opacity *= 0.98f;
            Rotation += Spin * ((Velocity.X > 0) ? 1f : -1f);
            Velocity *= 0.85f;

            float opacity = Utils.GetLerpValue(1f, 0.85f, LifetimeRatio, true);
            DrawColor *= opacity;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D tex =CIParticleTexture.HeavySmoke.Value;
            int animationFrame = (int)Math.Floor(Time / ((float)(Lifetime / (float)FrameAmount)));
            Rectangle frame = new Rectangle(80 * Variant, 80 * animationFrame, 80, 80);

            Color col = DrawColor * Opacity;

            if (AffectedByLight)
            {
                col = col.MultiplyRGBA(Lighting.GetColor((Position / 16).ToPoint()));
            }

            spriteBatch.Draw(tex, Position - Main.screenPosition, frame, col, Rotation, frame.Size() / 2f, Scale, SpriteEffects.None, 0);
        }
    }
}
