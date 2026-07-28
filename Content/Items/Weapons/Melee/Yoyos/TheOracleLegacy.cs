using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.HeldProj.Melee.Yoyos;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.CraftingStations;
using CalamityInheritance.Core.Utils;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Yoyos
{
    public class TheOracleLegacy : CIMelee
    {
        public const int AuraBaseDamage = 68;
        public const int AuraMaxDamage = 150;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 15;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 50;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = 205;
            Item.knockBack = 4f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item1;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.shoot = ProjectileType<OracleYoyoLegacy>();
            Item.shootSpeed = 16f;

            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = RarityType<CatalystViolet>();
        }
        public override bool MeleePrefix()
        {
            return true;
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient<BurningRevelationLegacy>().
                    AddIngredient<LaceratorLegacy>().
                    AddIngredient<Verdant>().
                    AddIngredient(ItemID.Terrarian).
                    AddIngredient<TheObliteratorLegacy>().
                    AddIngredient<AuricBarold>().
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();


                CreateRecipe().
                    AddIngredient<BurningRevelationLegacy>().
                    AddIngredient<LaceratorLegacy>().
                    AddIngredient<Verdant>().
                    AddIngredient(ItemID.Terrarian).
                    AddIngredient<TheObliteratorLegacy>().
                    AddIngredient(CalamityMaterials.AuricBar, 5).
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient<BurningRevelationLegacy>().
                    AddIngredient<LaceratorLegacy>().
                    AddIngredient<Verdant>().
                    AddIngredient(ItemID.Terrarian).
                    AddIngredient<TheObliteratorLegacy>().
                    AddIngredient<AuricBarold>().
                    AddTile<DraedonsForgeoldTile>().
                    Register();
            }
        }
    }
}
