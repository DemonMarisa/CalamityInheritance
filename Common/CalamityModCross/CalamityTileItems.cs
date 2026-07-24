using CalamityInheritance.Core.Utils;
using CalamityMod.Items.Placeables.FurnitureNavystone.FurnitureAncientNavystone;
using CalamityMod.Items.Placeables.FurnitureStatigel;
using CalamityMod.Items.Placeables.FurnitureWulfrum;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.CalamityModCross
{
    public class CalamityTileItems : ModSystem
    {
        public static int WulfrumLabstationItem;
        public static int EutrophicShelf;
        public static int StaticRefiner;
        public override void OnModLoad()
        {
            if (CIUtils.HasCalamity())
            {
                GetCalamityTileItemID();
            }
        }

        [JITWhenModsEnabled("CalamityMod")]
        public static void GetCalamityTileItemID()
        {
            WulfrumLabstationItem = ItemType<WulfrumLabstationItem>();
            EutrophicShelf = ItemType<EutrophicShelf>();
            StaticRefiner = ItemType<StaticRefiner>();
        }
    }
}
