using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Projectiles.Magic;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class PhantasmalFuryProjOld : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.DamageType = Main.zenithWorld ? DamageClass.Ranged : DamageClass.Magic;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
                Projectile.frame = 0;

            if (Projectile.timeLeft % 10 == 0)
            {
                if (Projectile.owner == Main.myPlayer)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<PhantomLegacy>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }

            Lighting.AddLight(Projectile.Center, 0.25f, 0.25f, 0.25f);

            for (int i = 0; i < 3; i++)
            {
                int spectre = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SpectreStaff, 0f, 0f, 100, default, 2f);
                Main.dust[spectre].noGravity = true;
                Main.dust[spectre].velocity *= 0.5f;
                Main.dust[spectre].velocity += Projectile.velocity * 0.1f;
            }

            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();

            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item43, Projectile.Center);
            for (int j = 0; j <= 10; j++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SpectreStaff, 0f, 0f, 100, default, 1f);
            }
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 8; i++)
                {
                    Vector2 velocity = (MathHelper.TwoPi * i / 8f).ToRotationVector2() * 3f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<PhantomLegacy>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
