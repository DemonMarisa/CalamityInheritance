using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;

namespace CalamityInheritance.Content.Items.Tools
{
    public class WulfrumPickaxe : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Tools";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 5;
            Item.DamageType = DamageClass.Melee;
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 7;
            Item.useAnimation = 14;
            Item.useTurn = true;
            Item.pick = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1f;
            Item.value = CIShopValue.RarityPriceBlue;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient<WulfrumMetalScrap>(8)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
    }
}