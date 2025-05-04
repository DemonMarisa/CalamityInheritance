using CalamityMod.Items;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityMod.Items.Materials;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class PearlGodLegacy : CIRanged, ILocalizedModType
    {

        private const int defaultSpread = 1;
        private int spread = defaultSpread;
        private bool finalShot = false;

        public override void SetDefaults()
        {
            Item.width = 80;
            Item.height = 46;
            Item.damage = 120;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item41;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.shoot = ModContent.ProjectileType<ShockblastRoundLegacy>();
            Item.useAmmo = AmmoID.Bullet;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (spread > 6)
            {
                spread = defaultSpread;
                finalShot = true;
            }


            float rotation = MathHelper.ToRadians(spread);
            if (!finalShot)
            {
                int totalLoops = 1;
                switch (spread)
                {
                    case 1:
                    case 2:
                        break;
                    case 3:
                    case 4:
                        totalLoops = 2;
                        break;
                    case 5:
                    case 6:
                        totalLoops = 3;
                        break;
                }

                for (int i = 0; i < totalLoops; i++)
                {
                    int bullet1 = Projectile.NewProjectile(source, position, velocity.RotatedBy(-rotation * (i + 1)), type, (int)(damage * 0.5), knockback * 0.5f, player.whoAmI);
                    Main.projectile[bullet1].extraUpdates += spread;
                    int bullet2 = Projectile.NewProjectile(source, position, velocity.RotatedBy(+rotation * (i + 1)), type, (int)(damage * 0.5), knockback * 0.5f, player.whoAmI);
                    Main.projectile[bullet2].extraUpdates += spread;
                }

                int shockblast = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ShockblastRoundLegacy>(), damage, knockback, player.whoAmI, 0f, spread);
                Main.projectile[shockblast].extraUpdates += spread;

                spread++;
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    int bigShockblast = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ShockblastRoundLegacy>(), damage * 2, knockback * 2f, player.whoAmI, 0f, 10f);
                    Main.projectile[bigShockblast].extraUpdates += 9;
                }

                finalShot = false;
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AeriesLegacy>().
                AddIngredient<LifeAlloy>(5).
                AddIngredient<RuinousSoul>(2).
                AddIngredient(ItemID.WhitePearl).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
