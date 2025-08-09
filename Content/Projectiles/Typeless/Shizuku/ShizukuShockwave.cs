using System;
using System.Composition.Hosting.Core;
using CalamityInheritance.NPCs.Boss.SCAL.Proj;
using CalamityMod;
using CalamityMod.Projectiles.Boss;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku
{
    public class ShizukuShockwave : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public float LifeTimeCompletion => 1f - Projectile.timeLeft / (float)120;
        public override string Texture => "CalamityMod/Projectiles/Typeless/ChlorophyteLifePulse";
        public Player Owner => Main.player[Projectile.owner];
        public ref float AttackType => ref Projectile.ai[0];
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 96;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.friendly = true;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(Color.Aqua, Color.Aquamarine, 1f - Projectile.Opacity) * Projectile.Opacity * 0.67f;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Color drawColor = Projectile.GetAlpha(lightColor) * 0.4f;
            for (int i = 0; i < 8; i++)
            {
                Vector2 drawOffset = (MathHelper.TwoPi * i / 8f).ToRotationVector2() * 4f;
                Vector2 drawPosition = Projectile.Center - Main.screenPosition + drawOffset;
                Main.EntitySpriteDraw(texture, drawPosition, null, drawColor, 0f, texture.Size() * 0.5f, Projectile.scale, 0, 0);
            }
            return false;
        }
        public override void AI()
        {
            Projectile.Opacity = 1f - (float)Math.Pow(LifeTimeCompletion, 1.45);
            Projectile.scale = MathHelper.Lerp(0.5f, 12f, LifeTimeCompletion);


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
                //3.排除其他射弹，如:不造成伤害，没有视觉等
                if (proj.Opacity == 0 && proj.damage == 0 && !proj.active)
                    continue;
                //4.最终特判：终灾红月不可消除
                if (proj.type == ModContent.ProjectileType<BrimstoneMonsterLegacy>() || proj.type == ModContent.ProjectileType<BrimstoneMonster>())
                    continue;
                //5.开始灭弹
                if (!Projectile.Hitbox.Intersects(proj.Hitbox))
                    continue;

                //粒子
                for (int i = 0; i < 10; i++)
                {
                    int d = Dust.NewDust(proj.Center, proj.Hitbox.X, proj.Hitbox.Y, DustID.PlatinumCoin);
                    Main.dust[d].scale *= Main.rand.NextFloat(0.8f, 1.3f); 
                }
                //做掉射弹
                proj.Kill();
            }
        }
    }
}