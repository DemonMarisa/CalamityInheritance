using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using LAP.Assets.TextureRegister;
namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class DesertMark : ModProjectile
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 900;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Color newColor3 = new Color(255, 255, 255);
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = -1;
                SoundEngine.PlaySound(SoundID.Item60, Projectile.Center);
            }
            if (Projectile.localAI[1] < 30f)
            {
                for (int num1134 = 0; num1134 < 1; num1134++)
                {
                    float value79 = -0.5f;
                    float value80 = 0.9f;
                    float amount4 = Main.rand.NextFloat();
                    Vector2 value81 = new Vector2(MathHelper.Lerp(0.1f, 1f, Main.rand.NextFloat()), MathHelper.Lerp(value79, value80, amount4));
                    value81.X *= MathHelper.Lerp(2.2f, 0.6f, amount4);
                    value81.X *= -1f;
                    Vector2 value82 = new Vector2(2f, 10f);
                    Vector2 position4 = Projectile.Center + new Vector2(60f, 200f) * value81 * 0.5f + value82;
                    Dust dust34 = Main.dust[Dust.NewDust(position4, 0, 0, DustID.Sandnado, 0f, 0f, 0, default, 0.5f)];
                    dust34.position = position4;
                    dust34.customData = Projectile.Center + value82;
                    dust34.fadeIn = 1f;
                    dust34.scale = 0.3f;
                    if (value81.X > -1.2f)
                    {
                        dust34.velocity.X = 1f + Main.rand.NextFloat();
                    }
                    dust34.velocity.Y = Main.rand.NextFloat() * -0.5f - 1f;
                }
            }
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 0.8f;
                Projectile.direction = 1;
                Point point9 = Projectile.Center.ToTileCoordinates();
                Projectile.Center = new Vector2(point9.X * 16 + 8, point9.Y * 16 + 8);
            }
            Projectile.rotation = Projectile.localAI[1] / 40f * MathHelper.TwoPi * Projectile.direction;
            if (Projectile.localAI[1] < 33f)
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 8;
                }
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
            }
            if (Projectile.localAI[1] > 103f)
            {
                if (Projectile.alpha < 255)
                {
                    Projectile.alpha += 16;
                }
                if (Projectile.alpha > 255)
                {
                    Projectile.alpha = 255;
                }
            }
            if (Projectile.alpha == 0)
            {
                Lighting.AddLight(Projectile.Center, newColor3.ToVector3() * 0.5f);
            }
            for (int num1135 = 0; num1135 < 2; num1135++)
            {
                if (Main.rand.NextBool(10))
                {
                    Vector2 value83 = Vector2.UnitY.RotatedBy((double)(num1135 * MathHelper.Pi), default).RotatedBy(Projectile.rotation, default);
                    Dust dust35 = Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustID.Sandnado, 0f, 0f, 225, newColor3, 1f)];
                    dust35.noGravity = true;
                    dust35.noLight = true;
                    dust35.scale = Projectile.Opacity * Projectile.localAI[0];
                    dust35.position = Projectile.Center;
                    dust35.velocity = value83 * 2.5f;
                }
            }
            for (int num1136 = 0; num1136 < 2; num1136++)
            {
                if (Main.rand.NextBool(10))
                {
                    Vector2 value84 = Vector2.UnitY.RotatedBy((double)(num1136 * MathHelper.Pi), default);
                    Dust dust36 = Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustID.Sandnado, 0f, 0f, 225, newColor3, 1.5f)];
                    dust36.noGravity = true;
                    dust36.noLight = true;
                    dust36.scale = Projectile.Opacity * Projectile.localAI[0];
                    dust36.position = Projectile.Center;
                    dust36.velocity = value84 * 2.5f;
                }
            }
            if (Projectile.localAI[1] < 33f || Projectile.localAI[1] > 87f)
            {
                Projectile.scale = Projectile.Opacity / 2f * Projectile.localAI[0];
            }
            Projectile.velocity = Vector2.Zero;
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] == 60f && Projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<DesertTornado>(), Projectile.damage, 2f, Projectile.owner, 0f, 0f);
            }
            if (Projectile.localAI[1] >= 120f)
            {
                Projectile.Kill();
                return;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color color25 = Lighting.GetColor((int)(Projectile.position.X + Projectile.width * 0.5) / 16, (int)((Projectile.position.Y + Projectile.height * 0.5) / 16.0));
            Vector2 vector38 = Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
            Texture2D invis = Request<Texture2D>(Texture).Value;
            Rectangle rectangle11 = invis.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
            Color alpha5 = Projectile.GetAlpha(color25);
            Vector2 origin7 = rectangle11.Size() / 2f;
            Color color47 = Main.hslToRgb(0.25f, 1f, 1f).MultiplyRGBA(new Color(255, 255, 255, 0));
            Main.spriteBatch.Draw(invis, vector38, new Microsoft.Xna.Framework.Rectangle?(rectangle11), color47, 0f, origin7, new Vector2(1f, 5f) * Projectile.scale * 2f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(invis, vector38, new Microsoft.Xna.Framework.Rectangle?(rectangle11), alpha5, Projectile.rotation, origin7, Projectile.scale, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(invis, vector38, new Microsoft.Xna.Framework.Rectangle?(rectangle11), alpha5, 0f, origin7, new Vector2(1f, 8f) * Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
