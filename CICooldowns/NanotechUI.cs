﻿using System;
using CalamityInheritance.Content.Items.Accessories.Rogue;
using CalamityInheritance.Utilities;
using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using static CalamityMod.CalamityUtils;
using static Terraria.ModLoader.ModContent;

namespace CalamityInheritance.CICooldowns
{
    public class NanotechUI : CooldownHandler
    {
        private static Color ringColorLerpStart = new Color(30, 49, 36);
        private static Color ringColorLerpEnd = new Color(0, 255, 110);
        private float AdjustedCompletion => instance.timeLeft / (float)NanotechOld.nanotechDMGStack;
        public static new string ID => "NanotechUI";
        public override bool CanTickDown => !instance.player.CalamityInheritance().nanotechold || instance.timeLeft <= 0;
        public override bool ShouldDisplay => instance.player.CalamityInheritance().nanotechold;
        public override LocalizedText DisplayName => CIFunction.GetText($"UI.Cooldowns.{ID}");
        public override string Texture => "CalamityInheritance/CICooldowns/NanotechUI";
        public override string OutlineTexture => "CalamityInheritance/CICooldowns/NanotechUIOutline";
        public override string OverlayTexture => "CalamityInheritance/CICooldowns/NanotechUIOverlay";
        public override Color OutlineColor => new Color(133, 204, 237);
        public override Color CooldownStartColor => Color.Lerp(ringColorLerpStart, ringColorLerpEnd, instance.Completion);
        public override Color CooldownEndColor => Color.Lerp(ringColorLerpStart, ringColorLerpEnd, instance.Completion);
        public override bool SavedWithPlayer => false;
        public override bool PersistsThroughDeath => false;
        public override void ApplyBarShaders(float opacity)
        {
            // Use the adjusted completion
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseOpacity(opacity);
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseSaturation(AdjustedCompletion);
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseColor(CooldownStartColor);
            GameShaders.Misc["CalamityMod:CircularBarShader"].UseSecondaryColor(CooldownEndColor);
            GameShaders.Misc["CalamityMod:CircularBarShader"].Apply();
        }
        public override void DrawExpanded(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
        {
            base.DrawExpanded(spriteBatch, position, opacity, scale);
            float Xoffset = instance.timeLeft > 9 ? -10f : -5;
            DrawBorderStringEightWay(spriteBatch, FontAssets.MouseText.Value, instance.timeLeft.ToString(), position + new Vector2(Xoffset, 4) * scale, Color.Lerp(ringColorLerpEnd, Color.ForestGreen, 1 - instance.Completion), Color.Black, scale);
        }
        public override void DrawCompact(SpriteBatch spriteBatch, Vector2 position, float opacity, float scale)
        {
            Texture2D sprite = Request<Texture2D>(Texture).Value;
            Texture2D outline = Request<Texture2D>(OutlineTexture).Value;
            Texture2D overlay = Request<Texture2D>(OverlayTexture).Value;
            // Draw the outline
            spriteBatch.Draw(outline, position, null, OutlineColor * opacity, 0, outline.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            // Draw the icon
            spriteBatch.Draw(sprite, position, null, Color.White * opacity, 0, sprite.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            // Draw the small overlay
            int lostHeight = (int)Math.Ceiling(overlay.Height * AdjustedCompletion);
            Rectangle crop = new Rectangle(0, lostHeight, overlay.Width, overlay.Height - lostHeight);
            spriteBatch.Draw(overlay, position + Vector2.UnitY * lostHeight * scale, crop, OutlineColor * opacity * 0.9f, 0, sprite.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            float Xoffset = instance.timeLeft > 9 ? -10f : -5;
            DrawBorderStringEightWay(spriteBatch, FontAssets.MouseText.Value, instance.timeLeft.ToString(), position + new Vector2(Xoffset, 4) * scale, Color.Lerp(ringColorLerpStart, Color.OrangeRed, 1 - instance.Completion), Color.Black, scale);
        }
    }
}
