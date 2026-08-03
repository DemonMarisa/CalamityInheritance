using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Typeless.Weapon.Attack;
using CalamityInheritance.Content.Rarity.Special;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Typeless.Attack

{
    public class AncientMurasama : CITypeless
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 72;
            Item.damage = 1200;
            Item.knockBack = 10;
            Item.value = Item.buyPrice(10,0,0,0);
            Item.rare = RarityType<MurasamRed>();
            Item.shootSpeed = 15f;
            Item.height = 78;
            Item.DamageType = DamageClass.Generic;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 5;
            Item.autoReuse = false;
            Item.shoot = ProjectileType<AncientMurasamaProj>();
        }
    }
}
