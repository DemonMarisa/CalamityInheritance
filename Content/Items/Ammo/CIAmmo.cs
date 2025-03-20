using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Ammo
{
    public abstract class CIAmmo : ModItem
    {
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.Ammo;
        }
    }
}