using CalamityMod.Buffs.DamageOverTime;
using CalamityMod;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Weapons;
using System.ComponentModel.DataAnnotations;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Rogue;
using System.Data;

namespace CalamityInheritance.Content.Projectiles.ExoLore
{
    public class CelestusBoomerangExoLoreHomeIn : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => $"{Generic.WeaponRoute}/Rogue/Celestusold";
        private bool initialized = false;
        private float Timer = 0f;
        private float Timer2 = 0f;
        #region 攻击类型枚举
        const float IsReturning = -1f;
        const float IsHoming = 0f;
        const float IsAttacking = 1f;
        const float IsIdleing = 2f;
        #endregion
        #region 数组别名
        const int AttackType = 0;
        const int PhaseTimer = 1;
        const int HitCounter = 2;
        #endregion
        #region 基础属性
        public int ForceTarget = -1;
        public int ColorTimer = 0;
        #endregion
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 94;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.alpha = 50;
            Projectile.localNPCHitCooldown = 20;
            Projectile.timeLeft = 600;
            Projectile.velocity *= -1f;
            Projectile.netUpdate = true;
        }
        public override bool? CanDamage()
        {
            return Projectile.ai[AttackType] == IsAttacking || Projectile.ai[AttackType] == IsReturning;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            //重做AI逻辑, 我们先获取这个敌怪单位
            NPC target = CIFunction.FindClosestTarget(Projectile, 5000f, true);
            //如果目标不存在直接执行返程AI
            if (target == null)
            {
                DoRetuningAI(player);
                Projectile.netUpdate = true;
                return;
            }
            //先直接让他直线飞行追踪最近的敌怪,
            if (!initialized)
                DoHoming(target);
            else
                Projectile.velocity *= 0.96f;

            //不出意外，上方的AI执行完下方的是可以正常执行的
            if (Projectile.owner == Main.myPlayer)
            {
                switch (Projectile.ai[AttackType])
                {
                    case IsAttacking:
                        DoAttacking(target, player);
                        break;
                    case IsIdleing:
                        DoIdleing(target, player);
                        break;
                    case IsReturning:
                        DoRetuningAI(player);
                        break;
                    default:
                        break;
                }
            }
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(CISoundID.SoundBoomerangs, Projectile.position);
            }
        }

        private void DoIdleing(NPC target, Player player)
        {
            Vector2 angleToTarget;
            if (target != null) angleToTarget = target.Center;
            else angleToTarget = player.Center;
            //从宙能那抄过来的
            float rot = Projectile.AngleTo(angleToTarget) - MathHelper.PiOver4;
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, rot, 0.2f);
            Projectile.velocity *= 0.97f;
            //启用这个计时器
            Projectile.ai[PhaseTimer] += 1f;
            //计时器达到要求，执行ai逻辑
            if (Projectile.ai[PhaseTimer] > 66f)
            {
                //回程至玩家手上
                //置为-1f, 不做任何事情
                Projectile.ai[AttackType] = IsReturning;
                Projectile.netUpdate = true;
                //Timer置零
                Projectile.ai[PhaseTimer] = 0f;
            }
        }

        //攻击AI逻辑，直接穿透1次然后返回
        public void DoAttacking(NPC target, Player player)
        {
            if (target == null) return;
            float setAngle = Projectile.AngleTo(player.Center) - MathHelper.PiOver4;
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, setAngle, 0.01f);
            Timer2++;
            if (Timer2 % 2 == 0)
                Timer += 5f;
            CIFunction.HomingNPCBetter(Projectile, target, 1800f, Celestusold.SetProjSpeed + Timer, 20f, 2, Celestusold.SetProjSpeed);
        }

        public void DoRetuningAI(Player player)
        {
            Projectile.ai[PhaseTimer] += 1f;
            if (Projectile.ai[PhaseTimer] > 10f)
            Projectile.rotation += 1f;
            float returnSpeed = 25f;
            float acceleration = 5f;
            CIFunction.BoomerangReturningAI(player, Projectile, returnSpeed, acceleration);
            if (Main.myPlayer == Projectile.owner)
            {
                Rectangle projHitbox = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                Rectangle playerHitbox = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
                if (projHitbox.Intersects(playerHitbox))
                    Projectile.Kill();
            }
        }

        public void DoHoming(NPC target)
        {
            //不断检测与敌怪的距离
            float distCheck = (Projectile.Center - target.Center).Length();
            //与上方一样，飞行过程周不断保持跟踪和转向。不过也有点区别。
            //射弹如果与这个距离相同，停止射弹的追踪，并执行Attacking的指令
            if (distCheck <= 1800f)
            {
                //检测射弹的AI是否还是低于30f, 如果是，直接设为30f
                if (Projectile.ai[PhaseTimer] < 30f)
                    Projectile.ai[PhaseTimer] = 30f;
                
                float rot = Projectile.AngleTo(target.Center) - MathHelper.PiOver4;
                Lighting.AddLight(Projectile.Center, Main.DiscoR * 0.5f / 255f, Main.DiscoG * 0.5f / 255f, Main.DiscoB * 0.5f / 255f);
                Projectile.rotation = Utils.AngleLerp(Projectile.rotation, rot, 0.2f);
                //同时逐渐降低速度
                Projectile.velocity *= 0.90f;
                //启用新的计时器
                Projectile.ai[PhaseTimer] += 1f;
                //符合条件启用新的AI
                if (Projectile.ai[PhaseTimer] > 90f)
                {
                    //标记射弹已经完成了第一个指令
                    initialized = true;
                    Projectile.ai[AttackType] = IsAttacking;
                    Projectile.ai[PhaseTimer] = 0f;
                    Projectile.netUpdate = true;
                }
            }
            //我们去追及这个敌怪
            else if (Projectile.ai[PhaseTimer] < 30f) 
            {
                Projectile.rotation += 1f;
                CIFunction.HomingNPCBetter(Projectile, target, 1800f, Celestusold.SetProjSpeed, 20f);
                Projectile.ai[PhaseTimer] += 1f;
            }
            
        }

        public override Color? GetAlpha(Color lightColor)
        {
            int red = 135;
            int blue = 206;
            int green = 250;
            int r = 255;
            int g = 255;
            int b = 255;
            ColorTimer++;
            if (r > red) r -= ColorTimer * 5;
            if (b > blue) b -= ColorTimer * 1;
            if (g > green) g -= ColorTimer * 1;
            if (r < red) r = red;
            if (b < blue) b = blue;
            if (g < green) g = green;
            return new Color(r, g, b, Projectile.alpha);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[AttackType] == IsAttacking)
            {
                Projectile.ai[AttackType] = IsIdleing;
                Projectile.netUpdate = true;
            }
            target.ExoDebuffs();
            OnHitEffects();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
            OnHitEffects();
        }

        private void OnHitEffects()
        {
            //将这个生成频率降低了一点，主要是一次弹出的伤害太多了
            if (Projectile.owner == Main.myPlayer && Main.rand.NextBool(2))
            {
                float spread = 45f * 0.0174f;
                double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 8f;
                double offsetAngle;
                for (int i = 0; i < 4; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 0.6f), (float)(Math.Cos(offsetAngle) * 0.6f), ModContent.ProjectileType<Celestus2ExoLore>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 0.6f), (float)(-Math.Cos(offsetAngle) * 0.6f), ModContent.ProjectileType<Celestus2ExoLore>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner);
                }
            }
            SoundEngine.PlaySound(SoundID.Item122, Projectile.Center);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
