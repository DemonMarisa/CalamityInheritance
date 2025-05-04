using System;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using CalamityInheritance.Content.Items;
using System.IO;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public static readonly SoundStyle UseSound = SoundID.Item89 with { Volume = 0.45f }; //Item89:流星法杖射弹击中时的音效
        private static readonly float RotationIncrement = 0.14f;
        private readonly float stealthSpeed = 32f; //追踪速度24->32f
        private static readonly int Lifetime = 3000;
        private static readonly float canHomingCounter = 100f; //大锤子体积过大，因此开始追踪前飞行的距离应当更长
        public ref int HitCounts => ref Main.player[Projectile.owner].CIMod().HammerCounts;
        public int TargetIndext
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public float HitSpins = 0f;
        public float GetStealth = 0f; //获取潜伏值的缓存

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            Projectile.DoSyncHandlerWrite(ref writer);
            writer.Write(HitCounts);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.DoSyncHandlerRead(ref reader);
            HitCounts = reader.ReadInt32();
        }
        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 160;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 2;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.usesIDStaticNPCImmunity= true;
            Projectile.idStaticNPCHitCooldown = 8;
            Projectile.timeLeft = Lifetime;
            //挂载锤子一定要上这个多人同步
            Projectile.netImportant = true;
        }

        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < (Lifetime - 10) && target.CanBeChasedBy(Projectile);

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            DrawOffsetX = -11;
            DrawOriginOffsetY = -10;
            DrawOriginOffsetX = 0;

            Lighting.AddLight(Projectile.Center, 0.7f, 0.3f, 0.6f);

            //锤子飞行过程中应当有声音
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 60;
                SoundEngine.PlaySound(CISoundID.SoundBoomerangs, Projectile.position);
            }
            Projectile.ai[0] += 1f;


            //使克隆锤子在发起跟踪之前受到重力影响
            //备注：这一段其实纯史山
            float pVelAcceleration = 0.147f;
            if(Projectile.ai[0] < 15f)
            {
                pVelAcceleration = 0.044f;
                Projectile.velocity.X -= 0.001f;
            }
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;
            Projectile.velocity.X *= 0.997f;
            Projectile.velocity.Y += pVelAcceleration;

            //克隆的锤子在飞行过程中会尝试变得更小
            if(Projectile.ai[0] < 30f) 
            Projectile.scale -= 0.07f;
            //使锤子跟踪, 需注意的是, 跟踪有较大的惯性
            if(Projectile.ai[0] > canHomingCounter) 
            {
                Projectile.ai[0] = canHomingCounter;
                CIFunction.HomeInOnNPC(Projectile, true, 3000f, stealthSpeed, 24f, 20f);
            }
            else
            //允许跟踪前会刷新锤子的存续时间
            Projectile.timeLeft = Lifetime; 
            //大锤子的转速比他下位的锤子更慢
            Projectile.rotation += RotationIncrement * 0.5f;

            //克隆锤子飞行过程中才会生成近似于原灾弑神锤飞行的粒子
            if (Main.rand.NextBool() && Projectile.ai[0] < canHomingCounter)
            {
                Vector2 offset = new Vector2(16f, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(8f, 0).RotatedBy(offset.ToRotation());
                float dFlyVelX = Projectile.velocity.X * 0.8f + velOffset.X;
                float dFlyVelY = Projectile.velocity.Y * 0.8f + velOffset.Y;

                //克隆锤子在追踪时生成的粒子速度要更快也更大一点
                dFlyVelX = Projectile.ai[0] == canHomingCounter? dFlyVelX * 1.45f : dFlyVelX;
                dFlyVelY = Projectile.ai[0] == canHomingCounter? dFlyVelY * 1.45f : dFlyVelY;
                offset = Projectile.ai[0] == canHomingCounter? offset * 1.05f : offset;
                float dScale = Projectile.ai[0] == canHomingCounter? 2.4f : 1.2f;
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, DustID.GemEmerald, new Vector2(dFlyVelX, dFlyVelY), 100, default, dScale);
                dust.noGravity = true;
            }

            if (Main.rand.NextBool(6) && Projectile.ai[0] < canHomingCounter)
            {
                Vector2 offset = new Vector2(16f, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(8f, 0).RotatedBy(offset.ToRotation());
                float dFlyVelX = Projectile.velocity.X * 0.7f + velOffset.X;
                float dFlyVelY = Projectile.velocity.Y * 0.7f + velOffset.Y;

                //克隆锤子在追踪时生成的粒子速度更快, 粒子大小更大, 且偏移也会更大一些
                dFlyVelX = Projectile.ai[0] == canHomingCounter? dFlyVelX * 1.35f : dFlyVelX;
                dFlyVelY = Projectile.ai[0] == canHomingCounter? dFlyVelY * 1.35f : dFlyVelY;
                offset = Projectile.ai[0] == canHomingCounter? offset * 1.05f : offset;
                float dScale = Projectile.ai[0] == canHomingCounter? 2.4f : 1.2f;
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, DustID.Vortex, new Vector2(dFlyVelX, dFlyVelY), 100, default, dScale);
                dust.noGravity = true;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(93, 226, 231, 45);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            SoundEngine.PlaySound(UseSound with { Pitch = 8 * 0.05f - 0.05f }, Projectile.Center);
            SpawnDust();
            SpawnExplosion();
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //现在只有生成锤子的时候才会生成一圈圆形的粒子
            SoundEngine.PlaySound(UseSound with { Pitch = 8 * 0.05f - 0.05f }, Projectile.Center);
            SpawnAdditionHammer();
            RestoreRogueStealth(target);
        }
        public override bool PreKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            //TODO:这个原本是用来标记锤子处于挂载状态然后增强星流投矛伤害的
            //但是我试了好几次都失败了，所以看情况哪天要是我会造了就行
            player.CIMod().IfCloneHtting = false; 

            return true;
        }
        public override void OnKill(int timeLeft)
        {
            //kill掉后锤子会尝试收回
            //这个收回的实现方法实际上就是生成一个直接拥有返程AI的新锤子
            //这样除了比较方便一点，而且也可以让他返程时“如果路径上可能的话”造成一些额外伤害, 作为给玩家的一种奖励
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj>(), (int)(4500 * 0.15), Projectile.knockBack, Projectile.owner, 0f, 34f, -1f);
            
        }
        private void SpawnDust()
        {
            CIFunction.DustCircle(Projectile.Center, 48f, Main.rand.NextFloat(1.6f, 1.7f), Main.rand.NextBool(2)?DustID.GemEmerald:DustID.Vortex, true, 15f);
        }
        private void SpawnExplosion()
        {
            Projectile.netUpdate = true;
            if (Projectile.owner == Main.myPlayer)
            {
                int explo = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjExplosion>(), (int)(Projectile.damage * 0.25), Projectile.knockBack, Projectile.owner, 0f, 0f);
                if(explo.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[explo].tileCollide = true; 
                }
            }
        }
        private void RestoreRogueStealth(NPC target)
        {
            Player player = Main.player[Projectile.owner];
            CalamityPlayer modPlayer = player.Calamity();
            if ((target.damage > 5 || target.boss) && !target.SpawnedFromStatue)
            {
                if (modPlayer.wearingRogueArmor && modPlayer.rogueStealthMax != 0 && HitCounts == 1)
                {
                    if (modPlayer.rogueStealth < modPlayer.rogueStealthMax)
                    {
                        float getRestore = modPlayer.rogueStealthMax * 0.25f;
                        if(modPlayer.rogueStealth > modPlayer.rogueStealthMax * 0.5f)
                        getRestore = 0f;
                        modPlayer.rogueStealth += getRestore;
                    }
                }
            }
        }
        //生成锤子
        private void SpawnAdditionHammer()
        {
            int hammerType = ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjEcho>();
            int hammerDamage = (int)(Projectile.damage * 1.4f);
            //Echo的生成角度
            float hammerAngle = 8f; 
            float hammerVelocity = 11f;
            float rotArg = 360f / hammerAngle;
            //将弧度转为角度
            float rotateAngel = MathHelper.ToRadians(rotArg * Main.rand.NextFloat(0f,8f));
            Player player = Main.player[Projectile.owner];
            //给Echo的生成提供一个速度，由于绑定了Rotateby这个旋转，所以不需要额外提供另外一个方向的速度
            Vector2 hammerVelOffset = new Vector2(hammerVelocity, 0f).RotatedBy(rotateAngel);
            //Clone只有造成第三下攻击的时候才会生成一次Echo
            if(Projectile.owner == Main.myPlayer && HitCounts > 2)
            {
                int newHammer = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, hammerVelOffset, hammerType, hammerDamage, Projectile.knockBack, Projectile.owner);
                Main.projectile[newHammer].localAI[0] = Math.Sign(Projectile.velocity.X);
                Main.projectile[newHammer].netUpdate = true;
                //只有Clone生成Echo的时候才会播报落星的提示音
                SoundEngine.PlaySound(SoundID.Item4 with {Volume = 0.3f}, Projectile.Center);
                //生成粒子, 因为这里用了封装，所以一句话就搞定了
                CIFunction.DustCircle(Projectile.Center, 48f, Main.rand.NextFloat(1.8f, 2.1f), 
                                      Main.rand.NextBool(2) ? DustID.GemEmerald : DustID.Vortex, 
                                      true, 11f);
                //Clone的逻辑是两个相反方向的锤子，因此这里是直接复制一遍，然后只取速度反向实现。
                int altHammer = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, -hammerVelOffset, hammerType, hammerDamage, Projectile.knockBack, Projectile.owner);
                Main.projectile[altHammer].localAI[0] = -Math.Sign(Projectile.velocity.X);
                Main.projectile[altHammer].netUpdate = true;
                HitCounts = 0;
            }
            else HitCounts ++;
        }
    }
}