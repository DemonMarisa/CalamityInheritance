using CalamityInheritance.Content.BaseClass.Projectiles.HeldProj;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.Melee.Spear;
using CalamityInheritance.Content.Projectiles.Melee.Spears;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Melee.Spears
{
    public class InsidiousImpalerProjLegacy : BaseSpear
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<InsidiousImpalerLegacy>();
        public override float RangeMin => 16;
        public override float RangeMax => 76;
        public override void ExAI()
        {
            Shoot();
        }
        public void Shoot()
        {
            if (Projectile.LAP().FirstFrame)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity, Projectile.velocity * 3.5f, ProjectileType<InsidiousHarpoonLegacy>(), (int)(Projectile.damage * 0.5), Projectile.knockBack * 0.85f, Projectile.owner);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CISulphuricPoisoning>(), 180);
            target.AddBuff(BuffID.Venom, 180);
        }
    }
}
