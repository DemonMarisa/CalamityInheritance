using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using LAP.Core.Graphics.DeepGlow;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Armor.Melee
{
    public class GodSlayerDart : CIArmorProj
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        // 行为枚举
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 46;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 360;
        }
        public override bool? CanDamage()
        {
            if (Projectile.timeLeft > 320)
                return false;
            else
                return null;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            DarkAI(Projectile);
        }
        public static void DarkAI(Projectile projectile)
        {
            if (Math.Abs(projectile.velocity.X) >= 2f || Math.Abs(projectile.velocity.Y) >= 2f)
            {
                for (int i = 0; i < 2; i++)
                {
                    float shortXVel = 0f;
                    float shortYVel = 0f;
                    if (i == 1)
                    {
                        shortXVel = projectile.velocity.X * 0.5f;
                        shortYVel = projectile.velocity.Y * 0.5f;
                    }
                    int d = Dust.NewDust(new Vector2(projectile.position.X + 3f + shortXVel, projectile.position.Y + 3f + shortYVel) - projectile.velocity * 0.5f, projectile.width - 8, projectile.height - 8, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 1f);
                    Main.dust[d].scale *= 1f + Main.rand.Next(5) * 0.1f;
                    Main.dust[d].velocity *= 0.2f;
                    Main.dust[d].noGravity = true;
                    d = Dust.NewDust(new Vector2(projectile.position.X + 3f + shortXVel, projectile.position.Y + 3f + shortYVel) - projectile.velocity * 0.5f, projectile.width - 8, projectile.height - 8, DustID.Butterfly, 0f, 0f, 100, default, 0.1f);
                    Main.dust[d].fadeIn = 1f + Main.rand.Next(5) * 0.1f;
                    Main.dust[d].velocity *= 0.05f;
                }
            }

            if (projectile.timeLeft > 320)
            {
                projectile.velocity *= 0.9f;
            }

            if (projectile.timeLeft < 320)
            {
                float maxSpeed = 25f;
                float acceleration = 0.08f * 20f;
                float homeInSpeed = MathHelper.Clamp(projectile.ai[1] += acceleration, 0f, maxSpeed);
                LAPUtilities.HomeInNPC(projectile, 2000f, homeInSpeed, 15f, 8f, !projectile.tileCollide);
                projectile.extraUpdates = 1;
            }
        }

        public override void PostDraw(Color lightColor)
        {
            Vector2 origin = new Vector2(11f, 23f);
            Main.EntitySpriteDraw(Request<Texture2D>($"{Texture}Glow").Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, origin, 1f, SpriteEffects.None, 0);

            DeepGlow.SubmitCustomGlow(() =>
            {
                Vector2 origin = new Vector2(11f, 23f);
                Main.EntitySpriteDraw(Request<Texture2D>($"{Texture}Glow").Value, Projectile.Center - Main.screenPosition, null, Color.Violet, Projectile.rotation, origin, 1f, SpriteEffects.None, 0);
            });
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffType<CIGodSlayerInferno>(), 120);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffType<CIGodSlayerInferno>(), 120);

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }
    }
}
