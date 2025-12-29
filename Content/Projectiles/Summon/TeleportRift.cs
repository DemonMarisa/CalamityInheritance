using CalamityInheritance.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Summon
{
    public class TeleportRift : ModProjectile, ILocalizedModType
    {
        public override string Texture => GenericProjRoute.StarProjRoute;
        public ref float GeneralRotationalOffset => ref Projectile.ai[0];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 170;
            Projectile.scale = 0.25f;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.Opacity = 0f;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 45;
            Projectile.netImportant = true;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            if (Projectile.ai[1] == 0)
            {
                for (int i = 0; i < 35; i++)
                {
                    Dust burstDust = Dust.NewDustPerfect(Projectile.Center, DustID.ShadowbeamStaff);
                    burstDust.velocity = (MathHelper.TwoPi * i / 35f).ToRotationVector2().RotatedByRandom(0.035f) * 12f;
                    burstDust.noGravity = true;

                    burstDust = Dust.NewDustDirect(Projectile.position, 35, 35, DustID.ShadowbeamStaff);
                    burstDust.velocity = Main.rand.NextVector2CircularEdge(6f, 6f);
                    burstDust.scale = Main.rand.NextFloat(1.3f, 1.75f);
                }
                Projectile.ai[1]++;
            }
            if (Projectile.ai[0] == 0f)
                Projectile.ai[0] = 45f;

            GeneralRotationalOffset += MathHelper.Pi / 8f;
            Projectile.Opacity = MathHelper.Clamp(GeneralRotationalOffset, 0f, 1f);

            if (Projectile.Opacity == 1f && Main.rand.NextBool(15))
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.RainbowMk2, 0f, 0f, 100, new Color(150, 100, 255, 255), 1f);
                dust.velocity.X = 0f;
                dust.noGravity = true;
                dust.position = Projectile.Center + Main.rand.NextVector2Unit() * Main.rand.NextFloat(4f, 20f);
                dust.fadeIn = 1f;
                dust.scale = 0.5f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            float currentFade = Utils.GetLerpValue(0f, 5f, Projectile.timeLeft, true) * Utils.GetLerpValue(Projectile.ai[0], Projectile.ai[0] - 5f, Projectile.timeLeft, true);
            currentFade *= (1f + 0.2f * (float)Math.Cos(Main.GlobalTimeWrappedHourly % 30f * MathHelper.Pi * 3f)) * 0.8f;

            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Color baseColor = new Color(150, 100, 255, 255) * Projectile.Opacity;
            baseColor *= 0.5f;
            baseColor.A = 0;
            Color colorA = baseColor;
            Color colorB = baseColor * 0.5f;
            colorA *= currentFade;
            colorB *= currentFade;
            Vector2 origin = texture.Size() / 2f;
            Vector2 scale = new Vector2(3f, 7f) * Projectile.Opacity * Projectile.scale * currentFade;

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(texture, drawPos, null, colorA, MathHelper.PiOver2, origin, scale, spriteEffects, 0);
            Main.EntitySpriteDraw(texture, drawPos, null, colorA, 0f, origin, scale, spriteEffects, 0);
            Main.EntitySpriteDraw(texture, drawPos, null, colorB, MathHelper.PiOver2, origin, scale * 0.8f, spriteEffects, 0);
            Main.EntitySpriteDraw(texture, drawPos, null, colorB, 0f, origin, scale * 0.8f, spriteEffects, 0);

            Main.EntitySpriteDraw(texture, drawPos, null, colorA, MathHelper.PiOver2 + GeneralRotationalOffset * 0.25f, origin, scale, spriteEffects, 0);
            Main.EntitySpriteDraw(texture, drawPos, null, colorA, GeneralRotationalOffset * 0.25f, origin, scale, spriteEffects, 0);
            Main.EntitySpriteDraw(texture, drawPos, null, colorB, MathHelper.PiOver2 + GeneralRotationalOffset * 0.5f, origin, scale * 0.8f, spriteEffects, 0);
            Main.EntitySpriteDraw(texture, drawPos, null, colorB, GeneralRotationalOffset * 0.5f, origin, scale * 0.8f, spriteEffects, 0);

            Main.EntitySpriteDraw(texture, drawPos, null, colorA, MathHelper.PiOver4, origin, scale * 0.6f, spriteEffects, 0);
            Main.EntitySpriteDraw(texture, drawPos, null, colorA, MathHelper.PiOver4 * 3f, origin, scale * 0.6f, spriteEffects, 0);
            Main.EntitySpriteDraw(texture, drawPos, null, colorB, MathHelper.PiOver4, origin, scale * 0.4f, spriteEffects, 0);
            Main.EntitySpriteDraw(texture, drawPos, null, colorB, MathHelper.PiOver4 * 3f, origin, scale * 0.4f, spriteEffects, 0);

            Main.EntitySpriteDraw(texture, drawPos, null, colorA, MathHelper.PiOver4 + GeneralRotationalOffset * 0.75f, origin, scale * 0.6f, spriteEffects, 0);
            Main.EntitySpriteDraw(texture, drawPos, null, colorA, MathHelper.PiOver4 * 3f + GeneralRotationalOffset * 0.75f, origin, scale * 0.6f, spriteEffects, 0);
            Main.EntitySpriteDraw(texture, drawPos, null, colorB, MathHelper.PiOver4 + GeneralRotationalOffset, origin, scale * 0.4f, spriteEffects, 0);
            Main.EntitySpriteDraw(texture, drawPos, null, colorB, MathHelper.PiOver4 * 3f + GeneralRotationalOffset, origin, scale * 0.4f, spriteEffects, 0);

            return false;
        }

        public override bool? CanDamage() => false;
    }
}
