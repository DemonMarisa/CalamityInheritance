using CalamityInheritance.Content.Projectiles.Melee.Yoyos;
using CalamityInheritance.Rarity;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Yoyos
{
    public class BurningRevelationLegacy : GeneralWeaponClass
    {
        public override WeaponDamageType UseDamageClass => WeaponDamageType.Melee;
        public override void SetStaticDefaults()
        {
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 38;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = 120;
            Item.knockBack = 7.5f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ProjectileType<BurningRevelationYoyoLegacy>();
            Item.shootSpeed = 16f;

            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = RarityType<BlueGreen>();
        }
    }
}
