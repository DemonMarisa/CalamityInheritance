using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Items.Weapons.Magic.MagicGun.Misc;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.CAWeapons.Helds
{
    public class WingmanHeldProj : CIMagicProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<WingmanLegacy>();
        public enum BehaviorType
        {
            FollowMouse,
            ReturnPlayerNearBy,
        }
        public override string Texture => GetInstance<WingmanLegacy>().Texture;
        public Player Owner => Main.player[Projectile.owner];
        public bool firstFrame = false;
        public override void SetStaticDefaults()
        {
            Projectile.AddHeldProj();
        }
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadInt32();
        }
        public override void AI()
        {
            Projectile.timeLeft = 2;

            Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.AngleTo(Owner.LocalMouseWorld()), 0.15f);

            ref float attackType = ref Projectile.ai[0];
            ref float attackTimer = ref Projectile.ai[1];

            attackType = (float)BehaviorType.FollowMouse;
            if (Owner.LAP().MouseRight)
                attackType = (float)BehaviorType.ReturnPlayerNearBy;

            if (!firstFrame)
            {
                Projectile.rotation = Projectile.AngleTo(Owner.LocalMouseWorld());
                Projectile.velocity = Vector2.Zero;
                firstFrame = true;
            }

            // Update damage based on curent magic damage stat (so Mana Sickness affects it)
            Projectile.damage = Owner.HeldItem is null ? 0 : Owner.GetWeaponDamage(Owner.HeldItem);

            switch ((BehaviorType)attackType)
            {
                case BehaviorType.FollowMouse:
                    DoBehavior_FollowMouse(ref attackTimer);
                    break;
                case BehaviorType.ReturnPlayerNearBy:
                    DoBehavior_ReturnPlayerNearBy(ref attackTimer);
                    break;
            }
            if (Owner.LAP().MouseLeft)
                Projectile.SetHeldProj(Owner, false);
            else
                Projectile.Kill();

            Vector2 vel = Projectile.rotation.ToRotationVector2();
            Projectile.spriteDirection = vel.X > 0 ? -1 : 1;
        }
        #region 跟随鼠标
        public void DoBehavior_FollowMouse(ref float attackTimer)
        {
            attackTimer++;
            const float DesiredDistance = 450f;    // 期望保持的距离
            const float ApproachSpeed = 0.06f;     // 基础移动速度
            const float RepelForce = 12f;        // 反向排斥力系数
            const float slowZone = 10f; // 静止缓冲区域

            Vector2 mousePosition = Owner.LocalMouseWorld();
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
                float distanceRatio = 1f - distanceToMouse / DesiredDistance;
                Vector2 repelVector = -direction * RepelForce * distanceRatio;
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

            if (attackTimer % 8 == 0 && Owner.CheckMana(Owner.ActiveItem(), (int)(Owner.HeldItem.mana * Owner.manaCost), true, false))
            {
                SoundEngine.PlaySound(CISounds.WingManFire, Projectile.Center);
                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, projectileVelocity, ProjectileType<AlphaBeam>(), Projectile.damage, Projectile.knockBack, Owner.whoAmI, 0.6f, Projectile.whoAmI, 1f);
            }
        }
        #endregion
        #region 返回到玩家周围
        public void DoBehavior_ReturnPlayerNearBy(ref float attackTimer)
        {
            attackTimer++;

            Vector2 playeraim = Vector2.Normalize(Owner.LocalMouseWorld() - Owner.Center);
            Vector2 offset = new Vector2(-80 * Owner.direction, -40);

            Projectile.Center = Vector2.Lerp(Projectile.Center, Owner.Center + offset, 0.15f);

            // 使用旋转角度计算方向
            Vector2 Projdirection = Vector2.UnitX.RotatedBy(Projectile.rotation);
            Projdirection.SafeNormalize(Vector2.UnitX);
            // 偏移向量
            Vector2 projectileVelocity = Projdirection * 3f;

            if (attackTimer % 8 == 0 && Owner.CheckMana(Owner.ActiveItem(), (int)(Owner.HeldItem.mana * Owner.manaCost), true, false))
            {
                SoundEngine.PlaySound(CISounds.WingManFire, Projectile.Center);
                Owner.CheckMana(Owner.ActiveItem(), (int)(Owner.HeldItem.mana * Owner.manaCost), true, false);
                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, projectileVelocity, ProjectileType<AlphaBeam>(), Projectile.damage, Projectile.knockBack, Owner.whoAmI, 0.6f, Projectile.whoAmI, 1f);
            }
        }
        #endregion
        #region 覆写绘制
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            float drawRotation = Projectile.rotation + (Projectile.spriteDirection == 1 ? MathHelper.Pi : 0f);
            Vector2 rotationPoint = texture.Size() * 0.5f;
            SpriteEffects flipSprite = Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.EntitySpriteDraw(texture, drawPosition, null, Projectile.GetAlpha(lightColor), drawRotation, rotationPoint, Projectile.scale * Main.player[Projectile.owner].gravDir, flipSprite);
            return false;
        }
        #endregion
    }
}
