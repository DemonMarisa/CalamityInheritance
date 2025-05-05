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
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 27;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 600;
            AIType = ProjectileID.UnholyTridentFriendly;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.2f / 255f, (255 - Projectile.alpha) * 0.01f / 255f, (255 - Projectile.alpha) * 0.2f / 255f);
            if (Projectile.localAI[1] > 7f)
            {
                int dType = Main.rand.Next(3);
                dType = dType switch
                {
                    0 => 14,
                    1 => 27,
                    _ => 173,
                };
                int d = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.velocity.X * 4f + 2f, Projectile.position.Y + 2f - Projectile.velocity.Y * 4f), 8, 8, dType, 0f, 0f, 100, default, 1.25f);
                Main.dust[d].velocity *= 0.1f;
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
            int tCounts = 5;
            SoundEngine.PlaySound(SoundID.Item103, target.Center);
            for (int i = 0; i < tCounts; i++)
            {
                float rAngle = Main.rand.NextFloat(0f, MathHelper.TwoPi);
                Vector2 tacleVel = new Vector2((float)Math.Cos(rAngle), (float)Math.Sin(rAngle));

                Vector2 tacleVelRand = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                tacleVelRand.Normalize();
                tacleVel = tacleVel * 4f + tacleVelRand;
                tacleVel.Normalize();
                tacleVel *= 5f;

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

                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, tacleVel, ProjectileID.ShadowFlame, Projectile.damage / 4, Projectile.knockBack, Projectile.owner, tentacleXDirection, tentacleYDirection);
                //ȡ��local�޵�֡
                Main.projectile[p].usesLocalNPCImmunity = false;
                //���þ�̬�޵�֡
                Main.projectile[p].usesIDStaticNPCImmunity = true;
                //����10
                Main.projectile[p].idStaticNPCHitCooldown = 10;
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
