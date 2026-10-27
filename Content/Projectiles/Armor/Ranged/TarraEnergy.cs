using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityMod;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Armor.Ranged
{
    public class TarraEnergy : CIArmorProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void SetStaticDefaults() => ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.extraUpdates = 1;
        }
        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < 170 && target.CanBeChasedBy(Projectile);
        public override void AI()
        {
            for (int i = 0; i < 2; i++)
            {
                float x2 = Projectile.position.X - Projectile.velocity.X / 10f * (float)i;
                float y2 = Projectile.position.Y - Projectile.velocity.Y / 10f * (float)i;
                Vector2 dspeed = Projectile.velocity * Main.rand.NextFloat(0.7f, 0.4f);
                int greenDust = Dust.NewDust(new Vector2(x2, y2), 1, 1, DustID.TerraBlade, 0f, 0f, 0, default, 1f);
                Main.dust[greenDust].alpha = Projectile.alpha;
                Main.dust[greenDust].position.X = x2;
                Main.dust[greenDust].position.Y = y2;
                Main.dust[greenDust].velocity = dspeed;
                Main.dust[greenDust].noGravity = true;
                Main.dust[greenDust].noLight = true;
            }

            if (Projectile.timeLeft < 170)
                Projectile.HomeInNPC(1200f, 18f, 20f);
        }
    }
}
