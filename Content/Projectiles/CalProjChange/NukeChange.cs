using CalamityMod.Projectiles.Melee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Projectiles.Ranged;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.CalProjChange
{
    public class NukeChange : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.type == ModContent.ProjectileType<Nuke>();
        }

        public override void AI(Projectile projectile)
        {
            Player player = Main.LocalPlayer;
            if (player.name == "Shizuku" || player.name == "shizuku")
            {
                CalamityUtils.HomeInOnNPC(projectile, !projectile.tileCollide, 1500f, 12f, 20f);
            }
        }
    }
}
