using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.AncientBloodflare
{
    [AutoloadEquip(EquipType.Legs)]
    public class AncientBloodflareCuisses : CIArmor
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = RarityType<BlueGreen>();
            Item.defense = 20;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.3f;
            player.LAP().MaxLifeAdditive += 100;
            player.lavaImmune = true;
            player.ignoreWater = true;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient<BloodstoneCore>(10).
                    AddIngredient(CalamityMaterials.RuinousSoul, 10).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
            else
            {

            }
        }
    }
}