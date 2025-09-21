using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.Ranged.TrueScarlet;
using CalamityInheritance.Core;
using CalamityInheritance.Rarity.Special;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class R99 : CIRanged, ILocalizedModType
    {
        public const int CrackedShieldTime = 300;
        public const int FleshHitTime = 310;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.width = 172;
            Item.height = 74;
            Item.damage = 14;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.knockBack = 5f;
            Item.DamageType = DamageClass.Ranged;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = CIShopValue.RarityPricePureRed;
            Item.rare = ModContent.RarityType<TrueScarlet>();
            Item.noMelee = true;
            Item.channel = true;
            Item.useAmmo = AmmoID.Bullet;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<R99HeldProj>();
            //不要给这武器近程设计
            Item.shootSpeed = 12f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity * 0.1f, ModContent.ProjectileType<R99HeldProj>(), damage, knockback, player.whoAmI);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Minigun>().
                AddIngredient<CelestialObliterator>().
                AddRecipeGroup(CIRecipeGroup.DragonGun).
                AddIngredient<ExoPrism>(5).
                AddIngredient<AshesofAnnihilation>(5).
                AddTile<DraedonsForge>().
                Register();

            CreateRecipe().
                AddIngredient<CalamitousEssence>().
                DisableDecraft().
                Register();

        }
    }
}