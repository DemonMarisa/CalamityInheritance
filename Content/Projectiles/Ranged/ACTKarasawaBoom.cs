using System;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Humanizer.In;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class ACTKarasawaBoom: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";
        private Color DustColor;
  
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.usesLocalNPCImmunity = false;
            Projectile.MaxUpdates = 49;
            Projectile.penetrate = 1;
            Projectile.alpha = 100;
            Projectile.timeLeft = 3000;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {

            if (Projectile.frameCounter >= 15)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (Projectile.frameCounter >= 25)
                    {
                        Projectile.ai[0] *= 0.99f;
                        if (Projectile.ai[0] <= 1.5f) Projectile.ai[0] = 1.5f;
                    }
                    if (Main.netMode != NetmodeID.Server)
                    {
                        if (Main.rand.NextBool())
                        {
                            Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(6f, 6f), DustID.RainbowMk2, Main.rand.NextVector2Unit() + Projectile.velocity);
                            d.noGravity = true;
                            d.scale = Projectile.ai[0] * 1.5f;
                            d.color = DustColor;
                            d.color.A = 0;
                        }
                        float squish = Projectile.frameCounter > 30 ? Projectile.frameCounter / 6.67f : 1.5f;
                        Particle par = new SquishyLightParticle(Projectile.Center + Main.rand.NextVector2Circular(6f, 6f) * 2f, Projectile.velocity, Main.rand.NextFloat(1, Projectile.ai[0]), DustColor, Main.rand.Next(24, 33), 0.2f, squish);
                        GeneralParticleHandler.SpawnParticle(par);
                    }
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    if (Projectile.ai[1] != -1f)
                    {
                        Dust d = Dust.NewDustPerfect(Projectile.Center - Projectile.velocity.RotatedBy(Math.PI / 2f) * (float)Math.Sin(Projectile.ai[1]) * 8f + Main.rand.NextVector2Unit(), DustID.RainbowMk2, Main.rand.NextVector2Unit());
                        d.noGravity = true;
                        d.scale = Projectile.ai[0];
                        d.color = DustColor;
                        d.color.A = 0;

                        float squish = Projectile.frameCounter > 30 ? Projectile.frameCounter / 6.67f : 1.5f;
                        Particle par2 = new SquishyLightParticle(Projectile.Center + Projectile.velocity.RotatedBy(Math.PI / 2f) * (float)Math.Sin(Projectile.ai[1]) * 8f, Main.rand.NextVector2Unit() * 0.5f, 0.5f, DustColor, Main.rand.Next(24, 33), 0.2f, squish, 10);
                        GeneralParticleHandler.SpawnParticle(par2);
                    }

                }
                if (Projectile.ai[1] != -1f) Projectile.ai[1] += 0.1f;
            }
            else
            {
                DustColor = Color.Lerp(Color.DodgerBlue, Color.Red, Projectile.ai[0]);
            }
            if (Projectile.frameCounter <= 30) Projectile.frameCounter++;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.numHits >= 2 && Main.player[Projectile.owner].Calamity().adrenalineModeActive && !Main.player[Projectile.owner].Calamity().draedonsHeart)
            {
                Main.player[Projectile.owner].Calamity().adrenaline = 0;
            }
            if (Projectile.numHits <= 0 && CalamityPlayer.areThereAnyDamnBosses)
            {
                CombatText.NewText(Main.player[Projectile.owner].Hitbox, Color.Red, Language.GetTextValue("Mods.CalamityInheritance.Status.KarasawaMiss"), dramatic: true);
            }
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!modifiers.SuperArmor && target.Calamity().DR <= 0.95f)
            {
                modifiers.DefenseEffectiveness *= 0f;
                modifiers.FinalDamage /= 1 - target.Calamity().DR;
            }
            if (Main.player[Projectile.owner].Calamity().adrenalineModeActive && !Main.player[Projectile.owner].Calamity().draedonsHeart)
            {
                float totalDamageMult = 1f;
                float extraMult = 0f;
                if (Main.player[Projectile.owner].Calamity().enraged)
                {
                    totalDamageMult += 1.25f;
                }
                if (Main.player[Projectile.owner].Calamity().witheredDebuff && Main.player[Projectile.owner].Calamity().witheringWeaponEnchant)
                {
                    totalDamageMult += 0.6f;
                }
                CalamityUtils.ApplyRippersToDamage(Main.player[Projectile.owner].GetModPlayer<CalamityPlayer>(), false, ref extraMult);
                modifiers.SourceDamage *= totalDamageMult + extraMult * 0.5f;
                modifiers.SourceDamage /= totalDamageMult + extraMult;

            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!Main.dedServ)
            {
                for (int i = 0; i < 60; i++)
                {
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.RainbowMk2, Main.rand.NextVector2Unit(0f, 6.2831855f) * Main.rand.NextFloat(2f, 32f));
                    d.noGravity = true;
                    d.scale = Main.rand.NextFloat(1.6f, 2f);
                    d.color = DustColor;
                    d.color.A = 0;
                }
                for (int i = 0; i < 45; i++)
                {
                    SquishyLightParticle fire = new SquishyLightParticle(Projectile.Center, Main.rand.NextVector2Unit(0f, 6.2831855f) * Main.rand.NextFloat(8f, 12f), 1f, DustColor, 64, 1.4f, 2.7f, 3f, 0f);
                    GeneralParticleHandler.SpawnParticle(fire);
                }
                for (int i = 0; i < 30; i++)
                {
                    SquishyLightParticle fire2 = new SquishyLightParticle(Projectile.Center, Main.rand.NextVector2Unit(0f, 6.2831855f) * Main.rand.NextFloat(6f, 9f), 2f, DustColor, 64, 1.4f, 2.7f, 3f, 0f);
                    GeneralParticleHandler.SpawnParticle(fire2);
                }
                for (int i = 0; i < 15; i++)
                {
                    SquishyLightParticle fire3 = new SquishyLightParticle(Projectile.Center, Main.rand.NextVector2Unit(0f, 6.2831855f) * Main.rand.NextFloat(4f, 6f), 3f, DustColor, 64, 1.4f, 2.7f, 3f, 0f);
                    GeneralParticleHandler.SpawnParticle(fire3);
                }
            }
            if (DustColor == Color.Red)
            {
                target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
            }
            if (Projectile.numHits == 0)
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.timeLeft = 5;
                Projectile.maxPenetrate = -1;
                Projectile.penetrate = -1;
                Projectile.usesLocalNPCImmunity = true;
                Projectile.localNPCHitCooldown = 10;
                Projectile.position = Projectile.Center;
                Projectile.width = 800;
                Projectile.height = 800;
                Projectile.position.X = Projectile.position.X - Projectile.width / 2;
                Projectile.position.Y = Projectile.position.Y - Projectile.height / 2;
            }
        }
    }
}
