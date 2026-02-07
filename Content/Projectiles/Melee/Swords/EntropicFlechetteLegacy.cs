using CalamityInheritance.Texture;
using CalamityMod;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Swords
{
    public class EntropicFlechetteLegacy : GeneralDamageProj
    {
        public override ProjDamageType UseDamageClass => ProjDamageType.Melee;
        public override string Texture => CITextureRegistry.EntropicFlechette1.Path;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
            ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 1;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Vector2 rotateVector = new Vector2(6f, 12f);
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] == 48f)
            {
                Projectile.localAI[0] = 0f;
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 dustRotate = -Vector2.UnitY.RotatedBy((double)(Projectile.localAI[0] * 0.1308997f + (float)i * 3.14159274f), default) * rotateVector;
                    int darkDust = Dust.NewDust(Projectile.Center, 0, 0, DustID.Shadowflame, 0f, 0f, 160, default, 1f);
                    Main.dust[darkDust].scale = 1f;
                    Main.dust[darkDust].noGravity = true;
                    Main.dust[darkDust].position = Projectile.Center + dustRotate;
                    Main.dust[darkDust].velocity = Projectile.velocity;
                }
            }

            int darkestDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0f, 100, default, 0.8f);
            Main.dust[darkestDust].noGravity = true;
            Main.dust[darkestDust].velocity *= 0f;
            Projectile.HomeInNPC(900f, 12f, 20f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture;
            if (Projectile.ai[0] == 0)
            {
                texture = CITextureRegistry.EntropicFlechette1.Value;
            }
            else if (Projectile.ai[0] == 1)
            {
                texture = CITextureRegistry.EntropicFlechette2.Value;
            }
            else
            {
                texture = CITextureRegistry.EntropicFlechette3.Value;
            }
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2, texture);
            return false;
        }
    }
}
