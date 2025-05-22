using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class DukeLegendaryRazor: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => "CalamityMod/Projectiles/TornadoProj";
        public bool SkyFall = false; 
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = 3;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }

        public override void AI()
        {
            Player p = Main.player[Projectile.owner];
            Projectile.rotation += Projectile.velocity.X * 0.7f;

            Projectile.alpha -= 100;
            if (Projectile.alpha < 5)
            {
                Projectile.alpha = 5;
            }

            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] > 4f)
            {
                for (int i = 0; i < 3; i++)
                {
                    int blueDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Flare_Blue, 0f, 0f, 100, new Color(53, Main.DiscoG, 255), 2f);
                    Main.dust[blueDust].noGravity = true;
                    Main.dust[blueDust].velocity *= 0f;
                }
     
            }
            //非从天而降的射弹开始不会释放轨迹
            else if(Projectile.ai[0] == 0f)
                TrailLine();
            //彻底发挥轮椅之光
            // 砍了一刀，搜索范围太大会导致打长直boss非常卡
            float homingDist = Main.player[Projectile.owner].CIMod().DukeTier1 ? 1600f : 450f; 
            NPC getTar = CIFunction.FindClosestTarget(Projectile, homingDist, true, false);
            switch (Projectile.ai[0])
            {
                case 0f:
                    if (!p.CIMod().DukeTier1)
                        CIFunction.HomeInOnNPC(Projectile, !Projectile.tileCollide, homingDist, 18f, 20f);
                    else if (getTar != null)
                    {
                        if (Projectile.ai[1] < 30f)
                        {
                            Projectile.ai[1] += 1f;
                            Projectile.velocity *= 0.97f;
                        }
                        else
                        {
                            CIFunction.HomingNPCBetter(Projectile, getTar, homingDist, 18f + Projectile.ai[1] / 10f, 20f);
                        }
                    }
                    break;
                //在没获得T2升级时候海爵剑是不会获得0f以外的Ai的，需注意
                default:
                    break;
            }
        }
        public void TrailLine()
        {
            if (Main.rand.NextBool(2))
            {
                Vector2 trailPos = Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation) * Main.rand.NextFloat(-16f, 16f);
                float trailScale = Main.rand.NextFloat(0.8f, 1.2f);
                Color trailColor = Main.rand.NextBool() ? Color.Aquamarine : Color.SkyBlue;
                Particle trail = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, trailColor);
                GeneralParticleHandler.SpawnParticle(trail);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity) => false;
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Flare_Blue, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 100, new Color(53, Main.DiscoG, 255));
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(53, 236, 255, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
