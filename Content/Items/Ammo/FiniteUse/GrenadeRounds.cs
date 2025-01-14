using CalamityInheritance.Content.Projectiles.Typeless.FiniteUse;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Ammo.FiniteUse
{
    public class GrenadeRounds : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 200;
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 9;
            Item.consumable = true;
            Item.knockBack = 10f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<GrenadeRound>();
            Item.shootSpeed = 12f;
            Item.ammo = ModContent.ItemType<GrenadeRounds>(); // CONSIDER -- Would item.type work here instead of a self reference?
        }
    }
}
