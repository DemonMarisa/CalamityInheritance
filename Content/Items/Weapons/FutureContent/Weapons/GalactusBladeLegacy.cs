using CalamityInheritance.Content.Projectiles.FutureContent.GalacticStar;
using CalamityInheritance.Rarity;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.FutureContent.Weapons
{
    public class GalactusBladeLegacy : CIMelee, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 58;
            Item.damage = 84;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 17;
            Item.knockBack = 6f;
            Item.UseSound = SoundID.Item105;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = RarityType<BlueGreen>();
            Item.shoot = ProjectileType<GalacticaStar>();
            Item.shootSpeed = 23f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float cometSpeed = Item.shootSpeed;
            for (int i = 0; i < 5; i++)
            {
                Vector2 realPlayerPos = new(player.Center.X + (Main.rand.NextFloat(201) * -(float)player.direction) + (Main.MouseWorld.X - player.position.X), player.MountedCenter.Y - 600f);
                realPlayerPos.X = (realPlayerPos.X + player.Center.X) / 2f + Main.rand.NextFloat(-200f, 201f);
                realPlayerPos.Y -= 100 * i;
                float mouseXDist = Main.MouseWorld.X - realPlayerPos.X;
                float mouseYDist = Main.MouseWorld.Y - realPlayerPos.Y;
                if (mouseYDist < 0f)
                {
                    mouseYDist *= -1f;
                }
                if (mouseYDist < 20f)
                {
                    mouseYDist = 20f;
                }
                float mouseDistance = new Vector2 (mouseXDist,mouseYDist).Length();
                mouseDistance = cometSpeed / mouseDistance;
                mouseXDist *= mouseDistance;
                mouseYDist *= mouseDistance;
                float speedX4 = mouseXDist + Main.rand.Next(-100, 101) * 0.02f;
                float speedY5 = mouseYDist + Main.rand.Next(-100, 101) * 0.02f;
                int projectile = Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, speedX4, speedY5, type, damage, knockback, player.whoAmI, 0f, Main.rand.Next(10));
            }
            return false;
        }
    }
}