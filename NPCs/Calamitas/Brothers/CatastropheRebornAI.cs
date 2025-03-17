using System;
using CalamityInheritance.Content.Items;
using CalamityInheritance.NPCs.Calamitas.Brothers.Projectiles;
using CalamityInheritance.Utilities;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

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
            //保转角
            float broRot = BrothersGeneric.KeepAngle(ref brother, 0.15f, player); 
            #endregion
            #region 兄弟脱战
            BrothersGeneric.BrothersDespawns(player, brother);
            #endregion
            switch (brother.ai[1])
            {
                case 0f:
                    BrothersGeneric.ChansingTarget(ref brother, player);
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
                                //TODO：无法发射射弹
                                brother.localAI[1] = 0f;
                                BrothersShoot.JustShoot(brother, player, ModContent.ProjectileType<CatastropheBall>(), 14f, 180);
                            }
                        }
                    }
                    break;
                case 1f:
                    //给冲刺速度一点微弱的随机性来使兄弟的冲刺有所差异
                    BrothersCharge.ChargeInit(brother, player, broRot, Main.rand.NextFloat(28f, 30f));
                    brother.ai[1] = 2f;
                    return;
                case 2f:
                    brother.damage = brother.defDamage;
                    brother.ai[2] += 1f;
                    if(brother.ai[2] >= 120f) BrothersGeneric.StopCharge(ref brother);
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
                    break;
                default:
                    break;
            }
        }
    }
}