using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Weapons.RogueMelee;
using LAP.Core.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.RogueMelee
{
    public class TerraDiskProj : CIMeleeRogueProj
    {
        public override string Texture => GetInstance<TerraDisk>().Texture;
        private int Lifetime = 240;
        private int ReboundTime = 65;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.timeLeft = Lifetime;
            Projectile.width = Projectile.height = 46;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.penetrate = -1;
            ModItem item = Projectile.Owner()?.HeldItem?.ModItem;
            if (item is not null && item is CIMeleeRogue rogue && rogue.IsMelee)
                Projectile.DamageType = DamageClass.Melee;
            else
                Projectile.DamageType = RogueDamage.Instance;
        }

        public override void AI()
        {
            BoomerangAI();
            SpawnFollowProj();
            LightingandDust();
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
            if (Projectile.timeLeft < Lifetime - ReboundTime)
            {
                Projectile.tileCollide = false;
                Projectile.extraUpdates = 1;
                Player owner = Main.player[Projectile.owner];
                Projectile.PredictHomeIn(12f, 0f, owner);
                if (Main.myPlayer == Projectile.owner)
                    if (Projectile.Hitbox.Intersects(owner.Hitbox))
                        Projectile.Kill();
            }
        }
        private void SpawnFollowProj()
        {
            if (Projectile.LAP().FirstFrame && Projectile.CI().Stealth)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    float spread = 360f * 0.0174f;
                    float startAngle = MathF.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                    float deltaAngle = spread / 6f;
                    for (int i = 0; i < 6; i++)
                    {
                        float angle = startAngle + deltaAngle * i;
                        Vector2 velocity = (MathHelper.TwoPi * i / 6f - MathHelper.PiOver2).ToRotationVector2() * 6f;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ProjectileType<TerraDiskProjSmall>(), Projectile.damage / 2, Projectile.knockBack * 0.5f, Projectile.owner, Projectile.whoAmI, angle);
                    }
                }
            }
            if (Projectile.owner == Main.myPlayer && Projectile.Owner().miscCounter % 50 == 0)
            {
                float spread = 360f * 0.0174f;
                float startAngle = MathF.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                float deltaAngle = spread / 6f;
                for (int i = 0; i < 6; i++)
                {
                    float angle = startAngle + deltaAngle * i;
                    Vector2 velocity = (MathHelper.TwoPi * i / 6f - MathHelper.PiOver2).ToRotationVector2() * 6f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ProjectileType<TerraDiskProjSmall_HomeIn>(), Projectile.damage / 2, Projectile.knockBack * 0.5f, Projectile.owner, Projectile.whoAmI, angle);
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
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }
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