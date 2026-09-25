using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.Items.Weapons.CAWeapons.Magic;
using CalamityInheritance.Content.Items.Weapons.Ranged.Rifle;
using LAP.Core.BaseClass.Projectiles;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.CAWeapons.Helds
{
    public class ACTGenisisHeldProj : BaseHeldProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<ACTGenisis>();
        public override string Texture => GetInstance<ACTGenisis>().Texture;
        public override Vector2 PositionOffset => new Vector2(0, 0).RotatedBy(Projectile.rotation);
        public bool PowerFull => Projectile.ai[0] != 0;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 66;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.MaxUpdates = 1;
            RotAmount = 0.25f;
        }
        public override void ExAI()
        {
            DrawPosOffset = new Vector2(25, 0).RotatedBy(Projectile.rotation);
            // Update damage based on curent magic damage stat (so Mana Sickness affects it)
            Projectile.damage = Owner.HeldItem is null ? 0 : Owner.GetWeaponDamage(Owner.HeldItem);
            // 使用旋转角度计算方向
            Vector2 Projdirection = Vector2.UnitX.RotatedBy(Projectile.rotation);
            Projdirection.SafeNormalize(Vector2.UnitX);
            Vector2 fireOffset = new(45f, 0f);
            fireOffset = fireOffset.RotatedBy(Projectile.rotation);
            if (UseDelay == 0 && Owner.LAP().MouseLeft && Owner.CheckMana(Owner.ActiveItem(), (int)(Owner.HeldItem.mana * Owner.manaCost), true, false))
            {
                SoundEngine.PlaySound(CISounds.GenisisFire, Projectile.Center);
                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + fireOffset, Projdirection * 0.001f, ProjectileType<AlphaBeam>(), Projectile.damage, Projectile.knockBack, Owner.whoAmI, 1f, 0f, 0f);
                Main.projectile[p].localNPCHitCooldown = 4;
                Main.projectile[p].usesLocalNPCImmunity = true;
                if (PowerFull)
                    UseDelay = 20;
                else
                    UseDelay = 30;
            }
            Owner.SetArmRot(Projectile.rotation);
        }
    }
}
