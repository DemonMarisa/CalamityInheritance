using System;
using CalamityInheritance.Content.Items;
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
    public static class CatastropheAI
    {        
        public static void ThisAI(NPC brother, Mod mod)
        {
//复制粘贴兄弟们
            #region 初始化
            CIGlobalNPC cign = brother.CIMod();
  
            if(CIGlobalNPC.ThisCalamitasRebornP2 < 0f || !Main.npc[CIGlobalNPC.ThisCalamitasRebornP2].active)
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
            CIFunction.SetGlow(brother, 1f, 0f, 0f);
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
            float rotSpeed = 0.15f;
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

            if (brother.rotation > broRot - broRotSpeed && brother.rotation < broRot + broRotSpeed)
                brother.rotation = broRot;
            if (brother.rotation < 0f)
                brother.rotation += MathHelper.TwoPi; 
            else if(brother.rotation > MathHelper.TwoPi)
                brother.rotation -= MathHelper.TwoPi;
            if(brother.rotation > broRot - broRotSpeed && brother.rotation < broRot + broRotSpeed)
                brother.rotation = broRot;
            #endregion
            #region 兄弟脱战
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
            #endregion
            #region 兄弟行为
            if (brother.ai[1] == 0f)
            {
                #region 发射射弹
                /********************************************重新解读*****************************
                *重新以自然语言对兄弟AI进行一次完整的解读.
                *首先，兄弟需要发射射弹，而发射射弹必须要有一个目标，因此射弹的目标, 即getProjTarget一定是玩家
                *其次，玩家是一直在动的，所以不可能取一个静态的坐标，这里的getProjTarget一定到最后是变成一个Vector的
                *
                *
                *
                *
                *
                *
                *
                *
                ********************************************************************************/
                
                //射弹的最高速度，4.5f
                float projMaxSpeed = 4.5f;
                //射弹的加速度，暂时取0.4f
                float projAccel = 0.4f; //projAccele 4.5f -> 0.4f
                //射弹的朝向。这里是固定的一个格式
                int projDir = 1;
                if (brother.Center.X < player.Center.X)
                    projDir = -1;
                //重点：projStartCenter:射弹发射的起始位置，即兄弟重心，这必须是一个不会停止移动的向量起点
                //获取的这个向量起点也是必须得恰好在兄弟的正中心的。总之，这是一个固定的方法。
                Vector2 projStartCenter = CIFunction.GetNpcCenter(brother);
                //projTarX: 获取玩家的水平位置
                float projTarX = player.Center.X + player.width * 0.5f + (projDir * 180) - projStartCenter.X;
                //projTarY: 获取
                float projTarY = player.Center.Y + player.height * 0.5f - projStartCenter.Y;
                float projTarDist = CIFunction.TryGetVectorMud(projTarX, projTarY);
                if(ifDeath)
                {
                    float speedUp = 0.5f;
                    if (projTarDist > 300f && projTarDist < 900f) //射弹是否与玩家距离过远(不过感觉更应该是是否兄弟离玩家太远)
                        projMaxSpeed+= speedUp + 0.05f; 
                        //原灾在这里整了10个if来复线加速度效果，这里直接改成普通的加速度了
                }
                //速度/距离 = 周期, 时间的倒数
                projTarDist = projMaxSpeed / projTarDist;
                //将X水平位置与时间倒数相乘？-> 新的水平速度
                //不过这也只是自然语言上的理解，如果有更好的解释的话会在这做出更新
                projTarX *= projTarDist;
                //同上
                projTarY *= projTarDist;
                #region 顺滑移动
                //下列都是为了保证兄弟能够跟上玩家做的
                //brother.velocity.X, npc的水平速度，projTarX玩家水平速度
                //小于这个值，加速。
                CIFunction.TryCatchTraget(brother, projTarX, projTarY, projAccel);
                /*
                if (brother.velocity.X < projTarX)
                {
                    brother.velocity.X += projAccel;
                    //速度小于0且玩家速度大于0，加速更快
                    if (brother.velocity.X < 0f && projTarX > 0f)
                        brother.velocity.X += projAccel;
                }
                //反之
                else if (brother.velocity.X > projTarX)
                {
                    brother.velocity.X -= projAccel;
                    if (brother.velocity.X > 0f && projTarX < 0f)
                        brother.velocity.X -= projAccel;
                }
                //同上
                if (brother.velocity.Y < projTarY)
                {
                    brother.velocity.Y += projAccel;
                    if (brother.velocity.Y < 0f && projTarY > 0f)
                        brother.velocity.Y += projAccel;
                }
                else if (brother.velocity.Y > projTarY)
                {
                    brother.velocity.Y -= projAccel;
                    if (brother.velocity.Y > 0f && projTarY < 0f)
                        brother.velocity.Y -= projAccel;
                }
                */
                #endregion
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
                            projStartCenter = new Vector2(brother.position.X + brother.width / 2, brother.position.Y + brother.height / 2);
                            projTarX = player.position.X + (player.width / 2) - projStartCenter.X;
                            projTarY = player.position.Y + (player.height/ 2) - projStartCenter.Y;
                            projTarDist = (float)Math.Sqrt(projTarX * projTarX + projTarY * projTarY);
                            projTarDist = projSpeed / projTarDist;
                            projTarX *= projTarDist;
                            projTarY *= projTarDist;
                            projTarX += brother.Center.X * 0.5f;
                            projTarY += brother.Center.Y * 0.5f;
                            projStartCenter.X -= projTarX;
                            projStartCenter.Y -= projTarY;
                            Projectile.NewProjectile(brother.GetSource_FromAI(), projStartCenter.X, projStartCenter.Y, projTarX, projTarY, projType, projDMG, 0f, Main.myPlayer, 0f, 0f);
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
                        brother.velocity.X *= 0.93f;
                        brother.velocity.Y *= 0.93f;
                        if (brother.velocity.X > -0.1 && brother.velocity.X < 0.1)
                            brother.velocity.X = 0f;
                        if (brother.velocity.Y > -0.1 && brother.velocity.Y < 0.1)
                            brother.velocity.Y = 0f;
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
    }
}