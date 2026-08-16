using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Accessories.Misc;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.AncientSilva
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientSilvaArmor : CIArmor
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
            Item.defense = 60;
        }
        public override void UpdateEquip(Player player)
        {
            player.LAP().MaxLifeAdditive += 250;
            player.LAP().MaxManaAdditive += 250;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.PlantyMush, 24).
                    AddIngredient(CalamityMaterials.EffulgentFeather, 30).
                    AddIngredient(CalamityMaterials.DarksunFragment, 15).
                    AddIngredient<LeadCore>().
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
            else
            {

            }
        }
    }
}