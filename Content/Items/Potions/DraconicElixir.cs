using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.Buffs.PotionBuff;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Potions
{
    public class DraconicElixir : CIPotion
    {
        public int frameCounter = 0;
        public int frame = 0;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 10;
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 36;
            Item.useTurn = true;
            Item.maxStack = 9999;
            Item.rare = RarityType<CatalystViolet>();
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = CISoundID.SoundPotions;
            Item.consumable = true;
            Item.buffType = BuffType<DraconicSurgeBuff>();
            Item.buffTime = 480 * 60;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
        }

        public override void AddRecipes()
        {
            //CreateRecipe().
            //    AddIngredient(ItemID.BottledWater).
            //    AddIngredient<YharonSoulFragment>().
            //    AddIngredient(ItemID.Daybloom).
            //    AddIngredient(ItemID.Moonglow).
            //    AddIngredient(ItemID.Fireblossom).
            //    AddTile(TileID.Bottles).
            //    Register();
        }
    }
}
