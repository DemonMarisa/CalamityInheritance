using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Assets.TextureRegister;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.Staff
{
    public class VisceraBeamLegacy : CIMagicProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public float healAmt = 1f;

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 9;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            healAmt *= 1.25f;
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
            }
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.canGhostHeal || Main.player[Projectile.owner].moonLeech)
                return;

            healAmt *= 1.25f;
            Player player = Main.player[Projectile.owner];
            player.statLife += (int)healAmt;
            player.HealEffect((int)healAmt);
        }
        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 9f)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 projPos = Projectile.position;
                    projPos -= Projectile.velocity * (i * 0.25f);
                    int bloody = Dust.NewDust(projPos, 1, 1, DustID.Blood, 0f, 0f, 0, default, 1f);
                    Main.dust[bloody].position = projPos;
                    Main.dust[bloody].scale = Main.rand.Next(70, 110) * 0.013f;
                    Main.dust[bloody].velocity *= 0.1f;
                }
            }
        }
    }
}
