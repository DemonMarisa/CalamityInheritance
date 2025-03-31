using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Audio;

namespace CalamityInheritance.Content.Projectiles.Melee.Spear
{
    //牢灾矛的基类是有动画问题的, 因此我舍去了使用牢灾的矛基类而是自己写了一个
    public class StreamGougeProjOld : ModProjectile, ILocalizedModType
    {
        protected virtual float RangeMin => 56f;
        protected virtual float RangeMax => 196f;
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = ProjAIStyleID.Spear;
            Projectile.timeLeft = 90;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
        }

        public override bool PreAI()
        {
            Player owner = Main.player[Projectile.owner];
            int dura = owner.itemAnimationMax;
            owner.heldProj = Projectile.whoAmI;

            //必要时刻重置生命
            if (Projectile.timeLeft > dura)
                Projectile.timeLeft = dura;

            Projectile.velocity = Vector2.Normalize(Projectile.velocity * 5);

            float halfDura = dura * 0.5f;
            float progression;

            if (Projectile.timeLeft < halfDura)
                progression = Projectile.timeLeft / halfDura;
            else
                progression = (dura - Projectile.timeLeft ) / halfDura;
            
            //让矛开始移动
            Projectile.Center = owner.MountedCenter + Vector2.SmoothStep(Projectile.velocity * RangeMin, Projectile.velocity * RangeMax, progression);
            if (Projectile.velocity == Projectile.velocity * RangeMax / 2)
                PortalDust();
            //给矛一个正确的转角
            if (Projectile.spriteDirection == -1)
                //贴图朝左，转45°
                Projectile.rotation += MathHelper.ToRadians(45f);
            else
                //贴图朝右，转135°
                Projectile.rotation += MathHelper.ToRadians(135f);
            if (Projectile.ai[0] == 0f)
            {
                //让矛刺出的第一帧发射弹幕，而非顶点发射
                ShootProj();
                Projectile.ai[0] = 1f;
            }

            
            //干掉AI钩子
            return false;
        }
        public void ShootProj()
        {
            int damage = (int)(Projectile.damage * 0.5f);
            float kb = Projectile.knockBack * 0.5f;
            Vector2 projPos = Projectile.Center + Projectile.velocity;
            Vector2 projVel = Projectile.velocity * 15f;
            if (Projectile.owner == Main.myPlayer)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), projPos, projVel, ModContent.ProjectileType<EssenceBeam>(), damage * 4, kb, Projectile.owner, 0f, 0f);

            SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);
            //粒子
            ExtraBehavior();
            //顶点处生成传送门粒子
            
        }
        public void PortalDust()
        {
            int circleDust = 18;
            Vector2 baseDustVel = new Vector2(3.8f, 0f);
            for (int i = 0; i < circleDust; ++i)
            {
                int dustID = 173;
                float angle = i * (MathHelper.TwoPi / circleDust);
                Vector2 dustVel = baseDustVel.RotatedBy(angle);

                int idx = Dust.NewDust(Projectile.Center, 1, 1, dustID);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].position = Projectile.Center;
                Main.dust[idx].velocity = dustVel;
                Main.dust[idx].scale = 2.4f;
            }
        }
        // public override void PostDraw(Color lightColor)
        // {
        //     Vector2 origin = new Vector2(0f, 0f);
        //     Main.EntitySpriteDraw(ModContent.Request<Texture2D>(${GenericProjRoute.ProjRoute}/Melee/Spear/StreamGougeProjOldGlowProj").Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, origin, 1f, SpriteEffects.None, 0f);
        // }
        public void ExtraBehavior()
        {
            int movingDust = 3;
            for (int i = 0; i < movingDust; ++i)
            {
                int dustID = 173;
                Vector2 corner = 0.5f * Projectile.position + 0.5f * Projectile.Center;
                int idx = Dust.NewDust(corner, Projectile.width / 2, Projectile.height / 2, dustID);

                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity = Vector2.Zero;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 300);
        }
    }
}
