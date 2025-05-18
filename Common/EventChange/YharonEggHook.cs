using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Events;
using CalamityMod.Items.SummonItems;
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
    public class YharonEggHook
    {
        public static void Load(Mod mod)
        {
            MethodInfo originalMethod2 = typeof(YharonEgg).GetMethod(nameof(YharonEgg.UseItem));
            MonoModHooks.Add(originalMethod2, UseItem_Hook);
        }

        public static bool? UseItem_Hook(Player player)
        {
            if(!CIDownedBossSystem.DownedLegacyYharonP1)
            {
                CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Boss.Text.YharonPreEclipseSummon", Color.Orange);
                return false;
            }
            else
                CIFunction.SpawnBossUsingItem(player, ModContent.NPCType<Yharon>(), Yharon.FireSound);
            return true;
        }
    }
}
