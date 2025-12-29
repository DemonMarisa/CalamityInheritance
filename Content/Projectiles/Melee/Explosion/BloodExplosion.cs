using CalamityMod.Buffs.DamageOverTime;
using LAP.Assets.TextureRegister;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Explosion
{
    public class BloodExplosion : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 5;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            if (Projectile.ai[1] > 0f)
                return;

            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int d = 0; d < 4; d++)
            {
                int index = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 100, default, 1.5f);
                Main.dust[index].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[index].scale = 0.5f;
                    Main.dust[index].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int d = 0; d < 6; d++)
            {
                int index = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 100, default, 2f);
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity *= 5f;
                index = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 100, default, 1.5f);
                Main.dust[index].velocity *= 2f;
            }
            Projectile.ai[1]++;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<BurningBlood>(), 60);
        }
    }
}
