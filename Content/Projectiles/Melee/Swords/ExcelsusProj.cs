using CalamityInheritance.Texture;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Swords
{
    public class ExcelsusProj : ModProjectile, ILocalizedModType
    {
        public float TexStyle => Projectile.ai[0];
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => CITextureRegistry.ExcelsusMain.Path;
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 300;
            Projectile.alpha = 100;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) < 16f && Projectile.timeLeft > 85)
            {
                Projectile.velocity *= 1.05f;
            }
            if (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) > 0f && Projectile.timeLeft <= 85)
            {
                Projectile.velocity *= 0.98f;
            }
            Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.02f;
            if (Main.rand.NextBool(8))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueFairy, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.tileCollide = false;
            if (Projectile.timeLeft > 85)
            {
                Projectile.timeLeft = 85;
            }
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * ((float)b2 / 255f));
                return new Color((int)b2, (int)b2, (int)b2, (int)a2);
            }
            return default(Color);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 origin = new Vector2(39f, 46f);
            Texture2D tex = CITextureRegistry.ExcelsusMain.Value;
            Texture2D GlowTex = CITextureRegistry.ExcelsusMain.Value;
            if (TexStyle == 1)
            {
                tex = CITextureRegistry.ExcelsusBlue.Value;
                GlowTex = CITextureRegistry.ExcelsusBlueGlow.Value;
            }
            if (TexStyle == 2)
            {
                tex = CITextureRegistry.ExcelsusPink.Value;
                GlowTex = CITextureRegistry.ExcelsusPinkGlow.Value;
            }
            Color color;
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * ((float)b2 / 255f));
                color = new Color((int)b2, (int)b2, (int)b2, (int)a2);
            }
            else
            {
                color = new Color(255, 255, 255, 100);
            }
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(GlowTex, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.timeLeft > 85)
            {
                Projectile.timeLeft = 85;
            }
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 180);
        }
    }
}
