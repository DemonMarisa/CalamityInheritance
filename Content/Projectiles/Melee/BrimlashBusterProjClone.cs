using CalamityInheritance.Content.Items;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class BrimlashBusterProjClone: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => "CalamityInheritance/Content/Projectiles/Melee/BrimlashBusterProj";
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40; 
            Projectile.extraUpdates = 1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            //刷新射弹属性 
            Projectile.penetrate = 1;
            //不管是哪个射弹，都会需要飞行一段时间    
            Projectile.ai[0] += 1f; //依旧是计时器的自增
            //飞行至一段时间后才允许追踪。
            if (Projectile.ai[0] > 30f)
                CIFunction.HomeInOnNPC(Projectile, false, 700f, 24f, 20f);
            TrailDust();
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;
            return true;
        }
        //飞行时的轨迹粒子
        public void TrailDust()
        {
            for (int i = 0; i < 5; i++)
            {
                int d = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, CIDustID.DustBlood, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 200);
                Main.dust[d].noGravity = true;
                Main.dust[d].scale *= 1.5f;
            }
        }
    }
}