using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Placeables
{
    public abstract class CIPlaceable: ModItem
    {
        public static string Local => "Content.Items.Placeables";
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.PlacableObjects;
        }
    }
}