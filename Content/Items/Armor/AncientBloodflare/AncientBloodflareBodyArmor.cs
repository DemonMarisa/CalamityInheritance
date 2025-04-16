using CalamityInheritance.Rarity;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientBloodflare
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientBloodflareBodyArmor : CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = ModContent.RarityType<BlueGreen>();
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.defense = 45;
        }
        
        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 300;
            player.statManaMax2 += 300;
            if (player.lavaWet == true)
            {
                player.statDefense += 30;
                player.lifeRegen += 10;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BloodflareBodyArmor>(1).
                AddIngredient<BloodstoneCore>(25).
                AddIngredient<RuinousSoul>(25).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}