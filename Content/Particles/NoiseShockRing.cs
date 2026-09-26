using LAP.Assets.Effects;
using LAP.Assets.TextureRegister;
using LAP.Core.Enums;
using LAP.Core.ParticleSystem;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace CalamityInheritance.Content.Particles
{
    public class NoiseShockRing : BaseParticle
    {
        public override int UseBlendStateID => BlendStateID.Additive;
        public float BeginScale;
        public int Index;
        public Projectile Father => Main.projectile[Index];
        public Vector2 Offset;
        public bool Follow = true;
        public NoiseShockRing(Vector2 position, Vector2 velocity, Color color, int lifetime, float opacity, float scale, int father, Vector2 offset, bool follow = false)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = color;
            Lifetime = lifetime;
            Opacity = opacity;
            Scale = scale;
            BeginScale = scale;
            Index = father;
            Offset = offset;
            Follow = follow;
            Important = true;
        }
        public override void OnSpawn()
        {
            Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        }
        public override void Update()
        {
            if (Index == -1)
                Follow = false;

            if (Follow)
                Position = Father.Center + Offset.RotatedBy(Father.rotation);

            if (LifetimeRatio < 0.5f)
            {
                Scale = MathHelper.Lerp(0f, BeginScale, EasingHelper.EaseOutCubic(LifetimeRatio * 2));
            }
            else
            {
                float progress = LifetimeRatio - 0.5f;
                Opacity = MathHelper.Lerp(1f, 0f, EasingHelper.EaseOutCubic(progress * 2));
            }
            Rotation += 0.05f;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            LAPUtilities.ReSetToBeginShader();
            Effect shader = LAPShaderRegister.PolarDistortShaderWithR.Value;
            shader.Parameters["uWidthMult"].SetValue(1f);
            shader.Parameters["uRingMult"].SetValue(4f);
            shader.Parameters["uYTime"].SetValue(Main.GlobalTimeWrappedHourly);
            shader.CurrentTechnique.Passes[0].Apply();

            Main.instance.GraphicsDevice.Textures[1] = LAPTextureRegister.BloomRing.Value;

            Texture2D texture = LAPTextureRegister.Aura_01.Value;
            Vector2 orig = texture.Size() / 2;
            Main.spriteBatch.Draw(texture, Position - Main.screenPosition, null, DrawColor * Opacity, Rotation, orig, Scale, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(texture, Position - Main.screenPosition, null, DrawColor * Opacity, Rotation, orig, Scale, SpriteEffects.None, 0);
            LAPUtilities.ReSetToEndShader(BlendState.Additive);
        }
    }
}
