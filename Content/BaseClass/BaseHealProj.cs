using CalamityInheritance.Utilities;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.BaseClass
{
    public abstract class BaseHealProj : ModProjectile
    {
        // 需要搭配方法使用
        #region 别名
        public ref float FlySpeed => ref Projectile.ai[0];
        public ref float Acceleration => ref Projectile.ai[1];
        public ref float HealAmt => ref Projectile.ai[2];
        public Player Healer => Main.player[Projectile.owner];
        #endregion

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 24;
            Projectile.friendly = true;
            //默认300
            Projectile.timeLeft = 30000;
            //干掉不可穿墙
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            ExSD();
        }
        // 额外的SD
        public virtual void ExSD()
        {

        }

        public override void AI()
        {
            //直接追踪锁定玩家位置就行了。我也不知道为什么要做别的事情。
            //人都似了为什么还要跑这个弹幕？
            //距离玩家过远也直接处死这个弹幕，没得玩的
            if (!Healer.active || Healer.dead || (Projectile.Center - Healer.Center).Length() > 3000f)
            {
                Projectile.netUpdate = true;
                Projectile.Kill();
                return;
            }
            //设置跟踪玩家的AI
            CIFunction.HomeInPlayer(Healer, Projectile, 18f, FlySpeed, Acceleration);
            float distance = (Projectile.Center - Healer.Center).Length();
            if (Projectile.Hitbox.Intersects(Healer.Hitbox) || distance < 20f)
            {
                //干掉射弹即可
                Projectile.netUpdate = true;
                Projectile.Kill();
            }
            ExAI();
        }
        // 额外的AI
        public virtual void ExAI()
        {

        }
        public override void OnKill(int timeLeft)
        {
            //根据提供的恢复量给予治疗
            Healer.Heal((int)HealAmt);
            ExKill();
        }
        // 额外的Kill
        public virtual void ExKill()
        {

        }
    }
}
