using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Weapons;
using CalamityMod;
using CalamityMod.Particles;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class EclipseSpearProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => $"{Generic.WeaponRoute}/Rogue/EclipseSpear";
        public override void SetStaticDefaults()
        {

        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
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
            //普攻保留原灾的飞行轨迹
            if (Main.rand.NextBool(5))
            {
                Vector2 trailPos = Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation) * Main.rand.NextFloat(-16f, 16f);
                float trailScale = Main.rand.NextFloat(0.8f, 1.2f);
                Color trailColor = Main.rand.NextBool() ? Color.Black : Color.DarkOrange;
                Particle eclipseTrail = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, trailColor);
                GeneralParticleHandler.SpawnParticle(eclipseTrail);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => RainDownSomeSpears(target.position);
        public override void OnHitPlayer(Player target, Player.HurtInfo info) => RainDownSomeSpears(target.position);
        //现在我们需要开始从天上降下投矛
        public void RainDownSomeSpears(Vector2 tarPos)
        {
            //普攻的情况下固定生成6个
            int pAmt = 6;
            for (int i = 0; i < pAmt; i++)
            {
                //随机水平位置
                float pSummonPosX = tarPos.X + Main.rand.NextFloat(-200f, 201f);
                //生成的高度
                float pSummonPosY = tarPos.Y - Main.rand.NextFloat(670f, 1080f);
                Vector2 pPos = new (pSummonPosX, pSummonPosY);
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
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), pPos, speed, ModContent.ProjectileType<EclipseSpearSmall>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
                //在那个位置生成粒子
                SpawndDust(pPos);
            }
        }
        public void SpawndDust(Vector2 projPos)
        {
            for (int i = 0; i < 2 ; i++)
            {
                int d = Dust.NewDust(projPos, Projectile.width, Projectile.height, CIDustID.DustAspalt, 0f, 0f, 0, default, 0.5f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 1f;
                d = Dust.NewDust(projPos, Projectile.width, Projectile.height, DustID.HallowedWeapons, 0f, 0f, 100, default, 0.5f);
                Main.dust[d].velocity *= 1f;
                Main.dust[d].noGravity = true;
            }
        }
    }
}