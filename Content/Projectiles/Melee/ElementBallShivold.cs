using CalamityMod.Projectiles.Melee;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class ElementBallShivold : ModProjectile, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public new string LocalizationCategory => "Projectiles.Melee";
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;
            Projectile.aiStyle = ProjAIStyleID.Beam;
        }

        public override void AI()
        {
            int rainbow = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowTorch, Projectile.direction * 2, 0f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1.3f);
            Main.dust[rainbow].noGravity = true;
            Main.dust[rainbow].velocity = Vector2.Zero;

            CalamityUtils.HomeInOnNPC(Projectile, true, 300f, 12f, 20f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft > 50)
                return false;

            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 0);
            return false;
        }

        [Obsolete]
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 2; k++)
            {
                int rainbow = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.RainbowTorch, Projectile.direction * 2, 0f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
                Main.dust[rainbow].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            OnHitEffects(target.Center);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            OnHitEffects(target.Center);
        }

        private void OnHitEffects(Vector2 targetPos)
        {
            var source = Projectile.GetSource_FromThis();
            for (int x = 0; x < 4; x++)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    CalamityUtils.ProjectileBarrage(source, Projectile.Center, targetPos, x > 2, 800f, 800f, 0f, 800f, 1f, ModContent.ProjectileType<SHIVold>(), Projectile.damage, Projectile.knockBack, Projectile.owner, false, 50f);
                }
            }
        }
    }
}
