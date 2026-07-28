using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Magic.MagicGun.Misc;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.MagicGun.Misc
{
    public class MagnusEye : CIMagic
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 76;
            Item.height = 48;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 100;
            Item.knockBack = 2f;
            Item.mana = 12;
            Item.autoReuse = true;
            Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 20;
            Item.shoot = ProjectileType<MagnusProj>();
            Item.shootSpeed = 7f;
            Item.UseSound = CISounds.LaserCannon;
            Item.rare = RarityType<MaliceChallengeDrop>();
            Item.value = CIShopValue.RarityMaliceDrop;
        }
    }
}