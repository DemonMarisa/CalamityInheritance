using CalamityMod;
using CalamityMod.Projectiles.BaseProjectiles;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;

namespace CalamityInheritance.Content.Projectiles.Melee.Shortsword
{
    public class SporeKnifeProj : BaseShortswordProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => GetInstance<SporeKnife>().Texture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NoMeleeSpeedVelocityScaling[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(14);
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = GetInstance<TrueMeleeDamageClass>(); ;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 1;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void SetVisualOffsets()
        {
            const int HalfSpriteWidth = 28 / 2;
            const int HalfSpriteHeight = 28 / 2;

            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;

            DrawOriginOffsetX = 0;
            DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
            DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
        }
        public override void ExtraBehavior()
        {
            if (Main.rand.NextBool(5))
            {
                int Grass = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Grass, Projectile.direction * 2, 0f, 15, default, 1.3f);
            }
        }
        public override Action<Projectile> EffectBeforePullback => (proj) =>
        {
            CalamityInheritancePlayer modPlayer = Main.player[Projectile.owner].GetModPlayer<CalamityInheritancePlayer>();
            modPlayer.ProjectilHitCounter2++;
            if (modPlayer.ProjectilHitCounter2 > 1)
            {
                int numberOfProjectiles = Main.rand.Next(2, 4);
                float spreadAngle = MathHelper.ToRadians(Main.rand.Next(10, 30));
                float baseAngle = Projectile.velocity.ToRotation();

                for (int i = 0; i < numberOfProjectiles; i++)
                {
                    float randomAngle = baseAngle + Main.rand.NextFloat(-spreadAngle / 2, spreadAngle / 2);
                    Vector2 randomDirection = new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle));

                    int newProjectileId = Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.Center, randomDirection * 6f,ProjectileID.Leaf, Projectile.damage * 1, Projectile.knockBack,Projectile.owner);
                    Main.projectile[newProjectileId].DamageType = DamageClass.Melee;
                }
                modPlayer.ProjectilHitCounter2 = 0;
            }
        };
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            CalamityInheritancePlayer modPlayer = Main.player[Projectile.owner].GetModPlayer<CalamityInheritancePlayer>();
            var source = Projectile.GetSource_FromThis();
            modPlayer.ProjectilHitCounter++;
            if (modPlayer.ProjectilHitCounter >= 5)
            {
                int[] projectileTypes = { ProjectileID.SporeGas, ProjectileID.SporeGas2, ProjectileID.SporeGas3 };
                float baseAngleIncrement = 2 * MathHelper.Pi / 8;
                float randomAngleOffset = (float)(Main.rand.NextDouble() * MathHelper.Pi / 4 - MathHelper.Pi / 8);
                for (int i = 0; i < 8; i++)
                {
                    float angle = i * baseAngleIncrement + randomAngleOffset;
                    Vector2 direction = new((float)Math.Cos(angle), (float)Math.Sin(angle));
                    int randomProjectileType = projectileTypes[Main.rand.Next(projectileTypes.Length)];
                    float randomSpeed = Main.rand.NextFloat(1f, 2.5f);
                    Projectile.NewProjectile(source, target.Center, direction * randomSpeed, randomProjectileType, Projectile.damage * 1, Projectile.knockBack);
                }
                target.AddBuff(BuffID.Poisoned, 120);
                modPlayer.ProjectilHitCounter = 0;
            }
        }

    }
}
