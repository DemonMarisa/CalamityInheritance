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
using CalamityInheritance.Sounds.Custom;
using System.IO;

namespace CalamityInheritance.Content.Projectiles.ExoLore
{
    public class CelestusBoomerangExoLoreHomeIn : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => GetInstance<Celestusold>().Texture;
        private bool initialized = false;
        private float Timer = 0f;
        private float Timer2 = 0f;
        #region 攻击类型枚举
        const float IsReturning = -3f;
        const float IsAttacking = 1f;
        const float IsIdleing = 2f;
        #endregion
        #region 数组别名
        public ref float AttackType => ref Projectile.ai[0];
        public ref float AttackTimer => ref Projectile.ai[1];
        public int AttackTarget
        {
            get => (int)Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }
        #endregion
        #region 基础属性
        public int ColorTimer = 0;
        #endregion
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
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
            //我在想是不是因为这个无敌帧导致他一个射弹短时间内进行了多判导致c出问题
            Projectile.localNPCHitCooldown = -1;
            Projectile.DamageType = GetInstance<RogueDamageClass>();
            Projectile.alpha = 255;
            Projectile.timeLeft = 600;
            Projectile.velocity *= -1f;
            Projectile.netUpdate = true;
            Projectile.noEnchantmentVisuals = true;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            Projectile.DoSyncHandlerWrite(ref writer);
            writer.Write(Timer);
            writer.Write(Timer2);
            writer.Write(initialized);
            writer.Write(ColorTimer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.DoSyncHandlerRead(ref reader);
            Timer = reader.ReadInt32();
            Timer2 = reader.ReadInt32();
            ColorTimer = reader.ReadInt32();
            initialized = reader.ReadBoolean();
        }
        //取消返程伤害
        public override bool? CanHitNPC(NPC target)
        {
            return AttackType == IsAttacking;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            //补：在这个过程中alpha会快速上升
            if (Projectile.alpha > 50)
                Projectile.alpha -= 10;
            //重做AI逻辑, 我们先获取这个敌怪单位
            NPC target = CIFunction.FindClosestTarget(Projectile, 5000f, true);
            //如果目标不存在直接执行返程AI
            if (target is null)
            {
                DoRetuningAI(player);
                Projectile.netUpdate = true;
                return;
            }
            //先直接让他直线飞行追踪最近的敌怪,
            if (!initialized)
                DoHoming(target);

            //不出意外，上方的AI执行完下方的是可以正常执行的
            if (Projectile.owner == Main.myPlayer)
            {
                switch (AttackType)
                {
                    case IsAttacking:
                        DoAttacking(target, player);
                        break;
                    case IsIdleing:
                        DoIdleing(player);
                        break;
                    case IsReturning:
                        DoRetuningAI(player);
                        break;
                    default:
                        break;
                }
            }
        }

        private void DoIdleing(Player player)
        {
            NPC target = Main.npc[AttackTarget];
            Vector2 angleToTarget;
            if (target != null) angleToTarget = target.Center;
            else angleToTarget = player.Center;
            //从宙能那抄过来的
            float rot = Projectile.AngleTo(angleToTarget) - MathHelper.PiOver4;
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, rot, 0.2f);
            Projectile.velocity *= 0.97f;
            //启用这个计时器
            AttackTimer += 1f;
            //计时器达到要求，执行ai逻辑
            if (AttackTimer > 60f)
            {
                //回程至玩家手上
                //置为-1f, 不做任何事情
                AttackType = IsReturning;
                Projectile.netUpdate = true;
                //Timer置零
                AttackTimer = 0f;
            }
        }

        //攻击AI逻辑，直接穿透1次然后返回
        public void DoAttacking(NPC target, Player player)
        {
            if (target == null)
                return;

            //移除这一段
            // float setAngle = Projectile.AngleTo(player.Center) - MathHelper.PiOver4;
            // Projectile.rotation = Utils.AngleLerp(Projectile.rotation, setAngle, 0.01f);
            Timer2++;
            if (Timer2 % 2 == 0)
                Timer += 5f;
            CIFunction.HomingNPCBetter(Projectile, target, 1800f, Celestusold.SetProjSpeed + Timer, 20f, 2, Celestusold.SetProjSpeed);
        }

        public void DoRetuningAI(Player player)
        {
            AttackTimer += 1f;
            if (AttackTimer > 10f)
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
            //移除距离检测
            if (AttackTimer < 60f)
            {
                AttackTimer += 1f;
                float rot = Projectile.AngleTo(target.Center) - MathHelper.PiOver4;
                Lighting.AddLight(Projectile.Center, Main.DiscoR * 0.5f / 255f, Main.DiscoG * 0.5f / 255f, Main.DiscoB * 0.5f / 255f);
                Projectile.rotation = Utils.AngleLerp(Projectile.rotation, rot, 0.2f);
                //同时逐渐降低速度
                Projectile.velocity *= 0.90f;
            }
            else
            {
                Projectile.rotation += 1f;
                //标记射弹已经完成了第一个指令
                initialized = true;
                AttackType = IsAttacking;
                Projectile.netUpdate = true;
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
            if (AttackType == IsAttacking)
            {
                //在这个位置置零Timer
                AttackTimer = 0f;
                AttackType = IsIdleing;
                Projectile.netUpdate = true;
                //我不太确定是不是丢判了
                AttackTarget = target.whoAmI;
                //播放一次。
                SoundStyle[] getSound =
                [
                    CISoundMenu.CelestusOnHit1,
                    CISoundMenu.CelestusOnHit2,
                    CISoundMenu.CelestusOnHit3
                ];
                SoundEngine.PlaySound(Utils.SelectRandom(Main.rand, getSound) with {MaxInstances = 0}, Projectile.position);
            }
            target.ExoDebuffs();
            OnHitEffects();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffType<MiracleBlight>(), 300);
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
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 0.6f), (float)(Math.Cos(offsetAngle) * 0.6f), ProjectileType<Celestus2ExoLore>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 0.6f), (float)(-Math.Cos(offsetAngle) * 0.6f), ProjectileType<Celestus2ExoLore>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
