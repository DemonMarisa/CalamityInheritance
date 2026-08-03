using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.ReaverLegacy
{
    [AutoloadEquip(EquipType.Legs)]
    public class ReaverCuissesRevamped : CIArmor
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<GenericDamageClass>() += 5;
            player.GetDamage<GenericDamageClass>() += 0.05f;
            player.moveSpeed += 0.12f;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.PerennialBar, 5).
                    AddIngredient(ItemID.JungleSpores, 4).
                    AddIngredient<CoreofEleum>().
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient(ItemID.ChlorophyteBar, 5).
                    AddIngredient(ItemID.JungleSpores, 4).
                    AddIngredient<CoreofEleum>().
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
        }
    }
}
