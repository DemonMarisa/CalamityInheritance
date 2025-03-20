using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Placeables
{
    public abstract class CIPlaceable: ModItem
    {
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.PlacableObjects;
        }
    }
}