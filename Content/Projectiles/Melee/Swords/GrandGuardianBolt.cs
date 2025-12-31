using CalamityMod;
using LAP.Assets.TextureRegister;
using LAP.Core.BaseClass;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Swords
{
    public class GrandGuardianBolt : GeneralDamageProj
    {
        public override ProjDamageType UseDamageClass => ProjDamageType.Melee;
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        private const int TimeLeft = 180;

        private const int HomingTime = TimeLeft - 30;

        public override void ExSD()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.penetrate = 1;
            Projectile.timeLeft = TimeLeft;
        }

        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < HomingTime && target.CanBeChasedBy(Projectile);

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(SoundID.Item9, Projectile.Center);
                Projectile.localAI[0] += 1f;
            }

            int rainbow = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowTorch, 0f, 0f, 100, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 2f);
            Main.dust[rainbow].noGravity = true;
            Main.dust[rainbow].velocity *= 0f;

            if (Projectile.timeLeft < HomingTime)
                CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, 600f, 12f, 20f);
            else
                Projectile.velocity *= 0.95f;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item60, Projectile.Center);
            for (int k = 0; k < 5; k++)
            {
                int rain = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.RainbowTorch, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB));
                Main.dust[rain].noGravity = true;
            }
        }
    }
}