using CalamityInheritance.Assets;
using LAP.Core.Enums;
using LAP.Core.ParticleSystem;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace CalamityInheritance.Content.Particles
{
    public class FireStrike : BaseParticle
    {
        public override int UseBlendStateID => BlendStateID.Additive;
        public float BeginScale;
        public SpriteEffects se = SpriteEffects.None;
        public bool UseFadeIn = true;
        public FireStrike(Vector2 position, Vector2 velocity, Color color, int lifetime, float opacity, float rot, float scale)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = color;
            Lifetime = lifetime;
            Opacity = opacity;
            Rotation = rot;
            Scale = scale;
            BeginScale = scale;
        }
        public override void OnSpawn()
        {
            if (Main.rand.NextBool())
                se = SpriteEffects.FlipHorizontally;

            if (UseFadeIn)
                Scale = BeginScale;
        }
        public override void Update()
        {
            if (LifetimeRatio < 0.5f)
            {
                Scale = MathHelper.Lerp(0f, BeginScale, EasingHelper.EaseOutCubic(LifetimeRatio * 2));
            }
            if (LifetimeRatio > 0.5f)
            {
                float progress = LifetimeRatio - 0.5f;
                Opacity = MathHelper.Lerp(1f, 0f, EasingHelper.EaseOutCubic(progress));
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = CIParticleTexture.FireStrike.Value;
            Vector2 DrawOrig = new Vector2(texture.Size().X / 2, texture.Size().Y);
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, DrawColor * Opacity, Rotation, DrawOrig, Scale, se, 0f);
        }
    }
}
