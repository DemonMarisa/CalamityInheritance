using System;
using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Utilities;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class ACTExcelsusMain : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        #region 基础属性
        //阶段Timer
        public int Timer = 0;
        //发起追踪的Timer
        public int TimerAlt = 0;
        public const int IdleTimer = 30;
        //非追踪状态下的旋转
        public const float NonHomingRotation = 0.45f;
        
        #endregion
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 300;
            Projectile.alpha = 100;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Timer++;
            if (Timer > ACTExcelsus.HomingTimer)
                DoHoming();
            else
                DoFlying();

            //保留特效
            if (Main.rand.NextBool(8))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueFairy, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
            if (Main.rand.NextBool(8))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, Main.rand.NextBool(3) ? 56 : 242, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.tileCollide = false;
            if (Projectile.timeLeft > 85)
            {
                Projectile.timeLeft = 85;
            }
            return false;
        }
        public void DoFlying()
        {
            //飞行过程中射弹会一直保持高速旋转, 除此之外就……就不干什么了。
            Projectile.rotation += ACTExcelsus.NonHomingRotation;
        }
        //追踪逻辑
        public void DoHoming()
        {
            //首先搜索附近的NPC实例
            NPC tar = CIFunction.FindClosestTarget(Projectile, 3200f, true);
            //需注意的是，AI执行的这段时间内也会一直检索目标。
            //如果实例存在，将刀片指向这个实例
            if (tar != null)
            {
                //刷新射弹生命与判定次数
                Projectile.timeLeft = 300;
                Projectile.penetrate = 1;
                Vector2 targetPos = tar.Center;
                //原地减速，指向这个敌怪
                float rot = Projectile.AngleTo(targetPos) + MathHelper.PiOver4;
                Projectile.rotation = Utils.AngleLerp(Projectile.rotation, rot, 0.2f);
                Projectile.velocity *= 0.9f;
                //而后在一段时间后发起追踪
                TimerAlt ++;
                if (TimerAlt > ACTExcelsus.IdleTimer)
                {
                    //给多一个额外更新
                    Projectile.extraUpdates += 1;
                    CIFunction.HomeInOnNPC(Projectile, true, 1800f, ACTExcelsus.HomingSpeed, 20f);
                }
            }
            //自定义：其余状态下单独让这个刀片指向指针, 并且在原地进行待机。
            else
            {
                float rot = Projectile.AngleTo(Main.MouseWorld) + MathHelper.PiOver4;
                Projectile.rotation = Utils.AngleLerp(Projectile.rotation, rot, 0.2f);
                Projectile.velocity *= 0.9f;
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * (b2 / 255f));
                return new Color(b2, b2, b2, a2);
            }
            return default(Color);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Color color;
            if (Projectile.timeLeft < 85)
            {
                byte b2 = (byte)(Projectile.timeLeft * 3);
                byte a2 = (byte)(100f * (b2 / 255f));
                color = new Color(b2, b2, b2, a2);
            }
            else
            {
                color = new Color(255, 255, 255, 100);
            }
            Vector2 origin = new Vector2(39f, 46f);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>($"{GenericProjRoute.ProjRoute}/Melee/ACTExcelsusMainGlow").Value, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.timeLeft > 85)
            {
                Projectile.timeLeft = 85;
            }
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 180);
        }
    }
}
