using CalamityMod;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientAstral
{
    [AutoloadEquip(EquipType.Legs)]
    public class AncientAstralLeggings: ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 14;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.defense = 16;
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 +=20;
            player.GetCritChance<RogueDamageClass>() += 5;
            player.lifeRegen += 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MeteoriteBar, 10).
                AddIngredient<LifeAlloy>(5).
                AddIngredient<StarblightSoot>(10).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}