using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons
{
    public abstract class CIMagic: ModItem
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Magic";
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.MagicWeapon;
        }
    }
}