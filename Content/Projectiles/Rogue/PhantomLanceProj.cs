﻿using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Projectiles.Magic;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items.Weapons;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class PhantomLanceProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => $"{Generic.WeaponPath}/Rogue/PhantomLance";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void AI()
        {
            if (!Projectile.Calamity().stealthStrike)
            {
                if (Projectile.timeLeft <= 255)
                    Projectile.alpha += 1;
                if (Projectile.timeLeft >= 75)
                {
                    Projectile.velocity.X *= 0.995f;
                    Projectile.velocity.Y *= 0.995f;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.SpectreStaff, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 0, default, 0.85f);
            Projectile.ai[0]++;
            if (Projectile.ai[0] % 18f == 0f)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    float damageMult = Projectile.timeLeft * 0.7f / 300f;
                    if (Projectile.Calamity().stealthStrike)
                        damageMult = 0.7f;
                    int soulDamage = (int)(Projectile.damage * damageMult);
                    int soul = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Phantom>(), soulDamage, Projectile.knockBack, Projectile.owner);
                    Main.projectile[soul].DamageType = ModContent.GetInstance<RogueDamageClass>();
                    if (soul.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[soul].DamageType = ModContent.GetInstance<RogueDamageClass>();
                        Main.projectile[soul].usesLocalNPCImmunity = true;
                        Main.projectile[soul].localNPCHitCooldown = -2;
                    }
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i <= 10; i++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.SpectreStaff, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }
    }
}
