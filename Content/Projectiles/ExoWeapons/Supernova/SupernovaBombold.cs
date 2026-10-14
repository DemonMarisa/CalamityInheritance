using CalamityInheritance.Assets;
using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.ExoWeapons;
using CalamityInheritance.Core.Misc;
using CalamityInheritance.Core.Utils;
using LAP.Content.Particles;
using LAP.Core.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.ExoWeapons.Supernova
{
    public class SupernovaBombold : CIExoProj
    {
        public override string Texture => GetInstance<Supernovaold>().Texture;
        public bool MouseInit = false;
        public bool hasPressRight = false;
        public bool exoLore;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = true;
            Projectile.DamageType = RogueDamage.Instance;
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_ItemUse iu && iu.Item is Item item && item.ModItem is not null && item.ModItem is Supernovaold supernovaold && supernovaold.UseAlt)
                exoLore = true;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(MouseInit);
            writer.Write(hasPressRight);
            writer.Write(exoLore);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            MouseInit = reader.ReadBoolean();
            hasPressRight = reader.ReadBoolean();
            exoLore = reader.ReadBoolean();
        }

        public override void AI()
        {
            //dust and lighting
            int dustType = Main.rand.NextBool() ? 107 : 234;
            if (Main.rand.NextBool(4))
            {
                dustType = CIDustID.DustSandnado;
            }
            if (Main.rand.NextBool(6))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
            Lighting.AddLight(Projectile.Center, Main.DiscoR * 0.5f / 255f, Main.DiscoG * 0.5f / 255f, Main.DiscoB * 0.5f / 255f);

            //速度
            float gravSpeed = 0.2f;
            if (hasPressRight)
            {
                gravSpeed = 0f;
                Projectile.netUpdate = true;
            }
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 10f)
            {
                Projectile.ai[0] = 10f;
                if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
                {
                    Projectile.velocity.X *= 0.97f;
                    if (Math.Abs(Projectile.velocity.X) < 0.01f)
                    {
                        Projectile.velocity.X = 0f;
                        Projectile.netUpdate = true;
                    }
                }

                if (!hasPressRight)
                    Projectile.velocity.Y += gravSpeed;
            }

            //转角
            Projectile.rotation += Projectile.velocity.X * 0.1f;

            //stealth strike
            if (Projectile.CI().Stealth && Projectile.timeLeft % 8 == 0 && Projectile.owner == Main.myPlayer)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.UnitY * 2f, ProjectileType<SupernovaHoming>(), (int)(Projectile.damage * 0.48), Projectile.knockBack, Projectile.owner, 0f, 0f);
            Player player = Main.player[Projectile.owner];
            if (player.LAP().MouseRight && exoLore)
            {
                if (!hasPressRight)
                {
                    SoundEngine.PlaySound(CISounds.SupernovaRightClick with { Volume = 0.25f}, player.Center);
                    hasPressRight = true;
                }
            }
            if (hasPressRight && Projectile.owner == Main.myPlayer)
            {
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                Projectile.timeLeft = 500;
                Projectile.extraUpdates = 8;
                Vector2 tar = player.LocalMouseWorld();
                Rectangle mouseHitBox = new((int)tar.X, (int)tar.Y, Projectile.width, Projectile.height);
                if (Projectile.Hitbox.Intersects(mouseHitBox))
                {
                    MouseInit = true;
                    Projectile.Kill();
                }
                else if (!MouseInit)
                {
                    Projectile.scale *= 0.98f;
                    Projectile.HomingTarget(tar, -1, 16f, 0);
                    SparkParticle line = new SparkParticle(Projectile.Center - Projectile.velocity * 1.1f, Projectile.velocity * 0.01f, false, 18, 1f, Color.GhostWhite);
                    line.Spawn();
                }
            }

        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Player player = Main.player[Projectile.owner];
            if (exoLore)
            {
                if (Projectile.velocity.X != oldVelocity.X)
                    Projectile.velocity.X = -oldVelocity.X;
                if (Projectile.velocity.Y != oldVelocity.Y)
                    Projectile.velocity.Y = -oldVelocity.Y;
                return false;
            }
            else return true;
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];

            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 128;
            Projectile.position.X = Projectile.position.X - Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
            SoundEngine.PlaySound(Supernovaold.ExplosionSound, Projectile.Center);

            //spawn projectiles
            int projAmt = Main.rand.Next(3, 5);
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<SupernovaBoomOldRogue>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                for (int i = 0; i < projAmt; i++)
                {
                    Vector2 velocity = CIUtils.RandomVelocity(10f, 7f, 10f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ProjectileType<SupernovaSpike>(), (int)(Projectile.damage * 0.6), 0f, Projectile.owner, 0f, 0f);
                }
                float spread = MathHelper.Pi / 3f;
                double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 6f;
                double offsetAngle;
                for (int i = 0; i < 3; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 2f), (float)(Math.Cos(offsetAngle) * 2f), ProjectileType<SupernovaHoming>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner, 0f, 0f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 2f), (float)(-Math.Cos(offsetAngle) * 2f), ProjectileType<SupernovaHoming>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner, 0f, 0f);
                }
            }
            if (Projectile.CI().Stealth && Projectile.timeLeft % 8 == 0 && Projectile.owner == Main.myPlayer)
            {
                if (exoLore)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<SupernovaStealthBoom>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                }
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<SupernovaBoomOld>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                for (int i = 0; i < projAmt; i++)
                {
                    Vector2 velocity = CIUtils.RandomVelocity(10f, 7f, 10f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ProjectileType<SupernovaSpike>(), (int)(Projectile.damage * 0.6), 0f, Projectile.owner, 0f, 0f);
                }
                float spread = MathHelper.Pi / 3f;
                double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 6f;
                double offsetAngle;
                for (int i = 0; i < 3; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 2f), (float)(Math.Cos(offsetAngle) * 2f), ProjectileType<SupernovaHoming>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner, 0f, 0f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 2f), (float)(-Math.Cos(offsetAngle) * 2f), ProjectileType<SupernovaHoming>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner, 0f, 0f);
                }
            }

            //dust effects
            int dustType = Main.rand.NextBool() ? 107 : 234;
            if (Main.rand.NextBool(4))
            {
                dustType = CIDustID.DustSandnado;
            }
            for (int d = 0; d < 5; d++)
            {
                int exo = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 2f);
                Main.dust[exo].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[exo].scale = 0.5f;
                    Main.dust[exo].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int d = 0; d < 9; d++)
            {
                int fire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3f);
                Main.dust[fire].noGravity = true;
                Main.dust[fire].velocity *= 5f;
                fire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                Main.dust[fire].velocity *= 2f;
            }

            //gore cloud effects
            if (Main.netMode != NetmodeID.Server)
            {
                Vector2 goreSource = Projectile.Center;
                int goreAmt = 3;
                Vector2 source = new Vector2(goreSource.X - 24f, goreSource.Y - 24f);
                for (int goreIndex = 0; goreIndex < goreAmt; goreIndex++)
                {
                    float velocityMult = 0.33f;
                    if (goreIndex < goreAmt / 3)
                    {
                        velocityMult = 0.66f;
                    }
                    if (goreIndex >= 2 * goreAmt / 3)
                    {
                        velocityMult = 1f;
                    }
                    int type = Main.rand.Next(61, 64);
                    int smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    Gore gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X += 1f;
                    gore.velocity.Y += 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X -= 1f;
                    gore.velocity.Y += 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X += 1f;
                    gore.velocity.Y -= 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X -= 1f;
                    gore.velocity.Y -= 1f;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIMiracleBlight>(), 300);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffType<CIMiracleBlight>(), 300);

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
