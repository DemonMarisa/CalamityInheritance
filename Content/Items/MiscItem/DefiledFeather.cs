using CalamityInheritance.Rarity;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.MiscItem
{
    public class DefiledFeather : CIMisc, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.MiscItem";
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(7, 8));
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 50;
            Item.rare = ModContent.RarityType<PureRed>();
        }
        public override bool CanUseItem(Player player) => false;
    }
}