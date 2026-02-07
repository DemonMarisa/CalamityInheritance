using CalamityInheritance.Utilities;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Combats
{
    internal class LumenousAmuletLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CIMod().LumenousAmulet = true;
            player.buffImmune[ModContent.BuffType<RiptideDebuff>()] = true;
            player.buffImmune[ModContent.BuffType<CrushDepth>()] = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AbyssalAmulet>().
                AddIngredient<Lumenyl>(15).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
