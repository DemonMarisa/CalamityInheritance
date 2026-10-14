using CalamityInheritance.Assets;
using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Particles;
using CalamityInheritance.Core.Utils;
using LAP.Assets.TextureRegister;
using LAP.Core.MiscDate;
using LAP.Core.SpecificEffectManagers;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.ExoWeapons.Supernova
{
    public class SupernovaStealthBoom : CIExoProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        private float radius = 90f;
        public bool damageFrame = false;
        public bool doDamage = true;

        public Color variedColor = Color.White;
        public Color mainColor = Color.LawnGreen;
        public Color randomColor = Color.White;
        public int colorTimer = 0;
        public int time = 0;

        private Player Owner => Main.player[Projectile.owner];

        public override void SetDefaults()
        {
            Projectile.width = 408;
            Projectile.height = 410;
            Projectile.scale = 1f;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = RogueDamage.Instance;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }

        public override void AI()
        {
            randomColor = Main.rand.Next(4) switch
            {
                0 => Color.Red,
                1 => Color.MediumTurquoise,
                2 => Color.Orange,
                _ => Color.LawnGreen,
            };
            if (time == 0)
            {
                mainColor = randomColor;
            }

            if (time % 20 == 0)
            {
                variedColor = colorTimer switch
                {
                    0 => Color.Red,
                    1 => Color.MediumTurquoise,
                    2 => Color.Orange,
                    _ => Color.LawnGreen,
                };
                colorTimer++;
                if (colorTimer >= 4)
                    colorTimer = 0;
            }

            mainColor = Color.Lerp(mainColor, variedColor, 0.07f);

            Lighting.AddLight(Projectile.Center, mainColor.ToVector3() * 4);

            if (time <= 72)
            {
                float orbScale = MathHelper.Clamp(Utils.GetLerpValue(85, 0, time), 0, 1);
                CustomPulse orb = new CustomPulse(Projectile.Center, Vector2.Zero, mainColor, CIExtraTexture.LargeBloom.Path, new Vector2(1, 1), Main.rand.NextFloat(-10, 10), 2.5f * orbScale, 2.5f * orbScale, 4);
                orb.Spawn();
                var orb2 = new CustomPulse(Projectile.Center, Vector2.Zero, Color.White, CIExtraTexture.LargeBloom.Path, new Vector2(1, 1), Main.rand.NextFloat(-10, 10), 2f * orbScale, 2f * orbScale, 4);
                orb2.Spawn();

                radius = 90 * orbScale;


                float numberOfDusts = 2f;
                float rotFactor = 360f / numberOfDusts;
                for (int i = 0; i < numberOfDusts; i++)
                {
                    randomColor = Main.rand.Next(4) switch
                    {
                        0 => Color.Red,
                        1 => Color.MediumTurquoise,
                        2 => Color.Orange,
                        _ => Color.LawnGreen,
                    };

                    float rot = MathHelper.ToRadians(i * rotFactor);
                    Vector2 velOffset = CIUtils.RandomVelocity(10f, 7f, 15f, 0.04f);
                    velOffset *= Main.rand.NextFloat(25, 45) * orbScale;
                    SquishyLightParticle exoEnergy = new(Projectile.Center + velOffset * 2.5f, -velOffset * Main.rand.NextFloat(0.08f, 0.12f), Main.rand.NextFloat(0.3f, 0.5f) * orbScale, randomColor, 9);
                    exoEnergy.Spawn();
                }
            }
            if (time == 40)
            {
                var pulse2 = new StaticPulseRing(Projectile.Center, Vector2.Zero, randomColor * 0.4f, new Vector2(1f, 1f), 0f, 5f, 0f, 40);
                pulse2.Spawn();
            }
            if (time == 50)
            {
                var pulse2 = new StaticPulseRing(Projectile.Center, Vector2.Zero, randomColor * 0.4f, new Vector2(1f, 1f), 0f, 5f, 0f, 30);
                pulse2.Spawn();
            }
            if (time == 60)
            {
                var pulse2 = new StaticPulseRing(Projectile.Center, Vector2.Zero, randomColor * 0.4f, new Vector2(1f, 1f), 0f, 5f, 0f, 20);
                pulse2.Spawn();
            }
            if (time == 70)
            {
                doDamage = false;
                float rotation = Main.rand.NextBool() ? 2.5f : -2.5f;
                GenericSparkle sparkle = new GenericSparkle(Projectile.Center, Vector2.Zero, Color.White, randomColor, 3f, 9, rotation, 2);
                sparkle.Spawn();
                GenericSparkle sparkle2 = new GenericSparkle(Projectile.Center, Vector2.Zero, Color.White, randomColor, 3.5f, 9, rotation, 2);
                sparkle2.Spawn();
            }
            if (time == 80)
            {
                radius = 2500;
                if (Main.zenithWorld)
                {
                    Projectile.hostile = true;
                    Projectile.friendly = true;
                    Projectile.damage *= 100;
                }

                SoundEngine.PlaySound(CISounds.SupernovaStealthExplode with { Pitch = Projectile.ai[2] }, Projectile.Center);
                Projectile.numHits = 0;
                damageFrame = true;
                doDamage = true;

                ScreenShakeSystem.AddScreenShakes(Projectile.Center, 15, 15, 0);

                for (int i = 0; i < 55; i++)
                {
                    Vector2 randVel = new Vector2(35, 35).RotatedByRandom(100) * Main.rand.NextFloat(0.05f, Main.rand.NextBool(3) ? 1f : 0.5f);
                    HeavySmokeParticle smoke = new HeavySmokeParticle(Projectile.Center + randVel, randVel, new Color(57, 46, 115) * 0.9f, Main.rand.Next(25, 35 + 1), Main.rand.NextFloat(0.9f, 2.3f), 0.5f);
                    smoke.Spawn();
                }
                for (int i = 0; i < 150; i++)
                {
                    Vector2 randVel = new Vector2(15, 15).RotatedByRandom(100) * Main.rand.NextFloat(0.1f, 1.6f);
                    Dust dust2 = Dust.NewDustPerfect(Projectile.Center + randVel, DustID.SteampunkSteam, randVel);
                    dust2.scale = Main.rand.NextFloat(1.75f, 2.5f);
                    dust2.noGravity = true;
                    dust2.color = new Color(57, 46, 115);
                    dust2.alpha = Main.rand.Next(40, 100 + 1);
                }

                var orb = new CustomPulse(Projectile.Center, Vector2.Zero, mainColor, CIExtraTexture.LargeBloom.Path, new Vector2(1, 1), Main.rand.NextFloat(-10, 10), 4.5f, 3.5f, 20);
                orb.Spawn();
                var orb2 = new CustomPulse(Projectile.Center, Vector2.Zero, Color.White, CIExtraTexture.LargeBloom.Path, new Vector2(1, 1), Main.rand.NextFloat(-10, 10), 4f, 3f, 20);
                orb.Spawn();

                Vector2 BurstFXDirection = new Vector2(0, 5);
                for (int i = 0; i < 8; i++)
                {
                    randomColor = Main.rand.Next(4) switch
                    {
                        0 => Color.Red,
                        1 => Color.MediumTurquoise,
                        2 => Color.Orange,
                        _ => Color.LawnGreen,
                    };

                    GlowSparkParticle spark = new GlowSparkParticle(Projectile.Center, BurstFXDirection * (i + 1f), false, 12, (0.25f - i * 0.02f) * 1.5f, randomColor, new Vector2(2.7f, 1.3f), true);
                    spark.Spawn();
                }
                for (int k = 0; k < 25; k++)
                {
                    randomColor = Main.rand.Next(4) switch
                    {
                        0 => Color.Red,
                        1 => Color.MediumTurquoise,
                        2 => Color.Orange,
                        _ => Color.LawnGreen,
                    };

                    GlowSparkParticle spark2 = new GlowSparkParticle(Projectile.Center + Main.rand.NextVector2Circular(30, 30), BurstFXDirection * Main.rand.NextFloat(1f, 20.5f), false, Main.rand.Next(40, 50 + 1), Main.rand.NextFloat(0.04f, 0.095f), randomColor, new Vector2(0.3f, 1.6f));
                    spark2.Spawn();
                }
                for (int i = 0; i < 8; i++)
                {
                    randomColor = Main.rand.Next(4) switch
                    {
                        0 => Color.Red,
                        1 => Color.MediumTurquoise,
                        2 => Color.Orange,
                        _ => Color.LawnGreen,
                    };

                    GlowSparkParticle spark = new GlowSparkParticle(Projectile.Center, -BurstFXDirection * (i + 1f), false, 12, (0.25f - i * 0.02f) * 1.5f, randomColor, new Vector2(2.7f, 1.3f), true);
                    spark.Spawn();
                }
                for (int k = 0; k < 25; k++)
                {
                    randomColor = Main.rand.Next(4) switch
                    {
                        0 => Color.Red,
                        1 => Color.MediumTurquoise,
                        2 => Color.Orange,
                        _ => Color.LawnGreen,
                    };

                    GlowSparkParticle spark2 = new GlowSparkParticle(Projectile.Center + Main.rand.NextVector2Circular(30, 30), -BurstFXDirection * Main.rand.NextFloat(1f, 20.5f), false, Main.rand.Next(40, 50 + 1), Main.rand.NextFloat(0.04f, 0.095f), randomColor, new Vector2(0.3f, 1.6f));
                    spark2.Spawn();
                }
                Vector2 BurstFXDirection2 = new Vector2(5, 0);
                for (int i = 0; i < 8; i++)
                {
                    randomColor = Main.rand.Next(4) switch
                    {
                        0 => Color.Red,
                        1 => Color.MediumTurquoise,
                        2 => Color.Orange,
                        _ => Color.LawnGreen,
                    };

                    GlowSparkParticle spark = new GlowSparkParticle(Projectile.Center, BurstFXDirection2 * (i + 1f), false, 12, (0.25f - i * 0.02f) * 1.5f, randomColor, new Vector2(2.7f, 1.3f), true);
                    spark.Spawn();
                }
                for (int k = 0; k < 25; k++)
                {
                    randomColor = Main.rand.Next(4) switch
                    {
                        0 => Color.Red,
                        1 => Color.MediumTurquoise,
                        2 => Color.Orange,
                        _ => Color.LawnGreen,
                    };

                    GlowSparkParticle spark2 = new GlowSparkParticle(Projectile.Center + Main.rand.NextVector2Circular(30, 30), BurstFXDirection2 * Main.rand.NextFloat(1f, 20.5f), false, Main.rand.Next(40, 50 + 1), Main.rand.NextFloat(0.04f, 0.095f), randomColor, new Vector2(0.3f, 1.6f));
                    spark2.Spawn();
                }
                for (int i = 0; i < 8; i++)
                {
                    randomColor = Main.rand.Next(4) switch
                    {
                        0 => Color.Red,
                        1 => Color.MediumTurquoise,
                        2 => Color.Orange,
                        _ => Color.LawnGreen,
                    };

                    GlowSparkParticle spark = new GlowSparkParticle(Projectile.Center, -BurstFXDirection2 * (i + 1f), false, 12, (0.25f - i * 0.02f) * 1.5f, randomColor, new Vector2(2.7f, 1.3f), true);
                    spark.Spawn();
                }
                for (int k = 0; k < 25; k++)
                {
                    randomColor = Main.rand.Next(4) switch
                    {
                        0 => Color.Red,
                        1 => Color.MediumTurquoise,
                        2 => Color.Orange,
                        _ => Color.LawnGreen,
                    };

                    GlowSparkParticle spark2 = new GlowSparkParticle(Projectile.Center + Main.rand.NextVector2Circular(30, 30), -BurstFXDirection2 * Main.rand.NextFloat(1f, 20.5f), false, Main.rand.Next(40, 50 + 1), Main.rand.NextFloat(0.04f, 0.095f), randomColor, new Vector2(0.3f, 1.6f));
                    spark2.Spawn();
                }

                for (int i = 0; i < 10; i++)
                {
                    randomColor = Main.rand.Next(4) switch
                    {
                        0 => Color.Red,
                        1 => Color.MediumTurquoise,
                        2 => Color.Orange,
                        _ => Color.LawnGreen,
                    };

                    var pulse2 = new CustomPulse(Projectile.Center, Vector2.Zero, randomColor * 0.7f, CIExtraTexture.FlameExplosion.Path, new Vector2(1f, 1f), Main.rand.NextFloat(-20, 20), 0f, 4f - i * 0.28f, 50);
                    pulse2.Spawn();
                }
            }
            if (time < 80)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC target = Main.npc[i];
                    bool isAPillar = target.type == NPCID.LunarTowerSolar || target.type == NPCID.LunarTowerVortex || target.type == NPCID.LunarTowerNebula || target.type == NPCID.LunarTowerStardust;
                    if (!isAPillar && !target.boss && target.damage > 5 && target.aiStyle != NPCAIStyleID.TargetDummy && target.type != NPCID.TargetDummy && target.CanBeChasedBy(Projectile, false))
                    {
                        if (target != null && !LAPInfo.AnyBossHere)
                        {
                            if (Vector2.Distance(target.Center, Projectile.Center) > 40 && Vector2.Distance(target.Center, Projectile.Center) < 2000)
                                target.Center += target.Center.DirectionTo(Projectile.Center).SafeNormalize(Vector2.UnitX) * 38;
                        }
                    }
                }
            }

            time++;
            if (time >= 82)
            {
                Projectile.Kill();
            }
        }

        public Color RandomizeColor(Color randomColor)
        {
            randomColor = Main.rand.Next(4) switch
            {
                0 => Color.Red,
                1 => Color.MediumTurquoise,
                2 => Color.Orange,
                _ => Color.LawnGreen,
            };

            return randomColor;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => LAPUtilities.CircularHitboxCollision(Projectile.Center, radius, targetHitbox);

        public override bool PreDraw(ref Color lightColor)
        {

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (damageFrame)
            {
                target.AddBuff(BuffType<CIMiracleBlight>(), 90);
                for (int i = 0; i <= MathHelper.Clamp(9 - Projectile.numHits, 3, 9); i++)
                {
                    randomColor = Main.rand.Next(4) switch
                    {
                        0 => Color.Red,
                        1 => Color.MediumTurquoise,
                        2 => Color.Orange,
                        _ => Color.LawnGreen,
                    };
                    Vector2 vel = target.Center.DirectionFrom(Projectile.Center).SafeNormalize(Vector2.UnitX) * 20 * Main.rand.NextFloat(0.05f, 1.2f);
                    Dust dust2 = Dust.NewDustPerfect(target.Center + Main.rand.NextVector2Circular(target.width * 0.5f, target.height * 0.5f), DustID.RainbowTorch, vel);
                    dust2.scale = Main.rand.NextFloat(1.15f, 2f);
                    dust2.noGravity = true;
                    dust2.color = Color.Lerp(Color.White, randomColor, 0.9f);
                }

                if (!target.boss && target.damage > 5 && target.aiStyle != NPCAIStyleID.TargetDummy && target.type != NPCID.TargetDummy && !LAPInfo.AnyBossHere && target.CanBeChasedBy(Projectile, false) && target != null)
                {
                    Vector2 velToApply = target.Center.DirectionFrom(Projectile.Center).SafeNormalize(Vector2.UnitX) * 40;
                    target.velocity = velToApply + (velToApply.Y <= 0 ? new Vector2(0, -5) : Vector2.Zero);
                }
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) // Add to regular plz
        {
            if (!damageFrame)
                modifiers.SourceDamage *= 0.0025f;
            if (Projectile.damage < 1)
                Projectile.damage = 1;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffType<CIMiracleBlight>(), 300);

        public override bool? CanDamage() => doDamage ? null : false;
    }
}
