using System.Reflection;
using CalamityInheritance.System.Configs;
using CalamityMod;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.CIOverride
{
    public class SoaringInsinaReturn : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type is ItemID.EmpressFlightBooster;
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (CIServerConfig.Instance.VanillaUnnerf)
                return;

            if (item.type != ItemID.EmpressFlightBooster)
                return;
            player.empressBrooch = false;
            //做掉经过灾厄削弱后的飞行奖励
            player.runAcceleration /= 1.1f;
            player.jumpSpeedBoost -= 0.5f;
            
            //而后将其重置为原版的数值
            player.runAcceleration *= 1.75f;
            player.jumpSpeedBoost += 1.8f;
            player.Calamity().infiniteFlight = true;

        }
    }
}