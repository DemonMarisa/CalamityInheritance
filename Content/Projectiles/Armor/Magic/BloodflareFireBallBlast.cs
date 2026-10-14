using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Particles;
using LAP.Assets.TextureRegister;
using LAP.Content.Configs;
using LAP.Content.Particles;
using LAP.Core.Presets.Content;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Armor.Magic
{
    public class BloodflareFireBallBlast : CIArmorProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 30;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 45;
            Projectile.netUpdate = true;
        }
        public override void AI()
        {
            float scale = 1;
            if (Projectile.LAP().FirstFrame)
            {
                SoundEngine.PlaySound(CISoundID.SoundFlamethrower, Projectile.Center);
                int glowBallCount = 45;
                for (int j = 0; j < glowBallCount; j++)
                {
                    Color RandomColor = Color.Lerp(Color.Orange, Color.OrangeRed, Main.rand.NextFloat(0, 1));
                    ParticlePreset.NewTGlowBall(Projectile.Center, Vector2.Zero, RandomColor, 180, 0.2f, Main.rand.NextFloat(3f, 4f));
                }
                int strikeCount = 9;
                for (int j = 0; j < strikeCount; j++)
                {
                    float rot = MathHelper.TwoPi / strikeCount;
                    new FireStrike(Projectile.Center, Vector2.Zero, Color.White, 30, 1f, rot * j + Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(0.3f, 0.35f) * scale).SpawnToPriority();
                }
                if (!LAPConfig.Instance.PerformanceMode)
                    new CrossGlow(Projectile.Center, Vector2.Zero, Color.Orange, 60, 1f, 0.4f).Spawn();
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Daybreak, 180);
        }

    }
}
