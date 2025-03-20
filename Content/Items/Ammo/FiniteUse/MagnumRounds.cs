using CalamityInheritance.Content.Projectiles.Typeless.FiniteUse;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Items.Ammo.FiniteUse
{
    public class MagnumRounds : CIAmmo, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Ammo";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.crit += 4;
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 12;
            Item.consumable = true;
            Item.knockBack = 8f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<MagnumRound>();
            Item.shootSpeed = 12f;
            Item.ammo = ModContent.ItemType<MagnumRounds>(); // CONSIDER -- Would item.type work here instead of a self reference?
        }
    }
}
