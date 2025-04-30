using CalamityInheritance.Content.Items.Placeables.Relic;
using CalamityInheritance.Tiles.BaseTiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace CalamityInheritance.Tiles.Relic
{
    public class CalCloneRelicTiles : CIBaseRelic
    {
        public override string RelicTextureName => "CalamityInheritance/Tiles/Relic/CalCloneRelicTiles";

        public override int AssociatedItem => ModContent.ItemType<CalCloneRelic>();
    }
}
