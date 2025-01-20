using CalamityMod.Balancing;
using CalamityMod.Projectiles;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class CIBlood : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
        }

        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < 150 && target.CanBeChasedBy(Projectile);

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
            {
                for (int i = 0; i < 2; i++)
                {
                    int blood = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 100, default, 1f);
                    Main.dust[blood].noGravity = true;
                    Main.dust[blood].velocity *= 0f;
                }
            }

            CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, 600f, 6f, 20f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int heal = (int)Math.Round(hit.Damage * 0.075);
            if (heal > 100)
                heal = 100;

            if (Main.player[Main.myPlayer].lifeSteal <= 0f || heal <= 0 || target.lifeMax <= 5)
                return;

            CalamityGlobalProjectile.SpawnLifeStealProjectile(Projectile, Main.player[Projectile.owner], heal, ProjectileID.VampireHeal, 3000f);
        }
    }
}
