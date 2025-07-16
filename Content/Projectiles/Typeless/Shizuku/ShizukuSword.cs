using System;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.Content.Items.Weapons.Typeless;
using CalamityInheritance.Utilities;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku
{
    public class ShizukuSwordProjectile : ModProjectile
    {
        public Player Owner => Main.player[Projectile.owner];
        public override string Texture => $"{Generic.WeaponPath}/Typeless/ShizukuItem/ShizukuSword";
        //目前没有作用，我认为应该去掉。
        public int TargetIndex
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public ref float AttackTimer => ref Projectile.ai[1];
        public ref float AttackType => ref Projectile.ai[2];
        #region AttackType
        const float IsSpawning = 0f;
        const float IsAngleTo = 1f;
        const float IsDashing = 2f;
        #endregion
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
        }
        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 112;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.friendly = true;
            Projectile.timeLeft = 18000;
            Projectile.tileCollide = false;
            Projectile.Opacity = 0f;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }
        public override bool? CanDamage()
        {
            return AttackType == IsDashing;
        }
        public override void AI()
        {
            //暂时禁用，这个射弹最后会和Shizuku Edge融为一体
            // CheckHeldItem();
            NPC anyTarget = Projectile.FindClosestTarget(CIFunction.SetDistance(100));
            switch (AttackType)
            {
                case IsSpawning:
                    DoSpawning(anyTarget);
                    break;
                case IsAngleTo:
                    DoAngleTo(anyTarget);
                    break;
                case IsDashing:
                    DoDashing(anyTarget);
                    break;
            }
        }
        //拖尾粒子
        private void TrailingDust()
        {
            if (Main.rand.NextBool(4))
            {
                int dustType = Main.rand.NextBool(5) ? 226 : 220;
                float scale = 0.8f + Main.rand.NextFloat(0.3f);
                float velocityMult = Main.rand.NextFloat(0.3f, 0.6f);
                int idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity = velocityMult * Projectile.velocity;
                Main.dust[idx].scale = scale;
            }
        }

        private void DoDashing(NPC anyTarget)
        {
            if (anyTarget is null)
                return;
            //dashing.
            TrailingDust();
            AttackTimer++;
            float homingDistance = CIFunction.SetDistance(200);
            Projectile.HomingNPCBetter(anyTarget, homingDistance, 20f + AttackTimer / 10f, 20f, 2);
        }

        private void DoAngleTo(NPC anyTarget)
        {
            if (anyTarget is null)
                return;
            float shouldAngleTo = Projectile.AngleTo(anyTarget.Center) + MathHelper.PiOver4;
            //这个没生效。
            if (Projectile.rotation == shouldAngleTo)
                AttackTimer = 25f;
            
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, shouldAngleTo, 0.15f);
            AttackTimer++;
            if (AttackTimer > 25f)
            {
                Projectile.netUpdate = true;
                AttackType = IsDashing;
                AttackTimer = 0f;
            }
        }

        private void DoSpawning(NPC anyTarget)
        {

            //减速
            Projectile.velocity *= 0.92f;
            //渐变
            Projectile.Opacity += 0.05f;
            if (anyTarget is not null)
            {
                float shouldAngleTo = Projectile.AngleTo(anyTarget.Center) + MathHelper.PiOver4;
                Projectile.rotation = Utils.AngleLerp(Projectile.rotation, shouldAngleTo, 0.10f);
            }
            else Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            TrailingDust();
            //查阅速度与Opacity是否达到要求。
            if (Projectile.Opacity >= 1 && Projectile.velocity.Length() <= 0.4f)
            {
                Projectile.netUpdate = true;
                AttackType = IsAngleTo;
            }

        }

        public void DoAttacking()
        {
            NPC target = Main.npc[TargetIndex];

        }

        public void IdlePosition()
        {

        }

        public void CheckHeldItem()
        {
            //if (Owner.HeldItem.type != ModContent.ItemType<ShizukuEdge>())
            //{
            //    Projectile.Kill();
            //}
        }
    }
}