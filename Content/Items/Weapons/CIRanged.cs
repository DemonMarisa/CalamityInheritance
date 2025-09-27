using CalamityMod;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons
{
    public abstract class CIRanged: ModItem
    {
        public new string LocalizationCategory => $"{Generic.BaseWeaponCategory}.Ranged";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.Calamity().canFirePointBlankShots = true;
            base.SetDefaults();
        }
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.RangedWeapon;
        }
    }
}