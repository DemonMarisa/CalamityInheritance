using CalamityInheritance.Assets;
using LAP.Core.Enums;
using LAP.Core.ParticleSystem;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;

namespace CalamityInheritance.Content.Particles
{
    public class MediumMistParticle : BaseParticle
    {
        public override int UseBlendStateID => BlendStateID.Additive;

        public int Variant;
        private Color ColorFire;
        private Color ColorFade;
        private float Spin;
        public int Alpha;
        public MediumMistParticle(Vector2 position, Vector2 velocity, Color colorFire, Color colorFade, float scale, float opacity, float rotationSpeed = 0f)
        {
            Position = position;
            Velocity = velocity;
            ColorFire = colorFire;
            ColorFade = colorFade;
            Scale = scale;
            Alpha = (int)opacity;
            Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            Spin = rotationSpeed;
            Variant = Main.rand.Next(3);
            Lifetime = 2;
            Time = 0;
        }

        public override void Update()
        {
            Rotation += Spin * ((Velocity.X > 0) ? 1f : -1f);
            Velocity *= 0.85f;

            if (Alpha > 90)
            {
                Lighting.AddLight(Position, DrawColor.ToVector3() * 0.1f);
                Scale += 0.01f;
                Alpha -= 3;
            }
            else
            {
                Scale *= 0.975f;
                Alpha -= 2;
            }

            if (Alpha > 2)
            {
                Lifetime = 2;
                Time = 0;
            }

            DrawColor = Color.Lerp(ColorFire, ColorFade, MathHelper.Clamp((float)((255 - Alpha) - 100) / 80, 0f, 1f)) * (Alpha / 255f);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = CIParticleTexture.MediumMist.Value;
            Rectangle rec = texture.Frame(1, 3, 0, Variant);
            LAPUtilities.Draw(texture, Position - Main.screenPosition, rec, DrawColor, Rotation, rec.Size() / 2, Scale, 0, 0);
        }
    }
}
