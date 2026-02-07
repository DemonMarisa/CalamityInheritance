using CalamityInheritance.Content.BaseClass;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Ranged
{
    public class MarniteBayonetHeldProj : BaseHeldProj, ILocalizedModType
    {
        public override string Texture => GetInstance<MarniteBayonet>().Texture;
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public int maxXdistance = 8;
        public float aniXdistance = 0;
        public override float OffsetX => aniXdistance - 6;
        public override float OffsetY => 0;
        public override float BaseOffsetY => 0;
        public override float WeaponRotation => 0;
        public override float DrawRotation => -MathHelper.ToRadians(7);
        // 旋转速度
        public override float AimResponsiveness => 0.25f;
        public Player Owner => Main.player[Projectile.owner];
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
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.MaxUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // Otherwise, perform an AABB line collision check to check the whole beam.
            float _ = float.NaN;
            bool c = Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(), 
                Projectile.Center - Projectile.rotation.ToRotationVector2() * 32,
                Projectile.Center + Projectile.rotation.ToRotationVector2() * 32, 
                12f, ref _);
            return c;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return null;
        }
        public override void HoldoutAI()
        {
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.extraUpdates = 0;
            // 使用类型 类型为0时为左键 为1时为右键
            ref float UseStyle = ref Projectile.ai[0];
            // 使用计时器
            ref float UseCounter = ref Projectile.ai[1];
            // 第一次的计数
            ref float firstFire = ref Projectile.ai[2];

            // 开火方向
            Vector2 firedirection = Vector2.UnitX.RotatedBy(Projectile.rotation + MathHelper.ToRadians(0.7f) * Owner.direction);
            firedirection = firedirection.SafeNormalize(Vector2.UnitX);

            if (UseCounter == 0)
            {
                Owner.PickAmmo(Owner.ActiveItem(), out int Proj, out float shootSpeed, out int damage, out float knockback, out _, false);
                SoundEngine.PlaySound(SoundID.Item41, Projectile.Center);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, firedirection * shootSpeed, Proj, damage, knockback, Projectile.owner);
            }
            RecoilAnimation(ref UseCounter);
        }
        #region 后坐力动画
        public void RecoilAnimation(ref float UseCounter)
        {
            int recoilani = Owner.HeldItem.useTime;
            UseCounter++;
            if (UseCounter < recoilani)
            {
                float progress = EasingHelper.EaseInOutQuad((float)UseCounter / recoilani);
                aniXdistance = MathHelper.Lerp(0, maxXdistance, progress);
            }
            else
                UseCounter = 0;
        }
        #endregion
        #region 删除条件
        public override void DelCondition()
        {
            // 偷了个懒，用了第一次发射的判定，不过第一次发射只有+1，影响不大
            Projectile.ai[2]++;
            if (Projectile.ai[2] > Owner.HeldItem.useTime)
                Projectile.Kill();
        }
        #endregion
    }
}
