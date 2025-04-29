using CalamityInheritance.Sounds.Custom;
using CalamityMod.Items.Accessories;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless
{
    public class NanoFlareLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Typeless";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 10;
            Projectile.timeLeft = 60;
        }

        public override void AI()
        {

            for (int i = 0; i < 3; i++)
            {
                int dint = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, 0f, 0f, 0, default, 0.75f);
                Dust dust = Main.dust[dint];
                dust.velocity = Projectile.velocity * Main.rand.NextFloat(-0.3f, 0.3f);
                dust.color = Main.hslToRgb((float)(0.4 + Main.rand.NextDouble() * 0.2), 0.9f, 0.5f);
                dust.color = Color.Lerp(dust.color, Color.White, 0.3f);
                //我们只在发射出去的第一帧播报音效。
                if (i == 0)
                    SoundEngine.PlaySound(RaidersTalisman.StealthHitSound, Projectile.Center);
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 16; i++)
            {
                Vector2 dspeed = new(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-6f, 6f));
                int dint = Dust.NewDust(Projectile.Center, 1, 1, DustID.TerraBlade, dspeed.X, dspeed.Y, 0, default, 0.7f);
                Dust dust = Main.dust[dint];
                dust.color = Main.hslToRgb((float)(0.4 + Main.rand.NextDouble() * 0.2), 0.9f, 0.5f);
                dust.color = Color.Lerp(dust.color, Color.White, 0.3f);
            }

            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            Projectile.position.X = Projectile.position.X - Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();

            if (Main.netMode != NetmodeID.Server)
            {
                Vector2 goreVec = new(Projectile.Center.X - 24f, Projectile.Center.Y - 24f);
                float smokeScale = 0.66f;
                for (int i = 0; i < 2; i++)
                {
                    int idx1 = Gore.NewGore(Projectile.GetSource_Death(), goreVec, default, Main.rand.Next(61, 64), 1f);
                    Main.gore[idx1].velocity *= smokeScale;
                    Main.gore[idx1].velocity.X += 1f;
                    Main.gore[idx1].velocity.Y += 1f;
                }
            }
        }
    }
}
