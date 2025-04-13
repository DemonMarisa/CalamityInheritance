using System;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class ACTExcelsusBlue : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        #region 基础属性
        public bool AlreadyHit = false;
        //阶段Timer
        public int Timer = 0;
        public int TimerAlt = 0;
        public int MouseTimer = 0;
        public const int IdleTimer = 30;
        //非追踪状态下的旋转
        public const float NonHomingRotation = 0.45f;
        
        #endregion
        #region 攻击枚举
        public enum DoStyle
        {
            IsFlying,
            IsHoming,
            IsIdleing,
            IsHit,
        }
        public static DoStyle[] DoAttack =>
        [
            DoStyle.IsFlying,
            DoStyle.IsHit,
            DoStyle.IsIdleing,
            DoStyle.IsHit,
        ];
        #endregion   
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 300;
            Projectile.alpha = 100;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        //重做所有AI
        public override bool? CanDamage() => Timer < ACTExcelsus.HomingTimer || TimerAlt > ACTExcelsus.IdleTimer;
        public override void AI()
        {

            ref float aStlye = ref Projectile.ai[0];
            //搜寻实例
            NPC tar = CIFunction.FindClosestTarget(Projectile, ACTExcelsus.MaxSearchDist, true);
            if (Timer > ACTExcelsus.HomingTimer)
            {
                if (tar != null)
                {
                    aStlye = AlreadyHit ? (int)DoStyle.IsHit : (int)DoStyle.IsHoming;
                }
                else
                {
                    aStlye = (int)DoStyle.IsIdleing;
                }
            }
            else aStlye = (int)DoStyle.IsFlying;
            //切换AI逻辑
            //这Dom的写法他就是香啊. 清晰多了
            switch ((DoStyle)aStlye)
            {
                case DoStyle.IsFlying:
                    DoFlying();
                    break;
                case DoStyle.IsHoming:
                    DoHoming(tar);
                    break;
                case DoStyle.IsIdleing:
                    DoIdleing();
                    break;
                case DoStyle.IsHit:
                    DoIsHit();
                    break;
            }
            //保留特效
            if (Main.rand.NextBool(8))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueFairy, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }
        //飞行逻辑
        private void DoIsHit()
        {
            //如果单纯击中了敌怪，则刷新timer和Alpha，使其逐渐消失
            if (Projectile.timeLeft > ACTExcelsus.SideFadeInTime)
                Projectile.timeLeft = ACTExcelsus.SideFadeInTime;
            Projectile.alpha += (int)Utils.GetLerpValue(0, 255, 15);
            Projectile.velocity *= ACTExcelsus.SideHitSlowSpeed;
        }

        private void DoIdleing()
        {
            //自定义：其余状态下单独让这个刀片指向指针, 并且在原地进行待机。
            float rot = Projectile.AngleTo(Main.MouseWorld) + MathHelper.PiOver4;
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, rot, ACTExcelsus.LerpAngle);
            Projectile.velocity *= ACTExcelsus.SideIdleSlowSpeed;
            MouseTimer += 1;
            if (MouseTimer > ACTExcelsus.IdleTimer)
            {
                Projectile.extraUpdates += 1;
                CIFunction.HomeInOnMouseBetter(Projectile, 16f, 20f, 1, false, Vector2.Distance(Projectile.Center, Main.MouseWorld) / 3);
            }

        }

        public void DoFlying()
        {
            Timer++;
            //飞行过程中射弹会一直保持高速旋转, 除此之外就……就不干什么了。
            Projectile.rotation += ACTExcelsus.NonHomingRotation;
        }
        //追踪逻辑
        public void DoHoming(NPC tar)
        {
            MouseTimer = 0;
            //需注意的是，AI执行的这段时间内也会一直检索目标。
            Player p = Main.player[Projectile.owner];
            float spiningDir = ACTExcelsus.LerpAngle;
            //如果实例存在，将刀片指向这个实例
            if (TimerAlt == 5)
                //刷新射弹生命与判定次数
                ACTExcelsus.GlobalResetProj(Projectile); 
            Vector2 targetPos = tar.Center;
            //原地减速，指向这个敌怪
            float rot = Projectile.AngleTo(targetPos) + MathHelper.PiOver4;
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, rot, spiningDir);
            Projectile.velocity *= ACTExcelsus.SideIdleSlowSpeed;
            //而后在一段时间后发起追踪
            TimerAlt ++;
            if (TimerAlt > ACTExcelsus.IdleTimer)
            {
                //给多一个额外更新
                Projectile.extraUpdates += 1;
                CIFunction.HomingNPCBetter(Projectile, tar, ACTExcelsus.MaxSearchDist, ACTExcelsus.HomingSpeed, 20f, 1);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.tileCollide = false;
            if (Projectile.timeLeft > 85)
            {
                Projectile.timeLeft = 85;
            }
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * (b2 / 255f));
                return new Color(b2, b2, b2, a2);
            }
            return default(Color);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Color color;
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * (b2 / 255f));
                color = new Color(b2, b2, b2, a2);
            }
            else
            {
                color = new Color(255, 255, 255, 100);
            }
            Vector2 origin = new Vector2(39f, 46f);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>($"{GenericProjRoute.ProjRoute}/Melee/ACTExcelsusBlueGlow").Value, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //追踪情况下标记为True
            if (Projectile.ai[0] == (int)DoStyle.IsHoming)
                AlreadyHit = true;     
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 180);
        }
}
}
