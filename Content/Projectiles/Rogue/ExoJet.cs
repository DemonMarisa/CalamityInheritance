using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Particles;
using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class ExoJet : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Melee";
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public override void SetDefaults()
        {
            Projectile.width = 25;
            Projectile.height = 25;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
            Projectile.MaxUpdates = 4;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 cinderSpawnPosition = -Vector2.UnitY.RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat(0f);
                    Vector2 cinderVelocity = -Vector2.UnitY.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0f);
                    Color cinderColor = CalamityUtils.MulticolorLerp(Main.rand.NextFloat(), CalamityUtils.ExoPalette);
                    SquishyLightParticle cinder = new(cinderSpawnPosition, cinderVelocity, 1.1f, cinderColor, 32, 1f, 4f);
                    GeneralParticleHandler.SpawnParticle(cinder);
                }
                Projectile.localAI[0] = 1f;
            }

            // Create smoke.
            for (int i = 0; i < 1; i++)
            {
                Color smokeColor = CalamityUtils.MulticolorLerp(Main.rand.NextFloat(), CalamityUtils.ExoPalette);
                smokeColor = Color.Lerp(smokeColor, Color.Gray, 0.55f);
                HeavySmokeParticle smoke = new(Projectile.Center, Main.rand.NextVector2Circular(0f, 0f), smokeColor, 40, 0.8f, 1f, 0.03f, true, 0.075f);
                GeneralParticleHandler.SpawnParticle(smoke);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
        }
    }
}
