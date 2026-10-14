using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Assets.TextureRegister;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Armor.Typeless
{
    public class GhostlyBolt : CIArmorProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 2;
            Projectile.timeLeft = 180 * Projectile.MaxUpdates;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 6f)
            {
                SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
                for (int i = 0; i < 40; i++)
                {
                    int cursedDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GiantCursedSkullBolt, 0f, 0f, 100, default, 1f);
                    Main.dust[cursedDust].velocity *= 3f;
                    Main.dust[cursedDust].velocity += Projectile.velocity * 0.75f;
                    Main.dust[cursedDust].scale *= 1.2f;
                    Main.dust[cursedDust].noGravity = true;
                }
            }
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 6f)
            {
                for (int j = 0; j < 3; j++)
                {
                    int cursedDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GiantCursedSkullBolt, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1f);
                    Main.dust[cursedDust2].velocity *= 0.6f;
                    Main.dust[cursedDust2].scale *= 1.4f;
                    Main.dust[cursedDust2].noGravity = true;
                }
            }
        }
    }
}
