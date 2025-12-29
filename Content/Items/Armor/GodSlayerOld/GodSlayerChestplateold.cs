using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityMod.CalPlayer;
namespace CalamityInheritance.Content.Items.Armor.GodSlayerOld
{
    [AutoloadEquip(EquipType.Body)]
    public class GodSlayerChestplateold : CIArmor, ILocalizedModType
    {
        
        public const int DashIFrames = 12;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 41;
            Item.rare = RarityType<DeepBlue>();
        }

        public override void UpdateEquip(Player player)
        {
            CalamityPlayer modPlayer1 = player.Calamity();
            CalamityInheritancePlayer modPlayer = player.CIMod();
            modPlayer.GodSlayerReflect = true;
            modPlayer.GodSlayerDMGprotect = true;
            player.thorns += 0.5f;
            player.statLifeMax2 += 60;
            player.GetDamage<GenericDamageClass>() += 0.1f;
            player.GetCritChance<GenericDamageClass>() += 6;
            player.moveSpeed += 0.15f;
        }
        public static string GetSpecial(int mode)
        {
            var modItme = ItemLoader.GetItem(ItemType<GodSlayerChestplateold>());
            string getValue = mode switch
            {
                1 => "OnlyReborn",
                2 => "OnlyDash",
                _ => "Both",
            };
            return modItme.GetLocalization(getValue).ToString();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CosmiliteBar>(15).
                AddIngredient<AscendantSpiritEssence>(3).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
