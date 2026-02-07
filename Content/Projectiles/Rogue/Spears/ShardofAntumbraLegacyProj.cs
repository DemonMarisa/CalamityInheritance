using CalamityInheritance.Content.Items.Weapons.Rogue.Spears;
using CalamityInheritance.Content.Projectiles;
using CalamityInheritance.Content.Projectiles.Typeless.NorProj;
using CalamityMod;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Rogue.Spears
{
    public class ShardofAntumbraLegacyProj : RogueDamageProj
    {
        public override string Texture => GetInstance<ShardofAntumbraLegacy>().Texture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void ExSD()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 2;
            Projectile.timeLeft = 600;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10 * Projectile.MaxUpdates; // 10 effective, 20 total
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation -= MathHelper.PiOver2;
            }
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.PurpleTorch, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
            if (Projectile.timeLeft % 12 == 0)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (IsStealth)
                    {
                        int star = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, ProjectileType<EmpyreanStellarDetritusLegacy>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner, 0f, 0f);
                        Main.projectile[star].DamageType = RogueDamageClass.Instance;
                    }
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}