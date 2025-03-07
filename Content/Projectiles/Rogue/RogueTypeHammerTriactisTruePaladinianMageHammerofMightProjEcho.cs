using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using CalamityInheritance.Content.Items;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjEcho: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public float speed = 25f;
        public static readonly float HitRange = 90f;
        public static readonly int LifeTime = 350;
        public static readonly float DefualtRotatoin = 0.22f;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 160;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 3;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = LifeTime;
            Projectile.scale *= 0.5f;
        }

        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < (LifeTime - 80) && target.CanBeChasedBy(Projectile);

        public override void AI()
        {
            Projectile.Calamity().stealthStrike = true;
            Player owner = Main.player[Projectile.owner];
            Lighting.AddLight(Projectile.Center, 0f, 0.5f, 0.75f);

            /********************大锤子EchoAI逻辑**********************
            *Echo只会由Clone生成, 且每次都是同时生成两个, 两个都会飞往相反的方向
            *Echo的AI逻辑近似于原灾从锤子重击(不过我没有参考过他们代码), 但差别更大
            *因为, Clone会以一个非常高的频率不断生成Echo, 因此Echo的行为使用了3eU, 速度更为暴力
            *Echo采用ai[0]的计时器, 在ai[0] < 25f(HitRange - 40f)时, Echo会以1.02的倍率加速, 转角速度会随着计时器的自增而变得更快
            *而后在ai[0]于(25f, 55f)的区间内, 速度倍率0.05f即大幅度的减速, 且转速也会调整的更慢
            *而后在55f~80f的时候, Echo将会停止, 需注意的是, 因为采用了3eU, 这里在视觉上应该仅仅一瞬间而已
            *在80f的位置播放音效与释放提醒粒子, 而后Echo将会以24f的速度重击敌人, 且这个过程也会被计时器增速
            ***********************************************************/
            Projectile.ai[0] += 1f;
            if(Projectile.ai[0] < HitRange - 60f) //Echo在上升过程中速度会一直增快， 旋转速度也一样
            {
                Projectile.velocity.X *=1.02f;
                Projectile.velocity.Y *=1.02f;
                Projectile.rotation += MathHelper.ToRadians(Projectile.ai[0]*0.9f) * Projectile.localAI[0];
            }
            else if(Projectile.ai[0] > HitRange - 60f && Projectile.ai[0] < HitRange - 30f)//echo达到一定距离后, 速度将会不断缩短
            {
                Projectile.velocity.X *= 0.05f;
                Projectile.velocity.Y *= 0.05f;
                Projectile.rotation += MathHelper.ToRadians(Projectile.ai[0]* 0.3f) * Projectile.localAI[0];
            }
            else if(Projectile.ai[0] > HitRange - 30f && Projectile.ai[0] < 5f)
            {
                Projectile.velocity.X = 0f; //Echo达到这个距离后停止加速
                Projectile.velocity.Y = 0f;
                Projectile.rotation += MathHelper.ToRadians(Projectile.ai[0]* 0.1f) * Projectile.localAI[0];
            }
            if(Projectile.ai[0] > HitRange) //只允许Echo在飞行至大于这个距离时重击
            {
                ReturnDust(Projectile);
                CIFunction.HomeInOnNPC(Projectile, true, 1800f, speed + 10f, 0f);
            }
            //无论何时, Echo都应该一直播放飞行的声音
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 60;
                SoundEngine.PlaySound(CISoundID.SoundBoomerangs, Projectile.position);
            }
            Projectile.rotation += DefualtRotatoin;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(152, 245, 249, 50);
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(CISoundMenu.HammerSmashID1 with {Volume = Main.rand.NextBool(2)? 0.75f : 0.90f}, Projectile.position);
            
            SpawnExplosion();
            SpawnSpark(hit);
        }

        private void SpawnSpark(NPC.HitInfo hit)
        {
            float getDMGLerp = Utils.GetLerpValue(670f, 2000f, hit.Damage, true);
            float getVelLerp = MathHelper.Lerp(0.08f, 0.2f, getDMGLerp);
            getVelLerp *= Main.rand.NextBool().ToDirectionInt() * Main.rand.NextFloat(0.75f, 1.25f);
            Vector2 splatterDirection = Projectile.velocity * 1.3f;
            for (int i = 0; i < 15; i++)
            {
                int getSparkTime = Main.rand.Next(10, 20);
                float getSparkSize = Main.rand.NextFloat(0.7f, Main.rand.NextFloat(3.3f, 5.5f)) + getDMGLerp * 0.85f;
                Color getColor = Color.Lerp(Color.Green, Color.Aquamarine, Main.rand.NextFloat(0.7f));
                getColor = Color.Lerp(getColor, Color.ForestGreen, Main.rand.NextFloat());

                Vector2 getVelocity = splatterDirection.RotatedByRandom(0.3f) * Main.rand.NextFloat(1f, 1.2f);
                getVelocity.Y -= 7f;
                SparkParticle spark = new SparkParticle(Projectile.Center, getVelocity, false, getSparkTime, getSparkSize, getColor);
                GeneralParticleHandler.SpawnParticle(spark);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            SpawnDust(DustID.GemSapphire);
        }
    
        private void SpawnExplosion()
        {
            Projectile.netUpdate = true;
            SoundEngine.PlaySound(SoundID.Item88 with {Volume = 0.35f}, Projectile.position);
            if (Projectile.owner == Main.myPlayer)
            {
                int explo = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjExplosion>(), (int)(Projectile.damage * 0.45), Projectile.knockBack, Projectile.owner, 0f, 0f);
                if(explo.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[explo].tileCollide = true; 
                }
            }
        }

        private static void ReturnDust(Projectile projectile)
        {
            Vector2 dustPosition = projectile.Center + Utils.NextVector2Circular(Main.rand, projectile.velocity.X, projectile.velocity.Y) / 2f; 

            for(int i = 0; i < 5 ; i++)
            {
                Dust dust1 = Dust.NewDustPerfect(dustPosition, DustID.GemEmerald);
                Dust dust2 = Dust.NewDustPerfect(dustPosition, DustID.Vortex);
                dust1.velocity = projectile.DirectionFrom(dustPosition);
                dust1.noGravity = true;
                dust1.scale = 1.4f;
                dust2.velocity = projectile.DirectionFrom(dustPosition);
                dust2.noGravity = true;
                dust2.scale = 1.4f;
            }
        }
        //正常击中敌人生成粒子
        private void SignalDust(int dustID)
        {
            CIFunction.DustCircle(Projectile.position, 20f, 2.5f, dustID, true, 6f); //将击中的粒子修改为圆形粒子而非传统爆炸粒子, 大幅度削减其粒子量
        }
        private void SpawnDust(int dustID)
        {
            CIFunction.DustCircle(Projectile.position, 32f, 1.5f, dustID, true, 15f); //将击中的粒子修改为圆形粒子而非传统爆炸粒子, 大幅度削减其粒子量
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
