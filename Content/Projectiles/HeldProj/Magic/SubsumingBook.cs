using CalamityInheritance.Content.BaseClass;
using CalamityInheritance.Content.Projectiles.ExoLore;
using CalamityInheritance.Texture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Magic
{
    public class SubsumingBook : BaseHeldProj, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override string Texture => $"{CIWeaponsResprite.CalMagicWeaponRoute}/SubsumingVortex";
        public override float OffsetX => -6;
        public override float OffsetY => 0;
        public override float BaseOffsetY => 0;
        public override float WeaponRotation => 0;
        public override float AimResponsiveness => 0.25f;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
        }
        public override void HoldoutAI()
        {
            ref float AttackTimer = ref Projectile.ai[0];
            AttackTimer++;
            Vector2 fireDir = Vector2.UnitX.RotatedBy(Projectile.rotation);
            fireDir = fireDir.SafeNormalize(Vector2.UnitX);
            //Shoot the Projectile, Thats what we need.
            Vector2 offset = new Vector2(OffsetX, 0).RotatedBy(Projectile.rotation);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + offset, fireDir, ModContent.ProjectileType<SubsumingVortexProjGiant>(), Projectile.damage, 0f, Projectile.owner);
        }
    }
}