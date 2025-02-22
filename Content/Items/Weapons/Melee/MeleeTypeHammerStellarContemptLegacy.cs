using CalamityMod.Items.Weapons.Melee;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Melee;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    [LegacyName("StellarContemptOld")]
    public class MeleeTypeHammerStellarContemptLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public static int BaseDamage = 300;
        public static float Speed = 18f;

        public override void SetStaticDefaults()
        {
            if(CalamityInheritanceConfig.Instance.CustomShimmer == true)
            {
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<StellarContempt>()] = ModContent.ItemType<MeleeTypeHammerStellarContemptLegacy>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<MeleeTypeHammerStellarContemptLegacy>()] = ModContent.ItemType<StellarContempt>();
            }
        }

        public override void SetDefaults()
        {
            Item.width = 74;
            Item.height = 74;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = BaseDamage;
            Item.knockBack = 9f;
            Item.useTime = 13;
            Item.useAnimation = 13;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;

            Item.shoot = ModContent.ProjectileType<MeleeTypeHammerStellarContemptLegacyProj>();
            Item.shootSpeed = Speed;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MeleeTypeHammerFallenPaladinsLegacy>().
                AddIngredient(ItemID.LunarBar, 5).
                AddIngredient(ItemID.FragmentSolar, 10).
                AddIngredient(ItemID.FragmentNebula, 10).
                AddTile(TileID.LunarCraftingStation).
                Register();



            if(CalamityInheritanceConfig.Instance.CustomShimmer == false)
            {
                CreateRecipe().
                    AddIngredient<FallenPaladinsHammer>().
                    AddIngredient(ItemID.LunarBar, 5).
                    AddIngredient(ItemID.FragmentSolar, 10).
                    AddIngredient(ItemID.FragmentNebula, 10).
                    AddTile(TileID.LunarCraftingStation).
                    Register();
            }
        }
    }
}
