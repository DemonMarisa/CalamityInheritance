using System;
using System.Data;
using System.Numerics;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Projectiles.Summon;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.NPCs.Calamitas
{
    public static class CalCloneCP
    {
        /*
        *一些微弱的解释性文案:
        *ai数组中ai[0] 暂时不清楚实际作用，搁置, ai[1]目前存放悬浮(0f)与水平(1f)的ai, ai[2]目前用作计时器转sub阶段的作用
        *考虑到旧灾的ai逻辑远比灾厄简单(灾厄怎么说都有一个弹幕炼狱), 可能不需要ai[4]去存ai状态, 最高用到ai[3]可能就够实现普灾的逻辑了
        *
        */
        public static void CalCloneLegacyAICP(NPC boss, Mod mod)
        {
            #region 初始化
            //Scarlet:这是一个副本文档，用于重新整顿普灾AI
            //大部分的编码从灾厄现版本转移过来
            CIGlobalNPC cign = boss.CalamityInheritance();

            //使普灾[下称旧灾]发光, 这个会一直持续下去
            float lightingPosX = (boss.position.X + (boss.width/2))/16;
            float lightingPosY = (boss.position.Y + (boss.width/2))/16;
            Lighting.AddLight(new Vector2(lightingPosX, lightingPosY), 1f, 0f, 0f); //(255,0,0)
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
            bool ifProviDead = CalamityConditions.DownedProvidence.IsMet();
            
            //将普灾最大血量先存进去
            if(cign.BossNewAI[0] == 0f && boss.life >0)
               cign.BossNewAI[0] = boss.lifeMax;

            //给普灾找到一个target, 也就是玩家
            if(boss.target < 0 || boss.target == Main.maxPlayers || Main.player[boss.target].dead || !Main.player[boss.target].active)
                boss.TargetClosest();

            //将这个玩家取出来用
            Player player = Main.player[boss.target];
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
            #region 旧灾, 与他的目标
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
            baseSpeed   = ifProviDead? baseSpeed    * 2f : baseSpeed;
            baseAccele  = ifProviDead? baseAccele   * 2f : baseAccele;
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
            #region 悬浮于头上,尝试发射火球
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
                    boss.localAI[1] += 1f; //依旧, localAI[1]作为一个计时器
                    if(!ifBrothers)
                    boss.localAI[1] += 2f * (1f - lifePercent); //兄弟不在场的时候, 计时器转的更快
                }
                if(boss.localAI[1] >= (ifBrothers?180f:120f))//计时器符合这两个值?符合就开始发射火球
                {
                    boss.localAI[1] = 0f;
                    float projVel = 14f;
                    //灾厄在这是用激怒作为差分，此处则采用是否击败了亵渎

                    int projType= ModContent.ProjectileType<BrimstoneHellfireball>(); //TODO4:使用旧版火球
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
            #region 在侧身位置, 发射与激光
            else if (boss.ai[1] == 1f)
            {
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    boss.localAI[1] += 1f; //计时器
                    if(!ifBrothers)
                    boss.localAI[1] += 1f;

                    // 发射激光
                    if(boss.localAI[1] >= (ifBrothers? 75f :50f) && Collision.CanHit(boss.position,boss.width,boss.height,player.position,player.width,player.height))
                    {
                        boss.localAI[1] = 0f;//重置
                        float projVel = ifProviDead ? 36f : 12.5f;
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
                       boss.ai[1] = 2f; //旧灾冲刺
                    //附2: 可能会出现旧灾与他的4个兄弟一起撞人情况
                    //但是我们模组非常强势，应该没问题？
                    
                    
                    boss.localAI[0] = 1f; //重新初始化这个timer
                    boss.netUpdate = true;
                }
            }
            #endregion
            #region 冲刺AI
            else if (boss.ai[1] == 2f)
            {
                //设置冲刺伤害
                boss.damage = boss.defDamage;
                //转角
                boss.rotation = rot;
                //冲刺速度?亵渎后提升四倍
                float chargeSpeed = ifDeath? 30f : 20f;
                chargeSpeed = ifProviDead ? chargeSpeed + 5f*4 : chargeSpeed;
                Vector2 newVec = Vector2.Normalize(player.Center + player.velocity * 20f);
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
                //旧灾发起反冲的时间
                float secondCharge = ifDeath ? 35f : 45f;
                if(boss.ai[2] >= secondCharge)
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

                if(boss.ai[2] >= secondCharge + 10f)
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
                    cign.BossNewAI[0] = boss.life; //刷新一次血量
                    if(cign.BossNewAI[0] < boss.lifeMax * 0.1) //10%
                    {
                        if(Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            /*生成四个兄弟*/
                        }
                        // string key = "这里需要生成兄弟的提示文本";
                        Color textColor = Color.Orange;
                    }
                    else if(cign.BossNewAI[0] <= boss.lifeMax * 0.4f) //40%
                    {
                        /*与上方相同的代码*/
                    }
                    else if(cign.BossNewAI[0] <= boss.lifeMax * 0.7f) //70%
                    {
                        //上同
                    }
                }
            }
            //兄弟重生时, 旧灾获得极高的防御力
            int calCloneDefense = boss.defDefense; //存储基本防御力
            calCloneDefense += (CIGlobalNPC.CatalysmCloneWhoAmI     != -1 && Main.npc[CIGlobalNPC.CatalysmCloneWhoAmI].active)?     (ifProviDead ? 200 : 50) : 0; //我也忘了加多少了，反正亵渎死球了+200防御力
            calCloneDefense += (CIGlobalNPC.CatastropheCloneWhoAmI  != -1 && Main.npc[CIGlobalNPC.CatastropheCloneWhoAmI].active)?  (ifProviDead ? 200 : 50) : 0;
            //谁活着, 都行
            if((CIGlobalNPC.CatalysmCloneWhoAmI != -1 && Main.npc[CIGlobalNPC.CatalysmCloneWhoAmI].active) || (CIGlobalNPC.CatastropheCloneWhoAmI  != -1 && Main.npc[CIGlobalNPC.CatastropheCloneWhoAmI].active))
                ifBrothers = true;
            boss.defense = ifBrothers ? boss.defDefense + calCloneDefense : boss.defDefense;
            #endregion
            
        }
        public static void CataclysmLegacyAI(NPC boss, Mod mod)
        {

        }
        public static void CatastropheLegacyAI(NPC boss, Mod mod)
        {

        }
        //粒子生成
        public static void SpawnDust()
        {
            
        }
    }
}