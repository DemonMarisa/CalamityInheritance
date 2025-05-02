using CalamityMod.Items.Materials;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.System.Configs;
using CalamityInheritance.System.DownedBoss;

namespace CalamityInheritance.Common.EventChange
{
    public class SolarEclipseChange : GlobalItem
    {
        public override void OnSpawn(Item item, IEntitySource source)
        {
            if (item.type == ModContent.ItemType<DarksunFragment>() && CIServerConfig.Instance.SolarEclipseChange && !CIDownedBossSystem.DownedBuffedSolarEclipse)
            {
                item.active = false; //  删除物品
            }
        }

        public override bool OnPickup(Item item, Player player)
        {
            if (item.type == ModContent.ItemType<DarksunFragment>() && CIServerConfig.Instance.SolarEclipseChange && !CIDownedBossSystem.DownedBuffedSolarEclipse)
            {
                item.active = false; // 删除物品
                return false;
            }
            return base.OnPickup(item, player);
        }
    }
}
               