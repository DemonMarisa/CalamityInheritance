using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Core.Utils;
using LAP.Content.Projectiles.LifeStealProj;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Ranged.HandGun
{
    public class ClaretCannonBullet : CIRangedProj
    {
        public override void SetStaticDefaults()
        {
            Projectile.SetTrail(10, 0);
        }
        public override void ExSD()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 1;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            AIType = ProjectileID.Bullet;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 0f / 255f, (255 - Projectile.alpha) * 0f / 255f);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return true;
        }
        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 2; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.LifeDrain, Projectile.oldVelocity.X * 0.15f, Projectile.oldVelocity.Y * 0.15f);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int heal = (int)Math.Round(hit.Damage * 0.02);
            if (heal > 100)
                heal = 100;
            Projectile.Owner().SpawnLifeStealProj(target, Projectile.GetSource_FromThis(), ProjectileType<StandardHealProj>(), Projectile.Center, Vector2.Zero, heal);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
