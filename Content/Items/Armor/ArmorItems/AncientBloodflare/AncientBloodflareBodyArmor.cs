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
    [AutoloadEquip(EquipType.Body)]
    public class AncientBloodflareBodyArmor : CIArmor
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = RarityType<BlueGreen>();
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.defense = 45;
        }

        public override void UpdateEquip(Player player)
        {
            player.LAP().MaxLifeAdditive += 300;
            player.LAP().MaxManaAdditive += 300;
            if (player.lavaWet == true)
            {
                player.statDefense += 30;
                player.lifeRegen += 10;
            }
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient<BloodstoneCore>(25).
                    AddIngredient(CalamityMaterials.RuinousSoul, 25).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
            else
            {

            }
        }
    }
}