using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Typeless.Explosions
{
    public class AbaddonCrit : CITypelessProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public Player Owner => Main.player[Projectile.owner];
        private static float ExplosionRadius = 300f;

        public override void SetDefaults()
        {
            //These shouldn't matter because its circular
            Projectile.width = 300;
            Projectile.height = 300;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            for (int i = 0; i <= 30; i++)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, Main.rand.NextBool(4) ? 218 : DustID.LifeDrain, new Vector2(5, 5).RotatedByRandom(MathHelper.ToRadians(360)) * Main.rand.NextFloat(1.1f, 2.2f), 0, default, Main.rand.NextFloat(2.8f, 3.4f));
                dust.shader = GameShaders.Armor.GetSecondaryShader(player.cFace, player);
                dust.noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIBrimstoneFlames>(), 360);
            SoundEngine.PlaySound(SoundID.Item89 with { Volume = 0.5f, PitchVariance = 0.4f }, Projectile.Center);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => LAPUtilities.CircularHitboxCollision(Projectile.Center, ExplosionRadius, targetHitbox);
        public override bool? CanDamage() => base.CanDamage();
        public override bool? CanCutTiles() => false;
    }
}
