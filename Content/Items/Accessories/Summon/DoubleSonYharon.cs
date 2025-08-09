using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Placeables.Plates;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Summon
{
    public class DoubleSonYharon: CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.Summon";
        public override void SetStaticDefaults()
        {
            Type.ShimmerEach<AuricSoulArtifact>();
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.defense = 5;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.value = CIShopValue.RarityPriceCatalystViolet;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var usPlayer = player.CIMod();
            usPlayer.GodlySons = true;
            player.lifeRegen += 4;
            player.moveSpeed += 0.1f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ExodiumCluster>(25).
                AddIngredient<Plagueplate>(25).
                AddIngredient<YharonSoulFragment>(5).
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}
