using CalamityInheritance.Content.Projectiles.Typeless;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Swords
{
    public class BalefulHarvesterProjectileLegacy : GeneralDamageProj
    {
        public override ProjDamageType UseDamageClass => ProjDamageType.Melee;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.CultistIsResistantTo[Type] = true;
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 300;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (Projectile.ai[0] < 0f)
            {
                Projectile.alpha = 0;
            }
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 50;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 4)
                Projectile.frame = 0;
            Projectile.HomeInNPC(1000f, 12f, 20f);
            for (int j = 0; j < 2; j++)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.position.X + 4f, Projectile.position.Y + 4f), Projectile.width - 8, Projectile.height - 8, Main.rand.NextBool() ? 5 : 6, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[dust].position -= Projectile.velocity * 2f;
                Main.dust[dust].noGravity = true;
                Dust expr_7A4A_cp_0_cp_0 = Main.dust[dust];
                expr_7A4A_cp_0_cp_0.velocity.X *= 0.3f;
                Dust expr_7A65_cp_0_cp_0 = Main.dust[dust];
                expr_7A65_cp_0_cp_0.velocity.Y *= 0.3f;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(200, 200, 200, Main.rand.Next(0, 128));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            for (int j = 0; j < 5; j++)
            {
                int deathDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 100, default, 2f);
                Main.dust[deathDust].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[deathDust].scale = 0.5f;
                    Main.dust[deathDust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int k = 0; k < 10; k++)
            {
                int deathDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3f);
                Main.dust[deathDust2].noGravity = true;
                Main.dust[deathDust2].velocity *= 5f;
                deathDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                Main.dust[deathDust2].velocity *= 2f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire3, 240);
            if (Projectile.owner == Main.myPlayer)
            {
                int maxProjectiles = 2;
                for (int k = 0; k < maxProjectiles; k++)
                {
                    Vector2 flareVelocity = Main.rand.NextVector2CircularEdge(3.5f, 3.5f) + Projectile.oldVelocity;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, flareVelocity, ProjectileType<TinyFlareLegacy>(), (int)(Projectile.damage * 0.35), Projectile.knockBack * 0.35f, Main.myPlayer);
                }
            }
        }
    }
}
