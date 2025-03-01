using System;
using System.Drawing;
using System.Globalization;
using System.Numerics;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.NPCs.Calamitas;
using CalamityMod.NPCs.CalClone;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.NPCs.Calamitas
{
    /*
    *一些基本修改查阅:
    *判定boss击倒的类从CalamityWorld转给CalamityCondition了, 调用的时候要在CalamityCondion.成员名后增加.IsMet();
    *
    */
    
    public class CICalCloneLegacyAI
    {
        public static readonly int CalCloneDefense = 25;
        //一阶段普灾发射火球的转角速度
        public static readonly float BallRot = 9.5f; 
        //二阶段普灾发射火球的转角速度
        public static readonly float BallRotP2 = 10f;
        //一阶段普灾发射火球的转角速度(死亡)
        public static readonly float BallRotDeath = 10.5f;
        //二阶段普灾发射火球的转角速度(死亡)
        public static readonly float BallRotDeathP2 = 11.5f;
        //一阶段普灾发射火球时自身加速度
        public static readonly float BallAccele = 0.170f;
        //二阶段普灾发射火球时自身加速度
        public static readonly float BallAcceleP2 = 0.185f;
        //一阶段普灾发射火球时自身加速度(死亡)
        public static readonly float BallAcceleDeath = 0.180f;
        //二阶段普灾发射火球时自身加速度(死亡)
        public static readonly float BallAcceleDeathP2 = 0.21f;
        //一阶段普灾发射激光时自身转角速度, 一般情况下这个不应该会被使用
        public static readonly float LaserRot = 8f;
        //二阶段普灾发射激光时自身转角速度 
        public static readonly float LaserRotP2 = 8.5f;
        //一阶段普灾发射激光时自身转角速度(死亡), 一般情况下这个不应该会被使用
        public static readonly float LaserRotDeath = 9.5f;
        //二阶段普灾发射激光时自身转角速度(死亡)
        public static readonly float LaserRotDeathP2 = 10f;
        //一阶段普灾发射激光时自身加速度
        public static readonly float LaserAccele = 0.2f;
        //二阶段普灾发射激光时自身加速度
        public static readonly float LaserAcceleP2 = 0.205f;
        //一阶段普灾发射激光时自身加速度(死亡)
        public static readonly float LaserAcceleDeath = 0.25f;
        //二阶段普灾发射激光时自身加速度(死亡)
        public static readonly float LaserAcceleDeathP2 = 0.255f;
        /// <summary>
        /// 普灾AI的封装
        /// </summary>
        /// <param name="npc">普灾npc本身</param>
        /// <param name="mod">mod,不出意外你直接填Mod都没事</param>
        /// <param name="calCloneDefense">普灾防御的情况, 这里更推荐用固定的全局变量赋值，而不是使用npc.defense</param>
        /// <param name="ifPhase2">是否处于二阶段</param>
        public static void CalCloneAI(NPC npc, Mod mod, int calCloneDefense, bool ifPhase2)
        {
            CIGlobalNPC getNPC = npc.CalamityInheritance();     

            Vector2 getCalPos = new((int)((npc.position.X + (npc.width/2))/16f), (int)((npc.position.Y + (npc.height/2))/16f));
            float CalClonePosX = npc.position.X;
            float CalClonePosY = npc.position.Y;
            //发光
            Lighting.AddLight(getCalPos, TorchID.Red);
            //查看剩余血量
            float getLifePercent = npc.life / (float)npc.lifeMax;
            //提示boss进入二阶段, 并多人同步
            if(getLifePercent <= 0.75f && Main.netMode != NetmodeID.MultiplayerClient && !ifPhase2)
            {
                NPC.NewNPC(npc.GetSource_FromThis(),(int)npc.Center.X, (int)npc.position.Y + npc.height, ModContent.NPCType<CalamitasPhase2>(), npc.whoAmI);
                //这里需要一个发送进入二阶段的文本，旧灾的哪个
                // if(Main.netMode == NetmodeID.SinglePlayer); //如果是单人游戏，直接发送
                // else if(Main.netMode == NetmodeID.Server); //如果不是，采用netMessage.BroadcastChatMessage进行多人同步
                npc.active = false;
                npc.netUpdate = true;
                return; //进入二阶段后可以ban掉这个boss了
            }
            //玩家当前处于的难度
            bool ifDeath = CalamityWorld.death;
            bool ifRevengence = CalamityWorld.revenge;
            bool ifExpert = Main.expertMode;
            bool ifDayTime = Main.dayTime;
            bool ifProviDowned = CalamityConditions.DownedProvidence.IsMet();
            //查看兄弟是否还在
            bool ifBrotherAlive = false;
            //二阶段的AI们
            if(ifPhase2)
            {
                //试图生成探魂眼环
                CIGlobalAI.CalamitasCloneWhoAmI = npc.whoAmI; //get这个AI
                //将calamitas的AI存到这个自建的数组里面
                if(getNPC.BossNewAI[1] == 0f && getLifePercent <= 0.35f && ifExpert) 
                {
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        SoundEngine.PlaySound(SoundID.Item74, new Vector2(npc.position.X, npc.position.Y)); //播报生成音
                        for(int i = 0; i<5; i++) //生成5个
                        {
                            int getNewEye = NPC.NewNPC(npc.GetSource_FromThis(), (int)(CalClonePosX + (Math.Sin(i * 72)*150)), (int)(CalClonePosY + (Math.Cos(i * 72) * 150)), ModContent.NPCType<SoulSeeker>(), npc.whoAmI, 0,0,0,-1);
                            NPC whatEye = Main.npc[getNewEye];
                            whatEye.ai[0] = i * 72; //???
                        }
                    }
                    string key = "这里需要三阶段的文本";
                    Color getTextColor = Color.Orange;
                    //多人同步
                    if(Main.netMode == NetmodeID.SinglePlayer)
                        Main.NewText(Language.GetTextValue(key), getTextColor);
                    else if(Main.netMode == NetmodeID.Server)
                        ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), getTextColor);
                    //生成完毕后将数组内ai置1
                    getNPC.BossNewAI[1] = 1f;
                }
                //生成兄弟
                if(getNPC.BossNewAI[0] == 0f && npc.life>0)
                    getNPC.BossNewAI[0] = npc.lifeMax; //将calClone的最大生命值存放进去
                if(npc.life >0)
                {
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        //旧版兄弟会分别在70%, 40%与10%都重生一次，这个变量就是取30%不生成的差值
                        int spawnBrotherPhase = (int)(npc.lifeMax * 0.3);
                        //普灾当前生命值 + 上述的差值若小于数组存放的boss血量
                        if((npc.life + spawnBrotherPhase) < getNPC.BossNewAI[0]) 
                        {
                            getNPC.BossNewAI[0] = npc.life; //更新普灾的血量
                            if(getNPC.BossNewAI[0] <= (float)npc.lifeMax * 0.1)//这个干嘛的？
                            {
                                //生成两个兄弟, 先做占位符，ai后面在考虑
                                NPC.NewNPC(npc.GetSource_FromThis(), (int)CalClonePosX, (int)(CalClonePosY + npc.height), ModContent.NPCType<Cataclysm>(), npc.whoAmI);
                                NPC.NewNPC(npc.GetSource_FromThis(), (int)CalClonePosX, (int)(CalClonePosY + npc.height), ModContent.NPCType<Cataclysm>(), npc.whoAmI);
                                //这里需要发送生成兄弟的文本
                                string brotherSpawnKey = "兄弟重生的相关文本";
                                Color getTextColor = Color.Orange;
                                //多人信息同步
                                if(Main.netMode == NetmodeID.SinglePlayer)
                                    Main.NewText(Language.GetTextValue(brotherSpawnKey),getTextColor);
                                else if (Main.netMode == NetmodeID.Server)
                                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey(brotherSpawnKey), getTextColor);
                            }
                            else if(getNPC.BossNewAI[0] <= (float)npc.lifeMax * 0.3)
                            {
                                //生成两个兄弟, 先做占位符，ai后面在考虑
                                NPC.NewNPC(npc.GetSource_FromThis(), (int)CalClonePosX, (int)(CalClonePosY + npc.height), ModContent.NPCType<Cataclysm>(), npc.whoAmI);
                                NPC.NewNPC(npc.GetSource_FromThis(), (int)CalClonePosX, (int)(CalClonePosY + npc.height), ModContent.NPCType<Cataclysm>(), npc.whoAmI);
                                //这里需要发送生成兄弟的文本
                                string brotherSpawnKey = "兄弟重生的相关文本";
                                Color getTextColor = Color.Orange;
                                //多人信息同步
                                if(Main.netMode == NetmodeID.SinglePlayer)
                                    Main.NewText(Language.GetTextValue(brotherSpawnKey),getTextColor);
                                else if (Main.netMode == NetmodeID.Server)
                                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey(brotherSpawnKey), getTextColor);
                            }
                            else
                            {
                                //生成两个兄弟, 先做占位符，ai后面在考虑
                                NPC.NewNPC(npc.GetSource_FromThis(), (int)CalClonePosX, (int)(CalClonePosY + npc.height), ModContent.NPCType<Cataclysm>(), npc.whoAmI);
                                NPC.NewNPC(npc.GetSource_FromThis(), (int)CalClonePosX, (int)(CalClonePosY + npc.height), ModContent.NPCType<Cataclysm>(), npc.whoAmI);
                                //这里需要发送生成兄弟的文本
                                string brotherSpawnKey = "兄弟重生的相关文本";
                                Color getTextColor = Color.Orange;
                                //多人信息同步
                                if(Main.netMode == NetmodeID.SinglePlayer)
                                    Main.NewText(Language.GetTextValue(brotherSpawnKey),getTextColor);
                                else if (Main.netMode == NetmodeID.Server)
                                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey(brotherSpawnKey), getTextColor);
                            }
                        }
                    }
                }
                #region 兄弟存活时,给予普灾防御增幅
                //这里在原本的AI里套了你妈蛋的四层缩进合计22行来实现普灾兄弟在场时提供普灾防御增幅的功能, 
                //总之下面是优化后的代码, 去掉注释一共8行
                //先根据是否处于二阶段AI存放普灾的防御值, CalCloneDefense是二阶段的普灾防御力
                int defenseStored = ifPhase2 ? calCloneDefense : calCloneDefense - 15;
                int defenseBuff = 0;
                defenseBuff += (CIGlobalAI.CatalysmCloneWhoAmI != -1 && Main.npc[CIGlobalAI.CatalysmCloneWhoAmI].active)?  255 : 0;
                defenseBuff += (CIGlobalAI.CatastropheCloneWhoAmI != -1 && Main.npc[CIGlobalAI.CatastropheCloneWhoAmI].active)? 255 : 0;
                //有兄弟在场都行
                if((CIGlobalAI.CatalysmCloneWhoAmI != -1 && Main.npc[CIGlobalAI.CatalysmCloneWhoAmI].active) || (CIGlobalAI.CatastropheCloneWhoAmI != -1 && Main.npc[CIGlobalAI.CatastropheCloneWhoAmI].active))
                    ifBrotherAlive = true;
                //不出意外的话，双兄弟都死球的时候，这里应该只会直接返回defenseStored.
                npc.defense = ifProviDowned ? defenseStored + defenseBuff : (defenseStored + defenseBuff) * 50;
                //取消普灾的跟踪
                npc.chaseable = !ifBrotherAlive;
                #endregion
            }
            #region 普灾朝向玩家
            //为普灾寻找一个攻击对象
            if(npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
                npc.TargetClosest(true);

            Player player = Main.player[npc.target];

            //贴图旋转, 还是number大佬
            float tarRotateX = npc.position.X + (npc.width/2)  - player.position.X  - (player.width/2);
            float tarRotateY = npc.position.Y + (npc.height/2) - player.position.Y - (player.height/2);
            float tarRotateReal = (float)Math.Atan2(tarRotateX, tarRotateY) + MathHelper.PiOver2;
            if(tarRotateReal < 0f) tarRotateReal += MathHelper.TwoPi;
            else if(tarRotateReal > MathHelper.TwoPi) tarRotateReal -= MathHelper.TwoPi;

            float getRotateSpeed = 0.1f; //WHAT?
            if(npc.rotation < tarRotateReal) //npc的转角<需要的转角时
            {
                if((tarRotateReal - npc.rotation) > MathHelper.Pi) //再跑这个bool类语句，看需要的转角-npc转角后是否大于pi
                    npc.rotation -= getRotateSpeed; //如果是，减转角
                else
                    npc.rotation += getRotateSpeed; //如果不是，加转角, 下逻辑相同
            }       //等会，为什么要这么写？
            else if(npc.rotation>tarRotateReal)
            {
                if((npc.rotation - tarRotateReal) > MathHelper.Pi)
                    npc.rotation += getRotateSpeed;
                else
                    npc.rotation -= getRotateSpeed;
            }
            //不行，这里我看不懂了
            if(npc.rotation > tarRotateReal - getRotateSpeed && npc.rotation < tarRotateReal + getRotateSpeed)
                npc.rotation = tarRotateReal;
            if(npc.rotation < 0f)
                npc.rotation += MathHelper.TwoPi;//byd为什么这里也要原地转一圈
            else if(npc.rotation > MathHelper.TwoPi)
                npc.rotation -= MathHelper.TwoPi; //还转?
            //相同的代码为什么还要再跑一次?
            if(npc.rotation > tarRotateReal - getRotateSpeed && npc.rotation < tarRotateReal + getRotateSpeed)
                npc.rotation = tarRotateReal;
            #endregion
            #region 普灾退场(非击杀)
            if(!player.active || player.dead || (ifDayTime && !Main.eclipse))
            {
                npc.TargetClosest(false); //取消普灾索敌
                player = Main.player[npc.target];
                //相同的代码怎么还要再跑一次？
                if(!player.active || player.dead || (ifDayTime && !Main.eclipse))
                {
                    if(npc.velocity.Y > 3f)
                        npc.velocity.Y = 3f;
                    npc.velocity.Y -= 0.1f;
                    if(npc.velocity.Y < -12f)
                        npc.velocity.Y  = -12f;
                    if(npc.timeLeft > 60)
                        npc.timeLeft = 60;
                    if(npc.ai[1]!= 0f)
                    {
                        npc.ai[1] = 0f;
                        npc.ai[2] = 0f;
                        npc.netUpdate = true;
                    }
                    return;
                }
            }
            else if (npc.timeLeft < 1800)
                npc.timeLeft = 1800;
            #endregion
            #region 普灾攻击AI:漂浮在玩家头上,发射激光与火球
            

            //这下面原代码一点注释都没，还全是num战神   
            //我家姑奶奶来了都不会他妈把相同的代码复制四份再配上if else
            if(npc.ai[1] < 2f)
            {
                //a[1]存储行为逻辑, 其中ai[1] = 0时普灾悬在头顶上发射火球, ai[1] = 1时普灾则水平方向上发射火球与激光 
                //行为逻辑的不同，普灾的转角速度和加速度都不同
                float calCloneRotate = 0f;
                float calCloneAccle = 0f;
                switch (npc.ai[1])
                {
                    case 0f: //头顶发射火球
                        //死亡?是->二阶段?是->使用死亡模式P2的参数/否->进入一阶段死亡模式判定，下同
                        calCloneRotate  = ifDeath ? (ifPhase2? BallRotDeathP2    : BallRotDeath)     : (ifPhase2 ? BallRotP2     : BallRot);
                        calCloneAccle   = ifDeath ? (ifPhase2? BallAcceleDeathP2 : BallAcceleDeath)  : (ifPhase2 ? BallAcceleP2  : BallAccele);
                        break;
                    case 1f: //水平发射激光
                        calCloneRotate  = ifDeath ? (ifPhase2? LaserRotDeathP2    : LaserRotDeath)     : (ifPhase2 ? LaserRotP2     : LaserRot);
                        calCloneAccle   = ifDeath ? (ifPhase2? LaserAcceleDeathP2 : LaserAcceleDeath)  : (ifPhase2 ? LaserAcceleP2  : LaserAccele);
                        break;
                    default: //其他状态，直接跑路 
                        break;
                }

                //玩家如果手持真近战武器，降低普灾的加速度
                Item ifHeldingTrueMelee = player.inventory[player.selectedItem];
                if(ifHeldingTrueMelee.CountsAsClass<TrueMeleeDamageClass>()) calCloneAccle *= 0.5f;
                //查看亵渎天神是否已经被干掉了?如果是，上述的基础数值乘以1.25f
                calCloneRotate = ifProviDowned ? calCloneRotate * 1.25f : calCloneRotate;
                calCloneAccle = ifProviDowned ? calCloneAccle * 1.25f : calCloneAccle;
                //为普灾创建一个基本的AI
                Vector2 calCloneStart = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float getDistToPlrX = player.position.X + (player.width/2)  - calCloneStart.X;
                float getDistToPlrY = player.position.Y + (player.height/2) - calCloneStart.Y;
                float getDistVector = (float)Math.Sqrt(getDistToPlrX*getDistToPlrX + getDistToPlrY*getDistToPlrY);
                getDistVector = calCloneRotate / getDistVector;
                getDistToPlrX *= getDistVector;
                getDistToPlrY *= getDistVector;
                //下面这串AI类似于回旋镖的加速效果，用来确保普灾能跟得上玩家的?我也不知道
				#region 普灾加速
                if(npc.velocity.X < getDistToPlrX)
                {
                    npc.velocity.X += calCloneAccle; //普灾加速
                    if(npc.velocity.X < 0f && getDistToPlrX > 0f)
                        npc.velocity.X += calCloneAccle;
                }
                else if(npc.velocity.X>getDistToPlrX)
                {
                    npc.velocity.X -= calCloneAccle;
                    if(npc.velocity.X > 0f && getDistToPlrX < 0f)
                        npc.velocity.X -= calCloneAccle;
                }
                if(npc.velocity.Y < getDistToPlrY)
                {
                    npc.velocity.X += calCloneAccle; //普灾加速
                    if(npc.velocity.Y < 0f && getDistToPlrY > 0f)
                        npc.velocity.Y += calCloneAccle;
                }
                else if(npc.velocity.Y>getDistToPlrY)
                {
                    npc.velocity.Y -= calCloneAccle;
                    if(npc.velocity.Y > 0f && getDistToPlrY < 0f)
                        npc.velocity.Y -= calCloneAccle;
                }
				#endregion
                //这里，ai[2]应该是作为一个计时器使用, 用来表示普灾在水平方向上发射弹幕的CD
                npc.ai[2] += 1f;
                if(npc.ai[2]>= (ifPhase2 ? 200f : 300f)) //300f->5秒, 200f->大约3秒
                {
                    npc.ai[1] = 1f;
                    npc.ai[2] = 0f;
                    npc.TargetClosest(true);
                    npc.netUpdate = true;
                }
                //
                getDistToPlrX = player.position.X = (player.width/2)  - calCloneStart.X;
                getDistToPlrY = player.position.Y = (player.height/2) - calCloneStart.Y;
                #region 发射火球
                if(Main.netMode!=NetmodeID.MultiplayerClient)
                {
                    float timerBuffer = 0f;
                    npc.localAI[1] += 1f; //初始化计时器
                    //ai大致上都相同，但是计时器有一定区分:
                    if(!ifBrotherAlive) //只有兄弟不在场的时候, 普灾发射弹幕的速度才是正常的3秒, 否则, 都会在下面不断地自增
                    {
                        if(ifExpert) timerBuffer += ifDeath? 1f : 1f * (1f - getLifePercent);
                        if(ifRevengence) timerBuffer += 0.5f;
                        npc.localAI[1] += timerBuffer;
                    }
                    if(npc.localAI[1] > 180) //计时器达到180f(3秒)后, 发射一个火球弹幕
                    {
                        npc.localAI[0] = 0f; //重置发射火球的计时器
                        //给弹幕一个基本的AI
                        //我不知道原文是啥，只能猜是弹幕转角，因为下面就要准备发射弹幕了
                        float projRotate = ifExpert ? 14f : 12.5f;
                        //一阶段与二阶段的唯一差距只有伤害
                        //BYD他妈的灾厄在这里实现是完完整整复制一份到下面，真尼玛逆天啊
                        int projDMG = ifDeath ? (ifPhase2? 170 : 150) : (ifPhase2? 120 : 100); 
                        int getProjType = ModContent.ProjectileType<BrimstoneHellfireball>();
                        //获取弹幕与玩家之间的距离, 这是一个向量模运算
                        float getProjDist = (float)Math.Sqrt(getDistToPlrX * getDistToPlrX + getDistToPlrY * getDistToPlrY);
                        //可能是转角，我也不清楚
                        getProjDist = projRotate / getProjDist;
                        getDistToPlrX *= getProjDist;
                        getDistToPlrY *= getProjDist;
                        //弹幕的起始位置修改
                        calCloneStart.X += getDistToPlrX * 6f;
                        calCloneStart.Y += getDistToPlrY * 6f;
                        //弹幕速度
                        Vector2 projVel = new(getDistToPlrX, getDistToPlrY);
                        //发射这个弹幕, 保证使其穿墙
                        int fireBall = Projectile.NewProjectile(npc.GetSource_FromThis(), calCloneStart, projVel, getProjType, projDMG + (ifProviDowned?200:0), 0f, npc.whoAmI);
                        if(!Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
                        Main.projectile[fireBall].tileCollide = false;
                    }
                }
                #endregion
                
            }
            #region 水平方向上发射激光
            if(npc.ai[1] == 1f)    
            {

            }
            #endregion
            #endregion
        }
        #region Calamitas Clone
		public static void CalamitasCloneAI(NPC npc, Mod mod, bool phase2)
		{
			CIGlobalNPC calamityGlobalNPC = npc.CalamityInheritance();

			// Emit light
			Lighting.AddLight((int)((npc.position.X + (npc.width / 2)) / 16f), (int)((npc.position.Y + (npc.height / 2)) / 16f), 1f, 0f, 0f);

			// Percent life remaining
			float lifeRatio = npc.life / (float)npc.lifeMax;

			// Spawn phase 2 Cal
			if (lifeRatio <= 0.75f && Main.netMode != NetmodeID.MultiplayerClient && !phase2)
			{
				NPC.NewNPC(npc.GetSource_FromThis(),(int)npc.Center.X, (int)npc.position.Y + npc.height, ModContent.NPCType<CalamitasRun3>(), npc.whoAmI);
				string key = "Mods.CalamityMod.CalamitasBossText";
				Color messageColor = Color.Orange;
				if (Main.netMode == NetmodeID.SinglePlayer)
				{
					Main.NewText(Language.GetTextValue(key), messageColor);
				}
				else if (Main.netMode == NetmodeID.Server)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
				}
				npc.active = false;
				npc.netUpdate = true;
				return;
			}

			// Variables for increasing difficulty
			bool death = CalamityWorld.death || CalamityWorld.bossRushActive;
			bool revenge = CalamityWorld.revenge || CalamityWorld.bossRushActive;
			bool expertMode = Main.expertMode || CalamityWorld.bossRushActive;
			bool dayTime = Main.dayTime && !CalamityWorld.bossRushActive;
			bool provy = CalamityWorld.downedProvidence && !CalamityWorld.bossRushActive;

			// Variable for live brothers
			bool brotherAlive = false;

			if (phase2)
			{
				// For seekers
				CalamityGlobalNPC.calamitas = npc.whoAmI;

				// Seeker ring
				if (calamityGlobalNPC.newAI[1] == 0f && lifeRatio <= 0.35f && expertMode)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 74);
						for (int I = 0; I < 5; I++)
						{
							int FireEye = NPC.NewNPC((int)(npc.Center.X + (Math.Sin(I * 72) * 150)), (int)(npc.Center.Y + (Math.Cos(I * 72) * 150)), ModContent.NPCType<SoulSeeker>(), npc.whoAmI, 0, 0, 0, -1);
							NPC Eye = Main.npc[FireEye];
							Eye.ai[0] = I * 72;
						}
					}

					string key = "Mods.CalamityMod.CalamitasBossText3";
					Color messageColor = Color.Orange;
					if (Main.netMode == NetmodeID.SinglePlayer)
						Main.NewText(Language.GetTextValue(key), messageColor);
					else if (Main.netMode == NetmodeID.Server)
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);

					calamityGlobalNPC.newAI[1] = 1f;
				}

				// Spawn brothers
				if (calamityGlobalNPC.newAI[0] == 0f && npc.life > 0)
					calamityGlobalNPC.newAI[0] = npc.lifeMax;

				if (npc.life > 0)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int num660 = (int)(npc.lifeMax * 0.3); //70%, 40%, and 10%
						if ((npc.life + num660) < calamityGlobalNPC.newAI[0])
						{
							calamityGlobalNPC.newAI[0] = npc.life;
							if (calamityGlobalNPC.newAI[0] <= (float)npc.lifeMax * 0.1)
							{
								NPC.NewNPC((int)npc.Center.X, (int)npc.position.Y + npc.height, ModContent.NPCType<CalamitasRun>(), npc.whoAmI);
								NPC.NewNPC((int)npc.Center.X, (int)npc.position.Y + npc.height, ModContent.NPCType<CalamitasRun2>(), npc.whoAmI);

								string key = "Mods.CalamityMod.CalamitasBossText2";
								Color messageColor = Color.Orange;
								if (Main.netMode == NetmodeID.SinglePlayer)
									Main.NewText(Language.GetTextValue(key), messageColor);
								else if (Main.netMode == NetmodeID.Server)
									ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
							}
							else if (calamityGlobalNPC.newAI[0] <= (float)npc.lifeMax * 0.4)
								NPC.NewNPC((int)npc.Center.X, (int)npc.position.Y + npc.height, ModContent.NPCType<CalamitasRun2>(), npc.whoAmI);
							else
								NPC.NewNPC((int)npc.Center.X, (int)npc.position.Y + npc.height, ModContent.NPCType<CalamitasRun>(), npc.whoAmI);
						}
					}
				}

				// Huge defense boost if brothers are alive
				int num568 = 0;
				if (expertMode)
				{
					if (CalamityGlobalNPC.cataclysm != -1)
					{
						if (Main.npc[CalamityGlobalNPC.cataclysm].active)
						{
							brotherAlive = true;
							num568 += 255;
						}
					}
					if (CalamityGlobalNPC.catastrophe != -1)
					{
						if (Main.npc[CalamityGlobalNPC.catastrophe].active)
						{
							brotherAlive = true;
							num568 += 255;
						}
					}
					npc.defense += num568 * 50;
					if (!brotherAlive)
						npc.defense = provy ? 150 : 25;
				}

				// Disable homing if brothers are alive
				npc.chaseable = !brotherAlive;
			}

			// Get a target
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
				npc.TargetClosest(true);

			// Target variable
			Player player = Main.player[npc.target];

			// Rotation
			float num801 = npc.position.X + (npc.width / 2) - player.position.X - (player.width / 2);
			float num802 = npc.position.Y + npc.height - 59f - player.position.Y - (player.height / 2);
			float num803 = (float)Math.Atan2(num802, num801) + MathHelper.PiOver2;
            //控制普灾朝向是否对头的
            //不是哥们，为什么你朝向错了要原地旋转360度啊?
			if (num803 < 0f)
				num803 += MathHelper.TwoPi;
			else if (num803 > MathHelper.TwoPi)
				num803 -= MathHelper.TwoPi;

			float num804 = 0.1f;
			if (npc.rotation < num803)
			{
				if ((num803 - npc.rotation) > MathHelper.Pi)
					npc.rotation -= num804;
				else
					npc.rotation += num804;
			}
			else if (npc.rotation > num803)
			{
				if ((npc.rotation - num803) > MathHelper.Pi)
					npc.rotation += num804;
				else
					npc.rotation -= num804;
			}

			if (npc.rotation > num803 - num804 && npc.rotation < num803 + num804)
				npc.rotation = num803;
			if (npc.rotation < 0f)
				npc.rotation += MathHelper.TwoPi;
			else if (npc.rotation > MathHelper.TwoPi)
				npc.rotation -= MathHelper.TwoPi;
			if (npc.rotation > num803 - num804 && npc.rotation < num803 + num804)
				npc.rotation = num803;

			// Despawn
			if (!player.active || player.dead || (dayTime && !Main.eclipse))
			{
				npc.TargetClosest(false);
				player = Main.player[npc.target];
				if (!player.active || player.dead || (dayTime && !Main.eclipse))
				{
					if (npc.velocity.Y > 3f)
						npc.velocity.Y = 3f;
					npc.velocity.Y -= 0.1f;
					if (npc.velocity.Y < -12f)
						npc.velocity.Y = -12f;

					if (npc.timeLeft > 60)
						npc.timeLeft = 60;

					if (npc.ai[1] != 0f)
					{
						npc.ai[1] = 0f;
						npc.ai[2] = 0f;
						npc.netUpdate = true;
					}
					return;
				}
			}
			else if (npc.timeLeft < 1800)
				npc.timeLeft = 1800;

			// Float above target and fire lasers or fireballs
			if (npc.ai[1] == 0f)
			{
				float num823 = expertMode ? 9.5f : 8f;
				float num824 = expertMode ? 0.175f : 0.15f;
				if (phase2)
				{
					num823 = expertMode ? 10f : 8.5f;
					num824 = expertMode ? 0.18f : 0.155f;
				}
				if (death)
				{
					num823 += 1f;
					num824 += 0.02f;
				}

				// Reduce acceleration if target is holding a true melee weapon
				Item targetSelectedItem = player.inventory[player.selectedItem];
				if (targetSelectedItem.CountsAsClass(DamageClass.Melee) && (targetSelectedItem.shoot == 0 || CalamityMod.trueMeleeProjectileList.Contains(targetSelectedItem.shoot)))
				{
					num824 *= 0.5f;
				}

				if (provy)
				{
					num823 *= 1.25f;
					num824 *= 1.25f;
				}
				if (CalamityWorld.bossRushActive)
				{
					num823 *= 1.5f;
					num824 *= 1.5f;
				}

				Vector2 vector82 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
				float num825 = player.position.X + (player.width / 2) - vector82.X;
				float num826 = player.position.Y + (player.height / 2) - ((CalamityWorld.bossRushActive ? 400f : 300f) + (phase2 ? 60f : 0f)) - vector82.Y;
				float num827 = (float)Math.Sqrt(num825 * num825 + num826 * num826);
				num827 = num823 / num827;
				num825 *= num827;
				num826 *= num827;

				if (npc.velocity.X < num825)
				{
					npc.velocity.X += num824;
					if (npc.velocity.X < 0f && num825 > 0f)
						npc.velocity.X += num824;
				}
				else if (npc.velocity.X > num825)
				{
					npc.velocity.X -= num824;
					if (npc.velocity.X > 0f && num825 < 0f)
						npc.velocity.X -= num824;
				}
				if (npc.velocity.Y < num826)
				{
					npc.velocity.Y += num824;
					if (npc.velocity.Y < 0f && num826 > 0f)
						npc.velocity.Y += num824;
				}
				else if (npc.velocity.Y > num826)
				{
					npc.velocity.Y -= num824;
					if (npc.velocity.Y > 0f && num826 < 0f)
						npc.velocity.Y -= num824;
				}

				npc.ai[2] += 1f;
				if (npc.ai[2] >= (phase2 ? 200f : 300f))
				{
					npc.ai[1] = 1f;
					npc.ai[2] = 0f;
					npc.TargetClosest(true);
					npc.netUpdate = true;
				}

				num825 = player.position.X + (player.width / 2) - vector82.X;
				num826 = player.position.Y + (player.height / 2) - vector82.Y;

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.localAI[1] += 1f;
					if (phase2)
					{
						if (!brotherAlive)
						{
							if (expertMode)
								npc.localAI[1] += death ? 1f : 1f * (1f - lifeRatio);
							if (revenge)
								npc.localAI[1] += 0.5f;
						}

						if (npc.localAI[1] > 180f)
						{
							npc.localAI[1] = 0f;
							float num828 = CalamityWorld.bossRushActive ? 16f : (expertMode ? 14f : 12.5f);
							if (calamityGlobalNPC.enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && CalamityWorld.bossRushActive))
								num828 += 5f;

							int num829 = expertMode ? CalamityUtils.GetMasterModeProjectileDamage(34, 1.5) : 42;
							int num830 = ModContent.ProjectileType<BrimstoneHellfireball>();
							num827 = (float)Math.Sqrt(num825 * num825 + num826 * num826);
							num827 = num828 / num827;
							num825 *= num827;
							num826 *= num827;
							vector82.X += num825 * 6f;
							vector82.Y += num826 * 6f;
							if (!Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
							{
								int proj = Projectile.NewProjectile(vector82.X, vector82.Y, num825, num826, num830, num829 + (provy ? 30 : 0), 0f, Main.myPlayer, player.Center.X, player.Center.Y);
								Main.projectile[proj].tileCollide = false;
							}
							else
								Projectile.NewProjectile(vector82.X, vector82.Y, num825, num826, num830, num829 + (provy ? 30 : 0), 0f, Main.myPlayer, 0f, 0f);
						}
					}
					else
					{
						if (revenge)
							npc.localAI[1] += 0.5f;

						if (npc.localAI[1] > 180f && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
						{
							npc.localAI[1] = 0f;
							float num828 = CalamityWorld.bossRushActive ? 16f : (expertMode ? 13f : 10.5f);
							int num829 = expertMode ? CalamityUtils.GetMasterModeProjectileDamage(28, 1.5) : 35;
							int num830 = ModContent.ProjectileType<BrimstoneLaser>();
							num827 = (float)Math.Sqrt(num825 * num825 + num826 * num826);
							num827 = num828 / num827;
							num825 *= num827;
							num826 *= num827;
							vector82.X += num825 * 12f;
							vector82.Y += num826 * 12f;
							Projectile.NewProjectile(vector82.X, vector82.Y, num825, num826, num830, num829 + (provy ? 30 : 0), 0f, Main.myPlayer, 0f, 0f);
						}
					}
				}
			}

			// Float to the side of the target and fire lasers
			else if (npc.ai[1] == 1f)
			{
				int num831 = 1;
				if (npc.position.X + (npc.width / 2) < player.position.X + player.width)
					num831 = -1;

				float num832 = expertMode ? 9.5f : 8f;
				float num833 = expertMode ? 0.25f : 0.2f;
				if (phase2)
				{
					num832 = expertMode ? 10f : 8.5f;
					num833 = expertMode ? 0.255f : 0.205f;
				}
				if (death)
				{
					num832 += 1f;
					num833 += 0.02f;
				}

				// Reduce acceleration if target is holding a true melee weapon
				Item targetSelectedItem = player.inventory[player.selectedItem];
				if (targetSelectedItem.CountsAsClass(DamageClass.Melee) && (targetSelectedItem.shoot == 0 || CalamityMod.trueMeleeProjectileList.Contains(targetSelectedItem.shoot)))
				{
					num833 *= 0.5f;
				}

				if (provy)
				{
					num832 *= 1.25f;
					num833 *= 1.25f;
				}
				if (CalamityWorld.bossRushActive)
				{
					num832 *= 1.5f;
					num833 *= 1.5f;
				}

				Vector2 vector83 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
				float num834 = player.position.X + (player.width / 2) + (num831 * (CalamityWorld.bossRushActive ? 460 : 360)) - vector83.X;
				float num835 = player.position.Y + (player.height / 2) - vector83.Y;
				float num836 = (float)Math.Sqrt(num834 * num834 + num835 * num835);
				num836 = num832 / num836;
				num834 *= num836;
				num835 *= num836;

				if (npc.velocity.X < num834)
				{
					npc.velocity.X += num833;
					if (npc.velocity.X < 0f && num834 > 0f)
						npc.velocity.X += num833;
				}
				else if (npc.velocity.X > num834)
				{
					npc.velocity.X -= num833;
					if (npc.velocity.X > 0f && num834 < 0f)
						npc.velocity.X -= num833;
				}
				if (npc.velocity.Y < num835)
				{
					npc.velocity.Y += num833;
					if (npc.velocity.Y < 0f && num835 > 0f)
						npc.velocity.Y += num833;
				}
				else if (npc.velocity.Y > num835)
				{
					npc.velocity.Y -= num833;
					if (npc.velocity.Y > 0f && num835 < 0f)
						npc.velocity.Y -= num833;
				}

				num834 = player.position.X + (player.width / 2) - vector83.X;
				num835 = player.position.Y + (player.height / 2) - vector83.Y;

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.localAI[1] += 1f;
					if (phase2)
					{
						if (!brotherAlive)
						{
							if (revenge)
								npc.localAI[1] += 0.5f;
							if (calamityGlobalNPC.enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && CalamityWorld.bossRushActive))
								npc.localAI[1] += 0.5f;
							if (expertMode)
								npc.localAI[1] += 0.5f;
						}

						if (npc.localAI[1] >= 60f && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
						{
							npc.localAI[1] = 0f;
							float num837 = CalamityWorld.bossRushActive ? 15f : 11f;
							int num838 = brotherAlive ? (expertMode ? CalamityUtils.GetMasterModeProjectileDamage(34, 1.5) : 42) : (expertMode ? CalamityUtils.GetMasterModeProjectileDamage(28, 1.5) : 35);
							int num839 = brotherAlive ? ModContent.ProjectileType<BrimstoneHellfireball>() : ModContent.ProjectileType<BrimstoneLaser>();
							num836 = (float)Math.Sqrt(num834 * num834 + num835 * num835);
							num836 = num837 / num836;
							num834 *= num836;
							num835 *= num836;
							vector83.X += num834 * 12f;
							vector83.Y += num835 * 12f;
							if (!Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
							{
								int proj = Projectile.NewProjectile(vector83.X, vector83.Y, num834, num835, ModContent.ProjectileType<BrimstoneHellfireball>(), (expertMode ? CalamityUtils.GetMasterModeProjectileDamage(34, 1.5) : 42) + (provy ? 30 : 0), 0f, Main.myPlayer, player.Center.X, player.Center.Y);
								Main.projectile[proj].tileCollide = false;
							}
							else
								Projectile.NewProjectile(vector83.X, vector83.Y, num834, num835, num839, num838 + (provy ? 30 : 0), 0f, Main.myPlayer, 0f, 0f);
						}
					}
					else
					{
						if (revenge)
							npc.localAI[1] += 0.5f;

						if (npc.localAI[1] >= 60f && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
						{
							npc.localAI[1] = 0f;
							float num837 = CalamityWorld.bossRushActive ? 14f : 10.5f;
							int num838 = expertMode ? CalamityUtils.GetMasterModeProjectileDamage(20, 1.5) : 24;
							int num839 = ModContent.ProjectileType<BrimstoneLaser>();
							num836 = (float)Math.Sqrt(num834 * num834 + num835 * num835);
							num836 = num837 / num836;
							num834 *= num836;
							num835 *= num836;
							vector83.X += num834 * 12f;
							vector83.Y += num835 * 12f;
							Projectile.NewProjectile(vector83.X, vector83.Y, num834, num835, num839, num838 + (provy ? 30 : 0), 0f, Main.myPlayer, 0f, 0f);
						}
					}
				}

				npc.ai[2] += 1f;
				if (npc.ai[2] >= (phase2 ? 120f : 180f))
				{
					npc.ai[1] = phase2 && !brotherAlive && lifeRatio < 0.7f && revenge ? 4f : 0f;
					npc.ai[2] = 0f;
					npc.TargetClosest(true);
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[1] == 2f)
			{
				npc.rotation = num803;

				float chargeVelocity = (CalamityWorld.death || CalamityWorld.bossRushActive) ? 27f : 25f;

				if (provy)
					chargeVelocity *= 1.25f;

				if (CalamityWorld.bossRushActive)
					chargeVelocity *= 1.5f;

				Vector2 vector = Vector2.Normalize(player.Center + player.velocity * 10f - npc.Center);
				npc.velocity = vector * chargeVelocity;

				npc.ai[1] = 3f;
			}
			else if (npc.ai[1] == 3f)
			{
				npc.ai[2] += 1f;

				float chargeTime = 70f;
				if (CalamityWorld.bossRushActive)
					chargeTime *= 0.8f;

				if (npc.ai[2] >= chargeTime)
				{
					npc.velocity *= 0.93f;
					if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
						npc.velocity.X = 0f;
					if (npc.velocity.Y > -0.1 && npc.velocity.Y < 0.1)
						npc.velocity.Y = 0f;
				}
				else
					npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - MathHelper.PiOver2;

				if (npc.ai[2] >= chargeTime + 15f)
				{
					npc.ai[3] += 1f;
					npc.ai[2] = 0f;
					npc.target = 255;
					npc.rotation = num803;
					if (npc.ai[3] > 1f)
					{
						npc.ai[1] = 0f;
						npc.ai[3] = 0f;
						return;
					}
					npc.ai[1] = 4f;
				}
			}
			else
			{
				int num62 = 500;
				float num63 = (calamityGlobalNPC.enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && CalamityWorld.bossRushActive)) ? 20f : 14f;
				float num64 = (calamityGlobalNPC.enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && CalamityWorld.bossRushActive)) ? 0.5f : 0.35f;

				if (provy)
				{
					num63 *= 1.25f;
					num64 *= 1.25f;
				}

				if (CalamityWorld.bossRushActive)
				{
					num63 *= 1.5f;
					num64 *= 1.5f;
				}

				int num408 = 1;
				if (npc.position.X + (npc.width / 2) < Main.player[npc.target].position.X + Main.player[npc.target].width)
					num408 = -1;

				Vector2 vector11 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
				float num65 = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2) + (num62 * num408) - vector11.X;
				float num66 = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2) - vector11.Y;
				float num67 = (float)Math.Sqrt(num65 * num65 + num66 * num66);

				num67 = num63 / num67;
				num65 *= num67;
				num66 *= num67;

				if (npc.velocity.X < num65)
				{
					npc.velocity.X += num64;
					if (npc.velocity.X < 0f && num65 > 0f)
						npc.velocity.X += num64;
				}
				else if (npc.velocity.X > num65)
				{
					npc.velocity.X -= num64;
					if (npc.velocity.X > 0f && num65 < 0f)
						npc.velocity.X -= num64;
				}
				if (npc.velocity.Y < num66)
				{
					npc.velocity.Y += num64;
					if (npc.velocity.Y < 0f && num66 > 0f)
						npc.velocity.Y += num64;
				}
				else if (npc.velocity.Y > num66)
				{
					npc.velocity.Y -= num64;
					if (npc.velocity.Y > 0f && num66 < 0f)
						npc.velocity.Y -= num64;
				}

				npc.ai[2] += 1f;
				if (npc.ai[2] >= 45f)
				{
					npc.TargetClosest(true);
					npc.ai[1] = 2f;
					npc.ai[2] = 0f;
					npc.netUpdate = true;
				}
			}
		}

		public static void CataclysmAI(NPC npc, Mod mod)
		{
			CalamityGlobalNPC calamityGlobalNPC = npc.Calamity();

			// Emit light
			Lighting.AddLight((int)((npc.position.X + (npc.width / 2)) / 16f), (int)((npc.position.Y + (npc.height / 2)) / 16f), 1f, 0f, 0f);

			CalamityGlobalNPC.cataclysm = npc.whoAmI;

			bool death = CalamityWorld.death || CalamityWorld.bossRushActive;
			bool revenge = CalamityWorld.revenge || CalamityWorld.bossRushActive;
			bool expertMode = Main.expertMode || CalamityWorld.bossRushActive;
			bool dayTime = Main.dayTime && !CalamityWorld.bossRushActive;
			bool provy = CalamityWorld.downedProvidence && !CalamityWorld.bossRushActive;

			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
				npc.TargetClosest(true);

			Player player = Main.player[npc.target];

			float num840 = npc.position.X + (npc.width / 2) - player.position.X - (player.width / 2);
			float num841 = npc.position.Y + npc.height - 59f - player.position.Y - (player.height / 2);
			float num842 = (float)Math.Atan2(num841, num840) + MathHelper.PiOver2;
			if (num842 < 0f)
				num842 += MathHelper.TwoPi;
			else if (num842 > MathHelper.TwoPi)
				num842 -= MathHelper.TwoPi;

			float num843 = 0.15f;
			if (npc.rotation < num842)
			{
				if ((num842 - npc.rotation) > MathHelper.Pi)
					npc.rotation -= num843;
				else
					npc.rotation += num843;
			}
			else if (npc.rotation > num842)
			{
				if ((npc.rotation - num842) > MathHelper.Pi)
					npc.rotation += num843;
				else
					npc.rotation -= num843;
			}

			if (npc.rotation > num842 - num843 && npc.rotation < num842 + num843)
				npc.rotation = num842;
			if (npc.rotation < 0f)
				npc.rotation += MathHelper.TwoPi;
			else if (npc.rotation > MathHelper.TwoPi)
				npc.rotation -= MathHelper.TwoPi;
			if (npc.rotation > num842 - num843 && npc.rotation < num842 + num843)
				npc.rotation = num842;

			if (!player.active || player.dead || (dayTime && !Main.eclipse))
			{
				npc.TargetClosest(false);
				player = Main.player[npc.target];
				if (!player.active || player.dead || (dayTime && !Main.eclipse))
				{
					if (npc.velocity.Y > 3f)
						npc.velocity.Y = 3f;
					npc.velocity.Y -= 0.1f;
					if (npc.velocity.Y < -12f)
						npc.velocity.Y = -12f;

					calamityGlobalNPC.newAI[0] = 1f;

					if (npc.timeLeft > 60)
						npc.timeLeft = 60;

					if (npc.ai[1] != 0f)
					{
						npc.ai[1] = 0f;
						npc.ai[2] = 0f;
						npc.ai[3] = 0f;
						npc.netUpdate = true;
					}
					return;
				}
			}
			else
				calamityGlobalNPC.newAI[0] = 0f;

			if (npc.ai[1] == 0f)
			{
				float num861 = 5f;
				float num862 = 0.1f;
				if (provy)
				{
					num861 *= 1.25f;
					num862 *= 1.25f;
				}
				if (CalamityWorld.bossRushActive)
				{
					num861 *= 1.5f;
					num862 *= 1.5f;
				}

				int num863 = 1;
				if (npc.position.X + (npc.width / 2) < player.position.X + player.width)
					num863 = -1;

				Vector2 vector86 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
				float num864 = player.position.X + (player.width / 2) + (num863 * (CalamityWorld.bossRushActive ? 270 : 180)) - vector86.X;
				float num865 = player.position.Y + (player.height / 2) - vector86.Y;
				float num866 = (float)Math.Sqrt(num864 * num864 + num865 * num865);

				if (expertMode || provy)
				{
					if (num866 > 300f)
						num861 += 0.5f;
					if (num866 > 400f)
						num861 += 0.5f;
					if (num866 > 500f)
						num861 += 0.55f;
					if (num866 > 600f)
						num861 += 0.55f;
					if (num866 > 700f)
						num861 += 0.6f;
					if (num866 > 800f)
						num861 += 0.6f;
				}

				num866 = num861 / num866;
				num864 *= num866;
				num865 *= num866;

				if (npc.velocity.X < num864)
				{
					npc.velocity.X += num862;
					if (npc.velocity.X < 0f && num864 > 0f)
						npc.velocity.X += num862;
				}
				else if (npc.velocity.X > num864)
				{
					npc.velocity.X -= num862;
					if (npc.velocity.X > 0f && num864 < 0f)
						npc.velocity.X -= num862;
				}
				if (npc.velocity.Y < num865)
				{
					npc.velocity.Y += num862;
					if (npc.velocity.Y < 0f && num865 > 0f)
						npc.velocity.Y += num862;
				}
				else if (npc.velocity.Y > num865)
				{
					npc.velocity.Y -= num862;
					if (npc.velocity.Y > 0f && num865 < 0f)
						npc.velocity.Y -= num862;
				}

				npc.ai[2] += (calamityGlobalNPC.enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && CalamityWorld.bossRushActive)) ? 2f : 1f;
				if (npc.ai[2] >= 240f)
				{
					npc.TargetClosest(true);
					npc.ai[1] = 1f;
					npc.ai[2] = 0f;
					npc.target = 255;
					npc.netUpdate = true;
				}

				bool fireDelay = npc.ai[2] > 120f || npc.life < npc.lifeMax * 0.9;
				if (Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height) && fireDelay)
				{
					npc.localAI[2] += 1f;
					if (npc.localAI[2] > 22f)
					{
						npc.localAI[2] = 0f;
						Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 34);
					}

					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						npc.localAI[1] += 1f;
						if (revenge)
							npc.localAI[1] += 0.5f;

						if (npc.localAI[1] > 12f)
						{
							npc.localAI[1] = 0f;
							float num867 = CalamityWorld.bossRushActive ? 9f : 6f;
							int num868 = expertMode ? CalamityUtils.GetMasterModeProjectileDamage(30, 1.5) : 38;
							int num869 = ModContent.ProjectileType<BrimstoneFire>();
							vector86 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
							num864 = player.position.X + (player.width / 2) - vector86.X;
							num865 = player.position.Y + (player.height / 2) - vector86.Y;
							num866 = (float)Math.Sqrt(num864 * num864 + num865 * num865);
							num866 = num867 / num866;
							num864 *= num866;
							num865 *= num866;
							num865 += npc.velocity.Y * 0.5f;
							num864 += npc.velocity.X * 0.5f;
							vector86.X -= num864 * 1f;
							vector86.Y -= num865 * 1f;
							Projectile.NewProjectile(vector86.X, vector86.Y, num864, num865, num869, num868 + (provy ? 30 : 0), 0f, Main.myPlayer, 0f, 0f);
						}
					}
				}
			}
			else
			{
				if (npc.ai[1] == 1f)
				{
					Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
					npc.rotation = num842;

					float num870 = 14f;
					if (expertMode)
						num870 += 2f;
					if (revenge)
						num870 += 2f;
					if (death)
						num870 += 2f;
					if (calamityGlobalNPC.enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && CalamityWorld.bossRushActive))
						num870 += 4f;
					if (provy)
						num870 *= 1.15f;
					if (CalamityWorld.bossRushActive)
						num870 *= 1.25f;

					Vector2 vector87 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
					float num871 = player.position.X + (player.width / 2) - vector87.X;
					float num872 = player.position.Y + (player.height / 2) - vector87.Y;
					float num873 = (float)Math.Sqrt(num871 * num871 + num872 * num872);
					num873 = num870 / num873;
					npc.velocity.X = num871 * num873;
					npc.velocity.Y = num872 * num873;
					npc.ai[1] = 2f;
					return;
				}

				if (npc.ai[1] == 2f)
				{
					npc.ai[2] += 1f;
					if (expertMode)
						npc.ai[2] += 0.25f;
					if (revenge)
						npc.ai[2] += 0.25f;
					if (CalamityWorld.bossRushActive)
						npc.ai[2] += 0.25f;

					if (npc.ai[2] >= 75f)
					{
						npc.velocity.X *= 0.93f;
						npc.velocity.Y *= 0.93f;

						if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
							npc.velocity.X = 0f;
						if (npc.velocity.Y > -0.1 && npc.velocity.Y < 0.1)
							npc.velocity.Y = 0f;
					}
					else
						npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - MathHelper.PiOver2;

					if (npc.ai[2] >= 105f)
					{
						npc.ai[3] += 1f;
						npc.ai[2] = 0f;
						npc.target = 255;
						npc.rotation = num842;
						if (npc.ai[3] >= 3f)
						{
							npc.ai[1] = 0f;
							npc.ai[3] = 0f;
							return;
						}
						npc.ai[1] = 1f;
					}
				}
			}
		}

		public static void CatastropheAI(NPC npc, Mod mod)
		{
			CalamityGlobalNPC calamityGlobalNPC = npc.Calamity();

			// Emit light
			Lighting.AddLight((int)((npc.position.X + (npc.width / 2)) / 16f), (int)((npc.position.Y + (npc.height / 2)) / 16f), 1f, 0f, 0f);

			CalamityGlobalNPC.catastrophe = npc.whoAmI;

			bool death = CalamityWorld.death || CalamityWorld.bossRushActive;
			bool revenge = CalamityWorld.revenge || CalamityWorld.bossRushActive;
			bool expertMode = Main.expertMode || CalamityWorld.bossRushActive;
			bool dayTime = Main.dayTime && !CalamityWorld.bossRushActive;
			bool provy = CalamityWorld.downedProvidence && !CalamityWorld.bossRushActive;

			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
				npc.TargetClosest(true);

			Player player = Main.player[npc.target];

			float num840 = npc.position.X + (npc.width / 2) - player.position.X - (player.width / 2);
			float num841 = npc.position.Y + npc.height - 59f - player.position.Y - (player.height / 2);
			float num842 = (float)Math.Atan2(num841, num840) + MathHelper.PiOver2;
			if (num842 < 0f)
				num842 += MathHelper.TwoPi;
			else if (num842 > MathHelper.TwoPi)
				num842 -= MathHelper.TwoPi;

			float num843 = 0.15f;
			if (npc.rotation < num842)
			{
				if ((num842 - npc.rotation) > MathHelper.Pi)
					npc.rotation -= num843;
				else
					npc.rotation += num843;
			}
			else if (npc.rotation > num842)
			{
				if ((npc.rotation - num842) > MathHelper.Pi)
					npc.rotation += num843;
				else
					npc.rotation -= num843;
			}

			if (npc.rotation > num842 - num843 && npc.rotation < num842 + num843)
				npc.rotation = num842;
			if (npc.rotation < 0f)
				npc.rotation += MathHelper.TwoPi;
			else if (npc.rotation > MathHelper.TwoPi)
				npc.rotation -= MathHelper.TwoPi;
			if (npc.rotation > num842 - num843 && npc.rotation < num842 + num843)
				npc.rotation = num842;

			if (!player.active || player.dead || (dayTime && !Main.eclipse))
			{
				npc.TargetClosest(false);
				player = Main.player[npc.target];
				if (!player.active || player.dead || (dayTime && !Main.eclipse))
				{
					if (npc.velocity.Y > 3f)
						npc.velocity.Y = 3f;
					npc.velocity.Y -= 0.1f;
					if (npc.velocity.Y < -12f)
						npc.velocity.Y = -12f;

					calamityGlobalNPC.newAI[0] = 1f;

					if (npc.timeLeft > 60)
						npc.timeLeft = 60;

					if (npc.ai[1] != 0f)
					{
						npc.ai[1] = 0f;
						npc.ai[2] = 0f;
						npc.ai[3] = 0f;
						npc.netUpdate = true;
					}
					return;
				}
			}
			else
				calamityGlobalNPC.newAI[0] = 0f;

			if (npc.ai[1] == 0f)
			{
				float num861 = 4.5f;
				float num862 = 0.2f;
				if (provy)
				{
					num861 *= 1.25f;
					num862 *= 1.25f;
				}
				if (CalamityWorld.bossRushActive)
				{
					num861 *= 1.5f;
					num862 *= 1.5f;
				}

				int num863 = 1;
				if (npc.position.X + (npc.width / 2) < player.position.X + player.width)
					num863 = -1;

				Vector2 vector86 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
				float num864 = player.position.X + (player.width / 2) + (num863 * (CalamityWorld.bossRushActive ? 270 : 180)) - vector86.X;
				float num865 = player.position.Y + (player.height / 2) - vector86.Y;
				float num866 = (float)Math.Sqrt(num864 * num864 + num865 * num865);

				if (expertMode || provy)
				{
					if (num866 > 300f)
						num861 += 0.5f;
					if (num866 > 400f)
						num861 += 0.5f;
					if (num866 > 500f)
						num861 += 0.55f;
					if (num866 > 600f)
						num861 += 0.55f;
					if (num866 > 700f)
						num861 += 0.6f;
					if (num866 > 800f)
						num861 += 0.6f;
				}

				num866 = num861 / num866;
				num864 *= num866;
				num865 *= num866;

				if (npc.velocity.X < num864)
				{
					npc.velocity.X += num862;
					if (npc.velocity.X < 0f && num864 > 0f)
						npc.velocity.X += num862;
				}
				else if (npc.velocity.X > num864)
				{
					npc.velocity.X -= num862;
					if (npc.velocity.X > 0f && num864 < 0f)
						npc.velocity.X -= num862;
				}
				if (npc.velocity.Y < num865)
				{
					npc.velocity.Y += num862;
					if (npc.velocity.Y < 0f && num865 > 0f)
						npc.velocity.Y += num862;
				}
				else if (npc.velocity.Y > num865)
				{
					npc.velocity.Y -= num862;
					if (npc.velocity.Y > 0f && num865 < 0f)
						npc.velocity.Y -= num862;
				}

				npc.ai[2] += (calamityGlobalNPC.enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && CalamityWorld.bossRushActive)) ? 2f : 1f;
				if (npc.ai[2] >= 180f)
				{
					npc.TargetClosest(true);
					npc.ai[1] = 1f;
					npc.ai[2] = 0f;
					npc.target = 255;
					npc.netUpdate = true;
				}

				bool fireDelay = npc.ai[2] > 120f || npc.life < npc.lifeMax * 0.9;
				if (Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height) && fireDelay)
				{
					npc.localAI[2] += 1f;
					if (npc.localAI[2] > 36f)
					{
						npc.localAI[2] = 0f;
						Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 34);
					}

					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						npc.localAI[1] += 1f;
						if (revenge)
							npc.localAI[1] += 0.5f;

						if (npc.localAI[1] > 50f)
						{
							npc.localAI[1] = 0f;
							float num867 = CalamityWorld.bossRushActive ? 18f : 12f;
							int num868 = expertMode ? CalamityUtils.GetMasterModeProjectileDamage(29, 1.5) : 36;
							int num869 = ModContent.ProjectileType<BrimstoneBall>();
							vector86 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
							num864 = player.position.X + (player.width / 2) - vector86.X;
							num865 = player.position.Y + (player.height / 2) - vector86.Y;
							num866 = (float)Math.Sqrt(num864 * num864 + num865 * num865);
							num866 = num867 / num866;
							num864 *= num866;
							num865 *= num866;
							num865 += npc.velocity.Y * 0.5f;
							num864 += npc.velocity.X * 0.5f;
							vector86.X -= num864 * 1f;
							vector86.Y -= num865 * 1f;
							Projectile.NewProjectile(vector86.X, vector86.Y, num864, num865, num869, num868 + (provy ? 30 : 0), 0f, Main.myPlayer, 0f, 0f);
						}
					}
				}
			}
			else
			{
				if (npc.ai[1] == 1f)
				{
					Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
					npc.rotation = num842;

					float num870 = 16f;
					if (expertMode)
						num870 += 2f;
					if (revenge)
						num870 += 2f;
					if (death)
						num870 += 2f;
					if (calamityGlobalNPC.enraged > 0 || (CalamityConfig.Instance.BossRushXerocCurse && CalamityWorld.bossRushActive))
						num870 += 4f;
					if (provy)
						num870 *= 1.15f;
					if (CalamityWorld.bossRushActive)
						num870 *= 1.25f;

					Vector2 vector87 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
					float num871 = player.position.X + (player.width / 2) - vector87.X;
					float num872 = player.position.Y + (player.height / 2) - vector87.Y;
					float num873 = (float)Math.Sqrt(num871 * num871 + num872 * num872);
					num873 = num870 / num873;
					npc.velocity.X = num871 * num873;
					npc.velocity.Y = num872 * num873;
					npc.ai[1] = 2f;
					return;
				}

				if (npc.ai[1] == 2f)
				{
					npc.ai[2] += 1f;
					if (expertMode)
						npc.ai[2] += 0.25f;
					if (revenge)
						npc.ai[2] += 0.25f;
					if (CalamityWorld.bossRushActive)
						npc.ai[2] += 0.25f;

					if (npc.ai[2] >= 60f) //50
					{
						npc.velocity.X *= 0.93f;
						npc.velocity.Y *= 0.93f;

						if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
							npc.velocity.X = 0f;
						if (npc.velocity.Y > -0.1 && npc.velocity.Y < 0.1)
							npc.velocity.Y = 0f;
					}
					else
						npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - MathHelper.PiOver2;

					if (npc.ai[2] >= 90f) //80
					{
						npc.ai[3] += 1f;
						npc.ai[2] = 0f;
						npc.target = 255;
						npc.rotation = num842;
						if (npc.ai[3] >= 4f)
						{
							npc.ai[1] = 0f;
							npc.ai[3] = 0f;
							return;
						}
						npc.ai[1] = 1f;
					}
				}
			}
		}
		#endregion
    }
}