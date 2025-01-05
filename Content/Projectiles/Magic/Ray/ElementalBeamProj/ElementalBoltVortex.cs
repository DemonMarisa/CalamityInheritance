using CalamityMod;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic.Ray.ElementalBeamProj
{
    public class ElementalBoltVortex : ModProjectile
    {
        public new string LocalizationCategory => "Projectiles.Magic";
        public override string Texture => "CalamityInheritance/Content/Projectiles/InvisibleProj";

        public const int Lifetime = 150;
        public ref float Time => ref Projectile.ai[0];
        public ref float ShardCooldown => ref Projectile.ai[1];
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
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            var source = Projectile.GetSource_FromThis();
            CalamityUtils.ProjectileRain(source, Projectile.Center, 380f, 0f, 600f, 800f, 6f, ModContent.ProjectileType<ElementalBolt>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }
    }
}
