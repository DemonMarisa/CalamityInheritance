using CalamityMod.World;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Buffs.Potions;

namespace CalamityInheritance.Content.Items.Potions
{
    public class PurifiedJam : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Potions";
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 18;
            Item.useTurn = true;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Orange;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.UseSound = CISoundID.SoundPotions;
            Item.consumable = true;
            Item.value = CIShopValue.RarityPriceOrange;
        }

        public override bool CanUseItem(Player player)
        {
            return player.FindBuffIndex(BuffID.PotionSickness) == -1;
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<Invincible>(), (CalamityWorld.death) ? 300 : 600);
            player.AddBuff(BuffID.PotionSickness, player.pStone ? 1500 : 1800);
            return true;
        }
    }
}
