using CalamityMod.Dusts;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Summon.SarosPossessionL
{
    public class SarosSunfireLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Summon";
        public Player Owner => Main.player[Projectile.owner];

        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.timeLeft = 600;
            Projectile.light = 1f;

            Projectile.width = Projectile.height = 10;

            Projectile.DamageType = DamageClass.Summon;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            NPC target = LAPUtilities.FindClosestTarget(Projectile.Center, 1500f, true);

            for (int moreDust = 0; moreDust < 4; moreDust++)
            {
                Dust bootlegSprite = Dust.NewDustPerfect(Projectile.Center, (int)CalamityDusts.ProfanedFire);
                bootlegSprite.velocity = Vector2.One.RotatedByRandom(MathHelper.TwoPi);
                bootlegSprite.scale = bootlegSprite.velocity.Length() * 0.25f;
                bootlegSprite.color = Color.Lerp(Color.White, Color.DarkOrange, 0.25f);
                bootlegSprite.noGravity = true;
            }

            // Move towards the target.
            if (target != null)
            {
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 25f, 0.1f);
                Projectile.netUpdate = true;
            }
        }

        public override void OnKill(int timeLeft) // Makes a dust explosion effect on death.
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            for (int i = 0; i < 60; i++)
            {
                Dust dustExplosion = Dust.NewDustPerfect(Projectile.Center, (int)CalamityDusts.ProfanedFire);
                dustExplosion.velocity = Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(2f, 6f);
                dustExplosion.noGravity = true;
            }
        }
    }
}
