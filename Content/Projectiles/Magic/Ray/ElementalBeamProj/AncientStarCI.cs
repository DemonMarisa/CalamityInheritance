using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray.ElementalBeamProj
{
    public class AncientStarCI : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Magic";
        public ref float Time => ref Projectile.ai[0];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 72;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 360;
            Projectile.tileCollide = false;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Magic;
        }

        private int hitCount = 0;
        public override void AI()
        {
            if (Projectile.timeLeft > 300)
            {
                //曲线变速
                float progress = (360 - Projectile.timeLeft) / 60f;
                Projectile.scale = MathHelper.Clamp(1f - (float)Math.Exp(-5f * progress), 0f, 1f);
            }

            if (Projectile.timeLeft > 200 && Projectile.timeLeft < 320)
            {
                float maxSpeed = 18f;
                float acceleration = 0.04f * 5f;
                float homeInSpeed = MathHelper.Clamp(Projectile.ai[0] += acceleration, 0f, maxSpeed);
                if(hitCount < 1)
                CalamityUtils.HomeInOnNPC(Projectile, true, 1500f, homeInSpeed, 40f);
            }

            if (Main.rand.NextBool(4))
            {
                Dust stardust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.DungeonSpirit, 0f, 0f, 100, default, 1f);
                stardust.position = Projectile.Center;
                stardust.scale += Main.rand.NextFloat(0.5f);
                stardust.noGravity = true;
                stardust.velocity.Y -= 2f;
            }
            if (Main.rand.NextBool(6))
            {
                Dust stardust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BubbleBurst_Blue, 0f, 0f, 100, default, 1f);
                stardust.position = Projectile.Center;
                stardust.scale += Main.rand.NextFloat(0.3f, 1.1f);
                stardust.noGravity = true;
                stardust.velocity *= 0.1f;
            }
            if (Projectile.timeLeft < 120)
            {
                Projectile.velocity *= 0.97f;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hitCount++;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if(Projectile.timeLeft == 360)
            {
                return false;
            }
            Vector2 drawPosition;
            Texture2D starTexture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                float scale = Projectile.scale * MathHelper.Lerp(0.9f, 0.6f, i / (float)Projectile.oldPos.Length) * 0.56f;
                drawPosition = Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition;
                Main.EntitySpriteDraw(starTexture, drawPosition, null, Color.White, 0f, starTexture.Size() * 0.5f, scale, SpriteEffects.None, 0);
            }

            drawPosition = Projectile.Center - Main.screenPosition;
            Main.EntitySpriteDraw(starTexture, drawPosition, null, Color.White, 0f, starTexture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BubbleBurst_Blue, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.DungeonSpirit, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }
    }
}
