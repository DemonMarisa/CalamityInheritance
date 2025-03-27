using System;
using CalamityInheritance.Utilities;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class DukeLegendarySpout: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 150;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            Projectile.timeLeft = 60;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            int pScale = 32;
            float pScaleMody = 1.5f;
            int pWidth = 150;
            int pHeight = 42;
            if (Projectile.velocity.X != 0f)
            {
                Projectile.direction = Projectile.spriteDirection = -Math.Sign(Projectile.velocity.X);
            }
            Projectile.frame = CIFunction.FramesChanger(Projectile, 6, 6);
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                Projectile.position.X = Projectile.position.X + Projectile.width / 2;
                Projectile.position.Y = Projectile.position.Y + Projectile.height / 2;
                Projectile.scale = (pScale - Projectile.ai[1]) * pScaleMody / pScale;
                Projectile.width = (int)(pWidth * Projectile.scale);
                Projectile.height = (int)(pHeight * Projectile.scale);
                Projectile.position.X = Projectile.position.X - Projectile.width / 2;
                Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
                Projectile.netUpdate = true;
            }
            if (Projectile.ai[1] != -1f)
            {
                Projectile.scale = (pScale - Projectile.ai[1]) * pScaleMody / pScale;
                Projectile.width = (int)(pWidth * Projectile.scale);
                Projectile.height = (int)(pHeight * Projectile.scale);
            }
            if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                Projectile.alpha -= 3;
                if (Projectile.alpha < 60)
                {
                    Projectile.alpha = 60;
                }
            }
            else
            {
                Projectile.alpha += 3;
                if (Projectile.alpha > 150)
                {
                    Projectile.alpha = 150;
                }
            }
            if (Projectile.ai[0] > 0f)
            {
                Projectile.ai[0] -= 1f;
            }
            if (Projectile.ai[0] == 1f && Projectile.ai[1] > 0f && Projectile.owner == Main.myPlayer)
            {
                Projectile.netUpdate = true;
                Vector2 center = Projectile.Center;
                center.Y -= pHeight * Projectile.scale / 2f;
                float newSeg = (pScale - Projectile.ai[1] + 1f) * pScaleMody / pScale;
                center.Y -= pHeight * newSeg / 2f;
                center.Y += 2f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), center.X, center.Y, Projectile.velocity.X, Projectile.velocity.Y, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner, 8f, Projectile.ai[1] - 1f);
            }
            if (Projectile.ai[0] <= 0f)
            {
                float sSize = MathHelper.Pi / 30f;
                float sWidth = Projectile.width / 5f;
                sWidth *= 2f;
                float xChange = (float)(Math.Cos((double)(sSize * -(double)Projectile.ai[0])) - 0.5) * sWidth;
                Projectile.position.X = Projectile.position.X - xChange * (float)-(float)Projectile.direction;
                Projectile.ai[0] -= 1f;
                xChange = (float)(Math.Cos((double)(sSize * -(double)Projectile.ai[0])) - 0.5) * sWidth;
                Projectile.position.X = Projectile.position.X + xChange * (float)-(float)Projectile.direction;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(53, Main.DiscoG, 255, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int f = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
            int y = f * Projectile.frame;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, y, tex.Width, f)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(tex.Width / 2f, f / 2f), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
