using CalamityInheritance.Content.Items.Weapons.DraedonsArsenal;
using CalamityInheritance.Content.Projectiles.DraedonsArsenal;
using CalamityInheritance.Sounds.Cals;
using LAP.Core.BaseClass.Projectiles;
using LAP.Core.MiscDate;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Magic
{
    public class GatlingLaserHeldProj : BaseHeldProj, ILocalizedModType
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<GatlingLaserLegacy>();
        public SlotId gatlingLaserLoopID;
        public int dust = LAPDustID.DustVampireHeal;
        public override void ExSSD()
        {
            Main.projFrames[Projectile.type] = 4;
            Projectile.RegisterFrame(new Point(1, 4));
        }
        public override void ExSD()
        {
            Projectile.width = 58;
            Projectile.height = 24;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void Initialize()
        {
            RotAmount = 0.25f;
        }
        public override void ExAI()
        {
            if (Projectile.LAP().FirstFrame)
            {
                SoundEngine.PlaySound(CICalSounds.GatlingLaserFireStart, Projectile.Center);
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 13)
            {
                Projectile.frameCounter = 10;
                frameY++;
                if (frameY >= Main.projFrames[Projectile.type])
                    frameY = 0;
                if (Projectile.soundDelay <= 0)
                {
                    Projectile.soundDelay = 12;
                    gatlingLaserLoopID = SoundEngine.PlaySound(CICalSounds.GatlingLaserFireLoop, Projectile.Center);
                }
                if (UseDelay <= 0)
                {
                    if (Owner.CheckMana(Owner.GetRealManaCost(Owner.ActiveItem().mana), true))
                    {
                        LaserBurst(2.4f, 4.2f); // 60 dusts
                        Vector2 fireoffset = new Vector2(0, 1);
                        Vector2 fireoffset2 = new Vector2(20, 0).RotatedBy(Projectile.rotation);
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + fireoffset + fireoffset2, Projectile.velocity.RotatedByRandom(0.02f) * 3, ProjectileType<GatlingLaserShot>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                        UseDelay = 2;
                    }
                    else
                    {
                        if (SoundEngine.TryGetActiveSound(gatlingLaserLoopID, out var ShootingSound) && ShootingSound.IsPlaying)
                            ShootingSound.Stop();
                        Projectile.Kill();
                    }
                }
            }
            Owner.SetArmRot(Projectile.rotation);
            DrawPosOffset = new Vector2(20, 0).RotatedBy(Projectile.rotation);
        }
        public override void OnKill(int timeLeft)
        {
            if (SoundEngine.TryGetActiveSound(gatlingLaserLoopID, out var ShootingSound) && ShootingSound.IsPlaying)
                ShootingSound.Stop();
            SoundEngine.PlaySound(CICalSounds.GatlingLaserFireEnd, Projectile.Center);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Main.gamePaused)
            {
                if (SoundEngine.TryGetActiveSound(gatlingLaserLoopID, out var ShootingSound) && ShootingSound.IsPlaying)
                    ShootingSound.Stop();
            }
            base.PreDraw(ref lightColor);
            return false;
        }
        private void LaserBurst(float speed1, float speed2)
        {
            float angleRandom = 0.05f;
            Vector2 fireoffset = new Vector2(40, 0).RotatedBy(Projectile.rotation) + new Vector2(0, 1);
            int inc;
            for (int j = 0; j < 4; j = inc + 1)
            {
                float dustSpeed = Main.rand.NextFloat(speed1, speed2);
                Vector2 dustVel = new Vector2(dustSpeed, 0.0f).RotatedBy(Projectile.velocity.ToRotation());
                dustVel = dustVel.RotatedBy(-angleRandom);
                dustVel = dustVel.RotatedByRandom(2.0f * angleRandom);

                int burstDust = Dust.NewDust(Projectile.Center, 1, 1, dust, dustVel.X, dustVel.Y, 200, default, 0.9f);
                Main.dust[burstDust].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() + fireoffset;
                Main.dust[burstDust].noGravity = true;

                Dust dust2 = Main.dust[burstDust];
                dust2.velocity *= 2f;
                dust2 = Main.dust[burstDust];

                burstDust = Dust.NewDust(Projectile.Center, 1, 1, dust, dustVel.X, dustVel.Y, 100, default, 0.66f);
                Main.dust[burstDust].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() + fireoffset;

                dust2 = Main.dust[burstDust];
                dust2.velocity *= 1.5f;

                Main.dust[burstDust].noGravity = true;
                Main.dust[burstDust].fadeIn = 1f;
                Main.dust[burstDust].color = Color.Crimson * 0.5f;

                dust2 = Main.dust[burstDust];

                inc = j;
            }
            for (int k = 0; k < 2; k = inc + 1)
            {
                float dustSpeed = Main.rand.NextFloat(speed1, speed2);
                Vector2 dustVel = new Vector2(dustSpeed, 0.0f).RotatedBy(Projectile.velocity.ToRotation());
                dustVel = dustVel.RotatedBy(-angleRandom);
                dustVel = dustVel.RotatedByRandom(2.0f * angleRandom);

                int moreBurstDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dust, dustVel.X, dustVel.Y, 0, default, 1.2f);
                Main.dust[moreBurstDust].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(MathHelper.Pi).RotatedBy(Projectile.velocity.ToRotation()) + fireoffset;
                Main.dust[moreBurstDust].noGravity = true;

                Dust dust2 = Main.dust[moreBurstDust];
                dust2.velocity *= 0.5f;
                dust2 = Main.dust[moreBurstDust];

                inc = k;
            }
        }
    }
}
