using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Core.Utils;
using CalamityMod;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray
{
    public class FabOrbOld : CIMagicProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] % 10 == 0)
            {
                NPC target = LAPUtilities.FindClosestTarget(Projectile.Center, 300f);
                if (target is not null)
                {
                    Vector2 vel = LAPUtilities.GetVector2(Projectile.Center, target.Center) * 6;
                    Projectile.NewProj(ProjectileType<FabBoltOld>(), Projectile.Center, vel, 0.5f);
                }
            }
        }
    }
}
