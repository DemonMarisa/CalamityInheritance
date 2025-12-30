using CalamityMod;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue.Boomerang
{
    public class ReaperFuryLegacy : ModProjectile, ILocalizedModType
    {
        public ref float AttackState => ref Projectile.ai[0];
        public ref float CantHomeIn => ref Projectile.ai[1];
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.DamageType = RogueDamageClass.Instance;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 2;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
        }

        public override void AI()
        {
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] > 10f && Main.rand.NextBool(3))
            {
                int dustAmt = 6;
                for (int i = 0; i < dustAmt; ++i)
                {
                    Vector2 dustRotation = (Vector2.Normalize(Projectile.velocity) * new Vector2((float)Projectile.width, (float)Projectile.height) / 2f).RotatedBy((double)(i - (dustAmt / 2 - 1)) * Math.PI / (double)dustAmt, new Vector2()) + Projectile.Center;
                    Vector2 randomRotation = ((Main.rand.NextFloat() * MathHelper.Pi) - MathHelper.PiOver2).ToRotationVector2() * (float)Main.rand.Next(3, 8);
                    int nuclearDust = Dust.NewDust(dustRotation + randomRotation, 0, 0, DustID.FishronWings, randomRotation.X * 2f, randomRotation.Y * 2f, 100, new Color(), 1.4f);
                    Dust dust = Main.dust[nuclearDust];
                    dust.noGravity = true;
                    dust.noLight = true;
                    dust.velocity /= 4f;
                    dust.velocity -= Projectile.velocity;
                }
                Projectile.alpha -= 5;
                if (Projectile.alpha < 50)
                    Projectile.alpha = 50;
                Lighting.AddLight((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, 0.1f, 0.4f, 0.6f);
            }
            Projectile.rotation += Projectile.velocity.X * 0.1f;
            Projectile.rotation += Projectile.velocity.Y * 0.1f;
            if (CantHomeIn > 0)
                CantHomeIn--;
            if (AttackState == 0)
                Projectile.HomeInNPC(900f, 14f, 14f);
            else
            {
                if (CantHomeIn == 0)
                    Projectile.HomeInNPC(1500f, 14f, 0, 15);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            AttackState++;
            CantHomeIn = 15;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
            for (int k = 0; k < 5; k++)
            {
                int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BreatheBubble, 0f, 0f);
                Main.dust[dust].velocity *= 0f;
                Main.dust[dust].noGravity = true;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(200, 200, 200, 200);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
