using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.AncientAstral
{
    [AutoloadEquip(EquipType.Legs)]
    public class AncientAstralLeggings : CIArmor
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
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
            player.LAP().MaxLifeAdditive += 20;
            player.GetCritChance<ThrowingDamageClass>() += 5;
            player.lifeRegen += 1;
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(ItemID.MeteoriteBar, 10).
                    AddIngredient(CalamityMaterials.LifeAlloy, 5).
                    AddIngredient(CalamityMaterials.StarblightSoot, 10).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {

            }
        }
    }
}