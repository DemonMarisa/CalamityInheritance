using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku.SwordArk
{
    public partial class ShizukuStar : ModProjectile, ILocalizedModType
    {
        private void HandleEvilBiome(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
        private void HandleHallowedBiome(NPC target, NPC.HitInfo hit, int damageDone)
        {

        }
        //处于日食/霜月/南瓜月下，对敌怪造成多种致命减益
        private void HandleThreeEvent(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Nightwither>(), 600);
            // target.AddBuff(ModContent.BuffType)
        }
        private void HandlePillarEvent(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //四柱事件下提供对应增益。
            //日耀增益 ： 25%防前免伤
            Owner.Calamity().projectileDamageReduction += 0.25f;
            Owner.Calamity().contactDamageReduction += 0.25f;
            //星璇增益 ： 为玩家
        }
        private void HandleDefault(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //树妖增防
            Owner.AddBuff(BuffID.DryadsWard, 600);
            Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_OnHit(target), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<CosmicOrbLegacy>(), Projectile.damage / 2, Projectile.knockBack, Owner.whoAmI);
            proj.DamageType = DamageClass.Generic;
            proj.penetrate = 1;
        }
    }
}