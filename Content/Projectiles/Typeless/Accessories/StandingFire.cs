using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using LAP.Assets.TextureRegister;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Typeless.Accessories
{
    public class StandingFire : CITypelessProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 240;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 7;
        }

        public override void AI()
        {
            if (Projectile.velocity.X != Projectile.velocity.X)
            {
                Projectile.velocity.X = Projectile.velocity.X * -0.1f;
            }
            if (Projectile.velocity.X != Projectile.velocity.X)
            {
                Projectile.velocity.X = Projectile.velocity.X * -0.5f;
            }
            if (Projectile.velocity.Y != Projectile.velocity.Y && Projectile.velocity.Y > 1f)
            {
                Projectile.velocity.Y = Projectile.velocity.Y * -0.5f;
            }
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 5f)
            {
                Projectile.ai[0] = 5f;
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X = Projectile.velocity.X * 0.97f;
                    if (Projectile.velocity.X > -0.01 && Projectile.velocity.X < 0.01)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }
                Projectile.velocity.Y = Projectile.velocity.Y + 0.2f;
            }
            Projectile.rotation += Projectile.velocity.X * 0.1f;
            if (Projectile.ai[1] == 0f && Projectile.type >= ProjectileID.GreekFire1 && Projectile.type <= ProjectileID.GreekFire3)
            {
                Projectile.ai[1] = 1f;
                SoundEngine.PlaySound(SoundID.Item13, Projectile.position);
            }
            int brimDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.LifeDrain, 0f, 0f, 100, default, 1f);
            Dust dust = Main.dust[brimDust];
            dust.position.X -= 2f;
            dust.position.Y += 2f;
            dust.scale += Main.rand.Next(50) * 0.01f;
            dust.noGravity = true;
            dust.velocity.Y -= 2f;
            if (Main.rand.NextBool())
            {
                int brimDusty = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.LifeDrain, 0f, 0f, 100, default, 1f);
                Dust dusty = Main.dust[brimDusty];
                dusty.position.X -= 2f;
                dusty.position.Y += 2f;
                dusty.scale += 0.3f + Main.rand.Next(50) * 0.01f;
                dusty.noGravity = true;
                dusty.velocity *= 0.1f;
            }
            if (Projectile.velocity.Y < 0.25 && Projectile.velocity.Y > 0.15)
            {
                Projectile.velocity.X = Projectile.velocity.X * 0.8f;
            }
            Projectile.rotation = -Projectile.velocity.X * 0.05f;
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffType<CIBrimstoneFlames>(), 120);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffType<CIBrimstoneFlames>(), 120);
    }
}
