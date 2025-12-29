using CalamityInheritance.Content.Items.Weapons.Melee.Yoyos;
using CalamityInheritance.Content.Projectiles.Typeless.NorProj;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Yoyos
{
    public class ObliteratorYoyoLegacy : ModProjectile
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<TheObliteratorLegacy>();
        private const int FramesPerShot = 5;
        public int time = 0;
        private int extraUpdateCounter = 0;
        private const int UpdatesPerFrame = 3;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = -1f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 720f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 54f / UpdatesPerFrame;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = ProjAIStyleID.Yoyo;
            Projectile.width = Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = UpdatesPerFrame;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6 * UpdatesPerFrame;
        }
        public override void AI()
        {
            time++;
            if ((Projectile.position - Main.player[Projectile.owner].position).Length() > 3200f) //200 blocks
                Projectile.Kill();
            extraUpdateCounter = (extraUpdateCounter + 1) % UpdatesPerFrame;
            if (extraUpdateCounter != UpdatesPerFrame - 1)
                return; 
            Lighting.AddLight(Projectile.Center, 0.8f, 0.3f, 1f);

            Projectile.localAI[1]++;
            if (Projectile.localAI[1] >= 4 * FramesPerShot)
                Projectile.localAI[1] = 0f;

            // Attempt to fire a laser every 5 frames
            if (Projectile.localAI[1] % FramesPerShot == 0f)
            {
                List<int> targets = new List<int>();
                float laserRange = 300f;
                foreach (NPC n in Main.ActiveNPCs)
                {
                    if (n.CanBeChasedBy(Projectile, false) && (n.Center - Projectile.Center).Length() <= laserRange && Collision.CanHit(Projectile.Center, 1, 1, n.Center, 1, 1))
                    {
                        targets.Add(n.whoAmI);
                        // Bosses are added 5 times instead of 1 so that they are preferentially but not exclusively targeted.
                        if (n.boss)
                            for (int j = 0; j < 4; ++j)
                                targets.Add(n.whoAmI);
                    }
                }
                if (targets.Count == 0)
                    return;
                // Pick which of the four corners the laser is spawning in
                Vector2 laserSpawnPosition = Projectile.Center;
                Vector2 offset;
                if (Projectile.localAI[1] < FramesPerShot)
                    offset = new Vector2(4, 4);
                else if (Projectile.localAI[1] < 2 * FramesPerShot)
                    offset = new Vector2(-4, 4);
                else if (Projectile.localAI[1] < 3 * FramesPerShot)
                    offset = new Vector2(-4, -4);
                else
                    offset = new Vector2(4, -4);
                laserSpawnPosition += offset.RotatedBy(Projectile.rotation);

                ref NPC target = ref Main.npc[targets[Main.rand.Next(targets.Count)]];
                const float laserSpeed = 6f;
                Vector2 velocity = target.Center - Projectile.Center;
                velocity = velocity.SafeNormalize(Vector2.Zero) * laserSpeed;
                if (Projectile.owner == Main.myPlayer)
                {
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), laserSpawnPosition, velocity, ProjectileType<NebulaShotLegacy>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    if (proj.WithinBounds(Main.maxProjectiles))
                        Main.projectile[proj].DamageType = DamageClass.MeleeNoSpeed;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
        public override void OnKill(int timeLeft)
        {
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 180);
        }
    }
}
