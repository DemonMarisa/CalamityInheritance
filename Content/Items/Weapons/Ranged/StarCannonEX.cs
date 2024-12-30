using CalamityMod.Projectiles.Typeless;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Potions;
using CalamityInheritance.Content.Projectiles.Ranged;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class StarCannonEX : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 95;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 74;
            Item.height = 24;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.rare = ItemRarityID.Lime;
            Item.noMelee = true;
            Item.knockBack = 8f;
            Item.value = Item.buyPrice(0, 60, 0, 0);
            Item.UseSound = SoundID.Item9;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FallenStarProj>();
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.FallenStar;
        }

        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int num6 = Main.rand.Next(1, 3);
            for (int index = 0; index < num6; ++index)
            {
                float SpeedX = velocity.X + (float)Main.rand.Next(-15, 16) * 0.05f;
                float SpeedY = velocity.Y + (float)Main.rand.Next(-15, 16) * 0.05f;
                type = Utils.SelectRandom(Main.rand, new int[]
                {
                    ModContent.ProjectileType<AstralStar>(),
                    ProjectileID.Starfury,
                    ModContent.ProjectileType<FallenStarProj>()
                });
                int star = Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI);
                if (star.WithinBounds(Main.maxProjectiles))
                    Main.projectile[star].CalamityInheritance().forceRanged = true;
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SuperStarCannon).
                AddIngredient<AureusCell>(10).
                AddIngredient<StarblightSoot> (25).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
