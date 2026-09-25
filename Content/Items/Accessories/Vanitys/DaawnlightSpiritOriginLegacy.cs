using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Vanitys;
using CalamityInheritance.Core.Path;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Vanitys
{
    public class DaawnlightSpiritOriginLegacy : ModItem, ILocalizedModType
    {
        public override string LocalizationCategory => LocalizationPath.VanityItem;
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 32;
            Item.rare = ItemRarityID.Master;
            Item.vanity = true;
            Item.accessory = true;
        }
        public override void UpdateVanity(Player player)
        {
            if (!player.HasProj<DaawnlightSpiritOriginMinionLegacy>())
            {
                Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, Vector2.UnitX, ProjectileType<DaawnlightSpiritOriginMinionLegacy>(), 0, 0, player.whoAmI);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.LunarBar, 10).
                AddIngredient<GalacticaSingularity>(4).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
