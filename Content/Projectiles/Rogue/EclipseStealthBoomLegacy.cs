using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class EclipseStealthBoomLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";

        public const int Lifetime = 21; // 7 animation frames, 12 FPS

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 102;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = RogueDamageClass.Instance;
            Projectile.penetrate = -1;
            Projectile.timeLeft = Lifetime;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.scale = 0.75f;
        }
        
        public override void AI()
        {
            Projectile.frameCounter++;
            Projectile.frame = Projectile.frameCounter / 3;

            // 第一帧随机旋转
            if (Projectile.ai[0] == 0)
            {
                Projectile.rotation += Main.rand.NextFloat(0f, MathHelper.TwoPi);
                Projectile.ai[0] = 1;
            }


            if (Projectile.frameCounter > Lifetime)
                Projectile.Kill();
        }
    }
}
