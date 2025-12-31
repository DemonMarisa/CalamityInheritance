using CalamityInheritance.Content.Items.Weapons.Rogue.Boomerang;
using CalamityMod;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Rogue.Boomerang
{
    public class SandslasherProj : RogueDamageProj
    {
        public override string Texture => GetInstance<Sandslasher>().Texture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void ExSD()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 16;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;
            Projectile.ai[1] += 1f;
            if (Projectile.ai[0] == 3f)
                Projectile.tileCollide = true;
            if (Projectile.velocity.X < 0f)
            {
                Projectile.velocity.X -= 0.07f;
                if ((Projectile.ai[0] %= 30f) == 0f)
                    Projectile.damage -= (int)(Projectile.velocity.X * 2f);
            }
            else if (Projectile.velocity.X > 0f)
            {
                Projectile.velocity.X += 0.07f;
                if ((Projectile.ai[0] %= 30f) == 0f)
                    Projectile.damage += (int)(Projectile.velocity.X * 2f);
            }
            Projectile.rotation += 0.1f * Projectile.direction + (Projectile.velocity.X / 85);
            if (Projectile.Calamity().stealthStrike && Projectile.ai[1] >= 5f)
            {
                Vector2 speed = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, speed, ProjectileType<DuststormCloud>(), (int)(Projectile.damage * 0.4), 0f, Projectile.owner);
                Projectile.ai[1] = 0;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.UnusedBrown, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 0, default, 1f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}