using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Content.Items;
using System.Linq.Expressions;
using System;
using CalamityMod.Projectiles.Rogue;
using CalamityMod.Items.Weapons.Melee;
using System.Security;
using System.IO;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        bool ifSummonClone = false;
        public static readonly SoundStyle UseSound = SoundID.Item89 with { Volume = 0.45f }; //Item89:流星法杖射弹击中时的音效
        #region 别名
        public ref float AttackTimer => ref Projectile.ai[0];
        public ref float AttackType => ref Projectile.ai[1];
        public ref float IsCloned => ref Projectile.ai[2];
        public Player Owner => Main.player[Projectile.owner];
        #endregion
        #region 攻击枚举
        public const float IsShooted = 0f;
        public const float IsReturning = 1f;
        public const float IsStealth = 2f;
        #endregion
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            Projectile.DoSyncHandlerWrite(ref writer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.DoSyncHandlerRead(ref reader);
        }
        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 160;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.usesLocalNPCImmunity= true;
            Projectile.localNPCHitCooldown= 7; //6-7
            Projectile.timeLeft = 1000;
        }

        public override void AI()
        {

            DoGeneric();
            switch (AttackType)
            {
                case IsShooted:
                    DoShooted();
                    break;
                case IsReturning:
                    DoReturning();
                    break;
                case IsStealth:
                    DoStealth();
                    break;
            }
            
        }

        private void DoGeneric()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0.5f, 0.75f);
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 30;
                SoundEngine.PlaySound(CISoundID.SoundBoomerangs, Projectile.position);
            }
            //无论状态，锤子都应当在飞行过程中旋转
            Projectile.rotation += 0.22f;
        }

        private void DoStealth()
        {
            CIFunction.HomeInOnNPC(Projectile, true, 1250f, 24f, 0f, 30f);
            ifSummonClone = true;
            Projectile.netUpdate = true;
        }

        private void DoReturning()
        {
            float returnSpeed = 20f;
            float acceleration = 3.2f;
            //回旋镖返程的AI封装, 把这个封装了真的方便了很多东西
            CIFunction.BoomerangReturningAI(Owner, Projectile, returnSpeed, acceleration);
            if (Main.myPlayer == Projectile.owner)
            {
                //ai[2]用于查看锤子是否已经挂载过敌人，如果挂载过了就会赋一个-1f的值
                //这个主要是为了实现锤子的返程效果，具体看Clone那写的注释
                bool isNotClonedReturnProj = IsCloned != -1f;
                int pWidth  = isNotClonedReturnProj ? Owner.width  * 2 : (int)(Owner.width  * 1.5f);
                int pHeight = isNotClonedReturnProj ? Owner.height * 2 : (int)(Owner.height * 1.5f);
                //大锤子体积太大了，所以为了防止视觉上像是“锤子”敲了玩家头一样， 因此这个锤子会在离玩家更远的地方判定
                Rectangle mplrHitbox = new ((int)Owner.position.X, (int)Owner.position.Y, pWidth, pHeight);
                if (Projectile.Hitbox.Intersects(mplrHitbox))
                {
                    if(Projectile.Calamity().stealthStrike && isNotClonedReturnProj) 
                    {
                        Projectile.velocity *= -1.3f;
                        Projectile.timeLeft = 600;
                        Projectile.penetrate = 1;
                        Projectile.localNPCHitCooldown = -1;
                        Projectile.netUpdate = true;
                        AttackType = IsStealth;
                    }
                    else
                    {
                        //干掉这个弹幕
                        Projectile.Kill();
                        //减少一层缩进
                        if (isNotClonedReturnProj)
                            return;

                        ReturnDust(); 
                        SoundEngine.PlaySound(Main.rand.NextBool(2)? CISoundMenu.HammerReturnID1 with {Volume = 0.8f} : CISoundMenu.HammerReturnID2 with {Volume = 0.8f}, Projectile.Center);
                    }
                }
            }
        }

        private void DoShooted()
        {
            AttackTimer += 1f;
            if (AttackTimer >= 40f)
            {
                AttackType = IsReturning;
                AttackTimer = 0f;
                Projectile.netUpdate = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(250, 250, 250, 50);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //普攻与潜伏共享的效果: 爆炸
            int boom = ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjExplosion>();
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, boom, (int)(Projectile.damage * 0.25), Projectile.knockBack, Projectile.owner, 0f, 0f);
            Player owner = Main.player[Projectile.owner];
            if(Projectile.Calamity().stealthStrike)
            {
                //潜伏专属效果：大幅度压制的爆炸粒子
                StealthSpawnDust();
                SpawnSparks(hit);
                SoundEngine.PlaySound(UseSound with { Pitch = 8 * 0.05f - 0.05f }, Projectile.Center);
                //潜伏时生成的锤子才会具备挂载属性
                //这个ifSummonClone是纯多余的，但是也不好改了   
                if(ifSummonClone) 
                {
                    SoundEngine.PlaySound(CISoundMenu.HammerSmashID2 with {Volume = 0.8f}, Projectile.Center);
                    SpawnSparks(hit);
                    int pTyoe = ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone>();
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, pTyoe, (int)(Projectile.damage * 1.1f), Projectile.knockBack, Main.myPlayer, 0f, target.whoAmI);
                }
                ifSummonClone = false;
            }
            
            //普攻效果
            if(!Projectile.Calamity().stealthStrike) 
            {
                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
                //只允许普攻的锤子生成原灾的粒子
                if(Projectile.ai[2] != -1f) 
                SpawnDust();
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            //打人只会造成爆炸
            if(!Projectile.Calamity().stealthStrike) 
            {
                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
                if(Projectile.ai[2] != -1f)
                SpawnDust();
            }
        }

        private void ReturnDust()
        {
            //成功返程至玩家手中时生成粒子
            for (int i = 0; i < 36; i++)
            {
                Dust fire = Dust.NewDustPerfect(Projectile.Center, DustID.GemEmerald);
                fire.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitY).RotatedByRandom(0.8f) * new Vector2(4f, 1.25f) * Main.rand.NextFloat(0.9f, 1f);
                fire.velocity = fire.velocity.RotatedBy(Projectile.rotation - MathHelper.PiOver2);
                fire.velocity += Projectile.velocity/4 *(5*0.04f);
                fire.noGravity = true;
                fire.scale = Main.rand.NextFloat(0.6f, 0.9f) * 5;
                fire = Dust.CloneDust(fire);
                fire.velocity = Main.rand.NextVector2Circular(3f, 3f);
                fire.velocity += Projectile.velocity/2*(5*0.04f);
            }
        }

        //潜伏攻击的粒子, 这个已经被压制到一个非常非常低的值了
        private void StealthSpawnDust() 
        {
            for (int i = 0; i < 7; i++)
            {
                int triactisDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 2f);
                Main.dust[triactisDust].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[triactisDust].scale = 0.5f;
                    Main.dust[triactisDust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 7; j++)
            {
                int triactisDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 2f);
                Main.dust[triactisDust2].noGravity = true;
                Main.dust[triactisDust2].velocity *= 5f;
                triactisDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 2f);
                Main.dust[triactisDust2].velocity *= 2f;
            }

        }
        //潜伏造成的攻击特效更多由下面这个火花代替
        private void SpawnSparks(NPC.HitInfo hit)
        {
            //由造成伤害的多少来获取火花的距离偏移
            float getDMGLerp = Utils.GetLerpValue(670f, 2000f, hit.Damage, true); 
            //根据速度直接获得偏移
            float getVelLerp = MathHelper.Lerp(0.08f, 0.2f, getDMGLerp);    
            //没有作用
            getVelLerp *= Main.rand.NextBool().ToDirectionInt() * Main.rand.NextFloat(0.75f, 1.25f); 
            //火花生成的方向, 在速度的正前方(不出意外的话)
            Vector2 splatterDirection = Projectile.velocity * 1.1f; 
            for (int i = 0; i < 15; i++)
            {
                //火花的生命刻
                int getSparkTime = Main.rand.Next(15, 35); 
                //火花的大小
                float getSparkSize = Main.rand.NextFloat(0.7f, Main.rand.NextFloat(3.3f, 5.5f)) + getDMGLerp * 0.85f;
                //火花的颜色
                Color getColor = Color.Lerp(Color.Green, Color.GhostWhite, Main.rand.NextFloat(0.7f)); 
                getColor = Color.Lerp(getColor, Color.ForestGreen, Main.rand.NextFloat());
                //火花的飞行速度
                Vector2 getVelocity = splatterDirection.RotatedByRandom(0.1f) * Main.rand.NextFloat(1f, 1.2f); 
                SparkParticle spark = new SparkParticle(Projectile.Center, getVelocity, false, getSparkTime, getSparkSize, getColor);
                GeneralParticleHandler.SpawnParticle(spark);
            }

        }
        //普攻击中敌人正常生成粒子
        private void SpawnDust()
        {
            for (int i = 0; i < 40; i++)
            {
                int triactisDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 2f);
                Main.dust[triactisDust].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[triactisDust].scale = 0.5f;
                    Main.dust[triactisDust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int j = 0; j < 70; j++)
            {
                int triactisDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 3f);
                Main.dust[triactisDust2].noGravity = true;
                Main.dust[triactisDust2].velocity *= 5f;
                triactisDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextBool() ? DustID.GemEmerald : DustID.Vortex, 0f, 0f, 100, default, 2f);
                Main.dust[triactisDust2].velocity *= 2f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
