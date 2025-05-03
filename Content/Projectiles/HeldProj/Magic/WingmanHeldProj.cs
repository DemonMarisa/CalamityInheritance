using CalamityInheritance.Content.BaseClass;
using CalamityMod;
using System;
using Terraria.Localization;
using Terraria.ModLoader;
using CalamityInheritance.Content.Items.Weapons.Magic;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Magic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using CalamityInheritance.Sounds.Custom;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Magic
{
    public class WingmanHeldProj : BaseHeldProj, ILocalizedModType
    {
        public override LocalizedText DisplayName => CalamityUtils.GetItemName<WingmanLegacy>();
        public enum BehaviorType
        {
            FollowMouse,
            ReturnPlayerNearBy,
        }
        public override float OffsetX => 0;
        public override float OffsetY => 0;
        public override float BaseOffsetY => 0;
        public override float WeaponRotation => 0;
        // 旋转速度
        public override float AimResponsiveness => 0.15f;
        // public override string TexturePath => "Magic/WingmanLegacy";
        public Player Owner => Main.player[Projectile.owner];
        public bool firstFrame = false;
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
        }

        public override void HoldoutAI()
        {
            ref float attackType = ref Projectile.ai[0];
            ref float attackTimer = ref Projectile.ai[1];

            attackType = (float)BehaviorType.FollowMouse;
            if (Main.mouseRight)
                attackType = (float)BehaviorType.ReturnPlayerNearBy;

            if (!firstFrame)
            {
                Projectile.velocity = Vector2.Zero;
                firstFrame = true;
            }

            // Update damage based on curent magic damage stat (so Mana Sickness affects it)
            Projectile.damage = Owner.HeldItem is null ? 0 : Owner.GetWeaponDamage(Owner.HeldItem);

            if (Main.mouseRight)
                Projectile.damage = (int)(Owner.HeldItem is null ? 0 : Owner.GetWeaponDamage(Owner.HeldItem) * 1.2f);

            switch ((BehaviorType)attackType)
            {
                case BehaviorType.FollowMouse:
                    DoBehavior_FollowMouse(ref attackTimer);
                    break;
                case BehaviorType.ReturnPlayerNearBy:
                    DoBehavior_ReturnPlayerNearBy(ref attackTimer);
                    break;
            }
        }
        #region 跟随鼠标
        public void DoBehavior_FollowMouse(ref float attackTimer)
        {
            attackTimer++;
            const float DesiredDistance = 450f;    // 期望保持的距离
            const float ApproachSpeed = 0.06f;     // 基础移动速度
            const float RepelForce = 12f;        // 反向排斥力系数
            const float slowZone = 10f; // 静止缓冲区域

            Vector2 mousePosition = Main.MouseWorld;
            Vector2 toMouse = mousePosition - Projectile.Center;
            float distanceToMouse = toMouse.Length();
            // 处理零向量情况
            if (toMouse == Vector2.Zero)
                toMouse = Vector2.UnitY;
            // 获取标准化方向
            Vector2 direction = Vector2.Normalize(toMouse);
            // 计算目标位置（鼠标外延的期望距离点）
            Vector2 desiredPosition = mousePosition - direction * DesiredDistance;
            // 根据距离动态调整移动方式
            if (distanceToMouse > DesiredDistance)
            {
                // 向目标位置移动
                Projectile.Center = Vector2.Lerp(Projectile.Center, desiredPosition, ApproachSpeed);
            }
            
            else if (distanceToMouse < DesiredDistance)
            {
                // 当距离过近时施加反向斥力，离得越近力越大
                float distanceRatio = 1f - (distanceToMouse / DesiredDistance);
                Vector2 repelVector = - direction * RepelForce * distanceRatio;
                Projectile.Center += repelVector;
                Projectile.velocity *= 1.03f;
            }
            // 当距离适中时减速
            else if (Math.Abs(distanceToMouse - DesiredDistance) < slowZone)
            {
                Projectile.velocity *= 0.97f;
            }

            // 使用旋转角度计算方向
            Vector2 Projdirection = Vector2.UnitX.RotatedBy(Projectile.rotation);
            Projdirection.SafeNormalize(Vector2.UnitX);
            // 偏移向量
            Vector2 projectileVelocity = Projdirection * 3f;

            if (attackTimer % 8 == 0)
            {
                SoundEngine.PlaySound(CISoundMenu.WingManFire, Projectile.Center);
                Owner.CheckMana(Owner.ActiveItem(), (int)(Owner.HeldItem.mana * Owner.manaCost), true, false);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, projectileVelocity, ModContent.ProjectileType<AlphaBeam>(), Projectile.damage, Projectile.knockBack, Owner.whoAmI, 0f, Projectile.whoAmI, 1f);
            }
        }
        #endregion
        #region 返回到玩家周围
        public void DoBehavior_ReturnPlayerNearBy(ref float attackTimer)
        {
            attackTimer++;

            Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.AngleTo(Main.MouseWorld), AimResponsiveness);

            Vector2 playeraim = Vector2.Normalize(Main.MouseWorld - Owner.Center);
            Vector2 offset = new Vector2(-80 * Owner.direction, -40);

            Projectile.Center = Vector2.Lerp(Projectile.Center, Owner.Center + offset, AimResponsiveness);

            // 使用旋转角度计算方向
            Vector2 Projdirection = Vector2.UnitX.RotatedBy(Projectile.rotation);
            Projdirection.SafeNormalize(Vector2.UnitX);
            // 偏移向量
            Vector2 projectileVelocity = Projdirection * 3f;

            if (attackTimer % 8 == 0)
            {
                SoundEngine.PlaySound(CISoundMenu.WingManFire, Projectile.Center);
                Owner.CheckMana(Owner.ActiveItem(), (int)(Owner.HeldItem.mana * Owner.manaCost), true, false);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, projectileVelocity, ModContent.ProjectileType<AlphaBeam>(), Projectile.damage, Projectile.knockBack, Owner.whoAmI, 0f, Projectile.whoAmI, 1f);
            }
        }
        #endregion
        #region 覆写玩家效果
        public override void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
        {
            float attackType = Projectile.ai[0];
            // 从弹幕中心指向鼠标中心
            Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.AngleTo(Main.MouseWorld), AimResponsiveness);
            if (Main.MouseWorld.X < Projectile.Center.X)
                Projectile.spriteDirection = -1;
            else
                Projectile.spriteDirection = 1;
        }
        #endregion 
        #region 覆写指向目标
        public override void UpdateAim(Vector2 source, float speed)
        {
        }
        #endregion
        #region 覆写绘制
        public override bool ExtraPreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            float drawRotation = Projectile.rotation + (Projectile.spriteDirection == -1 ? MathHelper.Pi : 0f);
            Vector2 rotationPoint = texture.Size() * 0.5f;
            SpriteEffects flipSprite = (Projectile.spriteDirection == -1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.EntitySpriteDraw(texture, drawPosition, null, Projectile.GetAlpha(lightColor), drawRotation, rotationPoint, Projectile.scale * Main.player[Projectile.owner].gravDir, flipSprite);
            return false;
        }
        #endregion
    }
}
