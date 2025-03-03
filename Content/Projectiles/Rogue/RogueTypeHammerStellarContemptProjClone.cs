using CalamityMod.Buffs.DamageOverTime;
using CalamityMod;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using Microsoft.Build.Evaluation;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueTypeHammerStellarContemptProjClone : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeHammerStellarContempt";

        public static readonly SoundStyle UseSound = SoundID.Item89 with { Volume = 0.35f }; //Item89:流星法杖射弹击中时的音效
        public int addFlares = 1;
        private static readonly float RotationIncrement = 0.20f;
        private static readonly int Lifetime = 340;
        private static readonly float canHomingCounter = 65f;
        private readonly float stealthSpeed = 24f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 45;
            Projectile.timeLeft = Lifetime;
        }

        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < Lifetime -10 && target.CanBeChasedBy(Projectile);

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
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }
            Projectile.ai[0] += 1f;


            //使克隆锤子在发起跟踪之前受到重力影响
            float pVelAcceleration = 0.197f;
            if(Projectile.ai[0] < 15f)
            {
                pVelAcceleration = 0.017f;
                Projectile.velocity.X -= 0.001f;
            }
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;
            Projectile.velocity.X *= 0.997f;
            Projectile.velocity.Y += pVelAcceleration;
            if(Projectile.ai[0] < 45f) //克隆的锤子将会在飞行的过程中增大, 直到ai[0] = 45f为止
            Projectile.scale += 0.01f;

            if(Projectile.ai[0] > canHomingCounter) //使锤子跟踪, 需注意的是, 跟踪有较大的惯性
            {
                Projectile.ai[0] = canHomingCounter;
                CIFunction.HomeInOnNPC(Projectile, true, 12050f, stealthSpeed, 24f, 20f);
                Projectile.ai[2] += 1f;
                if(Projectile.ai[2] == 50f) //这是一个额外的计时器, 仅用来操作月耀弹幕的生成量的
                {
                   Projectile.ai[2] = 0;
                   addFlares += 1; //每次+2
                }
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
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, 229, new Vector2(dFlyVelX, dFlyVelY), 100, default, dScale);
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
            float numberOfDusts = 15f;
            float rotFactor = 360f / numberOfDusts;
            for (int i = 0; i < numberOfDusts; i++)
            {
                float rot = MathHelper.ToRadians(i * rotFactor);
                Vector2 offset = new Vector2(4.8f, 0).RotatedBy(rot * Main.rand.NextFloat(1.1f, 4.1f));
                Vector2 velOffset = new Vector2(4f, 0).RotatedBy(rot * Main.rand.NextFloat(1.1f, 4.1f));
                Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, 229, new Vector2(velOffset.X, velOffset.Y), 0, default, 0.5f);
                dust.noGravity = true;
                dust.velocity = velOffset;
                dust.scale = Main.rand.NextFloat(1.5f, 3.2f);
            }

            // 固定造成Nihtwither的debuff
            target.AddBuff(ModContent.BuffType<Nightwither>(), 240);
            SpawnFlares(target.Center, target.width, target.height);
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
                target.AddBuff(ModContent.BuffType<Nightwither>(), 240);
            
            SoundEngine.PlaySound(UseSound with { Pitch = 8 * 0.05f - 0.05f }, Projectile.Center);

            SpawnFlares(target.Center, target.width, target.height);
        }
        public override bool PreKill(int timeLeft)
        {
            //克隆锤子被击杀之前重置月耀的生成数量, 一般情况下应该不会出问题不过以防万一
            addFlares = 3;
            return true;
        }

        private void SpawnFlares(Vector2 targetPos, int width, int height)
        {
            // Play the Lunar Flare sound centered on the user, not the target (consistent with Lunar Flare and Stellar Striker)
            Player user = Main.player[Projectile.owner];
            Projectile.netUpdate = true;
            int numFlares = addFlares; //每次ai[2]总是等于50f时, 都会增加月耀的弹幕量
            int flareDamage = (int)(0.2f*Projectile.damage) * addFlares; //伤害跑高
            float flareKB = 4f;
            for (int i = 0; i < numFlares; ++i)
            {
                float flareSpeed = Main.rand.NextFloat(9f, 13f);

                // Flares never come from straight up, there is always at least an 80 pixel horizontal offset
                float xDist = Main.rand.NextFloat(80f, 320f) * (Main.rand.NextBool() ? -1f : 1f);
                float yDist = Main.rand.NextFloat(1200f, 1440f);
                Vector2 startPoint = targetPos + new Vector2(xDist, -yDist);

                // The flare is somewhat inaccurate based on the size of the target.
                float xVariance = width / 4f;
                if (xVariance < 8f)
                    xVariance = 8f;
                float yVariance = height / 4f;
                if (yVariance < 8f)
                    yVariance = 8f;
                float xOffset = Main.rand.NextFloat(-xVariance, xVariance);
                float yOffset = Main.rand.NextFloat(-yVariance, yVariance);
                Vector2 offsetTarget = targetPos + new Vector2(xOffset, yOffset);

                // Finalize the velocity vector and make sure it's going at the right speed.
                Vector2 velocity = offsetTarget - startPoint;
                velocity.Normalize();
                velocity *= flareSpeed;

                float AI1 = Main.rand.Next(3);
                //由于锤子会在敌怪身上旋转一段时间造成持续性的伤害, 因此这里生成的月耀射弹将会是取1/2的概率
                if (Projectile.owner == Main.myPlayer)
                {
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), startPoint, velocity, ProjectileID.LunarFlare, flareDamage, flareKB, Main.myPlayer, 0f, AI1);
                    if (proj.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[proj].DamageType = ModContent.GetInstance<RogueDamageClass>();
                        Main.projectile[proj].tileCollide = false;
                    }
                }
            }
        }
    }
}
