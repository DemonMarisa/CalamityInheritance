using CalamityMod.Buffs.DamageOverTime;
using LAP.Assets.TextureRegister;
using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using CalamityMod;
using Microsoft.Build.Evaluation;
using CalamityInheritance.Utilities;
using Terraria.DataStructures;
using CalamityMod.Particles;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray.ElementalBeamProj
{
    public class ElementalNebula : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 12;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public Vector2 ProjScale = new( 0.5f, 0.5f);
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
            Projectile.Size = ProjScale;
        }
        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < 148;

        public override void AI()
        {
            int dType = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 2f);
            Main.dust[dType].noGravity = true;
            Main.dust[dType].velocity *= 0f;
            if (Projectile.timeLeft < 190)
                CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, 3000f, 12f, 25f);
            SparkParticle line = new SparkParticle(Projectile.Center - Projectile.velocity * 1.1f, Projectile.velocity * 0.01f, false, 18, 1f, Color.Purple);
            GeneralParticleHandler.SpawnParticle(line);
            Projectile.rotation += 0.12f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<ElementalMix>(), 180);
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
