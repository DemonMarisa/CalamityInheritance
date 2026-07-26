using CalamityInheritance.Assets;
using LAP.Core.Enums;
using LAP.Core.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Particles
{
    public class LineVFX : BaseParticle
    {
        public override int UseBlendStateID => BlendStateID.Additive;

        public float opacity;
        public Vector2 LineVector;
        public bool Concave;
        public float Expansion;

        public LineVFX(Vector2 startPoint, Vector2 lineVector, float thickness, Color color, bool concave = false, bool telegraph = false, float expansion = 0f)
        {
            Position = startPoint;
            LineVector = lineVector;
            Scale = thickness;
            DrawColor = color;
            Concave = concave;
            Expansion = expansion;
            Velocity = Vector2.Zero;
            Rotation = 0;
            Lifetime = 2;
            Important = telegraph;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D tex;
            if (Concave)
                tex = ModContent.Request<Texture2D>("CalamityMod/Particles/ThickEndedLine").Value;
            else
                tex = CIParticleTexture.ThinEndedLine.Value;

            Vector2 drawPosition = Position - Utils.SafeNormalize(LineVector, Vector2.Zero) * (float)Math.Sqrt(1f - (float)Math.Pow(LifetimeRatio - 1f, 2)) * Expansion / 2f;
            Vector2 expandedLine = LineVector + Utils.SafeNormalize(LineVector, Vector2.Zero) * (float)Math.Sqrt(1f - (float)Math.Pow(LifetimeRatio - 1f, 2)) * Expansion;

            float rot = LineVector.ToRotation() + MathHelper.PiOver2;
            Vector2 origin = new Vector2(tex.Width / 2f, tex.Height);
            Vector2 scale = new Vector2(Scale, expandedLine.Length() / tex.Height);

            spriteBatch.Draw(tex, drawPosition - Main.screenPosition, null, DrawColor, rot, origin, scale, SpriteEffects.None, 0);
        }
    }
}
