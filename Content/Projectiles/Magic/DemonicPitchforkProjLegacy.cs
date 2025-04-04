using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class DemonicPitchforkProjLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 27;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 600;
            AIType = ProjectileID.UnholyTridentFriendly;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.2f / 255f, (255 - Projectile.alpha) * 0.01f / 255f, (255 - Projectile.alpha) * 0.2f / 255f);
            if (Projectile.localAI[1] > 7f)
            {
                int num307 = Main.rand.Next(3);
                if (num307 == 0)
                {
                    num307 = 14;
                }
                else if (num307 == 1)
                {
                    num307 = 27;
                }
                else
                {
                    num307 = 173;
                }
                int num308 = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.velocity.X * 4f + 2f, Projectile.position.Y + 2f - Projectile.velocity.Y * 4f), 8, 8, num307, 0f, 0f, 100, default, 1.25f);
                Main.dust[num308].velocity *= 0.1f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int tentacleNum = 5;
            SoundEngine.PlaySound(SoundID.Item103, target.Center);
            for (int i = 0; i < tentacleNum; i++)
            {
                float randomAngle = Main.rand.NextFloat(0f, MathHelper.TwoPi);
                Vector2 tentacleVelocity = new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle));

                Vector2 tentacleRandVelocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                tentacleRandVelocity.Normalize();
                tentacleVelocity = tentacleVelocity * 4f + tentacleRandVelocity;
                tentacleVelocity.Normalize();
                tentacleVelocity *= 5f;

                float tentacleYDirection = Main.rand.Next(10, 80) * 0.001f;
                if (Main.rand.NextBool())
                {
                    tentacleYDirection *= -1f;
                }
                float tentacleXDirection = Main.rand.Next(10, 80) * 0.001f;
                if (Main.rand.NextBool())
                {
                    tentacleXDirection *= -1f;
                }

                int newProjectileId1 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, tentacleVelocity, ProjectileID.ShadowFlame, Projectile.damage / 4, Projectile.knockBack, Projectile.owner, tentacleXDirection, tentacleYDirection);
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 2; k++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Demonite, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Shadowflame, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.ShadowbeamStaff, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
            
        }
    }
}
