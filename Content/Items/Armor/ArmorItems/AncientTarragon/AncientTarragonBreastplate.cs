using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.AncientTarragon
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientTarragonBreastplate : CIArmor
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.lifeRegen = 3;
            Item.rare = RarityType<BlueGreen>();
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.defense = 45;
        }


        public override void UpdateEquip(Player player)
        {
            player.LAP().MaxLifeAdditive += 400;
            player.LAP().MaxManaAdditive += 300;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.UelibloomBar, 25).
                    AddIngredient(CalamityMaterials.DivineGeode, 25).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
            else
            {

            }
        }
    }
}