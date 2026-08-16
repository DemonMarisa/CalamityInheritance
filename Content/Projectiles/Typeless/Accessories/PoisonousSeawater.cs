using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Assets.TextureRegister;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Typeless.Accessories
{
    public class PoisonousSeawater : CITypelessProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 6;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.idStaticNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0f / 255f, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.4f / 255f);
            int randomDust = Main.rand.Next(4);
            if (randomDust == 3)
            {
                randomDust = 89;
            }
            else
            {
                randomDust = 33;
            }
            for (int i = 0; i < 2; i++)
            {
                int seaDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, randomDust, 0f, 0f, 100, default, 1f);
                if (randomDust == 89)
                {
                    Main.dust[seaDust].scale *= 0.35f;
                }
                Main.dust[seaDust].velocity *= 0f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffID.Venom, 60);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffID.Venom, 60);
    }
}
