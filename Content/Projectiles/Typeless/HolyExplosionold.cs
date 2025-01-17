using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Typeless
{
    public class HolyExplosionold : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            Projectile.position.X = Projectile.position.X + Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y + Projectile.height / 2;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.position.X = Projectile.position.X - Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;

            for (int i = 0; i < 20; i++)
            {
                int holyDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0f, 0f, 100, default, 2f);
                Main.dust[holyDust].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[holyDust].scale = 0.5f;
                    Main.dust[holyDust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 40; j++)
            {
                int holyDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0f, 0f, 100, default, 3f);
                Main.dust[holyDust2].noGravity = true;
                Main.dust[holyDust2].velocity *= 5f;
                holyDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0f, 0f, 100, default, 2f);
                Main.dust[holyDust2].velocity *= 2f;
            }
        }
    }
}
