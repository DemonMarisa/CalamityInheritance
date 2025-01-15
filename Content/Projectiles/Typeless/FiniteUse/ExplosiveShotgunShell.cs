using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.FiniteUse
{
    public class ExplosiveShotgunShell : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Explosive Shotgun Shell");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.light = 0.5f;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 10;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.BulletHighVelocity;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage.Flat += target.lifeMax / 200; // 120 + 30 = 150 + (100000 / 400 = 250) = 400 * 8 = 3200 * 2 (explosion) = 6400 = 6.4% of boss HP
        }//12.8% of boss HP

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 32;
            Projectile.position.X = Projectile.position.X - Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
            for (int num193 = 0; num193 < 2; num193++)
            {
                Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 1.5f);
            }
            for (int num194 = 0; num194 < 20; num194++)
            {
                int num195 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 0, default, 2.5f);
                Main.dust[num195].noGravity = true;
                Main.dust[num195].velocity *= 3f;
                num195 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1.5f);
                Main.dust[num195].velocity *= 2f;
                Main.dust[num195].noGravity = true;
            }
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
        }
    }
}
