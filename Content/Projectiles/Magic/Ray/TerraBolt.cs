using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Dusts;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray
{
    public class TerraBolt : ModProjectile
    {
        public new string LocalizationCategory => "Projectiles.Magic";
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bolt");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.extraUpdates = 100;
            Projectile.friendly = true;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
            Projectile.localAI[1] += 1f;
            if (Projectile.localAI[1] >= 29f && Projectile.owner == Main.myPlayer)
            {
                Projectile.localAI[1] = 0f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<TerraOrb2>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner, 0f, 0f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.immune[Projectile.owner] = 8;
        }
    }
}
