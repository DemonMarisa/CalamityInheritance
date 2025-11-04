using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using Terraria.Audio;
using CalamityInheritance.Sounds.Custom;
using LAP.Core.Utilities;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class BlueAura : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";

        public int MaxDistance = 450;
        public bool BeginReturn = false;
        public float FollowVector = 14f;
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(CISoundMenu.AncientShivProjSpawn, Projectile.position);
            CIFunction.DustCircle(Projectile.Center, 24, 1, DustID.MagicMirror, false, 0, 255, 0, 6f);
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(CISoundMenu.AncientShivProjSpawn, Projectile.position);
            CIFunction.DustCircle(Projectile.Center, 24, 1, DustID.MagicMirror, false, 0, 255, 0, 6f);
        }

        public override void AI()
        {
            Projectile.timeLeft = 300;
            // 弹幕拥有者
            Player owner = Main.player[Projectile.owner];

            for (int num468 = 0; num468 < 4; num468++)
            {
                int num469 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.MagicMirror, 0f, 0f, 100, default, 1.3f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 0f;
            }

            if (BeginReturn)
            {
                ReturnAI(owner);
                if (Projectile.Hitbox.Intersects(owner.Hitbox))
                    Projectile.Kill();
                return;
            }

            // 鼠标与玩家的距离
            float distanceToPlayer = Vector2.Distance(Owner.LocalMouseWorld(), owner.Center);
            // 玩家到鼠标的位置
            float PlayerToMouse = (Owner.LocalMouseWorld() - owner.Center).ToRotation();
            // 设置距离
            Vector2 Distance = new(MaxDistance, 0);
            Vector2 targetPos;
            if (distanceToPlayer > MaxDistance)
            {
                // 根据距离与旋转确定目标位置
                // 如果距离玩家过远，则限制在一定距离
                targetPos = owner.Center + Distance.RotatedBy(PlayerToMouse);
                // 检测与目标的距离
                float DistanceToTarget = (targetPos - Projectile.Center).Length();
                // 如果距离小于当前速度，则直接设置速度为目标位置到中心的向量
                if (DistanceToTarget < FollowVector)
                {
                    Projectile.velocity = Vector2.Zero;
                    Projectile.Center = targetPos;
                }
                else
                    MoveToTarget(targetPos);
            }
            else
            {
                // 否则跟随鼠标
                targetPos = Owner.LocalMouseWorld();

                MoveToTarget(targetPos);
                // 如果距离小于当前速度，则直接设置速度为目标位置到中心的向量
                // 检测与目标的距离
                float DistanceToTarget = (targetPos - Projectile.Center).Length();
                if (DistanceToTarget < Projectile.velocity.Length())
                {
                    Projectile.velocity = targetPos - Projectile.Center;
                }
            }

            if (owner.HeldItem.type != ModContent.ItemType<AncientShiv>() || !Owner.LAP().MouseLeft)
            {
                BeginReturn = true;
            }
        }
        public void MoveToTarget(Vector2 targetPos)
        {
            // 开始跟随鼠标
            // 插值目标坐标
            Vector2 newVec = Vector2.Lerp(Projectile.Center, targetPos, 0.25f);
            // 应用向量
            Projectile.velocity = (newVec - Projectile.Center).SafeNormalize(Vector2.UnitY) * FollowVector;
        }
        public void ReturnAI(Player player)
        {
            CIFunction.HomeInPlayer(player, Projectile, 0, 18f, 2);
        }
    }
}
