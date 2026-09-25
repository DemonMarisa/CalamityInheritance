using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.CAWeapons.Ranged;
using CalamityInheritance.Content.Particles;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.CAWeapons
{
    public class ACTKarasawaBoom : CIRangedProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<ACTKarasawa>();
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
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
                        var par = new SquishyLightParticle(Projectile.Center + Main.rand.NextVector2Circular(6f, 6f) * 2f, Projectile.velocity, Main.rand.NextFloat(1, Projectile.ai[0]), DustColor, Main.rand.Next(24, 33), 0.2f, squish);
                        par.Spawn();
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
                        var par2 = new SquishyLightParticle(Projectile.Center + Projectile.velocity.RotatedBy(Math.PI / 2f) * (float)Math.Sin(Projectile.ai[1]) * 8f, Main.rand.NextVector2Unit() * 0.5f, 0.5f, DustColor, Main.rand.Next(24, 33), 0.2f, squish, 10);
                        par2.Spawn();
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
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!modifiers.SuperArmor && target.LAP().TargetDR <= 0.95f)
            {
                modifiers.DefenseEffectiveness *= 0f;
                modifiers.FinalDamage /= 1 - target.LAP().TargetDR;
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
                    fire.Spawn();
                }
                for (int i = 0; i < 30; i++)
                {
                    SquishyLightParticle fire2 = new SquishyLightParticle(Projectile.Center, Main.rand.NextVector2Unit(0f, 6.2831855f) * Main.rand.NextFloat(6f, 9f), 2f, DustColor, 64, 1.4f, 2.7f, 3f, 0f);
                    fire2.Spawn();
                }
                for (int i = 0; i < 15; i++)
                {
                    SquishyLightParticle fire3 = new SquishyLightParticle(Projectile.Center, Main.rand.NextVector2Unit(0f, 6.2831855f) * Main.rand.NextFloat(4f, 6f), 3f, DustColor, 64, 1.4f, 2.7f, 3f, 0f);
                    fire3.Spawn();
                }
            }
            if (DustColor == Color.Red)
            {
                target.AddBuff(BuffType<CIBrimstoneFlames>(), 300);
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
