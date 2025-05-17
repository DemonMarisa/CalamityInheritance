using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Graphics.Metaballs;
using CalamityMod.Items.Weapons.Ranged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.CalProjChange
{
    public class NewExoFire : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";
        public ref float LightPower => ref Projectile.ai[2];

        public Color sparkColor;
        public int Time = 0;
        public ref int audioCooldown => ref Main.player[Projectile.owner].Calamity().PhotoAudioCooldown;
        public ref int PhotoTimer => ref Main.player[Projectile.owner].Calamity().PhotoTimer;

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 200;
            Projectile.timeLeft = 200;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 6;
        }

        public override void AI()
        {
            sparkColor = Main.rand.Next(4) switch
            {
                0 => Color.Red,
                1 => Color.MediumTurquoise,
                2 => Color.Orange,
                _ => Color.LawnGreen,
            };

            Time++;
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.2f);
            Player Owner = Main.player[Projectile.owner];
            float targetDist = Vector2.Distance(Owner.Center, Projectile.Center); //used for some drawing prevention for when it's offscreen since it makes a fuck load of particles
            if (targetDist < 1400f)
            {
                if (PhotoTimer == 0)
                    PhotoMetaball3.SpawnParticle(Projectile.Center + Owner.velocity, (42 - Time * 0.165f) * 0.5f);
                if (PhotoTimer == 1)
                    PhotoMetaball3.SpawnParticle(Projectile.Center + Owner.velocity, (37 - Time * (PhotoTimer == 0 ? 0.165f : 0.088f) - PhotoTimer * 0.2f + (PhotoTimer == 1 ? 20 : 0)) * 0.5f); ;

                PhotoMetaball4.SpawnParticle(Projectile.Center + Owner.velocity, (37 - Time * (PhotoTimer == 0 ? 0.165f : 0.088f) - PhotoTimer * 0.2f + (PhotoTimer == 1 ? 20 : 0)) * 0.5f);
            }
            if (Main.rand.NextBool(35) && targetDist < 1400f && Time > 5)
            {
                Dust dust = Dust.NewDustPerfect(Projectile.Center, 263, new Vector2(0, -5).RotatedByRandom(0.05f) * Main.rand.NextFloat(0.3f, 1.6f));
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(0.3f, 1f);
                dust.color = sparkColor;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
            if (audioCooldown == 0)
            {
                SoundEngine.PlaySound(Photoviscerator.HitSound, target.Center);
                audioCooldown = 16;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);

    }
}
