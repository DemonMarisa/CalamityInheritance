using CalamityInheritance.System.Configs;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Events;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.NPCs.Yharon;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.EventChange
{
    public class YharonEggChange : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation) => item.type == ModContent.ItemType<YharonEgg>();
        public override bool CanUseItem(Item item, Player player)
        {
            if(!CIServerConfig.Instance.SolarEclipseChange)
                return true;

            if (!CIDownedBossSystem.DownedBuffedSolarEclipse)
            {
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.YharonPreEclipseSummon", Color.Orange);
                return false;
            }
            else
                return true;
        }
    }
}
