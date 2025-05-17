using CalamityInheritance.Content.BaseClass;
using CalamityMod;
using System;
using Terraria.Localization;
using Terraria.ModLoader;
using CalamityInheritance.Content.Items.Weapons.Magic;
using Terraria;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Magic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Audio;
using CalamityInheritance.Sounds.Custom;
using System.IO;
using Terraria.ID;
using CalamityInheritance.NPCs;
using System.Collections.Generic;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Magic.Alpha
{
    public class AlphaWingmanHeldProj : BaseHeldProjMagic, ILocalizedModType
    {
        public override LocalizedText DisplayName => CalamityUtils.GetItemName<WingmanLegacy>();
        public enum BehaviorType
        {
            FollowMouse,
            ReturnPlayerNearBy,
            FollowEnemy,
        }
        public override float OffsetX => 0;
        public override float OffsetY => 0;
        public override float BaseOffsetY => 0;
        public override float WeaponRotation => 0;
        // 旋转速度
        public override float AimResponsiveness => 0.15f;
        public Player Owner => Main.player[Projectile.owner];
        public bool firstFrame = false;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 10000;
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
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
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
            writer.Write(Projectile.localAI[2]);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadInt32();
            Projectile.localAI[1] = reader.ReadInt32();
            Projectile.localAI[2] = reader.ReadInt32();
        }
        public NPC target = null;
        public override void HoldoutAI()
        {
            ref float attackType = ref Projectile.localAI[0];
            ref float SearchCD = ref Projectile.localAI[2];
            ref float attackTimer = ref Projectile.ai[1];
            ref float isSecondProj = ref Projectile.ai[2];
            SearchCD++;
            if (SearchCD % 30 == 0)
                target = CIFunction.FindClosestTarget(Projectile, 5000, true, true);

            attackType = (float)BehaviorType.FollowMouse;

            if (Main.mouseRight)
                attackType = (float)BehaviorType.ReturnPlayerNearBy;

            if (target != null && attackType != (float)BehaviorType.ReturnPlayerNearBy)
            {
                attackType = (float)BehaviorType.FollowEnemy;
                Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.AngleTo(target.Center), AimResponsiveness);
                Projectile.netUpdate = true;
            }
            else
                Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.AngleTo(Main.MouseWorld), AimResponsiveness);
            
            // localai1只是用来单帧判定
            if (!firstFrame && Projectile.localAI[1] == 0)
            {
                Projectile.rotation = Projectile.AngleTo(Main.MouseWorld);
                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<AlphaWingmanHeldProj2>(), Projectile.damage, Projectile.knockBack, Owner.whoAmI, Projectile.whoAmI, 0f, -1f);
                Projectile.localAI[1]++;
                Projectile.ai[0] = Main.projectile[p].whoAmI;
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
                case BehaviorType.FollowEnemy:
                    DoBehavior_FollowEnemy(ref attackTimer, target);
                    break;
                case BehaviorType.ReturnPlayerNearBy:
                    DoBehavior_ReturnPlayerNearBy(ref attackTimer, isSecondProj);
                    break;
            }
        }
        #region 发射
        public void ShootProj(float attackTimer)
        {
            // 使用旋转角度计算方向
            Vector2 Projdirection = Vector2.UnitX.RotatedBy(Projectile.rotation);
            Projdirection.SafeNormalize(Vector2.UnitX);
            // 偏移向量
            Vector2 projectileVelocity = Projdirection * 3f;

            if (attackTimer % 8 == 0)
            {
                // 使用一号位存储的数据
                SoundEngine.PlaySound(CISoundMenu.WingManFire, Projectile.Center);
                Owner.CheckMana(Owner.ActiveItem(), (int)(Owner.HeldItem.mana * Owner.manaCost), true, false);
                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, projectileVelocity, ModContent.ProjectileType<AlphaBeamEx>(), Projectile.damage, Projectile.knockBack, Owner.whoAmI, 0f, Projectile.GetByUUID(Projectile.owner, Projectile.whoAmI), 1f);
                Projectile.CalamityInheritance().ProjNewAI[0] = Main.projectile[p].whoAmI;
            }
        }
        #endregion
        #region 跟随鼠标
        public void DoBehavior_FollowMouse(ref float attackTimer)
        {
            attackTimer++;
            const float DesiredDistance = 450f;    // 期望保持的距离
            const float ApproachSpeed = 0.06f;     // 基础移动速度
            const float RepelForce = 12f;        // 反向排斥力系数
            const float slowZone = 20f; // 静止缓冲区域

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

            ShootProj(attackTimer);
            DoBehavior_FlyAway();
        }
        #endregion
        #region 跟随敌人
        public void DoBehavior_FollowEnemy(ref float attackTimer, NPC target)
        {
            attackTimer++;
            const float DesiredDistance = 550f;    // 期望保持的距离
            const float ApproachSpeed = 0.06f;     // 基础移动速度
            const float RepelForce = 12f;        // 反向排斥力系数
            const float slowZone = 10f; // 静止缓冲区域

            Vector2 targetPosition = target.Center;
            Vector2 toMouse = targetPosition - Projectile.Center;
            float distanceToMouse = toMouse.Length();
            // 处理零向量情况
            if (toMouse == Vector2.Zero)
                toMouse = Vector2.UnitY;
            // 获取标准化方向
            Vector2 direction = Vector2.Normalize(toMouse);
            // 计算目标位置（鼠标外延的期望距离点）
            Vector2 desiredPosition = targetPosition - direction * DesiredDistance;
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
                Vector2 repelVector = -direction * RepelForce * distanceRatio;
                Projectile.Center += repelVector;
                Projectile.velocity *= 1.03f;
            }
            // 当距离适中时减速
            else if (Math.Abs(distanceToMouse - DesiredDistance) < slowZone)
            {
                Projectile.velocity *= 0.97f;
            }

            ShootProj(attackTimer);
            DoBehavior_FlyAway();
        }
        #endregion
        #region 返回到玩家周围
        public void DoBehavior_ReturnPlayerNearBy(ref float attackTimer, float isSecondProj)
        {
            attackTimer++;

            Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.AngleTo(Main.MouseWorld), AimResponsiveness);

            Vector2 playeraim = Vector2.Normalize(Main.MouseWorld - Owner.Center);
            Vector2 offset = new Vector2(55, 35 * isSecondProj).RotatedBy(playeraim.ToRotation());

            Projectile.Center = Vector2.Lerp(Projectile.Center, Owner.Center + offset, AimResponsiveness);

            ShootProj(attackTimer);
        }
        #endregion
        #region 远离同类弹幕
        public void DoBehavior_FlyAway()
        {
            const float DesiredDistance = 100f;    // 期望保持的距离
            const float RepelForce = 4.8f;        // 反向排斥力系数
            const float slowZone = 10f; // 静止缓冲区域
            // 记录另一个弹幕
            int parent = (int)Projectile.ai[0];

            Vector2 anoProjPosition = Main.projectile[parent].Center;
            Vector2 toMouse = anoProjPosition - Projectile.Center;
            float distanceToMouse = toMouse.Length();
            // 处理零向量情况
            if (toMouse == Vector2.Zero)
                toMouse = Vector2.UnitY;
            // 获取标准化方向
            Vector2 direction = Vector2.Normalize(toMouse);

            if (distanceToMouse < DesiredDistance)
            {
                // 当距离过近时施加反向斥力，离得越近力越大
                float distanceRatio = 1f - (distanceToMouse / DesiredDistance);
                Vector2 repelVector = -direction * RepelForce * distanceRatio;
                Projectile.Center += repelVector;
                Projectile.velocity *= 1.03f;
            }
            // 当距离适中时减速
            else if (Math.Abs(distanceToMouse - DesiredDistance) < slowZone)
            {
                Projectile.velocity *= 0.97f;
            }
        }
        #endregion
        #region 覆写玩家效果
        public override void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
        {
            if (Projectile.localAI[0] != (float)BehaviorType.ReturnPlayerNearBy)
            {
                if (Projectile.rotation > -MathHelper.PiOver2 && Projectile.rotation < MathHelper.PiOver2)
                    Projectile.spriteDirection = 1;
                else
                    Projectile.spriteDirection = -1;
            }
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
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
        public override bool ExtraPreDraw(ref Color lightColor)
        {
            DrawLaserBeam();
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            float drawRotation = Projectile.rotation + (Projectile.spriteDirection == -1 ? 0f : MathHelper.Pi);
            Vector2 rotationPoint = texture.Size() * 0.5f;
            SpriteEffects flipSprite = Projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(texture, drawPosition, null, Projectile.GetAlpha(lightColor), drawRotation, rotationPoint, Projectile.scale * Main.player[Projectile.owner].gravDir, flipSprite);
            return false;
        }
        #endregion

        #region 绘制
        #region 绘制激光束
        public void DrawLaserBeam()
        {
            Projectile Laser = Main.projectile[(int)Projectile.CalamityInheritance().ProjNewAI[0]];
            // 基础参数
            const int laserLength = 4400;
            float alphaMultiplier = Math.Max(0, Laser.Opacity * Math.Min(1, Laser.timeLeft / 3f));
            float beamRotation = Projectile.rotation;
            float Scale = Laser.ai[2] == 1 ? 0.5f : 1.5f * Laser.ai[0];
            // 颜色
            Color baseColor = Color.White * alphaMultiplier;
            Color Auxiliarycolor = Color.DodgerBlue * alphaMultiplier;
            baseColor.A = Auxiliarycolor.A = 0;

            // 纹理
            Texture2D mainTexture = ModContent.Request<Texture2D>("CalamityInheritance/Content/Projectiles/Magic/AlphaBeam").Value;
            Texture2D bloomTexture = Main.Assets.Request<Texture2D>("Images/Extra_197").Value;
            Texture2D headTexture = Main.Assets.Request<Texture2D>("Images/Projectile_927").Value;
            Texture2D tailTexture = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Healing/EssenceFlame").Value;

            DrawGlowEffects(headTexture, Auxiliarycolor, Scale, Laser);
            DrawBloomEffect(bloomTexture, Auxiliarycolor, beamRotation, laserLength, Scale, Laser);

            DrawMainBeam(mainTexture, baseColor, beamRotation, laserLength, Scale, Laser);
            DrawTailEffect(tailTexture, baseColor, beamRotation, laserLength, Scale);
        }
        #endregion
        #region 绘制头部星星
        public void DrawGlowEffects(Texture2D headTexture, Color color, float Scale,Projectile proj)
        {
            const int glowCount = 5;
            var projAI = proj.CalamityInheritance().ProjNewAI;

            for (int i = 0; i < glowCount; i++)
            {
                Rectangle rect = new Rectangle(0, 0, headTexture.Width / 2, headTexture.Height);
                Vector2 origin = new Vector2(headTexture.Width / 2, headTexture.Height / 2);
                Vector2 scale = new Vector2(
                    proj.localAI[0] / 20f / (proj.ai[2] + 1),
                    proj.localAI[0] / 20f / (proj.ai[2] + 1));

                Main.EntitySpriteDraw(headTexture, Projectile.Center - Main.screenPosition,
                    rect, color * 0.8f, projAI[i],// projAI[i]为绘制星星时的随机旋转
                    origin, scale * Scale,
                    SpriteEffects.None);
            }
        }
        #endregion
        #region 绘制本体辉光
        public void DrawBloomEffect(Texture2D texture, Color color, float rotation, int length, float Scale, Projectile proj)
        {
            Rectangle rect = new Rectangle(0, 0, length, texture.Height);
            Vector2 scale = new Vector2(1, proj.localAI[0] / 60f / (proj.ai[2] + 2));
            Vector2 origin = new Vector2(0, texture.Height / 2);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, rect,
                color, rotation, origin, scale * Scale,
                SpriteEffects.None);
        }
        #endregion
        #region 绘制主光束
        private void DrawMainBeam(Texture2D texture, Color color, float rotation, int length, float Scale, Projectile proj)
        {
            Rectangle rect = new Rectangle(0, 0, length, texture.Height);
            Vector2 scale = new Vector2(1, proj.localAI[0] / 9f);
            Vector2 origin = new Vector2(0, texture.Height / 2);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition,
                new Rectangle(0, 0, length, texture.Height),
                color, rotation, origin, scale * Scale,
                SpriteEffects.None);
        }
        // 你画的啥
        private void DrawTailEffect(Texture2D texture, Color color, float rotation, int length, float Scale)
        {
            Vector2 tailPosition = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * (length - 18) - Main.screenPosition;
            Vector2 origin = new Vector2(texture.Width, texture.Height / 4);
            Vector2 scale = new Vector2(0.5f, 0.5f);

            Rectangle rect = new Rectangle(0, 0, length, texture.Height / 4);
            Main.EntitySpriteDraw(texture, tailPosition, rect, color,
                rotation - MathHelper.PiOver2, origin, scale * Scale,
                SpriteEffects.None);
        }
        #endregion
        #endregion
    }
}
