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
    public class GenericBloom : BaseParticle
    {
        public override string Texture => "CalamityMod/Particles/Light";
        public bool UseAltVisual = true;
        public override int UseBlendStateID => UseAltVisual ? BlendStateID.Additive : BlendStateID.NonPremult;

        private float opacity;
        private Color BaseColor;
        private bool ProduceLight;

        public GenericBloom(Vector2 position, Vector2 velocity, Color color, float scale, int lifeTime, bool produceLight = true, bool AddativeBlend = true)
        {
            Position = position;
            Velocity = velocity;
            BaseColor = color;
            Scale = scale;
            Lifetime = lifeTime;
            Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            ProduceLight = produceLight;
            UseAltVisual = AddativeBlend;
        }

        public override void Update()
        {
            opacity = (float)Math.Sin(LifetimeRatio * MathHelper.Pi);
            DrawColor = BaseColor * opacity;
            if (ProduceLight)
            {
                Lighting.AddLight(Position, DrawColor.R / 255f, DrawColor.G / 255f, DrawColor.B / 255f);
            }
            Velocity *= 0.95f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D tex = CIParticleTexture.Light.Value;
            spriteBatch.Draw(tex, Position - Main.screenPosition, null, DrawColor * opacity, Rotation, tex.Size() / 2f, Scale, SpriteEffects.None, 0);
        }
    }
}
