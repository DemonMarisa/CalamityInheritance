using System;
using CalamityInheritance.Utilities;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class DestroyerLegendaryBomb: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public int boomTimer = 120;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            float lights = Main.rand.Next(90, 111) * 0.01f;
            lights *= Main.essScale;
            Lighting.AddLight(Projectile.Center, 1f * lights, 0.2f * lights, 0.75f * lights);
            Projectile.alpha -= 2;
            Projectile.frame = CIFunction.FramesChanger(Projectile, 4, 4);
            Projectile.ai[0] = Main.rand.Next(-100, 101) * 0.0025f;
            Projectile.ai[1] = Main.rand.Next(-100, 101) * 0.0025f;
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.scale += 0.05f;
                if (Projectile.scale > 1.2)
                {
                    Projectile.localAI[0] = 1f;
                }
            }
            else
            {
                Projectile.scale -= 0.05f;
                if (Projectile.scale < 0.8)
                {
                    Projectile.localAI[0] = 0f;
                }
            }
            Projectile.velocity.X *= 0.985f;
            Projectile.velocity.Y *= 0.985f;
            float projX = Projectile.Center.X;
            float projY = Projectile.Center.Y;
            float explodeRange = 250f;
            bool canExplode = false;
            foreach (NPC n in Main.ActiveNPCs)
            {
                if (n.CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, n.Center, 1, 1))
                {
                    float npcX = n.position.X + n.width / 2;
                    float npcY = n.position.Y + n.height / 2;
                    float npcDist = Math.Abs(Projectile.position.X + Projectile.width / 2 - npcX) + Math.Abs(Projectile.position.Y + Projectile.height / 2 - npcY);
                    if (npcDist < explodeRange)
                    {
                        explodeRange = npcDist;
                        canExplode = true;
                    }
                }
            }
            if (canExplode)
            {
                boomTimer--;
                if (boomTimer <= 0)
                {
                    Projectile.Kill();
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, Main.DiscoG, 155, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int framing = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
            int y6 = framing * Projectile.frame;
            Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, y6, texture2D13.Width, framing)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture2D13.Width / 2f, framing / 2f), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item105, Projectile.Center);
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<SHPExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
            }
        }
    }
}
