using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Utilities;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.Melee.Spear
{
    public class VictideSpearProj: ModProjectile, ILocalizedModType
    {
        protected virtual float RangeMin => 24f;
        protected virtual float RangeMax => 96f;
        public new string LocalizationCategory => "Content.Projectiles.Melee"; 
        public override void SetDefaults()
        {
            Projectile.width = 56;  
            Projectile.aiStyle = 19;
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Projectile.timeLeft = 90;
            Projectile.height = 56;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
        }
        public override bool PreAI()
        {
            if (!Projectile.CalamityInheritance().ThrownMode)
            {
                Player owner = Main.player[Projectile.owner];
                int dura = owner.itemAnimationMax;
                owner.heldProj = Projectile.whoAmI;

                //必要时刻重置生命
                if (Projectile.timeLeft > dura)
                    Projectile.timeLeft = dura;

                Projectile.velocity = Vector2.Normalize(Projectile.velocity);

                float halfDura = dura * 0.5f;
                float progression;

                if (Projectile.timeLeft < halfDura)
                    progression = Projectile.timeLeft / halfDura;
                else
                    progression = (dura - Projectile.timeLeft ) / halfDura;
                
                //让矛开始移动
                Projectile.Center = owner.MountedCenter + Vector2.SmoothStep(Projectile.velocity * RangeMin, Projectile.velocity * RangeMax, progression);
                //给猫一个正确的转角
                if (Projectile.spriteDirection == -1)
                    //贴图朝左，转45°
                    Projectile.rotation += MathHelper.ToRadians(45f);
                else
                    //贴图朝右，转135°
                    Projectile.rotation += MathHelper.ToRadians(135f);

                // 避免粒子生成在服务器生成
                if (!Main.dedServ)
                    FlyingDust();
                return false;
            }
            return true;
        }
        public override void AI()
        {
            if (Projectile.CalamityInheritance().ThrownMode)
            {
                //干掉原本的属性
                ResetProj();
                //矛不会受到重力影响，但是也该有飞行的粒子
                FlyingDust();
            }
        }

        public void FlyingDust()
        {
            if (Main.rand.NextBool(3))
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, CIDustID.DustWater, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f, 128, default, 1.2f);
            if (Main.rand.NextBool(4))
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, CIDustID.DustWaterCandle, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f, 128, default, 0.3f);
        }
        public void ResetProj()
        {
            //刷新投矛碰撞箱避免撞墙
            Projectile.width = Projectile.height = 28;
            //如果执行投掷的AI则刷新一次穿透次数(其实就是只能打一次)
            Projectile.penetrate = 1;
            //考虑到胜朝矛的特殊性，给予一个较短点的生命刻会更合适
            Projectile.timeLeft = 180;
            //取消矛的ai，即aistyle变成自定义
            Projectile.aiStyle = 0;
            Projectile.extraUpdates = 2;
            //干掉穿墙属性，我也不知道为什么他能穿墙了
            Projectile.tileCollide = true;
        }
        public override void OnKill(int timeLeft)
        {
            //只有投掷模式下被任意形式干掉才会生成粒子
            if (Projectile.CalamityInheritance().ThrownMode)
            {
                CIFunction.DustCircle(Projectile.position, 24, 2f, CIDustID.DustWater, true, 12f);
                //而后，生成水环
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, new Vector2(Main.rand.NextFloat(-0.4f, 0.5f), Main.rand.NextFloat(-3,-6)), ModContent.ProjectileType<VictideSpearWaterRing>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Projectile.netUpdate = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!Projectile.CalamityInheritance().ThrownMode)
                //注：只允许长矛本身击中敌人的时候发射一个弹幕. 或者说别的，我也不知道这个球干嘛的。
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Projectile.velocity, ModContent.ProjectileType<VictideSpearBall>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
            target.AddBuff(BuffID.Poisoned, 300);
        }
    }
}
