using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles.HeldProj;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace CalamityInheritance.Content.Projectiles.Melee.Shortsword
{
    public class CosmicShivProjold : CIBaseShortswordProjectile
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<CosmicShivold>();
        public override string Texture => GetInstance<CosmicShivold>().Texture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NoMeleeSpeedVelocityScaling[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(24);
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = GetInstance<TrueMelee>();
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 1;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override Action<Projectile> EffectBeforePullback => (proj) =>
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 6f, ProjectileType<CosmicShivBallold>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
        };

        public override void SetVisualOffsets()
        {
            const int HalfSpriteWidth = 48 / 2;
            const int HalfSpriteHeight = 48 / 2;

            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;

            DrawOriginOffsetX = 0;
            DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
            DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
        }

        public override void ExtraBehavior()
        {
            Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.ShadowbeamStaff);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int k = 0; k < 36; k++)
            {
                int dustID = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height - 16, DustID.ShadowbeamStaff, 0f, 0f, 0, default, 1f);
                Main.dust[dustID].velocity *= 3f;
                Main.dust[dustID].scale *= 2f;
            }
            target.AddBuff(BuffType<CIGodSlayerInferno>(), 120);
        }
    }
}
