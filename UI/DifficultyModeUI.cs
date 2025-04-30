using CalamityInheritance.Content.Items.Placeables.Vanity;
using CalamityInheritance.System.Configs;
using CalamityInheritance.World;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static CalamityInheritance.Utilities.CIFunction;

namespace CalamityInheritance.UI
{
    public class DifficultyModeUI
    {
        internal float DefaultPosX = CIConfig.Instance.UIX;
        internal float DefaultPosY = CIConfig.Instance.UIX;
        private static Texture2D BG, Arma, Death, Malice, Rev, Rune;

        public static void Load()
        {
            BG = ModContent.Request<Texture2D>("CalamityInheritance/UI/ModeUITexture/ModeIndicatorArea", AssetRequestMode.ImmediateLoad).Value;
            Arma = ModContent.Request<Texture2D>("CalamityInheritance/UI/ModeUITexture/ModeIndicatorArma", AssetRequestMode.ImmediateLoad).Value;
            Death = ModContent.Request<Texture2D>("CalamityInheritance/UI/ModeUITexture/ModeIndicatorDeath", AssetRequestMode.ImmediateLoad).Value;
            Malice = ModContent.Request<Texture2D>("CalamityInheritance/UI/ModeUITexture/ModeIndicatorMalice", AssetRequestMode.ImmediateLoad).Value;
            Rev = ModContent.Request<Texture2D>("CalamityInheritance/UI/ModeUITexture/ModeIndicatorRev", AssetRequestMode.ImmediateLoad).Value;
            Rune = ModContent.Request<Texture2D>("CalamityInheritance/UI/ModeUITexture/ModeIndicatorRune", AssetRequestMode.ImmediateLoad).Value;
        }

        public static void Unload()
        {
            BG = Arma = Death = Malice = Rev = Rune = null;
        }
        public static void Draw(SpriteBatch spriteBatch, Player player)
        {
            float yCenter = Main.screenHeight * 0.5f;
            float xCenter = Main.screenWidth * 0.5f;
            Vector2 drawBGPosition = new Vector2(xCenter, yCenter);
            // 绘制底子
            spriteBatch.Draw(BG, drawBGPosition, null, Color.White, 0f, BG.Size() / 2, 1f, SpriteEffects.None, 0f);

            if (CIWorld.Malice)
                spriteBatch.Draw(Malice, drawBGPosition, null, Color.White, 0f, BG.Size() / 2, 1f, SpriteEffects.None, 0f);
            else if (CalamityWorld.death)
                spriteBatch.Draw(Death, drawBGPosition, null, Color.White, 0f, BG.Size() / 2, 1f, SpriteEffects.None, 0f);
            else if (CalamityWorld.revenge)
                spriteBatch.Draw(Rev, drawBGPosition, null, Color.White, 0f, BG.Size() / 2, 1f, SpriteEffects.None, 0f);

            if (CalamityWorld.armageddon)
                spriteBatch.Draw(Arma, drawBGPosition, null, Color.White, 0f, BG.Size() / 2, 1f, SpriteEffects.None, 0f);

            if (CIWorld.Defiled)
                spriteBatch.Draw(Rune, drawBGPosition, null, Color.White, 0f, BG.Size() / 2, 1f, SpriteEffects.None, 0f);
        }
    }
}
