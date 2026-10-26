using LAP.Assets.TextureRegister;
using LAP.Core.ParticleSystem;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Particles
{
    public class SparkParticle : BaseParticle
    {
        public Color InitialColor;
        public bool AffectedByGravity;
        public bool FadeIn = false;
        public float FadeInScale = 0f;

        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public SparkParticle(Vector2 relativePosition, Vector2 velocity, bool affectedByGravity, int lifetime, float scale, Color color, bool fadeIn = false)
        {
            Position = relativePosition;
            Velocity = velocity;
            AffectedByGravity = affectedByGravity;
            Scale = scale;
            FadeInScale = scale;
            Lifetime = lifetime;
            DrawColor = InitialColor = color;
            FadeIn = fadeIn;

            if (FadeIn)
                Scale = 0f;
        }

        public override void Update()
        {
            if (!FadeIn)
            {
                Scale *= 0.95f;
                DrawColor = Color.Lerp(InitialColor, Color.Transparent, (float)Math.Pow(LifetimeRatio, 3D));
            }
            else
            {
                if ((float)Time / (float)Lifetime < 0.5f)
                {
                    Scale = MathHelper.Lerp(Scale, FadeInScale, 0.2f);
                }
                else
                {
                    Scale = MathHelper.Lerp(Scale, FadeInScale, -0.21f);
                }
            }
            Velocity *= 0.95f;
            if (Velocity.Length() < 12f && AffectedByGravity)
            {
                Velocity.X *= 0.94f;
                Velocity.Y += 0.25f;
            }
            Rotation = Velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 scale = new Vector2(0.5f, 1.6f) * Scale;
            Texture2D texture = TextureAssets.Extra[ExtrasID.SharpTears].Value;

            Color col = DrawColor;

            spriteBatch.Draw(texture, Position - Main.screenPosition, null, col, Rotation, texture.Size() * 0.5f, scale, 0, 0f);
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, col, Rotation, texture.Size() * 0.5f, scale * new Vector2(0.45f, 1f), 0, 0f);
        }
    }
}
