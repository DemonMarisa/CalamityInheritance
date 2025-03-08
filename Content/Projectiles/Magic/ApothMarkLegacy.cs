using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using System;
using Terraria;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class ApothMarkLegacy : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Jaws of Annihilation");
        }

        public override void SetDefaults()
        {
            Projectile.width = 123;
            Projectile.height = 105;
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
            if (Projectile.timeLeft < 30)
                Projectile.alpha = Projectile.alpha + 6;
            else if (Projectile.timeLeft < 150)
            {
                Projectile.velocity.X *= 0.9f;
                Projectile.velocity.Y *= 0.9f;
            }
            else if (Projectile.timeLeft == 180)
            {
                Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
                double offsetHyp = Math.Sqrt(48 * 48 + 38 * 38);
                double offsetRotation = Math.Atan2(-38, 48) + Projectile.rotation;
                double offsetX = offsetHyp * Math.Cos(offsetRotation);
                double offsetY = offsetHyp * Math.Sin(offsetRotation);
                if (Projectile.owner == Main.myPlayer)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.Center.X + (float)offsetX, Projectile.Center.Y + (float)offsetY, Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<ApothJawsLegacy>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.rotation, 0f);
                }
                offsetRotation = Math.Atan2(38, 48) + Projectile.rotation;
                offsetX = offsetHyp * Math.Cos(offsetRotation);
                offsetY = offsetHyp * Math.Sin(offsetRotation);

                if (Projectile.owner == Main.myPlayer)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + Projectile.width / 2 + (float)offsetX, Projectile.position.Y + Projectile.height / 2 + (float)offsetY, Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<ApothJawsLegacy>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.rotation, 1f);     
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
