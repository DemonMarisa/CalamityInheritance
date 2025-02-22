using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueTypeHammerTriactisTruePaladinianMageHammerofMightProjClone : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeHammerTriactisTruePaladinianMageHammerofMight";
        public static readonly SoundStyle UseSound = SoundID.Item89 with { Volume = 0.35f }; //Item89:流星法杖射弹击中时的音效
        private static float RotationIncrement = 0.14f;
        private static int Lifetime = 1425;
        private static readonly float canHomingCounter = 100f; //大锤子体积过大，因此开始追踪前飞行的距离应当更长
        private float stealthSpeed = 24f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
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
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
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
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }
            Projectile.ai[0] += 1f;


            //使克隆锤子在发起跟踪之前受到重力影响
            float pVelAcceleration = 0.147f;
            if(Projectile.ai[0] < 15f)
            {
                pVelAcceleration = 0.044f;
                Projectile.velocity.X -= 0.001f;
            }
            Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;
            Projectile.velocity.X *= 0.997f;
            Projectile.velocity.Y += pVelAcceleration;
            if(Projectile.ai[0] < 45f) //大锤子会在飞行过程中缩小一点, 直到ai[0] = 45f为止
            Projectile.scale += 0.01f;

            if(Projectile.ai[0] > canHomingCounter) //使锤子跟踪, 需注意的是, 跟踪有较大的惯性
            {
                Projectile.ai[0] = canHomingCounter;
                CalamityInheritanceUtils.HomeInOnNPC(Projectile, true, 3000f, stealthSpeed, 24f, MathHelper.ToRadians(20f));
            }
            else
            Projectile.timeLeft = Lifetime; //允许跟踪前会刷新锤子的存续时间
            Projectile.rotation += RotationIncrement * 0.5f;//大锤子的旋转增长速度比他下位的锤子更慢

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
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, DustID.GemEmerald, new Vector2(dFlyVelX, dFlyVelY), 100, default, dScale);
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
                Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + offset, DustID.Vortex, new Vector2(dFlyVelX, dFlyVelY), 100, default, dScale);
                dust.noGravity = true;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(250, 250, 250, 50);
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
            SoundEngine.PlaySound(UseSound with { Pitch = 8 * 0.05f - 0.05f }, Projectile.Center);
            SpawnDust(); 
            SpawnExplosion();
        }
        public override void OnKill(int timeLeft)
        {
            //kill掉后锤子会尝试收回
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<RogueTypeHammerTriactisTruePaladinianMageHammerofMightProj>(), (int)(Projectile.damage * 0.25), Projectile.knockBack, Projectile.owner, 0f, 34f, -1f);
        }
        private void SpawnDust()
        {
            //从灾厄上抄下来的, 由于有一些特殊效果所以粒子会少一点
            float numberOfDusts = 32f;
            float rotFactor = 360f / numberOfDusts;
            for (int i = 0; i < numberOfDusts; i++)
            {
                int hDust = Main.rand.NextBool(2)? DustID.GemEmerald:DustID.Vortex;
                float rot = MathHelper.ToRadians(i * rotFactor);
                Vector2 offset = new Vector2(4.8f, 0).RotatedBy(rot * Main.rand.NextFloat(1.1f, 4.1f));
                Vector2 velOffset = new Vector2(4f, 0).RotatedBy(rot * Main.rand.NextFloat(1.1f, 4.1f));
                Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, hDust, new Vector2(velOffset.X, velOffset.Y), 0, default, 0.7f);
                dust.noGravity = false;
                dust.velocity = velOffset;
                dust.scale = Main.rand.NextFloat(1.2f, 2.0f);
            }
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
    }
}
        