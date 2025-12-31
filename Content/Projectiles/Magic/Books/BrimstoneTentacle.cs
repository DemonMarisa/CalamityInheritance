using CalamityInheritance.Content.Projectiles;
using CalamityMod.Dusts;
using LAP.Assets.TextureRegister;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.Books
{
    public class BrimstoneTentacle : GeneralDamageProj
    {
        public override ProjDamageType UseDamageClass => ProjDamageType.Magic;
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void ExSD()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.penetrate = 5;
            Projectile.MaxUpdates = 3;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
        }

        public override void AI()
        {
            Projectile.localAI[1]++;
            if (Projectile.velocity.X != Projectile.velocity.X)
            {
                if (Math.Abs(Projectile.velocity.X) < 1f)
                {
                    Projectile.velocity.X = -Projectile.velocity.X;
                }
                else
                {
                    Projectile.Kill();
                }
            }
            if (Projectile.velocity.Y != Projectile.velocity.Y)
            {
                if (Math.Abs(Projectile.velocity.Y) < 1f)
                {
                    Projectile.velocity.Y = -Projectile.velocity.Y;
                }
                else
                {
                    Projectile.Kill();
                }
            }
            Vector2 center10 = Projectile.Center;
            Projectile.scale = 1f - Projectile.localAI[0];
            Projectile.width = (int)(20f * Projectile.scale);
            Projectile.height = Projectile.width;
            Projectile.position.X = center10.X - Projectile.width / 2;
            Projectile.position.Y = center10.Y - Projectile.height / 2;
            if (Projectile.localAI[0] < 0.1f)
            {
                Projectile.localAI[0] += 0.01f;
            }
            else
            {
                Projectile.localAI[0] += 0.025f;
            }
            if (Projectile.localAI[0] >= 0.95f)
            {
                Projectile.Kill();
            }
            Projectile.velocity.X = Projectile.velocity.X + Projectile.ai[0] * 1.5f;
            Projectile.velocity.Y = Projectile.velocity.Y + Projectile.ai[1] * 1.5f;
            if (Projectile.velocity.Length() > 16f)
            {
                Projectile.velocity.Normalize();
                Projectile.velocity *= 16f;
            }
            Projectile.ai[0] *= 1.05f;
            Projectile.ai[1] *= 1.05f;
            if (Projectile.scale < 1f && Projectile.localAI[1] > 5f)
            {
                int scaleLoopCheck = 0;
                while (scaleLoopCheck < Projectile.scale * 10f)
                {
                    int brimDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, (int)CalamityDusts.Brimstone, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1.5f);
                    Main.dust[brimDust].position = (Main.dust[brimDust].position + Projectile.Center) / 2f;
                    Main.dust[brimDust].noGravity = true;
                    Main.dust[brimDust].velocity *= 0.1f;
                    Main.dust[brimDust].velocity -= Projectile.velocity * (1.3f - Projectile.scale);
                    Main.dust[brimDust].fadeIn = 100 + Projectile.owner;
                    Main.dust[brimDust].scale += Projectile.scale * 0.75f;
                    scaleLoopCheck++;
                }
            }
        }
    }
}