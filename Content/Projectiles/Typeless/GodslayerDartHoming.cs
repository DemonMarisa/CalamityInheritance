using System;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Typeless
{
    public class GodSlayerDartHoming: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override string Texture => "CalamityInheritance/Content/Projectiles/Typeless/GodSlayerDart";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 46;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            if (Projectile.alpha > 0)
                Projectile.alpha -= 5;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.owner == Main.myPlayer && Projectile.timeLeft <= 3)
            {
                Projectile.tileCollide = false;
                Projectile.ai[1] = 0f;
                Projectile.alpha = 255;
                Projectile.position.X = Projectile.position.X + Projectile.width / 2;
                Projectile.position.Y = Projectile.position.Y + Projectile.height / 2;
                Projectile.width = 100;
                Projectile.height = 100;
                Projectile.position.X = Projectile.position.X - Projectile.width / 2;
                Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
                Projectile.knockBack = 5f;
            }
            else
            {
                if (Projectile.localAI[0] >= 15f)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        float shortXVel = 0f;
                        float shortYVel = 0f;
                        if (i == 1)
                        {
                            shortXVel = Projectile.velocity.X * 0.5f;
                            shortYVel = Projectile.velocity.Y * 0.5f;
                        }
                        int d = Dust.NewDust(new Vector2(Projectile.position.X + 3f + shortXVel, Projectile.position.Y + 3f + shortYVel) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 1f);
                        Main.dust[d].scale *= 1f + Main.rand.Next(5) * 0.1f;
                        Main.dust[d].velocity *= 0.2f;
                        Main.dust[d].noGravity = true;
                        d = Dust.NewDust(new Vector2(Projectile.position.X + 3f + shortXVel, Projectile.position.Y + 3f + shortYVel) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, DustID.Butterfly, 0f, 0f, 100, default, 0.1f);
                        Main.dust[d].fadeIn = 1f + Main.rand.Next(5) * 0.1f;
                        Main.dust[d].velocity *= 0.05f;
                    }
                }
            }
            //这个用于处理追踪逻辑用
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] < 10f)
                Projectile.velocity *= 1.005f;
            
            if (Projectile.localAI[0] >= 10f && Projectile.localAI[0] < 15f)
                //生成时大幅度减速
                Projectile.velocity *= 0.94f;
            if (Projectile.localAI[0] >= 15f)
            {
                //而后发起高速的追踪
                if (Main.rand.NextBool(2))
                CIFunction.HomeInOnNPC(Projectile, true, 1800f, 18f, 20f);
            }
        }

        public override void PostDraw(Color lightColor)
        {
            Vector2 origin = new Vector2(11f, 23f);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>("CalamityMod/Projectiles/Typeless/GodKillerGlow").Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, origin, 1f, SpriteEffects.None, 0);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item89, Projectile.position);
            for (int j = 0; j < 5; j++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Butterfly, 0f, 0f, 100, default, 1.5f);
                Main.dust[dust].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[dust].scale = 0.5f;
                    Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int k = 0; k < 10; k++)
            {
                int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 2f);
                Main.dust[dust2].noGravity = true;
                Main.dust[dust2].velocity *= 5f;
                dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 1.5f);
                Main.dust[dust2].velocity *= 2f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 120);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 120);

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }
    }
}
