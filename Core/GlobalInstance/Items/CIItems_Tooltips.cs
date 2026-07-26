using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Items
{
    public partial class CIGlobalItems : GlobalItem
    {
        public bool donerItem = false;
        public bool devItem = false;
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.CI().donerItem)
            {
                tooltips.CreateTooltip("GlobalDataBase.ItemOwner.DonerItem", Color.HotPink);
            }
            if (item.CI().devItem)
            {
                tooltips.CreateTooltip("GlobalDataBase.ItemOwner.DevelopItem", Color.Red);
            }
        }
    }
}
