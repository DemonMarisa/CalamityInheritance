using System;
using CalamityInheritance.Content.Items;
using CalamityInheritance.NPCs.Calamitas.Brothers;
using CalamityInheritance.NPCs.Calamitas.Minions;
using CalamityInheritance.NPCs.Calamitas.Projectiles;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.NPCs.Calamitas
{
    public static class CalamitasRebornAIPhase2
    {
        //这个AI有可能需要重写，冲刺的逻辑非常难改
        //将代码整体改干净一点
        public static readonly float BalltoLaserPhaseTimer = 300f; 
        public static void CalamitasRebornAI(NPC boss, Mod mod)
        {
            #region 初始化

            CIGlobalNPC cign = boss.CIMod();
            CIFunction.SetGlow(boss, 1f, 0f, 0f);
            
            //查看血量百分比.
            float lifePercent = boss.life / (float)boss.lifeMax;
            //旧灾的血量阶段, 这三个血量阶段仅用于生成高强度的兄弟战
            bool ifPhase2 = lifePercent < 0.7f; //70%, 第一波兄弟
            bool ifPhase3 = lifePercent < 0.4f; //40%, 第二波兄弟
            bool ifPhase4 = lifePercent < 0.3f; //30%, 生成探魂眼
            bool ifPhase5 = lifePercent < 0.1f; //10%, 最后的兄弟战

            //兄弟是否在场?
            bool ifBrothers = NPC.AnyNPCs(ModContent.NPCType<CataclysmReborn>()) || NPC.AnyNPCs(ModContent.NPCType<CatastropheReborn>());
            
            //将普灾最大血量先存进去
            if(cign.BossNewAI[0] == 0f && boss.life > 0)
               cign.BossNewAI[0] = boss.lifeMax;

            //给普灾找到一个target, 也就是玩家
            if(boss.target < 0 || boss.target == Main.maxPlayers || Main.player[boss.target].dead || !Main.player[boss.target].active)
                boss.TargetClosest();

            Player player = Main.player[boss.target];
            CIGlobalNPC.ThisCalamitasRebornP2 = boss.whoAmI;
            //生成探魂
            Seekers(boss, cign, ifPhase3);

            Vector2 getPlayerCenter = new(player.position.X - (player.width/2), player.position.Y - (player.height/2));
            #endregion

            #region 尝试视角朝向玩家

            Vector2 bossCenter = new(boss.Center.X, boss.Center.Y + boss.height - 59f);
            Vector2 lockTar = getPlayerCenter; 
            Vector2 tryLockPlayer = bossCenter - lockTar;
            float rot = (float)Math.Atan2(tryLockPlayer.Y, tryLockPlayer.X) + MathHelper.PiOver2;
            BrothersGeneric.TryKeeping(boss, 0.1f, rot);
            #endregion

            #region 使旧灾脱战
            //      玩家不存在      玩家似了         白天           非日食      都会让普灾脱战
            if(!player.active || player.dead || Main.dayTime || !Main.eclipse)
                DespawnThis.Despawn(boss, cign);
            else if (boss.timeLeft < 1800)
                boss.timeLeft = 1800;
            #endregion

            #region 普灾的常规移动,也就是追逐玩家等这些

            //旧灾与玩家如果是这个距离，就停止移动
            float getMoveDistGate = 100f;
            //移除死亡差分。
            float baseSpeed  =  18f  * (boss.ai[1] == 4f? 1.8f: 1f);
            float baseAccele =  0.2f * (boss.ai[1] == 4f? 1.8f : 1f);
            //TODO2: 这里，用于查看玩家是否手持真近战的减速，可能有潜在问题，到时候再看
            Item ifTrueMelee = player.inventory[player.selectedItem];
            if(ifTrueMelee.CountsAsClass<TrueMeleeDamageClass>()) baseAccele *= 0.5f;
            //旧灾的朝向? 取决于玩家位置
            int side = 1;
            if(boss.Center.X < player.Center.X)
                side = -1;
            
            //旧灾应该离玩家多远? 需注意的是如果要冲刺的话会距离更短
            float baseDist = 500f;
            float chargeDist = 400f;

            //旧灾实际应该针对谁?
            Vector2 realTar = (cign.BossNewAI[2] > 0f || boss.ai[1] == 0f) ? 
                               new(player.Center.X, player.Center.Y - baseDist) :
                               new(player.Center.X + chargeDist * side, player.Center.Y);
            
            //在部分攻击结束后，与玩家的距离赋予一点随机性
            if(boss.localAI[0] == 1f)
            {
                boss.localAI[0] = 0f;
                boss.localAI[2] = Main.rand.Next(-49, 52);
                boss.localAI[3] = Main.rand.Next(-299, 302);
                boss.netUpdate = true;
            }

            //随机度
            realTar.X += boss.ai[1] == 0f? boss.localAI[3] : boss.localAI[2];
            realTar.Y += boss.ai[1] == 0f? boss.localAI[2] : boss.localAI[3];

            //旧灾与针对的对象的实际距离应该是?
            Vector2 distToRealTar = realTar - boss.Center; 

            //让旧灾动起来, 暂时使用原灾的封装
            if(boss.ai[1] == 0f || boss.ai[1] == 1f || boss.ai[1] == 4f || cign.BossNewAI[2] > 0f)
                CalamityUtils.SmoothMovement(boss, getMoveDistGate, distToRealTar, baseSpeed, baseAccele, true);
            #endregion

            #region 攻击AI
            switch (boss.ai[1])
            {
                case 0f:
                    //进入发射激光AI的时间刻
                    boss.ai[2] += 1f;
                    if(boss.ai[2] >= BalltoLaserPhaseTimer)
                    {
                        boss.ai[1] = 1f;
                        boss.ai[2] = 0f;
                        boss.localAI[0] = 1f;
                        boss.netUpdate = true;
                    }
                    //尝试, 发射火球
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        boss.localAI[1] += 1f; 
                        if(!ifBrothers)
                        boss.localAI[1] += 2f * (1f - lifePercent); 
                    }

                    if(boss.localAI[1] >= (ifBrothers? 150f : 100f))
                    {
                        boss.localAI[1] = 0f;
                        float projVel = 15f;
                        int projType= ModContent.ProjectileType<HellfireballReborn>(); 
                        int projDMG = boss.GetProjectileDamage(projType);
                        //火球是否应当有预判?不过, 死亡模式下是默认有1/2概率预判的
                        bool projPredictive = Main.rand.NextBool();
                        Vector2 projPredictiveFactor = projPredictive? player.velocity * 20f : Vector2.Zero;
                        Vector2 fireBallVel = Vector2.Normalize(player.Center + projPredictiveFactor - boss.Center) * projVel;
                        Vector2 projOffset = Vector2.Normalize(fireBallVel) * 40f; 
                        int getProj = Projectile.NewProjectile(boss.GetSource_FromAI(), bossCenter + projOffset, fireBallVel, projType, projDMG, 0f, Main.myPlayer, player.position.X, player.position.Y);
                        Main.projectile[getProj].netUpdate = true;
                    }
                    break;
                case 1f:
                    boss.ai[2] += 1f;
                    if(boss.ai[2] >= 180f)
                    {   
                        //在第一波兄弟发起之前旧灾不会使出冲刺
                        if (boss.life >= boss.lifeMax * 0.7f) 
                            boss.ai[1] = 0f;
                        else boss.ai[1] = 2f; 

                        boss.ai[2] = 0f;
                        boss.localAI[0] = 1f; 
                        boss.netUpdate = true;
                    }
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        //计时器
                        boss.localAI[1] += ifBrothers? 2f : 1f; 
                        // 发射激光
                        if(boss.localAI[1] >= (ifBrothers? 75f : 45f))
                        {
                            //重置计时器
                            boss.localAI[1] = 0f;
                            int projType = ifBrothers? ModContent.ProjectileType<HellfireballReborn>() : ModContent.ProjectileType<BrimstoneLaser>();
                            //伤害写死
                            TryShootLaser(boss, player, bossCenter, 13f, projType, 120);
                        }
                    }
                    //发射激光三秒，发射完后悬浮到头上继续射火球, 或者进入冲刺ai
                    
                    break;
                case 2f:
                    //设置冲刺伤害, 兄弟在场的时候普灾没有伤害
                    boss.damage = ifBrothers ? 0 : boss.defDamage;
                    SpwanVisualChargeDust(boss);
                    BrothersCharge.ChargeInit(boss, player, rot, 42f);
                    boss.ai[1] = 3f;
                    boss.netUpdate = true;
                    return;
                /*
                *这个AI史了点但我总算知道怎么回事了
                *ai[2]间接作为冲刺时间的作用
                *在ai[1] = 3f标志进入冲刺的时候，这个时间就已经开始自增了
                *刚开始的0f-30f实际上是第一次冲刺时间，然后30f-75f的时候是第二次冲刺的时间
                *我草，我就因为这个破东西硬控两天？
                */
                case 3f:
                    //设置伤害
                    boss.damage = ifBrothers ? 0 : boss.defDamage;
                    SpwanVisualChargeDust(boss);
                    boss.ai[2] += 1f;
                    //微弱的加速度
                    boss.velocity *= 1.001f;

                    if(boss.ai[2] >= 30f)
                    {
                        boss.velocity.Y *= 0.9f;
                        boss.velocity.X *= 0.9f;
                        if(boss.velocity.X > -0.1 && boss.velocity.X < 0.1)
                            boss.velocity.X = 0f;
                        if(boss.velocity.Y > -0.1 && boss.velocity.Y < 0.1)
                            boss.velocity.Y = 0f;
                    }
                    else boss.rotation = (float)Math.Atan2(boss.velocity.Y, boss.velocity.X) - MathHelper.PiOver2;

                    if(boss.ai[2] >= 40f)
                    {
                        boss.ai[2] = 0f;
                        boss.ai[3] += 1f;
                        boss.rotation = rot;
                        boss.netUpdate = true;
                        //冲刺结束，间隔一段时间后执行其他AI
                        if(boss.ai[3] >= 2f)
                        {
                            boss.TargetClosest();
                            boss.ai[1] = 0f; //重新开始发射火球
                            boss.ai[3] = 0f;
                            return;
                        }
                        boss.ai[1] = 4f;
                    }
                    break;
                default:
                    boss.ai[2] += 1f;
                    if(boss.ai[2] >= 30f)
                    {
                        boss.ai[1] = 2f;
                        boss.ai[2] = 0f;
                        boss.localAI[0] = 0f;
                        boss.netUpdate = true;
                    }
                    break;
            }
            #endregion  
            #region 兄弟重生, 这一过程经历3次
            //70%, 40%, 10%重生一次
            if(boss.life > 0)
            {
                int getPhaseHP = (int)(boss.lifeMax * 0.3f);
                if(boss.life + getPhaseHP < cign.BossNewAI[0])
                {
                    cign.BossNewAI[0] = boss.life; //刷新一次血量
                    if(cign.BossNewAI[0] < boss.lifeMax * 0.1) //10%
                    {
                        SummonBrother(ref boss);
                    }
                    else if(cign.BossNewAI[0] <= boss.lifeMax * 0.4f) //40%
                    {
                        SummonBrother(ref boss);
                        /*与上方相同的代码*/
                    }
                    else if(cign.BossNewAI[0] <= boss.lifeMax * 0.7f) //70%
                    {
                        SummonBrother(ref boss);
                    }
                }
            }
            //兄弟重生时, 旧灾获得极高的防御力
            int calCloneDefense = boss.defDefense; //存储基本防御力
            calCloneDefense += (CIGlobalNPC.CatalysmCloneWhoAmI     != -1 && Main.npc[CIGlobalNPC.CatalysmCloneWhoAmI].active)?    50 : 0; //我也忘了加多少了，反正亵渎死球了+200防御力
            calCloneDefense += (CIGlobalNPC.CatastropheCloneWhoAmI  != -1 && Main.npc[CIGlobalNPC.CatastropheCloneWhoAmI].active)? 50 : 0;
            //谁活着, 都行
            boss.defense = ifBrothers ? boss.defDefense + calCloneDefense : boss.defDefense;
            //兄弟在场时旧灾不会被锁定
            boss.chaseable = !ifBrothers;
            #endregion
            //按理来说，不出意外的话我们只需要这样就能完成一个普灾AI了，但是实际情况？还得再看看细节上的问题
        }
        public static void SummonBrother(ref NPC boss)
        {
            //给予生成的一组兄弟一定的初始值来保证其不会与另一组兄弟同时冲刺
            float Ai0 = 5f;
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X + boss.width, (int)boss.Center.Y + boss.height, ModContent.NPCType<CataclysmReborn>(),   boss.whoAmI);
                NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X + boss.width, (int)boss.Center.Y - boss.height, ModContent.NPCType<CataclysmReborn>(),   boss.whoAmI, Ai0);
                NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X - boss.width, (int)boss.Center.Y + boss.height, ModContent.NPCType<CatastropheReborn>(), boss.whoAmI);
                NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X - boss.width, (int)boss.Center.Y - boss.height, ModContent.NPCType<CatastropheReborn>(), boss.whoAmI, Ai0);
            }
            //我们需要兄弟重生的文本
        }
        public static void SpwanVisualChargeDust(NPC boss)
        {
            if (Main.rand.NextBool())
            {
                Vector2 offset = new Vector2(12,0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 realOffset = new Vector2(4,0).RotatedBy(offset.ToRotation());
                Vector2 getDustVelocity = new (boss.velocity.X * 0.4f + realOffset.X, boss.velocity.Y * 0.4f + realOffset.Y);
                float dustScale = 1.5f;
                Dust d = Dust.NewDustPerfect(new Vector2(boss.Center.X, boss.Center.Y) + offset, CIDustID.DustBlood, getDustVelocity, 100, Color.Red, dustScale);
                d.noGravity = true;
            }

            if (Main.rand.NextBool(6))
            {
                Vector2 offset = new Vector2(12,0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 realOffset = new Vector2(4,0).RotatedBy(offset.ToRotation());
                Vector2 getDustVelocity = new (boss.velocity.X * 0.5f + realOffset.X, boss.velocity.Y * 0.5f + realOffset.Y);
                float dustScale = 1.9f;
                int getDust = CIDustID.DustHeatRay;
                Dust d = Dust.NewDustPerfect(new Vector2(boss.Center.X, boss.Center.Y) + offset, getDust, getDustVelocity, 100, Color.Red, dustScale);
                d.noGravity = true;
            }
        }
        /// <summary>
        /// 发射激光
        /// </summary>
        /// <param name="npc">普灾</param>
        /// <param name="target">玩家</param>
        /// <param name="npcCenter">普灾中心</param>
        /// <param name="pSpeed">射弹速度</param>
        /// <param name="pType">射弹类型</param>
        /// <param name="pDmg">射弹伤害</param>
        public static void TryShootLaser(NPC npc, Player target, Vector2 npcCenter,float pSpeed, int pType, int pDmg)
        {
            Vector2 pVel = Vector2.Normalize(target.Center - npc.Center) * pSpeed;
            Vector2 pPos = Vector2.Normalize(pVel) * 40f;
            if (!Collision.CanHit(npc.position,npc.width,npc.height,npc.position,npc.width,npc.height))
                Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + pPos, pVel, ModContent.ProjectileType<BrimstoneLaser>(), pDmg, 0f, Main.myPlayer, target.position.X, target.position.Y);
            else Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + pPos, pVel, pType, pDmg, 0f, Main.myPlayer, target.position.X, target.position.Y);
        }
        /// <summary>
        /// 生成探魂眼环
        /// </summary>
        /// <param name="Boss">旧灾</param>
        public static void Seekers(NPC boss, CIGlobalNPC cign, bool ifPhase3)
        {
            if(cign.BossNewAI[1] == 0f && ifPhase3)
            {
                SoundEngine.PlaySound(SoundID.Item72, boss.Center);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int eyeAmt = 12;
                    int eyespread = 360 / 10;
                    int eyeDist = 180;
                    for (int i = 0; i < eyeAmt; i++)
                    {
                        int newEye = NPC.NewNPC(boss.GetSource_FromAI(), (int)(boss.Center.X + (Math.Sin(i * eyespread) * eyeDist)), (int)(boss.Center.Y + (Math.Cos(i * eyespread) * eyeDist)), ModContent.NPCType<SoulSeekerReborn>(), boss.whoAmI, 0, 0, 0, -1);
                        Main.npc[newEye].ai[0] = i * eyespread;
                    }
                }
            }
            //此处需要一个探魂眼承重生文本
            cign.BossNewAI[1] = 1f;
        }
    }
}