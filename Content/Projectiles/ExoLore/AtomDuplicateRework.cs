using System;
using System.IO;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoLore
{
    public class AtomDuplicateRework : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        #region 别名
        ref float AttackType => ref Projectile.ai[0];
        ref float AttackTimer => ref Projectile.ai[1];
        ref float TargetIndex => ref Projectile.ai[2];
        ref int Alpha => ref Projectile.alpha;
        public Player Owner => Main.player[Projectile.owner]; 
        #endregion
        #region 攻击枚举
        const float IsIdle = 0f;
        const float IsShooted = 1f;
        const float isHit = 2f;
        const float IsNoTarget = 3f;
        #endregion
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.scale = 0.66f;
            Projectile.width = Projectile.height = (int)(124f * Projectile.scale);
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            //反正下方会手动刷新的。
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            //干掉了timeLeft，我们会手动处死这个射弹，不用担心
            Projectile.extraUpdates = 1;
            Projectile.alpha = 0;
            Projectile.DamageType = GetInstance<RogueDamageClass>();
            Projectile.noEnchantmentVisuals = true;
        }
        public override void SendExtraAI(BinaryWriter writer) => Projectile.DoSyncHandlerWrite(ref writer);
        public override void ReceiveExtraAI(BinaryReader reader) => Projectile.DoSyncHandlerRead(ref reader);
        public override bool? CanDamage() => AttackType != IsIdle || Alpha < 200;
        public override void AI()
        {
            DoGeneral();
            //Target当前是否可用, 不可用我们才搜索别人
            NPC target = Main.npc[(int)TargetIndex];
            if (!target.active || !target.chaseable)
            {
                AttackType = isHit;
                Projectile.netUpdate = true;
            }
            switch (AttackType)
            {
                case IsIdle:
                    DoIdle(target);
                    break;
                case IsShooted:
                    DoShooted(target);
                    break;
                case isHit:
                    DoHit();
                    break;
                case IsNoTarget:
                    DoNoTarget();
                    break;

            }
        }
        public override Color? GetAlpha(Color lightColor) => base.GetAlpha(lightColor);
        
        public override bool PreDraw(ref Color lightColor)
        {
            if (AttackTimer <= 1f)
                return false;

            Color drawColor = CalamityUtils.MulticolorLerp((AttackTimer/ 35f + Projectile.identity / 4f) % 1f, CalamityUtils.ExoPalette);
            drawColor.A = 0;
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], drawColor);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //无论什么情况下击中敌人我们都直接标记isHit状态
            if (AttackType != isHit)
                AttackType = isHit;
            //音效。
            SoundStyle[] hitSound =
            [
                CISoundMenu.AtomHit1,
                CISoundMenu.AtomHit2,
                CISoundMenu.AtomHit3
            ];
            SoundEngine.PlaySound(Utils.SelectRandom(Main.rand, hitSound), Projectile.Center);
        }

        //搜索附近的敌怪即可。
        private void DoNoTarget() => CIFunction.HomeInOnNPC(Projectile, true, 3600f, 24f, 20f);
        private void DoHit()
        {
            //减速
            Projectile.velocity *= 0.97f;
            //减速的过程中降低alpha通道
            if (Projectile.alpha < 255)
                Projectile.alpha += 10;
            else
                //kill掉射弹
                Projectile.Kill();
        }
        private void DoShooted(NPC target)
        {
            //直接索敌。
            AttackTimer++;
            float acceleration = AttackTimer / 10f + 0.0056f;
            Projectile.HomingNPCBetter(target, 3600f, 20f + acceleration, 20f, 2);
        }

        private void DoIdle(NPC target)
        {
            //绘制会在下方的predraw进行。这里我们只管AI    
            //对准target
            float rot = Projectile.AngleTo(target.Center) + MathHelper.PiOver4;
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, rot, 0.2f);
            //一些特殊的东西：生成的时候需要全程校准射弹与敌怪的位置
            //满足条件，开始索敌
            AttackTimer++;
            if (AttackTimer > 45f)
            {
                AttackType = IsShooted;
                AttackTimer = 0f;
                Projectile.netUpdate = true; 
            }
        }

        private void DoGeneral()
        {
            //转角。我已经说累了
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            //距离玩家过远处死
            Vector2 getDist = Projectile.Center - Owner.Center;
            if (getDist.Length() > 4000f)
                Projectile.Kill();
        }
    }
}