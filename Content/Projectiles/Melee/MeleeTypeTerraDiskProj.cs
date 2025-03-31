using System;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Weapons;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    [LegacyName("TerraDiskProjectileLegacyMelee")]
    public class MeleeTypeTerraDiskProj: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => $"{Generic.WeaponRoute}/Melee/MeleeTypeTerraDisk";

        private bool initialized = false;
        private int Lifetime = 180;
        private int ReboundTime = 45;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 46;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
        }

        public override void AI()
        {
            Initialize();
            BoomerangAI();
            SpawnProjectilesNearEnemies();
            LightingandDust();
        }

        private void Initialize()
        {
            if (initialized)
                return;

            Lifetime =  180;
            ReboundTime =  45;
            Projectile.timeLeft = Lifetime;
            initialized = true;
        }

        private void BoomerangAI()
        {

            Projectile.rotation += 0.4f * Projectile.direction;

            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(CISoundID.SoundBoomerangs, Projectile.position);
            }

            if (Projectile.timeLeft < Lifetime - ReboundTime)
                Projectile.ai[0] = 1f;

            if (Projectile.ai[0] == 1f)
            {

                Projectile.tileCollide = false;
                Projectile.extraUpdates = 1;
                Player player = Main.player[Projectile.owner];
                float returnSpeed = 16f;
                float acceleration = 1.4f;
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

            const float maxDistance = 300f;
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
                if (Main.player[Projectile.owner].miscCounter % 50 == 0)
                {
                    int splitProj = ModContent.ProjectileType<MeleeTypeTerraDiskProjSplit>();
                    int damageOutPut = Projectile.damage / 2;
                    if (Projectile.owner == Main.myPlayer)
                    {
                        float spread = 60f * 0.0174f;
                        double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                        double deltaAngle = spread / 6f;
                        for (int i = 0; i < 6; i++)
                        {
                            Vector2 velocity = ((MathHelper.TwoPi * i / 6f) - MathHelper.PiOver2).ToRotationVector2() * 6f;
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(),
                                                     Projectile.Center,
                                                     velocity,
                                                     splitProj,
                                                     damageOutPut,
                                                     Projectile.knockBack * 0.5f,
                                                     Projectile.owner);
                        }
                    }
                }
            }
        }

        private void LightingandDust()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0.75f, 0f);
            if (!Main.rand.NextBool(5))
                return;
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.TerraBlade, Projectile.velocity.X, Projectile.velocity.Y);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[1]++;
        }

        // Make it bounce on tiles.
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // Impacts the terrain even though it bounces off.
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);

            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            Projectile.ai[0] = 1f;
            return false;
        }
    }
}
