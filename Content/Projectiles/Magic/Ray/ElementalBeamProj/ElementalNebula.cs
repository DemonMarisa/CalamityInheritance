using CalamityMod.Buffs.DamageOverTime;
using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using CalamityMod;
using Microsoft.Build.Evaluation;
using CalamityInheritance.Utilities;
using Terraria.DataStructures;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray.ElementalBeamProj
{
    public class ElementalNebula : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 200;
            Projectile.extraUpdates = 1;
        }
        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < 148;

        public override void AI()
        {
            int dType = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 2f);
            Main.dust[dType].noGravity = true;
            Main.dust[dType].velocity *= 0f;
            Projectile.ai[0] += 1f;
            Projectile.velocity *= 0.91f;
            //星云烈焰会先飞行一段时间，并逐渐地减速
            if(Projectile.ai[0] > 30f)
            {
                Projectile.velocity *= 0.5f;
                if(Projectile.ai[0] > 40f)
                {
                    Projectile.ai[0] = 40f;
                    Projectile.localAI[0] += 1f;
                    CIFunction.HomeInOnNPC(Projectile, true, 800f, 10f + Projectile.localAI[0], 10f, 45f);
                }
            }
            Projectile.rotation += 0.12f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<ElementalMix>(), 180);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
                 //技术力不够先注释掉了，等哪天有技术力了就改
                // for (int i = 0; i< Main.maxNPCs; i++)
                // {
                //     if (Main.npc[i].active && Main.npc[i].life > 5)
                //     {
                //         float getEnemyX = Main.npc[i].position.X + Main.npc[i].width/2;
                //         float getEnemyY = Main.npc[i].position.Y + Main.npc[i].height/2;
                //         //获得距离
                //         Vector2 getEnemyPos = new (getEnemyX, getEnemyY);
                //         //获得坐标位置
                //         Vector2 getProjEnemyDist = Main.npc[i].Center - Projectile.Center;
                //         //开始旋转
                //         Projectile.Center = Main.npc[i].Center + new Vector2(9f, 0).RotatedBy(Projectile.ai[1] + Projectile.ai[0] * MathHelper.PiOver2);
                //         Projectile.ai[1] += 0.1f;
                //         Projectile.velocity.X = (getProjEnemyDist.X > 0f) ? -0.001f : 0f;
                //         //需注意的是，这一过程会一直让lifeTimie取这个值
                //         Projectile.timeLeft = 101;
                //     }
                // }
