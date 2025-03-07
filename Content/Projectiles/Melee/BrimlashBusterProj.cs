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
            Projectile.extraUpdates = 2;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            //这武器除了贴图和攻击模板本身，可能跟之前也不是一个东西了
            //不过一把武器除了这两个以外，你也很难从其他地方看出不同点

            Projectile.localAI[0] += 1f; //依旧是计时器的自增
            Projectile.velocity *= 0.98f; //速度将会不断缩减
            if(Projectile.localAI[0] > 20f)
            {
                Projectile.Kill();//干掉这个弹幕
            }
            
        }
        //飞行时的轨迹粒子
        public static void TrailDust()
        {

        }
        public override void OnKill(int timeLeft)
        {
            //我们需要他往弹幕速度方向一个扇形区域分裂出多个射弹
            //直接开始生成射弹
            for(int i = 0; i < 3; i++)
            {
                float rotDir = Main.rand.NextFloat(1f, 120f); //随机取值用于控制射弹的发射方向
                float rotArg = 360f/rotDir; //定好转角
                float rotate = MathHelper.ToRadians(rotArg);
                //新建射弹的速度，因为i已经通过改变转角的方式来改变发射方向，我们只需要一个水平向量即可
                Vector2 getSpeed = new Vector2(Projectile.velocity.X + 12f, 0f).RotatedBy(rotate);
                
            }
        }
    }
}