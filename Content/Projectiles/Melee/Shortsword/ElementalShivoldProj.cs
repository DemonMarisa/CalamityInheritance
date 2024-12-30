using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.Projectiles.Melee;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace CalamityInheritance.Content.Projectiles.Melee.Shortsword
{
    public class ElementalShivoldProj : BaseShortswordProjectile
    {
        public override string Texture => "CalamityMod/Items/Weapons/Melee/ElementalShiv";

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(22);
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 1;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
        }

        public override Action<Projectile> EffectBeforePullback => (proj) =>
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 4f, ModContent.ProjectileType<ElementBallShivold>(), (int)(Projectile.damage * 0.5), Projectile.knockBack, Projectile.owner, 0f, 0f);
        };

        public override void SetVisualOffsets()
        {
            const int HalfSpriteWidth = 44 / 2;
            const int HalfSpriteHeight = 44 / 2;

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
                int num250 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.RainbowTorch, (float)(Projectile.direction * 2), 0f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1.3f);
                Main.dust[num250].velocity *= 0.2f;
                Main.dust[num250].noGravity = true;
            }
        }
    }
}
