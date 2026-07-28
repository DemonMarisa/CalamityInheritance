using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles.HeldProj;
using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityInheritance.Content.Projectiles.Typeless.General;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Melee.Shortsword
{
    public class AquaticDischargeProj : CIBaseShortswordProjectile
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<AquaticDischarge>();
        public override string Texture => GetInstance<AquaticDischarge>().Texture;
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(16);
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = GetInstance<TrueMelee>();
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = 360;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
        }

        public override void SetVisualOffsets()
        {
            const int HalfSpriteWidth = 32 / 2;
            const int HalfSpriteHeight = 32 / 2;

            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;

            DrawOriginOffsetX = 0;
            DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
            DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
        }

        public override void ExtraBehavior()
        {
            if (Main.rand.NextBool(5))
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Electric);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var source = Projectile.GetSource_FromThis();
            Projectile.NewProjectile(source, target.Center, Vector2.Zero, ProjectileType<ElectricSpark>(), Projectile.damage, hit.Knockback, Main.myPlayer);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Player player = Main.player[Projectile.owner];
            var source = Projectile.GetSource_FromThis();
            Projectile.NewProjectile(source, target.Center, Vector2.Zero, ProjectileType<ElectricSpark>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
        }
    }
}
