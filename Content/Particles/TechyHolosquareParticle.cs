using CalamityInheritance.Assets;
using CalamityInheritance.Core.Utils;
using LAP.Core.Enums;
using LAP.Core.ParticleSystem;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace CalamityInheritance.Content.Particles
{
    public class TechyHolosquareParticle : BaseParticle
    {
        public override int UseBlendStateID => BlendStateID.Additive;
        internal Rectangle Frame;
        public float OpacityMult;
        public int Variant;
        public float BeginScale;
        public TechyHolosquareParticle(Vector2 position, Vector2 speed, float scale, Color color, int lifetime, float opacity = 1f)
        {
            Position = position;
            Scale = scale;
            BeginScale = scale;
            DrawColor = color;
            Velocity = speed;
            Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            Opacity = opacity;
            OpacityMult = opacity;
            Variant = Main.rand.Next(6);
            Lifetime = lifetime;

            switch (Variant)
            {
                case 0:
                    Frame = new Rectangle(8, 0, 6, 6);
                    break;
                case 1:
                    Frame = new Rectangle(6, 8, 10, 6);
                    break;
                case 2:
                    Frame = new Rectangle(4, 16, 14, 8);
                    break;
                case 3:
                    Frame = new Rectangle(2, 26, 18, 10);
                    break;
                case 4:
                    Frame = new Rectangle(2, 38, 18, 8);
                    break;
                case 5:
                    Frame = new Rectangle(6, 48, 12, 12);
                    break;
            }
        }

        public override void Update()
        {
            Opacity = (float)Math.Pow(LifetimeRatio, 0.5f) * OpacityMult;
            Lighting.AddLight(Position, DrawColor.ToVector3() * Opacity);
            Rotation = Velocity.ToRotation();

            Velocity *= 0.875f;
            Scale = MathHelper.Lerp(BeginScale, 0f, EasingHelper.EaseInCubic(LifetimeRatio));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D baseTex = CIParticleTexture.TechyHolosquare.Value;
            for (int i = -1; i <= 1; i++)
            {
                Color colorMult = Color.White;
                switch (i)
                {
                    case -1:
                        colorMult = new Color(255, 0, 0, 0);
                        break;
                    case 0:
                        colorMult = new Color(0, 255, 0, 0);
                        break;
                    case 1:
                        colorMult = new Color(0, 0, 255, 0);
                        break;
                }

                Vector2 offset = Vector2.UnitX.RotatedBy(Rotation).RotatedBy(1.5707963705062866) * i;
                offset *= 1.5f;
                spriteBatch.Draw(baseTex, Position + offset - Main.screenPosition, Frame, CIUtils.MultiplyRGB(DrawColor, colorMult) * Opacity, Rotation, Frame.Size() / 2, Scale / 4f, SpriteEffects.None, 0);
            }
        }
    }
}
