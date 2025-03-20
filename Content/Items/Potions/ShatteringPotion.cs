using CalamityMod.Items.Materials;
using CalamityMod;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Potions;
using CalamityInheritance.Buffs.Potions;
using CalamityInheritance.Content.Items.Potions.CIPotions;

namespace CalamityInheritance.Content.Items.Potions
{
    public class ShatteringPotion : CIPotion, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Potions";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 10;
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.useTurn = true;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Yellow;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = CISoundID.SoundPotions;
            Item.consumable = true;
            Item.buffType = ModContent.BuffType<ArmorShattering>();
            Item.buffTime = CalamityUtils.SecondsToFrames(480f);
            Item.value = CIShopValue.RarityPriceYellow;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FlaskOfCrumbling>(2).
                AddIngredient(ItemID.BeetleHusk).
                AddTile(TileID.AlchemyTable).
                Register();

            CreateRecipe().
                AddIngredient(ItemID.BottledWater).
                AddIngredient<BloodOrb>(30).
                AddIngredient(ItemID.BeetleHusk).
                AddTile(TileID.AlchemyTable).
                Register();
        }
    }
}
