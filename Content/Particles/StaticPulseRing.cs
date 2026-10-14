using CalamityInheritance.Assets;
using LAP.Core.Enums;
using LAP.Core.ParticleSystem;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace CalamityInheritance.Content.Particles
{
    public class StaticPulseRing : BaseParticle
    {
        public override int UseBlendStateID => BlendStateID.Additive;
        private float OriginalScale;
        private float FinalScale;
        private Vector2 Squish;
        private Color BaseColor;

        //Basically DirectionalPulseRing, but doesn't move or change opacity
        public StaticPulseRing(Vector2 position, Vector2 velocity, Color color, Vector2 squish, float rotation, float originalScale, float finalScale, int lifeTime)
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
            float pulseProgress = EasingHelper.EaseOutCubic(LifetimeRatio);
            Scale = MathHelper.Lerp(OriginalScale, FinalScale, pulseProgress);

            DrawColor = BaseColor;
            Lighting.AddLight(Position, DrawColor.R / 255f, DrawColor.G / 255f, DrawColor.B / 255f);
            Velocity *= 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D tex = CIExtraTexture.HighResHollowCircleHardEdge.Value;
            spriteBatch.Draw(tex, Position - Main.screenPosition, null, DrawColor, Rotation, tex.Size() / 2f, Scale * Squish, SpriteEffects.None, 0);
        }

    }
}
