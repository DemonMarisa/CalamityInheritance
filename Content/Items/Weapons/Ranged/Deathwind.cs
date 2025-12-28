using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class Deathwind : CIRanged, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Type.ShimmerEach<ThreadOfEradication>(false);
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 82;
            Item.damage = 148;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DWArrow>();
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Arrow;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int index = 0; index < 4; ++index)
            {
                float SpeedX = velocity.X + Main.rand.Next(-20, 21) * 0.05f;
                float SpeedY = velocity.Y + Main.rand.Next(-20, 21) * 0.05f;
                if (CIConfig.Instance.AmmoConversion)
                {
                    Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<DWArrow>(), (int)(damage * 1.75), knockback, player.whoAmI);
                }
                else
                {
                    if (CalamityUtils.CheckWoodenAmmo(type, player))
                    {
                        Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, ModContent.ProjectileType<DWArrow>(), (int)(damage * 1.75), knockback, player.whoAmI);
                    }
                    else
                    {
                        int baseArrow = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, (int)(damage), knockback, player.whoAmI);
                        Main.projectile[baseArrow].noDropItem = true;
                    }
                }
            }
            return false;
        }
    }
}
