using LAP.Assets.TextureRegister;
using LAP.Core.Enums;
using LAP.Core.ParticleSystem;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace CalamityInheritance.Content.Particles
{
    public class Shockwave01 : BaseParticle
    {
        public override int UseBlendStateID => BlendStateID.Additive;
        public float BeginScale;
        public SpriteEffects se = SpriteEffects.None;
        public bool UseFadeIn = true;
        public Shockwave01(Vector2 position, Vector2 velocity, Color color, int lifetime, float opacity, float scale)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = color;
            Lifetime = lifetime;
            Opacity = opacity;
            Scale = scale;
            BeginScale = scale;
        }
        public override void OnSpawn()
        {
        }
        public override void Update()
        {
            if (LifetimeRatio < 0.5f)
            {
                Scale = MathHelper.Lerp(0f, BeginScale, EasingHelper.EaseOutCubic(LifetimeRatio * 2));
            }
            else
            {
                float progress = LifetimeRatio - 0.5f;
                Opacity = MathHelper.Lerp(1f, 0f, EasingHelper.EaseOutCubic(progress * 2));
            }
        }
        // 这里采样没有问题，他贴图就是这样
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = LAPTextureRegister.Shockwave_01.Value;
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, DrawColor * Opacity, Rotation, texture.Size() / 2, Scale, se, 0f);
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, DrawColor * Opacity, Rotation, texture.Size() / 2, Scale, se, 0f);
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, DrawColor * Opacity, Rotation, texture.Size() / 2, Scale, se, 0f);
        }
    }
}
