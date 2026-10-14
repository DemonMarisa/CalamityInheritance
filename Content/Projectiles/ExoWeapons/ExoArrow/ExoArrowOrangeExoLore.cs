using CalamityInheritance.Assets;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Particles;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoWeapons.ExoArrow
{
    public class ExoArrowOrangeExoLore : CIExoProj
    {
        public override string Texture => CITextureRegister.LaserProj.Path;
        private bool ColorStyle = false;
        private float SizeVariance;
        private float SizeBonus = 2;
        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.arrow = true;

        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0)
            {
                SizeVariance = Main.rand.NextFloat(0.95f, 1.05f);
                ColorStyle = Main.rand.NextBool();
                Projectile.velocity *= 0.7f;
            }
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 25;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            float num55 = 40f;
            float num56 = 1.5f;
            if (Projectile.ai[1] == 0f)
            {
                Projectile.localAI[0] += num56;
                if (Projectile.localAI[0] > num55)
                {
                    Projectile.localAI[0] = num55;
                }
            }
            else
            {
                Projectile.localAI[0] -= num56;
                if (Projectile.localAI[0] <= 0f)
                {
                    Projectile.Kill();
                }
            }
            NPC npc = LAPUtilities.FindClosestTarget(Projectile.Center, 1500f);
            if (npc != null)
            {
                Projectile.extraUpdates = 4;
                Projectile.HomingTarget(npc.Center, 1000f, 16f, 18f);
            }
            else
            {
                Projectile.extraUpdates = 3;
            }
        }

        public override Color? GetAlpha(Color lightColor) => new Color(250, 100, 0, Projectile.alpha);

        public override bool PreDraw(ref Color lightColor) => Projectile.DrawBeam(40f, 1.5f, lightColor);

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIMiracleBlight>(), 300);
        }
        public override void OnKill(int timeLeft)
        {
            Projectile.Resize(188, 188);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 0;
            Projectile.Damage();
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            Vector2 Offset = Main.rand.NextVector2Circular(15, 15);

            SparkleParticle impactParticle = new SparkleParticle(Projectile.Center + Offset, Vector2.Zero, Color.White, ColorStyle ? Color.Orange : Color.OrangeRed, 2.5f * (SizeBonus * 0.3f), 7, 0f, 2f);
            impactParticle.Spawn();

            for (int k = 0; k < 9; k++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, Main.rand.NextBool() ? 262 : 87, new Vector2(4, 4).RotatedByRandom(100) * Main.rand.NextFloat(0.5f, 1.5f));
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(0.7f, 1.25f);
                dust.alpha = 235;
                if (Main.rand.NextBool())
                {
                    Dust dust2 = Dust.NewDustPerfect(Projectile.Center, DustID.SteampunkSteam, new Vector2(3, 3).RotatedByRandom(100) * Main.rand.NextFloat(0.5f, 1.5f));
                    dust2.noGravity = true;
                    dust2.scale = Main.rand.NextFloat(0.8f, 1.5f);
                    dust2.alpha = 70;
                }
            }
            for (int k = 0; k < 3; k++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.FireworksRGB, new Vector2(4, 4).RotatedByRandom(100) * Main.rand.NextFloat(0.5f, 1.5f) + new Vector2(0, -3));
                dust.noGravity = false;
                dust.scale = Main.rand.NextFloat(0.85f, 1f);
                dust.color = ColorStyle ? Color.Orange : Color.OrangeRed;
            }
        }
    }
}
