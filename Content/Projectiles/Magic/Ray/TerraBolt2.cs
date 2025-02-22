using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Magic;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray
{
    public class TerraBolt2 : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";

        public const int Lifetime = 150;
        public ref float Time => ref Projectile.ai[0];
        public ref float ShardCooldown => ref Projectile.ai[1];
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.extraUpdates = 100;
            Projectile.friendly = true;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            for (int i = 0; i < 2; i++)
            {
                int dustType = Main.rand.NextBool(4) ? 107 : 180;
                Vector2 dustSpawnPos = Projectile.position - Projectile.velocity * i / 2f;
                Dust crimtameMagic = Dust.NewDustPerfect(dustSpawnPos, dustType);
                crimtameMagic.scale = Main.rand.NextFloat(0.96f, 1.04f) * MathHelper.Lerp(1f, 1.7f, Time / Lifetime);
                crimtameMagic.noGravity = true;
                crimtameMagic.velocity *= 0.1f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 8;

            if (ShardCooldown > 0f)
                return;

            // The "Center" of the laser is actually the start of it in this context.
            // Collision is done separately. This might have a slight offset due to collision
            // boxes, but that should be negligible.
            float lengthFromStart = Projectile.Distance(target.Center);

            int totalShards = (int)MathHelper.Lerp(1, 3, MathHelper.Clamp(lengthFromStart / 1200f * 1.5f, 0f, 1f));
            int shardType = ModContent.ProjectileType<PhotosyntheticShard>();
            int shardDamage = (int)(Projectile.damage * 0.5);
            for (int i = 0; i < totalShards; i++)
            {
                int tries = 0;
                Vector2 spawnOffset;
                do
                {
                    spawnOffset = Main.rand.NextVector2CircularEdge(target.width * 0.5f + 40f, target.height * 0.5f + 40f);
                    tries++;
                }
                while (Collision.SolidCollision((target.Center + spawnOffset).ToTileCoordinates().ToVector2(), 4, 4) && tries < 10);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center + spawnOffset, Main.rand.NextVector2CircularEdge(6f, 6f), shardType, shardDamage, Projectile.knockBack, Projectile.owner);
            }

            ShardCooldown = 3f;
            Projectile.netUpdate = true;
        }
    }
}
