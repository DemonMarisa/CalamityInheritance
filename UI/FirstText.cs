using CalamityInheritance.Content.Items.Placeables.Vanity;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityInheritance.World;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using static CalamityInheritance.Utilities.CIFunction;

namespace CalamityInheritance.UI
{
    public class FirstText
    {
        public static void Draw(SpriteBatch spriteBatch)
        {
            if (CIConfig.Instance.TurnOffFirstText == false)
                return;

            float drawPositionY = Main.screenHeight * 0.5f;
            float drawPositionX = Main.screenWidth * 0.5f;
            // 如果不想或者懒得新建存储，可以直接用这个透明材质
            Texture2D InvisibleUI = ModContent.Request<Texture2D>("CalamityInheritance/UI/DraedonsTexture/InvisibleUI").Value;
            string key = Language.GetTextValue("Mods.CalamityInheritance.ConfigsMessage.FirstText");
            DrawText(spriteBatch, key, 1f, 1f, drawPositionX, drawPositionY, 1f, Color.Gold, Color.DarkGoldenrod, InvisibleUI, 15, 800f, 1.2f);

            Main.NewText("BeDraw");
        }
    }
}
