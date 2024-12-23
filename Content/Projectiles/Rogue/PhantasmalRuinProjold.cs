using CalamityMod.Projectiles.Rogue;
using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using CalamityInheritance.Utilities;
using CalamityMod.Projectiles.Magic;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class PhantasmalRuinProjold : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Projectiles.Rogue";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Rogue/PhantasmalRuinold";

        private const int Lifetime = 600;
        private const int FramesPerSubProjectile = 13;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = Lifetime;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 3);
            return false;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.SpectreStaff, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 0, default(Color), 0.85f);
            if (Projectile.timeLeft % 18 == 0)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Projectile.Calamity().stealthStrike)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, ModContent.ProjectileType<PhantasmalRuinGhost>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner);
                    }
                    else
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.Center.X, Projectile.Center.Y, 0f, Main.rand.NextFloat(-2, 2), ModContent.ProjectileType<LostSoulFriendly>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner, 1f);
                    }
                }
            }
            bool shouldFireSubProjectile = (Lifetime - Projectile.timeLeft) % (Projectile.MaxUpdates * FramesPerSubProjectile) == 8;
            if (Projectile.owner == Main.myPlayer && shouldFireSubProjectile)
            {
                bool ss = Projectile.Calamity().stealthStrike;
                int soulDamage = (int)(Projectile.damage * 0.7f);
                int projID = ss ? ModContent.ProjectileType<PhantasmalRuinGhost>() : ModContent.ProjectileType<LostSoulFriendly>();
                int soul = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Phantom>(), soulDamage, Projectile.knockBack, Projectile.owner);
                Main.projectile[soul].CalamityInheritance().forceRogue = true;
                int damage = (int)(Projectile.damage * 0.25f);
                float kb = Projectile.knockBack * (ss ? 1f : 0.25f);
                Vector2 velocity = ss
                    ? (Projectile.velocity * 0.4f).RotatedBy(Main.rand.NextFloat(-0.04f, 0.04f))
                    : (Projectile.velocity * 0.08f) + Main.rand.NextVector2Circular(0.4f, 0.4f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, projID, damage, kb, Projectile.owner);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => OnHitEffects();
        public override void OnHitPlayer(Player target, Player.HurtInfo info) => OnHitEffects();

        private void OnHitEffects()
        {
            SoundEngine.PlaySound(SoundID.NPCDeath39 with { PitchVariance = 0.4f }, Projectile.position);
                float spread = 45f * 0.0174f;
                double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 8f;
                double offsetAngle;
                if (Projectile.owner == Main.myPlayer)
                {
                    if (Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<PhantasmalSoulold>()] < 4)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            float ai1 = Main.rand.NextFloat() + 0.5f;
                            float randomSpeed = (float)Main.rand.Next(1, 7);
                            float randomSpeed2 = (float)Main.rand.Next(1, 7);
                            offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                            int num23 = Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f) + randomSpeed, ModContent.ProjectileType<PhantasmalSoulold>(), (int)(Projectile.damage * 0.5), 0f, Projectile.owner, 1f, ai1);
                            int num24 = Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f) + randomSpeed2, ModContent.ProjectileType<PhantasmalSoulold>(), (int)(Projectile.damage * 0.5), 0f, Projectile.owner, 1f, ai1);
                        }
                    }
                }
        }
    }
}
