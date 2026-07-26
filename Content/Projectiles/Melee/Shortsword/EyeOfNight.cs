using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Core.Path;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Shortsword
{
    public class EyeOfNight : BaseStickyProj, ILocalizedModType
    {
        public new string LocalizationCategory => LocalizationPath.MeleeProj;
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.width = Projectile.height = 10;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 180;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 90;
            Projectile.DamageType = DamageClass.Melee;
        }
        public override void ExAI()
        {
            if (!Main.dedServ && Projectile.velocity.Length() > 5f)
                Dust.NewDustPerfect(Projectile.Center, DustID.CursedTorch).noGravity = true;
        }
        public override void OnKill(int timeLeft)
        {
            if (!Main.dedServ)
            {
                for (int i = 0; i < 10; i++)
                    Dust.NewDustDirect(Projectile.position, 36, 36, DustID.CursedTorch).noGravity = true;
            }
        }
    }
}
