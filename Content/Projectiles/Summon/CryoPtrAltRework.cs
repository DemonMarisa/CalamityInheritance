using System;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Summon
{
    public class CryoPtrAltRework : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Summon";
        public override string Texture => $"{GenericProjRoute.ProjRoute}/Summon/CryogenPtr";
        public override void SetDefaults() => Projectile.CloneDefaults(ProjectileType<CryoPtrRework>());
        public float FloatyDist = 90f;
        const int NPCStored = 0;
        const int RoundingAI = 1;
        const int AttackTimeChanger = 120;
        /*
        *这里有两套逻辑
        *1: PreAI(): 实际的攻击AI, 即右键功能下发起攻击的AI
        *2: AI(): 动画AI，绕圈是在这里做的
        */
        public override bool PreAI()
        {
            Player p = Main.player[Projectile.owner];
            NPC getTarget = CIFunction.FindClosestTarget(Projectile, 1800f, true, true);
            //如果搜索到敌怪，直接执行下方的AI钩子
            if (getTarget != null)
            {
                //将这个npc存放到数组内
                Projectile.ai[NPCStored] = getTarget.whoAmI;
                //刷新射弹生命，我们需要这个射弹生命去间接实现攻击逻辑
                Projectile.timeLeft = 3000;
                return true;
            }
            //否则不执行右键，并干掉下方的AI钩子 
            return false;
        }
        public override void AI()
        {
            //将敌对单位提取出来使用
            int tarIndex = (int)Projectile.ai[NPCStored];
            NPC target = Main.npc[tarIndex];
            //但凡指定单位已经不存在，直接干掉射弹
            if (target != null && (!target.active || target.life <= 0))
                Projectile.Kill();
            //无论是哪个状态下面这个AI都是该执行的
            Projectile.Center = target.Center + Projectile.ai[RoundingAI].ToRotationVector2() * FloatyDist;
            Projectile.rotation = Projectile.ai[RoundingAI] + MathHelper.PiOver2;
            Vector2 vel = Projectile.rotation.ToRotationVector2() - target.Center;
            vel.Normalize();
            Projectile.ai[RoundingAI] -= MathHelper.ToRadians(4f);
            //整个AI分两个阶段：环绕阶段与开始攻击的阶段，采用timeLeft去执行
            if (Projectile.timeLeft > AttackTimeChanger)
            {
                
            }
            else
            {
                //现在这个东西要开始攻击了，我们需要刷新其射弹属性
                ResetProj();
                //刷新后根据与敌怪的距离逐渐放缩floatyDist
                Projectile.rotation = Projectile.timeLeft <= 60 ? Projectile.ai[RoundingAI] - MathHelper.PiOver2 : Projectile.rotation - (MathHelper.Distance(Projectile.rotation, -Projectile.rotation) / (120 - 60));
                if (Projectile.timeLeft > 60)
                    FloatyDist += 5;
                else
                    FloatyDist -= 10;
                
            }
        }

        private void ResetProj()
        {
            Projectile.usesIDStaticNPCImmunity = false;
            //一次
            Projectile.penetrate = 1;
        }
    }
}