using CalamityInheritance.Rarity;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientTarragon
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientTarragonBreastplate : CIArmor, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.lifeRegen = 3;
            Item.rare = ModContent.RarityType<BlueGreen>();
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.defense = 45;
        }
        

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 400;
            player.statManaMax2 += 200;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<TarragonBreastplate>().
                AddIngredient<UelibloomBar>(25).
                AddIngredient<DivineGeode>(25).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}