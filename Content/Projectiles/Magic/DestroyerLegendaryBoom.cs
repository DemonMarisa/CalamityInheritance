using System;
using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ModLoader;
using LAP.Assets.TextureRegister;
namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class DestroyerLegendaryBoom: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public override void SetDefaults()
        {
            Projectile.width = 500;
            Projectile.height = 500;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner]; 
            var usPlayer = player.CIMod();
            if (usPlayer.DestroyerTier1)
                modifiers.SetCrit();
            if (usPlayer.DestroyerTier2)
            {
                //取玩家当前法术暴击加成。
                float getCrits = player.GetTotalCritChance<MagicDamageClass>() + 4f;
                if (getCrits > 0f)
                {
                    //将暴击加成小数点前置两位。
                    getCrits /= 100f;
                    //与1结算。
                    float realReduceDefense = 1f - getCrits;
                    if (realReduceDefense < 0f)
                        realReduceDefense = 0f;
                    //直接补到伤害上
                    modifiers.DefenseEffectiveness *= realReduceDefense;
                }
            }
        }
        public override void AI()
        {
            float lights = Main.rand.Next(90, 111) * 0.01f;
            lights *= Main.essScale;
            Lighting.AddLight(Projectile.Center, 5f * lights, 1f * lights, 4f * lights);
            float pTimer = 25f;
            if (Projectile.ai[0] > 180f)
            {
                pTimer -= (Projectile.ai[0] - 180f) / 2f;
            }
            if (pTimer < 0f)
            {
                pTimer = 0f;
                Projectile.Kill();
            }
            pTimer *= 0.7f;
            Projectile.ai[0] += 4f;
            int tCounter = 0;
            while (tCounter < pTimer)
            {
                float r = Main.rand.Next(-40, 41);
                float r2 = Main.rand.Next(-40, 41);
                float r3 = Main.rand.Next(12, 36);
                float rAdj = (float)Math.Sqrt((double)(r * r + r2 * r2));
                rAdj = r3 / rAdj;
                r *= rAdj;
                r2 *= rAdj;
                int rDust = 246;
                if (Main.rand.NextBool())
                    rDust = 73;
                if (Main.rand.NextBool())
                    rDust = 187;

                int boom = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, rDust, 0f, 0f, 100, default, 2f);
                Main.dust[boom].noGravity = true;
                Main.dust[boom].position.X = Projectile.Center.X;
                Main.dust[boom].position.Y = Projectile.Center.Y;
                Dust extraBoom = Main.dust[boom];
                extraBoom.position.X += Main.rand.Next(-10, 11);
                Dust extraBoom2 = Main.dust[boom];
                extraBoom2.position.Y += Main.rand.Next(-10, 11);
                Main.dust[boom].velocity.X = r;
                Main.dust[boom].velocity.Y = r2;
                tCounter++;
            }
        }
    }
}
