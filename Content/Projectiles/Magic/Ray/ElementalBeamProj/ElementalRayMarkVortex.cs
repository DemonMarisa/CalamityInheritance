using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using MonoMod.Core.Utils;
using Terraria.Audio;
using Terraria.ID;
using CalamityMod.Projectiles.Magic;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray.ElementalBeamProj
{
    public class ElementalRayMarkVortex : ModProjectile
    {
        public new string LocalizationCategory => "Projectiles.Magic";
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            int shardType = ModContent.ProjectileType<ElementalOrbVortex>();
            int shardDamage = (int)(Projectile.damage * 0.5);
            if (Projectile.timeLeft <= 1)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, shardType, shardDamage, Projectile.knockBack, Projectile.owner);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 8;
        }
    }
}
