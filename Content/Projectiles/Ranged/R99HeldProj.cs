using System;
using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.NPCs.NormalNPCs;
using Microsoft.Build.ObjectModelRemoting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class R99HeldProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectile.Ranged";
        public override string Texture => $"{Generic.WeaponPath}/Ranged/R99";
        public Player Owner => Main.player[Projectile.owner];
        public const float OffsetX = 0f;
        public const float OffsetY = 0f;
        public enum AttackType : int
        {
            NotCrackedShield = 1,
            IsCrackedShield = 2,
            RechargingShield = 3
        };
        public ref float AttackTimer => ref Projectile.ai[0];
        public ref float AttackStyle => ref Projectile.ai[1];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.height = 74;
            Projectile.width = 172;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.MaxUpdates = 1;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void AI()
        {
            Projectile.extraUpdates = 0;
            Vector2 rrp = Owner.RotatedRelativePoint(Owner.MountedCenter, true);
            //更新手持位置
            UpdateVisual(rrp);
            if (Projectile.owner == Main.myPlayer)
            {
                UpdateAiming(rrp, 1);
                bool ifStillUse = (Owner.channel || Owner.controlUseTile) && !Owner.noItems && !Owner.CCed;
                if (ifStillUse)
                    HoldoutAI();
                else
                    DelCondition();
            }
            Projectile.timeLeft = 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 offset = new(10 * Owner.direction, -2);

            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            float drawRot = Projectile.rotation + (Projectile.spriteDirection == -1 ? MathHelper.Pi : 0);
            Vector2 rotationPoint = tex.Size() * 0.5f;
            SpriteEffects flipSprite = Projectile.spriteDirection * Main.player[Projectile.owner].gravDir == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Main.spriteBatch.Draw(tex, drawPos + offset.RotatedBy(drawRot), null, Projectile.GetAlpha(lightColor), drawRot, rotationPoint, Projectile.scale * Main.player[Projectile.owner].gravDir * 0.6f, flipSprite, default);
            return false;
        }
        private void DelCondition()
        {
            Projectile.Kill();
        }

        private void HoldoutAI()
        {
            RecoilAnimation();
            FiringBullet();
        }

        private void FiringBullet()
        {
            var src = Projectile.GetSource_FromThis();
            Vector2 fireDir = Vector2.UnitX.RotatedBy(Projectile.rotation);
            fireDir = fireDir.SafeNormalize(Vector2.UnitX);
            Vector2 dustVelocity = Projectile.velocity.SafeNormalize(Vector2.UnitY) * 15;
            Owner.PickAmmo(ActiveItem(Owner), out int Proj, out float shootSpeed, out int damage, out float kb, out int ammoID);

            Vector2 velocity = fireDir * shootSpeed;
            AttackTimer += 1f;
            if (AttackTimer % 1 == 0)
            {
                // PlayFireDust(tipPos);
                SoundStyle[] R99Sound =
                [
                    CISoundMenu.R99Fired1,
                    CISoundMenu.R99Fired2,
                    CISoundMenu.R99Fired3
                ];
                SoundStyle choose1 = Utils.SelectRandom(Main.rand, R99Sound);
                Vector2 offset = new(0, -13);
                if (AttackTimer % 2 == 0)
                {
                    SoundEngine.PlaySound(choose1 with { MaxInstances = 0, Volume = 0.64f }, Projectile.Center);
                }
                //校准枪口位置 
                Vector2 dynamicCalibra = CorrectedAngle(fireDir);
                //位置上下偏差值，这个仅用于使两颗同时发射的子弹在不同出现（虽然正常情况下看不出来）
                float yetAnotherOffset = 0.75f;
                for (int i = -1; i < 2; i += 2)
                {
                    //应用水平位置、垂直位置校准值
                    Vector2 firePos = new((Projectile.Center + offset).X + dynamicCalibra.X, (Projectile.Center + offset).Y + yetAnotherOffset * i + dynamicCalibra.Y);
                    int p = Projectile.NewProjectile(src, firePos, fireDir * shootSpeed, Proj, damage, kb, Projectile.owner);
                    Main.projectile[p].extraUpdates += 4;
                }
            }
        }
        #region 枪口子弹校准
        public Vector2 CorrectedAngle(Vector2 fireDir)
        {
            //角度的最大校准值
            float maxCalibra = 14;
            //有效校准范围(45~135)
            float angleThre = 45f;
            //速度相关的最大校准，额外补充的玩家相关
            float maxSpeedCalibration = 12f; 
            //速度对校准的影响系数
            float speedInfluenceFactor = 0.6f;
            //获取玩家移动向量并标准化处理
            Vector2 playerMoveVector = Owner.velocity;
            float moveSpeed = playerMoveVector.Length();
            //移动方向
            Vector2 normalizedMoveDir = playerMoveVector.SafeNormalize(Vector2.Zero);
            //处理玩家向量相关的校准向量。
            float speedCalibrationAmount = MathHelper.Clamp(
                moveSpeed * speedInfluenceFactor, 
                0f, 
                maxSpeedCalibration
            );
            Vector2 speedBasedCalibration = normalizedMoveDir * speedCalibrationAmount;
            //对枪口位置进行校准
            float rotDegree = MathHelper.ToDegrees(Projectile.rotation) % 360;
            if (rotDegree < 0)
                rotDegree += 360;
            //玩家朝向校准
            float playerDirCorrect = fireDir.X > 0 ? 1f : -1f;
            //算角度（与90/270）的夹角
            float angleTo90 = Math.Min(Math.Abs(rotDegree - 90), 360 - Math.Abs(rotDegree - 90));
            float angleTo270 = Math.Min(Math.Abs(rotDegree - 270), 360 - Math.Abs(rotDegree - 270));
            //校准，与校准方向
            float calibraFactor = 0f;
            float calibraDir = 1f;
            //处理90°校准
            if (angleTo90 < angleThre)
            {
                calibraFactor = 1 - (angleTo90 / angleThre);
                calibraDir = 1f;
            }
            else if (angleTo270 < angleThre)
            {
                calibraFactor = 1 - (angleTo270 / angleThre);
                calibraDir = -1f;
            }
            float dynamicCalibra = maxCalibra * calibraFactor * calibraDir * playerDirCorrect;
            //角度校准向量，只影响水平位置（垂直位置一般情况下正常）
            Vector2 dynamicCali = new Vector2(dynamicCalibra, 0f);
            //最终校准向量
            Vector2 finalCali = dynamicCali - speedBasedCalibration;
            return finalCali;
        }
        #endregion
        public static Item ActiveItem(Player player)
        {
            if (!Main.mouseItem.IsAir)
            {
                return Main.mouseItem;
            }

            return player.HeldItem;
        }

        private void RecoilAnimation()
        {
            //Shaking这个武器即可
            Projectile.position += Main.rand.NextVector2Circular(1.4f, 1.4f);
        }

        private void UpdateAiming(Vector2 rrp, float shootSpeed)
        {
            Vector2 aim = Vector2.Normalize(Main.MouseWorld - rrp);

            if (aim.HasNaNs())
                aim = -Vector2.UnitY;

            aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), aim, 0.5f));
            aim *= shootSpeed;

            if (aim != Projectile.velocity)
                Projectile.netUpdate = true;

            Projectile.velocity = aim;
        }

        private void UpdateVisual(Vector2 rrp)
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(Owner.direction);
            Projectile.Center = rrp;
            Projectile.spriteDirection = Projectile.direction;
            //刷新玩家的itemItem等让他持有射弹
            Owner.ChangeDir(Projectile.direction);
            Owner.heldProj = Projectile.whoAmI;
            Owner.itemTime = 2;
            Owner.itemAnimation = 2;
            Owner.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation() - MathHelper.PiOver2 * Owner.direction;
            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Owner.itemRotation);
        }
    }
}