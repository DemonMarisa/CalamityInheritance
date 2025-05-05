using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items.Weapons.Ranged;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Sounds.Custom;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class MagnaStrikerLegacy : CIRanged, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 72;
            Item.height = 38;
            Item.useTime = 5;
            Item.useAnimation = 20;
            Item.reuseDelay = 6;
            Item.useLimitPerAnimation = 4;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2.25f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = CISoundMenu.OpalStriker;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<OpalStrikeLegacy>();
            Item.shootSpeed = 15f;
            Item.useAmmo = AmmoID.None;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int pType = Main.rand.NextBool() ? type : ModContent.ProjectileType<MagnaStrikerProj>();
            Projectile.NewProjectile(source, position, velocity, pType, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(CIRecipeGroup.OpalStriker).
                AddRecipeGroup(CIRecipeGroup.MagnaCannon).
                AddRecipeGroup("AnyAdamantiteBar", 6).
                AddIngredient(ItemID.Ectoplasm, 5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
