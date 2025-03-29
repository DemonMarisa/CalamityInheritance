using CalamityInheritance.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.Content.Projectiles.Typeless
{
    public class ShrineMarbleSwordClone: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override string Texture => "CalamityInheritance/Content/Projectiles/Typeless/ShrineMarbleSword";
        public double rotation = 0;
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
        public override void AI()
        {
            bool ifSummon = Projectile.type == ModContent.ProjectileType<ShrineMarbleSwordClone>();
            Player p = Main.player[Projectile.owner];
            var modPlayer = p.CIMod();
            if (!modPlayer.SMarble)
            {
                Projectile.active = false;
                return;
            }
            if (ifSummon)
            {
                if (p.dead)
                {
                    modPlayer.SMarbleSword = false;
                }
                if (modPlayer.SMarbleSword)
                {
                    Projectile.timeLeft = 2;
                }
            }
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.01f / 255f);
            Vector2 rot = p.Center - Projectile.Center;
            Projectile.rotation = rot.ToRotation() - 1.57f;
            Projectile.Center = p.Center + new Vector2(80, 0).RotatedBy(rotation);
            rotation -= 0.03;
            if (rotation <= 0)
            {
                rotation = 360;
            }
            Projectile.velocity.X = (rot.X > 0f) ? -0.000001f : 0f;
        }
    }
}