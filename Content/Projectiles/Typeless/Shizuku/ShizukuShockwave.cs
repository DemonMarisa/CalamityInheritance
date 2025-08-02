using System;
using System.Composition.Hosting.Core;
using CalamityInheritance.NPCs.Boss.SCAL.Proj;
using CalamityMod;
using CalamityMod.Projectiles.Boss;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku
{
    public class ShizukuShockwave : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public float LifeTimeCompletion => 1f - Projectile.timeLeft / (float)120;
        public Player Owner => Main.player[Projectile.owner];
        public ref float AttackType => ref Projectile.ai[0];
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 72;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.friendly = true;
            Projectile.timeLeft = 120;
            Projectile.scale = 0.001f;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            Projectile.Opacity = 1f - (float)Math.Pow(LifeTimeCompletion, 1.45);
            Projectile.scale = MathHelper.Lerp(0.5f, 12f, LifeTimeCompletion);
            Projectile.rotation += Main.rand.NextFloat(0.1f, 0.3f) * 0.012f;
            //重要：灭弹效果
            ClearBullet();
        }

        private void ClearBullet()
        {
            foreach (var proj in Main.projectile)
            {
                //1.排除所有非敌对射弹
                if (!proj.hostile)
                    continue;
                //2.排除所有在冲击波hitbox以外的射弹
                if (Math.Abs((proj.Center - Projectile.Center).Length() + proj.width) > Math.Abs(proj.Center.X - proj.width))
                    continue;
                //3.排除其他射弹，如:不造成伤害，没有视觉等
                if (proj.Opacity == 0 && proj.damage == 0 && !proj.active)
                    continue;
                //4.最终特判：终灾红月不可消除
                if (proj.type == ModContent.ProjectileType<BrimstoneMonsterLegacy>() || proj.type == ModContent.ProjectileType<BrimstoneMonster>())
                    continue;
                //5.开始灭弹
                if (!Projectile.Hitbox.Intersects(proj.Hitbox))
                    continue;

                //将所有射弹设置为0伤害, 且使射弹的视觉效果渐变为0
                proj.damage = 0;
                proj.Opacity -= 0.1f;
                //在射弹即将为0的时候释放粒子
                if (proj.Opacity < 0.1f)
                {
                    for (int i = 0; i < 10; i++)
                    {
                       int d = Dust.NewDust(proj.Center, proj.Hitbox.X, proj.Hitbox.Y, DustID.PlatinumCoin);
                        Main.dust[d].scale *= Main.rand.NextFloat(0.8f, 1.3f); 
                    }
                }
            }
        }
    }
}