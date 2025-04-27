using CalamityMod.Items.Materials;
using CalamityMod;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Buffs.Potions;
using CalamityInheritance.Content.Items.Potions.CIPotions;

namespace CalamityInheritance.Content.Items.Potions
{
    public class ProfanedRagePotion : CIPotion, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Potions";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 10;
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 42;
            Item.useTurn = true;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Purple;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = CISoundID.SoundPotions;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<ProfanedRageBuff>();
            Item.buffTime = CalamityUtils.SecondsToFrames(300f);
            Item.value = CIShopValue.RarityPricePurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.RagePotion).
                AddIngredient<UnholyEssence>().
                AddTile(TileID.AlchemyTable).
                AddConsumeItemCallback(Recipe.ConsumptionRules.Alchemy).
                Register();

            CreateRecipe().
                AddIngredient(ItemID.BottledWater).
                AddIngredient<BloodOrb>(40).
                AddIngredient<UnholyEssence>().
                AddTile(TileID.AlchemyTable).
                Register();
        }
    }
}
