using System;
using CalamityMod.Buffs.DamageOverTime;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class ProfanedNukeDust: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";
        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 46;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 15;
        }
        //我挺好奇灾厄怎么能造这样的史.
        //确实史
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 0f / 255f);
            //?
            float getTimer = 25f;
            if (Projectile.ai[0] > 60f)
                getTimer -= (Projectile.ai[0] - 60f) / 2f;
            if (getTimer <= 0f)
                Projectile.Kill();
            //正中敌人生成的粒子的生命会更短
            Projectile.ai[0] += 2f + Projectile.ai[1];
            int getTimerCount = 0;
            while (getTimerCount < getTimer)
            {
                //基本就是生成粒子的AI了，照抄了
                 float r1 = Main.rand.Next(-8, 9);
                float r2 = Main.rand.Next(-8, 9);
                float r3 = Main.rand.Next(2, 7);
                float rAdj = (float)Math.Sqrt((double)(r1 * r1 + r2 * r2));
                rAdj = r3 / rAdj;
                r1 *= rAdj;
                r2 *= rAdj;
                int dType = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CopperCoin, 0f, 0f, 100, default, 1.5f);
                Dust d = Main.dust[dType];
                d.noGravity = true;
                d.position.X = Projectile.Center.X;
                d.position.Y = Projectile.Center.Y;
                d.position.X += Main.rand.Next(-10, 11);
                d.position.Y += Main.rand.Next(-10, 11);
                d.velocity.X = r1;
                d.velocity.Y = r2;
                getTimerCount++;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
    }
}