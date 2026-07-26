using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles.HeldProj;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Melee.Shortsword
{
    public class AncientShivProj : CIBaseShortswordProjectile
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<AncientShiv>();
        public override string Texture => GetInstance<AncientShiv>().Texture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NoMeleeSpeedVelocityScaling[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(15);
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
        public override void SetVisualOffsets()
        {
            const int HalfSpriteWidth = 30 / 2;
            const int HalfSpriteHeight = 30 / 2;

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
                int MagicMirror = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.MagicMirror, Projectile.direction * 2, 0f, 15, default, 1.3f);
            }
        }
    }
}
