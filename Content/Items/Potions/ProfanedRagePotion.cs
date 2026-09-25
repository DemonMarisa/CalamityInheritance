using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.Buffs.PotionBuff;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Potions
{
    public class ProfanedRagePotion : CIPotion
    {
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
            Item.buffType = BuffType<ProfanedRageBuff>();
            Item.buffTime = CIUtils.SecondsToFrames(300f);
            Item.value = CIShopValue.RarityPricePurple;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.RagePotion).
                AddIngredient<UnholyEssenceLegacy>().
                AddTile(TileID.AlchemyTable).
                AddConsumeItemCallback(Recipe.ConsumptionRules.Alchemy).
                Register();
        }
    }
}
