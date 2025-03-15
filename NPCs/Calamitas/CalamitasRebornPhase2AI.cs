using System;
using CalamityInheritance.Content.Items;
using CalamityInheritance.NPCs.Calamitas.Brothers;
using CalamityInheritance.NPCs.Calamitas.Minions;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.NPCs.CalClone;
using CalamityMod.Projectiles.Boss;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.NPCs.Calamitas
{
    public static class CalamitasRebornAIPhase2
    {
   
        public static void CalamitasRebornAI(NPC boss, Mod mod)
        {
            #region 初始化
            //Scarlet:这是一个副本文档，用于重新整顿普灾AI
            //大部分的编码从灾厄现版本转移过来
            CIGlobalNPC cign = boss.CIMod();
            //使普灾[下称旧灾]发光, 这个会一直持续下去
            CIFunction.SetGlow(boss, 1f, 0f, 0f);
            //返厂的旧灾只会有一个死亡模式的差分，并不需要bool，但是这里写下来主要是为了强化印象
            bool ifDeath = CalamityWorld.death || CalamityWorld.revenge || Main.expertMode; //让所有的难度都直接跑

            //查看血量百分比.
            float lifePercent = boss.life / (float)boss.lifeMax;

            //旧灾的血量阶段, 这三个血量阶段仅用于生成高强度的兄弟战
            //灾厄中死亡模式默认开局二阶段，但旧灾不是
            bool ifPhase2 = lifePercent < 0.7f; //70%, 第一波兄弟
            bool ifPhase3 = lifePercent < 0.4f; //40%, 第二波兄弟
            bool ifPhase4 = lifePercent < 0.3f; //30%, 生成探魂眼
            bool ifPhase5 = lifePercent < 0.1f; //10%, 最后的兄弟战

            //兄弟是否在场?
            bool ifBrothers = false;
            //亵渎天神是否击败?
            //3/7撤销
            // bool ifProviDead = CalamityConditions.DownedProvidence.IsMet();
            
            //将普灾最大血量先存进去
            if(cign.BossNewAI[0] == 0f && boss.life > 0)
               cign.BossNewAI[0] = boss.lifeMax;

            //给普灾找到一个target, 也就是玩家
            if(boss.target < 0 || boss.target == Main.maxPlayers || Main.player[boss.target].dead || !Main.player[boss.target].active)
                boss.TargetClosest();

            //将这个玩家取出来用
            Player player = Main.player[boss.target];
            CIGlobalNPC.ThisCalamitasRebornP2 = boss.whoAmI;
            //生成探魂，战斗吧孩子
            Seekers(boss, cign, ifPhase3);
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
            BrothersGeneric.KeepAngle(boss, 0.1f, rot);
            #endregion
            #region 使旧灾脱战
            //      玩家不存在      玩家似了         白天           非日食      都会让普灾脱战
            if(!player.active || player.dead || Main.dayTime || !Main.eclipse)
                DespawnThis.Despawn(boss, cign);
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
                if(boss.ai[2] >= nextPhase || ifPhase4)
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
                    if(!ifBrothers)
                    //兄弟不在场的时候, 计时器转的更快
                    boss.localAI[1] += 2f * (1f - lifePercent); 
                }
                //计时器符合这两个值?符合就开始发射火球
                if(boss.localAI[1] >= (ifBrothers?120f:90f))
                {
                    boss.localAI[1] = 0f;
                    float projVel = 15f;
                    //TODO4:使用旧版火球
                    int projType= ModContent.ProjectileType<BrimstoneHellfireball>(); 
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
                    //计时器
                    boss.localAI[1] += 1f; 
                    if(!ifBrothers)
                    boss.localAI[1] += 1f;

                    // 发射激光
                    if(boss.localAI[1] >= (ifBrothers? 75f :45f) && Collision.CanHit(boss.position,boss.width,boss.height,player.position,player.width,player.height))
                    {
                        //重置计时器
                        boss.localAI[1] = 0f;
                        float projVel = 12.5f;
                        int projType = ifBrothers? ModContent.ProjectileType<BrimstoneHellfireball>() : ModContent.ProjectileType<BrimstoneHellblast>();
                        int projDMG = boss.GetProjectileDamage(projType);
                        Vector2 laserVel = Vector2.Normalize(player.Center - boss.Center) * projVel;
                        Vector2 laserOffset = Vector2.Normalize(laserVel) * 40f;
                        if(!Collision.CanHit(boss.position,boss.width,boss.height,player.position,player.width,player.height))
                        {
                            projType = ModContent.ProjectileType<BrimstoneHellblast>();
                            projDMG = boss.GetProjectileDamage(projType);
                            Projectile.NewProjectile(boss.GetSource_FromAI(), boss.Center + laserOffset, laserVel, projType, projDMG, 0f, Main.myPlayer, player.position.X, player.position.Y);
                        }
                        else
                        {
                            float Ai0 = projType == ModContent.ProjectileType<BrimstoneHellblast>() ? 1f : player.position.X;
                            float Ai1 = projType == ModContent.ProjectileType<BrimstoneHellblast>() ? 0f : player.position.Y;
                            Projectile.NewProjectile(boss.GetSource_FromAI(), boss.Center + laserOffset, laserVel, projType, projDMG, 0f, Main.myPlayer, Ai0, Ai1);
                        }
                    }
                }
                boss.ai[2] += 1f;
                float nextPhase = 180f;
                if(boss.ai[2] >= nextPhase)
                {   
                    //在第一波兄弟发起之前 或 兄弟在场时, 旧灾不会使出冲刺
                    //附:其实我很好奇旧灾跟兄弟一起冲刺的场景
                    //改了，现在兄弟在场也能发起冲刺, 开杀!
                    if(boss.life >= boss.lifeMax * 0.7f) 
                       boss.ai[1] = 0f;
                    else
                       //旧灾发起冲刺的标记
                       //附2: 可能会出现旧灾与他的4个兄弟一起撞人情况
                       //但是我们模组非常强势，应该没问题？
                       boss.ai[1] = 2f; 
                    

                    //重新初始化这个timer
                    boss.localAI[0] = 1f; 
                    boss.netUpdate = true;
                }
            }
            #endregion
            #region 冲刺
            else if (boss.ai[1] == 2f)
            {
                //设置冲刺伤害
                boss.damage = boss.defDamage;
                //转角
                boss.rotation = rot;
                //冲刺速度?
                float chargeSpeed = ifDeath? 40f : 30f;
                Vector2 newVec = Vector2.Normalize(player.oldPosition + player.oldVelocity - boss.Center);
                chargeSpeed += 0.5f + Main.rand.NextFloat(0.4f, 0.8f); 
                boss.velocity = newVec * chargeSpeed;

                //冲刺完毕后, set3f, 普灾反向在冲刺一次
                boss.ai[1] = 3f;
                boss.netUpdate = true;
            }
            else if (boss.ai[1] == 3f)
            {
                //设置伤害
                boss.damage = boss.defDamage;
                boss.ai[2] += 1f; //计时器
                //旧灾再次冲刺
                float chargeTime = ifDeath ? 120f : 120f;
                if(boss.ai[2] >= chargeTime)
                {
                    boss.velocity *= 0.9f;
                    if(boss.velocity.X > -0.1 && boss.velocity.X < 0.1)
                       boss.velocity.X = 0f;
                    if(boss.velocity.Y > -0.1 && boss.velocity.Y < 0.1)
                       boss.velocity.Y = 0f;
                }
                else
                {
                    boss.rotation = (float)Math.Atan2(boss.velocity.Y, boss.velocity.X) - MathHelper.PiOver2;
                }

                if(boss.ai[2] >= chargeTime + 10f)
                {
                    boss.ai[2] = 0f;
                    boss.ai[3] += 1f;
                    boss.rotation = rot;
                    boss.netUpdate = true;
                    if(boss.ai[3] >=2f)
                    {
                        boss.TargetClosest();
                        boss.ai[1] = 0f; //重新开始发射火球
                        boss.ai[3] = 0f;
                        return;
                    }
                    boss.ai[1] = 4f;
                }
            }
            else
            {
                boss.ai[2] += 1f;
                if(boss.ai[2] >= 30f)
                {
                    boss.ai[1] = 2f;
                    boss.ai[0] = 0f;
                    boss.localAI[0] = 0f;
                    boss.netUpdate = true;
                }
            }
            #endregion  
            #region 兄弟重生, 这一过程经历3次
            //70%, 40%, 10%重生一次
            if(boss.life > 0)
            {
                int getPhaseHP = (int)(boss.lifeMax * 0.3f);
                if(boss.life + getPhaseHP < cign.BossNewAI[0])
                {
                    //给予生成的一组兄弟一定的初始值来保证其不会与另一组兄弟同时冲刺
                    float Ai0 = 5f;
                    cign.BossNewAI[0] = boss.life; //刷新一次血量
                    if(cign.BossNewAI[0] < boss.lifeMax * 0.1) //10%
                    {
                        if(Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            /*生成四个兄弟*/
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X + boss.width, (int)boss.Center.Y + boss.height, ModContent.NPCType<CataclysmReborn>(),   boss.whoAmI);
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X + boss.width, (int)boss.Center.Y - boss.height, ModContent.NPCType<CataclysmReborn>(),   boss.whoAmI, Ai0);
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X - boss.width, (int)boss.Center.Y + boss.height, ModContent.NPCType<CatastropheReborn>(), boss.whoAmI);
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X - boss.width, (int)boss.Center.Y - boss.height, ModContent.NPCType<CatastropheReborn>(), boss.whoAmI, Ai0);
                        }
                    }
                    else if(cign.BossNewAI[0] <= boss.lifeMax * 0.4f) //40%
                    {
                        if(Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X + boss.width, (int)boss.Center.Y + boss.height, ModContent.NPCType<CataclysmReborn>(),   boss.whoAmI);
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X + boss.width, (int)boss.Center.Y - boss.height, ModContent.NPCType<CataclysmReborn>(),   boss.whoAmI, Ai0);
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X - boss.width, (int)boss.Center.Y + boss.height, ModContent.NPCType<CatastropheReborn>(), boss.whoAmI);
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X - boss.width, (int)boss.Center.Y - boss.height, ModContent.NPCType<CatastropheReborn>(), boss.whoAmI, Ai0);
                            
                        }
                        /*与上方相同的代码*/
                    }
                    else if(cign.BossNewAI[0] <= boss.lifeMax * 0.7f) //70%
                    {
                        if(Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X + boss.width, (int)boss.Center.Y + boss.height, ModContent.NPCType<CataclysmReborn>(),   boss.whoAmI);
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X + boss.width, (int)boss.Center.Y - boss.height, ModContent.NPCType<CataclysmReborn>(),   boss.whoAmI, Ai0);
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X - boss.width, (int)boss.Center.Y + boss.height, ModContent.NPCType<CatastropheReborn>(), boss.whoAmI);
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X - boss.width, (int)boss.Center.Y - boss.height, ModContent.NPCType<CatastropheReborn>(), boss.whoAmI, Ai0);
                        }
                        //上同
                    }
                }
            }
            //兄弟重生时, 旧灾获得极高的防御力
            int calCloneDefense = boss.defDefense; //存储基本防御力
            calCloneDefense += (CIGlobalNPC.CatalysmCloneWhoAmI     != -1 && Main.npc[CIGlobalNPC.CatalysmCloneWhoAmI].active)?    50 : 0; //我也忘了加多少了，反正亵渎死球了+200防御力
            calCloneDefense += (CIGlobalNPC.CatastropheCloneWhoAmI  != -1 && Main.npc[CIGlobalNPC.CatastropheCloneWhoAmI].active)? 50 : 0;
            //谁活着, 都行
            if((CIGlobalNPC.CatalysmCloneWhoAmI != -1 && Main.npc[CIGlobalNPC.CatalysmCloneWhoAmI].active) || (CIGlobalNPC.CatastropheCloneWhoAmI  != -1 && Main.npc[CIGlobalNPC.CatastropheCloneWhoAmI].active))
                ifBrothers = true;
            boss.defense = ifBrothers ? boss.defDefense + calCloneDefense : boss.defDefense;
            //兄弟在场时旧灾不会被锁定
            boss.chaseable = !ifBrothers;
            #endregion
            //按理来说，不出意外的话我们只需要这样就能完成一个普灾AI了，但是实际情况？还得再看看细节上的问题
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