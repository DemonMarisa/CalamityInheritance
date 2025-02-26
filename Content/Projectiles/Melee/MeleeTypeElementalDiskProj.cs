using System;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class MeleeTypeElementalDiskProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Melee/MeleeTypeElementalDisk";
        private readonly int Lifetime = 400;
        private readonly int ReboundTime = 30;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 56;
            Projectile.height = 56;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
            Projectile.penetrate = -1;
            Projectile.timeLeft = Lifetime;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
        }

        public override void AI()
        {
            SpawnProjectilesNearEnemies();
            BoomerangAI();
            LightingAndDust();
        }

        private void BoomerangAI()
        {
            // Boomerang rotation
            Projectile.rotation += 0.4f * Projectile.direction;

            // Boomerang sound
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }

            // Returns after some number of frames in the air
            int timeMult =  1;
            if (Projectile.timeLeft < Lifetime * timeMult - ReboundTime * timeMult)
                Projectile.ai[0] = 1f;

            if (Projectile.ai[0] == 1f)
            {
                Player player = Main.player[Projectile.owner];
                float returnSpeed = 14f;
                float acceleration = 0.6f;
                CIFunction.BoomerangReturningAI(player, Projectile, returnSpeed, acceleration);
                // Delete the projectile if it touches its owner.
                if (Main.myPlayer == Projectile.owner)
                    if (Projectile.Hitbox.Intersects(player.Hitbox))
                        Projectile.Kill();
            }
        }

        private void SpawnProjectilesNearEnemies()
        {
            if (!Projectile.friendly)
                return;

            float maxDistance = 300f;
            bool homeIn = false;

            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (npc.CanBeChasedBy(Projectile, false))
                {
                    float extraDistance = npc.width / 2 + npc.height / 2;

                    bool canHit = true;
                    if (extraDistance < maxDistance)
                        canHit = Collision.CanHit(Projectile.Center, 1, 1, npc.Center, 1, 1);

                    if (Vector2.Distance(npc.Center, Projectile.Center) < maxDistance + extraDistance && canHit)
                    {
                        homeIn = true;
                        break;
                    }
                }
            }

            if (homeIn)
            {
                int counter = 30; //分裂数量更多、分裂的CD更短
                if (Main.player[Projectile.owner].miscCounter % counter == 0)
                {
                    int splitProj = ModContent.ProjectileType<MeleeTypeElementalDiskProjSplit>();
                    if (Projectile.owner == Main.myPlayer && Main.player[Projectile.owner].ownedProjectileCounts[splitProj] < 9999)
                    {
                        float spread = 45f * 0.0174f;
                        double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                        double deltaAngle = spread / 8f;
                        double offsetAngle;
                        for (int i = 0; i < 8; i++)
                        {
                            offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X,
                                                     Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 5f),
                                                     (float)(Math.Cos(offsetAngle) * 5f), splitProj,
                                                     Projectile.damage / 2, Projectile.knockBack,
                                                     Projectile.owner);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X,
                                                     Projectile.Center.Y,
                                                     (float)(-Math.Sin(offsetAngle) * 5f),
                                                     (float)(-Math.Cos(offsetAngle) * 5f), splitProj,
                                                     Projectile.damage / 2, Projectile.knockBack,
                                                     Projectile.owner);
                        }
                    }
                }
            }
        }

        private void LightingAndDust()
        {
            if (Main.rand.NextBool(3))
            {
                int rainbow = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowTorch, Projectile.direction * 2, 0f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1.3f);
                Main.dust[rainbow].noGravity = true;
                Main.dust[rainbow].velocity *= 0f;
            }

            Lighting.AddLight(Projectile.Center, 0.15f, 1f, 0.25f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GlacialState>(), 180);
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 180);
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 180);
            target.AddBuff(ModContent.BuffType<Plague>(), 180);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {

        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }
    }
}
