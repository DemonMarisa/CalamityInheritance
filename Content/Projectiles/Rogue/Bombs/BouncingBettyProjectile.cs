using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Items.Weapons.Rogue.Bombs;
using CalamityInheritance.Content.Projectiles.Rogue.Explosion;
using CalamityInheritance.Content.Projectiles.Typeless.General;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Rogue.Bombs
{
    public class BouncingBettyProjectile : CIRogueProj
    {
        public override string Texture => GetInstance<BouncingBetty>().Texture;
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 3;
            Projectile.DamageType = RogueDamage.Instance;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        private void Explode()
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<BettyExplosion>(), Projectile.damage, 8f, Projectile.owner);
                if (Projectile.CI().Stealth)
                {
                    int projectileCount = 12;
                    for (int i = 0; i < projectileCount; i++)
                    {
                        if (Main.rand.NextBool(2))
                        {
                            Vector2 shrapnelVelocity = (Vector2.UnitY * Main.rand.NextFloat(-12f, -4f)).RotatedByRandom(MathHelper.ToRadians(30f));
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity + shrapnelVelocity, ProjectileType<BouncingBettyShrapnel>(), (int)(Projectile.damage * 0.5f), 3f, Projectile.owner);
                        }
                        else
                        {
                            Vector2 fireVelocity = (Vector2.UnitY * Main.rand.NextFloat(-12f, -4f)).RotatedByRandom(MathHelper.ToRadians(40f));
                            Projectile fire = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity + fireVelocity, ProjectileType<FlameLegacy>(), (int)(Projectile.damage * 0.6f), 1f, Projectile.owner);
                            fire.localNPCHitCooldown = 9;
                            fire.timeLeft = 240;
                        }
                    }
                }
            }
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Point tileCoords = Projectile.Bottom.ToTileCoordinates();
            if (Main.tile[tileCoords.X, tileCoords.Y + 1].HasUnactuatedTile &&
                WorldGen.SolidTile(Main.tile[tileCoords.X, tileCoords.Y + 1]) &&
                Projectile.timeLeft < 575)
            {
                Explode();
                Projectile.Kill();
            }
            else
            {
                Projectile.velocity.Y += 0.4f;
                if (Projectile.velocity.Y > 16f)
                    Projectile.velocity.Y = 16f;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity *= -1f;
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            Explode();
            Projectile.velocity *= -1f;
            Projectile.netUpdate = true;
            Projectile.netSpam = 0;
        }
    }
}
