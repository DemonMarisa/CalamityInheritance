using System;
using System.Collections.Generic;
using CalamityInheritance.Core;
using CalamityInheritance.Texture;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using LAP.Assets.TextureRegister;
using static CalamityInheritance.Utilities.CIFunction;
namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class WhiteFlameLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";

        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 50;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Player projOwner = Main.player[Projectile.owner];
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 0.5f / 255f, (255 - Projectile.alpha) * 0.5f / 255f);
            for (int i = 0; i < 4; i++)
            {
                int dWhite = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemDiamond, 0f, 0f, 100, default, 1.2f);
                Main.dust[dWhite].noGravity = true;
                Main.dust[dWhite].velocity *= 0.5f;
                Main.dust[dWhite].velocity += Projectile.velocity * 0.1f;
                Main.dust[dWhite].scale = 0.8f;
            }
            float homingRange = 1000f;
            Projectile.localAI[0] += 1f;
            if (projOwner.active && !projOwner.dead)
            {
                if (Projectile.Distance(projOwner.Center) > homingRange)
                {
                    Projectile.SafeDirectionTo(projOwner.Center, Vector2.UnitX);
                    return;
                }
                if (Projectile.localAI[0] > 10f)
                    HomeInOnNPC(Projectile, true, 700f, 16f, 20f);
            }
            else
            {
                if (Projectile.timeLeft > 30)
                    Projectile.timeLeft = 30;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item122, Projectile.position);
            int summonCounts = 2;
            if (Projectile.owner == Main.myPlayer)
            {
                for (int j = 0; j < summonCounts; j++)
                {
                    Vector2 speed = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                    while (speed.X == 0f && speed.Y == 0f)
                    {
                        speed = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                    }
                    speed.Normalize();
                    speed *= Main.rand.Next(70, 101) * 0.1f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.oldPosition.X + Projectile.width / 2, Projectile.oldPosition.Y + Projectile.height / 2, speed.X, speed.Y, ProjectileType<WhiteFlameAltLegacy>(), (int)(double)Projectile.damage, 0f, Projectile.owner, 0f, 0f);
                }
            }
            target.AddBuff(BuffType<HolyFlames>(), 360);
        }
    }
}
