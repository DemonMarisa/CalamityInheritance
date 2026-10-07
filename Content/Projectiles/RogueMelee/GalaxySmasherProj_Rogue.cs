using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.RogueMelee;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.Melee.CurvedSword;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.RogueMelee
{
    public class GalaxySmasherProj_Rogue : CIMeleeRogueProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<GalaxySmasher>();
        public override string Texture => GetInstance<GalaxySmasher>().Texture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 62;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 4;
            Projectile.timeLeft = 240;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
            ModItem item = Projectile.Owner()?.HeldItem?.ModItem;
            if (item is not null && item is CIMeleeRogue rogue && rogue.IsMelee)
                Projectile.DamageType = DamageClass.Melee;
            else
                Projectile.DamageType = RogueDamage.Instance;
        }
        public override void AI()
        {
            int numDust = 2;
            for (int i = 0; i < numDust; ++i)
            {
                int dustType = Main.rand.NextBool(6) ? 112 : 173;
                float scale = 0.8f + Main.rand.NextFloat(0.6f);
                int idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity = Vector2.Zero;
                Main.dust[idx].scale = scale;
            }
            Player owner = Main.player[Projectile.owner];
            DrawOffsetX = -11;
            DrawOriginOffsetY = -10;
            DrawOriginOffsetX = 0;
            Lighting.AddLight(Projectile.Center, 0.7f, 0.3f, 0.6f);
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 60;
                SoundEngine.PlaySound(CISoundID.SoundBoomerangs, Projectile.position);
            }
            Projectile.HomeInNPC(1250f, 24f, 20f);
            Projectile.rotation += 0.22f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProj(ProjectileType<GalaxySmasherMountProj_Rogue>(), Projectile.Center, Projectile.velocity, 1f, 1f);
            // Applies God Slayer Inferno on contact.
            target.AddBuff(BuffType<CIGodSlayerInferno>(), 240);
            OnHitEffect(target.Center);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            // Applies God Slayer Inferno on contact.
            target.AddBuff(BuffType<CIGodSlayerInferno>(), 240);
            OnHitEffect(target.Center);
        }

        private void OnHitEffect(Vector2 targetPos)
        {
            // Some dust gets produced on impact.
            int dustSets = Main.rand.Next(5, 8);
            int dustRadius = 6;
            Vector2 corner = new Vector2(targetPos.X - dustRadius, targetPos.Y - dustRadius);
            for (int i = 0; i < dustSets; ++i)
            {
                // Bigger, flying orb dust
                float scaleOrb = 1.2f + Main.rand.NextFloat(1f);
                int orb = Dust.NewDust(corner, 2 * dustRadius, 2 * dustRadius, DustID.Clentaminator_Purple);
                Main.dust[orb].noGravity = true;
                Main.dust[orb].velocity *= 4f;
                Main.dust[orb].scale = scaleOrb;

                // Add six sparkles per flying orb
                for (int j = 0; j < 6; ++j)
                {
                    float scaleSparkle = 0.8f + Main.rand.NextFloat(1.1f);
                    int sparkle = Dust.NewDust(corner, 2 * dustRadius, 2 * dustRadius, DustID.ShadowbeamStaff);
                    Main.dust[sparkle].noGravity = true;
                    float dustSpeed = Main.rand.NextFloat(10f, 18f);
                    Main.dust[sparkle].velocity = Main.rand.NextVector2Unit() * dustSpeed;
                    Main.dust[sparkle].scale = scaleSparkle;
                }
            }

            // Makes an explosion sound.
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            int laserID = ProjectileType<NebulaShot>();
            int laserDamage = (int)(0.2f * Projectile.damage);
            float laserKB = 2.5f;
            int numLasers = 3;
            for (int i = 0; i < numLasers; ++i)
            {
                float startDist = Main.rand.NextFloat(260f, 270f);
                Vector2 startDir = Main.rand.NextVector2Unit();
                Vector2 startPoint = targetPos + startDir * startDist;

                float laserSpeed = Main.rand.NextFloat(15f, 18f);
                Vector2 velocity = startDir * -laserSpeed;

                if (Projectile.owner == Main.myPlayer)
                {
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), startPoint, velocity, laserID, laserDamage, laserKB, Projectile.owner);
                    if (proj.InBounds(Main.maxProjectiles))
                    {
                        Main.projectile[proj].DamageType = DamageClass.MeleeNoSpeed;
                        Main.projectile[proj].tileCollide = false;
                        Main.projectile[proj].timeLeft = 30;
                    }
                }
            }
        }
    }
}
