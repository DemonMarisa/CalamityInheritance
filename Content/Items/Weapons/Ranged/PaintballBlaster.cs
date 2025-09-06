using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class PaintballBlaster : CIRanged, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Type.ShimmerEach<SpeedBlaster>();
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 26;
            Item.damage = 26;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 4;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = CIShopValue.RarityPricePink;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 24;
            Item.knockBack = 2.25f;
            Item.shootSpeed = 26f;
            Item.shoot = ProjectileID.PainterPaintball;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source,position, velocity, ProjectileID.PainterPaintball, damage, knockback, player.whoAmI, 0f, Main.rand.Next(12) / 6f);
            return false;
        }
    }
}