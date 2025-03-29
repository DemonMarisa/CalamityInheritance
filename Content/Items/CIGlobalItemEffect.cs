using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items
{
    public partial class CalamityInheritanceGlobalItem : GlobalItem
    {
        public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
        {
            //if(item.ModItem.Mod == ModContent.GetInstance<CalamityInheritance>())
                CIFunction.BetterSwing(player);
        }
    }
}
