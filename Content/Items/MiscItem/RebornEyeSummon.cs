using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.MiscItem
{
    public class RebornEyeSummon: CIMisc, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.MiscItem";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 28;
            Item.noMelee = true;
            Item.useTime = 20;
            Item.useAnimation = 20;
        }
    }
}