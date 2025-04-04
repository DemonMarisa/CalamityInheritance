using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using CalamityMod;
using CalamityInheritance.Buffs.Statbuffs;
using CalamityInheritance.Utilities;
using CalamityInheritance.System.Configs;
using CalamityInheritance.NPCs.Boss.Calamitas;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class PolarStarLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => $"{GenericProjRoute.LaserProjRoute}";

        private int dust1 = 86;
        private int dust2 = 91;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            if (Projectile.ai[1] == 2f || Projectile.ai[1] == 1f)
            {
                Vector2 value7 = new Vector2(5f, 10f);
                Lighting.AddLight(Projectile.Center, 0.25f, 0f, 0.25f);
                Projectile.localAI[0] += 1f;
                if (Projectile.localAI[0] == 48f)
                {
                    Projectile.localAI[0] = 0f;
                }
                else
                {
                    for (int d = 0; d < 2; d++)
                    {
                        int dustType = d == 0 ? dust1 : dust2;
                        Vector2 value8 = Vector2.UnitX * -12f;
                        value8 = -Vector2.UnitY.RotatedBy((double)(Projectile.localAI[0] * 0.1308997f + d * MathHelper.Pi), default) * value7;
                        int num42 = Dust.NewDust(Projectile.Center, 0, 0, dustType, 0f, 0f, 160, default, 1f);
                        Main.dust[num42].scale = dustType == dust1 ? 1.5f : 1f;
                        Main.dust[num42].noGravity = true;
                        Main.dust[num42].position = Projectile.Center + value8;
                        Main.dust[num42].velocity = Projectile.velocity;
                        int num458 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 100, default, 0.8f);
                        Main.dust[num458].noGravity = true;
                        Main.dust[num458].velocity *= 0f;
                    }
                }
            }
            if (Projectile.ai[1] == 2f) //第三阶段追踪 
            {
                //普灾重生如果在附近，北辰鹦哥鱼将默认追踪并翻倍追踪的属性
                if(CIFunction.IsThereNpcNearby(ModContent.NPCType<CalamitasRebornPhase2>(), Main.player[Projectile.owner], 3000f) || Main.zenithWorld)
                {
                    CIFunction.HomeInOnNPC(Projectile, false, 1600f, 16f, 20f);
                    Projectile.extraUpdates = 3;
                }
                else
                {
                    //注:允许追踪的距离被提升至800f,  同时获得2eU
                    Projectile.extraUpdates = 2;
                    CIFunction.HomeInOnNPC(Projectile, false, 800f, 12f, 20f);
                }
            }
            else if (Projectile.ai[1] == 1f) //第二阶段的分裂
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 25;
                }
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
                float num55 = 30f;
                float num56 = 1f;
                if (Projectile.ai[1] == 1f)
                {
                    Projectile.localAI[0] += num56;
                    if (Projectile.localAI[0] > num55)
                    {
                        Projectile.localAI[0] = num55;
                    }
                }
                else
                {
                    Projectile.localAI[0] -= num56;
                    if (Projectile.localAI[0] <= 0f)
                    {
                        Projectile.Kill();
                    }
                }
            }
            else //普通射弹
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 25;
                }
                if (Projectile.alpha < 0)
                {
                    Projectile.alpha = 0;
                }
                float num55 = 40f;
                float num56 = 1.5f;
                if (Projectile.ai[1] == 0f)
                {
                    Projectile.localAI[0] += num56;
                    if (Projectile.localAI[0] > num55)
                    {
                        Projectile.localAI[0] = num55;
                    }
                }
                else
                {
                    Projectile.localAI[0] -= num56;
                    if (Projectile.localAI[0] <= 0f)
                    {
                        Projectile.Kill();
                    }
                }
            }
        }

        public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, Projectile.alpha);

        public override bool PreDraw(ref Color lightColor) => Projectile.DrawBeam(40f, 1.5f, lightColor);

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if ((target.damage > 5 || target.boss) && Projectile.owner == Main.myPlayer && !target.SpawnedFromStatue)
            {
                player.AddBuff(ModContent.BuffType<PolarisBuffLegacy>(), 480);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item62 with { Volume = SoundID.Item62.Volume * 0.5f}, Projectile.position);
            if (Projectile.ai[1] == 1f)
            {
                int projectiles = Main.rand.Next(2, 5);
                if (Projectile.owner == Main.myPlayer)
                {
                    for (int k = 0; k < projectiles; k++)
                    {
                        int split = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, Main.rand.Next(-10, 11) * 2f, Main.rand.Next(-10, 11) * 2f, ModContent.ProjectileType<ChargedBlastLegacy2>(),
                        (int)(Projectile.damage * 0.85), (int)(Projectile.knockBack * 0.5), Main.myPlayer, 0f, 0f);
                    }
                }
            }
            if (Projectile.ai[1] == 2f)
            {
                Projectile.ExpandHitboxBy(150);
                Projectile.maxPenetrate = -1;
                Projectile.penetrate = -1;
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 10;
                Projectile.damage /= 2;
                Projectile.Damage();

                int dustAmt = 36;
                for (int d = 0; d < dustAmt; d++)
                {
                    Vector2 source = Vector2.Normalize(Projectile.velocity) * new Vector2(Projectile.width / 2f, Projectile.height) * 0.3f;
                    source = source.RotatedBy((double)((d - (dustAmt / 2 - 1)) * MathHelper.TwoPi / dustAmt), default) + Projectile.Center;
                    Vector2 dustVel = source - Projectile.Center;

                    int i = Dust.NewDust(source + dustVel, 0, 0, Main.rand.NextBool(2) ? dust1 : dust2, dustVel.X * 0.3f, dustVel.Y * 0.3f, 100, default, 2f);
                    Main.dust[i].noGravity = true;

                    int j = Dust.NewDust(source + dustVel, 0, 0, Main.rand.NextBool(2) ? dust1 : dust2, dustVel.X * 0.2f, dustVel.Y * 0.2f, 100, default, 2f);
                    Main.dust[j].noGravity = true;

                    int k = Dust.NewDust(source + dustVel, 0, 0, Main.rand.NextBool(2) ? dust1 : dust2, dustVel.X * 0.1f, dustVel.Y * 0.1f, 100, default, 2f);
                    Main.dust[k].noGravity = true;
                }

                bool random = Main.rand.NextBool();
                float angleStart = Main.rand.NextFloat(0f, MathHelper.TwoPi);
                for (float angle = 0f; angle < MathHelper.TwoPi; angle += 0.05f) // 125 dusts
                {
                    random = !random;
                    Vector2 velocity = angle.ToRotationVector2() * (2f + (float)(Math.Sin(angleStart + angle * 3f) + 1) * 2.5f) * Main.rand.NextFloat(0.95f, 1.05f);
                    Dust d = Dust.NewDustPerfect(Projectile.Center, random ? dust1 : dust2, velocity);
                    d.noGravity = true;
                    d.customData = 0.025f;
                    d.scale = 2f;
                }
            }
        }
    }
}
