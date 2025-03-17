using System;
using CalamityInheritance.NPCs.Calamitas.Minions;
using CalamityInheritance.NPCs.Calamitas.Projectiles;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.NPCs.Calamitas
{
    public static class CalamitasRebornAIPhase1
    {
   
        public static void CalamitasRebornAI(NPC boss, Mod mod)
        {
            #region 初始化
            //Scarlet:这是一个副本文档，用于重新整顿普灾AI
            //大部分的编码从灾厄现版本转移过来
            CIGlobalNPC cign = boss.CIMod();

            //使普灾[下称旧灾]发光, 这个会一直持续下去
            AddRedLight(boss);
            //返厂的旧灾只会有一个死亡模式的差分，并不需要bool，但是这里写下来主要是为了强化印象
            bool ifDeath = CalamityWorld.death || CalamityWorld.revenge || Main.expertMode; //让所有的难度都直接跑

            //查看血量百分比.
            float lifePercent = boss.life / (float)boss.lifeMax;
            //在原有的死亡模式下，普灾的一阶段实际仅仅只有30%血量
            //这里也是一样，我们并不需要非常复杂的AI，只需要一个阶段，然后在这个时候生成新的二阶段AI就行了
            bool ifNewCal = lifePercent < 0.7f; //70%

            //将普灾最大血量先存进去
            if(cign.BossNewAI[0] == 0f && boss.life >0)
               cign.BossNewAI[0] = boss.lifeMax;

            int getPhaseHP = (int)(boss.lifeMax * 0.3f); //我们只需要一个30%血量
            if (boss.life <= boss.lifeMax - getPhaseHP && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X, (int)boss.Center.Y + boss.height, ModContent.NPCType<CalamitasRebornPhase2>(), boss.whoAmI);
                //这里需要一个生成的文本
                // string key = null;
                // Color keyColor = Color.Orange;
                // if(Main.netMode == NetmodeID.SinglePlayer)
                // {
                //     Main.NewText(Language.GetTextValue(key), keyColor);
                // }
                boss.active = false;
                boss.netUpdate = true;
                return;
            }
            //给普灾找到一个target, 也就是玩家
            if(boss.target < 0 || boss.target == Main.maxPlayers || Main.player[boss.target].dead || !Main.player[boss.target].active)
                boss.TargetClosest();

            //将这个玩家取出来用
            Player player = Main.player[boss.target];
            CIGlobalNPC.ThisCalamitasReborn = boss.whoAmI;
            //获取玩家的中心位置, 只有这个作用, 如果有别的需求得另寻他法
            Vector2 getPlayerCenter = new(player.position.X - (player.width/2), player.position.Y - (player.height/2));
            #endregion
            #region 尝试视角朝向玩家
            //普灾旋转一分钟.gif
            Vector2 bossCenter = new(boss.Center.X, boss.Center.Y + boss.height - 59f); //Comment: 你这贴图有问题啊
            Vector2 lockTar = getPlayerCenter; //转变量名强化印象
            Vector2 tryLockPlayer = bossCenter - lockTar; //尝试朝向玩家
            //角度
            float rot = (float)Math.Atan2(tryLockPlayer.Y, tryLockPlayer.X) + MathHelper.PiOver2;
            if(rot < 0f) rot +=MathHelper.TwoPi;
            else if (rot > MathHelper.TwoPi) rot -= MathHelper.TwoPi; //确保转角一直在2pi内

            float rotSpeed = 0.1f; //转速 
            //下面这些代码都是为了保证转角是对的,
            if(boss.rotation < rot)
            {
                if((rot - boss.rotation) > MathHelper.Pi) //小于pi, 减转速
                    boss.rotation -= rotSpeed;
                else boss.rotation += rotSpeed;
            }
            else if (boss.rotation > rot)
            {
                if((boss.rotation - rot) > MathHelper.Pi)
                    boss.rotation += rotSpeed;
                else
                    boss.rotation -= rotSpeed;
            }

            if(boss.rotation > rot - rotSpeed && boss.rotation < rot + rotSpeed)
               boss.rotation = rot;
            //确保boss转角一直在2pi内
            if(boss.rotation < 0f)
               boss.rotation += MathHelper.TwoPi;
            else if(boss.rotation > MathHelper.TwoPi)
               boss.rotation -= MathHelper.TwoPi;
            if(boss.rotation > rot - rotSpeed && boss.rotation < rot +rotSpeed)
               boss.rotation = rot;
            #endregion
            #region 使旧灾脱战
            //      玩家不存在      玩家似了         白天           非日食      都会让普灾脱战
            if(!player.active || player.dead || Main.dayTime || !Main.eclipse)
            {
                boss.TargetClosest(false);
                player = Main.player[boss.target];
                if(!player.active || player.dead)
                {
                    if(boss.velocity.Y > 3f)
                       boss.velocity.Y = 3f;
                    boss.velocity.Y -= 0.1f;
                    if(boss.velocity.Y < -12f)
                       boss.velocity.Y = -12f;
                    
                    if(boss.timeLeft > 60)
                       boss.timeLeft = 60;
                    
                    if(boss.ai[0] != 0f)
                    {
                        boss.ai[1] = 0f;
                        boss.ai[2] = 0f;
                        boss.ai[3] = 0f;
                        cign.BossNewAI[2] = 0f;
                        cign.BossNewAI[3] = 0f;
                        boss.alpha = 0;
                        boss.netUpdate = true;
                    }
                    return;
                }
            }
            else if (boss.timeLeft < 1800)
                boss.timeLeft = 1800;
            #endregion
            #region 旧灾与他的目标
            //旧灾与玩家如果是这个距离，就停止移动
            float getMoveDistGate = 100f;

            //旧灾追上玩家的速度，因为本mod只考虑死亡，因此只有死亡的差分
            //TODO1:假定旧灾能跑起来，这里的速度可能需要降低以匹配武器强度
            float baseSpeed =   (ifDeath? 12f : 8f)     * (boss.ai[1] == 4f? 1.8f: 1f);
            float baseAccele =  (ifDeath? 0.2f : 0.15f) * (boss.ai[1] == 4f? 1.8f : 1f);
            //TODO2: 这里，用于查看玩家是否手持真近战的减速，可能有潜在问题，到时候再看
            Item ifTrueMelee = player.inventory[player.selectedItem];
            if(ifTrueMelee.CountsAsClass<TrueMeleeDamageClass>()) baseAccele *= 0.5f;
            //TODO3: 同上, 但这里是亵渎天神如果被干掉的情况
            //旧灾的朝向? 取决于玩家位置
            int side = 1;
            if(boss.Center.X < player.Center.X)
                side = -1;
            
            //旧灾应该离玩家多远? 需注意的是冲刺的话会距离更短
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
            #region 悬浮于头上尝试发射火球
            if(boss.ai[1] == 0f)
            {
                boss.ai[2] += 1f;
                float nextPhase = 250f;
                if(boss.ai[2] >= nextPhase)
                {
                    boss.ai[1] = 1f; //兄弟不在的时候进入水平
                    boss.ai[2] = 0f;
                    boss.localAI[0] = 1f;
                    boss.netUpdate = true;
                }
                //尝试, 发射火球
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    //依旧, localAI[1]作为一个计时器
                    boss.localAI[1] += 1f; 
                    //兄弟不在场的时候, 计时器转的更快
                    boss.localAI[1] += 2f * (1f - lifePercent); 
                }
                if(boss.localAI[1] >= 120f)
                {
                    boss.localAI[1] = 0f;
                    float projVel = 14f;
                    int projType= ModContent.ProjectileType<HellfireballReborn>(); //TODO4:使用旧版火球
                    int projDMG = boss.GetProjectileDamage(projType);
                    //火球是否应当有预判?不过, 死亡模式下是默认有1/2概率预判的
                    bool projPredictive = Main.rand.NextBool();
                    Vector2 projPredictiveFactor = projPredictive? player.velocity * 20f : Vector2.Zero;
                    Vector2 fireBallVel = Vector2.Normalize(player.Center + projPredictiveFactor - boss.Center) * projVel;
                    Vector2 projOffset = Vector2.Normalize(fireBallVel) * 40f; 
                    int getProj = Projectile.NewProjectile(boss.GetSource_FromAI(), bossCenter + projOffset, fireBallVel, projType, projDMG, 0f, Main.myPlayer, player.position.X, player.position.Y);
                    Main.projectile[getProj].netUpdate = true;
                }
            }
            #endregion
            #region 在侧身位置发射激光
            else if (boss.ai[1] == 1f)
            {
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    boss.localAI[1] += 2f; //计时器

                    // 发射激光
                    if(boss.localAI[1] >= 50f && Collision.CanHit(boss.position,boss.width,boss.height,player.position,player.width,player.height))
                    {
                        boss.localAI[1] = 0f;//重置
                        float projVel = 12.5f;
                        int projType = ModContent.ProjectileType<BrimstoneLaser>();
                        int projDMG = boss.GetProjectileDamage(projType);
                        Vector2 laserVel = Vector2.Normalize(player.Center - boss.Center) * projVel;
                        Vector2 laserOffset = Vector2.Normalize(laserVel) * 40f;
                        if(!Collision.CanHit(boss.position,boss.width,boss.height,player.position,player.width,player.height))
                        {
                            projType = ModContent.ProjectileType<BrimstoneLaser>();
                            projDMG = boss.GetProjectileDamage(projType);
                            Projectile.NewProjectile(boss.GetSource_FromAI(), boss.Center + laserOffset, laserVel, projType, projDMG, 0f, Main.myPlayer, player.position.X, player.position.Y);
                        }
                        else
                        {
                            float Ai0 = projType == ModContent.ProjectileType<BrimstoneLaser>() ? 1f : player.position.X;
                            float Ai1 = projType == ModContent.ProjectileType<BrimstoneLaser>() ? 0f : player.position.Y;
                            Projectile.NewProjectile(boss.GetSource_FromAI(), boss.Center + laserOffset, laserVel, projType, projDMG, 0f, Main.myPlayer, Ai0, Ai1);
                        }
                    }
                }
                boss.ai[2] += 1f;
                float nextPhase = 180f;
                if(boss.ai[2] >= nextPhase)
                {   
                    boss.ai[1] = 0f;
                    boss.localAI[0] = 1f; //重新初始化这个timer
                    boss.netUpdate = true;
                }
            }
            #endregion
            //按理来说，不出意外的话我们只需要这样就能完成一个普灾AI了，但是实际情况？还得再看看细节上的问题
        }
        /// <summary>
        /// 生成粒子
        /// </summary>
        public static void SpawnDust()
        {
            
        }
        /// <summary>
        /// 补红光
        /// </summary>
        /// <param name="boss">需要补红光的单位</param>
        public static void AddRedLight(NPC boss)
        {
            float lightingPosX = (boss.position.X + (boss.width/2))/16;
            float lightingPosY = (boss.position.Y + (boss.width/2))/16;
            Lighting.AddLight(new Vector2(lightingPosX, lightingPosY), 1f, 0f, 0f); //(255,0,0)
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