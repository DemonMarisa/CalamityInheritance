using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class Crescent : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.timeLeft = 200;
            Projectile.aiStyle = 0;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.knockBack = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(230, 230, 255, Projectile.alpha);
        }

        public override void AI()
        {
            Projectile parent = Main.projectile[(int)Projectile.ai[0]];
            if (!parent.active)
                Projectile.Kill();

            Projectile.rotation += 0.7f;
            Projectile.ai[1]++;

            if (Projectile.ai[1] > 20)
            {
                Projectile.velocity += CalamityUtils.SafeDirectionTo(Projectile,parent.Center,null) * 2.5f;
                if (Projectile.WithinRange(parent.Center, 75f))
                    Projectile.Kill();
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
