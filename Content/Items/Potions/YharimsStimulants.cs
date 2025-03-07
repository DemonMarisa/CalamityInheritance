using CalamityMod.Items.Materials;
using CalamityMod;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Buffs.Potions;

namespace CalamityInheritance.Content.Items.Potions
{
    public class YharimsStimulants : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Potions";
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useTurn = true;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Orange;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = CISoundID.SoundPotions;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<YharimPower>();
            Item.buffTime = CalamityUtils.SecondsToFrames(1800f);
            Item.value = CIShopValue.RarityPriceOrange;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("AnyFood").
                AddIngredient(ItemID.EndurancePotion).
                AddIngredient(ItemID.IronskinPotion).
                AddIngredient(ItemID.SwiftnessPotion).
                AddIngredient(ItemID.TitanPotion).
                AddTile(TileID.AlchemyTable).
                Register();

            CreateRecipe().
                AddIngredient(ItemID.BottledWater).
                AddIngredient<BloodOrb>(50).
                AddTile(TileID.AlchemyTable).
                Register();
        }
    }
}
