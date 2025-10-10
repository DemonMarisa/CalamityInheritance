using CalamityInheritance.Content.Items.Weapons.Typeless.ShizukuItem;
using CalamityInheritance.ExtraTextures.Metaballs;
using CalamityInheritance.Sounds.Custom.Shizuku;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
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
        public bool _isGrowing
        {
            get => Projectile.ai[0] == 1f;
            set => Projectile.ai[0] = value ? 1f : 0f;
        }
        public ref float ShootStarTimer => ref Projectile.ai[1];
        public ref float SizeTimer => ref Projectile.ai[2];
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
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 40000;
        }
        public override void AI()
        {
            NPC Target = Projectile.FindClosestTarget(1800f);
            BaseEasingSize sizeStruct = new(60f, 50f, 68f);
            //刷新时间
            if (!Owner.noItems && !Owner.CCed && Owner.HeldItem.type == ModContent.ItemType<ShizukuSword>() && Owner.ownedProjectileCounts[ModContent.ProjectileType<ShizukuSwordHoldout>()] > 0)
                Projectile.timeLeft = 3;
            //控制射弹运动，在鼠标指针位置更新
            Vector2 topHead = new(Main.MouseWorld.X, Main.MouseWorld.Y);
            Vector2 distance = topHead - Projectile.Center;
            Vector2 homeDirection = distance.SafeNormalize(Vector2.UnitY);
            //这里追踪速度一会被一定程度上降低
            Vector2 newVelocity = (Projectile.velocity * 15f + homeDirection * 16f) / (15f + 1f);
            Projectile.velocity = newVelocity;
            Projectile.velocity *= 1 + 1.0f / 100;
            DrawMetaball(sizeStruct);
            if (Target != null && Target.CanBeChasedBy(Projectile))
            {
                ShootDarkStar(Target.Center);
                DrawMark(Target);
            }
        }
        private void DrawMark(NPC target)
        {
            if (Owner.ownedProjectileCounts[ModContent.ProjectileType<ShizukuStarMark>()] < 1)
            {
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<ShizukuStarMark>(), 0, 0f, Owner.whoAmI, target.whoAmI);
                proj.DamageType = DamageClass.Magic;
                proj.originalDamage = Projectile.damage;
            }
        }

   
        private void ShootDarkStar(Vector2 Target)
        {
            ShootStarTimer += 1;
            //准备发射射弹，这个算很简单了。
            Vector2 dire = (Target - Projectile.Center).SafeNormalize(Vector2.UnitX);
            if (ShootStarTimer > 60)
            {
                if (ShootStarTimer % 20 is 0)
                {
                    //每次发射消耗20点魔力值
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, dire * 23f, ModContent.ProjectileType<ShizukuStar>(), (int)(Projectile.damage * 1.5f), 0f, Owner.whoAmI);
                    proj.DamageType = DamageClass.Magic;
                    SoundStyle starToss = Utils.SelectRandom(Main.rand, ShizukuSounds.StarToss.ToArray());
                    SoundEngine.PlaySound(starToss with { MaxInstances = 0, Volume = 0.6f}, Projectile.Center);
                }
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
            //大型星
            ShizukuStarMetaball.SpawnParticle(Projectile.Center, Vector2.Zero, curSize);
        }
    }
}