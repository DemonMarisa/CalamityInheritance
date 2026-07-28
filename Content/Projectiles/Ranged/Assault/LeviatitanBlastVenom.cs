using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Ranged.Assault
{
    public class LeviatitanBlastVenom : CIRangedProj
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }
        public override void ExSD()
        {
            Projectile.width = Projectile.height = 12;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 1;
        }
        public override void AI()
        {
            Projectile.FramesChanger(5, 4);
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi) + MathHelper.ToRadians(90) * Projectile.direction;
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0f / 255f, (255 - Projectile.alpha) * 0f / 255f, (255 - Projectile.alpha) * 0.75f / 255f);
            for (int i = 0; i < 2; i++)
            {
                Vector2 dspeed = -Projectile.velocity * Main.rand.NextFloat(0.5f * 0.8f);
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Water, 0f, 0f, 100, default, 1f);
                dust.noGravity = true;
                dust.velocity = dspeed;
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int lol = 0; lol < 10; lol++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Water, 0f, 0f, 100, default, 1f);
            }
        }
    }
}
