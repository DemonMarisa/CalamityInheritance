using System;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class WarbladeProj: ModProjectile, ILocalizedModType
    {
        //这把刀只会转180°
        private const float HitRange = (float)Math.PI;
        //第一次挥刀时的幅度
        private const float FirstSwing = 0.45f;
        private const float SpinRange = 3.5f * (float)Math.PI;
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.timeLeft = 10000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
        }
    }
}