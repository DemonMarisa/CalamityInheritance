using CalamityInheritance.Core;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Common.EventChange
{
    public class SolarEclipseChange : GlobalItem
    {
        public override void OnSpawn(Item item, IEntitySource source)
        {
            if (item.type == ModContent.ItemType<DarksunFragment>() && CIServerConfig.Instance.SolarEclipseChange)
            {
                item.active = false; //  删除物品
            }
        }

        public override bool OnPickup(Item item, Player player)
        {
            if (item.type == ModContent.ItemType<DarksunFragment>() && CIServerConfig.Instance.SolarEclipseChange)
            {
                item.active = false; // 删除物品
                return false;
            }
            return base.OnPickup(item, player);
        }
    }
}
