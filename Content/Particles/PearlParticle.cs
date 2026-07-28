using CalamityInheritance.Assets;
using LAP.Core.Enums;
using LAP.Core.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace CalamityInheritance.Content.Particles
{
    public class PearlParticle : BaseParticle
    {
        public override int UseBlendStateID => BlendStateID.NonPremult;
        public Color InitialColor;
        public bool AffectedByGravity;
        public float ShrinkSpeed;
        public float RotationSpeed;
        public bool HitTiles;
        public bool hasTileHit;
        public float pVelX;
        public float pVelY;
        public PearlParticle(Vector2 relativePosition, Vector2 velocity, bool affectedByGravity, int lifetime, float scale, Color color, float shrinkSpeed = 0.95f, float rotationSpeed = 0, bool hitTiles = false)
        {
            Position = relativePosition;
            Velocity = velocity;
            AffectedByGravity = affectedByGravity;
            Scale = scale;
            Lifetime = lifetime;
            DrawColor = InitialColor = color;
            ShrinkSpeed = shrinkSpeed;
            RotationSpeed = rotationSpeed;
            HitTiles = hitTiles;
        }

        public override void Update()
        {
            if (HitTiles)
            {
                if (hasTileHit)
                {
                    if (Velocity.X != pVelX)
                    {
                        Velocity.X = -pVelX;
                    }
                    if (Velocity.Y != pVelY)
                    {
                        Velocity.Y = -pVelY;
                    }
                    HitTiles = false;
                }
                if (Collision.SolidCollision(Position, (int)(7f * Scale), (int)(7f * Scale)))
                {
                    hasTileHit = true;
                    pVelX = Velocity.X;
                    pVelY = Velocity.Y;
                }
            }
            Scale *= ShrinkSpeed;
            RotationSpeed *= ShrinkSpeed;
            DrawColor = Color.Lerp(InitialColor, Color.Transparent, (float)Math.Pow(LifetimeRatio, 3D));
            Velocity *= 0.95f;
            if (Velocity.Length() < 12f && AffectedByGravity)
            {
                Velocity.X *= 0.94f;
                Velocity.Y += 0.25f;
            }
            Rotation += RotationSpeed;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 scale = new Vector2(1f, 1f) * Scale;
            Texture2D texture = CIParticleTexture.PearlParticle.Value;
            Texture2D texture2 = CIParticleTexture.PearlParticleGlow.Value;

            spriteBatch.Draw(texture2, Position - Main.screenPosition, null, DrawColor, Rotation, texture.Size() * 0.5f, scale, 0, 0f);
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, Color.Lerp(Color.White, Color.Transparent, (float)Math.Pow(LifetimeRatio, 3D)), Rotation, texture.Size() * 0.5f, scale, 0, 0f);
        }
    }
}
