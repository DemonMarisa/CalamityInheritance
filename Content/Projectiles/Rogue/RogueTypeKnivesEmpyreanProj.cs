using System;
using CalamityMod;
using CalamityMod.Projectiles;
using LAP.Content.Projectiles.LifeStealProj;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    [LegacyName("EmpyreanKnivesProjectileLegacyRogue")]
    public class RogueTypeKnivesEmpyreanProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public static readonly int GodSlayerKnivesLifeStealCap = 500;
        public static readonly int GodSlayerKnivesLifeTime = 900;
        public static readonly float GodSlayerKnivesLifeStealRange = 3000f;
        public static readonly float GodSlayerKnivesChasingSpeed = 9f;
        public static readonly float GodSlayerKnivesChasingRange = 1500f;
        //更逆天的追踪速度与追踪距离，同时三倍其存在时间
        private int bounce = 3;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.penetrate = 1;
            Projectile.timeLeft = 900;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 360f)
            {
                Projectile.alpha += 10;
                Projectile.damage = (int)(Projectile.damage * 0.95);
                Projectile.knockBack = Projectile.knockBack * 0.95f;
                if (Projectile.alpha >= 255)
                    Projectile.active = false;
            }
            if (Projectile.ai[0] < 360f)
                Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
            else
            {
                Projectile.rotation += 0.5f;
            }
            CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, GodSlayerKnivesChasingRange, GodSlayerKnivesChasingSpeed, 20f);
            if (Main.rand.NextBool(6))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Enchanted_Pink, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bounce--;
            if (bounce <= 0)
                Projectile.Kill();
            else
            {
                if (Projectile.velocity.X != oldVelocity.X)
                    Projectile.velocity.X = -oldVelocity.X;
                if (Projectile.velocity.Y != oldVelocity.Y)
                    Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                int empyreanDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Enchanted_Pink, 0f, 0f, 100, default, 0.8f);
                Main.dust[empyreanDust].noGravity = true;
                Main.dust[empyreanDust].velocity *= 1.2f;
                Main.dust[empyreanDust].velocity -= Projectile.oldVelocity * 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.Owner().SpawnLifeStealProj(target, Projectile, ModContent.ProjectileType<StandardHealProj>(), -1);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
