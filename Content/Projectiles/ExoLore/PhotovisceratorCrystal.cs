﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.ExoLore
{
    public class PhotovisceratorCrystal : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";

        public static readonly int[] FrameToDustIDTable = new int[]
        {
            107,
            234,
            269,
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
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.Opacity = Utils.GetLerpValue(180f, 174f, Projectile.timeLeft, true);

            if (Projectile.localAI[0] == 0f)
            {
                // Create a circular puff of green dust.
                float initialSpeed = Main.rand.NextFloat(2.5f, 4.5f);
                for (int i = 0; i < 12; i++)
                {
                    Dust crystalShard = Dust.NewDustPerfect(Projectile.Center, 267);
                    crystalShard.velocity = (MathHelper.TwoPi * i / 12f).ToRotationVector2() * initialSpeed * Main.rand.NextFloat(0.6f, 1f);
                    crystalShard.velocity = crystalShard.velocity.RotatedByRandom(0.37f);
                    crystalShard.scale = 1.25f;
                    crystalShard.color = Color.ForestGreen;
                    crystalShard.noGravity = true;
                }
                Projectile.localAI[0] = 1f;
            }

            CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, 500f, 12f, 8f);
        }

        public override void OnKill(int timeLeft)
        {
            // Play a shatter sound.
            SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);

            // Create a circular puff of green dust.
            float initialSpeed = Main.rand.NextFloat(2.5f, 4.5f);
            for (int i = 0; i < 16; i++)
            {
                Dust crystalShard = Dust.NewDustPerfect(Projectile.Center, 267);
                crystalShard.velocity = (MathHelper.TwoPi * i / 16f).ToRotationVector2() * initialSpeed;
                crystalShard.scale = 1.25f;
                crystalShard.color = Color.ForestGreen;
                crystalShard.noGravity = true;
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
