using CalamityInheritance.Assets;
using LAP.Core.Enums;
using LAP.Core.ParticleSystem;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace CalamityInheritance.Content.Particles
{
    public class GlowSparkParticle : BaseParticle
    {
        public override int UseBlendStateID => BlendStateID.Additive;
        public Color InitialColor;
        public bool AffectedByGravity;
        public bool QuickShrink;
        public bool Glowing;
        public Vector2 Squash = new Vector2(0.5f, 1.6f);

        public GlowSparkParticle(Vector2 relativePosition, Vector2 velocity, bool affectedByGravity, int lifetime, float scale, Color color, Vector2 squash, bool quickShrink = false, bool glow = true)
        {
            Position = relativePosition;
            Velocity = velocity;
            AffectedByGravity = affectedByGravity;
            Scale = scale;
            Lifetime = lifetime;
            DrawColor = InitialColor = color;
            Squash = squash;
            QuickShrink = quickShrink;
            Glowing = glow;
            Rotation = Velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override void Update()
        {
            Scale *= 0.95f;
            DrawColor = Color.Lerp(InitialColor, Color.Transparent, (float)Math.Pow(LifetimeRatio, 3D));
            Velocity *= 0.95f;
            if (QuickShrink)
            {
                Squash.X *= 0.8f;
                Squash.Y *= 1.25f;
            }
            if (Velocity.Length() < 12f && AffectedByGravity)
            {
                Velocity.X *= 0.94f;
                Velocity.Y += 0.25f;
            }
            Rotation = Velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 scale = Squash * Scale;
            Texture2D texture = CIParticleTexture.GlowSpark.Value;

            spriteBatch.Draw(texture, Position - Main.screenPosition, null, DrawColor, Rotation, texture.Size() * 0.5f, scale, 0, 0f);
            if (Glowing)
                spriteBatch.Draw(texture, Position - Main.screenPosition, null, Color.White, Rotation, texture.Size() * 0.5f, scale * new Vector2(0.45f, 1f), 0, 0f);
        }
    }
}
