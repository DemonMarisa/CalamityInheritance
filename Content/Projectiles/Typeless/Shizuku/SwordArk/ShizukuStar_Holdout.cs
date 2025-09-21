using System;
using CalamityInheritance.Content.Items.Weapons.Typeless.ShizukuItem;
using CalamityInheritance.ExtraTextures.Metaballs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Weapons.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku.SwordArk
{
    public struct BaseEasingSize(float easingDuration, float minSize, float maxSize)
    {
        public float EasingDuration = easingDuration;
        public float MinSize = minSize;
        public float MaxSize = maxSize;
    }
    public class ShizukuStarHoldout : ModProjectile, ILocalizedModType
    {
        public Player Owner => Projectile.GetProjOwner();
        public ref float AttackTimer => ref Projectile.ai[0];
        public ref float ShootStarTimer => ref Projectile.ai[1];
        public ref float SizeTimer => ref Projectile.ai[2];
        private bool _isGrowing = true;
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override string Texture => GenericProjRoute.InvisProjRoute;
        public Vector2 _lastAnchorPos = Vector2.Zero;

        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            //大小无所谓，也不会造成伤害。
            Projectile.width = Projectile.height = 32;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            Projectile.timeLeft = 40000;
        }
        public override void OnSpawn(IEntitySource source)
        {
            // _lastAnchorPos = Projectile.Center;
            // _rotCenter = Projectile.Center;
        }
        public override void AI()
        {
            NPC Target = Projectile.FindClosestTarget(1800f);
            AttackTimer++;
            BaseEasingSize sizeStruct = new(60f, 50f, 68f);
            //刷新时间
            if (!Owner.noItems && !Owner.CCed && Owner.HeldItem.type == ModContent.ItemType<ShizukuSword>() && Owner.ownedProjectileCounts[ModContent.ProjectileType<ShizukuSwordHoldout>()] > 0)
                Projectile.timeLeft = 3;
            //控制射弹运动，在鼠标指针位置更新
            //这里射弹运动都是通过velocity实现的
            // DrawDynamicArc();
            Vector2 topHead = new(Main.MouseWorld.X, Main.MouseWorld.Y);
            Vector2 distance = topHead - Projectile.Center;
            Vector2 homeDirection = distance.SafeNormalize(Vector2.UnitY);
            Vector2 newVelocity = (Projectile.velocity * 18f + homeDirection * 16f) / (18f + 1f);
            Projectile.velocity = newVelocity;
            Projectile.velocity *= 1 + 1.0f / 100;
            DrawMetaball(sizeStruct);
            //目前排查到的问题是检索敌怪之后固定在181帧后处死自己
            Vector2 drawCenter = Main.MouseWorld;
            bool notActiveTarget = Target != null && Target.active && Target.chaseable && !Target.dontTakeDamage;
            if (notActiveTarget)
                drawCenter = Target.Center;

            ShootDarkStar(drawCenter);
            // ShootDarkStar(drawCenter);
            ShootStarTimer += 1f;
        }
        public override bool PreKill(int timeLeft)
        {
            Main.NewText($"存续时间：{AttackTimer}", Color.SkyBlue);
            Main.NewText($"拥有的星数量：{Owner.ownedProjectileCounts[Type]}个");
            return base.PreKill(timeLeft);
        }
        private void ShootDarkStar(Vector2 Target)
        {
            //准备发射射弹，这个算很简单了。
            Vector2 dire = (Target- Projectile.Center).SafeNormalize(Vector2.UnitX);
            if (ShootStarTimer > 60)
            {
                if (ShootStarTimer % 20 is 0)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, dire * 23f, ModContent.ProjectileType<ShizukuStar>(), Projectile.damage, 0f, Owner.whoAmI);
                if (ShootStarTimer > 60 + 20 * 3)
                    ShootStarTimer = 0;
            }
        }

        private void DrawMetaball(BaseEasingSize sizeStruct)
        {
            //控制Metaball的大小缓动
            if (_isGrowing)
            {
                SizeTimer += 1f / sizeStruct.EasingDuration;
                if (SizeTimer >= 1f)
                {
                    SizeTimer = 1f;
                    _isGrowing = false;
                }
            }
            else
            {
                SizeTimer -= 1f / sizeStruct.EasingDuration;
                if (SizeTimer <= 0f)
                {
                    SizeTimer = 0f;
                    _isGrowing = true;
                }
            }
            float curSize = MathHelper.Lerp(sizeStruct.MinSize, sizeStruct.MaxSize, SizeTimer);
            // Main.NewText($"{AttackTimer}");
            //大型星
            ShizukuStarMetaball.SpawnParticle(Projectile.Center, Vector2.Zero, curSize);
        }
    }
}