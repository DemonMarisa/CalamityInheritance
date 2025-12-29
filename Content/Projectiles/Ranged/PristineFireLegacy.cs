using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using LAP.Assets.TextureRegister;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class PristineFireLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        private int dust1 = (int)CalamityDusts.ProfanedFire;
        private int dust2 = DustType<HolyFireDust>();

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 120;
        }

        public override void AI()
        {
            if (Projectile.scale <= 1.5f)
            {
                Projectile.scale *= 1.01f;
            }
            Lighting.AddLight(Projectile.Center, 1f, 1f, 0.25f);
            if (Projectile.timeLeft > 90)
            {
                Projectile.timeLeft = 120;
            }

            int dustTypeOnTimer = dust1;
            if (Main.zenithWorld)
                CIFunction.HomeInOnNPC(Projectile, true, 1800f, 24f, 20f);
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 5f)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 vector33 = Projectile.position;
                    vector33 -= Projectile.velocity * (i * 0.25f);
                    Projectile.alpha = 255;
                    int dType = Dust.NewDust(vector33, 1, 1, dustTypeOnTimer, 0f, 0f, 0, default, 1f);
                    Main.dust[dType].noGravity = true;
                    Main.dust[dType].position = vector33;
                    Main.dust[dType].scale = Main.rand.Next(70, 110) * 0.013f;
                    Main.dust[dType].velocity *= 0.2f;
                    Main.dust[dType].noLight = true;
                    Main.dust[dType].color = CalamityUtils.ColorSwap(new Color(255, 168, 53), new Color(255, 249, 0), 2f);
                }
                Projectile.ai[0] += 1f;
                if (Projectile.ai[0] == 48f)
                {
                    Projectile.ai[0] = 0f;

                    if (dustTypeOnTimer == dust1)
                        dustTypeOnTimer = dust2;
                    else
                        dustTypeOnTimer = dust1;
                }
                else
                {
                    Vector2 value7 = new Vector2(5f, 10f);

                    for (int j = 0; j < 2; j++)
                    {
                        int dustType = j == 0 ? dust1 : dust2;
                        Vector2 vel = Vector2.UnitX * -12f;
                        vel = -Vector2.UnitY.RotatedBy((double)(Projectile.ai[0] * 0.1308997f + j * 3.14159274f), default) * value7 * 1.5f;
                        int dGet = Dust.NewDust(Projectile.Center, 0, 0, dustType, 0f, 0f, 160, default, 1f);
                        Main.dust[dGet].scale = 0.75f;
                        Main.dust[dGet].noGravity = true;
                        Main.dust[dGet].position = Projectile.Center + vel;
                        Main.dust[dGet].velocity = Projectile.velocity;
                        Main.dust[dGet].noLight = true;
                        Main.dust[dGet].color = CalamityUtils.ColorSwap(new Color(255, 168, 53), new Color(255, 249, 0), 2f);
                    }
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<HolyFlames>(), 240);
        }

        public override void OnKill(int timeLeft)
        {
            int dustType = Utils.SelectRandom(Main.rand,
            [
                dust1,
                dust2
            ]);
            int height = 50;
            float dScale = 1.7f;
            float dScale2 = 0.8f;
            float dScale3 = 2f;
            Vector2 projVel = (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2();
            Vector2 dVel = projVel * Projectile.velocity.Length() * Projectile.MaxUpdates;
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = height;
            Projectile.Center = Projectile.position;
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
            for (int i = 0; i < 40; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, 0f, 0f, 200, default, dScale);
                Dust dust = Main.dust[d];
                dust.position = Projectile.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                dust.noGravity = true;
                dust.velocity *= 3f;
                dust.velocity += dVel * Main.rand.NextFloat();
                dust.color = CalamityUtils.ColorSwap(new Color(255, 168, 53), new Color(255, 249, 0), 2f);
                d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, dScale2);
                dust.position = Projectile.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                dust.velocity *= 2f;
                dust.noGravity = true;
                dust.fadeIn = 1f;
                dust.velocity += dVel * Main.rand.NextFloat();
                dust.color = CalamityUtils.ColorSwap(new Color(255, 168, 53), new Color(255, 249, 0), 2f);
            }
            for (int j = 0; j < 20; j++)
            {
                int d2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, 0f, 0f, 0, default, dScale3);
                Dust dust = Main.dust[d2];
                dust.position = Projectile.Center + Vector2.UnitX.RotatedByRandom(MathHelper.Pi).RotatedBy((double)Projectile.velocity.ToRotation(), default) * Projectile.width / 3f;
                dust.noGravity = true;
                dust.velocity *= 0.5f;
                dust.velocity += dVel * (0.6f + 0.6f * Main.rand.NextFloat());
                dust.color = CalamityUtils.ColorSwap(new Color(255, 168, 53), new Color(255, 249, 0), 2f);
            }
        }
    }
}
