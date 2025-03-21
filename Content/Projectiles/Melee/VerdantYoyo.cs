using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class VerdantYoyo : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //这里可能无法动态修改，看情况
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = -1f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = Main.zenithWorld? 1800f : 560f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = Main.zenithWorld ? 28f :17f;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = ProjAIStyleID.Yoyo;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.scale = Main.zenithWorld ? 4f : 1.1f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Main.zenithWorld? 1 : 9;
        }

        public override void AI()
        {
            Projectile.velocity *= 1.01f;

            if (Main.rand.NextBool(5))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.CursedTorch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);

            if (!Main.zenithWorld)
                CalamityUtils.MagnetSphereHitscan(Projectile, 600f, 8f, 54f, 5, ProjectileID.CrystalLeafShot, 0.8);
            else
                CalamityUtils.MagnetSphereHitscan(Projectile, 3600f, 24f, 5f, 30, ProjectileID.CrystalLeafShot, 1.8, true);

            if ((Projectile.position - Main.player[Projectile.owner].position).Length() > 3200f) //200 blocks
                Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
