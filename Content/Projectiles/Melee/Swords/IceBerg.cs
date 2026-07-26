using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Swords
{
    public class IceBerg : CIMeleeProj
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] < 30f)
            {
                if (Projectile.ai[0] % 15f == 0)
                {
                    SoundEngine.PlaySound(SoundID.Item30, Projectile.Center);
                }
                Projectile.velocity *= 0.96f;
            }
            else if (Projectile.ai[0] >= 30f)
            {
                if (Projectile.ai[1] == 0)
                {
                    SignalDust();
                    Projectile.ai[1] = 1f;
                }
                NPC npc = LAPUtilities.FindClosestTarget(Projectile.Center, 1500);
                if (npc is not null)
                {
                    Projectile.HomeInNPC(1500f, 12f, 35f);
                }
                else
                {
                    Projectile.velocity *= 0.96f;
                }
            }
            Projectile.rotation += 0.2f;
            TrailDustNormal();
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            for (int i = 0; i < 5; i++)
                Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, DustID.IceRod, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {

        }

        public void TrailDustHoming()
        {
            for (int i = 0; i < 2; i++)
            {
                Dust newD = Dust.NewDustPerfect(Projectile.Center, DustID.Water_Snow);
                newD.velocity = Projectile.velocity / 2f;
                newD.noGravity = false;
                newD.scale *= 1.2f;
            }
        }
        public void TrailDustNormal()
        {
            int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.Water_Snow);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0f;
            Main.dust[dust].scale *= 1.2f;
            Main.dust[dust].alpha = 100;
        }
        public void SignalDust()
        {
            CIUtils.DustCircle(Projectile.Center, 32f, 1.2f, DustID.SnowflakeIce, true, 10f);
        }
    }
}