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

namespace CalamityInheritance.NPCs.Calamitas.Brothers
{
    public static class CatastropheAI
    {        
        public static void ThisAI(NPC brother, Mod mod)
        {
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
            /*
            // float broToTarDistX = brother.position.X + brother.width/2 - player.position.X - player.width/2;
            // float broToTarDistY = brother.position.Y + brother.height - 59f - player.position.Y - player.height / 2;
            // float broRot = (float)Math.Atan2(broToTarDistX, broToTarDistddY) + MathHelper.PiOver2;
            */
            //保转角
            float bDistX = brother.position.X + (brother.width / 2) - player.position.X - (player.width / 2);
            float bDistY = brother.position.Y + brother.height - 59f - player.position.Y - (player.height /2);
            float broRot = (float)Math.Atan2(bDistY, bDistX) + MathHelper.PiOver2;
            // BrothersKeepRotation(brother, broRot, 0.15f);
            //丢弃
            brother.rotation = BrothersGeneric.KeepAngle(brother, 0.15f, broRot); 
            #endregion
            #region 兄弟脱战
            BrothersGeneric.BrothersDespawns(player, brother);
            #endregion
            #region 兄弟行为
            if (brother.ai[1] == 0f)
            {
                #region 射弹
                float projMaxSpeed = 4.5f;
                float projAccel = 0.2f;
                //射弹的朝向。这里是固定的一个格式
                int projDir = 1;
                if (brother.Center.X < player.Center.X)
                    projDir = -1;
                //重点：projStartCenter:射弹发射的起始位置，即兄弟重心，这必须是一个不会停止移动的向量起点
                //获取的这个向量起点也是必须得恰好在兄弟的正中心的。总之，这是一个固定的方法。
                Vector2 projStartCenter = brother.Center;
                float projTarX = player.Center.X + projDir * 180 - projStartCenter.X;
                float projTarY = player.Center.Y - projStartCenter.Y;
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
                projTarX *= projTarDist;
                projTarY *= projTarDist;
                #region 顺滑移动
                CIFunction.TryCatchTraget(brother, projTarX, projTarY, projAccel);
                #endregion
                brother.ai[2] += 1f;
                //兄弟进入冲刺的ai时间给予一定的随机性。
                if (Main.rand.NextBool()) brother.ai[2] += 1f;
                if (brother.ai[2] >= 120f)
                {
                    brother.ai[1] = 1f;
                    brother.ai[2] = 0f;
                    brother.target = 255;
                    brother.netUpdate = true;
                }
                bool getFireDelay = brother.ai[2] > 120f;
                if(Collision.CanHit(brother.position, brother.width, brother.height, player.position, player.width, player.height) && getFireDelay)
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
                            BrothersShoot.JustShoot(brother, player, ModContent.ProjectileType<BrimstoneBall>(), 14f, brother.defDamage);
                            /*
                            float projSpeed = ifDeath ? 14f : 12f;
                            int projType = ModContent.ProjectileType<BrimstoneBall>();
                            int projDMG = brother.GetProjectileDamage(projType);
                            projStartCenter = new Vector2(brother.position.X + brother.width / 2, brother.position.Y + brother.height / 2);
                            projTarX = player.position.X + player.width / 2 - projStartCenter.X;
                            projTarY = player.position.Y + player.height/ 2 - projStartCenter.Y;
                            projTarDist = CIFunction.TryGetVectorMud(projTarX, projTarY);
                            projTarDist = projSpeed / projTarDist;
                            projTarX *= projTarDist;
                            projTarY *= projTarDist;
                            projTarX += brother.Center.X * 0.5f;
                            projTarY += brother.Center.Y * 0.5f;
                            projStartCenter.X -= projTarX;
                            projStartCenter.Y -= projTarY;
                            Projectile.NewProjectile(brother.GetSource_FromAI(), projStartCenter.X, projStartCenter.Y, projTarX, projTarY, projType, projDMG, 0f, Main.myPlayer, 0f, 0f);
                            */
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
                    //给冲刺速度一点微弱的随机性来使兄弟的冲刺有所差异
                    BrothersCharge.ChargeInit(brother, player, broRot, Main.rand.NextFloat(32f, 36f));
                    /*
                    float chargeSpeed = (NPC.AnyNPCs(ModContent.NPCType<CataclysmReborn>()) ? 12f : 16f) + (ifDeath ? 4f : 0f);
                    Vector2 chargeCenter = new(brother.position.X + brother.width/2, brother.position.Y + brother.height/2);
                    float chargeTarX = player.position.X + player.width / 2 - chargeCenter.X;
                    float chargeTarY = player.position.Y + player.width / 2 - chargeCenter.Y;
                    float chargeDist = CIFunction.TryGetVectorMud(chargeTarX, chargeTarY);
                    //忘记取倒数
                    chargeDist = chargeSpeed / chargeDist;
                    brother.velocity.X = chargeTarX * chargeDist;
                    brother.velocity.Y = chargeTarY * chargeDist;
                    */
                    brother.ai[1] = 2f;
                    return;
                }
                if (brother.ai[1] == 2f)
                {
                    brother.damage = brother.defDamage;

                    brother.ai[2] += 1f;
                    if(brother.ai[2] >= 120f) 
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