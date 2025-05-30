using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Magic;
using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityMod.Projectiles.Rogue;
using Mono.Cecil;

namespace CalamityInheritance.Content.Projectiles.ExoLore
{
    public class CIVividBeamExoLore : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";

        private bool initialized = false;
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            if (!initialized)
            {
                SoundEngine.PlaySound(Utils.SelectRandom(Main.rand, VividClarityOld.UseSound), Projectile.Center);
                initialized = true;
                float dustAmt = 16f;
                int d = 0;
                while (d < dustAmt)
                {
                    Vector2 offset = Vector2.UnitX * 0f;
                    offset += -Vector2.UnitY.RotatedBy((double)(d * (MathHelper.TwoPi / dustAmt)), default) * new Vector2(1f, 4f);
                    offset = offset.RotatedBy((double)Projectile.velocity.ToRotation(), default);
                    int i = Dust.NewDust(Projectile.Center, 0, 0, DustID.RainbowTorch, 0f, 0f, 0, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
                    Main.dust[i].scale = 1.5f;
                    Main.dust[i].noGravity = true;
                    Main.dust[i].position = Projectile.Center + offset;
                    Main.dust[i].velocity = Projectile.velocity * 0f + offset.SafeNormalize(Vector2.UnitY) * 1f;
                    d++;
                }
            }

            float pi = MathHelper.Pi;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] == 48f)
            {
                Projectile.ai[0] = 0f;
            }
            else
            {
                for (int d = 0; d < 2; d++)
                {
                    Vector2 offset = Vector2.UnitX * -12f;
                    offset = -Vector2.UnitY.RotatedBy((double)(Projectile.ai[0] * pi / 24f + d * pi), default) * new Vector2(5f, 10f) - Projectile.rotation.ToRotationVector2() * 10f;
                    int i = Dust.NewDust(Projectile.Center, 0, 0, DustID.RainbowTorch, 0f, 0f, 160, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
                    Main.dust[i].scale = 0.75f;
                    Main.dust[i].noGravity = true;
                    Main.dust[i].position = Projectile.Center + offset;
                    Main.dust[i].velocity = Projectile.velocity;
                }
            }
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 4f)
            {
                for (int d = 0; d < 2; d++)
                {
                    Vector2 source = Projectile.position;
                    source -= Projectile.velocity * (d * 0.25f);
                    int i = Dust.NewDust(source, 1, 1, DustID.RainbowTorch, 0f, 0f, 0, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1f);
                    Main.dust[i].noGravity = true;
                    Main.dust[i].position = source;
                    Main.dust[i].scale = Main.rand.NextFloat(0.91f, 1.417f);
                    Main.dust[i].velocity *= 0.1f;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SummonLasers();
                SummonComet();
            }
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                SummonLasers();
                SummonComet();
            }
            target.ExoDebuffs();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Player player = Main.player[Projectile.owner];
            CalamityInheritancePlayer usPlayer = player.CIMod();

            if (Projectile.owner == Main.myPlayer)
            {
                SummonLasers();
            }
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
        }
        public void SummonComet()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.netUpdate = true;
            Vector2 playerToMouseVec = CalamityUtils.SafeDirectionTo(Main.LocalPlayer, Main.MouseWorld, -Vector2.UnitY);
            for (int k = 0; k < 3; k++)
            {
                // 位置计算
                float warpDist = Main.rand.NextFloat(500f, 700f);
                float warpAngle = Main.rand.NextFloat(-MathHelper.Pi / 4f, MathHelper.Pi / 4f);
                Vector2 warpOffset = -warpDist * playerToMouseVec.RotatedBy(warpAngle);
                Vector2 Finalposition = player.Center + warpOffset;

                Vector2 mouseToPlayer = (player.Center - Main.MouseWorld).SafeNormalize(Vector2.UnitY);
                // 方向计算
                float randomAngle = mouseToPlayer.ToRotation() +  Main.rand.NextFloat(-MathHelper.ToRadians(8), MathHelper.ToRadians(8));
                Vector2 randomDirection = new((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle));

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Finalposition, -randomDirection * 15f, ModContent.ProjectileType<CIExocometMagic>(), Projectile.damage * 2, Projectile.knockBack, Projectile.owner);
            }
        }
        public void SummonLasers()
        {
            var source = Projectile.GetSource_FromThis();
            switch (Projectile.ai[1])
            {
                case 0f:
                    CalamityUtils.ProjectileRain(source, Projectile.Center, 380f, 100f, 400f, 640f, 12f, ModContent.ProjectileType<VividClarityBeam>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    Projectile.NewProjectile(source, Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SupernovaBoomOld>(), Projectile.damage * 2, Projectile.knockBack, Projectile.owner);
                    break;

                case 1f:
                    if (!Main.zenithWorld)
                        Projectile.NewProjectile(source, Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SupernovaBoomOld>(), Projectile.damage * 2, Projectile.knockBack, Projectile.owner);
                    else
                    {
                        int i = Projectile.NewProjectile(source, Projectile.Center, Vector2.Zero, ModContent.ProjectileType<RainbowComet>(), Projectile.damage * 2, Projectile.knockBack, Projectile.owner);
                        Main.projectile[i].DamageType = DamageClass.Magic;
                    }
                    break;

                case 2f:
                    float spread = 30f * 0.01f * MathHelper.PiOver2;
                    double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                    double deltaAngle = spread / 8f;
                    double offsetAngle;
                    for (int i = 0; i < 4; i++)
                    {
                        offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;

                        Projectile.NewProjectile(source, Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<VividLaser2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                        Projectile.NewProjectile(source, Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<VividLaser2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                    int boomType = Main.zenithWorld? ModContent.ProjectileType<RainbowComet>(): ModContent.ProjectileType<SupernovaBoomOld>();
                    int p = Projectile.NewProjectile(source, Projectile.Center, Vector2.Zero, boomType, Projectile.damage * 2, Projectile.knockBack, Projectile.owner);
                    break;
            }
        }
    }
}
