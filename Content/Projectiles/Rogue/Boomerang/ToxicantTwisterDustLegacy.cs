using CalamityMod.Buffs.DamageOverTime;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Rogue.Boomerang
{
    public class ToxicantTwisterDustLegacy : RogueDamageProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public override void ExSD()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            int idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.UnusedBrown, 0f, 0f, 100, default, 1f);
            Main.dust[idx].noGravity = true;
            Main.dust[idx].velocity *= 0f;
            Projectile.HomeInNPC(1500, 24f, 35f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<SulphuricPoisoning>(), 120);
        }
    }
}
