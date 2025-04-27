using CalamityInheritance.Content.BaseClass;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Heal
{
    public class SilvaOrbLegacy : BaseHealProj, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";

        public override void ExSD()
        {
            Projectile.width = Projectile.height = 22;
            Projectile.extraUpdates = 3;
        }

        public override Color? GetAlpha(Color lightColor) => new Color(Main.DiscoR, 203, 103, Projectile.alpha);

        public override void ExKill()
        {
            for (int i = 0; i < 5; i++)
            {
                int silvaHeal = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ChlorophyteWeapon, 0f, 0f, 0, new Color(Main.DiscoR, 203, 103), 1.5f);
                Main.dust[silvaHeal].noGravity = true;
                Main.dust[silvaHeal].velocity *= 0f;
            }
        }
        public override void ExAI()
        {
            int dusty = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 100, default, 2f);
            Dust dust = Main.dust[dusty];
            dust.noGravity = true;
            dust.position.X -= Projectile.velocity.X * 0.2f;
            dust.position.Y += Projectile.velocity.Y * 0.2f;
        }
    }
}
