using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Tools
{
    public class XerocPickaxe : CITools
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.height = Item.width = 54;
            Item.DamageType = RogueDamage.Instance;
            Item.pick = 225;
            Item.useTime = 5;
            Item.useAnimation = 10;
            Item.tileBoost += 6;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.damage = 300;
            Item.knockBack = 5f;
            Item.rare = ItemRarityID.Red;
            Item.value = CIShopValue.RarityPriceRed;
            Item.autoReuse = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<NebulaBar>(9).
                AddIngredient(ItemID.LunarBar, 9).
                AddIngredient<GalacticaSingularity>(1).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}