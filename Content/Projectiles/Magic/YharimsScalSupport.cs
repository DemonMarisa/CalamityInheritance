using CalamityInheritance.Utilities;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class YharimsScalSupport : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        #region 别名
        public ref float AttackType => ref Projectile.ai[0];
        public ref float AttackTimer => ref Projectile.ai[1];
        public ref float AttackTarget => ref Projectile.ai[2];
        #endregion
        #region 攻击枚举
        const float IsShooted = 0f;
        const float IsIdle = 1f;
        const float IsHoming = 2f;
        const float IsFading = 3f;
        #endregion
        #region 属性
        const float GlobalNextActionTimer = 30f;
        #endregion
        public override void SetDefaults()
        {
            Projectile.width = 66;
            Projectile.height = 50;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.extraUpdates = 2;
            Projectile.localNPCHitCooldown = -1;
            Projectile.scale *= 0.75f;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.alpha = 0;
        }
        public override void AI()
        {
            //这里基本都是固定流程, 我们把这个target提取出来
            NPC target = Main.npc[(int)AttackTarget];
            if (target is null)
            {
                AttackType = IsFading;
                Projectile.netUpdate = true;
            }
            //执行不同的AI
            switch(AttackType)
            {
                case IsShooted:
                    DoShooted();
                    break;
                case IsIdle:
                    DoIdle(target);
                    break;
                case IsHoming:
                    DoHoming(target);
                    break;
                case IsFading:
                    DoFading();
                    break;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;

            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;
            return false;
        }
        public override bool? CanDamage() => AttackType == IsHoming;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 160;
            Projectile.position.X = Projectile.position.X - Projectile.width / 2;
            Projectile.position.Y = Projectile.position.Y - Projectile.height/ 2;
            OnHitDust();
            if (AttackType == IsHoming)
            {
                //击中敌人，播报这个声音
                SoundEngine.PlaySound(SoundID.DD2_KoboldFlyerHurt with {Volume = 0.7f, Pitch = 0.5f}, Projectile.Center);
                AttackType = IsFading;
                Projectile.netUpdate = true;
            }
        }

        
        #region 方法合集
        public void TrailLine()
        {
            Vector2 trailPos = Projectile.Center + Vector2.UnitY.RotatedBy(Projectile.rotation) * Main.rand.NextFloat(-16f, 16f);
            float trailScale = Main.rand.NextFloat(0.8f, 1.2f);
            Color trailColor = Color.DarkRed;
            Particle trail = new SparkParticle(trailPos, Projectile.velocity * 0.2f, false, 60, trailScale, trailColor);
            GeneralParticleHandler.SpawnParticle(trail);
        }
        private void OnHitDust()
        {
             for (int i = 0; i < 7; i++)
            {
                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemRuby, 0f, 0f, 100, Color.Red, 1.2f);
                Main.dust[d].velocity *= 3f;
                if (Main.rand.NextBool(2))
                {
                    Main.dust[d].scale = 0.5f;
                    Main.dust[d].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    Main.dust[d].color = Color.Red;
                }
            }
            for (int j = 0; j < 15; j++)
            {
                int d2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemRuby, 0f, 0f, 100, Color.Red, 1.7f);
                Main.dust[d2].noGravity = true;
                Main.dust[d2].velocity *= 5f;
                Main.dust[d2].color = Color.Red;
                d2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GemRuby, 0f, 0f, 100, Color.Red, 1f);
                Main.dust[d2].velocity *= 2f;
                Main.dust[d2].color = Color.Red;
            }
        }
        private void DoFading()
        {
            //这个是从隔壁海爵剑抄的
            Projectile.velocity *= 0.958f;
            if (Projectile.alpha < 255)
                Projectile.alpha += 5;
            else Projectile.Kill();
        }
        private void DoHoming(NPC target)
        {
            AttackTimer++;
            float acceleration = AttackTimer / 10f;
            if (acceleration > 10f)
                acceleration = 10f;
            Projectile.HomingNPCBetter(target, 1800f, 12f + acceleration, 20f, 1);
        }
        private void DoIdle(NPC target)
        {
            //转角。都是固定流程。
            float rot = Projectile.AngleTo(target.Center);
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, rot, 0.2f);
            AttackTimer += 1f;
            if (AttackTimer > GlobalNextActionTimer)
            {
                //发起追逐，播报声音
                Projectile.netUpdate = true;
                AttackType = IsHoming;
                AttackTimer = 0f;
            }
        }
        private void DoShooted()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            //刚发射出来的射弹会先缓慢减速
            Projectile.velocity *= 0.90f;
            //这个计时器会一直自增，直到符合条件为止
            AttackTimer += 1f;
            if (AttackTimer > 30f)
            {
                AttackType = IsIdle;
                Projectile.netUpdate = true;
                AttackTimer = 0f;
            }
            else
            {
                if (Main.rand.NextBool(5))
                TrailLine();
            }
        }
        #endregion
    }
}