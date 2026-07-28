using CalamityInheritance.Assets;
using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged.Sniper
{
    public class RubicoPrimeBullet : CIRangedProj
    {
        public override string Texture => CIProjectiles_Range.AMRShot.Path;
        private bool initialized = false;
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.light = 0.5f;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 7;
            Projectile.scale = 1.18f;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            AIType = ProjectileID.BulletHighVelocity;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (!initialized && Projectile.CountsAsClass<RangedDamageClass>())
            {
                initialized = true;
                if (Main.netMode != NetmodeID.Server)
                {
                    //灾厄你这是何意味
                    SoundEngine.PlaySound(CISounds.LargeWeaponFire with { Volume = CISounds.LargeWeaponFire.Volume * 0.45f }, Projectile.Center);
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.CritDamage += 0.25f;
        }

        public override bool PreDraw(ref Color lightColor) => Projectile.timeLeft < 600;
    }
}
