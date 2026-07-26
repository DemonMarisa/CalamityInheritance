using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles.HeldProj;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityInheritance.Content.Projectiles.Typeless;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Melee.Shortsword
{
    public class EutrophicShankProj : CIBaseShortswordProjectile
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<EutrophicShank>();
        public override string Texture => GetInstance<EutrophicShank>().Texture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NoMeleeSpeedVelocityScaling[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(22);
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = GetInstance<TrueMelee>(); ;
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 1;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void SetVisualOffsets()
        {
            const int HalfSpriteWidth = 36 / 2;
            const int HalfSpriteHeight = 44 / 2;

            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;

            DrawOriginOffsetX = 0;
            DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
            DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
        }
        public override void ExtraBehavior()
        {
            if (Main.rand.NextBool(3))
            {
                int UnusedWhiteBluePurple = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.UnusedWhiteBluePurple, Projectile.direction * 4, 0f, 15, default, 1.3f);
            }
        }
        public override Action<Projectile> EffectBeforePullback => (proj) =>
        {
            int numberOfProjectiles = 1;
            float spreadAngle = MathHelper.ToRadians(60f);
            float baseAngle = Projectile.velocity.ToRotation();

            for (int i = 0; i < numberOfProjectiles; i++)
            {
                float randomAngle = baseAngle + Main.rand.NextFloat(-spreadAngle / 2, spreadAngle / 2);
                Vector2 randomDirection = new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle));

                Projectile newProjectileId = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, velocity: randomDirection * 3f, ProjectileType<ElectricSpark>(), Projectile.damage * 1, Projectile.knockBack, Projectile.owner, 0f, 0f);
                newProjectileId.DamageType = DamageClass.Melee;
            }
            ;
        };
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var source = Projectile.GetSource_FromThis();
            Projectile newProjectileId = Projectile.NewProjectileDirect(source, target.Center, Vector2.Zero, ProjectileType<ElectricSpark>(), Projectile.damage, Projectile.knockBack);
            newProjectileId.DamageType = DamageClass.Melee;
        }
    }
}
