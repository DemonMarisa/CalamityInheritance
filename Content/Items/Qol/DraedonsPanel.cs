using CalamityMod.Rarities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.UI;
using CalamityInheritance.UI.QolPanelTotal;
using CalamityInheritance.Content.Items.MiscItem;

namespace CalamityInheritance.Content.Items.Qol
{
    public class DraedonsPanel : CIMisc, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Panel";
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.rare = ModContent.RarityType<DarkOrange>();
            Item.useAnimation = Item.useTime = 20;
            //给予一点reuseDelay以避免玩家无意间二次打开UI
            Item.reuseDelay = 20;
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
