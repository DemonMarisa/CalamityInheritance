using CalamityInheritance.Assets;
using LAP.Core.ParticleSystem;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace CalamityInheritance.Content.Particles
{
    public class SmallSmokeParticle : BaseParticle
    {
        public Color ColorFire;
        public Color ColorFade;
        public float Spin;
        public SmallSmokeParticle(Vector2 position, Vector2 velocity, Color colorFire, Color colorFade, float scale, float opacity, float rotationSpeed = 0f)
        {
            Position = position;
            Velocity = velocity;
            ColorFire = colorFire;
            ColorFade = colorFade;
            Scale = scale;
            Opacity = opacity;
            Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            Spin = rotationSpeed;
        }

        public override void Update()
        {
            Rotation += Spin * (Velocity.X > 0 ? 1f : -1f);
            Velocity *= 0.85f;

            if (Opacity > 90)
            {
                Lighting.AddLight(Position, DrawColor.ToVector3() * 0.1f);
                Scale += 0.01f;
                Opacity -= 3;
            }
            else
            {
                Scale *= 0.975f;
                Opacity -= 2;
            }
            if (Opacity < 0)
            {
                Lifetime = 0;
                Time = 2;
            }
            else
            {
                Lifetime = 2;
                Time = 0;
            }
            DrawColor = Color.Lerp(ColorFire, ColorFade, MathHelper.Clamp((float)(255 - Opacity - 100) / 80, 0f, 1f)) * (Opacity / 255f);

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = CIParticleTexture.SmallSmoke.Value;
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, DrawColor * Opacity, Rotation, texture.Size() / 2, Scale, SpriteEffects.None, 0);
        }
    }
}
