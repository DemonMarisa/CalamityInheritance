using CalamityInheritance.Content.Items.Weapons.Melee.Swords;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles;
using LAP.Assets.TextureRegister;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Melee.Swords
{
    public class GrandGuardianHeal : GeneralDamageProj
    {
        public override ProjDamageType UseDamageClass => ProjDamageType.Typeless;
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        private const int TimeLeft = 300;

        private const int HomingTime = TimeLeft - 120;

        private const int SlowDownTime = TimeLeft - 150;

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = TimeLeft;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 3;
        }

        public override void AI()
        {
            if (Projectile.timeLeft < HomingTime)
            {
                if (Projectile.timeLeft > SlowDownTime)
                    Projectile.velocity *= 0.9f;
                else
                    Projectile.HealingProjectile(GrandGuardianLegacy.HealPerOrb, Projectile.owner, 12f, 15f);
            }

            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowTorch, 0f, 0f, 100, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB));
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0f;
            Main.dust[dust].position.X -= Projectile.velocity.X * 0.2f;
            Main.dust[dust].position.Y += Projectile.velocity.Y * 0.2f;
        }
    }
}