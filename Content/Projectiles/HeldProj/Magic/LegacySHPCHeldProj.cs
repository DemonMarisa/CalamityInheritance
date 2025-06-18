using CalamityInheritance.Content.BaseClass;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CalamityInheritance.Utilities.CIFunction;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.Content.Projectiles.Magic;
using Mono.Cecil;
using static System.Net.Mime.MediaTypeNames;
using CalamityMod.Sounds;
using CalamityInheritance.System.Configs;
using static tModPorter.ProgressUpdate;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Magic
{
    public class LegacySHPCHeldProj : BaseHeldProjMagic, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public int maxXdistance = 3;
        public float aniXdistance = 0;
        public override float OffsetX => -10 + aniXdistance;
        public override float OffsetY => -12;
        public override float BaseOffsetY => 0;
        public override float WeaponRotation => 0;
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
            Projectile.width = 96;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }
        public override void HoldoutAI()
        {
            Projectile.extraUpdates = 0;
            // 使用类型 类型为0时为左键 为1时为右键
            ref float UseStyle = ref Projectile.ai[0];
            // 使用计时器
            ref float UseCounter = ref Projectile.ai[1];
            // 第一次的计数
            ref float firstFire = ref Projectile.ai[2];
            if (UseCounter == 0 && Owner.CheckMana(Owner.ActiveItem(), (int)(Owner.HeldItem.mana * Owner.manaCost), true, false))
            {
                CustomShoot();
            }
            RecoilAnimation(ref UseCounter);
        }
        public void CustomShoot()
        {
            Owner.PickAmmo(Owner.ActiveItem(), out _, out float shootSpeed, out int damage, out float knockback, out _, Main.rand.NextFloat() <= 0);
            Vector2 Projdirection = Vector2.UnitX.RotatedBy(Projectile.rotation);
            Projdirection.SafeNormalize(Vector2.UnitX);
            Projdirection *= shootSpeed / 2;

            Player player = Main.player[Projectile.owner];
            var p = player.CIMod();
            int bCounts = 3;
            int lCounts = 3;
            if (Projectile.ai[0] == 0)
            {
                SoundEngine.PlaySound(SoundID.Item92, Projectile.Center);
                maxXdistance = 2;
                for (int i = 0; i < lCounts; i++)
                {
                    float velX = Projdirection.X + Main.rand.Next(-20, 21) * 0.05f;
                    float velY = Projdirection.Y + Main.rand.Next(-20, 21) * 0.05f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new(velX, velY), ModContent.ProjectileType<DestroyerLegendaryLaser>(), damage, knockback * 0.5f, player.whoAmI, 0f, 0f);
                }
            }
            else
            {
                SoundEngine.PlaySound(CommonCalamitySounds.LaserCannonSound, Projectile.Center);
                maxXdistance = 3;
                for (int j = 0; j < bCounts; j++)
                {
                    float velX = Projdirection.X + Main.rand.Next(-40, 41) * 0.05f;
                    float velY = Projdirection.Y + Main.rand.Next(-40, 41) * 0.05f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new(velX, velY), ModContent.ProjectileType<DestroyerLegendaryBomb>(), (int)(damage * 1.1), knockback, player.whoAmI, 0f, 0f);
                }
            }
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

            aniXdistance = MathHelper.Lerp(aniXdistance, maxXdistance, 0.08f);
        }
        #endregion
    }
}
