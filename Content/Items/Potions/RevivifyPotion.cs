using CalamityMod.Items.Materials;
using CalamityMod;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Fishing.AstralCatches;
using CalamityInheritance.Buffs.Potions;
using CalamityInheritance.Content.Items.Potions.CIPotions;

namespace CalamityInheritance.Content.Items.Potions
{
    public class RevivifyPotion : CIPotion, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Potions";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 10;
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 36;
            Item.useTurn = true;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Orange;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = CISoundID.SoundPotions;
            Item.consumable = true;
            Item.buffType = BuffType<Revivify>();
            Item.buffTime = CalamityUtils.SecondsToFrames(180f);
            Item.value = CIShopValue.RarityPriceOrange;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.HolyWater, 1).
                AddIngredient<StarblightSoot>(4).
                AddIngredient(ItemID.CrystalShard, 1).
                AddIngredient<EssenceofSunlight>().
                AddIngredient<TwinklingPollox>().
                AddTile(TileID.AlchemyTable).
                Register();
            
            CreateRecipe().
                AddIngredient(ItemID.HolyWater, 1).
                AddIngredient<BloodOrb>(20).
                AddTile(TileID.AlchemyTable).
                Register();
        }
    }
}
