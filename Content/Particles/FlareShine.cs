using CalamityInheritance.Assets;
using LAP.Assets.TextureRegister;
using LAP.Core.Enums;
using LAP.Core.ParticleSystem;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static CalamityMod.CalamityUtils;

namespace CalamityInheritance.Content.Particles
{
    public class FlareShine : BaseParticle
    {
        public override int UseBlendStateID => BlendStateID.Additive;

        private float Spin;
        private float opacity;
        private Color Bloom;
        private Color LightColor => Bloom * opacity;
        private float BloomScale;
        private float HueShift;
        private Vector2 OriginalScale;
        private Vector2 FinalScale;
        private int SpawnDelay;

        public FlareShine(Vector2 position, Vector2 velocity, Color color, Color bloom, float angle, Vector2 scale, Vector2 finalScale, int lifeTime, float rotationSpeed = 0f, float bloomScale = 1f, float hueShift = 0f, int spawnDelay = 0)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = color;
            Bloom = bloom;

            OriginalScale = scale;
            FinalScale = finalScale;

            Scale = 1f;
            Lifetime = lifeTime;
            Rotation = angle % MathHelper.Pi;
            Spin = rotationSpeed;
            BloomScale = bloomScale;
            HueShift = hueShift;
            SpawnDelay = spawnDelay;
        }

        public override void Update()
        {
            if (SpawnDelay > 0) //Prevent the particle from existing
            {
                Time--;
                Position -= Velocity;
                SpawnDelay--;
                return;
            }

            opacity = (float)Math.Sin(MathHelper.PiOver2 + LifetimeRatio * MathHelper.PiOver2);
            Velocity *= 0.80f;
            Rotation += Spin * ((Velocity.X > 0) ? 1f : -1f) * (LifetimeRatio > 0.5 ? 1f : 0.5f);

            DrawColor = Main.hslToRgb((Main.rgbToHsl(DrawColor).X + HueShift) % 1, Main.rgbToHsl(DrawColor).Y, Main.rgbToHsl(DrawColor).Z);
            Bloom = Main.hslToRgb((Main.rgbToHsl(Bloom).X + HueShift) % 1, Main.rgbToHsl(Bloom).Y, Main.rgbToHsl(Bloom).Z);


            Lighting.AddLight(Position, LightColor.R / 255f, LightColor.G / 255f, LightColor.B / 255f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (SpawnDelay > 0)
                return;
            Texture2D sparkTexture = CIParticleTexture.ThinSparkle.Value;
            Texture2D bloomTexture = LAPTextureRegister.BloomCircle.Value;
            //Ajust the bloom's texture to be the same size as the star's
            float properBloomSize = (float)sparkTexture.Height / (float)bloomTexture.Height;
            Vector2 squish = Vector2.Lerp(OriginalScale, FinalScale, BezierEaseHelper.BezierSmooth(Vector2.UnitY, Vector2.One, LifetimeRatio));

            spriteBatch.Draw(bloomTexture, Position - Main.screenPosition, null, Bloom * opacity * 0.5f, 0, bloomTexture.Size() / 2f, squish * BloomScale * properBloomSize, SpriteEffects.None, 0);
            spriteBatch.Draw(sparkTexture, Position - Main.screenPosition, null, DrawColor * opacity, Rotation, sparkTexture.Size() / 2f, squish, SpriteEffects.None, 0);
        }
    }
}
