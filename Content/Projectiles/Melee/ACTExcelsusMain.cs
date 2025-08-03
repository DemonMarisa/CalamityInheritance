using System;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Utilities;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Melee;
using Microsoft.Build.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class ACTExcelsusMain : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        #region 基础属性
        //阶段Timer
        public int Timer = 0;
        //发起追踪的Timer
        public int TimerAlt = 0;
        public int AnotherTimer = 0;
        public int MouseTimer = 0;
        //发起追踪的射弹是否已经进行过追踪。
        public bool CheckIfHit = false;
        public int HomeOnHitCounts = 0;
        public int ForceTarget = -1;
        #endregion
        #region 攻击打表
        //追踪射弹击中敌怪后的回击
        const float IsOnHitHoming = 1f;
        //追踪逻辑
        const float IsHoming = 2f;
        //普通逻辑
        const float IsFlying = 3f;
        //指向鼠标的逻辑
        const float IsPointMouse = 4f;
        const short AttackType = 0;
        #endregion
        /*如果真的需要看得懂这史山的话找这下面这个攻击顺序去理解
        攻击顺序:
        1.正常飞行
        2.发起第一次追踪
        3.旋转，而后回击一次
        4.回击一次后，再次旋转，回击一次
        5.消失
        */
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.alpha = 100;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override bool? CanDamage()
        {
            //射弹刚发射出来，没有开始追踪的时候
            bool canHoming = Timer < ACTExcelsus.HomingTimer;
            //射弹已经开始发起第一次追踪，且完成了转角修正后 
            bool canIdling = TimerAlt > ACTExcelsus.IdleTimer;
            //射弹击中后再次发起追踪，且完成了转角修正后
            bool canOnHitHoming = AnotherTimer > ACTExcelsus.IdleTimer * 2;
            //符合上述情况的都会允许造成伤害
            return canHoming || canIdling || canOnHitHoming; 
        }
        //这一串代码写的有点史山
        public override void AI()
        {
            //Projectile.ai[AttackType] <=> Projectile.ai[0] 仅仅用于标记射弹的AI逻辑
            //timer是外置的
            
            //这个用于第五下逻辑，在所有AI代码之前优先判定
            if (HomeOnHitCounts == 3)
                DoFading();
            //搜寻实例，这个过程在AttackType == IsFading时撤销
            NPC tar = CIFunction.FindClosestTarget(Projectile, ACTExcelsus.MaxSearchDist, true);
            //击中了但凡一次敌怪之后我们都直接把这个实例换成被击中的那个实例 
            if (ForceTarget != -1)
            {
                tar = Main.npc[ForceTarget];
                Projectile.netUpdate = true;
            }
            if (Timer > ACTExcelsus.HomingTimer)
            {
                //这个东西如果已经在执行IsOnHitHoming的操作，撤销其搜寻
                if (tar != null && Projectile.ai[AttackType] != IsOnHitHoming && HomeOnHitCounts != 3)
                {
                    Projectile.ai[AttackType] = CheckIfHit ? IsOnHitHoming : IsHoming;
                    Projectile.netUpdate = true;
                }
                //理由同上，我们需要确保其一定执行IsOnHitHoming
                else if (tar == null)
                {
                    Projectile.ai[AttackType] = IsPointMouse;
                    Projectile.netUpdate = true;
                }
            }
            else Projectile.ai[AttackType] = IsFlying;
            //AI查询
            if (Projectile.owner == Main.myPlayer)
            switch (Projectile.ai[AttackType])
            {
                case IsFlying:
                    DoFlying();
                    break;
                case IsHoming:
                    DoHoming(tar);
                    break;
                case IsPointMouse:
                    DoPointMouse();
                    break;
                case IsOnHitHoming:
                    DoOnHitHoming(tar);
                    break;
                default:
                    break;
            }

            //保留特效
            if (Main.rand.NextBool(8))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueFairy, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            if (Main.rand.NextBool(8))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, Main.rand.NextBool(3) ? 56 : 242, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
        }
        //追踪射弹击中敌怪后的回击
        private void DoOnHitHoming(NPC tar)
        {
            MouseTimer = 0;
            Projectile.velocity *= 0.95f;
            AnotherTimer++;
            if (AnotherTimer < ACTExcelsus.IdleTimer * 2 && AnotherTimer > ACTExcelsus.IdleTimer)
            {
                //发起追踪前时候才会改变转角
                DoAngleToTarget(tar.Center); 
            }
            if (AnotherTimer > ACTExcelsus.IdleTimer * 2)
            {
                CIFunction.HomingNPCBetter(Projectile, tar, ACTExcelsus.MaxSearchDist, ACTExcelsus.HomingSpeed, 20f, 1, 10f);
            }
            //正式发起追踪前这个Timer得……变一下。
            else Projectile.timeLeft = 85;

        }

        private void DoAngleToTarget(Vector2 tar)
        {
            float rot = Projectile.AngleTo(tar) + MathHelper.PiOver4;
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, rot, ACTExcelsus.LerpAngle);
        }
        //没有搜索到目标的时候，指向鼠标
        private void DoPointMouse()
        {
            float rot = Projectile.AngleTo(Main.MouseWorld) + MathHelper.PiOver4;
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, rot, ACTExcelsus.LerpAngle);
            Projectile.velocity *= 0.9f;
        }

        //发起追踪前正常飞行
        public void DoFlying()
        {
            Timer++;
            //飞行过程中射弹会一直保持高速旋转, 除此之外就……就不干什么了。
            Projectile.rotation += ACTExcelsus.NonHomingRotation;
        }
        public void DoFading()
        {
            if (Projectile.timeLeft > ACTExcelsus.SideFadeInTime)
                Projectile.timeLeft = ACTExcelsus.SideFadeInTime;
            Projectile.alpha += (int)Utils.GetLerpValue(0, 255, 15);
            Projectile.velocity *= 0.95f;
        }
        //追踪逻辑
        public void DoHoming(NPC tar)
        {
            MouseTimer = 0;
            //首先搜索附近的NPC实例, 需注意的是，AI执行的这段时间内也会一直检索目标。
            Player p = Main.player[Projectile.owner];
            float spiningDir = ACTExcelsus.LerpAngle;
            Vector2 targetPos = tar.Center;
            //原地减速，指向这个敌怪
            float rot = Projectile.AngleTo(targetPos) + MathHelper.PiOver4;
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, rot, spiningDir);
            Projectile.velocity *= 0.9f;
            //而后在一段时间后发起追踪
            TimerAlt++;
            if (TimerAlt > ACTExcelsus.IdleTimer)
            {
                //给多2个额外更新
                Projectile.extraUpdates += 2;
                CIFunction.HomingNPCBetter(Projectile, tar, ACTExcelsus.MaxSearchDist, ACTExcelsus.HomingSpeed, 20f, 1, 10f);
                // CIFunction.HomeInOnNPC(Projectile, true, ACTExcelsus.MaxSearchDist, ACTExcelsus.HomingSpeed, 20f);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.tileCollide = false;
            if (Projectile.timeLeft > 85)
            {
                Projectile.timeLeft = 85;
            }
            return false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * (b2 / 255f));
                return new Color(b2, b2, b2, a2);
            }
            return default(Color);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Color color;
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * (b2 / 255f));
                color = new Color(b2, b2, b2, a2);
            }
            else
            {
                color = new Color(255, 255, 255, 100);
            }
            Vector2 origin = new Vector2(39f, 46f);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>($"{GenericProjRoute.ProjRoute}/Melee/ACTExcelsusMainGlow").Value, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(CISoundID.SoundLaser, Projectile.position);
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 180);
            //任何形式的攻击击中但凡一个单位，我们都直接把这个敌怪单位存进去，而且只存一次
            if (ForceTarget == -1)
            {
                //把当前这个敌怪单位存进去
                ForceTarget = target.whoAmI;
            }
            if (ForceTarget == target.whoAmI)
            {
                if (Projectile.ai[AttackType] == IsHoming && !CheckIfHit)
                {
                    //使AI类型转化为另外一种
                    Projectile.ai[AttackType] = IsOnHitHoming;
                    Projectile.netUpdate = true;
                    TimerAlt = 0;
                    CheckIfHit = true;
                }
                if (Projectile.ai[AttackType] == IsOnHitHoming && HomeOnHitCounts < 3)
                {
                    //该项攻击模式的攻击次数+1
                    HomeOnHitCounts += 1;
                    AnotherTimer = 0;
                }
                //第二次攻击判定后刷新其射弹属性为一次
                if (HomeOnHitCounts == 2)
                {
                    Projectile.penetrate = -1;
                    Projectile.localNPCHitCooldown= -1;
                }
                if (HomeOnHitCounts == 3)
                {
                    //直接脱锁
                    Projectile.ai[AttackType] = -1f;
                }
            }
            //火花, 从湮灭那抄过来的
            Vector2 particleSpawnDisplacement;
            Vector2 splatterDirection;

            particleSpawnDisplacement = new Vector2(2f * -Projectile.ai[2], 2f * -Projectile.ai[2]);
            splatterDirection = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);

            Vector2 SparkSpawnPosition = target.Center + particleSpawnDisplacement;

            if (Projectile.ai[1] % 4 == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    int sparkLifetime = Main.rand.Next(14, 21);
                    float sparkScale = Main.rand.NextFloat(0.8f, 1f) + 1f * 0.05f;
                    Color sparkColor = Color.Lerp(Color.Fuchsia, Color.AliceBlue, Main.rand.NextFloat(0.5f));
                    sparkColor = Color.Lerp(sparkColor, Color.Cyan, Main.rand.NextFloat());

                    if (Main.rand.NextBool(5))
                        sparkScale *= 1.4f;

                    Vector2 sparkVelocity = splatterDirection.RotatedByRandom(MathHelper.TwoPi);
                    sparkVelocity.Y -= 6f;
                    SparkParticle spark = new SparkParticle(SparkSpawnPosition, sparkVelocity, true, sparkLifetime, sparkScale, sparkColor);
                    GeneralParticleHandler.SpawnParticle(spark);
                }
            }
        }
    }
}
