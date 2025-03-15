using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Accessories.Melee
{
    public class YharimsInsignia : ModItem, ILocalizedModType
    {

        public new string LocalizationCategory => "Content.Items.Accessories.Melee";
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 38;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<BlueGreen>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer modPlayer = player.CIMod();
            modPlayer.YharimsInsignia = true;
            player.GetDamage<TrueMeleeDamageClass>() += 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WarriorEmblem).
                AddIngredient<NecklaceofVexation>().
                AddIngredient<BadgeofBravery>().
                AddIngredient<CoreofSunlight>(5).
                AddIngredient<DivineGeode>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();

            CreateRecipe().
                AddIngredient(ItemID.WarriorEmblem).
                AddIngredient<NecklaceofVexation>().
                AddIngredient<CalamityMod.Items.Accessories.BadgeofBravery>().
                AddIngredient<CoreofSunlight>(5).
                AddIngredient<DivineGeode>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
