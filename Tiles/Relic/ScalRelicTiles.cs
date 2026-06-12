using CalamityInheritance.Content.Items.Placeables.Relic;
using CalamityInheritance.Tiles.BaseTiles;

namespace CalamityInheritance.Tiles.Relic
{
    public class ScalRelicTiles : CIBaseRelic
    {
        public override string RelicTextureName => "CalamityInheritance/Tiles/Relic/ScalRelicTiles";

        public override int AssociatedItem => ItemType<ScalRelic>();
    }
}
