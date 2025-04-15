using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    [LegacyName("GalaxySmasherMelee")]
    public class MeleeTypeHammerGalaxySmasherLegacy : CIMelee, ILocalizedModType
    {
        
        public static int BaseDamage = 300;
        public static float Speed = 18f;

        public override void SetStaticDefaults()
        {
            if(CIServerConfig.Instance.CustomShimmer == true)
            {
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<GalaxySmasher>()] = ModContent.ItemType<MeleeTypeHammerGalaxySmasherLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<MeleeTypeHammerGalaxySmasherLegacy>()] = ModContent.ItemType<GalaxySmasher>();
            }
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 86;
            Item.height = 72;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = BaseDamage;
            Item.knockBack = 9f;
            Item.useAnimation = 13;
            Item.useTime = 13;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = CISoundID.SoundWeaponSwing;

            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;

            Item.shoot = ModContent.ProjectileType<MeleeTypeHammerGalaxySmasherLegacyProj>();
            Item.shootSpeed = Speed;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {

            if(CIServerConfig.Instance.CustomShimmer == false)
            {
            CreateRecipe().
                AddIngredient<StellarContempt>().
                AddIngredient<CosmiliteBar>(10).
                AddTile<CosmicAnvil>().
                Register();
            }
                CreateRecipe().
                    AddIngredient<MeleeTypeHammerStellarContemptLegacy>().
                    AddIngredient<CosmiliteBar>(10).
                    AddTile<CosmicAnvil>().
                    Register();
        }
    }
}
