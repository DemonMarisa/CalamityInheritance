using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoWeapons
{
    internal class MagnomalyAura : CIExoProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        private int radius = 100;

        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            if (Main.projectile.IndexInRange((int)Projectile.ai[0]))
            {
                Projectile parent = Main.projectile[(int)Projectile.ai[0]];
                if (parent.active && parent.type == (int)Projectile.ai[1])
                    Projectile.Center = parent.Center;
                else
                    Projectile.Kill();
            }
            else
            {
                Projectile.Kill();
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => LAPUtilities.CircularHitboxCollision(Projectile.Center, radius, targetHitbox);
    }
}
