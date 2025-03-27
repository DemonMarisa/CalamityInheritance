using CalamityInheritance.Content.Items;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class BrimlashBusterProj: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40; 
            Projectile.friendly = true;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            //这武器除了贴图和攻击模板本身，可能跟之前也不是一个东西了
            //不过一把武器除了这两个以外，你也很难从其他地方看出不同点
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            Projectile.velocity *= 0.96f; //速度将会不断缩减
            TrailDust();
            if(Projectile.timeLeft == 200)
            {
                for(int i = 0; i < 3; i++)
                {
                    float rotArg = 120f/i; //定好转角
                    float rotate = MathHelper.ToRadians(rotArg);
                    //新建射弹的速度，因为i已经通过改变转角的方式来改变发射方向，我们只需要一个水平向量即可
                    Vector2 getPosition = new Vector2(Projectile.velocity.X + 12f, 0f).RotatedBy(rotate);
                    Vector2 getSpeed = new Vector2(Projectile.velocity.X + 12f, 0f).RotatedBy(rotate);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), getPosition, getSpeed, ModContent.ProjectileType<BrimlashBusterProjClone>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
            }
            if (Projectile.timeLeft < 150)
                Projectile.Kill();//干掉这个弹幕
            
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
                int d = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height + 5, CIDustID.DustBlood, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 200);
                Main.dust[d].noGravity = true;
                Main.dust[d].scale *= 1.5f;
            }
        }
    }
}