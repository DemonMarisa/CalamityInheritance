using System;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Dusts;
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
    public static class CataclysmRebornAI
    {        
        public static void ThisAI(NPC brother, Mod mod)
        {
            #region 初始化
            CIGlobalNPC cign =brother.CIMod();
            if(CIGlobalNPC.ThisCalamitasRebornP2 < 0 || !Main.npc[CIGlobalNPC.ThisCalamitasRebornP2].active)
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
            float broToTarDistX = brother.position.X + brother.width/2 - player.position.X - player.width/2;
            float broToTarDistY = brother.position.Y + brother.height - 59f - player.position.Y - player.height / 2;
            float broRot = (float)Math.Atan2(broToTarDistX, broToTarDistY) + MathHelper.PiOver2;
            */
            //保转角
            float bDistX = brother.position.X + brother.width / 2 - player.position.X - player.width /2;
            float bDistY = brother.position.Y + brother.height / 2 - 59f - player.position.Y - player.height /2;
            float broRot = (float)Math.Atan2(bDistX, bDistY) + MathHelper.PiOver2;
            // BrothersKeepRotation(brother, broRot, 0.15f);
            //丢弃
            BrothersGeneric.KeepAngle(brother, 0.15f, broRot); 
            #endregion
            #region 兄弟脱战
            //封装
            BrothersGeneric.BrothersDespawns(player, brother);
            #endregion
            #region 兄弟行为
            if (brother.ai[1] == 0f)
            {
                #region 顺滑移动
                float broProjSpeedMax = 5f;
                float broProjAccel  = 0.1f;
                //射弹朝向
                int broProjAttackDir = 1; 
                if (brother.position.X + brother.width / 2 < player.position.X + player.width)
                    broProjAttackDir = -1;
                //获取射弹与玩家的距离
                Vector2 projVec = new(brother.position.X + brother.width * 0.5f, brother.position.Y + brother.height * 0.5f);
                float projTarX = player.position.X + player.width / 2 + broProjAttackDir * 180 - projVec.X;
                float projTarY = player.position.Y + player.height/ 2 -  projVec.Y;
                float projTarDist = (float)Math.Sqrt(projTarX * projTarX + projTarY * projTarY);
                if(ifDeath)
                {
                    float speedUp = 0.5f;
                    if (projTarDist > 300f && projTarDist < 900f)
                        broProjSpeedMax += speedUp + 0.05f; 
                        //原灾在这里整了10个if来复线加速度效果，这里直接改成普通的加速度了
                }
                //让boss的移动变得顺滑，这里类似于回旋镖的那个AI
                projTarDist = broProjSpeedMax / projTarDist;
                projTarX *= projTarDist;
                projTarY *= projTarDist;
                //追逐玩家
                CIFunction.TryCatchTraget(brother, projTarX, projTarY, broProjAccel);
                #endregion
                #region 兄弟发射弹幕
                //这里才正式开始发射弹幕
                brother.ai[2] += 1f; //计时器
                if(Main.rand.NextBool()) brother.ai[2] += 1f;
                if (brother.ai[2] >= 90f) //180f, 大约三秒
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
                            BrothersShoot.JustShoot(brother, player, projType, projSpeed, projDMG);
                            /*
                            projVec = new Vector2(brother.position.X + brother.width / 2, brother.position.Y + brother.height / 2);
                            projTarX = player.position.X + player.width / 2 - projVec.X;
                            projTarY = player.position.Y + player.height/ 2 - projVec.Y;
                            projTarDist = (float)Math.Sqrt(projTarX * projTarY+ projTarY * projTarY);
                            projTarDist = projSpeed / projTarDist;
                            projTarX *= projTarDist;
                            projTarY *= projTarDist;
                            projTarX += brother.velocity.X/2;
                            projTarY += brother.velocity.Y/2;
                            projVec.X -= projTarX;
                            projVec.Y -= projTarY;
                            Projectile.NewProjectile(brother.GetSource_FromAI(), projVec.X, projVec.Y, projTarX, projTarY, projType, projDMG, 0f, Main.myPlayer, 0f, 0f);   
                            */
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
                    BrothersCharge.ChargeInit(brother, player, broRot, 36f);
                    /*
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
                    //new Velocity -> 单独分配横轴纵轴
                    brother.velocity.X = chargeTarDistX * chargeTarDistReal;
                    brother.velocity.Y = chargeTarDistY * chargeTarDistReal;
                    //ai[1] = 2f用于减速
                    */
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
                        brother.velocity.X *= 0.93f;
                        brother.velocity.Y *= 0.93f;
                        if (brother.velocity.X > -0.1 && brother.velocity.X < 0.1)
                            brother.velocity.X = 0f;
                        if (brother.velocity.Y > -0.1 && brother.velocity.Y < 0.1)
                            brother.velocity.Y = 0f;
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
        
    }
}