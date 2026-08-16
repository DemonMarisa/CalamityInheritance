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
    [AutoloadEquip(EquipType.Legs)]
    public class AncientGodSlayerLeggings : CIArmor
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
            Item.defense = 20;
        }
        public override void UpdateEquip(Player player)
        {
            player.LAP().MaxLifeAdditive += 200;
            player.moveSpeed += 0.50f;
            player.lavaImmune = true;
            player.ignoreWater = true;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.CosmiliteBar, 20).
                    AddIngredient(CalamityMaterials.AscendantSpiritEssence, 10).
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
            else
            {

            }
        }
    }
}