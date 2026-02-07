using CalamityInheritance.Content.Items;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Spear
{
    public class MarniteThrowSpearProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 25;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if (Projectile.spriteDirection == -1)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            else
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4 * 3;
            Projectile.ai[0] += 1f;
            //固定飞行一段距离后才会受重力影响
            if (Projectile.ai[0] > 75f)
            {
                Projectile.velocity.Y += 0.09f;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //这里“只”允许投掷的投矛造成下面的效果
            OnThrowEffect(target, hit);
            OnThrowDust();
        }
        public override void OnKill(int timeLeft)
        {
            OnThrowDust();
        }
        public void OnThrowEffect(NPC target, NPC.HitInfo hit)
        {
            //将投矛“击中”前的速度存储进去
            Vector2 getVel = Vector2.Normalize(Projectile.oldVelocity);
            //现在给对方单位“强制”击退，或者说强行禁锢。这个是故意的做法
            if (target.lifeMax < 20000)
            {
                if (target.IsABoss())
                {
                    //如果敌对单位是一个boss，则给予的击退力量会更少
                    if (Main.rand.NextBool(8))
                        target.velocity = getVel * 0.45f;
                }
                else
                {
                    //否则，强制击退一段距离
                    target.velocity = getVel * 1.4f;
                }
            }
        }
        //只允许投矛状态获得击中粒子/与音效
        public void OnThrowDust()
        {
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, CIDustID.DustMeteor, Projectile.oldVelocity.X * 0.75f, Projectile.oldVelocity.Y * 0.75f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, CIDustID.DustFrostDagger, Projectile.oldVelocity.X * 0.75f, Projectile.oldVelocity.Y * 0.75f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Request<Texture2D>(Texture).Value;
            float offset = 0;
            if (Projectile.spriteDirection == -1)
                offset = MathHelper.PiOver4;
            else
                offset = MathHelper.PiOver4 * 3;
            Vector2 offset2 = new Vector2(-texture.Width / 2, 0).RotatedBy(Projectile.rotation - offset);
            Projectile.BaseProjPreDraw(texture, lightColor, 0, 1, offset2);
            return false;
        }
    }
}
