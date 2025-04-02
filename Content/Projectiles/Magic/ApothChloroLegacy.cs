using CalamityInheritance.Utilities;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class ApothChloroLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override string Texture => $"{GenericProjRoute.InvisProjRoute}";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.alpha = 70;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 1;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.light = 0.5f;
            Projectile.extraUpdates = 7;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.4f, 0.2f, 0.4f);
            for (int i = 0; i < 5; i++)
            {
                Dust dust4 = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PinkTorch, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 1f)];
                dust4.velocity = Vector2.Zero;
                dust4.position -= Projectile.velocity / 5f * i;
                dust4.noGravity = true;
                dust4.scale = 0.8f;
                dust4.noLight = true;
            }
            float num3 = (float)Math.Sqrt((double)(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y));
            float num4 = Projectile.localAI[0];
            if (num4 == 0f)
            {
                Projectile.localAI[0] = num3;
            }
            if (Projectile.alpha > 0)
                Projectile.alpha = Projectile.alpha - 25;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;
            CIFunction.HomeInOnNPC(Projectile, true, 600f, 30f, 10f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 600, true);
        }
    }
}
