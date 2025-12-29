using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Yoyos
{
    public class OracleYoyoOrb : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        private static int Lifetime = 40;

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = Lifetime;

            Projectile.alpha = 80;

            // Auric orbs never hit the same enemy more than once.
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Vector2 origin = texture.Size() * 0.5f;
            Main.EntitySpriteDraw(texture, drawPosition, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void AI()
        {
            // Produces golden dust while in flight
            int dustType = Main.rand.NextBool(3) ? 244 : 246;
            float scale = 0.8f + Main.rand.NextFloat(0.6f);
            int idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType);
            Main.dust[idx].noGravity = true;
            Main.dust[idx].velocity = Projectile.velocity / 3f;
            Main.dust[idx].scale = scale;

            Projectile.alpha += 4;
            Projectile.velocity *= 0.88f;
        }
    }
}
