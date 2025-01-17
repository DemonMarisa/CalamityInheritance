using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class ElementBall : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 150;
        }

        public override void AI()
        {
            int rainbowDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowTorch, Projectile.direction * 2, 0f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
            Main.dust[rainbowDust].noGravity = true;
            Main.dust[rainbowDust].velocity *= 0f;

            CalamityUtils.HomeInOnNPC(Projectile, true, 1600f, 12f, 20f);
        }
    }
}