using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class PristineLegacySecondary : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";

        private int dust1 = (int)CalamityDusts.ProfanedFire;
        private int dust2 = ModContent.DustType<HolyFireDust>();

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 3;
            Projectile.timeLeft = 150;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.98f;
            int dustType = Utils.SelectRandom(Main.rand, new int[]
            {
                dust1,
                dust2
            });
            Lighting.AddLight(Projectile.Center, 1f, 1f, 0.25f);
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
                if (Main.rand.NextBool(2))
                {
                    for (int i = 0; i < 1; i++)
                    {
                        int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1f);
                        Dust dust = Main.dust[d];
                        if (Main.rand.NextBool(3))
                        {
                            dust.noGravity = true;
                            dust.scale *= 1.25f;
                            dust.velocity.X *= 2f;
                            dust.velocity.Y *= 2f;
                        }
                        if (Main.rand.NextBool(6))
                        {
                            dust.noGravity = true;
                            dust.scale *= 1.5f;
                            dust.velocity.X *= 2f;
                            dust.velocity.Y *= 2f;
                        }
                        else
                        {
                            dust.scale *= 1f;
                        }
                        dust.velocity.X *= 1.2f;
                        dust.velocity.Y *= 1.2f;
                        dust.scale *= pScale;
                        dust.noLight = true;
                        dust.color = CalamityUtils.ColorSwap(new Color(255, 168, 53), new Color(255, 249, 0), 2f);
                    }
                }
            }
            else
            {
                Projectile.ai[0] += 1f;
            }
            Projectile.rotation += 0.3f * Projectile.direction;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 240);
        }
    }
}
