using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Items.Weapons.Rogue.Spear;
using CalamityInheritance.Core.Misc;
using LAP.Content.Particles;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Rogue.Spear
{
    public class EclipseSpearProj : CIRogueProj
    {
        public override string Texture => GetInstance<EclipseSpear>().Texture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = RogueDamage.Instance;
            Projectile.MaxUpdates = 3;
            //重新变成无限穿
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            //不然这也太没手感了
            Projectile.localNPCHitCooldown = 12;
            Projectile.timeLeft = 150 * Projectile.MaxUpdates;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 0.8f, 0.3f);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            EmitSparks();
            LAPUtilities.HomeInNPC(Projectile, 2500f, 12f, 0, 0.15f, !Projectile.tileCollide);
        }

        private void EmitSparks()
        {
            if (Main.rand.NextBool(3))
            {
                Vector2 trailPos = Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation) * Main.rand.NextFloat(-16f, 16f);
                float trailScale = Main.rand.NextFloat(0.8f, 1.2f);
                if (Main.rand.NextBool())
                {
                    SparkParticle eclipseTrail = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, Color.DarkOrange);
                    eclipseTrail.Spawn();
                }
                else
                {
                    SparkParticle eclipseTrai2 = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, Color.Black);
                    eclipseTrai2.SpawnToNonPreMult();
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            OnHitSparks();
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ProjectileType<EclipseStealthBoomLegacy>(), Projectile.damage * 2, Projectile.knockBack * Projectile.damage, Projectile.owner);
            RainDownSomeSpears(target.position);
            SoundEngine.PlaySound(CISounds.EclipseSpearBoom, target.Center);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            OnHitSparks();
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ProjectileType<EclipseStealthBoomLegacy>(), Projectile.damage * 2, Projectile.knockBack * Projectile.damage, Projectile.owner);
            RainDownSomeSpears(target.position);
        }
        public void OnHitSparks()
        {
            int sparkCount = Main.rand.Next(8, 16);
            for (int i = 0; i < sparkCount; i++)
            {
                Vector2 sVel = Projectile.velocity.RotatedByRandom(0.3f) * Main.rand.NextFloat(0.6f, 2f);
                int sLife = Main.rand.Next(23, 50);
                float sScale = Main.rand.NextFloat(1.6f, 2f) * 0.955f;
                Color trailColor = Color.DarkOrange;
                if (Main.rand.NextBool())
                {
                    SparkParticle eclipseTrail = new SparkParticle(sVel, Projectile.velocity * 0.2f, false, sLife, sScale, trailColor);
                    eclipseTrail.Spawn();
                }
                else
                {
                    SparkParticle eclipseTrai2 = new SparkParticle(sVel, Projectile.velocity * 0.2f, false, sLife, sScale, Color.Black);
                    eclipseTrai2.SpawnToNonPreMult();
                }
            }
        }
        // 现在我们需要开始从天上降下投矛
        public void RainDownSomeSpears(Vector2 tarPos)
        {
            //4-7个
            int pAmt = Main.rand.Next(2, 5);
            for (int i = 0; i < pAmt; i++)
            {
                //随机水平位置
                float pSummonPosX = tarPos.X + Main.rand.NextFloat(-200f, 201f);
                //生成的高度
                float pSummonPosY = tarPos.Y - Main.rand.NextFloat(670f, 1080f);
                Vector2 pPos = new(pSummonPosX, pSummonPosY);
                //速度
                Vector2 speed = tarPos - pPos;
                //水平速度一点随机读
                speed.X += Main.rand.NextFloat(-15f, 16f);
                float pSpeed = 24f;
                float tarDist = speed.Length();
                //固定格式
                tarDist = pSpeed / tarDist;
                speed.X *= tarDist;
                speed.Y *= tarDist;
                //生崽
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), pPos, speed, ProjectileType<EclipseSpearSmall>(), Projectile.damage / 5, Projectile.knockBack, Projectile.owner);
                //在那个位置生成粒子
                SpawndDust(pPos);
            }
        }
        public void SpawndDust(Vector2 projPos)
        {
            for (int i = 0; i < 2; i++)
            {
                int d = Dust.NewDust(projPos, Projectile.width, Projectile.height, CIDustID.DustAspalt, 0f, 0f, 0, default, 0.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 1f;
                d = Dust.NewDust(projPos, Projectile.width, Projectile.height, DustID.HallowedWeapons, 0f, 0f, 100, default, 0.5f);
                Main.dust[d].velocity *= 1f;
                Main.dust[d].noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}