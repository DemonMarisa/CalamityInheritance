using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Armor.Ranged
{
    public class BloodflareSoul : CIArmorProj
    {
        public int timer;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.scale = 1.2f;
            Projectile.alpha = 100;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 900;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (timer > 45)
                return base.CanHitNPC(target);
            else
                return false;
        }
        public override void AI()
        {
            timer++;
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
            {
                Projectile.frame = 0;
            }
            int num822 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch, 0f, 0f, 0, default, 1f);
            Dust dust = Main.dust[num822];
            dust.velocity *= 0.1f;
            Main.dust[num822].scale = 1.3f;
            Main.dust[num822].noGravity = true;
            float num953 = 30f * Projectile.ai[1]; //100
            float scaleFactor12 = 6f * Projectile.ai[1]; //5
            float num954 = 600f;
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) - 1.57f;
            Lighting.AddLight(Projectile.Center, 0.5f, 0.2f, 0.9f);
            if (Main.player[Projectile.owner].active && !Main.player[Projectile.owner].dead)
            {
                NPC npc = LAPUtilities.FindClosestTarget(Projectile.Center, 1500f);
                if (Projectile.Distance(Main.player[Projectile.owner].Center) > num954 && npc is null)
                {
                    Vector2 moveDirection = LAPUtilities.GetVector2(Projectile.Center, Main.player[Projectile.owner].Center);
                    Projectile.velocity = (Projectile.velocity * (num953 - 1f) + moveDirection * scaleFactor12) / num953;
                    return;
                }
                if (timer > 45)
                    Projectile.HomeInNPC(1500f, 11f, 20f);
            }
            else
            {
                if (Projectile.timeLeft > 30)
                {
                    Projectile.timeLeft = 30;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * (b2 / 255f));
                return new Color(b2, b2, b2, a2);
            }
            return new Color(255, 255, 255, 100);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath39, Projectile.position);
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 110;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            int num226 = 36;
            for (int num227 = 0; num227 < num226; num227++)
            {
                Vector2 vector6 = Vector2.Normalize(Projectile.velocity) * new Vector2((float)Projectile.width / 2f, (float)Projectile.height) * 0.75f;
                vector6 = vector6.RotatedBy((double)((num227 - (num226 / 2 - 1)) * 6.28318548f / num226), default) + Projectile.Center;
                Vector2 vector7 = vector6 - Projectile.Center;
                int num228 = Dust.NewDust(vector6 + vector7, 0, 0, DustID.RedTorch, vector7.X * 1.5f, vector7.Y * 1.5f, 100, default, 2f);
                Main.dust[num228].noGravity = true;
                Main.dust[num228].noLight = true;
                Main.dust[num228].velocity = vector7;
            }
            Projectile.Damage();
        }
    }
}
