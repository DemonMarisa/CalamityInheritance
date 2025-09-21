using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku.SwordArk
{
    public partial class ShizukuStar : ModProjectile, ILocalizedModType
    {
        private void HandleRogue(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
        private void HandleSummon(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
        //处于日食/霜月/南瓜月下，对敌怪造成多种致命减益
        private void HandleRanged(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //
        }
        private void HandleMagic(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //贝特西、碎甲与灵液
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 600);
            target.AddBuff(BuffID.Ichor, 600);
            target.AddBuff(BuffID.BetsysCurse, 600);
        }
        private void HandleDefault(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //树妖增防
            Owner.AddBuff(BuffID.DryadsWard, 600);
            Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_OnHit(target), Projectile.Center, Projectile.velocity, ProjectileID.TerrarianBeam, Projectile.damage / 2, Projectile.knockBack, Owner.whoAmI);
            proj.DamageType = DamageClass.Generic;
            proj.penetrate = 1;
        }
    }
}