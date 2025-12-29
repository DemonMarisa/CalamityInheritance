using CalamityMod.Buffs.DamageOverTime;
using CalamityMod;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items;
using System.IO;
using System;
using CalamityInheritance.Content.Items.Weapons.Rogue.Boomerang;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueGalaxySmasherProjClone : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => GetInstance<RogueGalaxySmasherProj>().Texture;
        public static readonly SoundStyle UseSound = SoundID.Item89 with { Volume = 0.35f }; //Item89:流星法杖射弹击中时的音效
        private static readonly float RotationIncrement = 0.20f;
        private static readonly int Lifetime = 360;
        private static readonly float canHomingCounter = 65f;
        private readonly float stealthSpeed = 30f;
        public int hitCounter = 30;
        #region 别名
        public ref float AttackTimer => ref Projectile.ai[0];
        public int TargetIndex
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        #endregion
        #region 属性
        public bool TargetAvalible = false;
        #endregion
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 62;
            Projectile.friendly = true;
            Projectile.DamageType = GetInstance<RogueDamageClass>();
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
            Projectile.timeLeft = Lifetime;
            Projectile.netImportant = true;
        }

        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < 300 && target.CanBeChasedBy(Projectile);
        public override void SendExtraAI(BinaryWriter writer)
        {
            Projectile.DoSyncHandlerWrite(ref writer);
            //这个可不会自己同步
            writer.Write(TargetAvalible);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.DoSyncHandlerRead(ref reader);
            TargetAvalible = reader.ReadBoolean();
        }
        public override void AI()
        {
            /*************************************弑神锤子潜伏描述***********************************************
            *弑神锤子潜伏继承夜明锤子潜伏的逻辑, 但追加了一个新的变种效果
            *这一修改发生在克隆弑神锤子(RogueGalaxySmasherProjClone)(下称"克隆")上.
            *克隆继承了夜明锤子的逻辑, 也会滞留在敌怪上不断造成伤害
            *在克隆kill()时, 会以一个非常微弱的初速度生成一个新的弑神锤子(RogueGalaxySmasherProj),下称普通锤子
            *普通锤子在一段时间后会执行类似于星神之杀的ai，即返回至玩家手中再追击敌怪
            *这样就能在视觉上模拟出"克隆滞留结束后收回至玩家手中再掷出"的额外效果
            *普通锤子采用ai[2] = -2f作为标识符, 使他不会再生成一个克隆导致游戏爆破
            *普通锤子由于采用8无敌帧, 导致他会在极短时间内造成极高频率的伤害
            *但是这样也变相模拟出高速旋转的过程, 因此我做了保留, 并使普通锤子的倍率乘了0.2f
            *发射星云射线的功能被限制在普攻与克隆攻击上, 但是, 克隆的星云射线是概率生成, 且被特意降低至一个非常低的值
            ***************************************************************************************************/
            DoGeneral();
            //获取这个NPC
            NPC target = Main.npc[TargetIndex];
            //这个用于决定下方的攻击AI
            TargetAvalible = target != null; 
            AttackTimer += 1f;
            //使克隆锤子在发起跟踪之前受到重力影响
            float pVelAcceleration = 0.147f;
            if(AttackTimer < 15f)
            {
                pVelAcceleration = 0.019f;
                Projectile.velocity.X -= 0.001f;
            }
            Projectile.velocity.X *= 0.997f;
            Projectile.velocity.Y += pVelAcceleration;

            if(AttackTimer > canHomingCounter) //使锤子跟踪, 需注意的是, 跟踪有较大的惯性
            {
                AttackTimer = canHomingCounter;
                CIFunction.HomeInOnNPC(Projectile, true, 3600f, stealthSpeed, 40f, 17f);
            }
            else
            Projectile.timeLeft = Lifetime; //允许跟踪前会刷新锤子的存续时间
        }

        private void DoGeneral()
        {
            DrawOffsetX = -11;
            DrawOriginOffsetY = -10;
            DrawOriginOffsetX = 0;
            Lighting.AddLight(Projectile.Center, 0.7f, 0.3f, 0.6f);
            //全程保朝向与旋转
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;
            //无论状态，锤子都应当在飞行过程中旋转, 但旋转的速度会慢一些
            Projectile.rotation += RotationIncrement;
            //克隆锤子飞行过程中才会生成近似于原灾弑神锤的粒子
            if(AttackTimer < canHomingCounter)
            SpawnFlyingDust();
        }

        private void SpawnFlyingDust()
        {
            if (Main.rand.NextBool())
            {
                Vector2 offset = new Vector2(12, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(4, 0).RotatedBy(offset.ToRotation());
                float dFlyVelX = Projectile.velocity.X * 0.4f + velOffset.X;
                float dFlyVelY = Projectile.velocity.Y * 0.4f + velOffset.Y;
                float dScale = 0.8f;
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, CIDustID.DustWitherLight272, new Vector2(dFlyVelX, dFlyVelY), 100, default, dScale);
                dust.noGravity = true;
            }

            if (Main.rand.NextBool(6))
            {
                Vector2 offset = new Vector2(12, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(4, 0).RotatedBy(offset.ToRotation());
                float dFlyVelX = Projectile.velocity.X * 0.5f + velOffset.X;
                float dFlyVelY = Projectile.velocity.Y * 0.5f + velOffset.Y;
                float dScale =  0.8f;
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, CIDustID.DustDeadlySphere, new Vector2(dFlyVelX, dFlyVelY), 100, default, dScale);
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(UseSound with { Pitch = 8 * 0.05f - 0.05f }, Projectile.Center);

            //从灾厄上抄下来的, 由于有一些特殊效果所以粒子会少一点
            float numberOfDusts = 8f;
            float rotFactor = 360f / numberOfDusts;
            for (int i = 0; i < numberOfDusts; i++)
            {
                int dType = Main.rand.NextBool(2)? CIDustID.DustDeadlySphere : CIDustID.DustWitherLight272;
                float rot = MathHelper.ToRadians(i * rotFactor);
                Vector2 offset = new Vector2(4.8f, 0).RotatedBy(rot * Main.rand.NextFloat(3.1f, 4.1f));
                Vector2 velOffset = new Vector2(4f, 0).RotatedBy(rot * Main.rand.NextFloat(3.1f, 4.1f));
                Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, dType, new Vector2(velOffset.X, velOffset.Y), 0, default, 0.3f);
                dust.noGravity = true;
                dust.velocity = velOffset;
                dust.scale = 1.2f;
            }

            // 固定造成弑神怒焰的debuff
            target.AddBuff(BuffType<GodSlayerInferno>(), 240);
           
            //克隆的锤子以极低的概率才会生成一次星云射线
            if(Main.rand.NextBool(5))
                SpawnNebulaShot(target.Center);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            // Some dust gets produced on impact.
            int dustCount = Main.rand.Next(20, 24);
            int dustRadius = 6;
            Vector2 corner = new(target.Center.X - dustRadius, target.Center.Y - dustRadius);
            for (int i = 0; i < dustCount; ++i)
            {
                int dustType = 229;
                float scale = 0.8f + Main.rand.NextFloat(1.1f);
                int idx = Dust.NewDust(corner, 2 * dustRadius, 2 * dustRadius, dustType);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity *= 3f;
                Main.dust[idx].scale = scale;
            }

            // Applies Nightwither on contact at night.
            if (!Main.dayTime)
                target.AddBuff(BuffType<GodSlayerInferno>(), 240);
            
            SoundEngine.PlaySound(UseSound with { Pitch = 8 * 0.05f - 0.05f }, Projectile.Center);

            SpawnNebulaShot(target.Center);
        }

        private void SpawnNebulaShot(Vector2 targetPos)
        {
            Projectile.netUpdate = true;
            int laserID = ProjectileType<RogueNebulaShot>();
            int laserDamage = Projectile.damage / 2;
            float laserKB = 2.5f;
            int numLasers = 3;
            for (int i = 0; i < numLasers; ++i)
            {
                float startDist = Main.rand.NextFloat(260f, 270f);
                Vector2 startDir = Main.rand.NextVector2Unit();
                Vector2 startPoint = targetPos + startDir * startDist;

                float laserSpeed = Main.rand.NextFloat(15f, 18f);
                Vector2 velocity = startDir * -laserSpeed;

                if (Projectile.owner == Main.myPlayer)
                {
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), startPoint, velocity, laserID, laserDamage, laserKB, Projectile.owner);
                    if (proj.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[proj].DamageType = GetInstance<RogueDamageClass>();
                        Main.projectile[proj].tileCollide = false;
                        Main.projectile[proj].timeLeft = 30;
                    }
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(UseSound with { Pitch = 8 * 0.05f - 0.05f }, Projectile.Center);
            //克隆被kill掉后再返回一个普通弑神锤子, 并只让他进行星神之杀的潜伏攻击模板
            float angles = Main.rand.NextFloat(3f, 56f);
            float anglesRot = 45f / angles;
            float rot = MathHelper.ToRadians(anglesRot);
            Vector2 velOffset = new Vector2(0f, 13f).RotatedBy(rot * Main.rand.NextFloat(1.5f, 2.4f));
            if(Main.rand.NextBool()) velOffset *= -1;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, velOffset, ProjectileType<RogueGalaxySmasherProj>(), (int)(Projectile.damage * 0.7f), Projectile.knockBack, Main.myPlayer, 0f, 0f, -2f);
        }
    }
}