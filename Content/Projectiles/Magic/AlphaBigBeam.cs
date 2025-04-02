using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class AlphaBigBeam : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content,Projectiles.Magic";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 100;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 9f)
            {
                Vector2 projPos = Projectile.position;
                projPos -= Projectile.velocity * 0.25f;
                int particleDust = Dust.NewDust(projPos, 1, 1, DustID.UnusedWhiteBluePurple, 0f, 0f, 0, default, 2.5f);
                Main.dust[particleDust].position = projPos;
                Main.dust[particleDust].velocity *= 0.1f;
            }
        }
    }
}
