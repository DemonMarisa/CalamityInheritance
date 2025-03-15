using CalamityMod.Rarities;
using CalamityMod.UI.DraedonLogs;
using CalamityMod.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.UI;

namespace CalamityInheritance.Content.Items.Qol
{
    public class DraedonsPanel : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Panel";
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.rare = ModContent.RarityType<DarkOrange>();
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.myPlayer == player.whoAmI)
                CalPopupGUIManager.FlipActivityOfGUIWithType(typeof(QolPanel));
            return true;
        }
    }

}
