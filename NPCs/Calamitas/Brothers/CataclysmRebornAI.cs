using System;
using CalamityInheritance.Content.Items;
using CalamityInheritance.NPCs.Calamitas.Brothers.Projectiles;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.World;
using Microsoft.Xna.Framework;
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
            #endregion
            //保转角
            float broRot = BrothersGeneric.KeepAngle(ref brother, 0.15f, player); 
            //兄弟脱战AI
            BrothersGeneric.BrothersDespawns(player, brother);
            #region 兄弟行为
            switch (brother.ai[1])
            {
                case 0f:
                    //兄弟追赶AI
                    BrothersGeneric.ChansingTarget(ref brother, player);
                    //这里才正式开始发射弹幕
                    brother.ai[2] += 1f; //计时器
                    if(Main.rand.NextBool()) brother.ai[2] += 1f;
                    if (brother.ai[2] >= 120f) //180f, 大约三秒
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
                                //目前的问题是他不发射射弹，两个兄弟都是，但封装反正在这了，看怎么整吧
                                //射弹，注意伤害是写死的
                                BrothersShoot.JustShoot(brother, player, ModContent.ProjectileType<CataclysmFire>(), NPC.AnyNPCs(ModContent.NPCType<CatastropheReborn>())? 4f : 6f, 120);
                            }
                        }
                    }
                    break;

                case 1f:
                    BrothersCharge.ChargeInit(brother, player, broRot, 30f);
                    brother.ai[1] = 2f;
                    return;

                case 2f:
                    brother.damage = brother.defDamage;
                    brother.ai[2] += ifDeath? 2f : 1.25f;
                    //Stop charging
                    if(brother.ai[2] >= 75f) BrothersGeneric.StopCharge(ref brother);
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
                    break;
                default:
                    break;
            }
            #endregion
        }
        
    }
}