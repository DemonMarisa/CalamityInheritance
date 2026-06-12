using CalamityInheritance.Content.Items.Placeables.Relic;
using CalamityInheritance.Tiles.BaseTiles;

namespace CalamityInheritance.Tiles.Relic
{
    public class CalCloneRelicTiles : CIBaseRelic
    {
        public override string RelicTextureName => "CalamityInheritance/Tiles/Relic/CalCloneRelicTiles";

        public override int AssociatedItem => ItemType<CalCloneRelic>();
    }
}
