using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.Buffs.PotionBuff;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Potions
{
    public class HolyWrathPotion : CIPotion
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 10;
        }
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 36;
            Item.useTurn = true;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Purple;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = CISoundID.SoundPotions;
            Item.consumable = true;
            Item.buffType = BuffType<HolyWrathBuff>();
            Item.buffTime = 300 * 60;
            Item.value = CIShopValue.RarityPricePurple;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(ItemID.WrathPotion).
                    AddIngredient(CalamityMaterials.UnholyEssence).
                    AddTile(TileID.Bottles).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient(ItemID.WrathPotion).
                    AddIngredient(ItemID.LunarBar).
                    AddTile(TileID.Bottles).
                    Register();
            }
        }
    }
}
