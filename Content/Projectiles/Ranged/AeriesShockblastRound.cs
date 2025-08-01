using CalamityInheritance.Content.Projectiles.Typeless.Heal;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Projectiles;
using CalamityMod.Projectiles.Healing;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class AeriesShockblastRound : ModProjectile
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
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.light = 0.5f;
            Projectile.extraUpdates = 2;
            AIType = ProjectileID.Bullet;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<AeriesShockblast>(), Projectile.damage, 0f, Projectile.owner, 0f, Projectile.ai[1]);
                Main.projectile[proj].scale = (Projectile.ai[1] * 0.5f) + 1f;
            }
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
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
                for (int num136 = 0; num136 < 2; num136++)
                {
                    Vector2 dspeed = -Projectile.velocity * Main.rand.NextFloat(0.5f, 0.7f);
                    float x2 = Projectile.Center.X - Projectile.velocity.X / 10f * num136;
                    float y2 = Projectile.Center.Y - Projectile.velocity.Y / 10f * num136;
                    int num137 = Dust.NewDust(new Vector2(x2, y2), 1, 1, 185, 0f, 0f, 0, default, 1f);
                    Main.dust[num137].alpha = Projectile.alpha;
                    Main.dust[num137].position.X = x2;
                    Main.dust[num137].position.Y = y2;
                    Main.dust[num137].velocity = dspeed;
                    Main.dust[num137].noGravity = true;
                }
            }
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit , int damage)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<AeriesShockblast>(), Projectile.damage, 0f, Projectile.owner, 0f, Projectile.ai[1]);
                Main.projectile[proj].scale = (Projectile.ai[1] * 0.5f) + 1f;
            }
            Player player = Main.player[Projectile.owner];
            if (player.moonLeech)
                return;
            if (player.CIMod().GlobalHealProjCD > 0)
                return;
            float healAmt = damage * 0.05f;
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

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction, ModContent.ProjectileType<BlueHealProj>(), 0, 0f, player.whoAmI, 0, 0, healAmt);
            }

            player.CIMod().GlobalHealProjCD = CD;
        }
    }
}
