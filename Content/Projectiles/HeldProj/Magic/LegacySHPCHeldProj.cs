using CalamityInheritance.Content.BaseClass;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.System.Configs;
using CalamityMod;
using CalamityMod.Sounds;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static CalamityInheritance.Utilities.CIFunction;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Magic
{
    public class LegacySHPCHeldProj : BaseHeldProjMagic, ILocalizedModType
    {
        public override string Texture => GetInstance<DestroyerLegendary>().Texture;
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public float aniXdistance = 0;
        public override float OffsetX => -10 - aniXdistance;
        public override float OffsetY => -12;
        public override float BaseOffsetY => 0;
        public override float WeaponRotation => 0;

        public int LeftCD = 49;
        public int RightCD = 7;
        // 旋转速度
        public override float AimResponsiveness => 0.5f;
        public Player Owner => Main.player[Projectile.owner]; 
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
            Projectile.AddHeldProj();
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

            if (Main.mouseLeft)
                UseStyle = 0;
            else
                UseStyle = 1;

            if (UseStyle == 0)
                LeftShoot();
            else
                RightShoot();

            aniXdistance = MathHelper.Lerp(aniXdistance, 0, 0.25f);
            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
        }
        public void RightShoot()
        {
            if (UseDelay == 0)
            {
                Owner.CheckMana(Owner.ActiveItem(), (int)(Owner.HeldItem.mana * Owner.manaCost * 0.33f), true, false);
                Owner.PickAmmo(Owner.ActiveItem(), out _, out float shootSpeed, out int damage, out float knockback, out _, Main.rand.NextFloat() <= 0);
                Vector2 Projdirection = Vector2.UnitX.RotatedBy(Projectile.rotation);
                Projdirection.SafeNormalize(Vector2.UnitX);
                Projdirection *= shootSpeed / 2;

                Player player = Main.player[Projectile.owner];
                var p = player.CIMod();
                int lCounts = 3;

                SoundEngine.PlaySound(SoundID.Item92, Projectile.Center);
                aniXdistance = 2;
                for (int i = 0; i < lCounts; i++)
                {

                    float velX = Projdirection.X + Main.rand.Next(-20, 21) * 0.05f;
                    float velY = Projdirection.Y + Main.rand.Next(-20, 21) * 0.05f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new(velX, velY), ProjectileType<DestroyerLegendaryLaser>(), Projectile.damage, knockback * 0.5f, player.whoAmI, 0f, 0f);
                }
                UseDelay = RightCD;
            }
        }

        public void LeftShoot()
        {
            if (UseDelay == 0)
            {
                Owner.CheckMana(Owner.ActiveItem(), (int)(Owner.HeldItem.mana * Owner.manaCost), true, false);
                Owner.PickAmmo(Owner.ActiveItem(), out _, out float shootSpeed, out int damage, out float knockback, out _, Main.rand.NextFloat() <= 0);
                Vector2 Projdirection = Vector2.UnitX.RotatedBy(Projectile.rotation);
                Projdirection.SafeNormalize(Vector2.UnitX);
                Projdirection *= shootSpeed / 2;

                Player player = Main.player[Projectile.owner];
                var p = player.CIMod();
                int bCounts = 3;

                SoundEngine.PlaySound(CommonCalamitySounds.LaserCannonSound, Projectile.Center);
                aniXdistance = 4;
                for (int j = 0; j < bCounts; j++)
                {
                    float velX = Projdirection.X + Main.rand.Next(-40, 41) * 0.05f;
                    float velY = Projdirection.Y + Main.rand.Next(-40, 41) * 0.05f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new(velX, velY), ProjectileType<DestroyerLegendaryBomb>(), (int)(Projectile.damage * 1.1), knockback, player.whoAmI, 0f, 0f);
                }
                UseDelay = LeftCD;
            }
        }

        #region 删除条件
        public override void DelCondition()
        {
            // 偷了个懒，用了第一次发射的判定，不过第一次发射只有+1，影响不大
            Projectile.ai[2]++;
            if (Projectile.ai[2] > Owner.HeldItem.useTime || Owner.dead)
                Projectile.Kill();

            aniXdistance = MathHelper.Lerp(aniXdistance, 0, 0.08f);
        }
        #endregion
    }
}
