using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Projectiles.Typeless
{
    public class GodSlayerBlaze : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override void SetDefaults()
        {
            Projectile.width = 250;
            Projectile.height = 250;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.5f, 0f, 0.75f);
            float num461 = 25f;
            if (Projectile.ai[0] > 180f)
            {
                num461 -= (Projectile.ai[0] - 180f) / 2f;
            }
            if (num461 <= 0f)
            {
                num461 = 0f;
                Projectile.Kill();
            }
            num461 *= 0.7f;
            Projectile.ai[0] += 4f;
            int num462 = 0;
            float scale = 0.7f;
            int dustType = Main.rand.NextBool(2) ? ModContent.DustType<AstralOrange>() : ModContent.DustType<AstralBlue>();
            if (Projectile.ai[1] == 0f)
            {
                scale = 1.5f;
                dustType = 173;
            }
            while (num462 < num461)
            {
                float num463 = Main.rand.Next(-30, 31);
                float num464 = Main.rand.Next(-30, 31);
                float num465 = Main.rand.Next(9, 27);
                float num466 = (float)Math.Sqrt((double)(num463 * num463 + num464 * num464));
                num466 = num465 / num466;
                num463 *= num466;
                num464 *= num466;
                int num467 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, scale);
                if (dustType != 173)
                    Main.dust[num467].color = new Color(255, 255, 255, 0);
                Main.dust[num467].noGravity = true;
                Main.dust[num467].position.X = Projectile.Center.X;
                Main.dust[num467].position.Y = Projectile.Center.Y;
                Dust expr_149DF_cp_0 = Main.dust[num467];
                expr_149DF_cp_0.position.X += Main.rand.Next(-10, 11);
                Dust expr_14A09_cp_0 = Main.dust[num467];
                expr_14A09_cp_0.position.Y += Main.rand.Next(-10, 11);
                Main.dust[num467].velocity.X = num463;
                Main.dust[num467].velocity.Y = num464;
                num462++;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[1] == 1f)
                target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 300);
            else
                target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 300);
        }
    }
}
