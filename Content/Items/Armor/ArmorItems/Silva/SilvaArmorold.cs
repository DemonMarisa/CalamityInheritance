using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Accessories.Misc;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.Silva
{
    [AutoloadEquip(EquipType.Body)]
    public class SilvaArmorold : CIArmor
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 44;
            Item.rare = RarityType<DeepBlue>();
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 80;
            player.GetDamage<GenericDamageClass>() += 0.12f;
            player.GetCritChance<GenericDamageClass>() += 8;
            player.moveSpeed += 0.20f;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.PlantyMush, 12).
                    AddIngredient(CalamityMaterials.EffulgentFeather, 10).
                    AddIngredient(CalamityMaterials.DarksunFragment, 10).
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
