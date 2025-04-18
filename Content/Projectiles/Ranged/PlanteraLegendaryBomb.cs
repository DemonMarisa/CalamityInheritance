using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class PlanteraLegendaryBomb : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.light = 0.2f;
        }
        public override void AI()
        {
            Projectile.alpha -= 2;
            if (Main.player[Projectile.owner].CIMod().PlanteraTier1)
                Projectile.extraUpdates = 2;
            switch (Projectile.localAI[0])
            {
                case 0f:
                    Projectile.scale += 0.05f;
                    Projectile.localAI[0] = Projectile.scale > 1.2f ? 1f : 0f;
                    break;
                default:
                    Projectile.scale -= 0.05f;
                    Projectile.localAI[0] = Projectile.scale < 0.8f ? 0f : 1f;
                    break;
            }

            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 60f) Projectile.ai[0] = 0f;
            Projectile.velocity.Y = (Projectile.ai[0] >= 20f && Projectile.ai[0] < 40f) ? Projectile.velocity.Y + 0.3f : Projectile.velocity.Y - 0.3f;
            Projectile.velocity.X = (Projectile.ai[0] >= 20f && Projectile.ai[0] < 40f) ? Projectile.velocity.X * 0.98f : Projectile.velocity.X * 1.02f;
            if (Projectile.ai[2] == 1f && Main.rand.NextBool(2))
                CIFunction.HomeInOnNPC(Projectile, false, 1800f, 24f, 20f);
        }
        
         public override Color? GetAlpha(Color lightColor)
        {
            return new Color(Main.DiscoR, 203, 103, Projectile.alpha);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int d = 0; d < 25; d++)
            {
                int index = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ChlorophyteWeapon, 0f, 0f, 0, new Color(Main.DiscoR, 203, 103), 1f);
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity *= 1.5f;
                Main.dust[index].scale = 1.5f;
            }
            int sporeAmt = Main.rand.Next(3, 7);
            if (Projectile.owner == Main.myPlayer)
            {
                for (int s = 0; s < sporeAmt; s++)
                {
                    Vector2 velocity = CalamityUtils.RandomVelocity(100f, 70f, 100f);
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ProjectileID.SporeGas + Main.rand.Next(3), (int)(Projectile.damage * 0.25), 0f, Projectile.owner);
                    if (proj.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[proj].DamageType = DamageClass.Ranged;
                        Main.projectile[proj].usesLocalNPCImmunity = true;
                        Main.projectile[proj].localNPCHitCooldown = 30;
                    }
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