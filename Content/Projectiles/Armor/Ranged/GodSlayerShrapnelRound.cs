using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Core.Utils;
using LAP.Assets.TextureRegister;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Armor.Ranged
{
    public class GodSlayerShrapnelRound : CIArmorProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(SoundID.Item73, Projectile.position);
                Projectile.localAI[0] += 1f;
            }
            for (int d = 0; d < 3; d++)
            {
                int cosmilite = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 1.2f);
                Main.dust[cosmilite].noGravity = true;
                Main.dust[cosmilite].velocity *= 0.5f;
                Main.dust[cosmilite].velocity += Projectile.velocity * 0.1f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                int shrapnelAmt = Main.rand.Next(4, 7);
                for (int s = 0; s < shrapnelAmt; s++)
                {
                    Vector2 velocity = CIUtils.RandomVelocity(10f, 7f, 10f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ProjectileType<GodSlayerShrapnel>(), (int)(Projectile.damage * 0.3), 0f, Projectile.owner, 0f, 0f);
                }
            }
        }
    }
}
