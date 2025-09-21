using System;
using CalamityInheritance.Content.Items.Weapons.Typeless.ShizukuItem;
using CalamityInheritance.ExtraTextures.Metaballs;
using CalamityInheritance.Utilities;
using CalamityMod;
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
        public ref float SizeTimer => ref Projectile.ai[2];
        public int FireDelay = 0;
        public int ShootStarTimer = 0;
        private bool _isGrowing = true;
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override string Texture => GenericProjRoute.InvisProjRoute;
        public Vector2 _lastAnchorPos = Vector2.Zero;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
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
        public override void AI()
        {
            AttackTimer++;
            BaseEasingSize sizeStruct = new(60f, 50f, 68f);
            //刷新时间
            if (Owner.channel && !Owner.noItems && !Owner.CCed)
                Projectile.timeLeft = 2;
            //控制射弹运动，在鼠标指针位置更新
            Vector2 topHead = new(Main.MouseWorld.X, Main.MouseWorld.Y);
            Vector2 distance = topHead - Projectile.Center;
            Vector2 homeDirection = distance.SafeNormalize(Vector2.UnitY);
            Vector2 newVelocity = (Projectile.velocity * 18f + homeDirection * 16f) / (18f + 1f);
            Projectile.velocity = newVelocity;
            Projectile.velocity *= 1 + 1.0f / 100;
            DrawMetaball(sizeStruct);
            ShootDarkStar();
        }

        private void UpdateOwner()
        {
            Owner.itemTime = 2;
            Owner.itemAnimation = 2;
            Owner.channel = true;
        }

        public override bool PreKill(int timeLeft)
        {
            Main.NewText($"存续时间：{AttackTimer}", Color.SkyBlue);
            Main.NewText($"拥有的星数量：{Owner.ownedProjectileCounts[Type]}个");
            Main.NewText($"武器状态：{Owner.CIMod().ShizukuSwordStyle}");
            return base.PreKill(timeLeft);
        }
        private void ShootDarkStar()
        {
            //准备发射射弹，这个算很简单了。
            float baseAngle = Projectile.velocity.ToRotation();
            float count = 8;
            float spreadAngle = MathHelper.TwoPi;
            float step = spreadAngle / count;
            float radStart = 22.5f + 45 * Main.rand.Next(0, 9);
            float curAngle = baseAngle - spreadAngle / 2 + (step * ShootStarTimer) + MathHelper.ToRadians(radStart);
            Vector2 dire = new((float)Math.Cos(curAngle), (float)Math.Sin(curAngle));
            if (FireDelay is 0 && ShootStarTimer < count)
            {
                Projectile.NewProjectile(Terraria.Entity.GetSource_None(), Projectile.Center, dire * 23f, ModContent.ProjectileType<ShizukuStar>(), Projectile.damage, 0f, Owner.whoAmI);
                FireDelay = 3;
                ShootStarTimer += 1;
                Projectile.netUpdate = true;
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