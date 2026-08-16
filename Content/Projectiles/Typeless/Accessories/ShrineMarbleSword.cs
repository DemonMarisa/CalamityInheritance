using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.Content.Projectiles.Typeless.Accessories
{
    public class ShrineMarbleSword : CITypelessProj
    {
        public double rotation = 0;
        public bool Filp => Projectile.ai[0] != 0;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 0f;
            Projectile.timeLeft = 18000;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.timeLeft *= 5;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
                return true;
            return OBBvsAABBCheck(targetHitbox, Projectile.Owner().Center, Projectile.Center, 24);
        }
        public override void AI()
        {
            bool ifSummon = Projectile.type == ProjectileType<ShrineMarbleSword>();
            Player p = Main.player[Projectile.owner];
            if (ifSummon)
            {
                if (p.dead)
                {
                    Projectile.Kill();
                }
                else
                {
                    Projectile.timeLeft = 2;
                }
            }
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.01f / 255f);
            Vector2 rot = p.Center - Projectile.Center;
            Projectile.rotation = rot.ToRotation() - 1.57f;
            Projectile.Center = p.Center;
            Projectile.Center = p.Center + new Vector2(80, 0).RotatedBy(rotation);
            if (Filp)
                Projectile.Center = p.Center - new Vector2(80, 0).RotatedBy(rotation);
            rotation += 0.03;
            if (rotation >= 360)
            {
                rotation = 0;
            }
            Projectile.velocity.X = rot.X > 0f ? -0.000001f : 0f;
        }
    }
}