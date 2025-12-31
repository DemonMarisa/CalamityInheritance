using CalamityMod;
using LAP.Assets.TextureRegister;
using log4net.Util;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.Guns
{
    public class ThunderstormShot : GeneralDamageProj
    {
        public override ProjDamageType UseDamageClass => ProjDamageType.Magic;
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.alpha = 255;
            Projectile.timeLeft = 600;
            Projectile.MaxUpdates = 30;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 9f)
            {
                for (int i = 0; i < 5; i++)
                {
                    Dust greenDust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TerraBlade, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1f)];
                    greenDust.velocity = Vector2.Zero;
                    greenDust.position -= Projectile.velocity / 5f * i;
                    greenDust.noGravity = true;
                    greenDust.noLight = true;
                    Dust lightningDust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1f)];
                    lightningDust.velocity = Vector2.Zero;
                    lightningDust.position -= Projectile.velocity / 5f * i;
                    lightningDust.noGravity = true;
                    lightningDust.noLight = true;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            var source = Projectile.GetSource_FromThis();
            SoundEngine.PlaySound(SoundID.Item125, Projectile.Center);
            if (Projectile.ai[1] == 0f)
            {
                for (int n = 0; n < 5; n++)
                {
                    Projectile proj = CalamityUtils.ProjectileRain(source, Projectile.Center, 200f, 100f, 1500f, 1500f, 29f, Type, Projectile.damage, Projectile.knockBack, Projectile.owner);
                    proj.ai[1] = 1f;
                    proj.MaxUpdates = 20;
                    proj.tileCollide = false;
                }
            }
            Projectile.ExpandHitboxBy(36);
            int dustAmt = 36;
            for (int j = 0; j < dustAmt; j++)
            {
                Vector2 dustRotate = Projectile.velocity.SafeNormalize(Vector2.Zero) * new Vector2(Projectile.width / 2f, Projectile.height) * 1f; //0.75
                dustRotate = dustRotate.RotatedBy((double)((j - (dustAmt / 2 - 1)) * MathHelper.TwoPi / dustAmt), default) + Projectile.Center;
                Vector2 dustDirection = dustRotate - Projectile.Center;
                int killDust = Dust.NewDust(dustRotate + dustDirection, 0, 0, DustID.Vortex, dustDirection.X, dustDirection.Y, 100, default, 1.2f);
                Main.dust[killDust].noGravity = true;
                Main.dust[killDust].noLight = true;
                Main.dust[killDust].velocity = dustDirection;
            }
            for (int j = 0; j < dustAmt; j++)
            {
                Vector2 dustRotate = Projectile.velocity.SafeNormalize(Vector2.Zero) * new Vector2(Projectile.width / 2f, Projectile.height) * 0.75f;
                dustRotate = dustRotate.RotatedBy((double)((j - (dustAmt / 2 - 1)) * MathHelper.TwoPi / dustAmt), default) + Projectile.Center;
                Vector2 dustDirection = dustRotate - Projectile.Center;
                int killDust = Dust.NewDust(dustRotate + dustDirection, 0, 0, DustID.TerraBlade, dustDirection.X, dustDirection.Y, 100, default, 1.2f);
                Main.dust[killDust].noGravity = true;
                Main.dust[killDust].noLight = true;
                Main.dust[killDust].velocity = dustDirection;
            }
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
        }
    }
}