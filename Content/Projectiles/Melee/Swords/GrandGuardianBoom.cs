using LAP.Assets.TextureRegister;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Swords
{
    public class GrandGuardianBoom : GeneralDamageProj
    {
        public override ProjDamageType UseDamageClass => ProjDamageType.Melee;
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void ExSD()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 20;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
        }

        public override void AI()
        {
            float projTimer = 25f;
            if (Projectile.ai[0] > 180f)
                projTimer -= (Projectile.ai[0] - 180f) / 2f;

            if (projTimer <= 0f)
            {
                projTimer = 0f;
                Projectile.Kill();
            }

            projTimer *= 0.7f;
            Projectile.ai[0] += 4f;
            int timerCounter = 0;
            while ((float)timerCounter < projTimer)
            {
                float rando1 = (float)Main.rand.Next(-30, 31);
                float rando2 = (float)Main.rand.Next(-30, 31);
                float rando3 = (float)Main.rand.Next(9, 27);
                float randoAdjuster = (float)Math.Sqrt((double)(rando1 * rando1 + rando2 * rando2));
                randoAdjuster = rando3 / randoAdjuster;
                rando1 *= randoAdjuster;
                rando2 *= randoAdjuster;
                int rainbow = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowTorch, 0f, 0f, 100, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
                Main.dust[rainbow].noGravity = true;
                Main.dust[rainbow].position.X = Projectile.Center.X;
                Main.dust[rainbow].position.Y = Projectile.Center.Y;
                Main.dust[rainbow].position.X += (float)Main.rand.Next(-10, 11);
                Main.dust[rainbow].position.Y += (float)Main.rand.Next(-10, 11);
                Main.dust[rainbow].velocity.X = rando1;
                Main.dust[rainbow].velocity.Y = rando2;
                timerCounter++;
            }
        }
    }
}