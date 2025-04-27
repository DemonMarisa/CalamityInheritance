using CalamityInheritance.Content.BaseClass;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Heal
{
    public class GodSlayerHealOrbLegacy : BaseHealProj, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";
        public override void ExSD()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.extraUpdates = 3;
        }

        public override void ExAI()
        {
            int dusty = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 2f);
            Dust dust = Main.dust[dusty];
            dust.noGravity = true;
            dust.position.X -= Projectile.velocity.X * 0.2f;
            dust.position.Y += Projectile.velocity.Y * 0.2f;
        }
    }
}
