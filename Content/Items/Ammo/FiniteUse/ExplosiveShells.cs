using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Typeless.FiniteUse;

namespace CalamityInheritance.Content.Items.Ammo.FiniteUse
{
    public class ExplosiveShells : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 6;
            Item.consumable = true;
            Item.knockBack = 10f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<ExplosiveShotgunShell>();
            Item.shootSpeed = 12f;
            Item.ammo = ModContent.ItemType<ExplosiveShells>(); // CONSIDER -- Would item.type work here instead of a self reference?
        }
    }
}
