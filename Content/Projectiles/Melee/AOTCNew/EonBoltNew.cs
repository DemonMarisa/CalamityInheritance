using CalamityMod.Particles;
using LAP.Assets.TextureRegister;
using LAP.Content.Particles;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.AOTCNew
{
    public class EonBoltNew : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public ref float Hue => ref Projectile.ai[0];
        public ref float HomeInstrenge => ref Projectile.ai[1];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.CultistIsResistantTo[Type] = true;
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 80 * (Projectile.extraUpdates + 1);
            Projectile.scale = 1.15f;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.LAP().FirstFrame)
                HomeInstrenge = 45f;
            if (HomeInstrenge > 12f)
                HomeInstrenge--;

            Projectile.HomeInNPC(1500, 8f, HomeInstrenge);
            
            Lighting.AddLight(Projectile.Center, 0.75f, 1f, 0.24f);
            for (int i = 0; i < 5; i++)
            {
                Vector2 vel = -Projectile.velocity / 5;
                new SmallGlowBall(Projectile.Center + vel * i, Vector2.Zero, Color.White, 18, 0.09f, 0).Spawn();
            }
            if (Main.rand.NextBool(2))
            {
                Particle smoke = new HeavySmokeParticle(Projectile.Center, Projectile.velocity * 0.5f, Color.Lerp(Color.DodgerBlue, Color.MediumVioletRed, (float)Math.Sin(Main.GlobalTimeWrappedHourly * 6f)), 20, Main.rand.NextFloat(0.6f, 1.2f) * Projectile.scale * 0.6f, 0.28f, 0, false, 0, true);
                GeneralParticleHandler.SpawnParticle(smoke);

                if (Main.rand.NextBool(3))
                {
                    Particle smokeGlow = new HeavySmokeParticle(Projectile.Center, Projectile.velocity * 0.5f, Main.hslToRgb(Hue, 1, 0.7f), 15, Main.rand.NextFloat(0.4f, 0.7f) * Projectile.scale *0.6f, 0.8f, 0, true, 0.05f, true);
                    GeneralParticleHandler.SpawnParticle(smokeGlow);
                }
            }
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overWiresUI.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.Lerp(lightColor, Color.White, 0.5f), Projectile.rotation, texture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            Texture2D spark = LAPTextureRegister.Sparkle.Value;
            Texture2D bloom = LAPTextureRegister.BloomCircle.Value;
            Vector2 drawpos = Projectile.Center - Main.screenPosition + new Vector2(0,12).RotatedBy(Projectile.rotation);
            LAPUtilities.Draw(bloom, drawpos, null, Color.White with { A = 0 } * 0.1f, Main.GlobalTimeWrappedHourly, bloom.Size() / 2, Projectile.scale * 0.55f, 0, 0);
            LAPUtilities.ReSetToBeginShader(BlendState.Additive);
            LAPUtilities.Draw(spark, drawpos, null, Color.White, Main.GlobalTimeWrappedHourly, spark.Size() / 2, Projectile.scale * 1.15f, 0, 0);
            LAPUtilities.Draw(spark, drawpos, null, Color.White, Main.GlobalTimeWrappedHourly, spark.Size() / 2, Projectile.scale * 1.15f, 0, 0);
            LAPUtilities.ReSetToEndShader();
            return false;
        }
    }
}
