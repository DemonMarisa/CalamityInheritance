using CalamityInheritance.Content.Projectiles.Pets;
using CalamityInheritance.Utilities;
using CalamityMod.Buffs.Pets;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Vanitys
{
    public class DaawnlightSpiritOriginLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Vanity";
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
            player.CIMod().CanHaveDaawnlightLegacy = true;
            if (!player.HasProj<DaawnlightSpiritOriginMinionLegacy>())
            {
                Projectile.NewProjectile(Item.GetSource_FromThis(), player.Center, Vector2.UnitX, ProjectileType<DaawnlightSpiritOriginMinionLegacy>(), 0, 0, player.whoAmI);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DeadshotBrooch>().
                AddIngredient<MysteriousCircuitry>(15).
                AddIngredient<DubiousPlating>(15).
                AddIngredient(ItemID.LunarBar, 10).
                AddIngredient<GalacticaSingularity>(4).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
