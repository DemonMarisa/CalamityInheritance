using CalamityInheritance.Utilities;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class TerraFireGreenLegacy2 : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 2;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 150;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 7;
        }

        public override void AI()
        {
            if (Main.zenithWorld)
            CIFunction.HomeInOnNPC(Projectile, true, 1800f, 24f, 20f);
            Lighting.AddLight(Projectile.Center, 0.15f, 0.45f, 0f);
            if (Projectile.ai[0] > 7f)
            {
                float pScale = 1f;
                if (Projectile.ai[0] == 8f)
                {
                    pScale = 0.25f;
                }
                else if (Projectile.ai[0] == 9f)
                {
                    pScale = 0.5f;
                }
                else if (Projectile.ai[0] == 10f)
                {
                    pScale = 0.75f;
                }
                Projectile.ai[0] += 1f;
                int dType = 66;
                if (Main.rand.NextBool(2))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 0.75f);
                        Dust dust = Main.dust[d];
                        if (Main.rand.NextBool(3))
                        {
                            dust.noGravity = true;
                            dust.scale *= 1.75f;
                            dust.velocity.X *= 2f;
                            dust.velocity.Y *= 2f;
                        }
                        else
                        {
                            dust.noGravity = true;
                            dust.scale *= 0.5f;
                        }
                        dust.velocity.X *= 1.2f;
                        dust.velocity.Y *= 1.2f;
                        dust.scale *= pScale;
                        dust.velocity += Projectile.velocity;
                    }
                }
            }
            else
            {
                Projectile.ai[0] += 1f;
            }
            Projectile.rotation += 0.3f * Projectile.direction;
        }
    }
}
