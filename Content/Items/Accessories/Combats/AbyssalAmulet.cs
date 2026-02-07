using CalamityInheritance.Utilities;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.FurnitureAcidwood;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Combats
{
    public class AbyssalAmulet : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CIMod().AbyssalAmuletLegacy = true; ;
            player.buffImmune[BuffType<RiptideDebuff>()] = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Acidwood>(12).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
