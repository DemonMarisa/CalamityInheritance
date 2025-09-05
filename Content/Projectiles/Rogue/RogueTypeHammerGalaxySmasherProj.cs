using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using CalamityMod.Particles;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Content.Items;
using System;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueTypeHammerGalaxySmasherProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        #region 射弹属性
        public static readonly SoundStyle UseSound = SoundID.Item89 with { Volume = 0.35f }; //Item89:流星法杖射弹击中时的音效
        public static readonly SoundStyle StealthOnHitSound = SoundID.Item88 with { Volume = 0.45f }; //Item88:使用流星法杖的音效
        private static readonly float RotationIncrement = 0.22f;
        private static readonly int Lifetime = 240;
        private static readonly float ReboundTime = 40f;
        public const float ClonedProj = -2f;
        public const float IsJustShooted = -3f;
        #endregion
        #region 别名
        public ref float AttackType => ref Projectile.ai[0];
        public ref float AttackTimer => ref Projectile.ai[1];
        public ref float IsCloneSummon => ref Projectile.ai[2];
        public Player Owner => Main.player[Projectile.owner];
        #endregion
        #region 攻击枚举
        public const float IsShooted = 0f;
        public const float IsReturning = 1f;
        public const float IsStealth = 2f;
        #endregion
        bool ifSummonClone = false;

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
            Projectile.timeLeft = Lifetime;

            // Slightly ignores iframes so it can easily hit twice.
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
            Projectile.netImportant = true;
        }

        public override void AI()
        {
            DoGeneral();
            switch(AttackType)
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
        #region AI方法
        private void DoGeneral()
        {
            //无论状态，锤子都应当在飞行过程中旋转
            Projectile.rotation += RotationIncrement;

            DrawOffsetX = -12;
            DrawOriginOffsetY = -5;
            DrawOriginOffsetX = 0;

            // Produces violet dust constantly while in flight. This lights the hammer.
            int numDust = 2;
            for (int i = 0; i < numDust; ++i)
            {
                int dustType = Main.rand.NextBool(6) ? 112 : 173;
                float scale = 0.8f + Main.rand.NextFloat(0.6f);
                int idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity = Vector2.Zero;
                Main.dust[idx].scale = scale;
            }

            // The hammer makes sound while flying.
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 60;
                SoundEngine.PlaySound(CISoundID.SoundBoomerangs, Projectile.position);
            }
        }

        private void DoStealth()
        {
            CIFunction.HomeInOnNPC(Projectile, true, 1250f, 25f / 2, 20f);
            //使其不再进行一次分裂。
            ifSummonClone = IsCloneSummon != ClonedProj;
        }

        public void DoReturning()
        {
            bool isCloned = IsCloneSummon == ClonedProj;
            Projectile.tileCollide = false;
            CIFunction.BoomerangReturningAI(Owner, Projectile, StellarContempt.Speed, 2.4f);
            if (Main.myPlayer == Owner.whoAmI)
            {
                int pWidth  = isCloned ? Owner.width  * 2 : Owner.width;
                int pHEight = isCloned ? Owner.height * 2 : Owner.height;
                Rectangle plrBox = new ((int)Owner.Center.X, (int)Owner.Center.Y, pWidth, pHEight);
                if (Projectile.Hitbox.Intersects(plrBox))
                {
                    //ClonedProj也会直接执行反击的AI
                    if (Projectile.Calamity().stealthStrike || IsCloneSummon == ClonedProj)
                    {
                        AttackType = IsStealth;
                        Projectile.velocity *= -0.7f;
                        Projectile.timeLeft = 600;
                        Projectile.penetrate = 1;
                        Projectile.localNPCHitCooldown = -1;
                        Projectile.netUpdate = true;
                    }
                    else Projectile.Kill();
                }
            }
        }
        public void DoShooted()
        {
            //回旋镖的通用AI
            AttackTimer += 1f;
            if (AttackTimer > ReboundTime - 5)
            {
                //只有收回的锤子才会播报下方的音效与粒子
                if (IsCloneSummon == ClonedProj)
                {
                    //收回并拐弯的时候播放使用落星的声音
                    SoundEngine.PlaySound(SoundID.Item4 with {Volume = 0.4f}, Projectile.position); 
                    //采用与返程时相同的粒子AI
                    ReturnDust();   
                }
                AttackType = IsReturning;
                AttackTimer = 0f;
                Projectile.netUpdate = true;
            }
        }
        #endregion
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 240);
            SpawnDust(target.Center);
            if (ifSummonClone)
                StealthOnHit(target);  //如果允许生成克隆弑神锤子

            else if (IsCloneSummon != ClonedProj && IsCloneSummon != IsJustShooted) 
                OnHitEffect(target.Center); //非滞留过后收回的锤子, 与非由潜伏打出来的锤子才允许发射星云射线

            else if (IsCloneSummon == ClonedProj)
                SpawnSpark(hit); //只会让挂载过的锤子执行这个函数

            else 
                SoundEngine.PlaySound(StealthOnHitSound with { Pitch = 8 * 0.05f - 0.05f }, Projectile.Center); //非挂载过的, 且由潜伏打出来的锤子, 播报这个声音
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 240);
        private static void SpawnDust(Vector2 targetPos)
        {
            // Some dust gets produced on impact.
            int dustSets = Main.rand.Next(5, 8);
            int dustRadius = 6;
            Vector2 corner = new Vector2(targetPos.X - dustRadius, targetPos.Y - dustRadius);
            for (int i = 0; i < dustSets; ++i)
            {
                // Bigger, flying orb dust
                float scaleOrb = 1.2f + Main.rand.NextFloat(1f);
                int orb = Dust.NewDust(corner, 2 * dustRadius, 2 * dustRadius, DustID.Clentaminator_Purple);
                Main.dust[orb].noGravity = true;
                Main.dust[orb].velocity *= 4f;
                Main.dust[orb].scale = scaleOrb;

                // Add six sparkles per flying orb
                for (int j = 0; j < 6; ++j)
                {
                    float scaleSparkle = 0.8f + Main.rand.NextFloat(1.1f);
                    int sparkle = Dust.NewDust(corner, 2 * dustRadius, 2 * dustRadius, DustID.ShadowbeamStaff);
                    Main.dust[sparkle].noGravity = true;
                    float dustSpeed = Main.rand.NextFloat(10f, 18f);
                    Main.dust[sparkle].velocity = Main.rand.NextVector2Unit() * dustSpeed;
                    Main.dust[sparkle].scale = scaleSparkle;
                }
            }
        }
        private void ReturnDust()
        {
            for (int i = 0; i < 30; i++)
            {
                Dust fire = Dust.NewDustPerfect(Projectile.Center, 181);
                fire.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitY).RotatedByRandom(0.8f) * new Vector2(4f, 1.25f) * Main.rand.NextFloat(0.9f, 1f);
                fire.velocity = fire.velocity.RotatedBy(Projectile.rotation - MathHelper.PiOver2);
                fire.velocity += Projectile.velocity/2 * (3* 0.04f);

                fire.noGravity = true;
                fire.scale = Main.rand.NextFloat(0.2f, 0.6f) * 5;
                fire = Dust.CloneDust(fire);
                fire.velocity = Main.rand.NextVector2Circular(3f, 3f);
                fire.velocity += Projectile.velocity/2*(5*0.04f);
            }
        }
        private void SpawnSpark(NPC.HitInfo hit)
        {
            SoundEngine.PlaySound(Main.rand.NextBool()? CISoundMenu.HammerSmashID1 with {Volume = 0.8f} : CISoundMenu.HammerSmashID2 with {Volume = 0.8f}, Projectile.Center);
            float getDMGLerp = Utils.GetLerpValue(670f, 2000f, hit.Damage, true);
            float getVelLerp = MathHelper.Lerp(0.08f, 0.2f, getDMGLerp);
            getVelLerp *= Main.rand.NextBool().ToDirectionInt() * Main.rand.NextFloat(0.75f, 1.25f);
            Vector2 splatterDirection = Projectile.velocity * 1.3f;
            for (int i = 0; i < 15; i++)
            {
                int getSparkTime = Main.rand.Next(10, 20);
                float getSparkSize = Main.rand.NextFloat(0.7f, Main.rand.NextFloat(3.3f, 5.5f)) + getDMGLerp * 0.85f;
                Color getColor = Color.Lerp(Color.Purple, Color.GhostWhite, Main.rand.NextFloat(0.7f));
                getColor = Color.Lerp(getColor, Color.MediumPurple, Main.rand.NextFloat());

                Vector2 getVelocity = splatterDirection.RotatedByRandom(0.3f) * Main.rand.NextFloat(1f, 1.2f);
                getVelocity.Y -= 7f;
                SparkParticle spark = new SparkParticle(Projectile.Center, getVelocity, false, getSparkTime, getSparkSize, getColor);
                GeneralParticleHandler.SpawnParticle(spark);
            }
        }
        private void StealthOnHit(NPC target)
        {
            SoundEngine.PlaySound(UseSound with { Pitch = 8 * 0.05f - 0.05f }, Projectile.Center);
            //潜伏返程击中后生成一个新的克隆锤子,这一克隆锤子将会滞留在敌怪上
            //从灾厄上抄下来的，只有返程追踪的锤子击中时才会生成这些粒子
            float numberOfDusts = 20f;
            float rotFactor = 360f / numberOfDusts;
            for (int i = 0; i < numberOfDusts; i++)
            {
                float rot = MathHelper.ToRadians(i * rotFactor);
                Vector2 offset = new Vector2(4.8f, 0).RotatedBy(rot * Main.rand.NextFloat(1.1f, 4.1f));
                Vector2 velOffset = new Vector2(4f, 0).RotatedBy(rot * Main.rand.NextFloat(1.1f, 4.1f));
                Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, DustID.Vortex, new Vector2(velOffset.X, velOffset.Y));
                dust.noGravity = true;
                dust.velocity = velOffset;
                dust.scale = Main.rand.NextFloat(1.5f, 3.2f);
            }
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.ProjectileType<RogueTypeHammerGalaxySmasherProjClone>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, target.whoAmI);
            //也会生成一个新的锤子, 这一锤子不会再生成新的克隆锤子
            ifSummonClone = false;
        }

        private void OnHitEffect(Vector2 targetPos)
        {
            // Makes an explosion sound.
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            // Three death lasers (aka "Nebula Shots") swarm the target.
            int laserID = ModContent.ProjectileType<RogueTypeNebulaShot>();
            int laserDamage = (int)(0.4f * Projectile.damage);
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
    }
}
