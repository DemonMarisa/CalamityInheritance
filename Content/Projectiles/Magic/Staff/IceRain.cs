using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.Staff
{

    public class IceRain : CIMagicProj
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public ref float AttackType => ref Projectile.ai[0];
        public ref float AttackTimer => ref Projectile.ai[1];
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.scale *= 1f;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 240;
        }

        public override void AI()
        {
            DoGeneric();
            Projectile.rotation += 0.5f;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 30f) 
            {
                if (Projectile.ai[0] < 40f)
                    Projectile.velocity *= 0.5f;
                if (Projectile.ai[0] == 42f)
                    SignalSend();
                if (Projectile.ai[0] > 45f)
                {
                    Projectile.extraUpdates = 1;
                    Projectile.rotation += Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                    LAPUtilities.HomeInNPC(Projectile, 1800f, 12f, 10f);
                }
            }
        }
        private void DoGeneric() => CreateDust();
        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item30, Projectile.Center);
            for (int i = 0; i < 5; i++)
                Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.IceRod, Projectile.velocity.X, Projectile.velocity.Y);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn2, 360);
            target.AddBuff(BuffID.Frostburn, 360);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.Frostburn2, 360);
            target.AddBuff(BuffID.Frostburn, 360);
        }
        public void CreateDust()
        {
            int newDust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.IceRod, 0f, 0f, 100, default, 0.7f);
            Main.dust[newDust].noGravity = true;
            Main.dust[newDust].velocity *= 0f;
        }
        public void SignalSend()
        {
            SoundEngine.PlaySound(SoundID.Item30, Projectile.Center);
            CIUtils.DustCircle(Projectile.Center, 16f, 1.2f, DustID.SnowflakeIce, true, 8f);
        }
    }
}