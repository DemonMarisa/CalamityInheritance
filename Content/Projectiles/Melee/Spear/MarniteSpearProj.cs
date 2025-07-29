using System;
using System.Numerics;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.Content.Projectiles.Melee.Spear
{
    public class MarniteSpearProj : ModProjectile, ILocalizedModType
    {
        protected virtual float RangeMin => 24f;
        protected virtual float RangeMax => 96f;
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 25;
            Projectile.aiStyle = ProjAIStyleID.Spear;    
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Projectile.friendly = true;
            Projectile.timeLeft = 90;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public void SpearAI()
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
                progression = (dura - Projectile.timeLeft) / halfDura;

            //让矛开始移动
            Projectile.Center = owner.MountedCenter + Vector2.SmoothStep(Projectile.velocity * RangeMin, Projectile.velocity * RangeMax, progression);

            //给猫一个正确的转角
            if (Projectile.spriteDirection == -1)
                //贴图朝左，转45°
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
            else
                //贴图朝右，转135°
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);

            // 避免粒子生成在服务器生成
            if (!Main.dedServ)
            {
                if (Main.rand.NextBool(3))
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, CIDustID.DustFrostDagger, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f, 128, default, 1.2f);
                if (Main.rand.NextBool(4))
                    Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, CIDustID.DustFrostDagger, Projectile.velocity.X * 2f, Projectile.velocity.Y * 2f, 128, default, 0.3f);
            }
        }
        public override void AI()
        {
            SpearAI();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //这里“只”允许投掷的投矛造成下面的效果
            OnThrowDust();
        }
        //只允许投矛状态获得击中粒子/与音效
        public void OnThrowDust()
        {
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height,CIDustID.DustMeteor, Projectile.oldVelocity.X * 0.75f, Projectile.oldVelocity.Y * 0.75f);
                Dust.NewDust(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height,CIDustID.DustFrostDagger, Projectile.oldVelocity.X * 0.75f, Projectile.oldVelocity.Y * 0.75f);
            }
        }
    }
}