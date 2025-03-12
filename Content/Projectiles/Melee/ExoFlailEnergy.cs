using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class ExoFlailEnergy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";

        private NPC target;

        private NPC possibleTarget;

        private bool foundTarget;

        private float maxDistance = 450f;

        private float distance;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            _ = Main.player[Projectile.owner];
            Projectile.ai[0] += 1f;
            int type = (Utils.NextBool(Main.rand, 2) ? 107 : 234);
            if (Utils.NextBool(Main.rand, 4))
            {
                type = CIDustID.DustSandnado;
            }
            if (Projectile.ai[0] % 2f == 0f)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, type, 0f, 0f, 100, default(Color), 1.5f);
                Main.dust[d].noGravity = true;
                Dust obj = Main.dust[d];
                obj.velocity *= 0.1f;
                Dust obj2 = Main.dust[d];
                obj2.velocity += Projectile.velocity * 0.8f;
            }
            if (Projectile.ai[0] > 20f)
            {
                for (int i = 0; i < 200; i++)
                {
                    possibleTarget = Main.npc[i];
                    distance = (possibleTarget.Center - Projectile.Center).Length();
                    if (distance < maxDistance && possibleTarget.active && !possibleTarget.dontTakeDamage && !possibleTarget.friendly && possibleTarget.chaseable && possibleTarget.lifeMax > 5 && !possibleTarget.immortal)
                    {
                        target = Main.npc[i];
                        foundTarget = true;
                        maxDistance = (target.Center - Projectile.Center).Length();
                    }
                }
                if (foundTarget)
                {
                    Vector2 value = target.Center - Projectile.Center;
                    value.Normalize();
                    value.X *= 5f;
                    value.Y *= 5f;
                    Projectile projectile = Projectile;
                    projectile.velocity = projectile.velocity + value;
                    if (Projectile.velocity.Length() > 16f)
                    {
                        Projectile.velocity = Utils.SafeNormalize(Projectile.velocity, -Vector2.UnitY) * 16f;
                    }
                    if (!target.active)
                    {
                        foundTarget = false;
                    }
                }
                maxDistance = 450f;
            }
            else
            {
                Projectile projectile2 = Projectile;
                projectile2.velocity = projectile2.velocity * 0.95f;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.ExoDebuffs();
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[0] < 20f)
            {
                return false;
            }
            return null;
        }
    }
}
