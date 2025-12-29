using CalamityInheritance.Content.Items.Placeables.Relic;
using CalamityInheritance.Tiles.BaseTiles;
using CalamityMod.Tiles.BaseTiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace CalamityInheritance.Tiles.Relic
{
    public class ScalRelicTiles : CIBaseRelic
    {
        public override string RelicTextureName => "CalamityInheritance/Tiles/Relic/ScalRelicTiles";

        public override int AssociatedItem => ItemType<ScalRelic>();
    }
}
