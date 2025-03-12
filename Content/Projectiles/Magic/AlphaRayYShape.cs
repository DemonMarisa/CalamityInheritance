using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class AlphaRayYShape: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 6;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 80;
        }

        public override void OnKill(int timeLeft)
        {
            int numProj = 2;
            float rotation = MathHelper.ToRadians(20);
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < numProj; i++)
                {
                    Vector2 perturbedSpeed = Projectile.velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numProj - 1)));
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<AlphaRayYShapeAlt>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                }
            }
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 9f)
            {
                Vector2 projPos = Projectile.position;
                projPos -= Projectile.velocity * 0.25f;
                int beamDust = Dust.NewDust(projPos, 1, 1, DustID.UnusedWhiteBluePurple, 0f, 0f, 0, default, 3f);
                Main.dust[beamDust].position = projPos;
                Main.dust[beamDust].velocity *= 0.1f;
            }
        }
    }
}
