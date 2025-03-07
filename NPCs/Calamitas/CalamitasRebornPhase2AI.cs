using System;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.NPCs.CalClone;
using CalamityMod.Projectiles.Boss;
using CalamityMod.World;
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
   
        public static void CalamitasRebornAI(NPC boss, Mod mod)
        {
            #region 初始化
            //Scarlet:这是一个副本文档，用于重新整顿普灾AI
            //大部分的编码从灾厄现版本转移过来
            CIGlobalNPC cign = boss.CalamityInheritance();

            //使普灾[下称旧灾]发光, 这个会一直持续下去
            AddRedLight(boss);
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
            if(cign.BossNewAI[0] == 0f && boss.life > 0)
               cign.BossNewAI[0] = boss.lifeMax;

            //给普灾找到一个target, 也就是玩家
            if(boss.target < 0 || boss.target == Main.maxPlayers || Main.player[boss.target].dead || !Main.player[boss.target].active)
                boss.TargetClosest();

            //将这个玩家取出来用
            Player player = Main.player[boss.target];
            CIGlobalNPC.CalamitasCloneWhoAmIP2 = boss.whoAmI;
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
                    boss.localAI[1] += 1f; //依旧, localAI[1]作为一个计时器
                    if(!ifBrothers)
                    boss.localAI[1] += 2f * (1f - lifePercent); //兄弟不在场的时候, 计时器转的更快
                }
                if(boss.localAI[1] >= (ifBrothers?120f:90f))//计时器符合这两个值?符合就开始发射火球
                {
                    boss.localAI[1] = 0f;
                    float projVel = 15f;
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
            #region 在侧身位置发射激光
            else if (boss.ai[1] == 1f)
            {
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    boss.localAI[1] += 1f; //计时器
                    if(!ifBrothers)
                    boss.localAI[1] += 1f;

                    // 发射激光
                    if(boss.localAI[1] >= (ifBrothers? 75f :45f) && Collision.CanHit(boss.position,boss.width,boss.height,player.position,player.width,player.height))
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
            #region 冲刺
            else if (boss.ai[1] == 2f)
            {
                //设置冲刺伤害
                boss.damage = boss.defDamage;
                //转角
                boss.rotation = rot;
                //冲刺速度?亵渎后提升四倍
                float chargeSpeed = ifDeath? 50f : 40f;
                chargeSpeed = ifProviDead ? chargeSpeed + 5f*4 : chargeSpeed;
                Vector2 newVec = Vector2.Normalize(player.Center + player.velocity * 20f - boss.Center);
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
                //旧灾再次发起冲刺的时间
                float chargeTime = ifDeath ? 50f : 70f;
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
                    cign.BossNewAI[0] = boss.life; //刷新一次血量
                    if(cign.BossNewAI[0] < boss.lifeMax * 0.1) //10%
                    {
                        if(Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            /*生成四个兄弟*/
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X, (int)boss.Center.Y + boss.height, ModContent.NPCType<CataclysmReborn>(),   boss.whoAmI);
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X, (int)boss.Center.Y + boss.height, ModContent.NPCType<CataclysmReborn>(),   boss.whoAmI);
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X, (int)boss.Center.Y + boss.height, ModContent.NPCType<CatastropheReborn>(), boss.whoAmI);
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X, (int)boss.Center.Y + boss.height, ModContent.NPCType<CatastropheReborn>(), boss.whoAmI);
                        }
                        // string key = "这里需要生成兄弟的提示文本";
                        Color textColor = Color.Orange;
                    }
                    else if(cign.BossNewAI[0] <= boss.lifeMax * 0.4f) //40%
                    {
                        if(Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X, (int)boss.Center.Y + boss.height, ModContent.NPCType<CataclysmReborn>(),   boss.whoAmI);
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X, (int)boss.Center.Y + boss.height, ModContent.NPCType<CataclysmReborn>(),   boss.whoAmI);
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X, (int)boss.Center.Y + boss.height, ModContent.NPCType<CatastropheReborn>(), boss.whoAmI);
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X, (int)boss.Center.Y + boss.height, ModContent.NPCType<CatastropheReborn>(), boss.whoAmI);
                        }
                        /*与上方相同的代码*/
                    }
                    else if(cign.BossNewAI[0] <= boss.lifeMax * 0.7f) //70%
                    {
                        if(Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X, (int)boss.Center.Y + boss.height, ModContent.NPCType<CataclysmReborn>(),   boss.whoAmI);
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X, (int)boss.Center.Y + boss.height, ModContent.NPCType<CataclysmReborn>(),   boss.whoAmI);
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X, (int)boss.Center.Y + boss.height, ModContent.NPCType<CatastropheReborn>(), boss.whoAmI);
                            NPC.NewNPC(boss.GetSource_FromAI(), (int)boss.Center.X, (int)boss.Center.Y + boss.height, ModContent.NPCType<CatastropheReborn>(), boss.whoAmI);
                        }
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
            //兄弟在场时旧灾不会被锁定
            boss.chaseable = !ifBrothers;
            #endregion
            //按理来说，不出意外的话我们只需要这样就能完成一个普灾AI了，但是实际情况？还得再看看细节上的问题
        }
        public static void CataclysmRebornAI(NPC brother, Mod mod)
        {
            #region 初始化
            CIGlobalNPC cign =brother.CalamityInheritance();
            if(CIGlobalNPC.CalamitasCloneWhoAmIP2 < 0f || !Main.npc[CIGlobalNPC.CalamitasCloneWhoAmIP2].active)
            {
                //普灾不在场直接干掉兄弟
                if(Main.netMode!=NetmodeID.MultiplayerClient)
                   brother.StrikeInstantKill();
                return;
            }
            //get兄弟的血量百分比
            float lifePercent = brother.life / (float)brother.lifeMax;
            CIGlobalNPC.CatalysmCloneWhoAmI = brother.whoAmI;
            //发光, 总之就是发光
            AddRedLight(brother);
            //判定难度, 同样, 只有死亡模式的差分
            bool ifDeath = CalamityWorld.death || CalamityWorld.revenge || Main.expertMode;

            //给他一个target
            if(brother.target < 0 || brother.target == Main.maxPlayers || Main.player[brother.target].dead || !Main.player[brother.target].active)
               brother.TargetClosest();

            //get这个target，也就是player本身
            Player player = Main.player[brother.target];
            //老操作, 给这家伙找到他的目标
            float broToTarDistX = brother.position.X + (brother.width/2) - player.position.X - (player.width/2);
            float broToTarDistY = brother.position.Y + brother.height - 59f - player.position.Y - (player.height / 2);
            float broRot = (float)Math.Atan2(broToTarDistX, broToTarDistY) + MathHelper.PiOver2;
            //保转角
            BrothersKeepRotation(brother, broRot, 0.15f);
            #endregion
            #region 兄弟脱战
            //封装
            BrotherDespawn(brother, player);
            #endregion
            #region 兄弟行为
            if (brother.ai[1] == 0f)
            {
                #region 顺滑移动
                float broProjSpeedMax = 5f;
                float broProjAccel  = 0.1f;
                //射弹朝向
                int broProjAttackDir = 1; 
                if (brother.position.X + (brother.width / 2) < player.position.X + player.width)
                    broProjAttackDir = -1;
                //获取射弹与玩家的距离
                Vector2 projVec = new(brother.position.X + brother.width * 0.5f, brother.position.Y + brother.height * 0.5f);
                float projTarX = player.position.X + (player.width / 2) + (broProjAttackDir * 100) - projVec.X;
                float projTarY = player.position.Y + (player.height/ 2) -  projVec.Y;
                float projTarDist = (float)Math.Sqrt(projTarX * projTarX + projTarY * projTarY);
                if(ifDeath) //是否为死亡模式?
                {
                    float speedUp = 0.5f;
                    if (projTarDist > 300f && projTarDist < 900f) //射弹是否与玩家距离过远(不过感觉更应该是是否兄弟离玩家太远)
                        broProjSpeedMax += speedUp + 0.05f; 
                        //原灾在这里整了10个if来复线加速度效果，这里直接改成普通的加速度了
                }
                //让boss的移动变得顺滑，这里类似于回旋镖的那个AI
                projTarDist = broProjSpeedMax / projTarDist;
                projTarX *= projTarDist;
                projTarY *= projTarDist;
                //封装
                BrothersSmoothMove(brother, projTarX, projTarY, broProjAccel);
                #endregion
                #region 兄弟发射弹幕
                //这里才正式开始发射弹幕
                brother.ai[2] += 1f; //计时器
                if (brother.ai[2] >= 180f) //180f, 大约三秒
                {
                    brother.ai[1] = 1f;
                    brother.ai[2] = 0f;
                    brother.target = 255; //?
                    brother.netUpdate = true;
                }
                //移除喷火间隔
                //喷火的兄弟，不过我记得他这个以前是纯粒子，有复现的方法吗？
                if(Collision.CanHit(brother.position, brother.width, brother.height, player.position, player.width, player.height))
                {
                    brother.localAI[2] += 1f; //计时器
                    //延迟播放喷火音
                    if(brother.localAI[2] > 22f)
                    {
                        brother.localAI[2] = 0f;
                        SoundEngine.PlaySound(CISoundID.SoundFlamethrower, brother.Center); 
                    }
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        brother.localAI[1] += 3f; //计时器
                        if(ifDeath) brother.localAI[1] += 1f;
                        if(brother.localAI[1] > 12f)
                        {
                            brother.localAI[1] = 0f; //喷火
                            //另一个兄弟在场时，喷火的速度会慢一点
                            float projSpeed = NPC.AnyNPCs(ModContent.NPCType<CatastropheReborn>())? 4f : 6f;
                            int projType = ModContent.ProjectileType<BrimstoneFire>(); //依旧是占位符
                            int projDMG = brother.GetProjectileDamage(projType);
                            projVec = new Vector2(brother.position.X + brother.width / 2, brother.position.Y + brother.height / 2);
                            projTarX = player.position.X + (player.width / 2) - projVec.X;
                            projTarY = player.position.Y + (player.height/ 2) - projVec.Y;
                            projTarDist = (float)Math.Sqrt(projTarX * projTarY+ projTarY * projTarY);
                            projTarDist = projSpeed / projTarDist;
                            projTarX *= projTarDist;
                            projTarY *= projTarDist;
                            projTarX += brother.velocity.X/2;
                            projTarY += brother.velocity.Y/2;
                            projVec.X -= projTarX;
                            projVec.Y -= projTarY;
                            Projectile.NewProjectile(brother.GetSource_FromAI(), projVec.X, projVec.Y, projTarX, projTarY, projType, projDMG, 0f, Main.myPlayer, 0f, 0f);   
                        }
                    }
                }
                #endregion
            }
            #region 兄弟冲刺
            else 
            {
                if (brother.ai[1] == 1f)
                {
                    brother.damage = brother.defDamage; 
                    SoundEngine.PlaySound(SoundID.Roar, brother.Center);//发起冲刺的时候嘶吼
                    brother.rotation = broRot;
                    //这下面都是冲刺的逻辑
                    float chargeSpeed = 18f;
                    if(ifDeath) chargeSpeed += 4f;
                    Vector2 chargeCenter = brother.Center;
                    float chargeTarDistX = player.Center.X - chargeCenter.X;
                    float chargeTarDistY = player.Center.Y - chargeCenter.Y;
                    float chargeTarDistReal = (float)Math.Sqrt(chargeTarDistX * chargeTarDistX + chargeTarDistY * chargeTarDistY);
                    chargeTarDistReal = chargeSpeed / chargeTarDistReal;
                    brother.velocity = new Vector2(chargeTarDistX * chargeTarDistReal, chargeTarDistY * chargeTarDistReal);
                    //ai[1] = 2f用于减速
                    brother.ai[1] = 2f;
                    return;
                }
                //我杀了你吗吗，能不能别造史了
                if (brother.ai[1] == 2f)
                {
                    brother.damage = brother.defDamage;

                    brother.ai[2] += ifDeath? 2f : 1.25f;
                    if(brother.ai[2] >= 75f)
                    {
                        BrotherChargeSlowDown(brother);
                    }
                    else brother.rotation = (float)Math.Atan2(brother.velocity.Y, brother.velocity.X) - MathHelper.PiOver2;
                    if (brother.ai[2] >= 105f) //冲刺结束，切换AI
                    {
                        brother.ai[3] += 1f;
                        brother.ai[2] = 0f;
                        brother.target = 255;
                        brother.rotation = broRot;
                        if (brother.ai[3] >= 3f)
                        {
                            brother.ai[1] = 0f;
                            brother.ai[3] = 0f;
                            return;
                        }
                        brother.ai[1] = 1f;
                    }
                }
                
            }
            #endregion
            #endregion
        }
        public static void CatastropheRebornAI(NPC brother, Mod mod)
        {
            //复制粘贴兄弟们
            #region 初始化
            CIGlobalNPC cign = brother.CalamityInheritance();
            if(CIGlobalNPC.CalamitasCloneWhoAmIP2 < 0f || !Main.npc[CIGlobalNPC.CalamitasCloneWhoAmIP2].active)
            {
                //普灾不在场直接干掉兄弟
                if(Main.netMode!=NetmodeID.MultiplayerClient)
                   brother.StrikeInstantKill();
                return;
            }
            //get兄弟的血量百分比
            float lifePercent = brother.life / (float)brother.lifeMax;
            CIGlobalNPC.CatastropheCloneWhoAmI = brother.whoAmI;
            //发光, 总之就是发光
            AddRedLight(brother);
            //判定难度, 同样, 只有死亡模式的差分
            bool ifDeath = CalamityWorld.death || CalamityWorld.revenge || Main.expertMode;

            //给他一个target
            if(brother.target < 0 || brother.target == Main.maxPlayers || Main.player[brother.target].dead || !Main.player[brother.target].active)
               brother.TargetClosest();

            //get这个target，也就是player本身
            Player player = Main.player[brother.target];
            //老操作, 给这家伙找到他的目标
            float broToTarDistX = brother.position.X + (brother.width/2) - player.position.X - (player.width/2);
            float broToTarDistY = brother.position.Y + brother.height - 59f - player.position.Y - (player.height / 2);
            float broRot = (float)Math.Atan2(broToTarDistX, broToTarDistY) + MathHelper.PiOver2;
            //保转角
            BrothersKeepRotation(brother, broRot, 0.15f);
            #endregion
            #region 兄弟脱战
            //封装
            BrotherDespawn(brother, player);
            #endregion
            #region 兄弟行为
            if (brother.ai[1] == 0f)
            {
                #region 发射射弹
                //大部分都是复制的, 复制的上面的
                float projMaxSpeed = 4.5f;
                float projAccel = 4.5f;
                int projDir = 1;
                if (brother.Center.X < player.Center.X)
                    projDir = -1;
                
                Vector2 projCenter = brother.Center;
                float projTarX = player.Center.X + (projDir * 180) - projCenter.X;
                float projTarY = player.Center.Y - projCenter.Y;
                float projTarDist = (float)Math.Sqrt(projTarX * projTarX + projTarY * projTarY);
                if(ifDeath)
                {
                    float speedUp = 0.5f;
                    if (projTarDist > 300f && projTarDist < 900f) //射弹是否与玩家距离过远(不过感觉更应该是是否兄弟离玩家太远)
                        projMaxSpeed+= speedUp + 0.05f; 
                        //原灾在这里整了10个if来复线加速度效果，这里直接改成普通的加速度了
                }   
                projTarDist = projMaxSpeed / projTarDist;
                projTarX *= projTarDist;
                projTarY *= projTarDist;
                //顺滑移动
                BrothersSmoothMove(brother, projTarX, projTarY, projAccel);
                brother.ai[2] += 1f;
                if (brother.ai[2] >= 90f)
                {
                    brother.ai[1] = 1f;
                    brother.ai[2] = 0f;
                    brother.target = 255;
                    brother.netUpdate = true;
                }
                if(Collision.CanHit(brother.position, brother.width, brother.height, player.position, player.width, player.height))
                {
                    brother.localAI[2] += 1f;
                    if (brother.localAI[2] > 36f) 
                    {
                        brother.localAI[2] = 0f;
                        SoundEngine.PlaySound(CISoundID.SoundFlamethrower, brother.Center);
                    }
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        //初始化完成开始发射弹幕
                        brother.localAI[1] += ifDeath? 1.5f : 1f;
                        if(brother.localAI[1] > 50f)
                        {
                            brother.localAI[1] = 0f;
                            float projSpeed = ifDeath ? 14f : 12f;
                            int projType = ModContent.ProjectileType<BrimstoneBall>();
                            int projDMG = brother.GetProjectileDamage(projType);
                            projCenter = new Vector2(brother.position.X + brother.width / 2, brother.position.Y + brother.height / 2);
                            projTarX = player.position.X + (player.width / 2) - projCenter.X;
                            projTarY = player.position.Y + (player.height/ 2) - projCenter.Y;
                            projTarDist = (float)Math.Sqrt(projTarX * projTarX + projTarY * projTarY);
                            projTarDist = projSpeed / projTarDist;
                            projTarX *= projTarDist;
                            projTarY *= projTarDist;
                            Vector2 projTarVel = new(projTarX, projTarY);
                            projTarX += brother.Center.X * 0.5f;
                            projTarY += brother.Center.Y * 0.5f;
                            Vector2 projCenterVel = new(projCenter.X - projTarX, projCenter.Y - projTarY);
                            Projectile.NewProjectile(brother.GetSource_FromAI(), projCenterVel, projTarVel, projType, projDMG, 0f, Main.myPlayer, 0f, 0f);
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region 冲刺AI
                if (brother.ai[1] == 1f)
                {
                    brother.damage = brother.defDamage;
                    SoundEngine.PlaySound(SoundID.Roar, brother.Center);
                    brother.rotation = broRot;

                    float chargeSpeed = (NPC.AnyNPCs(ModContent.NPCType<Cataclysm>()) ? 12f : 16f) + (ifDeath ? 4f : 0f);

                    Vector2 chargeCenter = new(brother.position.X + brother.width/2, brother.position.Y + brother.height/2);
                    float chargeTarX = player.position.X + (player.width / 2) - chargeCenter.X;
                    float chargeTarY = player.position.Y + (player.width / 2) - chargeCenter.Y;
                    float chargeDist = (float)Math.Sqrt(chargeTarX * chargeTarX + chargeTarY * chargeTarY);
                    brother.velocity.X = chargeTarX * chargeDist;
                    brother.velocity.Y = chargeTarY * chargeDist;
                    brother.ai[1] = 2f;
                    return;
                }
                if (brother.ai[1] == 2f)
                {
                    brother.damage = brother.defDamage;

                    brother.ai[2] += ifDeath? 2f : 1f;
                    if(brother.ai[2] >= 60f) 
                    {
                        BrotherChargeSlowDown(brother);
                    }
                    else brother.rotation = (float)Math.Atan2(brother.velocity.Y, brother.velocity.X) - MathHelper.PiOver2;

                    if (brother.ai[2] >= 90f)
                    {
                        brother.ai[3] += 1f;
                        brother.ai[2] = 0f;
                        brother.rotation = broRot;
                        if (brother.ai[3] >= 4f)
                        {
                            brother.ai[1] = 0f;
                            brother.ai[3] = 0f;
                            return;
                        }
                        brother.ai[1] = 1f;
                    }
                }
                #endregion
            }
            #endregion
        }
        /// <summary>
        /// 太史山了他这个代码，我必须得封装一下不然就是复制粘贴大赛了,这个用来保兄弟的转角不会出问题的
        /// </summary>
        /// <param name="brother">兄弟</param>
        /// <param name="broRot">需要的转角</param>
        /// <param name="rotSpeed">需要的转角速度</param>
        public static void BrothersKeepRotation(NPC brother, float broRot, float rotSpeed)
        {
            if(broRot < 0f)
               broRot += MathHelper.TwoPi;
            else if ( broRot > MathHelper.TwoPi)
               broRot -= MathHelper.TwoPi;
            
            float broRotSpeed = rotSpeed;
            if(brother.rotation < broRot)
            {
                if((broRot - brother.rotation) > MathHelper.Pi)
                    brother.rotation -= broRotSpeed;
                else
                    brother.rotation += broRotSpeed;
            }
            else if(brother.rotation > broRot)
            {
                if((brother.rotation - broRot) > MathHelper.Pi)
                    brother.rotation += broRotSpeed;
                else
                    brother.rotation -= broRotSpeed;
            }

            if (brother.rotation < broRot - broRotSpeed && brother.rotation < broRot + broRotSpeed)
                brother.rotation = broRot;
            if (brother.rotation < 0f)
                brother.rotation += MathHelper.TwoPi; 
            else if(brother.rotation > MathHelper.TwoPi)
                brother.rotation -= MathHelper.TwoPi;
            if(brother.rotation > broRot - broRotSpeed && brother.rotation < broRot + broRotSpeed)
                brother.rotation = broRot;
        }
        /// <summary>
        /// 太史了这个代码，封装了。这个是保证兄弟跟随玩家能顺滑移动，类似回旋镖的ai
        /// </summary>
        /// <param name="brother">兄弟</param>
        /// <param name="projTarX">兄弟与目标的水平距离</param>
        /// <param name="projTarY">兄弟与目标的垂直距离</param>
        /// <param name="broProjAccel">兄弟加速度</param>
        public static void BrothersSmoothMove(NPC brother, float projTarX, float projTarY, float broProjAccel)
        {
            if (brother.velocity.X < projTarX)
            {
                brother.velocity.X += broProjAccel;
                if (brother.velocity.X < 0f && projTarX > 0f)
                    brother.velocity.X += broProjAccel;
            }
            else if (brother.velocity.X > projTarX)
            {
                brother.velocity.X -= broProjAccel;
                if (brother.velocity.X > 0f && projTarX < 0f)
                    brother.velocity.X -= broProjAccel;
            }
            if (brother.velocity.Y < projTarY)
            {
                brother.velocity.Y += broProjAccel;
                if (brother.velocity.Y < 0f && projTarY > 0f)
                    brother.velocity.Y += broProjAccel;
            }
            else if (brother.velocity.Y > projTarY)
            {
                brother.velocity.Y -= broProjAccel;
                if (brother.velocity.Y > 0f && projTarY < 0f)
                    brother.velocity.Y -= broProjAccel;
            }

        }
        /// <summary>
        /// 太史了这个代码，封装了. 这个使兄弟能脱战
        /// </summary>
        /// <param name="brother">兄弟</param>
        /// <param name="player">玩家</param>
        public static void BrotherDespawn(NPC brother, Player player)
        {
            if(!player.active || player.dead)
            {
                brother.TargetClosest(false);
                player = Main.player[brother.target];
                if(!player.active || player.dead)
                {
                    if (brother.velocity.Y > 3f)
                        brother.velocity.Y = 3f;
                    brother.velocity.Y -= 0.1f;
                    if (brother.velocity.Y < -12f)
                        brother.velocity.Y = -12f;
                    
                    if (brother.timeLeft > 60)
                        brother.timeLeft = 60;
                    
                    if (brother.ai[1] != 0f)
                    {
                        brother.ai[1] = 0f;
                        brother.ai[2] = 0f;
                        brother.ai[3] = 0f;
                        brother.netUpdate = true;
                    }
                    return;
                }
            }
        }
        /// <summary>
        /// 你他妈，别造史了。这个用来兄弟冲刺后的减速
        /// </summary>
        /// <param name="brother">兄弟 </param>
        public static void BrotherChargeSlowDown(NPC brother)
        {
            brother.velocity.X *= 0.93f;
            brother.velocity.Y *= 0.93f;
            if (brother.velocity.X > -0.1 && brother.velocity.X < 0.1)
                brother.velocity.X = 0f;
            if (brother.velocity.Y > -0.1 && brother.velocity.Y < 0.1)
                brother.velocity.Y = 0f;
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