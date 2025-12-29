using CalamityInheritance.NPCs.Boss.Yharon;
using CalamityInheritance.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Boss.CalamitasClone.Sky
{
    public class CalCloneSky : CustomSky
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

        private bool isActive = false;
        public List<Cinder> Cinders = [];

        public static int CinderReleaseChance
        {
            get
            {
                return 4;
            }
        }
        public static float CinderSpeed
        {
            get
            {
                return 5.6f;
            }
        }
        public override void Update(GameTime gameTime)
        {
            if (!MiscFlagReset.CalCloneSkyActive)
                return;

            static Color selectCinderColor()
            {
                if (!NPC.AnyNPCs(NPCType<CalamitasCloneLegacy>()))
                    return Color.Transparent;

                return Color.Lerp(Color.OrangeRed, Color.Red, Main.rand.NextFloat(0.2f, 0.9f));
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

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 0 && minDepth < 0)
            {
                spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth * 2, Main.screenHeight * 2), Color.Black * GetIntensity());
            }

            // Draw cinders.
            Texture2D cinderTexture = Request<Texture2D>("CalamityMod/Skies/CalamitasCinder").Value;
            for (int i = 0; i < Cinders.Count; i++)
            {
                Vector2 drawPosition = Cinders[i].Center - Main.screenPosition;
                for (int j = 0; j < 3; j++)
                {
                    Vector2 offsetDrawPosition = drawPosition + (MathHelper.TwoPi * j / 3f).ToRotationVector2() * 1.4f;

                    spriteBatch.Draw(cinderTexture, offsetDrawPosition, null, Cinders[i].DrawColor, 0f, cinderTexture.Size() * 0.5f, Cinders[i].Scale * 1.5f, SpriteEffects.None, 0f);
                }
                spriteBatch.Draw(cinderTexture, drawPosition, null, Cinders[i].DrawColor, 0f, cinderTexture.Size() * 0.5f, Cinders[i].Scale, SpriteEffects.None, 0f);
            }
        }
        public float SkyStrength = 0f;
        public float GetIntensity()
        {
            if (!NPC.AnyNPCs(NPCType<CalamitasCloneLegacy>()) && SkyStrength > 0f)
                SkyStrength -= 0.02f;
            else if (SkyStrength < 1f)
                SkyStrength += 0.02f;

            return SkyStrength;
        }

        public override float GetCloudAlpha()
        {
            return 0f;
        }
        public override void Activate(Vector2 position, params object[] args)
        {
            isActive = true;
        }

        public override void Deactivate(params object[] args)
        {
            isActive = false;
        }

        public override void Reset()
        {
            isActive = false;
        }

        public override bool IsActive()
        {
            return isActive;
        }
    }
}
