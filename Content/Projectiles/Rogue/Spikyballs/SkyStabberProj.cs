using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Items.Weapons.Rogue.Spikyballs;
using CalamityInheritance.Content.Projectiles.Typeless.General;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue.Spikyballs
{
    public class SkyStabberProj : CIRogueProj
    {
        public override string Texture => GetInstance<SkyStabber>().Texture;

        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 15;
            Projectile.friendly = true;
            Projectile.DamageType = RogueDamage.Instance;
            Projectile.tileCollide = true;
            Projectile.penetrate = 10;
            Projectile.timeLeft = 1000;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;

            if (Projectile.ai[0] >= 90f)
            {
                Projectile.velocity.X *= 0.98f;
                Projectile.velocity.Y *= 0.98f;
            }
            else
            {
                Projectile.rotation += 0.3f * (float)Projectile.direction;
            }
        }

        // Makes the projectile bounce infinitely, as it stops mid-air anyway.
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X * 0.6f;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y * 0.6f;
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => OnHitEffects(target.Center);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => OnHitEffects(target.Center);

        private void OnHitEffects(Vector2 targetPos)
        {
            if (Projectile.CI().Stealth)
            {
                var source = Projectile.GetSource_FromThis();
                for (int n = 0; n < 4; n++)
                {
                    Projectile feather = CIUtils.ProjectileRain(source, targetPos, 400f, 100f, 500f, 800f, 20f, ModContent.ProjectileType<StickyFeatherAero>(), (int)(Projectile.damage * 0.25), Projectile.knockBack * 0.25f, Projectile.owner);
                    feather.DamageType = RogueDamage.Instance;
                }
            }
        }
    }
}
