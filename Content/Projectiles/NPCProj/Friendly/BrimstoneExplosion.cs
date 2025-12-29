using CalamityInheritance.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.NPCProj.Friendly
{
    public class BrimstoneExplosion : ModProjectile
    {
        private readonly int Dusts = 5;

        public override void SetDefaults()
        {
            Projectile.width = 120;
            Projectile.height = 120;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.alpha = 254;
            Projectile.friendly = true;
            Projectile.timeLeft = 50;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
        }

        public override void AI()
        {
            for (int i = 0; i < Dusts; i++)
            {
                Vector2 vector = new Vector2(Main.rand.Next(15), 0f);
                vector = Utils.RotatedByRandom(vector, (double)MathHelper.ToRadians(360f));
                int d = Dust.NewDust(Projectile.Center, Main.rand.Next(40) - 20, Main.rand.Next(40) - 20, DustID.Clentaminator_Red, vector.X, vector.Y, 0, new Color(255, 0, 0), 2f);
                Main.dust[d].noGravity = true;
            }
            Lighting.AddLight(Projectile.Center, 2.8f, 0f, 0f);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 2;
            target.AddBuff(BuffType<AbyssalFlames>(), 600, false);
            target.AddBuff(BuffType<VulnerabilityHexLegacy>(), 600, false);
            target.AddBuff(BuffType<Horror>(), 600, false);
        }
    }
}
