using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Weapons.ExoWeapons;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Core.GlobalInstance.Players;
using CalamityInheritance.Core.Misc;
using LAP.Core.BaseClass.Projectiles;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoWeapons
{
    public class PhotovisceratorLegacyHeldProj : BaseHeldProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<Photovisceratorold>();
        public override string Texture => GetInstance<Photovisceratorold>().Texture;
        //// X偏移
        //public override float OffsetX => -2;
        //public override float OffsetY => 0;
        //public override float BaseOffsetY => 5;
        //public override float WeaponRotation => 0;
        // 弹药消耗
        public static float AmmoNotConsumeChance = 0.95f;

        public int rightUseCD = 30;
        public int leftUseCD = 2;
        public int leftBombUseCD = 10;
        public int CrystalUseCD = 1;
        // 发射偏移
        public int Xoffset = 7;
        // 发射偏移
        public int XRightoffset = 30;
        // 炸弹发射的计数
        public int BombCounts = 0;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 84;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
            RotAmount = 0.25f;
        }

        public override void ExAI()
        {
            DrawPosOffset = new Vector2(22, 0).RotatedBy(Projectile.rotation) + Vector2.UnitY * -4;
            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
            RotAmount = 0.25f;
            // 使用类型 类型为0时为左键 为1时为右键
            ref float UseStyle = ref Projectile.ai[0];
            // 使用计时器
            ref float UseCounter = ref Projectile.ai[1];
            // 第一次的计数
            ref float fireFire = ref Projectile.ai[2];
            UseCounter++;
            // 重置计时器
            if (UseCounter > 300)
                UseCounter = 0;

            // 开火方向
            Vector2 firedirection = Vector2.UnitX.RotatedBy(Projectile.rotation);
            firedirection = firedirection.SafeNormalize(Vector2.UnitX);

            if (UseCounter % CrystalUseCD == 0)
                FireCrystal(Owner, Owner.CI(), firedirection);
            UseStyle = -1;
            if (Main.mouseLeft)
                UseStyle = 0;
            else if (Main.mouseRight)
                UseStyle = 1;
            // 左键发射
            if (UseStyle == 0)
                LeftFire(ref UseCounter, firedirection, Owner);
            // 右键发射
            else if (UseStyle == 1)
                RightFire(ref UseCounter, fireFire, firedirection, Owner);

            SpawnDust(Owner);
        }
        #region 发射星流水晶
        public void FireCrystal(Player player, CIPlayer usPlayer, Vector2 mouseToPlayer)
        {
            if (player.HeldItem.ModItem is not null && player.HeldItem.ModItem is CIExoWeapon exoWeapon && exoWeapon.UseAlt && Projectile.IsLocalPlayer())
            {
                Vector2 playerToMouseVec = LAPUtilities.GetVector2(Owner.Center, player.LocalMouseWorld());
                float warpDist = Main.rand.NextFloat(60f, 120f);
                float warpAngle = Main.rand.NextFloat(-MathHelper.Pi / 2.6f, MathHelper.Pi / 2.6f);
                Vector2 warpOffset = -warpDist * playerToMouseVec.RotatedBy(warpAngle);
                Vector2 Finalposition = Main.LocalPlayer.MountedCenter + warpOffset;
                // 消耗弹药
                Owner.PickAmmo(Owner.ActiveItem(), out _, out float shootSpeed, out int damage, out float knockback, out _, Main.rand.NextFloat() <= AmmoNotConsumeChance);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Finalposition, mouseToPlayer * 18f, ProjectileType<PhotovisceratorCrystal>(), damage / 2, 0f, Projectile.owner);
            }
        }
        #endregion
        #region 左键
        public void LeftFire(ref float UseCounter, Vector2 mouseToPlayer, Player player)
        {
            bool isLoreExo = player.HeldItem.ModItem is not null && player.HeldItem.ModItem is CIExoWeapon exoWeapon && exoWeapon.UseAlt;
            SoundStyle leftClick = isLoreExo ? CISounds.ExoFlameLeft : CISoundID.SoundFlamethrower;

            if (UseDelay == 0 && Projectile.IsLocalPlayer())
            {
                SoundEngine.PlaySound(leftClick, Projectile.Center);
                Vector2 offset = new Vector2(Xoffset, 0).RotatedBy(Projectile.rotation);
                // 消耗弹药
                Owner.PickAmmo(Owner.ActiveItem(), out _, out float shootSpeed, out int damage, out float knockback, out _, Main.rand.NextFloat() <= AmmoNotConsumeChance);
                // 发射主体火焰
                for (int i = 0; i < 2; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + offset, mouseToPlayer * 18f, ProjectileType<ExoFireold>(), (int)(damage * 0.75f), knockback, player.whoAmI, 0f, 0f);
                }
                UseDelay = leftUseCD;
                BombCounts++;
            }
            if (BombCounts >= 5 && Projectile.IsLocalPlayer())
            {
                SoundEngine.PlaySound(leftClick, Projectile.Center);
                for (int i = 0; i < 2; i++)
                {
                    // 发射向量
                    Vector2 newVector = mouseToPlayer * 18f;
                    // 消耗弹药
                    Owner.PickAmmo(Owner.ActiveItem(), out _, out float shootSpeed, out int damage, out float knockback, out _, Main.rand.NextFloat() <= AmmoNotConsumeChance);
                    // 发射位置与速度向量
                    Vector2 position = Projectile.Center + newVector.ToRotation().ToRotationVector2() * 64f;
                    int yDirection = (i == 0).ToDirectionInt();
                    newVector = newVector.RotatedBy(0.2f * yDirection);
                    Projectile lightBomb = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), position, newVector, ProjectileType<ExoLightold>(), damage, knockback, player.whoAmI);

                    lightBomb.localAI[1] = yDirection;
                    lightBomb.netUpdate = true;
                }
                BombCounts = 0;
            }
        }
        #endregion
        #region 右键
        public void RightFire(ref float UseCounter, float firefire, Vector2 mouseToPlayer, Player player)
        {
            rightUseCD = (int)(25 / Owner.GetWeaponAttackSpeed(Owner.HeldItem));
            if (UseDelay == 0 && Projectile.IsLocalPlayer())
            {
                bool isLoreExo = player.HeldItem.ModItem is not null && player.HeldItem.ModItem is CIExoWeapon exoWeapon && exoWeapon.UseAlt;
                SoundStyle rightClick = isLoreExo ? CISounds.ExoFlameRight : CISoundID.SoundFlamethrower;

                SoundEngine.PlaySound(rightClick, Projectile.Center);
                Vector2 offset = new Vector2(XRightoffset, 3 * player.direction).RotatedBy(Projectile.rotation);
                // 消耗弹药
                Owner.PickAmmo(Owner.ActiveItem(), out _, out float shootSpeed, out int damage, out float knockback, out _, Main.rand.NextFloat() <= AmmoNotConsumeChance);
                // 发射主体火焰
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + offset, mouseToPlayer * 18f, ProjectileType<ExoFlareClusterold>(), (int)(damage * 0.75f), knockback, player.whoAmI, 0f, 0f);
                UseDelay = rightUseCD;
            }
        }
        #endregion
        #region 绘制
        public override void PostDraw(Color lightColor)
        {
            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
        }
        #endregion
        #region 召唤粒子
        public void SpawnDust(Player player)
        {
            Vector2 offset = new Vector2(57, 4 * player.direction).RotatedBy(Projectile.rotation);
            Vector2 velocity = new(0, -3);

            Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, CIDustID.DustTerraBlade, Projectile.velocity * 0.6f);
            dust.scale = Main.rand.NextFloat(0.7f, 1.6f);
            dust.velocity = velocity.RotatedByRandom(0.3f) * Main.rand.NextFloat(0.1f, 1f);
            dust.color = Main.rand.NextBool() ? Color.ForestGreen : Color.Green;
            dust.noGravity = true;
            dust.noLight = false;
            dust.alpha = 90;
        }
        #endregion
    }
}
