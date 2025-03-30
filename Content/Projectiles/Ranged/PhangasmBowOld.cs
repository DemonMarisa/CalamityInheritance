using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class PhangasmBowOld : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Ranged/PhangasmOS";

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 82;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0.7f, 0.5f);
            Player player = Main.player[Projectile.owner];
            float pi = 0f;
            Vector2 playerRotation = player.RotatedRelativePoint(player.MountedCenter, true);
            if (Projectile.spriteDirection == -1)
            {
                pi = MathHelper.Pi;
            }
            Projectile.ai[0] += 1f;
            int fireSpeed = 0;
            if (Projectile.ai[0] >= 90f)
            {
                fireSpeed++;
            }
            if (Projectile.ai[0] >= 180f)
            {
                fireSpeed++;
            }
            if (Projectile.ai[0] >= 270f)
            {
                fireSpeed++;
            }
            int delayCompare = 24;
            int fireSpeedCompare = 2;
            Projectile.ai[1] -= 1f;
            bool fullSpeed = false;
            if (Projectile.ai[1] <= 0f)
            {
                Projectile.ai[1] = (float)(delayCompare - fireSpeedCompare * fireSpeed);
                fullSpeed = true;
                int arg_1EF4_0 = (int)Projectile.ai[0] / (delayCompare - fireSpeedCompare * fireSpeed);
            }
            bool canUseItem = !player.CantUseHoldout() && player.HasAmmo(player.ActiveItem());
            if (Projectile.localAI[0] > 0f)
            {
                Projectile.localAI[0] -= 1f;
            }
            if (Projectile.soundDelay <= 0 && canUseItem)
            {
                Projectile.soundDelay = delayCompare - fireSpeedCompare * fireSpeed;
                if (Projectile.ai[0] != 1f)
                {
                    SoundEngine.PlaySound(SoundID.Item5, Projectile.position);
                }
                Projectile.localAI[0] = 12f;
            }
            player.phantasmTime = 2;
            if (fullSpeed && Main.myPlayer == Projectile.owner)
            {
                int ammoType = ProjectileID.WoodenArrowFriendly;
                float scaleFactor11 = 14f;
                int weaponDamage2 = player.GetWeaponDamage(player.ActiveItem());
                float weaponKnockback2 = player.ActiveItem().knockBack;
                if (canUseItem)
                {
                    player.PickAmmo(player.ActiveItem(), out ammoType, out scaleFactor11, out weaponDamage2, out weaponKnockback2, out _);
                    weaponKnockback2 = player.GetWeaponKnockback(player.ActiveItem(), weaponKnockback2);
                    float scaleFactor12 = player.ActiveItem().shootSpeed * Projectile.scale;
                    Vector2 shootDirection = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY) - playerRotation;
                    if (player.gravDir == -1f)
                    {
                        shootDirection.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - playerRotation.Y;
                    }
                    Vector2 normalizeShoot = Vector2.Normalize(shootDirection);
                    if (float.IsNaN(normalizeShoot.X) || float.IsNaN(normalizeShoot.Y))
                    {
                        normalizeShoot = -Vector2.UnitY;
                    }
                    normalizeShoot *= scaleFactor12;
                    if (normalizeShoot.X != Projectile.velocity.X || normalizeShoot.Y != Projectile.velocity.Y)
                    {
                        Projectile.netUpdate = true;
                    }
                    Projectile.velocity = normalizeShoot * 0.55f;
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 randNormalize = Vector2.Normalize(Projectile.velocity) * scaleFactor11 * (0.6f + Main.rand.NextFloat() * 0.8f);
                        if (float.IsNaN(randNormalize.X) || float.IsNaN(randNormalize.Y))
                        {
                            randNormalize = -Vector2.UnitY;
                        }
                        Vector2 projRandomPos = playerRotation + Utils.RandomVector2(Main.rand, -15f, 15f);
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), projRandomPos, randNormalize, ammoType, weaponDamage2, weaponKnockback2, Projectile.owner);
                        Main.projectile[proj].noDropItem = true;
                    }
                }
                else
                {
                    Projectile.Kill();
                }
            }

            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);
            UpdateAim(rrp, player.HeldItem.shootSpeed);

            Projectile.position = player.RotatedRelativePoint(player.MountedCenter, true) - Projectile.Size / 2f;
            Projectile.rotation = Projectile.velocity.ToRotation() + pi;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.timeLeft = 2;
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2((double)(Projectile.velocity.Y * (float)Projectile.direction), (double)(Projectile.velocity.X * (float)Projectile.direction));
        }

        public override bool? CanDamage() => false;
        // 这个值控制着旋转速度，越高越快
        public const float AimResponsiveness = 1f;
        public void UpdateAim(Vector2 source, float speed)
        {
            // 获取玩家当前的瞄准方向作为归一化向量

            Vector2 aim = Vector2.Normalize(Main.MouseWorld - source);
            if (aim.HasNaNs())
            {
                aim = -Vector2.UnitY;
            }

            // 改变棱镜当前速度的一部分，使其指向鼠标，这会随着时间的推移提供平滑的运动。

            aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), aim, AimResponsiveness));
            aim *= speed;

            if (aim != Projectile.velocity)
            {
                Projectile.netUpdate = true;
            }
            Projectile.velocity = aim;
        }
    }
}
