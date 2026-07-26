using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Spark;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Swords
{
    public class TrueBloodyBladeProj : CIMeleeProj
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.aiStyle = ProjAIStyleID.Sickle;
            AIType = ProjectileID.DeathSickle;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.3f / 255f, (255 - Projectile.alpha) * 0.3f / 255f, (255 - Projectile.alpha) * 0f / 255f);
            if (Projectile.timeLeft < 230)
                Projectile.HomeInNPC(1000f, 35f, 35f, null, !Projectile.tileCollide);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * (b2 / 255f));
                return new Color(b2, b2, b2, a2);
            }
            return new Color(255, 255, 255, 100);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 生成 Dust
            int dustIndex = Dust.NewDust(Main.rand.NextVector2FromRectangle(target.Hitbox), 0, 0, DustID.Terra);
            Main.dust[dustIndex].noGravity = true;

            // 调用自定义粒子 TrueBloodyProjSpark
            Vector2 position = Main.rand.NextVector2FromRectangle(target.Hitbox);
            float intensity = 20f;
            float direction = Main.rand.NextFloatDirection();
            TrueBloodyProjSpark.GeneratePrettySparkles(position, intensity, Projectile.rotation);

            // Set the target's hit direction to away from the player so the knockback is in the correct direction.
            hit.HitDirection = Main.player[Projectile.owner].Center.X < target.Center.X ? 1 : -1;
            target.AddBuff(BuffType<CIBurningBlood>(), 60);
            target.AddBuff(BuffID.Ichor, 60);

            Player player = Main.player[Projectile.owner];
            if (target.type != NPCID.TargetDummy && target.canGhostHeal && !player.moonLeech)
            {
                int healAmount = Main.rand.Next(2) + 1;
                player.statLife += healAmount;
                player.HealEffect(healAmount);
            }
        }
    }
}
