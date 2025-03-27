using CalamityMod.Buffs.DamageOverTime;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.Typeless
{
    public class GodSlayerOrb : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 200;
            Projectile.extraUpdates = 1;
        }
        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < 190 && target.CanBeChasedBy(Projectile);
        public override void AI()
        {
            int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 2f);
            Main.dust[d].noGravity = true;
            Main.dust[d].velocity *= 0f;
            if (Projectile.timeLeft < 190)
                CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, 1500f, 12f, 25f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 180);
        }
    }
}
