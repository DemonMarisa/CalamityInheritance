using LAP.Assets.TextureRegister;
using LAP.Core.BaseClass.Projectiles;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Heals
{
    public class GodSlayerHealOrb : BaseHealProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override int HealAmt => Main.rand.Next(10, 18);
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
