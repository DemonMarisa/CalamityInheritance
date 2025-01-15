using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.Summon
{
    public class MidnightSunLaserold : ModProjectile,ILocalizedModType 
    {
        public new string LocalizationCategory => "Content.Projectiles.Summon";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Laser");
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.light = 0.5f;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 0f;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 1;
            AIType = 242;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
        }
        public override void AI()
        {
            CalamityUtils.HomeInOnNPC(Projectile, true, 200f, 18f, 20f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 5);
            return false;
        }
    }
}
