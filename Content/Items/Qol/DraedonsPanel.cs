using CalamityMod.Rarities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.UI;
using CalamityInheritance.UI.QolPanelTotal;
using CalamityInheritance.Content.Items.MiscItem;
using CalamityMod.Items.DraedonMisc;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.DraedonStructures;

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
                DraedonsPanelUIManager.FlipActivityOfGUIWithType();
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient<DraedonPowerCell>(100)
            .AddIngredient<DubiousPlating>(25)
            .AddIngredient<MysteriousCircuitry>(15)
            .AddIngredient(ItemID.Glass, 15)
            .AddRecipeGroup("AnyCopperBar", 5)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }

}
