using CalamityInheritance.Content.BaseClass.Projectiles;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.ArkWeapons
{
    public class ElementBall : CIMeleeProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 150;
        }

        public override void AI()
        {
            int rainbowDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowTorch, Projectile.direction * 2, 0f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
            Main.dust[rainbowDust].noGravity = true;
            Main.dust[rainbowDust].velocity *= 0f;

            LAPUtilities.HomeInNPC(Projectile, 1600f, 24f, 20f);
        }
    }
}