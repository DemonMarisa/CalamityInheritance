using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Projectiles.Ammo.FiniteUse;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Ammos.FiniteUse
{
    public class GrenadeRounds : CIAmmo
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }
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
            Item.shoot = ProjectileType<GrenadeRound>();
            Item.shootSpeed = 12f;
            Item.ammo = ItemType<GrenadeRounds>(); // CONSIDER -- Would item.type work here instead of a self reference?
        }
    }
}
