using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class PlagueHive : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Mods.CalamityInheritance.Content.Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 48;
            Item.value = CIShopValue.RarityPriceCyan;
            Item.rare = ItemRarityID.Cyan;
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AlchemicalFlask>().
                AddIngredient(ItemID.HoneyComb).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.uberBees = true;
            modPlayer.alchFlask = true;
            player.buffImmune[ModContent.BuffType<Plague>()] = true;
        }
    }
}
