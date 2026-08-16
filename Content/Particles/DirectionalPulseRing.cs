using CalamityInheritance.Assets;
using LAP.Core.Enums;
using LAP.Core.ParticleSystem;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Particles
{
    public class DirectionalPulseRing : BaseParticle
    {
        public override int UseBlendStateID => BlendStateID.Additive;

        private float OriginalScale;
        private float FinalScale;
        private float opacity;
        private Vector2 Squish;
        private Color BaseColor;

        public DirectionalPulseRing(Vector2 position, Vector2 velocity, Color color, Vector2 squish, float rotation, float originalScale, float finalScale, int lifeTime)
        {
            Position = position;
            Velocity = velocity;
            BaseColor = color;
            OriginalScale = originalScale;
            FinalScale = finalScale;
            Scale = originalScale;
            Lifetime = lifeTime;
            Squish = squish;
            Rotation = rotation;
        }

        public override void Update()
        {
            Scale = MathHelper.Lerp(OriginalScale, FinalScale, EasingHelper.EaseOutCubic(LifetimeRatio));

            opacity = (float)Math.Sin(MathHelper.PiOver2 + LifetimeRatio * MathHelper.PiOver2);

            DrawColor = BaseColor * opacity;
            Lighting.AddLight(Position, DrawColor.R / 255f, DrawColor.G / 255f, DrawColor.B / 255f);
            Velocity *= 0.95f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D tex = CITextureRegister.HighResHollowCircleHardEdge.Value;
            spriteBatch.Draw(tex, Position - Main.screenPosition, null, DrawColor * opacity, Rotation, tex.Size() / 2f, Scale * Squish * 0.1f, SpriteEffects.None, 0);
        }
    }
}
