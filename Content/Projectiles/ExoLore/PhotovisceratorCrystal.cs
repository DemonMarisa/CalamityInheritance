using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Projectiles.ExoLore
{
    public class PhotovisceratorCrystal : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";

        public static readonly int[] FrameToDustIDTable = new int[]
        {
            107,
            234,
            CIDustID.DustSandnado,
        };

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.MaxUpdates = 3;
            Projectile.timeLeft = Projectile.MaxUpdates * 180;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.ToRadians(5f);
            Projectile.Opacity = Utils.GetLerpValue(180f, 174f, Projectile.timeLeft, true);

            if(Projectile.timeLeft > Projectile.MaxUpdates * 160)
                Projectile.tileCollide = false;
            else
                Projectile.tileCollide = true;
            if (Projectile.localAI[0] == 0f)
            {
                float initialSpeed = Main.rand.NextFloat(2.5f, 4.5f);
                for (int i = 0; i < 12; i++)
                {
                    Dust cs = Dust.NewDustPerfect(Projectile.Center, 267);
                    cs.velocity = (MathHelper.TwoPi * i / 12f).ToRotationVector2() * initialSpeed * Main.rand.NextFloat(0.6f, 1f);
                    cs.velocity = cs.velocity.RotatedByRandom(0.37f);
                    cs.scale = 1.25f;
                    cs.color = Color.ForestGreen;
                    cs.noGravity = true;
                }
                Projectile.localAI[0] = 1f;
            }
            CIFunction.HomeInOnNPC(Projectile, !Projectile.tileCollide, 4000f, 12f, 0f);
        }

        public override void OnKill(int timeLeft)
        {
            // Play a shatter sound.
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);

            float initialSpeed = Main.rand.NextFloat(2.5f, 4.5f);
            for (int i = 0; i < 16; i++)
            {
                Dust cs = Dust.NewDustPerfect(Projectile.Center, 267);
                cs.velocity = (MathHelper.TwoPi * i / 16f).ToRotationVector2() * initialSpeed;
                cs.scale = 1.25f;
                cs.color = Color.ForestGreen;
                cs.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            int afterimageCount = ProjectileID.Sets.TrailCacheLength[Projectile.type];
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Vector2 origin = texture.Size() * 0.5f;
            for (int i = 0; i < afterimageCount; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    continue;

                float scaleFactor = MathHelper.Lerp(1f, 0.6f, i / (float)(afterimageCount - 1f));
                Color drawColor = Color.Lerp(Color.LightGreen, Color.White, i / (float)(afterimageCount - 1f));
                drawColor.A = (byte)(int)MathHelper.Lerp(105f, 0f, i / (float)(afterimageCount - 1f));
                drawPosition -= Projectile.velocity.SafeNormalize(Vector2.Zero) * scaleFactor * 4.5f;
                Main.EntitySpriteDraw(texture, drawPosition, null, drawColor, Projectile.rotation, origin, Projectile.scale * scaleFactor, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}
