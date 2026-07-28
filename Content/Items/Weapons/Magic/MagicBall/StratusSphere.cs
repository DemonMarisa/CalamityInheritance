using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.HeldProj.Magic.MagicBall;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.MagicBall
{
    public class StratusSphere : CIMagic
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 281;
            Item.DamageType = DamageClass.Magic;
            Item.width = 22;
            Item.height = 24;
            Item.useTime = 45;
            Item.useAnimation = 45;
            Item.shoot = ProjectileType<StratusSphereHold>();
            Item.shootSpeed = 3.5f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.mana = 30;
            Item.knockBack = 2;
            Item.noUseGraphic = true;
            Item.channel = true;
            Item.noMelee = true;
            Item.value = CIShopValue.RarityPriceAbsoluteGreen;
            Item.rare = RarityType<AbsoluteGreen>();
        }
        public override void OnConsumeMana(Player player, int manaConsumed) => player.statMana += manaConsumed;

        // This weapon uses a holdout projectile.
        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, ProjectileType<StratusSphereHold>(), damage, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.Lumenyl, 55).
                    AddIngredient(CalamityMaterials.RuinousSoul, 4).
                    AddIngredient(CalamityMaterials.ExodiumCluster, 12).
                    AddIngredient(ItemID.NebulaArcanum).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
            else
            {

            }
        }
    }
}
