using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System;
using CalamityInheritance.Utilities;
using CalamityMod.Enums;
using CalamityMod.Particles;

namespace CalamityInheritance.Content.Projectiles.Typeless
{
    public class NanotechOld : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public ref float AttackType => ref Projectile.ai[0];
        public ref float AttackTimer => ref Projectile.ai[1];
        public Player Owner => Main.player[Projectile.owner];
        const float IsShooted = 0f;
        const float IsSearchingNewTarget = 1f;
        const float IsFading = 2f;
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 240;
            Projectile.usesLocalNPCImkunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.penetrate = 1;
        }
        public override bool? CanDamage() => AttackTimer > 30f;
        public override Color? GetAlpha(Color lightColor) => base.GetAlpha(lightColor);
        public override void AI()
        {
            DoGeneric();
            switch(AttackType)
            {
                case IsShooted:
                    DoShooted();
                    break;
                case IsSearchingNewTarget:
                    DoSearching();
                    break;
                case IsFading:
                    DoFading();
                    break;
            }
        }

        private void DoFading()
        {
            Projectile.velocity *= 0.94f;
            Projectile.alpha += 10;
            if (Projectile.alpha >= 255)
            {
                Projectile.alpha = 255;
                Projectile.Kill();
                return;
            }
        }

        private void DoGeneric()
        {
            Lighting.AddLight(Projectile.Center, new Vector3(0.075f, 0.4f, 0.15f));
            Projectile.rotation += Projectile.velocity.X * 0.8f;
            //这个计时器是外置的
            AttackTimer += 1f;
        }

        private void DoSearching() 
        {
            if (Projectile.alpha > 0)
                Projectile.alpha -= 15;
            NPC target = Projectile.FindClosestTarget(1800f);
            float speed = 16f + AttackTimer / 30f;
            if (target != null)
                Projectile.HomingNPCBetter(target, 1800f, speed, 20f, 1, speed, null, true);
        }

        private void DoShooted()
        {
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 15;
                return;
            }
            AttackType = IsSearchingNewTarget;
            Projectile.netUpdate = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            AttackType = IsFading;
        }
        public override void OnKill(int timeLeft)
        {
            int inc;
            for (int i = 0; i < 2; i = inc + 1)
            {
                int dustScale = (int)(10f * Projectile.scale);
                int greenDust = Dust.NewDust(Projectile.Center - Vector2.One * dustScale, dustScale * 2, dustScale * 2, DustID.TerraBlade, 0f, 0f, 0, default, 1f);
                Dust nanoDust = Main.dust[greenDust];
                Vector2 dustDirection = Vector2.Normalize(nanoDust.position - Projectile.Center);
                nanoDust.position = Projectile.Center + dustDirection * dustScale * Projectile.scale;
                if (i < 30)
                {
                    nanoDust.velocity = dustDirection * nanoDust.velocity.Length();
                }
                else
                {
                    nanoDust.velocity = dustDirection * Main.rand.Next(45, 91) / 10f;
                }
                nanoDust.color = Main.hslToRgb((float)(0.4+ Main.rand.NextDouble() * 0.2), 0.9f, 0.5f);
                nanoDust.color = Color.Lerp(nanoDust.color, Color.White, 0.3f);
                nanoDust.noGravity = true;
                nanoDust.scale = 0.7f;
                inc = i;
            }
        }
    }
}
