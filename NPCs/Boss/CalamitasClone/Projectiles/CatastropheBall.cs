using CalamityMod.Dusts;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
namespace CalamityInheritance.NPCs.Boss.CalamitasClone.Projectiles
{
    public class CatastropheBall : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Boss.Projectiles";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brimstone Fireball");
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Projectile.rotation += 0.12f * Projectile.direction;

            Lighting.AddLight(Projectile.Center, 0.25f, 0f, 0f);

            for (int num468 = 0; num468 < 2; num468++)
            {
                Vector2 dspeed = -Projectile.velocity * 0.7f;
                int num469 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, (int)CalamityDusts.Brimstone, 0f, 0f, 150, default, 1.1f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity = dspeed;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
        }
    }
}
