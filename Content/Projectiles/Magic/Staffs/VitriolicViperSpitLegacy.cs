using CalamityInheritance.Texture;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.Staffs
{
    public class VitriolicViperSpitLegacy : GeneralDamageProj
    {
        public override ProjDamageType UseDamageClass => ProjDamageType.Magic;
        public override void ExSD()
        {
            Projectile.width = 16;
            Projectile.height = 26;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 14; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, 10, 10, DustID.Shadowflame);
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<SulphuricPoisoning>(), 60);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffType<SulphuricPoisoning>(), 60);
        }
    }
}
