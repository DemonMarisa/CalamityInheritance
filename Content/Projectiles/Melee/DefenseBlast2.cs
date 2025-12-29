using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using LAP.Assets.TextureRegister;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class DefenseBlast2: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 5;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0f / 255f);
            float projTimer = 25f;
            if (Projectile.ai[0] > 180f)
            {
                projTimer -= (Projectile.ai[0] - 180f) / 2f;
            }
            if (projTimer <= 0f)
            {
                projTimer = 0f;
                Projectile.Kill();
            }
            projTimer *= 0.7f;
            Projectile.ai[0] += 4f;
            int timerCounter = 0;
            while (timerCounter < projTimer)
            {
                float rando1 = Main.rand.Next(-5, 6);
                float rando2 = Main.rand.Next(-5, 6);
                float rando3 = Main.rand.Next(2, 5);
                float randoAdjuster = (float)Math.Sqrt((double)(rando1 * rando1 + rando2 * rando2));
                randoAdjuster = rando3 / randoAdjuster;
                rando1 *= randoAdjuster;
                rando2 *= randoAdjuster;
                int goldDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, 0f, 0f, 100, new Color(255, Main.DiscoG, 53), 1.5f);
                Main.dust[goldDust].noGravity = true;
                Main.dust[goldDust].position.X = Projectile.Center.X;
                Main.dust[goldDust].position.Y = Projectile.Center.Y;
                Dust expr_149DF_cp_0 = Main.dust[goldDust];
                expr_149DF_cp_0.position.X += Main.rand.Next(-10, 11);
                Dust expr_14A09_cp_0 = Main.dust[goldDust];
                expr_14A09_cp_0.position.Y += Main.rand.Next(-10, 11);
                Main.dust[goldDust].velocity.X = rando1;
                Main.dust[goldDust].velocity.Y = rando2;
                timerCounter++;
            }
        }
    }
}
