using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod.Projectiles.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles
{
    public class CalProjOverride : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Projectile proj)
        {
            if(CIServerConfig.Instance.VanillaUnnerf)
            {
                //回调原版所有悠悠球的无敌帧
                //注意其他方面都不会回调，只回调了无敌帧，但也足够了
                //砍无敌帧太傻逼了，纯纯砍手感的
                switch (proj.type)
                {
                    case ProjectileID.JungleYoyo:
                    case ProjectileID.Amarok:
                    case ProjectileID.CrimsonYoyo:
                    case ProjectileID.Chik:
                    case ProjectileID.Code1:
                    case ProjectileID.Code2:
                    case ProjectileID.FormatC:
                    case ProjectileID.Gradient:
                    case ProjectileID.HiveFive:
                    case ProjectileID.CorruptYoyo:
                    case ProjectileID.RedsYoyo:
                    case ProjectileID.ValkyrieYoyo:
                    case ProjectileID.Rally:
                    case ProjectileID.Valor:
                    case ProjectileID.Yelets:
                    case ProjectileID.WoodYoyo:
                        proj.localNPCHitCooldown = 10 * (1 + proj.extraUpdates);
                        break;
                }
            }
        }
    }
}
