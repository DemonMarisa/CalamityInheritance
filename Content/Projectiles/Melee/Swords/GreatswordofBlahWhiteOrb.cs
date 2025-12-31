using LAP.Assets.TextureRegister;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Swords
{
    public class GreatswordofBlahWhiteOrb : GeneralDamageProj
    {
        public override ProjDamageType UseDamageClass => ProjDamageType.Melee;
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void ExSD()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 60;
        }

        public override void AI()
        {
            Lighting.AddLight((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, (float)Main.DiscoR / 200f, (float)Main.DiscoG / 200f, (float)Main.DiscoB / 200f);
            for (int i = 0; i < 2; i++)
            {
                int rainbow = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowTorch, 0f, 0f, 100, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
                Main.dust[rainbow].noGravity = true;
                Main.dust[rainbow].velocity *= 0.5f;
                Main.dust[rainbow].velocity += Projectile.velocity * 0.1f;
            }
        }
    }
}