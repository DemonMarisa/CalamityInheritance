using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Ranged;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class PumplerLegacy : CIRanged, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Type.ShimmerEach<Pumpler>();
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.DamageType = DamageClass.Ranged;
            Item.height = 36;
            Item.width = 62;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1.5f;
            Item.rare = ItemRarityID.Green;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.autoReuse = true;
            Item.shoot = 10;
            Item.shootSpeed = 11f;
            Item.useAmmo = 97;
        }
        public override void AddRecipes()
        {

        }
    }
}