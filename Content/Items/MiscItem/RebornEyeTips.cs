using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.MiscItem
{
    public class RebornEyeTip: CIMisc, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.MiscItem";
        public override string Texture => "CalamityInheritance/Content/Items/MiscItem/ScalShopMessage";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ModContent.ItemType<ScalShopMessage>());
        }
    }
}