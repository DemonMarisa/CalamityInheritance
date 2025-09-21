
using System;
using CalamityInheritance.ExtraTextures.Metaballs;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku.SwordArk
{
    public class ShizukuCross : ModProjectile,ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override string Texture => GenericProjRoute.InvisProjRoute;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 32;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.penetrate = 4;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }
        public override void AI()
        {
        }
        //这里用Metaball代替了绘制。
        public override bool PreDraw(ref Color lightColor)
        {
            //使用世界插值手动算速度。或者说别的，不过大概不需要了。
            ShizukuCrossMetaball.SpawnParticle(Projectile.Center, Projectile.velocity, 16);
            return false;
        }
    }
}