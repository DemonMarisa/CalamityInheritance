using CalamityMod.Projectiles.Summon.Umbrella;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.Summon.Umbrella
{
    public class MagicBirdOld : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Mods.CalamityInheritance.Content.Content.Projectiles";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bird");
            Main.projFrames[Projectile.type] = 7;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.minion = true;
            Projectile.minionSlots = 0f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.alpha = 255;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= 7)
            {
                Projectile.frame = 0;
            }
            Projectile.ai[0]++;
            Projectile.ai[1]++;
            if (Projectile.ai[0] >= 20f)
            {
                Vector2 dspeed = Projectile.velocity * Main.rand.NextFloat(0.3f, 0.6f);
                int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.IceRod, dspeed.X, dspeed.Y, 50, default, 1.2f);
                Main.dust[dust].noGravity = true;
                Projectile.ai[0] = 0f;
            }

            //Homing
            if (Projectile.ai[1] > 30f)
                HomingAI();
            //Fade in
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 15;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
        }

        private void HomingAI()
        {
            Player player = Main.player[Projectile.owner];
            int targetIdx = -1;
            float maxHomingRange = MagicHat.Range;
            bool hasHomingTarget = false;
            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                if (npc.CanBeChasedBy(Projectile, false))
                {
                    float dist = (Projectile.Center - npc.Center).Length();
                    if (dist < maxHomingRange)
                    {
                        targetIdx = player.MinionAttackTargetNPC;
                        maxHomingRange = dist;
                        hasHomingTarget = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Main.npc.Length; ++i)
                {
                    NPC npc = Main.npc[i];
                    if (npc == null || !npc.active)
                        continue;

                    if (npc.CanBeChasedBy(Projectile, false))
                    {
                        float dist = (Projectile.Center - npc.Center).Length();
                        if (dist < maxHomingRange)
                        {
                            targetIdx = i;
                            maxHomingRange = dist;
                            hasHomingTarget = true;
                        }
                    }
                }
            }

            // Home in on said closest NPC.
            if (hasHomingTarget)
            {
                NPC target = Main.npc[targetIdx];
                Vector2 homingVector = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 32f;
                float homingRatio = 15f;
                Projectile.velocity = (Projectile.velocity * homingRatio + homingVector) / (homingRatio + 1f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 dspeed = new Vector2(Main.rand.NextFloat(-7f, 7f), Main.rand.NextFloat(-7f, 7f));
                int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.IceRod, dspeed.X, dspeed.Y, 50, default, 1.2f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 239, 0, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
