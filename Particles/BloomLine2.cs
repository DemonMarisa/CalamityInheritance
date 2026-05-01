using CalamityMod.Buffs.StatDebuffs;
using LAP.Assets.TextureRegister;
using LAP.Core.ParticleSystem;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using LAP.Core.Enums;

namespace CalamityInheritance.Particles
{
    public class BloomLine2 : BaseParticle
    {
        public BloomLine2(Vector2 position, Vector2 endPos, Color color, float scale, int lifeTime, bool fadeout)
        {
            Position = position;
            EndPos = endPos;
            DrawColor = color;
            Scale = scale;
            Lifetime = lifeTime;
            FadeOut = fadeout;
        }
        public Vector2 BeginPos;
        public Vector2 EndPos;
        public bool FadeOut;
        public override int UseBlendStateID => BlendStateID.Additive;
        public override void OnSpawn()
        {
            Opacity = 1f;
        }
        public override void Update()
        {
            if (FadeOut)
            {
                Opacity = MathHelper.Lerp(1f, 0f, EasingHelper.EaseInCubic(LifetimeRatio)) + 0.5f;
            }
        }
        public override void OnKill()
        {
            base.OnKill();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D tex = LAPTextureRegister.BloomLine2.Value;
            Vector2 LengthVector = (EndPos - Position);
            float Length = LengthVector.Length();
            float rot = LengthVector.ToRotation() + MathHelper.PiOver2;
            Vector2 origin = new Vector2(tex.Width / 2f, tex.Height);
            Vector2 scale = new Vector2(Scale, Length / tex.Height);
            spriteBatch.Draw(tex, Position - Main.screenPosition, null, DrawColor * Opacity, rot, origin, scale, SpriteEffects.None, 0);

            Texture2D cap = LAPTextureRegister.BloomLine2Cap.Value;
            scale = new Vector2(Scale, Scale);
            origin = new Vector2(cap.Width / 2f, cap.Height);

            spriteBatch.Draw(cap, Position - Main.screenPosition, null, DrawColor * Opacity, rot + MathHelper.Pi, origin, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(cap, Position + LengthVector - Main.screenPosition, null, DrawColor * Opacity, rot, origin, scale, SpriteEffects.None, 0);
        }
    }
}
