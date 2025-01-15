using CalamityMod.CalPlayer;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.Buffs.Summon;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Projectiles.Summon
{
    public class MidnightSunUFOold : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Mods.CalamityInheritance.Content.Content.Projectiles";
        public const float DistanceToCheck = 2600f;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 58;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 9;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, Color.SkyBlue.ToVector3());
            Player player = Main.player[Projectile.owner];
            CalamityPlayer modPlayer = player.Calamity();
            CalamityInheritancePlayer modPlayer1 = player.CalamityInheritance();
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.velocity.Y = Main.rand.NextFloat(8f, 11f) * Main.rand.NextBool(2).ToDirectionInt();
                Projectile.velocity.Y = Main.rand.NextFloat(3f, 5f) * Main.rand.NextBool(2).ToDirectionInt();
                Projectile.localAI[0] = 1f;
            }
            bool isProperProjectile = Projectile.type == ModContent.ProjectileType<MidnightSunUFOold>();
            player.AddBuff(ModContent.BuffType<MidnightSunBuffOld>(), 3600);
            if (isProperProjectile)
            {
                if (player.dead)
                {
                    modPlayer1.MidnnightSunBuff = false;
                }
                if (modPlayer1.MidnnightSunBuff)
                {
                    Projectile.timeLeft = 2;
                }
            }

            NPC potentialTarget = Projectile.Center.MinionHoming(DistanceToCheck, player);

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }

            if (potentialTarget != null)
            {
                if (Projectile.ai[0]++ % 360 < 180)
                {
                    Projectile.rotation = Projectile.rotation.AngleTowards(0f, 0.2f);
                    if (Projectile.ai[1] != 0f)
                    {
                        Projectile.ai[1] = 0f;
                    }
                    float angle = MathHelper.ToRadians(2f * Projectile.ai[0] % 180f);
                    Vector2 destination = potentialTarget.Center - new Vector2((float)Math.Cos(angle) * potentialTarget.width * 0.65f, 250f);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(destination) * 24f, 0.03f);
                    if (Projectile.ai[0] % 3f == 2f && potentialTarget.Top.Y > Projectile.Bottom.Y)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.Bottom, Projectile.DirectionTo(potentialTarget.Center).RotatedByRandom(0.15f) * 25f,
                            ModContent.ProjectileType<MidnightSunLaserold>(),
                            Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                    float acceleration = 0.35f;
                    for (int index = 0; index < Main.projectile.Length; index++)
                    {
                        Projectile proj = Main.projectile[index];
                        if (index != Projectile.whoAmI && proj.active && proj.owner == Projectile.owner && Math.Abs(Projectile.position.X - proj.position.X) + Math.Abs(Projectile.position.Y - proj.position.Y) < Projectile.width)
                        {
                            if (Projectile.position.X < proj.position.X)
                            {
                                Projectile.velocity.X -= acceleration;
                            }
                            else
                            {
                                Projectile.velocity.X += acceleration;
                            }
                            if (Projectile.position.Y < proj.position.Y)
                            {
                                Projectile.velocity.Y -= acceleration;
                            }
                            else
                            {
                                Projectile.velocity.Y += acceleration;
                            }
                        }
                    }
                }
                else
                {
                    const float framesUsedSpinning = MidnightSunBeamold.TrueTimeLeft;
                    float totalRadiansToSpin = MathHelper.ToRadians(120f);
                    float totalRadiansNegativeRange = totalRadiansToSpin - (totalRadiansToSpin / 2);
                    float radiansToSpinPerFrame = totalRadiansNegativeRange / framesUsedSpinning * 2f;
                    if (Projectile.ai[0] % 180 < 180 - framesUsedSpinning)
                    {
                        Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.AngleTo(potentialTarget.Center) - MathHelper.PiOver2 - totalRadiansNegativeRange, 0.15f);

                        Vector2 spawnPosition = Projectile.Center + Utils.NextVector2Unit(Main.rand).RotatedBy(Projectile.rotation) * new Vector2(13f, 6f) / 2f;
                        int idx = Dust.NewDust(spawnPosition - Vector2.One * 8f, 16, 16, DustID.Vortex, Projectile.velocity.X / 2f, Projectile.velocity.Y / 2f, 0, default, 1f);
                        Main.dust[idx].velocity = Vector2.Normalize(Projectile.Center - spawnPosition) * 2.6f;
                        Main.dust[idx].noGravity = true;
                        Main.dust[idx].scale = 0.9f;
                    }
                    else
                    {
                        Projectile.rotation += radiansToSpinPerFrame;
                        if (Projectile.ai[1] == 0f)
                        {
                            SoundEngine.PlaySound(SoundID.Item122, Projectile.Center);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, (Projectile.velocity.ToRotation() + MathHelper.PiOver2).ToRotationVector2(),
                                ModContent.ProjectileType<MidnightSunBeamold>(), Projectile.damage * 2, Projectile.knockBack, Projectile.owner,
                                radiansToSpinPerFrame, Projectile.whoAmI);
                            Projectile.ai[1] = 1f;
                        }
                    }
                    Projectile.velocity *= 0.935f;
                }
            }
            else
            {
                Projectile.velocity = (Projectile.velocity * 15f + Projectile.DirectionTo(player.Center - new Vector2(player.direction * -80f, 160f)) * 19f) / 16f;

                float acceleration = 0.35f;
                for (int index = 0; index < Main.projectile.Length; index++)
                {
                    Projectile proj = Main.projectile[index];
                    if (index != Projectile.whoAmI && proj.active && proj.owner == Projectile.owner && Math.Abs(Projectile.position.X - proj.position.X) + Math.Abs(Projectile.position.Y - proj.position.Y) < Projectile.width)
                    {
                        if (Projectile.position.X < proj.position.X)
                        {
                            Projectile.velocity.X -= acceleration;
                        }
                        else
                        {
                            Projectile.velocity.X += acceleration;
                        }
                        if (Projectile.position.Y < proj.position.Y)
                        {
                            Projectile.velocity.Y -= acceleration;
                        }
                        else
                        {
                            Projectile.velocity.Y += acceleration;
                        }
                    }
                }
                Projectile.rotation = Projectile.velocity.X * 0.03f;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
