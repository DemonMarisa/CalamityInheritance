using LAP.Assets.TextureRegister;
using LAP.Core.BaseClass.Projectiles;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Heals
{
    public class ManaPolarizerHeal : BaseHealProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void ExSD()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 480;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 3;
        }
        public override float FlySpeed => 3f;
        public override void ExAI()
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SpectreStaff, 0f, 0f, 255, Color.White, 2f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0f;
            Main.dust[dust].position.X -= Projectile.velocity.X * 0.2f;
            Main.dust[dust].position.Y += Projectile.velocity.Y * 0.2f;
        }
    }
}
