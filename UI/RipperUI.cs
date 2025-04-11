using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CalamityInheritance.UI
{
    internal class RipperUI
    {/*
        #region Draw Adrenaline Bar
        private static void DrawAdrenalineBar(SpriteBatch spriteBatch, CalamityPlayer modPlayer, Vector2 screenPos)
        {
            bool draedonHeart = modPlayer.draedonsHeart;
            bool useFullTexture = modPlayer.adrenaline >= modPlayer.adrenalineMax || modPlayer.adrenalineModeActive;

            float uiScale = Main.UIScale;
            Vector2 shakeOffset = modPlayer.adrenalineModeActive ? GetShakeOffset() : Vector2.Zero;

            Vector2 origin = new Vector2(adrenBorderTex.Width * 0.5f, (adrenBorderTex.Height / AdrenBarFrames) * 0.5f);
            if (useFullTexture)
                origin = new Vector2(adrenBorderTexFull.Width * 0.5f, (adrenBorderTexFull.Height / AdrenBarFullFrames) * 0.5f);

            if (draedonHeart)
            {
                adrenBarTimer++;
                if (adrenBarTimer >= AdrenBarFrameDelay)
                {
                    adrenBarTimer = 0;
                    adrenBarFrame++;
                    adrenBarFullFrame++;
                    if (adrenBarFrame == AdrenBarFrames)
                        adrenBarFrame = 1;
                    if (adrenBarFullFrame == AdrenBarFullFrames)
                        adrenBarFullFrame = 1;
                }
            }
            else
            {
                adrenBarTimer = 0;
                adrenBarFrame = 0;
                adrenBarFullFrame = 0;
            }

            // If adrenaline is full this frame and the animation hasn't started yet, start it.
            float adrenRatio = modPlayer.adrenaline / modPlayer.adrenalineMax;
            if (adrenRatio >= 1f && adrenAnimFrame == -1)
                adrenAnimFrame = 0;

            // If the animation has already finished and adrenaline isn't full anymore, reset it.
            else if (adrenRatio < 1f && adrenAnimFrame == AdrenAnimFrames)
                adrenAnimFrame = -1;

            // Otherwise, the animation runs to completion even if the user activates adrenaline in the middle of it.
            bool animationActive = adrenAnimFrame >= 0 && adrenAnimFrame < AdrenAnimFrames;
            if (animationActive)
            {
                adrenAnimTimer++;
                if (adrenAnimTimer >= AdrenAnimFrameDelay)
                {
                    adrenAnimTimer = 0;
                    adrenAnimFrame++; // This will eventually increment it to AdrenAnimFrames, thus stopping the animation.
                }
            }

            if (!useFullTexture)
            {
                int frameHeight = (adrenBorderTex.Height / AdrenBarFrames) - 1;
                Rectangle borderRect = new Rectangle(0, (frameHeight + 1) * adrenBarFrame, adrenBorderTex.Width, frameHeight);
                // Draw the border of the Adrenaline Bar first
                spriteBatch.Draw(adrenBorderTex, screenPos + shakeOffset, borderRect, Color.White, 0f, origin, uiScale, SpriteEffects.None, 0);
            }
            else
            {
                // Use a slightly different texture if Adrenaline is full or active
                int frameHeight = (adrenBorderTexFull.Height / AdrenBarFullFrames) - 1;
                Rectangle borderRect = new Rectangle(0, (frameHeight + 1) * adrenBarFullFrame, adrenBorderTexFull.Width, frameHeight);

                spriteBatch.Draw(adrenBorderTexFull, screenPos + shakeOffset, borderRect, Color.White, 0f, origin, uiScale, SpriteEffects.None, 0);
            }

            // The amount of the bar to draw depends on the player's current Adrenaline level
            // offset calculates the deadspace that is the border and not the bar. Bar is 24 pixels tall
            int barWidth = adrenBarTex.Width;
            float offset = (adrenBorderTex.Width - adrenBarTex.Width) * 0.5f;
            Rectangle cropRect = new Rectangle(0, 0, (int)(barWidth * adrenRatio), adrenBarTex.Height);
            spriteBatch.Draw(draedonHeart ? draedonBarTex : adrenBarTex, screenPos + shakeOffset + new Vector2(offset * uiScale, 2f), cropRect, Color.White, 0f, origin, uiScale, SpriteEffects.None, 0);

            // Determine which pearls to draw (and their positions) based off of which Adrenaline upgrades the player has.
            IList<Texture2D> pearls = new List<Texture2D>(3);
            if (modPlayer.adrenalineBoostOne) // Electrolyte Gel Pack
                pearls.Add(electrolyteGelTex);
            if (modPlayer.adrenalineBoostTwo) // Starlight Fuel Cell
                pearls.Add(starlightFuelTex);
            if (modPlayer.adrenalineBoostThree) // Ectoheart
                pearls.Add(ectoheartTex);
            IList<Vector2> offsets = GetPearlOffsets(pearls.Count);

            // Draw pearls at appropriate positions.
            for (int i = 0; i < pearls.Count; ++i)
                spriteBatch.Draw(pearls[i], screenPos + shakeOffset + offsets[i] * uiScale + new Vector2(0, 5f), null, Color.White, 0f, origin, uiScale, SpriteEffects.None, 0);

            // If the animation is active, draw the animation on top of both the border and the bar.
            if (animationActive)
            {
                float animOffset = 5f;
                float xOffset = (adrenBorderTex.Width - adrenAnimTex.Width) / 2f;
                int frameHeight = (adrenAnimTex.Height / AdrenAnimFrames) - 1;
                float yOffset = ((adrenBorderTex.Height / AdrenBarFrames) - frameHeight) / 2f + animOffset;
                if (useFullTexture)
                    yOffset = ((adrenBorderTexFull.Height / AdrenBarFullFrames) - frameHeight) / 2f + animOffset;
                Vector2 sizeDiffOffset = new Vector2(xOffset, yOffset);
                Rectangle animCropRect = new Rectangle(0, (frameHeight + 1) * adrenAnimFrame, adrenAnimTex.Width, frameHeight);
                spriteBatch.Draw(draedonHeart ? draedonAnimTex : adrenAnimTex, screenPos + shakeOffset + sizeDiffOffset, animCropRect, Color.White, 0f, origin, uiScale, SpriteEffects.None, 0);
            }
        }
        #endregion*/
    }
}
