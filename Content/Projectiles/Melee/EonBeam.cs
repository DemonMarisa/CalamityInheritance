using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class EonBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Beam");
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 27;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 300;
            AIType = ProjectileID.EnchantedBeam;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.3f / 255f, (255 - Projectile.alpha) * 0.4f / 255f, (255 - Projectile.alpha) * 1f / 255f);
            if (Projectile.localAI[1] > 7f)
            {
                int num308 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 66, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 150, new Color(Main.DiscoR, 100, 255), 1.2f);
                Main.dust[num308].velocity *= 0.1f;
                Main.dust[num308].noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(Main.DiscoR, 100, 255, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
			if (Projectile.timeLeft > 295)
				return false;

			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int k = 0; k < 7; k++)
            {
                int num308 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 66, 0f, 0f, 150, new Color(Main.DiscoR, 100, 255), 1.2f);
                Main.dust[num308].noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] != 1f) //excludes (True) Ark of the Ancients
            {
                target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
                target.AddBuff(BuffID.Frostburn, 120);
                target.AddBuff(ModContent.BuffType<Plague>(), 120);
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
            }
        }
    }
}