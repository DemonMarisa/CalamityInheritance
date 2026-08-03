using CalamityInheritance.Content.Particles;
using LAP.Assets.TextureRegister;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Armor.Summon
{
    public class ReaverBeam : ModProjectile, ILocalizedModType
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.extraUpdates = 70;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.friendly = true;
            Projectile.timeLeft = 500;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 500)
            {
                new StrongBloom(Projectile.Center, Vector2.Zero, Color.YellowGreen, 0.2f, 3).Spawn();
                new StrongBloom(Projectile.Center, Vector2.Zero, Color.ForestGreen, 0.13f, 3).Spawn();
            }
            if (Projectile.timeLeft % 3 == 0 && Projectile.timeLeft < 499)
            {
                new LineParticle(Projectile.Center, -Projectile.velocity * 0.05f, false, 6, 2.5f, Color.YellowGreen * 1f).Spawn();
                new LineParticle(Projectile.Center, -Projectile.velocity * 0.05f, false, 6, 1.3f, Color.ForestGreen * 1f).Spawn();
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i <= 4; i++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(4, 4), DustID.Terra, Projectile.velocity * 2 * Main.rand.NextFloat(0.1f, 0.9f));
                dust.scale = Main.rand.NextFloat(0.3f, 0.5f);
                dust.noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 90);
            if (hit.Damage > 1)
                Projectile.Kill();
        }
    }
}
