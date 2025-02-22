using CalamityMod.Buffs.DamageOverTime;
using CalamityMod;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueTypeHammerGalaxySmasherProjClone : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeHammerGalaxySmasher";
        public static readonly SoundStyle UseSound = SoundID.Item89 with { Volume = 0.35f }; //Item89:流星法杖射弹击中时的音效
        private static float RotationIncrement = 0.20f;
        private static int Lifetime = 260;
        private static readonly float canHomingCounter = 65f;
        private float stealthSpeed = 27f;
        public int hitCounter = 30;

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
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 18;
            Projectile.timeLeft = Lifetime;
        }

        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < 230 && target.CanBeChasedBy(Projectile);

        public override void AI()
        {

            /*************************************弑神锤子潜伏描述***********************************************
            *弑神锤子潜伏继承夜明锤子潜伏的逻辑, 但追加了一个新的变种效果
            *这一修改发生在克隆弑神锤子(RogueTypeHammerGalaxySmasherProjClone)(下称"克隆")上.
            *克隆继承了夜明锤子的逻辑, 也会滞留在敌怪上不断造成伤害
            *在克隆kill()时, 会以一个非常微弱的初速度生成一个新的弑神锤子(RogueTypeHammerGalaxySmasherProj),下称普通锤子
            *普通锤子在一段时间后会执行类似于星神之杀的ai，即返回至玩家手中再追击敌怪
            *这样就能在视觉上模拟出"克隆滞留结束后收回至玩家手中再掷出"的额外效果
            *普通锤子采用ai[2] = -2f作为标识符, 使他不会再生成一个克隆导致游戏爆破
            *普通锤子由于采用8无敌帧, 导致他会在极短时间内造成极高频率的伤害
            *但是这样也变相模拟出高速旋转的过程, 因此我做了保留, 并使普通锤子的倍率乘了0.2f
            *发射星云射线的功能被限制在普攻与克隆攻击上, 但是, 克隆的星云射线是概率生成, 且被特意降低至一个非常低的值
            ***************************************************************************************************/

            Player owner = Main.player[Projectile.owner];

            DrawOffsetX = -11;
            DrawOriginOffsetY = -10;
            DrawOriginOffsetX = 0;

            Lighting.AddLight(Projectile.Center, 0.7f, 0.3f, 0.6f);

            //锤子飞行过程中应当有声音
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }
            Projectile.ai[0] += 1f;


            //使克隆锤子在发起跟踪之前受到重力影响
            float pVelAcceleration = 0.147f;
            if(Projectile.ai[0] < 15f)
            {
                pVelAcceleration = 0.019f;
                Projectile.velocity.X -= 0.001f;
            }
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;
            Projectile.velocity.X *= 0.997f;
            Projectile.velocity.Y += pVelAcceleration;

            if(Projectile.ai[0] > canHomingCounter) //使锤子跟踪, 需注意的是, 跟踪有较大的惯性
            {
                Projectile.ai[0] = canHomingCounter;
                CalamityInheritanceUtils.HomeInOnNPC(Projectile, true, 12050f, stealthSpeed, 44f, MathHelper.ToRadians(15f));
            }
            else
            Projectile.timeLeft = Lifetime; //允许跟踪前会刷新锤子的存续时间
            Projectile.rotation += RotationIncrement;//无论状态，锤子都应当在飞行过程中旋转, 但旋转的速度会慢一些

            //克隆锤子飞行过程中才会生成近似于原灾弑神锤的粒子
            if (Main.rand.NextBool())
            {
                Vector2 offset = new Vector2(12, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(4, 0).RotatedBy(offset.ToRotation());
                float dFlyVelX = Projectile.velocity.X * 0.4f + velOffset.X;
                float dFlyVelY = Projectile.velocity.Y * 0.4f + velOffset.Y;

                //克隆锤子在追踪时生成的粒子速度要更快也更大一点
                dFlyVelX = Projectile.ai[0] == canHomingCounter? dFlyVelX * 1.25f : dFlyVelX;
                dFlyVelY = Projectile.ai[0] == canHomingCounter? dFlyVelY * 1.25f : dFlyVelY;
                offset = Projectile.ai[0] == canHomingCounter? offset * 1.05f : offset;
                float dScale = Projectile.ai[0] == canHomingCounter? 1.2f : 0.8f;
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, 272, new Vector2(dFlyVelX, dFlyVelY), 100, default, dScale);
                dust.noGravity = true;
            }

            if (Main.rand.NextBool(6))
            {
                Vector2 offset = new Vector2(12, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(4, 0).RotatedBy(offset.ToRotation());
                float dFlyVelX = Projectile.velocity.X * 0.5f + velOffset.X;
                float dFlyVelY = Projectile.velocity.Y * 0.5f + velOffset.Y;

                //克隆锤子在追踪时生成的粒子速度更快, 粒子大小更大, 且偏移也会更大一些
                dFlyVelX = Projectile.ai[0] == canHomingCounter? dFlyVelX * 1.25f : dFlyVelX;
                dFlyVelY = Projectile.ai[0] == canHomingCounter? dFlyVelY * 1.25f : dFlyVelY;
                offset = Projectile.ai[0] == canHomingCounter? offset * 1.05f : offset;
                float dScale = Projectile.ai[0] == canHomingCounter? 1.2f : 0.8f;
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, 226, new Vector2(dFlyVelX, dFlyVelY), 100, default, dScale);
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
                int dType = Main.rand.NextBool(2)? 226 : 272;
                float rot = MathHelper.ToRadians(i * rotFactor);
                Vector2 offset = new Vector2(4.8f, 0).RotatedBy(rot * Main.rand.NextFloat(1.1f, 4.1f));
                Vector2 velOffset = new Vector2(4f, 0).RotatedBy(rot * Main.rand.NextFloat(1.1f, 4.1f));
                Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, dType, new Vector2(velOffset.X, velOffset.Y), 0, default, 0.3f);
                dust.noGravity = true;
                dust.velocity = velOffset;
                dust.scale = Main.rand.NextFloat(1.5f, 3.2f);
            }

            // 固定造成弑神怒焰的debuff
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 240);
           
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
                target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 240);
            
            SoundEngine.PlaySound(UseSound with { Pitch = 8 * 0.05f - 0.05f }, Projectile.Center);

            SpawnNebulaShot(target.Center);
        }

        private void SpawnNebulaShot(Vector2 targetPos)
        {
            Projectile.netUpdate = true;
            // Three death lasers (aka "Nebula Shots") swarm the target.
            int laserID = ModContent.ProjectileType<RogueTypeNebulaShot>();
            int laserDamage = (int)(1.05f * Projectile.damage);
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
                        Main.projectile[proj].DamageType = ModContent.GetInstance<RogueDamageClass>();
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
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity * (-3.8f), ModContent.ProjectileType<RogueTypeHammerGalaxySmasherProj>(), (int)(Projectile.damage*0.2f), Projectile.knockBack, Main.myPlayer, 0f, 0f, -2f);
        }
    }
}