using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using CalamityInheritance.Buffs.Legendary;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Summon
{
    //FUCK YOU CALAMITY YOU ABSOLUTE SHIT HOW DARE YOU FUCKING WRITE SELDOM MEANINGFUL COMMENT YOU MOTHERFUCKERRRRRRERR!!!!!!!!!!!!!!!
    //NOW I'M FUCKING KNOW WHERE YOU ARE, I'M NOW GONNA FUCKING YOUR BITCH MOM SLAY YOUR ASSHOLE DAD!!!!!!!!!!!!! 
    public class CryogenPtr : ModProjectile, ILocalizedModType
    {
        //大芬代码我看不懂很多，没办法了
        public new string LocalizationCategory => "Content.Projectiles.Summon";
        public int IfRecharge = -1;
        //冰刺是否在玩家身上旋转
        public bool Idle = true;
        //冰刺是否在敌怪周围旋转
        public bool Rounding = true;
        //右键冰刺距离敌怪的距离
        public float FloatyDist = 90f;
        public NPC tar = null;
        const float RegulaPtr = 1f;
        const float IfRightClickPtr = 2f;
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 60;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 0f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
        }
        //写AI
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(IfRecharge);
            writer.Write(Rounding);
            writer.Write(Idle);
            writer.Write(FloatyDist);
            writer.Write(tar is null ? -1 : tar.whoAmI);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            IfRecharge = reader.ReadInt32();
            Rounding = reader.ReadBoolean();
            Idle = reader.ReadBoolean();
            int realTar = reader.ReadInt32();
            tar = realTar == -1 ? null : Main.npc[realTar];
        }
        /*
        *我要去杀了所有写代码不写注释的人的亲妈
        *这串代码中，ai[1]用于表示是否为右键功能的射弹标记，如果ai[1] == 2f, 则右键射弹， ai[1] == 1f, 则是左键常规的射弹逻辑, 需注意
        */
        public override bool PreAI()
        {
            if (IfRecharge == -1)
            {
                IfRecharge = Projectile.ai[1] == 0f ? 10 : 0;
                NewDust(30);
            }
            if (Projectile.ai[1] == RegulaPtr && Projectile.timeLeft > 1000)
            {
                Projectile.ai[1] = 0f;
                Projectile.timeLeft = 200;
                Rounding = Idle = false;
                Projectile.netUpdate = true;
            }
            else if (Projectile.ai[1] >= IfRightClickPtr && Projectile.timeLeft > 900)
            {
                tar = CalamityUtils.MinionHoming(Projectile.Center, 1000f, Main.player[Projectile.owner]);
                if (tar != null)
                {
                    //?
                    Projectile.timeLeft = 669;
                    Projectile.ai[1]++;
                    Idle = false;
                    float height = tar.getRect().Height;
                    float width = tar.getRect().Width;
                    FloatyDist = MathHelper.Min((height > width ? height : width) * 3f, Main.LogicCheckScreenWidth * Main.LogicCheckScreenHeight / 2);
                    if (FloatyDist > Main.LogicCheckScreenWidth /3)
                        FloatyDist = Main.LogicCheckScreenWidth;
                    Projectile.penetrate = -1;
                    Projectile.usesIDStaticNPCImmunity = true;
                    Projectile.idStaticNPCHitCooldown = 4;
                    Projectile.netUpdate = true;
                }
            }
            if (Idle)
            {
                ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
                if (Projectile.penetrate == 1)
                    Projectile.penetrate++;
            }
            return true;
        }
        public override void AI()
        {
            Player plr = Main.player[Projectile.owner];
            var mplr = plr.CIMod();
            var src = Projectile.GetSource_FromThis();
            //标记是否正在启用召唤物用
            if (plr.dead)
                mplr.IsColdDivityActiving = false;
            if (!mplr.IsColdDivityActiving)
            {
                Projectile.active = false;
                return;
            }
            
            //如果在玩家周围待机，占用召唤栏
            if (Idle)
            {
                Projectile.minionSlots = 1f;
                Projectile.timeLeft = 2;
                if (!mplr.IsColdDivityActiving && IfRecharge > 0)
                    Projectile.Kill();
            }
            //如果在敌怪周围，视情况而定干掉射弹
            if (!Idle && Rounding)
                if (tar != null && (!tar.active || tar.life <= 0))
                    Projectile.Kill();
            //重新部署的时间
            if (IfRecharge > 0)
            {
                IfRecharge--;
                if (IfRecharge == 0)
                {
                    //释放一些粒子
                    SoundEngine.PlaySound(SoundID.Item30 with {Pitch = 0.2f}, Projectile.position);
                    Projectile.netUpdate = true;
                }
            }
            //实际执行的AI
            if (Rounding)
            {
                //如果没在玩家周围转圈, 去寻找一个目标
                if (Rounding && !Idle && Projectile.timeLeft < 120)
                {
                    IfRecharge = 0;
                    Projectile.usesIDStaticNPCImmunity = false;
                    Projectile.penetrate = 1;
                    float appDist = tar.getRect().Width > tar.getRect().Height ? tar.getRect().Width : tar.getRect().Height;
                    if (Projectile.timeLeft > 60)
                        FloatyDist += 5;
                    else
                        FloatyDist -= 10;
                }
                //在玩家周围转圈
                if (Idle)
                {
                    //?
                    float num = IfRecharge == 0 ? 60f : (300 - IfRecharge) /3;
                    float stdDist = num > 60f ? 60f : num;
                    //取玩家中心点，绕着玩家转
                    Projectile.Center = plr.Center + Projectile.ai[0].ToRotationVector2() * stdDist;
                    //转角，别忘了
                    Projectile.rotation = Projectile.ai[0] + (float)Math.Atan(90);
                    Projectile.ai[0] -= MathHelper.ToRadians(4f);
                    //寻找附近的单位，如果找到了则发射弹幕
                    NPC aliveTar = IfRecharge > 0 ? null : CalamityUtils.MinionHoming(Projectile.Center, 800f, plr);
                    //如果单位不为空，则发射弹幕
                    if (aliveTar != null && Projectile.owner == Main.myPlayer)
                    {
                        IfRecharge = mplr.ColdDivityTier1 ? 10 : 180;
                        Vector2 vel = Projectile.ai[0].ToRotationVector2().RotatedBy(Math.Atan(0));
                        vel.Normalize();
                        vel *= 30f;
                        int s = Projectile.NewProjectile(src, Projectile.position, vel, Projectile.type, (int)(Projectile.damage * 1.05f), Projectile.knockBack, Projectile.owner, Projectile.ai[0], RegulaPtr);
                        //动态变化其伤害
                        if (Main.projectile.IndexInRange(s))
                            Main.projectile[s].originalDamage = (int)(Projectile.originalDamage * 1.05f);
                    }
                    Projectile.netUpdate = Projectile.owner == Main.myPlayer;
                }
                //用于处理右键时针对敌怪的射弹逻辑
                else
                {
                    Projectile.Center = tar.Center + Projectile.ai[0].ToRotationVector2() * FloatyDist;
                    Projectile.rotation = Projectile.ai[0] + (float)Math.Atan(90);
                    Vector2 vel = Projectile.rotation.ToRotationVector2() - tar.Center;
                    vel.Normalize();
                    if (Projectile.timeLeft <= 120)
                        Projectile.rotation = Projectile.timeLeft <= 60 ? Projectile.ai[0] - (float)Math.Atan(90) : Projectile.rotation - (MathHelper.Distance(Projectile.rotation, -Projectile.rotation) / (120 - 60));
                    Projectile.ai[0] -= MathHelper.ToRadians(4f);
                }
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.Atan(90);
                Homing();
            }
        }
        public override bool? CanDamage()
        {
            return IfRecharge <= 0 && (Idle || (Rounding && (Projectile.timeLeft >= 120 || Projectile.timeLeft <= 45)) || !Rounding) && !Projectile.hide ? null : false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //T3时提供自定义的减益: 失温虹吸, 作为月后等级的霜冻
            target.AddBuff(ModContent.BuffType<CryoDrain>(), 300);
            int cir = 0;
            foreach (Projectile proj in Main.ActiveProjectiles)
            {
                if (proj.owner == Projectile.owner && proj.type == Projectile.type)
                {
                    CryogenPtr ptr = (CryogenPtr)proj.ModProjectile;
                    if (proj.ai[1] > 2f)
                        cir += Main.rand.Next(1, 4);
                }
            }
            cir = (int)MathHelper.Min(Main.rand.Next(15, 21), cir);
            if (Projectile.ai[1] > 2f)
                Projectile.ai[1]++;
            if (Projectile.ai[1] >= (30f - cir) && Projectile.timeLeft >= 120)
                IfRecharge = 15;
            
            if (Rounding && target == tar && Projectile.timeLeft < 60)
            {
                if (Projectile.timeLeft < 60)
                    Projectile.Kill();
            }
            else if (Idle)
            {
                IfRecharge = 15;
                NewDust(20); 
            } 
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Rounding && target == tar && Projectile.timeLeft < 60)
            {
                NewDust(30);
                SoundEngine.PlaySound(SoundID.NPCHit5, Projectile.position);
                modifiers.SourceDamage *= 1.1f;
            }
            else if (Rounding && target == tar && Projectile.timeLeft > 60)
            {
                NewDust(5);
                //操你妈灾厄
                modifiers.SourceDamage *= 0.8f;
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<CryoDrain>(), 300);
            if (Idle)
            {
                IfRecharge = 300;
                NewDust(20);
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCHit5, Projectile.position);
            NewDust(20);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(IfRecharge > 0 ? lightColor.R : 53, IfRecharge > 0 ? lightColor.G : Main.DiscoG, IfRecharge > 0 ? lightColor.B : 255, IfRecharge > 200 ? 255 : 255 - IfRecharge);
        }

        private void NewDust(int dAmt)
        {
            for (int i = 0; i < dAmt; i++)
                Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Ice, Main.rand.NextFloat(1, 3), Main.rand.NextFloat(1, 3), 0, Color.Cyan, Main.rand.NextFloat(0.5f, 1.5f));
        }
        private void Homing()
        {
             if (Projectile.timeLeft <= 240)
            {
                if (tar != null)
                {
                    tar.checkDead();
                    if (tar.life <= 0 || !tar.active || !tar.CanBeChasedBy(this, false))
                        tar = null;
                }
                tar ??= CalamityUtils.MinionHoming(Projectile.Center, 1000f, Main.player[Projectile.owner]);
                //开始发起追踪的AI 
                if (tar != null)
                {
                    float projVel = 40f;
                    Vector2 targetDirection = Projectile.Center;
                    float targetX = tar.Center.X - targetDirection.X;
                    float targetY = tar.Center.Y - targetDirection.Y;
                    float targetDist = (float)Math.Sqrt((double)(targetX * targetX + targetY * targetY));
                    if (targetDist < 100f)
                    {
                        projVel = 28f; //14
                    }
                    targetDist = projVel / targetDist;
                    targetX *= targetDist;
                    targetY *= targetDist;
                    Projectile.velocity.X = (Projectile.velocity.X * 20f + targetX) / 21f;
                    Projectile.velocity.Y = (Projectile.velocity.Y * 20f + targetY) / 21f;
                }
            }
        }
    }
}