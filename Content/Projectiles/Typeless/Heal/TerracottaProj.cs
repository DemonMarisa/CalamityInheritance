using CalamityInheritance.Content.BaseClass;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

using LAP.Assets.TextureRegister;
namespace CalamityInheritance.Content.Projectiles.Typeless.Heal
{
    public class TerracottaProj : BaseHealProj, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void ExSD()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.extraUpdates = 3;
        }

        public override void ExAI()
        {
            float num498 = Projectile.velocity.X * 0.2f;
            float num499 = -(Projectile.velocity.Y * 0.2f);
            int num500 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GoldCoin, 0f, 0f, 100, default, 1f);
            Dust dust = Main.dust[num500];
            dust.noGravity = true;
            dust.position.X -= num498;
            dust.position.Y -= num499;
        }

        public override void ExKill()
        {
            for (int num621 = 0; num621 < 5; num621++)
            {
                int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GoldCoin, 0f, 0f, 100, default, 2f);
                Main.dust[num622].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[num622].scale = 0.5f;
                    Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
        }
    }
}
