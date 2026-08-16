using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.AncientGodSlayer
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientGodSlayerChestplate : CIArmor
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 80;
        }
        public override void UpdateEquip(Player player)
        {
            var modPlayer = player.CI();
            player.thorns = 10f;
            player.LAP().MaxLifeAdditive += 500;
            player.LAP().MaxManaAdditive += 500;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.CosmiliteBar, 40).
                    AddIngredient(CalamityMaterials.AscendantSpiritEssence, 15).
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
            else
            {

            }
        }
    }
}