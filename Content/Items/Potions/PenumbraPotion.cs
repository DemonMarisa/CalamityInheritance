using CalamityMod.Items.Materials;
using CalamityMod;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Buffs.Potions;
using CalamityInheritance.Content.Items.Potions.CIPotions;

namespace CalamityInheritance.Content.Items.Potions
{
    public class PenumbraPotion : CIPotion, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Potions";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 10;
        }
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
            Item.UseSound = CISoundID.SoundPotions;
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
