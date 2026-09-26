using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.RogueMelee;
using CalamityInheritance.Content.Projectiles.Melee.CurvedSword;
using CalamityInheritance.Content.Projectiles.Melee.Yoyos;
using CalamityInheritance.Core.Path;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.RogueMelee
{
    public class EradicatorProj_Rogue : BaseStickyProj, ILocalizedModType
    {
        public new string LocalizationCategory => LocalizationPath.RogueMeleeProj;
        public override string Texture => GetInstance<Eradicator>().Texture;
        private static float RotationIncrement = 0.15f;
        private static int Lifetime = 350;
        public static int StealthExtraLifetime = 240; // 1 extra update means this is double what you'd expect for 2 seconds
        private static int ReboundTime = 60;

        private float randomLaserCharge = 0f;
        public int Time;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 62;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 2;
            Projectile.timeLeft = Lifetime;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 18;
            ModItem item = Projectile.Owner()?.HeldItem?.ModItem;
            if (item is not null && item is CIMeleeRogue rogue && rogue.IsMelee)
                Projectile.DamageType = DamageClass.Melee;
            else
                Projectile.DamageType = RogueDamage.Instance;
        }

        public override void ExAI()
        {
            Time++;
            if (Projectile.timeLeft == Lifetime - ReboundTime)
                Projectile.netUpdate = true;
            if (Projectile.timeLeft <= Lifetime - ReboundTime)
            {
                Player owner = Main.player[Projectile.owner];
                Projectile.PredictHomeIn(12f, 12f, owner);
                if (Main.myPlayer == Projectile.owner)
                    if (Projectile.Hitbox.Intersects(owner.Hitbox))
                        Projectile.Kill();
            }
            Lighting.AddLight(Projectile.Center, 0.35f, 0f, 0.25f);
            float spin = Projectile.direction <= 0 ? -1f : 1f;
            Projectile.rotation += spin * RotationIncrement;
            if (isSticky)
                StealthStrikeGrind(spin);
            else
            {
                if (Time % 10 == 0)
                {
                    if (LAPUtilities.IsLocalPlayer(Projectile.owner))
                    {
                        NPC npc = LAPUtilities.FindClosestTarget(Projectile.Center, 900);
                        if (npc is not null)
                        {
                            int damage = (int)(Projectile.damage * 0.8f);
                            Vector2 vel = LAPUtilities.GetVector2(Projectile.Center, npc.Center) * 9f;
                            Projectile proj = LAPUtilities.NewProjWithClass(Projectile.GetSource_FromThis(), Projectile.Center, vel, ProjectileType<NebulaShotLegacy>(), damage, Projectile.knockBack, Projectile.owner, DamageClass.Melee);
                        }
                    }
                }
            }
        }

        private void StealthStrikeGrind(float spinDir)
        {
            // Spin extra fast to visually shred the enemy.
            Projectile.rotation += spinDir * RotationIncrement * 0.8f;

            // Randomly fire lasers while grinding. Each laser only does 12% damage.
            randomLaserCharge += Main.rand.NextFloat(0.09f, 0.14f);
            if (randomLaserCharge >= 1f)
            {
                randomLaserCharge -= 1f;
                Vector2 velocity = CIUtils.RandomVelocity(10f, 7f, 10f);

                int laserDamage = (int)(Projectile.damage * 0.12D);
                Projectile laser = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ProjectileType<NebulaShot>(), laserDamage, 0f, Projectile.owner);
                if (laser.whoAmI.InBounds(Main.maxProjectiles))
                {
                    laser.DamageType = RogueDamage.Instance;
                    laser.aiStyle = Main.rand.NextBool() ? ProjAIStyleID.Arrow : -1;
                    laser.penetrate = -1;
                    laser.usesLocalNPCImmunity = true;

                    // This projectile has a hefty amount of extra updates, which will influence the hit cooldown.
                    laser.localNPCHitCooldown = 120;
                }
            }
        }
        public override void ExOnHit(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIGodSlayerInferno>(), 180);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor);
            return false;
        }
    }
}
