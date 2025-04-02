using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
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
            float homingDist = Main.player[Projectile.owner].CIMod().DukeTier2 ? 3200f : 450f; 
            switch (Projectile.ai[0])
            {
                case 0f:
                    CIFunction.HomeInOnNPC(Projectile, !Projectile.tileCollide, homingDist, 18f, 20f);
                    break;
                //在没获得T2升级时候海爵剑是不会获得0f以外的Ai的，需注意
                default:
                    Projectile.ai[1] += 1f;
                    if (Projectile.ai[1] > 45f)
                    CIFunction.HomeInOnNPC(Projectile, !Projectile.tileCollide, homingDist, 18f, 20f);
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var p = Main.player[Projectile.owner];
            //T2海爵剑在击中敌人后会从天上降下一个一穿的台风。
            if (p.CIMod().DukeTier2 && !SkyFall && Projectile.ai[0] == 0f)
            {
                float posX = target.Center.X + Main.rand.NextFloat(-300f, 300f);
                //这里得取玩家头顶, 不然敌对单位在玩家底下的时候直接露馅
                float posY = p.Center.Y - Main.rand.NextFloat(670f, 1000f);
                //如果敌怪在玩家头顶才取敌怪头顶
                if (p.Center.Y > target.Center.Y)
                    posY = target.Center.Y - Main.rand.NextFloat(670f, 1000f);
                //1/2概率修改为底下
                if (Main.rand.NextBool())
                    posY *= -1;
                Vector2 pos = new (posX, posY);
                Vector2 setSpeed = target.Center - pos;
                setSpeed.X += Main.rand.NextFloat(-15f, 16f);
                float dist = setSpeed.Length();
                //固定格式
                dist = 24f / dist;
                setSpeed.X *= dist; 
                setSpeed.Y *= dist; 
                int p1 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), pos, setSpeed * 0.75f, ModContent.ProjectileType<DukeLegendaryRazor>(), Projectile.damage / 2, Projectile.knockBack, p.whoAmI, 1f);
                Main.projectile[p1].tileCollide = false;
                Main.projectile[p1].penetrate = 2;
                SkyFall = true;
            }
            //从天而降的射弹击中敌人后就会有这个轨迹
            if (Projectile.ai[0] == 1f)
            {
                TrailLine();
                //短暂免疫一点事件
                target.immune[Projectile.whoAmI] = 15;
            }
        }
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
