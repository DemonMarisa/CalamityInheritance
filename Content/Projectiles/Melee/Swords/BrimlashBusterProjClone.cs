using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Projectiles.Melee.Swords;
using CalamityInheritance.Core.Misc;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class BrimlashBusterProjClone : CIMeleeProj
    {
        public override string Texture => GetInstance<BrimlashBusterProj>().Texture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.extraUpdates = 1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = ProjAIStyleID.Beam;
            AIType = ProjectileID.LightBeam;
        }

        public override void AI()
        {
            //刷新射弹属性 
            Projectile.penetrate = 1;
            //不管是哪个射弹，都会需要飞行一段时间    
            Projectile.ai[0] += 1f; //依旧是计时器的自增
            //飞行至一段时间后才允许追踪。
            if (Projectile.ai[0] > 30f)
                LAPUtilities.HomeInNPC(Projectile, 3000f, 24f, 35f);

            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 0.05f / 255f, (255 - Projectile.alpha) * 0.05f / 255f);
            int num458 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.LifeDrain, 0f, 0f, 100, default, 1f);
            Main.dust[num458].noGravity = true;
            Main.dust[num458].velocity *= 0.5f;
            Main.dust[num458].velocity += Projectile.velocity * 0.1f;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(200, 200, 200, Projectile.alpha);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;
            return true;
        }
        //飞行时的轨迹粒子
        public void TrailDust()
        {
            for (int i = 0; i < 5; i++)
            {
                int d = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, CIDustID.DustBlood, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 200);
                Main.dust[d].noGravity = true;
                Main.dust[d].scale *= 1.5f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft > 295)
                return false;

            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            int num3;
            for (int num795 = 4; num795 < 31; num795 = num3 + 1)
            {
                float num796 = Projectile.oldVelocity.X * (30f / (float)num795);
                float num797 = Projectile.oldVelocity.Y * (30f / (float)num795);
                int num798 = Dust.NewDust(new Vector2(Projectile.oldPosition.X - num796, Projectile.oldPosition.Y - num797), 8, 8, DustID.LifeDrain, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.8f);
                Main.dust[num798].noGravity = true;
                Dust dust = Main.dust[num798];
                dust.velocity *= 0.5f;
                num798 = Dust.NewDust(new Vector2(Projectile.oldPosition.X - num796, Projectile.oldPosition.Y - num797), 8, 8, DustID.LifeDrain, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.4f);
                dust = Main.dust[num798];
                dust.velocity *= 0.05f;
                num3 = num795;
            }
        }
    }
}