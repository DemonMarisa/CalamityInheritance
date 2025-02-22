using System;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles;
using CalamityMod.Projectiles.Healing;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    [LegacyName("ShadowspecKnivesProjectileLegacyRogue")]
    public class RogueTypeKnivesShadowspecProj: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public static readonly int ShadowknivesLifeStealCap = 1000;
        public static readonly float ShadowknivesLifeStealRange = 15000;
        public static readonly float ShadowknivesChasingSpeed = 12f;
        public static readonly float ShadowknivesChasingRange = 2000f;
        //更逆天的索敌速度与索敌距离。以及回血。
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.penetrate = 1;
            Projectile.timeLeft = 450;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 240f)
            {
                Projectile.alpha += 4;
                Projectile.damage = (int)(Projectile.damage * 1.05);
                Projectile.knockBack = (int)(Projectile.knockBack * 0.95);
            }
            if (Projectile.ai[0] < 240f)
                Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
            else
            {
                Projectile.rotation += 0.5f;
            }
            CalamityUtils.HomeInOnNPC(Projectile, !Projectile.tileCollide, ShadowknivesChasingRange, ShadowknivesChasingSpeed, 20f);
            if (Main.rand.NextBool(6))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PurificationPowder, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
        }

        //Give it a custom hitbox shape so it may remain rectangular and elongated
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;
            float bladeHalfLength = 25f * Projectile.scale / 2f;
            float bladeWidth = 14f * Projectile.scale;

            Vector2 direction = (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2();

            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - direction * bladeHalfLength, Projectile.Center + direction * bladeHalfLength, bladeWidth, ref collisionPoint);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                int illustrious = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurificationPowder, 0f, 0f, 100, default, 0.8f);
                Main.dust[illustrious].noGravity = true;
                Main.dust[illustrious].velocity *= 1.2f;
                Main.dust[illustrious].velocity -= Projectile.oldVelocity * 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 180);

            int heal = (int)Math.Round(hit.Damage * 0.15);
            if (heal > ShadowknivesLifeStealCap)
                heal = ShadowknivesLifeStealCap;

            if (Main.player[Main.myPlayer].lifeSteal <= 0f || heal <= 0 || target.lifeMax <= 5)
                return;

            CalamityGlobalProjectile.SpawnLifeStealProjectile(Projectile, Main.player[Projectile.owner], heal, ModContent.ProjectileType<RoyalHeal>(), ShadowknivesLifeStealRange);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 180);

            int heal = (int)Math.Round(info.Damage * 0.015);
            if (heal > ShadowknivesLifeStealCap)
                heal = ShadowknivesLifeStealCap;

            if (Main.player[Main.myPlayer].lifeSteal <= 0f || heal <= 0)
                return;

            CalamityGlobalProjectile.SpawnLifeStealProjectile(Projectile, Main.player[Projectile.owner], heal, ModContent.ProjectileType<RoyalHeal>(), ShadowknivesLifeStealRange);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
