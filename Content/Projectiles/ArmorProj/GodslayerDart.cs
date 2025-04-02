using System;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ArmorProj
{
    public class GodSlayerDart : ModProjectile, ILocalizedModType
    {
        public bool initialized = false;
        public bool isMount = false;
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public Player Owner => Main.player[Projectile.owner];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        // 行为枚举
        public enum Dart
        {
            // 正常AI
            NorDart,
            // 挂载在玩家身上时
            MountDart,
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
            Projectile.timeLeft = 140;
        }
        public int hasfirecount = 0;
        public int firedely = 15;
        public override bool? CanHitNPC(NPC target) => Projectile.ai[0] != 0f;

        public override void AI()
        {
            if (Projectile.ai[0] == 0f)
            {
                MountDark(Projectile, ref firedely, ref hasfirecount);
                if (initialized == false)
                {
                    Projectile.timeLeft = 140;
                    initialized = true;
                    isMount = true;
                }
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

                DarkAI(Projectile);

                if (initialized == false)
                {
                    Projectile.timeLeft = 360;
                    initialized = true;
                }
            }
        }

        public static void MountDark(Projectile projectile , ref int firerelay, ref int hasfirecount)
        {
            Player Owner = Main.player[projectile.owner];

            Vector2 armPosition = Owner.RotatedRelativePoint(Owner.MountedCenter, true);
            var source = projectile.GetSource_FromThis();;
            firerelay--;
            float baseAngle = projectile.velocity.ToRotation();
            int numberOfProjectiles = 8;
            float spreadAngle = MathHelper.ToRadians(360);
            float angleStep = spreadAngle / numberOfProjectiles;
            float currentAngle = baseAngle - spreadAngle / 2 + (angleStep * hasfirecount) ;
            Vector2 direction = new((float)Math.Cos(currentAngle), (float)Math.Sin(currentAngle));

            if(firerelay == 0 && hasfirecount < 8)
            {
                Projectile.NewProjectile(source, armPosition, direction * 8f, ModContent.ProjectileType<GodSlayerDart>(), projectile.damage, 2f, projectile.owner, 1, 0f);
                firerelay = 5;
                hasfirecount++;
            }
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

            if (projectile.timeLeft > 300)
            {
                projectile.velocity *= 0.97f;
            }

            if (projectile.timeLeft < 300)
            {
                float maxSpeed = 20f;
                float acceleration = 0.05f * 12f;
                float homeInSpeed = MathHelper.Clamp(projectile.ai[1] += acceleration, 0f, maxSpeed);

                CIFunction.HomeInOnNPC(projectile, !projectile.tileCollide, 10000f, homeInSpeed, 15f, 5f);
            }
        }
        public override void PostDraw(Color lightColor)
        {
            if (!isMount)
            {
                Vector2 origin = new Vector2(11f, 23f);
                Main.EntitySpriteDraw(ModContent.Request<Texture2D>($"{GenericProjRoute.ProjRoute}/ArmorProj/GodslayerDartGlow").Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, origin, 1f, SpriteEffects.None, 0);
            }
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[0] != 0f)
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
