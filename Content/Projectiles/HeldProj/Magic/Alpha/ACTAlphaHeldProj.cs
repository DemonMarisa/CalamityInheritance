using CalamityInheritance.Content.BaseClass;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Sounds.Custom;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Magic.Alpha
{
    public class ACTAlphaHeldProj : BaseHeldProj, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override float OffsetX => 20;
        public override float OffsetY => 0;
        public override float BaseOffsetY => 0;
        public override float WeaponRotation => 0;
        public override float AimResponsiveness => 0.25f;
        public Player Owner => Main.player[Projectile.owner];
        public override void SetDefaults()
        {
            Projectile.width = 74;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void HoldoutAI()
        {
            ref float attackTimer = ref Projectile.ai[0];
            attackTimer++;
            // Update damage based on curent magic damage stat (so Mana Sickness affects it)
            Projectile.damage = Owner.HeldItem is null ? 0 : Owner.GetWeaponDamage(Owner.HeldItem);
            // 使用旋转角度计算方向
            Vector2 Projdirection = Vector2.UnitX.RotatedBy(Projectile.rotation);
            Projdirection.SafeNormalize(Vector2.UnitX);
            Vector2 fireOffset = new(10f, 0f);
            fireOffset = fireOffset.RotatedBy(Projectile.rotation);

            if (Projectile.ai[2] == 0f)
            {
                Owner.CheckMana(Owner.ActiveItem(), (int)(Owner.HeldItem.mana * Owner.manaCost), true, false);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + fireOffset, Projdirection, ModContent.ProjectileType<AlphaBeam>(), Projectile.damage, Projectile.knockBack, Owner.whoAmI, 1.5f, 0f, 0f);
                Projectile.ai[2]++;
            }
            if (attackTimer % 20 == 0)
            {
                SoundEngine.PlaySound(CISoundMenu.GenisisFire, Projectile.Center);
                Owner.CheckMana(Owner.ActiveItem(), (int)(Owner.HeldItem.mana * Owner.manaCost), true, false);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + fireOffset, Projdirection, ModContent.ProjectileType<AlphaBeam>(), Projectile.damage, Projectile.knockBack, Owner.whoAmI, 1.5f, 0f, 0f);
            }
        }
        #region 删除条件
        public override void DelCondition()
        {
            Projectile.ai[2]++;
            if (Projectile.ai[2] > 30)
                Projectile.Kill();
        }
        #endregion
    }
}
