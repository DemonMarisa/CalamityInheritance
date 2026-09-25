using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Items
{
    public partial class CIGlobalItems : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public int timesUsed;
        public override void GrabRange(Item item, Player player, ref int grabRange)
        {
            if (player.CI().ExShootSpeed != 0)
                grabRange += player.CI().ExGrabRange;
        }
    }
}
