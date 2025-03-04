using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Projectiles.Melee;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    [LegacyName("FallenPaladinsHammerOld")]
    public class MeleeTypeHammerFallenPaladinsLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public override void SetStaticDefaults()
        {
            if (CIConfig.Instance.CustomShimmer == true)
            {
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<MeleeTypeHammerFallenPaladinsLegacy>()] = ModContent.ItemType<FallenPaladinsHammer>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<FallenPaladinsHammer>()] = ModContent.ItemType<MeleeTypeHammerFallenPaladinsLegacy>();
            }
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.damage = 87;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 13;
            Item.knockBack = 20f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.height = 28;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<MeleeTypeHammerFallenPaladinsLegacyProj>();
            Item.shootSpeed = 14f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PaladinsHammer).
                AddIngredient<MeleeTypeHammerPwnageLegacy>(). //改成旧锤子
                AddIngredient<ScoriaBar>(5).
                AddIngredient<AshesofCalamity>(5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
