using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Projectiles.Magic;
using System;
using Terraria;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class ApothJawsLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        private const float degrees = (float)(Math.PI / 180) * 2;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Jaws of Annihilation");
        }

        public override void SetDefaults()
        {
            Projectile.width = 144;
            Projectile.height = 72;
            Projectile.alpha = 70;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.light = 1.5f;
        }

        public override void AI()
        {
            if (Projectile.timeLeft % 10 == 0)
            {
                double angle = Main.rand.Next(360) * Math.PI / 180;
                float offsetX = Projectile.position.X + Main.rand.Next(Projectile.width);
                float offsetY = Projectile.position.Y + Main.rand.Next(Projectile.height);
                if (Projectile.owner == Main.myPlayer)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(),offsetX, offsetY, 14 * (float)Math.Cos(angle), 14 * (float)Math.Sin(angle), ModContent.ProjectileType<ApothChloroLegacy>(), Projectile.damage, Projectile.knockBack / 2, Projectile.owner);
                }
            }
            if (Projectile.timeLeft < 30)
                Projectile.alpha = Projectile.alpha + 6;
            else if (Projectile.timeLeft < 150)
            {
                Projectile.velocity.X *= 0.9f;
                Projectile.velocity.Y *= 0.9f;
            }
            else if (Projectile.timeLeft < 180)
            {
                if (Projectile.ai[1] == 0)
                    Projectile.rotation += 1.3f * degrees;
                else
                    Projectile.rotation -= 1.3f * degrees;
            }
            else if (Projectile.timeLeft == 180)
            {
                if (Projectile.ai[1] == 0)
                    Projectile.rotation = Projectile.ai[0] - 30 * degrees;
                else
                {
                    Projectile.rotation = Projectile.ai[0] + 30 * degrees + (float)Math.PI;
                    Projectile.spriteDirection = -1;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 600, true);
            target.AddBuff(ModContent.BuffType<ArmorCrunch>(), 600, true);
        }
    }
}
