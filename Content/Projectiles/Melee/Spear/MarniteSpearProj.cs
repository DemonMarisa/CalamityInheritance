using System;
using System.Numerics;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.Content.Projectiles.Melee.Spear
{
    public class MarniteSpearProj : ModProjectile, ILocalizedModType
    {
        protected virtual float RangeMin => 24f;
        protected virtual float RangeMax => 96f;
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 50;
            Projectile.aiStyle = ProjAIStyleID.Spear;    
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.timeLeft = 90;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.ignoreWater = true;
        }

        public override bool PreAI()
        {
            if (!Projectile.CalamityInheritance().ThrownMode)
            {
                Player owner = Main.player[Projectile.owner];
                int dura = owner.itemAnimationMax;
                owner.heldProj = Projectile.whoAmI;

                //必要时刻重置生命
                if (Projectile.timeLeft > dura)
                    Projectile.timeLeft = dura;

                Projectile.velocity = Vector2.Normalize(Projectile.velocity);

                float halfDura = dura * 0.5f;
                float progression;

                if (Projectile.timeLeft < halfDura)
                    progression = Projectile.timeLeft / halfDura;
                else
                    progression = (dura - Projectile.timeLeft ) / halfDura;
                
                //让矛开始移动
                Projectile.Center = owner.MountedCenter + Vector2.SmoothStep(Projectile.velocity * RangeMin, Projectile.velocity * RangeMax, progression);

                //给猫一个正确的转角
                if (Projectile.spriteDirection == -1)
                    //贴图朝左，转45°
                    Projectile.rotation += MathHelper.ToRadians(45f);
                else
                    //贴图朝右，转135°
                    Projectile.rotation += MathHelper.ToRadians(135f);

                // 避免粒子生成在服务器生成
                if (!Main.dedServ)
                {
                    if (Main.rand.NextBool(3))
                        Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, CIDustID.DustFrostDagger, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f, 128, default, 1.2f);
                    if (Main.rand.NextBool(4))
                        Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, CIDustID.DustFrostDagger, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f, 128, default, 0.3f);
                }
                //干掉AI钩子
                return false;
            }
            //否则正常执行AI
            return true;
        }
        public override void AI()
        {
            
            //刷新射弹属性
            ResetProj();
            Projectile.ai[0] += 1f;
            //固定飞行一段距离后才会受重力影响
            if (Projectile.ai[0] > 75f)
            {
                Projectile.velocity.Y += 0.09f;
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
        }
        public void ResetProj()
        {
            //如果执行投掷的AI则刷新一次穿透次数(其实就是只能打一次)
            Projectile.penetrate = 1;
            //给予足够的生命值，让他不至于飞行到中途就离世
            Projectile.timeLeft = 600;
            //给予2eU, 这个eU的作用仅仅是为了提速
            Projectile.extraUpdates = 1;
            //取消矛的ai，即aistyle变成自定义
            Projectile.aiStyle = 0;
            //干掉穿墙
            Projectile.tileCollide = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //这里“只”允许投掷的投矛造成下面的效果
            if (Projectile.CalamityInheritance().ThrownMode)
            {
                OnThrowEffect(target, hit);
                OnThrowDust();
            }
        }
        public void OnThrowEffect(NPC target, NPC.HitInfo hit)
        {
            //将投矛“击中”前的速度存储进去
            Vector2 getVel = Vector2.Normalize(Projectile.oldVelocity);
            //现在给对方单位“强制”击退，或者说强行禁锢。这个是故意的做法
            if (target.IsABoss())
            {
                //如果敌对单位是一个boss，则给予的击退力量会更少
                if (Main.rand.NextBool(5))
                    target.velocity = getVel * 0.05f;
                //但是允许对boss造成100%的暴击
                if (!hit.Crit)
                    hit.Damage *= 2;
            }
            else
            {
                //否则，强制击退一段距离
                target.velocity = getVel * 1.4f;
            }
        }
        //只允许投矛状态获得击中粒子/与音效
        public void OnThrowDust()
        {
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height,CIDustID.DustMeteor, Projectile.oldVelocity.X * 0.75f, Projectile.oldVelocity.Y * 0.75f);
                Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height,CIDustID.DustFrostDagger, Projectile.oldVelocity.X * 0.75f, Projectile.oldVelocity.Y * 0.75f);
            }
        }
    }
}