using CalamityInheritance.System.Configs;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.Utilities;
using CalamityMod.Items.SummonItems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Common.EventChange
{
    public class YharonEggChange : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation) => item.type == ItemType<YharonEgg>();
        public override bool CanUseItem(Item item, Player player)
        {
            if (!CIServerConfig.Instance.SolarEclipseChange)
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
