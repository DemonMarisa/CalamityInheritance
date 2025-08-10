using System;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.FutureContent.GalacticStar
{
    public class GalacticaStar : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        private int noTileHitCounter = 90;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.alpha = 100;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(Main.DiscoR, 100, 255, Projectile.alpha);
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[0] == 1f)
                return;

            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 68;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            for (int i = 0; i < 4; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TeleportationPotion, 0f, 0f, 50, default, 1.5f);
            }
            for (int j = 0; j < 20; j++)
            {
                int galaxyDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex, 0f, 0f, 0, default, 2.5f);
                Main.dust[galaxyDust].noGravity = true;
                Main.dust[galaxyDust].velocity *= 3f;
                galaxyDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex, 0f, 0f, 50, default, 1.5f);
                Main.dust[galaxyDust].velocity *= 2f;
                Main.dust[galaxyDust].noGravity = true;
            }
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.DamageType != DamageClass.Ranged)
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 180);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D projTex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 ori = projTex.Size() / 2f;
            Vector2 baseDrawPos = Projectile.Center - Main.screenPosition;
            Color baseColor = Color.White;
            //残影绘制
            int trailingLength = 10;
            for (int i = 0; i < trailingLength; i++)
            {
                Vector2 trrailDrawPos = baseDrawPos - Projectile.velocity * i * 1.1f;
                //使用一个反比函数处理透明度的衰减系数
                float faded = 1 - (i / (float)trailingLength);
                //使用平方增强
                faded = MathF.Pow(faded, 2);
                Color trailColor = baseColor * faded;
                Main.EntitySpriteDraw(projTex, trrailDrawPos, null, trailColor, Projectile.rotation, ori, Projectile.scale, SpriteEffects.None, 0);
            }
            return false;
        }
        public override void AI()
        {
            noTileHitCounter -= 1;
            if (noTileHitCounter == 0)
                Projectile.tileCollide = true;

            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 20 + Main.rand.Next(40);
                if (Main.rand.NextBool(5))
                    SoundEngine.PlaySound(SoundID.Item9 with {MaxInstances = 0}, Projectile.position);
            }

            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] == 18f)
            {
                Projectile.localAI[0] = 0f;
                for (int l = 0; l < 12; l++)
                {
                    Vector2 dustRotate = Vector2.UnitX * (float)-(float)Projectile.width / 2f;
                    dustRotate += -Vector2.UnitY.RotatedBy((double)(l * MathHelper.Pi / 6f), default) * new Vector2(8f, 16f);
                    dustRotate = dustRotate.RotatedBy((double)(Projectile.rotation - MathHelper.PiOver2), default);
                    int galactic = Dust.NewDust(Projectile.Center, 0, 0, Main.rand.NextBool() ? 164 : 229, 0f, 0f, 160, default, 1f);
                    Main.dust[galactic].noGravity = true;
                    Main.dust[galactic].position = Projectile.Center + dustRotate;
                    Main.dust[galactic].velocity = Projectile.velocity * 0.1f;
                    Main.dust[galactic].velocity = Vector2.Normalize(Projectile.Center - Projectile.velocity * 3f - Main.dust[galactic].position) * 1.25f;
                }
            }

            Projectile.alpha -= 15;
            int alphaControl = 150;
            if (Projectile.Center.Y >= Projectile.ai[1])
                alphaControl = 0;
            if (Projectile.alpha < alphaControl)
                Projectile.alpha = alphaControl;

            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

            if (Main.rand.NextBool(16))
            {
                Vector2 rotation = Vector2.UnitX.RotatedByRandom(MathHelper.PiOver2).RotatedBy((double)Projectile.velocity.ToRotation(), default);
                int pinkDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.TeleportationPotion, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 150, default, 1f);
                Main.dust[pinkDust].velocity = rotation * 0.66f;
                Main.dust[pinkDust].position = Projectile.Center + rotation * 12f;
            }

            if (Main.rand.NextBool(48) && Main.netMode != NetmodeID.Server)
            {
                int gored = Gore.NewGore(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f), 16, 1f);
                Main.gore[gored].velocity *= 0.66f;
                Main.gore[gored].velocity += Projectile.velocity * 0.3f;
            }

            if (Projectile.ai[1] == 1f)
            {
                Projectile.light = 0.5f;
                if (Main.rand.NextBool(10))
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 150, default, 1f);
                if (Main.rand.NextBool(20) && Main.netMode != NetmodeID.Server)
                    Gore.NewGore(Projectile.GetSource_FromAI(), Projectile.position, new Vector2(Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f), Main.rand.Next(16, 18), 1f);
            }
        }
    }
}