using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.HeldProj.Melee.Spears;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Spear
{
    public class HolidayHalberd : CIMelee
    {

        public override void SetDefaults()
        {
            Item.width = 70;
            Item.damage = 98;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useTurn = false;
            Item.knockBack = 7.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 72;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.shootSpeed = 12f;

            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ProjectileType<HolidayHalberdProj>();
        }
    }
}
