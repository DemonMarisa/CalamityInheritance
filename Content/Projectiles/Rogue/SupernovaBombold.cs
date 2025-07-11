﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Projectiles.Rogue;
using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Weapons;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Particles;
using Microsoft.Xna.Framework.Input;
using System.IO;
using CalamityInheritance.Sounds.Custom;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class SupernovaBombold : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => $"{Generic.WeaponPath}/Rogue/Supernovaold";
        public bool MouseInit = false;
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
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }
        public bool hasPressRight = false;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(MouseInit);
            writer.Write(hasPressRight);
            Projectile.DoSyncHandlerWrite(ref writer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            MouseInit = reader.ReadBoolean();
            hasPressRight = reader.ReadBoolean();
            Projectile.DoSyncHandlerRead(ref reader);
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

                if(!hasPressRight)
                    Projectile.velocity.Y += gravSpeed;
            }

            //转角
            Projectile.rotation += Projectile.velocity.X * 0.1f;

            //stealth strike
            if (Projectile.Calamity().stealthStrike && Projectile.timeLeft % 8 == 0 && Projectile.owner == Main.myPlayer)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.UnitY * 2f, ModContent.ProjectileType<SupernovaHoming>(), (int)(Projectile.damage * 0.48), Projectile.knockBack, Projectile.owner, 0f, 0f);
            Player player = Main.player[Projectile.owner];
            if (Main.mouseRight && (player.CIMod().LoreExo || player.CIMod().PanelsLoreExo))
            {
                if (!hasPressRight)
                {

                    SoundEngine.PlaySound(CISoundMenu.SupernovaRightClick, player.Center);
                    hasPressRight = true;
                }
            }
            if (hasPressRight && Projectile.owner == Main.myPlayer)
            {
                Projectile.ignoreWater = true;
                Projectile.tileCollide = false;
                Projectile.timeLeft = 500;
                Projectile.extraUpdates = 8;
                //除非你没有鼠标，不然这里肯定会在下方赋予成鼠标位置
                Vector2 tar = Main.MouseWorld;
                Rectangle mouseHitBox = new ((int)tar.X, (int)tar.Y, Projectile.width, Projectile.height);
                if (Projectile.Hitbox.Intersects(mouseHitBox))
                {
                    MouseInit = true;
                    Projectile.Kill();
                }
                else if (!MouseInit)
                {
                    Projectile.scale *= 0.98f;
                    CIFunction.HomeInOnMouseBetter(Projectile, 16f, 0f, 2, true);
                    SparkParticle line = new SparkParticle(Projectile.Center - Projectile.velocity * 1.1f, Projectile.velocity * 0.01f, false, 18, 1f, Color.GhostWhite);
                    GeneralParticleHandler.SpawnParticle(line);
                }
            }

        }
        
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Player player = Main.player[Projectile.owner];
            var usPlayer = player.CIMod();
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
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
            CalamityInheritancePlayer usPlayer = player.CIMod();

            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 128;
            Projectile.position.X = Projectile.position.X - Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
            SoundEngine.PlaySound(Supernova.ExplosionSound, Projectile.Center);

            //spawn projectiles
            int projAmt = Main.rand.Next(3, 5);
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SupernovaBoom>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                for (int i = 0; i < projAmt; i++)
                {
                    Vector2 velocity = CalamityUtils.RandomVelocity(100f, 70f, 100f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<SupernovaSpike>(), (int)(Projectile.damage * 0.6), 0f, Projectile.owner, 0f, 0f);
                }
                float spread = MathHelper.Pi / 3f;
                double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 6f;
                double offsetAngle;
                for (int i = 0; i < 3; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 2f), (float)(Math.Cos(offsetAngle) * 2f), ModContent.ProjectileType<SupernovaHoming>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner, 0f, 0f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 2f), (float)(-Math.Cos(offsetAngle) * 2f), ModContent.ProjectileType<SupernovaHoming>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner, 0f, 0f);
                }
            }
            if (Projectile.Calamity().stealthStrike && Projectile.timeLeft % 8 == 0 && Projectile.owner == Main.myPlayer)
            {
                if(usPlayer.LoreExo || usPlayer.PanelsLoreExo)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SupernovaStealthBoom>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                }
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SupernovaBoom>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                for (int i = 0; i < projAmt; i++)
                {
                    Vector2 velocity = CalamityUtils.RandomVelocity(100f, 70f, 100f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<SupernovaSpike>(), (int)(Projectile.damage * 0.6), 0f, Projectile.owner, 0f, 0f);
                }
                float spread = MathHelper.Pi / 3f;
                double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 6f;
                double offsetAngle;
                for (int i = 0; i < 3; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 2f), (float)(Math.Cos(offsetAngle) * 2f), ModContent.ProjectileType<SupernovaHoming>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner, 0f, 0f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 2f), (float)(-Math.Cos(offsetAngle) * 2f), ModContent.ProjectileType<SupernovaHoming>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner, 0f, 0f);
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
                    if (goreIndex < (goreAmt / 3))
                    {
                        velocityMult = 0.66f;
                    }
                    if (goreIndex >= (2 * goreAmt / 3))
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
            target.ExoDebuffs();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
