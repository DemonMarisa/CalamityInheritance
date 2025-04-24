using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueTypeHammerStellarContemptProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";

        #region 射弹属性
        private static float RotationIncrement = 0.22f;
        private static int Lifetime = 240;
        private float stealthSpeed = 25f;
        private static float ReboundTime = 40f;
        bool ifSummonClone = false;
        #endregion

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
            Projectile.timeLeft = Lifetime;
            Projectile.netImportant = true;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            DrawOffsetX = -11;
            DrawOriginOffsetY = -10;
            DrawOriginOffsetX = 0;

            Lighting.AddLight(Projectile.Center, 0.7f, 0.3f, 0.6f);

            // The hammer makes sound while flying.
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 60;
                SoundEngine.PlaySound(CISoundID.SoundBoomerangs, Projectile.position);
            }

            /*
            *25/2/21 Scarlet:
            *ai[0]用来存储锤子的状态，0f说明处于投掷状态, 1f说明处于返程状态
            *激活潜伏攻击时,处于投掷状态的夜明锤子将会在返程的时候尝试采用星神之杀的ai
            */
            switch (Projectile.ai[0])
            {
                case 0f:
                    Projectile.ai[1] += 1f;
                    if (Projectile.ai[1] >= ReboundTime)
                    {
                        Projectile.ai[0] = 1f;
                        Projectile.ai[1] = 0f;
                        Projectile.netUpdate = true;
                    }
                    break;

                case 1f:
                    Projectile.tileCollide = false;
                    float returnSpeed = StellarContempt.Speed;
                    float acceleration = 3.2f;
                    CIFunction.BoomerangReturningAI(owner, Projectile, returnSpeed, acceleration);
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Rectangle projHitbox = new((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                        Rectangle mplrHitbox = new((int)owner.position.X, (int)owner.position.Y, owner.width, owner.height);
                        if (projHitbox.Intersects(mplrHitbox))
                        {
                            //主要是星神之杀的代码
                            if(Projectile.Calamity().stealthStrike)
                            {
                                //这个速度会稍微慢一点
                                Projectile.velocity *= -0.7f;
                                Projectile.timeLeft = 600;
                                Projectile.penetrate = 1;
                                Projectile.localNPCHitCooldown = -1;
                                Projectile.ai[0] = 2f;
                                Projectile.netUpdate = true;
                            }
                            else
                            {
                                Projectile.Kill();
                            }
                        }
                    }
                    break;
                
                case 2f:
                    CIFunction.HomeInOnNPC(Projectile, true, 1250f, stealthSpeed/2, 20f);
                    ifSummonClone = true;
                    break;
                default:
                    break;
            }
            //无论状态，锤子都应当在飞行过程中旋转, 且逐渐加速
            Projectile.rotation += RotationIncrement;

            //同时也会生成粒子
            if (Main.rand.NextBool())
            {
                Vector2 offset = new Vector2(10, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(2, 0).RotatedBy(offset.ToRotation());
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, DustID.Vortex, new Vector2(Projectile.velocity.X * 0.2f + velOffset.X, Projectile.velocity.Y * 0.2f + velOffset.Y), 100, default, 0.8f);
                dust.noGravity = true;
            }

            if (Main.rand.NextBool(5))
            {
                Vector2 offset = new Vector2(12, 0).RotatedByRandom(MathHelper.ToRadians(360f));
                Vector2 velOffset = new Vector2(4, 0).RotatedBy(offset.ToRotation());
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, DustID.Vortex, new Vector2(Projectile.velocity.X * 0.15f + velOffset.X, Projectile.velocity.Y * 0.15f + velOffset.Y), 100, default, 0.8f);
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
            // Some dust gets produced on impact.
            int dustCount = Main.rand.Next(20, 24);
            int dustRadius = 6;
            Vector2 corner = new(target.Center.X - dustRadius, target.Center.Y - dustRadius);
            if(Projectile.ai[0] == 2f)  //从灾厄上抄下来的，只有返程追踪的锤子击中时才会生成这些粒子
            {
                float numberOfDusts = 40f;
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
            }
            else
            {
                for (int i = 0; i < dustCount; ++i)
                {
                    int dustType = 229;
                    float scale = 0.8f + Main.rand.NextFloat(1.1f);
                    int idx = Dust.NewDust(corner, 2 * dustRadius, 2 * dustRadius, dustType);
                    Main.dust[idx].noGravity = true;
                    Main.dust[idx].velocity *= 3f;
                    Main.dust[idx].scale = scale;
                }
            }
            if (!Main.dayTime)
                target.AddBuff(ModContent.BuffType<Nightwither>(), 240);
            
            //潜伏攻击击中敌怪时将会尝试生成再生成一个追踪锤子,这个追踪锤子会造成面板的1.15f伤害
            if(ifSummonClone) 
            {
                int getClone = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.ProjectileType<RogueTypeHammerStellarContemptProjClone>(), (int)(Projectile.damage*1.15f), Projectile.knockBack, Main.myPlayer);
                if(Main.rand.NextBool(3))
                Main.projectile[getClone].Calamity().stealthStrike = true;
                ifSummonClone = false;
            }
            SpawnFlares(target.Center, target.width, target.height);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            // Some dust gets produced on impact.
            int dustCount = Main.rand.Next(20, 24);
            int dustRadius = 6;
            Vector2 corner = new Vector2(target.Center.X - dustRadius, target.Center.Y - dustRadius);

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

            SpawnFlares(target.Center, target.width, target.height);
        }

        private void SpawnFlares(Vector2 targetPos, int width, int height)
        {
            // Play the Lunar Flare sound centered on the user, not the target (consistent with Lunar Flare and Stellar Striker)
            Player user = Main.player[Projectile.owner];
            SoundEngine.PlaySound(SoundID.Item88, Projectile.position);
            Projectile.netUpdate = true;


            int numFlares = 2;
            if(Projectile.ai[0] == 2f || Projectile.ai[2] == -1f)
            numFlares *= numFlares;
            int flareDamage = (int)(0.3f * Projectile.damage);
            float flareKB = 4f;
            for (int i = 0; i < numFlares; ++i)
            {
                float flareSpeed = Main.rand.NextFloat(8f, 11f);

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
                //如果是返程追踪的锤子，落下的月曜射弹的速度将会被2.5f倍率
                if(Projectile.ai[0] == 2f)
                velocity *= 2.5f;

                float AI1 = Main.rand.Next(3);
                if (Projectile.owner == Main.myPlayer)
                {
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), startPoint, velocity, ProjectileID.LunarFlare, flareDamage, flareKB, Main.myPlayer, 0f, AI1);
                    if (proj.WithinBounds(Main.maxProjectiles))
                        Main.projectile[proj].DamageType = ModContent.GetInstance<RogueDamageClass>();
                        Main.projectile[proj].tileCollide = false;
                }
            }
        }
    }
}
