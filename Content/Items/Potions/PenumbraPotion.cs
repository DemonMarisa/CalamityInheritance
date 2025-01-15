using CalamityMod.Items.Materials;
using CalamityMod;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Buffs.Potions;

namespace CalamityInheritance.Content.Items.Potions
{
    public class PenumbraPotion : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Potions";
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 30;
            Item.useTurn = true;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Lime;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<PenumbraBuff>();
            Item.buffTime = CalamityUtils.SecondsToFrames(480f);
            Item.value = CIShopValue.RarityPriceLime;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.BottledWater).
                AddIngredient<SolarVeil>(3).
                AddIngredient(ItemID.LunarTabletFragment).
                AddTile(TileID.AlchemyTable).
                AddConsumeItemCallback(Recipe.ConsumptionRules.Alchemy).
                Register();

            CreateRecipe().
                AddIngredient(ItemID.BottledWater).
                AddIngredient<BloodOrb>(30).
                AddIngredient<SolarVeil>().
                AddTile(TileID.AlchemyTable).
                Register();
        }
    }
}
