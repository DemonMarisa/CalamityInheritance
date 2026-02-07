using CalamityInheritance.Content.Items.Weapons.Melee.Yoyos;
using LAP.Content.Projectiles.LifeStealProj;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Yoyos
{
    public class LaceratorYoyoLegacy : ModProjectile
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<LaceratorLegacy>();
        public const int MaxUpdates = 3;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = -1f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 640f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 48f / MaxUpdates;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = ProjAIStyleID.Yoyo;
            Projectile.width = Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = MaxUpdates;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 3 * MaxUpdates;
        }

        public override void AI()
        {
            if ((Projectile.position - Main.player[Projectile.owner].position).Length() > 3200f) //200 blocks
                Projectile.Kill();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.player[Projectile.owner].moonLeech)
                return;

            Player player = Main.player[Projectile.owner];
            player.lifeRegenTime += 2;

            Projectile.Owner().SpawnLifeStealProj(target, Projectile.GetSource_FromThis(), ProjectileType<StandardHealProj>(), Projectile.Center, Vector2.Zero, Main.rand.Next(1, 3));

        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
