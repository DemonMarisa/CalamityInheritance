using CalamityMod.Dusts;
using LAP.Core.SystemsLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Summon.Sentrys
{
    internal class ProfanedEnergyLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Summon";
        private float count = 0f;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            Projectile.AddProtectedProj(false);
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 60;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.sentry = true;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 5)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
            {
                Projectile.frame = 0;
            }
            if (count == 0f)
            {
                SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);
                for (int i = 0; i < 5; i++)
                {
                    int holy = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, (int)CalamityDusts.ProfanedFire, 0f, 0f, 100, default, 2f);
                    Main.dust[holy].velocity *= 3f;
                    Main.dust[holy].position = Projectile.Center;
                    if (Main.rand.NextBool())
                    {
                        Main.dust[holy].scale = 0.5f;
                        Main.dust[holy].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int j = 0; j < 10; j++)
                {
                    int fire = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.GoldCoin, 0f, 0f, 100, default, 3f);
                    Main.dust[fire].noGravity = true;
                    Main.dust[fire].velocity *= 5f;
                    Main.dust[fire].position = Projectile.Center;
                    fire = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.GoldCoin, 0f, 0f, 100, default, 2f);
                    Main.dust[fire].velocity *= 2f;
                    Main.dust[fire].position = Projectile.Center;
                }
                count += 1f;
            }
            if (Projectile.owner == Main.myPlayer)
            {
                if (Projectile.ai[0] != 0f)
                {
                    Projectile.ai[0] -= 1f;
                    return;
                }
                bool canAttack = false;
                float projX = Projectile.Center.X;
                float projY = Projectile.Center.Y;
                float attackDistance = 1000f;
                int target = 0;
                if (player.HasMinionAttackTargetNPC)
                {
                    NPC npc = Main.npc[player.MinionAttackTargetNPC];
                    if (npc.CanBeChasedBy(Projectile, false))
                    {
                        float targetX = npc.Center.X;
                        float targetY = npc.Center.Y;
                        float targetDist = Math.Abs(Projectile.Center.X - targetX) + Math.Abs(Projectile.Center.Y - targetY);
                        if (targetDist < attackDistance && Collision.CanHit(Projectile.Center, Projectile.width, Projectile.height, npc.Center, npc.width, npc.height))
                        {
                            projX = targetX;
                            projY = targetY;
                            canAttack = true;
                            target = npc.whoAmI;
                        }
                    }
                }
                if (!canAttack)
                {
                    foreach (NPC n in Main.ActiveNPCs)
                    {
                        if (n.CanBeChasedBy(Projectile, false))
                        {
                            float npcX = n.position.X + (float)(n.width / 2);
                            float npcY = n.position.Y + (float)(n.height / 2);
                            float npcDist = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - npcX) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - npcY);
                            if (npcDist < attackDistance && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, n.position, n.width, n.height))
                            {
                                attackDistance = npcDist;
                                projX = npcX;
                                projY = npcY;
                                canAttack = true;
                                target = n.whoAmI;
                            }
                        }
                    }
                }
                if (canAttack)
                {
                    float projXStore = projX;
                    float projYStore = projY;
                    projX -= Projectile.Center.X;
                    projY -= Projectile.Center.Y;
                    if (projX < 0f)
                    {
                        Projectile.spriteDirection = 1;
                    }
                    else
                    {
                        Projectile.spriteDirection = -1;
                    }
                    float speed = 25f;
                    Vector2 fireDirection = Projectile.Center;
                    float fireXVel = projXStore - fireDirection.X;
                    float fireYVel = projYStore - fireDirection.Y;
                    float fireVelocity = (float)Math.Sqrt((double)(fireXVel * fireXVel + fireYVel * fireYVel));
                    fireVelocity = speed / fireVelocity;
                    fireXVel *= fireVelocity;
                    fireYVel *= fireVelocity;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, fireXVel, fireYVel, ProjectileType<FlameBurstLegacy>(), Projectile.damage, Projectile.knockBack, Projectile.owner, (float)target, 0f);

                    Projectile.ai[0] = 16f;
                }
            }
        }

        public override bool? CanDamage() => false;
    }
}
