using CalamityMod.Projectiles.Ranged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Materials;
using Terraria.DataStructures;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class AeriesLegacy : CIRanged, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Item.damage = 35;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 56;
            Item.height = 30;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<AeriesShockblastRound>();
            Item.useAmmo = AmmoID.Bullet;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(CIConfig.Instance.AmmoConversion)
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AeriesShockblastRound>(), damage, knockback, player.whoAmI);
            }
            else
            {
                if (type == ProjectileID.Bullet)
                    Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AeriesShockblastRound>(), damage, knockback, player.whoAmI);
                else
                    Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<CursedCapper>())
                .AddIngredient(ItemID.FallenStar, 3)
                .AddIngredient(ItemID.ShroomiteBar, 5)
                .AddIngredient(ModContent.ItemType<EssenceofSunlight>())
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
