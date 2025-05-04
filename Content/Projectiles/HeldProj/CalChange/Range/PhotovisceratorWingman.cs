using CalamityInheritance.Content.BaseClass;
using CalamityInheritance.System.Configs;
using CalamityMod;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static CalamityInheritance.Utilities.CIFunction;
using static System.Net.Mime.MediaTypeNames;

namespace CalamityInheritance.Content.Projectiles.HeldProj.CalChange.Range
{
    public class PhotovisceratorWingman : BaseHeldProj, ILocalizedModType
    {
        public int Reentry = 1;
        public override LocalizedText DisplayName => CalamityUtils.GetItemName<Photoviscerator>();
        // 利用Y偏移和修改这个进度来实现动画
        public float aniYdistance = 0;
        public float maxYdistance = 80;
        public float minYdistance = -80;
        public override float OffsetX => -60;
        public override float OffsetY => aniYdistance;
        public override float BaseOffsetY => 0;
        public override float WeaponRotation => 0;
        // 旋转速度
        public override float AimResponsiveness => 0.1f;
        public Player Owner => Main.player[Projectile.owner];
        // 弹药消耗
        public static float AmmoNotConsumeChance = 0.95f;

        public float leftUseCD = 30;
        public float animationProgress = 40;
        public float AnimationCD = 20;
        public bool firstani = false;
        public bool firstFrame = false;
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
        }
        public override void HoldoutAI()
        {
            ref float timer = ref Projectile.ai[0];
            ref float animationCD = ref Projectile.ai[1];
            if(!firstFrame)
            {
                Reentry = (int)Projectile.ai[2];
                firstFrame = true;
            }

            // 动画CD
            if (animationCD > 0)
                animationCD--;

            if (!firstani)
                FirstMove(ref timer);
            else
                NorMove(ref timer, ref animationCD);

        }
        #region 覆写玩家效果
        public override void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
        {
            if (Projectile.rotation < 0f)
                Projectile.rotation += MathHelper.TwoPi;
            else if (Projectile.rotation > MathHelper.TwoPi)
                Projectile.rotation -= MathHelper.TwoPi; //确保转角一直在2pi内

            Vector2 Projaim = Vector2.Normalize(Main.MouseWorld - Projectile.Center);
            Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.AngleTo(Main.MouseWorld), AimResponsiveness);

            Vector2 playeraim = Vector2.Normalize(Main.MouseWorld - player.Center);
            Vector2 offset = new Vector2(OffsetX, OffsetY).RotatedBy(playeraim.ToRotation());

            Projectile.Center = Vector2.Lerp(Projectile.Center, player.Center + offset, AimResponsiveness);
        }
        #endregion
        #region 覆写指向目标
        public override void UpdateAim(Vector2 source, float speed)
        {
        }

        #endregion
        #region 移动
        // 处理第一次移动
        public void FirstMove(ref float timer)
        {
            if (timer < animationProgress)
            {
                timer++;
                float progress = EasingHelper.EaseOutExpo((float)timer / animationProgress);
                aniYdistance = MathHelper.Lerp(0, maxYdistance * Reentry, progress);
            }
            else
            {
                timer = 0;
                firstani = true;
            }
        }
        // 处理后续移动
        public void NorMove(ref float timer, ref float CD)
        {
            // 有CD不会处理移动
            if (CD <= 0)
            {
                timer++;
                if (timer < animationProgress)
                {
                    float progress = EasingHelper.EaseOutExpo((float)timer / animationProgress);
                    aniYdistance = MathHelper.Lerp(maxYdistance * Reentry, minYdistance * Reentry, progress);
                }

                if (timer >= animationProgress)
                {
                    if (Reentry == 1)
                        Reentry = -1;
                    else
                        Reentry = 1;

                    timer = 0;
                    CD = AnimationCD;
                    Shoot();
                }
            }
        }
        #endregion
        #region 发射
        public void Shoot()
        {
            // Consume ammo and retrieve projectile stats; has a chance to not consume ammo
            Owner.PickAmmo(Owner.ActiveItem(), out _, out float shootSpeed, out int damage, out float knockback, out _, Main.rand.NextFloat() <= AmmoNotConsumeChance);

            Vector2 Projaim = Main.MouseWorld - Projectile.Center;
            Projaim.SafeNormalize(Vector2.UnitX);

            Vector2 velocity = Projaim * shootSpeed;

            for (int i = 0; i < 2; i++)
            {
                Vector2 bombPos = Projectile.Center;
                int yDirection = (i == 0).ToDirectionInt();
                Vector2 bombVel = velocity.RotatedBy(0.2f * yDirection);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), bombPos, bombVel * 0.01f, ModContent.ProjectileType<ExoLight>(), damage, knockback, Projectile.owner, yDirection);
            }
        }
        #endregion
    }
}
