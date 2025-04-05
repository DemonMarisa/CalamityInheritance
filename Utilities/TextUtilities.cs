using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Utilities
{
    public static partial class CIFunction
    {
        /// <summary>
        /// 在聊天框中发送文本，支持多人模式。
        /// </summary>
        public static void BroadcastLocalizedText(string key, Color? textColor = null)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText(Language.GetTextValue(key), textColor ?? Color.White);
            else if (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.MultiplayerClient)
                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), textColor ?? Color.White);
        }
    }
}
