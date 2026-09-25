using CalamityInheritance.Core.CIWorlds;
using LAP.Assets.TextureRegister;
using LAP.Core.Graphics.LAPCustomSky;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;

namespace CalamityInheritance.Content.NPCs.Boss.CalamitasClone.Sky
{
    public class CalCloneSky : LAPCustomSky
    {
        public class Cinder
        {
            public int Time;
            public int Lifetime;
            public int IdentityIndex;
            public float Scale;
            public float Depth;
            public Color DrawColor;
            public Vector2 Velocity;
            public Vector2 Center;
            public Cinder(int lifetime, int identity, float depth, Color color, Vector2 startingPosition, Vector2 startingVelocity)
            {
                Lifetime = lifetime;
                IdentityIndex = identity;
                Depth = depth;
                DrawColor = color;
                Center = startingPosition;
                Velocity = startingVelocity;
            }
        }
        public List<Cinder> Cinders = [];
        public const int CinderReleaseChance = 4;
        public const float CinderSpeed = 5.6f;
        public float SkyStrength = 0f;
        public override void Update()
        {
            if (CIBossAliveFlag.ActiveCalCloneFlag == -1)
            {
                SkyStrength = MathHelper.Lerp(SkyStrength, 0f, 0.03f);
            }
            else
            {
                SkyStrength = MathHelper.Lerp(SkyStrength, 1f, 0.03f);
            }
            static Color selectCinderColor()
            {
                return Color.Lerp(Color.Orange, Color.Red, Main.rand.NextFloat(0.2f, 0.9f));
            }

            // Randomly add cinders.
            if (Main.rand.NextBool(CinderReleaseChance))
            {
                int lifetime = Main.rand.Next(285, 445);
                float depth = Main.rand.NextFloat(1.8f, 5f);
                Vector2 startingPosition = Main.screenPosition + new Vector2(Main.screenWidth * Main.rand.NextFloat(-0.1f, 1.1f), Main.screenHeight * 1.05f);
                Vector2 startingVelocity = -Vector2.UnitY.RotatedByRandom(0.91f);
                Cinders.Add(new Cinder(lifetime, Cinders.Count, depth, selectCinderColor(), startingPosition, startingVelocity));
            }

            // Update all cinders.
            for (int i = 0; i < Cinders.Count; i++)
            {
                Cinders[i].Scale = Utils.GetLerpValue(Cinders[i].Lifetime, Cinders[i].Lifetime / 3, Cinders[i].Time, true);
                Cinders[i].Scale *= MathHelper.Lerp(0.6f, 0.9f, Cinders[i].IdentityIndex % 6f / 6f);

                Vector2 idealVelocity = -Vector2.UnitY.RotatedBy(MathHelper.Lerp(-0.94f, 0.94f, (float)Math.Sin(Cinders[i].Time / 36f + Cinders[i].IdentityIndex) * 0.5f + 0.5f)) * CinderSpeed;
                float movementInterpolant = MathHelper.Lerp(0.01f, 0.08f, Utils.GetLerpValue(45f, 145f, Cinders[i].Time, true));
                Cinders[i].Velocity = Vector2.Lerp(Cinders[i].Velocity, idealVelocity, movementInterpolant);
                Cinders[i].Velocity = Cinders[i].Velocity.SafeNormalize(-Vector2.UnitY) * CinderSpeed;
                Cinders[i].Time++;

                Cinders[i].Center += Cinders[i].Velocity;
            }

            // Clear away all dead cinders.
            Cinders.RemoveAll(c => c.Time >= c.Lifetime);
        }
        public override void Draw()
        {
            Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth * 2, Main.screenHeight * 2), Color.Black * SkyStrength);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            // Draw cinders.
            Texture2D cinderTexture = LAPTextureRegister.WhiteCube.Value;
            Texture2D glowTexture = LAPTextureRegister.SmallGlowBall.Value;
            for (int i = 0; i < Cinders.Count; i++)
            {
                Vector2 drawPosition = Cinders[i].Center - Main.screenPosition;
                for (int j = 0; j < 3; j++)
                {
                    Vector2 offsetDrawPosition = drawPosition + (MathHelper.TwoPi * j / 3f).ToRotationVector2() * 1.4f;

                    Main.spriteBatch.Draw(cinderTexture, offsetDrawPosition, null, Cinders[i].DrawColor * SkyStrength, 0f, cinderTexture.Size() * 0.5f, Cinders[i].Scale * 0.3f, SpriteEffects.None, 0f);
                }
                Main.spriteBatch.Draw(cinderTexture, drawPosition, null, Cinders[i].DrawColor * SkyStrength, 0f, cinderTexture.Size() * 0.5f, Cinders[i].Scale * 0.2f, SpriteEffects.None, 0f);
                float breath = MathF.Sin(Main.GlobalTimeWrappedHourly + Cinders[i].Depth) * 0.2f;
                float opacity = 0.55f + breath;
                Main.spriteBatch.Draw(glowTexture, drawPosition, null, Cinders[i].DrawColor * SkyStrength * opacity, 0f, glowTexture.Size() * 0.5f, Cinders[i].Scale * 0.4f, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }
        public override bool DeActiveCondition()
        {
            return SkyStrength < 0.001f && CIBossAliveFlag.ActiveCalCloneFlag == -1;
        }
        public override void OnDeActive()
        {
            Cinders.Clear();
        }
    }
}
