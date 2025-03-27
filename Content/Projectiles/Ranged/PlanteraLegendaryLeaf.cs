using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class PlanteraLegendaryLeaf : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.CIMod().PlanteraLegendaryTier1)
                Projectile.extraUpdates = 2;
            Projectile.alpha -= 2;
            //还有你能告诉我这两个ai是干吗用的吗？
            Projectile.ai[0] = Main.rand.Next(-100, 101) * 0.0025f;
            Projectile.ai[1] = Main.rand.Next(-100, 101) * 0.0025f;
            switch (Projectile.localAI[0])
            {
                case 0f:
                    Projectile.scale += 0.05f;
                    Projectile.localAI[0] = Projectile.scale > 1.2 ? 1f : 0f;
                    break;
                case 1f:
                    Projectile.scale -= 0.05f;
                    Projectile.localAI[0] = Projectile.scale < 0.8 ? 1f : 0f;
                    break;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
            //用表达式几行就写完了……哪里需要这种嵌套ifelse……
            Projectile.localAI[1] += (Projectile.localAI[1] > 30f && Projectile.localAI[1] <= 60f) ? 2f : 1f;
            Projectile.velocity.X *= (Projectile.localAI[1] > 30f && Projectile.localAI[1] <= 60f) ? 1.025f : 0.975f;
            Projectile.velocity.Y *= (Projectile.localAI[1] > 30f && Projectile.localAI[1] <= 60f) ? 1.025f : 0.975f;
            if (Projectile.localAI[1] > 60f) Projectile.localAI[1] = 0f;
            if (Main.player[Projectile.owner].CIMod().PlanteraLegendaryTier3 && Main.rand.NextBool(2))
                CIFunction.HomeInOnNPC(Projectile, false, 1800f, 24f, 20f);
                
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(Main.DiscoR, 203, 103, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Grass, Projectile.position);
            Projectile.localAI[1] += 1f;
            for (int i = 0; i < 5; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.ChlorophyteWeapon, 0f, 0f, 0, new Color(Main.DiscoR, 203, 103), 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 3f;
                Main.dust[dust].scale = 1.5f;
            }
        }
    }
}