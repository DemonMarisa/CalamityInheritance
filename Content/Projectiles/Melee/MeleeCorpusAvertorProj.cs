using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityMod;
using CalamityMod.Projectiles;
using LAP.Content.Projectiles.LifeStealProj;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class MeleeCorpusAvertorProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => GetInstance<MeleeCorpusAvertor>().Texture;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.02f;

            if (Projectile.ai[0] < 120f)
                Projectile.ai[0] += 1f;

            if (Projectile.ai[0] < 61f)
            {
                if (Projectile.ai[0] % 20f == 0f)
                {
                    Vector2 velocity = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
                    float mult = Projectile.ai[0] / 80f; // Ranges from 0.25 to 0.5 to 0.75
                    velocity *= mult;

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ProjectileType<MeleeCorpusAvertorProjClone>(),
                        (int)(Projectile.damage * mult), Projectile.knockBack * mult, Projectile.owner, Projectile.ai[0]);
                }
            }
            else
            {
                Projectile.velocity.X *= 1.01f;
                Projectile.velocity.Y *= 1.01f;

                int scale = (int)((Projectile.ai[0] - 60f) * 4.25f);
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0f, 0f, 100, new Color(scale, 0, 0, 50), 2f);
                Main.dust[dust].velocity *= 0f;
                Main.dust[dust].noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.ai[0] >= 61f)
            {
                int scale = (int)((Projectile.ai[0] - 60f) * 4.25f);
                return new Color(scale, 0, 0, 50);
            }
            return new Color(0, 0, 0, 50);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.Owner().SpawnLifeStealProj(target, Projectile, ProjectileType<StandardHealProj>(), Main.rand.Next(1, 3));
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
        }
    }
}
