using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Tools
{
    public class WulfrumAxe : CITools
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 7;
            Item.DamageType = DamageClass.Melee;
            Item.width = 62;
            Item.height = 48;
            Item.useTime = 8;
            Item.useAnimation = 16;
            Item.useTurn = true;
            Item.axe = 7;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3f;
            Item.value = CIShopValue.RarityPriceBlue;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe()
                .AddIngredient(CalamityMaterials.WulfrumMetalScrap, 4)
                .AddTile(TileID.WorkBenches)
                .Register();
            }
        }
    }
}