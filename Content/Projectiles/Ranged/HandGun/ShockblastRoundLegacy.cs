using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Items.Weapons.Ranged.HandGun;
using CalamityInheritance.Content.Projectiles.Heals;
using CalamityInheritance.Content.Projectiles.Ranged.Explosions;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged.HandGun
{
    public class ShockblastRoundLegacy : CIRangedProj
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.5f;
            Projectile.extraUpdates = 3;
            AIType = ProjectileID.Bullet;

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<ShockblastLegacy>(), Projectile.damage / 2, 0f, Projectile.owner, 0f, Projectile.ai[1]);
                Main.projectile[proj].scale = Projectile.ai[1] * 0.5f + 1f;
            }
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override bool PreAI()
        {
            //Rotation
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi) + MathHelper.ToRadians(90) * Projectile.direction;

            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 dspeed = -Projectile.velocity * Main.rand.NextFloat(0.5f, 0.7f);
                    float x2 = Projectile.Center.X - Projectile.velocity.X / 10f * i;
                    float y2 = Projectile.Center.Y - Projectile.velocity.Y / 10f * i;
                    int dust = Dust.NewDust(new Vector2(x2, y2), 1, 1, DustID.FrostHydra, 0f, 0f, 0, default, 1f);
                    Main.dust[dust].alpha = Projectile.alpha;
                    Main.dust[dust].position.X = x2;
                    Main.dust[dust].position.Y = y2;
                    Main.dust[dust].velocity = dspeed;
                    Main.dust[dust].noGravity = true;
                }
            }

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<ShockblastLegacy>(), Projectile.damage / 2, 0f, Projectile.owner, 0f, Projectile.ai[1]);
                Main.projectile[proj].scale = Projectile.ai[1] * 0.5f + 1f;
            }

            Player player = Main.player[Projectile.owner];
            if (player.moonLeech)
                return;
            if (player.CI().GlobalHealProjCD > 0)
                return;
            int healdamage = (int)player.GetDamage<RangedDamageClass>().ApplyTo(PearlGodLegacy.damage);
            if (hit.Crit)
                healdamage *= 2;
            float healAmt = healdamage * 0.05f;
            int CD = Main.rand.Next(40, 60);
            float rot = (player.Center - Projectile.Center).ToRotation();
            for (int i = 0; i < 3; i++)
            {
                int angleoffset = 145;
                Vector2 direction = new Vector2(12f, 0).RotatedBy(rot);

                if (i == 1)
                    direction = new Vector2(12f, 0).RotatedBy(rot).RotatedBy(MathHelper.ToRadians(angleoffset));

                if (i == 2)
                    direction = new Vector2(12f, 0).RotatedBy(rot).RotatedBy(MathHelper.ToRadians(-angleoffset));

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction, ProjectileType<BlueHealProj>(), 0, 0f, player.whoAmI, 0, 0, healAmt);
            }

            player.CI().GlobalHealProjCD = CD;
        }
    }
}
