using System;
using System.Collections.Generic;
using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class EclipseSpearProjStealth : ModProjectile, ILocalizedModType
    {
        //使用Projectile.ai[0]来查看其是否在发起挂载
        public bool IsSticking
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }
        //使用Projectile.ai[1]来存放一个挂载的敌怪单位
        public int WhatTar
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        //使用Projectile.localAI[0]来存放挂载时间
        public float StickingTime
        {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }
        public bool ResetProj = false;
        public bool TooFarAway = false;
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => $"{Generic.WeaponRoute}/Rogue/EclipseSpear";

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            //潜伏就应该扔一个超高速的东西出来
            Projectile.MaxUpdates = 8;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 150 * Projectile.MaxUpdates;
        }
        //现在我们开始处理AI
        public override void AI()
        {
            //666, tMod写挂载都不用800行你灾能写800行
            if (!IsSticking)
                NormalAI();
            else
                StickingAI();
        }
        private void NormalAI()
        {
            //本质上就是一个直线飞行的射弹我们不需要写任何东西, 使其发光，保住转角即可
            //更新: 追加了日食爆炸
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Lighting.AddLight(Projectile.Center, 1f, 0.8f, 0.3f);

            //获取玩家的位置
            Player plr = Main.player[Projectile.owner];
            float distX = Projectile.Center.X - plr.Center.X;
            float distY = Projectile.Center.Y - plr.Center.Y;
            float realDist = CIFunction.TryGetVectorMud(distX, distY);
            //如果距离太远则取真，强行使其追踪随机一个敌怪以发起挂载
            if (realDist > 600f)
            {
                TooFarAway = true;
                CIFunction.HomeInOnNPC(Projectile, true, 1000f, 20f, 20f);
            }
                
        }
        private void StickingAI()
        {
            //刷新射弹属性
            if (!ResetProj)
            {
                //发起挂载时射弹伤害会被砍成1/2
                Projectile.damage = (int)(Projectile.damage * 0.5f);
                Projectile.MaxUpdates = 1;
                Projectile.usesLocalNPCImmunity = true;
                //1秒1判，挂载10秒，总共10判
                Projectile.penetrate = -1;
                Projectile.localNPCHitCooldown = 60;
                Projectile.timeLeft = 600;
                ResetProj = true;
            }
            int npcTarget = WhatTar;
            if (Main.npc[npcTarget].active && !Main.npc[npcTarget].dontTakeDamage)
            {
                //往死里绑定这个玩意
                Projectile.Center = Main.npc[npcTarget].Center - Projectile.velocity * 2f;
				Projectile.gfxOffY = Main.npc[npcTarget].gfxOffY;
                if (Projectile.timeLeft % 15 == 0)
                    RainDownSpears();
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!IsSticking)
            {
                //击中一次，产生一次爆炸
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.ProjectileType<EclipseStealthBoom>(), Projectile.damage * 2, Projectile.knockBack * Projectile.damage, Projectile.owner);
                //LocalAI[1]用于存储击中次数
                Projectile.localAI[1] += 1;
                //如果是距离过远的直接给这个取真
                if (Projectile.localAI[1] == 5 || TooFarAway)
                    IsSticking = true;
            }
            else
            {
                WhatTar = target.whoAmI;
                Projectile.velocity = (target.Center - Projectile.Center) * 0.75f;
           
            }
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (IsSticking) {
				int npcIndex = WhatTar;
				if (npcIndex >= 0 && npcIndex < 200 && Main.npc[npcIndex].active) {
					if (Main.npc[npcIndex].behindTiles) {
						behindNPCsAndTiles.Add(index);
					}
					else {
						behindNPCsAndTiles.Add(index);
					}
					return;
				}
			}
			behindNPCsAndTiles.Add(index);
        }
        private void RainDownSpears()
        {
            //潜伏情况下一直在发起挂载，则每次从天上落下2~3个
            Vector2 tarPos = Projectile.Center;
            int pAmt = Main.rand.Next(2,4);
            for (int i = 0; i < pAmt; i++)
            {
                //随机水平位置
                float pSummonPosX = tarPos.X + Main.rand.NextFloat(-200f, 201f);
                //生成的高度
                float pSummonPosY = tarPos.Y - Main.rand.NextFloat(550f, 880f);
                Vector2 pPos = new (pSummonPosX, pSummonPosY);
                //速度
                Vector2 speed = tarPos - pPos;
                //水平速度一点随机读
                speed.X += Main.rand.NextFloat(-15f, 16f);
                float pSpeed = 24f;
                float tarDist = speed.Length();
                //固定格式
                tarDist = pSpeed / tarDist;
                speed.X *= tarDist;
                speed.Y *= tarDist;
                //生崽
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), pPos, speed, ModContent.ProjectileType<EclipseSpearSmall>(), Projectile.damage / 4, Projectile.knockBack, Projectile.owner);
            }
        }
    }
}