using LAP.Assets.TextureRegister;
using LAP.Core.BaseClass.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Heals
{
    public class PhoenixBladeHeal : BaseHealProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 480;
            Projectile.extraUpdates = 3;
        }
        public override void ExAI()
        {
            float dVelX = Projectile.velocity.X * 0.2f;
            float dVelY = -Projectile.velocity.Y * 0.2f;
            int dType = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.CopperCoin, 0f, 0f, 100, default, 1f);
            Main.dust[dType].noGravity = true;
            Main.dust[dType].velocity *= 0f;
            Dust dClone = Main.dust[dType];
            dClone.position.X -= dVelX; //???
            Dust dAlter = Main.dust[dType];
            dAlter.position.Y -= dVelY;
        }
        public override void ExKill()
        {
            SoundEngine.PlaySound(SoundID.Item74 with { Volume = 0.5f });
        }
    }
}