using LAP.Core.Enums;
using LAP.Core.ParticleSystem;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace CalamityInheritance.Content.Particles
{
    public class CustomPulse : BaseParticle
    {
        public bool UseAltVisual = true;
        public override int UseBlendStateID => UseAltVisual ? BlendStateID.Additive : BlendStateID.Alpha;

        private string NewTexture;
        private float OriginalScale;
        private float FinalScale;
        private float opacity;
        private Vector2 Squish;
        private Color BaseColor;

        public CustomPulse(Vector2 position, Vector2 velocity, Color color, string texture, Vector2 squish, float rotation, float originalScale, float finalScale, int lifeTime, bool UseAdditiveBlend = true)
        {
            Position = position;
            Velocity = velocity;
            BaseColor = color;
            NewTexture = texture;
            OriginalScale = originalScale;
            FinalScale = finalScale;
            Scale = originalScale;
            Lifetime = lifeTime;
            Squish = squish;
            Rotation = rotation;
            UseAltVisual = UseAdditiveBlend;
        }

        public override void Update()
        {
            float pulseProgress = EasingHelper.EaseOutCubic(LifetimeRatio);
            Scale = MathHelper.Lerp(OriginalScale, FinalScale, pulseProgress);

            opacity = (float)Math.Sin(MathHelper.PiOver2 + LifetimeRatio * MathHelper.PiOver2);

            DrawColor = BaseColor * opacity;
            Lighting.AddLight(Position, DrawColor.R / 255f, DrawColor.G / 255f, DrawColor.B / 255f);
            Velocity *= 0.95f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D tex = Request<Texture2D>(NewTexture).Value;
            spriteBatch.Draw(tex, Position - Main.screenPosition, null, DrawColor * opacity, Rotation, tex.Size() / 2f, Scale * Squish, SpriteEffects.None, 0);
        }

    }
}
