using LAP.Assets.TextureRegister;
using LAP.Core.Enums;
using LAP.Core.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace CalamityInheritance.Content.Particles
{
    public class StrongBloom : BaseParticle
    {
        public override int UseBlendStateID => BlendStateID.Additive;
        private float opacity;
        private Color BaseColor;

        public StrongBloom(Vector2 position, Vector2 velocity, Color color, float scale, int lifeTime)
        {
            Position = position;
            Velocity = velocity;
            BaseColor = color;
            Scale = scale;
            Lifetime = lifeTime;
            Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        }
        public override void Update()
        {
            opacity = (float)Math.Sin(LifetimeRatio * MathHelper.Pi);
            DrawColor = BaseColor * opacity;
            Lighting.AddLight(Position, DrawColor.R / 255f, DrawColor.G / 255f, DrawColor.B / 255f);
            Velocity *= 0.95f;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D tex = LAPTextureRegister.BloomCircle.Value;
            spriteBatch.Draw(tex, Position - Main.screenPosition, null, DrawColor * opacity, Rotation, tex.Size() / 2f, Scale, SpriteEffects.None, 0);
        }
    }
}
