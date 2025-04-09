﻿using CalamityMod.Events;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.NPCs.Boss.SCAL.Brother;
using CalamityInheritance.NPCs.Boss.SCAL.ScalWorm;

namespace CalamityInheritance.NPCs.Boss.SCAL.Sky
{
    public class SCalSkyLegacy : CustomSky
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
        private float intensity = 0f;
        private int SCalIndex = -1;
        public List<Cinder> Cinders = [];

        public static bool RitualDramaProjectileIsPresent
        {
            get;
            internal set;
        }

        public static int CinderReleaseChance
        {
            get
            {
                if (!Main.npc.IndexInRange(CIGlobalNPC.LegacySCal) || Main.npc[CIGlobalNPC.LegacySCal].type != ModContent.NPCType<SupremeCalamitasLegacy>())
                    return int.MaxValue;

                NPC scal = Main.npc[CIGlobalNPC.LegacySCal];
                float lifeRatio = scal.life / (float)scal.lifeMax;

                // Release more and more cinders at the end of the battle until the acceptance phase, where it all slows down dramatically, with just a few ashes.
                if (lifeRatio < 0.1f)
                {
                    if (lifeRatio <= 0.01f)
                        return 2;

                    return (int)Math.Round(MathHelper.Lerp(2f, 11f, Utils.GetLerpValue(0.03f, 0.1f, lifeRatio, true)));
                }

                // Release a good amount of cinders while brothers or Sepulcher-1 are alive. Sepulcher-2 falls into an above case and does not execute this return.
                if (NPC.AnyNPCs(ModContent.NPCType<SupremeCataclysmLegacy>()) || NPC.AnyNPCs(ModContent.NPCType<SupremeCatastropheLegacy>()) || NPC.AnyNPCs(ModContent.NPCType<SCalWormHead>()))
                    return 4;

                // Release a moderate amount of cinders normally.
                return 6;
            }
        }
        public static float CinderSpeed
        {
            get
            {
                if (!Main.npc.IndexInRange(CIGlobalNPC.LegacySCal) || Main.npc[CIGlobalNPC.LegacySCal].type != ModContent.NPCType<SupremeCalamitasLegacy>())
                    return 0f;

                NPC scal = Main.npc[CIGlobalNPC.LegacySCal];
                float lifeRatio = scal.life / (float)scal.lifeMax;

                // Move more and more quickly at the end of the battle until the acceptance phase, where it all slows down dramatically.
                if (lifeRatio < 0.1f)
                {
                    if (lifeRatio <= 0.01f)
                        return 4.5f;

                    return MathHelper.Lerp(10.75f, 6.7f, Utils.GetLerpValue(0.03f, 0.1f, lifeRatio, true));
                }

                // Move a little quickly while brothers or Sepulcher-1 are alive. Sepulcher-2 falls into an above case and does not execute this return.
                if (NPC.AnyNPCs(ModContent.NPCType<SupremeCataclysmLegacy>()) || NPC.AnyNPCs(ModContent.NPCType<SupremeCatastropheLegacy>()) || NPC.AnyNPCs(ModContent.NPCType<SCalWormHead>()))
                    return 7.4f;

                // Move moderately quickly usually.
                return 5.6f;
            }
        }
        public static float OverridingIntensity = 0f;

        public override void Update(GameTime gameTime)
        {
            if (SCalIndex == -1)
            {
                UpdateSCalIndex();
                if (SCalIndex == -1)
                    isActive = false;
            }

            if (!Main.npc.IndexInRange(CIGlobalNPC.LegacySCal) || Main.npc[CIGlobalNPC.LegacySCal].type != ModContent.NPCType<SupremeCalamitasLegacy>())
                isActive = false;

            static Color selectCinderColor()
            {
                if (!Main.npc.IndexInRange(CIGlobalNPC.LegacySCal) || Main.npc[CIGlobalNPC.LegacySCal].type != ModContent.NPCType<SupremeCalamitasLegacy>())
                    return Color.Transparent;

                NPC scal = Main.npc[CIGlobalNPC.LegacySCal];
                float lifeRatio = scal.life / (float)scal.lifeMax;
                if (lifeRatio > 0.5f)
                    return Color.Lerp(Color.DarkRed, Color.Orange, Main.rand.NextFloat(0.8f));
                else if (lifeRatio > 0.3f)
                    return Color.Lerp(Color.CornflowerBlue, Color.Lerp(Color.Blue, Color.DarkBlue, Main.rand.NextFloat() * 0.65f), 0.45f);
                else if (lifeRatio > 0.01f)
                    return Color.Lerp(Color.Red, Color.Yellow, Main.rand.NextFloat(0.2f, 0.9f));
                else
                    return Color.Lerp(Color.Gray, Color.White, Main.rand.NextFloat(0.2f, 0.9f));
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

        private float GetIntensity()
        {
            if (RitualDramaProjectileIsPresent)
                return OverridingIntensity;

            OverridingIntensity = 0f;
            if (UpdateSCalIndex())
            {
                float x = 0f;
                if (SCalIndex != -1)
                    x = Vector2.Distance(Main.player[Main.myPlayer].Center, Main.npc[this.SCalIndex].Center);
                float intensityFactor = BossRushEvent.BossRushActive ? -0.2f : 1f;

                return (1f - Utils.SmoothStep(3000f, 6000f, x)) * intensityFactor;
            }
            return 0f;
        }

        public Color Phase1Colore = new(205, 100, 100);

        public override Color OnTileColor(Color color)
        {
            return Color.Lerp(color, Phase1Colore, 1f - GetIntensity());
        }

        private bool UpdateSCalIndex()
        {
            int SCalType = ModContent.NPCType<SupremeCalamitasLegacy>();
            if (SCalIndex >= 0 && Main.npc[SCalIndex].active && Main.npc[SCalIndex].type == SCalType)
            {
                return true;
            }
            SCalIndex = -1;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == SCalType)
                {
                    SCalIndex = i;
                    break;
                }
            }
            return SCalIndex != -1;
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (maxDepth >= 0 && minDepth < 0)
            {
                float intensity = GetIntensity();
                spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth * 2, Main.screenHeight * 2), Color.Black * intensity);
            }

            // Draw cinders.
            Texture2D cinderTexture = ModContent.Request<Texture2D>("CalamityMod/Skies/CalamitasCinder").Value;
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
            return isActive || intensity > 0f;
        }
    }
}
