﻿using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod;
using Terraria.GameContent.Drawing;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class ShortTerraBeam : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 6;
            Projectile.aiStyle = 27;
            Projectile.scale = Main.zenithWorld ? 2f : 1f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            AIType = 132;
        }
        private int hitCount = 0;
        public override void AI()
        {
            if (Main.rand.NextBool(5))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.TerraBlade, Projectile.velocity.X, Projectile.velocity.Y);
            }
            if (hitCount < 1)
            {
                if (Main.zenithWorld)
                CIFunction.HomeInOnNPC(Projectile, true, 1800f, 20f, 20f);
                else
                CalamityUtils.HomeInOnNPC(Projectile, true, 1000, 16f, 90f);
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 200, 0, 0);
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft >= 359)
                return false;
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hitCount++;

            target.immune[Projectile.owner] = 7;
            // Vanilla has several particles that can easily be used anywhere.
            // The particles from the Particle Orchestra are predefined by vanilla and most can not be customized that much.
            // Use auto complete to see the other ParticleOrchestraType types there are.
            // Here we are spawning the Excalibur particle randomly inside of the target's hitbox.
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TerraBlade,
                new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) },
                Projectile.owner);

            // You could also spawn dusts at the enemy position. Here is simple an example:
            int dustIndex = Dust.NewDust(Main.rand.NextVector2FromRectangle(target.Hitbox), 0, 0, DustID.Terra);
            Main.dust[dustIndex].noGravity = true;

            // Set the target's hit direction to away from the player so the knockback is in the correct direction.
            hit.HitDirection = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            int num3;
            for (int num795 = 4; num795 < 31; num795 = num3 + 1)
            {
                float num796 = Projectile.oldVelocity.X * (30f / num795);
                float num797 = Projectile.oldVelocity.Y * (30f / num795);
                int num798 = Dust.NewDust(new Vector2(Projectile.oldPosition.X - num796, Projectile.oldPosition.Y - num797), 8, 8, DustID.TerraBlade, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.8f);
                Main.dust[num798].noGravity = true;
                Dust dust = Main.dust[num798];
                dust.velocity *= 0.5f;
                num798 = Dust.NewDust(new Vector2(Projectile.oldPosition.X - num796, Projectile.oldPosition.Y - num797), 8, 8, DustID.TerraBlade, Projectile.oldVelocity.X, Projectile.oldVelocity.Y, 100, default, 1.4f);
                dust = Main.dust[num798];
                dust.velocity *= 0.05f;
                num3 = num795;

            }
        }
    }
}
