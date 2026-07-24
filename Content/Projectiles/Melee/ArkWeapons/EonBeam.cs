using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.ArkWeapons
{
    public class EonBeam : CIMeleeProj
    {
        public ref float UseTexture => ref Projectile.ai[1];
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 500;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            int dustID = Main.rand.Next(3) switch { 0 => 15, 1 => 57, _ => 58 };
            int num225 = Dust.NewDust(new Vector2(Projectile.position.X - Projectile.velocity.X * 4f + 2f, Projectile.position.Y + 2f - Projectile.velocity.Y * 4f), 8, 8, dustID, 0f, 0f, 100, default, 1.25f);
            Dust dust59 = Main.dust[num225];
            Dust dust3 = dust59;
            dust3.velocity *= 0.1f;

            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.3f / 255f, (255 - Projectile.alpha) * 0.4f / 255f, (255 - Projectile.alpha) * 1f / 255f);
            if (Projectile.localAI[1] > 7f)
            {
                int dType = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowTorch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 150, new Color(53, Main.DiscoG, 255), 1.2f);
                Main.dust[dType].velocity *= 0.1f;
                Main.dust[dType].noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(53, Main.DiscoG, 255, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.timeLeft > 490)
                return false;
            UseTexture = MathHelper.Clamp(UseTexture, 1f, 4f);
            Color color;
            if (UseTexture == 1f)
            {
                color = Color.Violet;
            }
            else if (UseTexture == 2f)
            {
                color = Color.Turquoise;
            }
            else if (UseTexture == 3f)
            {
                color = Color.SkyBlue;
            }
            else
            {
                color = Color.Orange;
            }
            Projectile.GetProjDrawInfo_Melee(out Texture2D texture, out Vector2 drawPos, out float drawRot, out Vector2 orig, out SpriteEffects spriteEffects);
            Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(color), drawRot - MathHelper.PiOver4, texture.Size() / 2, Projectile.scale, spriteEffects, 0f);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 7; k++)
            {
                int dType = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowTorch, 0f, 0f, 150, new Color(53, Main.DiscoG, 255), 1.2f);
                Main.dust[dType].noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] != 1f) //excludes True Ark of the Ancients
            {
                target.AddBuff(BuffType<CIBrimstoneFlames>(), 120);
                target.AddBuff(BuffID.Frostburn, 120);
                target.AddBuff(BuffType<CIHolyFlames>(), 120);
                target.AddBuff(BuffType<CIPlague>(), 120);
            }
        }
    }
}
