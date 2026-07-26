using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.HeldProj.Melee.Flails;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Flails
{
    public class UrchinFlail : CIMelee
    {
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 36;
            Item.damage = 20;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 6f;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.channel = true;
            Item.shoot = ProjectileType<UrchinBall>();
            Item.shootSpeed = 16f;
        }
    }
}
