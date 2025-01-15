using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.ArmorProj
{
    public class MiniRocket : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Mods.CalamityInheritance.Content.Content.Projectiles";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rocket");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction = Utils.ToDirectionInt(Projectile.velocity.X > 0f);
            Projectile.rotation = Utils.ToRotation(Projectile.velocity) + ((Projectile.spriteDirection == 1) ? 0f : ((float)Math.PI)) + MathHelper.ToRadians(90f) * Projectile.direction;

            for (int num246 = 0; num246 < 2; num246++)
            {
                float num247 = 0f;
                float num248 = 0f;
                if (num246 == 1)
                {
                    num247 = Projectile.velocity.X * 0.5f;
                    num248 = Projectile.velocity.Y * 0.5f;
                }
                int num249 = Dust.NewDust(new Vector2(Projectile.position.X + 3f + num247, Projectile.position.Y + 3f + num248) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, DustID.Torch, 0f, 0f, 100, default, 1f);
                Main.dust[num249].scale *= 2f + Main.rand.Next(10) * 0.1f;
                Main.dust[num249].velocity *= 0.2f;
                Main.dust[num249].noGravity = true;
                num249 = Dust.NewDust(new Vector2(Projectile.position.X + 3f + num247, Projectile.position.Y + 3f + num248) - Projectile.velocity * 0.5f, Projectile.width - 8, Projectile.height - 8, DustID.Smoke, 0f, 0f, 100, default, 0.5f);
                Main.dust[num249].fadeIn = 1f + Main.rand.Next(5) * 0.1f;
                Main.dust[num249].velocity *= 0.05f;
            }
                
            if (Projectile.timeLeft > 170)
            {
                Projectile.velocity *= 0.9f;
            }

            if (Projectile.timeLeft < 170)
            {
                float maxSpeed = 20f;
                float acceleration = 0.1f * 8f;
                float homeInSpeed = MathHelper.Clamp(Projectile.ai[0] += acceleration, 0f, maxSpeed);

                CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, 2500f, homeInSpeed, 15f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if(Projectile.timeLeft >197)
            {
                return false;
            }
            return base.PreDraw(ref lightColor);
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 16;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            for (int num621 = 0; num621 < 20; num621++)
            {
                int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
                Main.dust[num622].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int num623 = 0; num623 < 30; num623++)
            {
                int num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3f);
                Main.dust[num624].noGravity = true;
                Main.dust[num624].velocity *= 5f;
                num624 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                Main.dust[num624].velocity *= 2f;
            }
            Vector2 goreSource = Projectile.Center;
            Vector2 source = new Vector2(goreSource.X - 24f, goreSource.Y - 24f);
            for (int num625 = 0; num625 < 3; num625++)
            {
                float scaleFactor10 = 0.33f;
                if (num625 == 1)
                {
                    scaleFactor10 = 0.66f;
                }
                if (num625 == 2)
                {
                    scaleFactor10 = 1f;
                }
                int type = Main.rand.Next(61, 64);
                int num626 = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Gore expr_13AB6_cp_0 = Main.gore[num626];
                expr_13AB6_cp_0.velocity.X += 1f;
                Gore expr_13AD6_cp_0 = Main.gore[num626];
                expr_13AD6_cp_0.velocity.Y += 1f;
                num626 = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Gore expr_13B79_cp_0 = Main.gore[num626];
                expr_13B79_cp_0.velocity.X -= 1f;
                Gore expr_13B99_cp_0 = Main.gore[num626];
                expr_13B99_cp_0.velocity.Y += 1f;
                num626 = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Gore expr_13C3C_cp_0 = Main.gore[num626];
                expr_13C3C_cp_0.velocity.X += 1f;
                Gore expr_13C5C_cp_0 = Main.gore[num626];
                expr_13C5C_cp_0.velocity.Y -= 1f;
                num626 = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                Main.gore[num626].velocity *= scaleFactor10;
                Gore expr_13CFF_cp_0 = Main.gore[num626];
                expr_13CFF_cp_0.velocity.X -= 1f;
                Gore expr_13D1F_cp_0 = Main.gore[num626];
                expr_13D1F_cp_0.velocity.Y -= 1f;
            }
        }
    }
}
