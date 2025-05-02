using CalamityMod.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.Utilities;
using Terraria.ID;
using CalamityMod.Projectiles.Ranged;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class DualpoonProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
            Projectile.timeLeft = 900;
            Projectile.Calamity().pointBlankShotDuration = CalamityGlobalProjectile.DefaultPointBlankDuration;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] > 5f)
            {
                Projectile.alpha = 0;
            }
            if (player.dead)
            {
                Projectile.Kill();
                return;
            }
            if (Projectile.ai[0] == 0f)
            {
                Projectile.extraUpdates = 2;
            }
            else
            {
                Projectile.extraUpdates = 3;
            }
            Vector2 halfDist = Projectile.Center;
            float xDist = player.position.X + (float)(player.width / 2) - halfDist.X;
            float yDist = player.position.Y + (float)(player.height / 2) - halfDist.Y;
            float playerDistance = (float)Math.Sqrt((double)(xDist * xDist + yDist * yDist));
            if (Projectile.ai[0] == 0f)
            {
                if (playerDistance > 1200f)
                {
                    Projectile.ai[0] = 1f;
                }
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] > 5f)
                {
                    Projectile.alpha = 0;
                }
                if (Projectile.ai[1] > 8f)
                {
                    Projectile.ai[1] = 8f;
                }
                if (Projectile.ai[1] >= 10f)
                {
                    Projectile.ai[1] = 15f;
                    Projectile.velocity.Y = Projectile.velocity.Y + 0.3f;
                }
            }
            else if (Projectile.ai[0] == 1f)
            {
                Projectile.tileCollide = false;
                Projectile.rotation = (float)Math.Atan2((double)yDist, (double)xDist) - MathHelper.PiOver2;
                float returnSpeed = 20f;
                if (playerDistance < 50f)
                {
                    Projectile.Kill();
                }
                playerDistance = returnSpeed / playerDistance;
                xDist *= playerDistance;
                yDist *= playerDistance;
                Projectile.velocity.X = xDist;
                Projectile.velocity.Y = yDist;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.ai[0] = 1f;
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[0]++;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
