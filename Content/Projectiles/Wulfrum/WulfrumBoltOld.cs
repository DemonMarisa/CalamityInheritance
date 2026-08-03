using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Wulfrum
{
    public class WulfrumBoltOld : CIMagicProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 2;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, (255 - Projectile.alpha) * 0.1f / 255f, 0f);
            for (int num151 = 0; num151 < 3; num151++)
            {
                int num154 = 14;
                int num155 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width - num154 * 2, Projectile.height - num154 * 2, DustID.GreenTorch, 0f, 0f, 100, default, 3f);
                Main.dust[num155].noGravity = true;
                Main.dust[num155].noLight = true;
                Main.dust[num155].velocity *= 0.1f;
                Main.dust[num155].velocity += Projectile.velocity * 0.5f;
            }
            if (Main.rand.NextBool(8))
            {
                int num156 = 16;
                int num157 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width - num156 * 2, Projectile.height - num156 * 2, DustID.GreenTorch, 0f, 0f, 100, default, 2.25f);
                Main.dust[num157].velocity *= 0.25f;
                Main.dust[num157].noLight = true;
                Main.dust[num157].velocity += Projectile.velocity * 0.5f;
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] < 30f)
                Projectile.velocity *= 0.96f;
            else
            {
                Projectile.extraUpdates = 1;
                if (Projectile.ai[1] < 24f)
                    Projectile.ai[1]++;
                LAPUtilities.HomeInNPC(Projectile, 1500f, Projectile.ai[1] * 0.5f, 35f, null, false);
            }
        }
    }
}
