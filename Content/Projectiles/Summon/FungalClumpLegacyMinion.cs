using System;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod.Projectiles.Typeless;
using CalamityMod;
using CalamityInheritance.Buffs.Summon;

namespace CalamityInheritance.Content.Projectiles.Summon
{
    public class FungalClumpLegacyMinion : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Summon";
        bool returnToPlayer = false;
        public Player Owner => Main.player[Projectile.owner];
        public ref float AttackTimer => ref Projectile.ai[0];
        public bool ShoulBack
        {
            get => Projectile.ai[1] != 0;
            set => Projectile.ai[1] = value ? 1f : 0f;
        }
        public bool JustSpawn = false;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.minionSlots = 0f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            TrueAI();
            /*
            bool shouldThisMinion = Projectile.type == ModContent.ProjectileType<FungalClumpLegacyMinion>();
            Owner.AddBuff(ModContent.BuffType<FungalClumpLegacyBuff>(), 18000);
            var UsPlayer = Owner.CIMod();
            if (!UsPlayer.FungalClumpLegacySummon)
            {
                Projectile.active = false;
                return;
            }
            // TrueAI();
            if (!shouldThisMinion)
                return;
            if (Owner.dead)
                UsPlayer.FungalClumpLegacySummon = false;
            
            if (UsPlayer.FungalClumpLegacySummon)
                Projectile.timeLeft = 2;
            
            SpawnDust();
            TrailingDust();
            PushingAway(0.05f);

            //距离玩家过远的情况下飞行速度变快
            float awayRange = 500;
            if (AttackTimer != 0f || Projectile.friendly)
                awayRange = 2400f;
            if ((Projectile.Center - Owner.Center).Length() > awayRange)
                ShoulBack = true;

            int searchRange = 1800;
            int attackTarget = -1;
            Projectile.Minion_FindTargetInRange(searchRange, ref attackTarget, false);
            bool foundTarget = attackTarget != -1;
            //如果有target则追踪敌人
            if (foundTarget)
            {
                //计时器。
                AttackTimer = 17f * (AttackTimer == -1f).ToInt();
                AttackTimer -= 1f * (AttackTimer > 0f).ToInt();
                if (AttackTimer == 0f)
                {
                    Projectile.friendly = true;
                    Projectile.HomingNPCBetter(Main.npc[attackTarget], searchRange, 28f, 20f);
                }
                else
                {
                    Projectile.friendly = false;
                    if (Projectile.velocity.Length() < 10f)
                        Projectile.velocity *= 1.05f;
                }
                Projectile.rotation = Projectile.velocity.X * 0.05f;
                if (Math.Abs(Projectile.velocity.X) <= 0.2f)
                {
                    return;
                }
                Projectile.spriteDirection = -Projectile.direction;
            }
            else
            {
                //没有target的时候就返回玩家
                Projectile.friendly = true;
                float backSpeed = 8f;
                float turnSpeed = 20f;
                if (ShoulBack)
                    backSpeed = 12f;

                Vector2 playerVector = Owner.Center - Projectile.Center;
                playerVector.Y -= 60f;
                float dist = playerVector.Length();
                if (dist < 100f && ShoulBack && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                    ShoulBack = false;
                if (dist > 2000f)
                    Projectile.Center = Owner.Center;

                if (awayRange > 70f)
                {
                    playerVector.Normalize();
                    playerVector *= backSpeed;
                    Projectile.velocity = (Projectile.velocity * turnSpeed + playerVector) / (turnSpeed + 1f);
                }

                //仆从常态保持移动
                else
                {
                    bool standing = Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f;
                    Projectile.velocity.X = -0.15f * standing.ToInt();
                    Projectile.velocity.Y = -0.05f * standing.ToInt();
                    Projectile.velocity *= 1.01f;
                    Projectile.friendly = false;
                    Projectile.rotation = Projectile.velocity.X * 0.05f;
                    if (Math.Abs(Projectile.velocity.X) <= 0.2f)
                    {
                        return;
                    }
                    Projectile.spriteDirection = -Projectile.direction;
                }
            }
            */
        }
        private void TrueAI()
        {
            var modPlayer = Owner.CIMod();

            bool correctMinion = Projectile.type == ModContent.ProjectileType<FungalClumpMinion>();
            if (!modPlayer.FungalClumpLegacySummon)
            {
                Projectile.active = false;
                return;
            }
            if (correctMinion)
            {
                if (Owner.dead)
                {
                    modPlayer.FungalClumpLegacySummonBuff = false;
                }
                if (modPlayer.FungalClumpLegacySummonBuff)
                {
                    Projectile.timeLeft = 2;
                }
            }

            Projectile.damage = (int)Owner.GetBestClassDamage().ApplyTo(Projectile.originalDamage);

            SpawnDust();
            //粒子
            TrailingDust();
            //推开同类仆从
            PushingAway(0.05f);

            float playerRange = 500f;
            if (Projectile.ai[1] != 0f || Projectile.friendly)
                playerRange = 1400f;
            if ((Owner.Center - Projectile.Center).Length() > playerRange)
                returnToPlayer = true;

            //试了下tMod方法
            Vector2 targetVec = Projectile.Center;
            int range = 1200;
            int targetIndex = -1;
            Projectile.Minion_FindTargetInRange(range, ref targetIndex, false);
            bool npcFound = targetIndex != -1;

            Projectile.tileCollide = !returnToPlayer;

            if (!npcFound)
            {
                Projectile.friendly = true;
                float homingSpeed = 8f;
                float turnSpeed = 20f;
                //返程提速
                if (returnToPlayer)
                    homingSpeed = 12f;
                Vector2 playerVector = Owner.Center - Projectile.Center;
                playerVector.Y -= 60f;
                float playerDist = playerVector.Length();
                if (playerDist < 100f && returnToPlayer && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                    returnToPlayer = false;
                if (playerDist > 2000f)
                {
                    Projectile.Center = Owner.Center;
                }
                //超出距离返程
                if (playerDist > 70f)
                {
                    playerVector.Normalize();
                    playerVector *= homingSpeed;
                    Projectile.velocity = (Projectile.velocity * turnSpeed + playerVector) / (turnSpeed + 1f);
                }
                //不让仆从保持静止状态
                else
                {
                    if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
                    {
                        Projectile.velocity.X = -0.15f;
                        Projectile.velocity.Y = -0.05f;
                    }
                    Projectile.velocity *= 1.01f;
                }
                Projectile.friendly = false;
                Projectile.rotation = Projectile.velocity.X * 0.05f;
                if (Math.Abs(Projectile.velocity.X) <= 0f)
                    return;
                Projectile.spriteDirection = -Projectile.direction;
            }
            else
            {
                if (AttackTimer == -1f)
                    AttackTimer = 17f;
                if (AttackTimer > 0f)
                {
                    AttackTimer -= 1f;
                }
                if (AttackTimer == 0f)
                {
                    Projectile.friendly = true;
                    NPC target = Main.npc[targetIndex];
                    float minionSpeed = 8f;
                    float turnSpeed = 14f;
                    targetVec = Projectile.Center - target.Center;
                    float targetDist = Projectile.Distance(targetVec);
                    if (targetDist < 100f)
                        minionSpeed = 10f;

                    Projectile.HomingNPCBetter(target, range, minionSpeed, turnSpeed);
                }
                else
                {
                    Projectile.friendly = false;
                    if (Projectile.velocity.Length() < 10f)
                        Projectile.velocity *= 1.05f;
                }
                Projectile.rotation = Projectile.velocity.X * 0.05f;
                if (Math.Abs(Projectile.velocity.X) <= 0.2f)
                    return;
                Projectile.spriteDirection = -Projectile.direction;
            }
        }
        

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int healAmt = (int)(damageDone * 0.25f);
            if (healAmt == 0)
                return;
            else
            {
                CIFunction.SpawnHealProj(Projectile.GetSource_FromThis(), Projectile.Center, Owner, healAmt);
            }

        }
        private void PushingAway(float pushForce)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile getProj = Main.projectile[i];
                //简化这个Loop    
                if ((getProj.Center - Owner.Center).Length() < CIFunction.SetDistance(15))
                    continue;
                if (!getProj.active || getProj.owner != Owner.whoAmI || !getProj.minion || i == Projectile.whoAmI)
                    continue;
                //与灾厄同类的仆从做距离处理
                bool similarType = getProj.type == ModContent.ProjectileType<FungalClumpMinion>();
                float distBetween = (getProj.Center - Projectile.Center).Length();
                if (similarType && distBetween < Projectile.width)
                {
                    if (Projectile.Center.X < getProj.Center.X)
                        Projectile.velocity.X -= pushForce;
                    else
                        Projectile.velocity.X += pushForce;

                    if (Projectile.Center.Y < getProj.Center.Y)
                        Projectile.velocity.Y -= pushForce;
                    else
                        Projectile.velocity.Y += pushForce;
                }
            }
        }

        private void SpawnDust()
        {
            if (JustSpawn)
            {
                int dCount = 36;
                for (int i = 0; i < dCount; i++)
                {
                    Vector2 fuckYouCalamity = Vector2.Normalize(Projectile.velocity) * new Vector2(Projectile.Center.X, Projectile.height) * 0.75f;
                    fuckYouCalamity = fuckYouCalamity.RotatedBy((double)((i - (dCount / 2 - 1)) * MathHelper.TwoPi / dCount), default) + Projectile.Center;
                    Vector2 realCenter = fuckYouCalamity - Projectile.Center;
                    int d = Dust.NewDust(fuckYouCalamity + realCenter, 0, 0, DustID.BlueFairy, realCenter.X * 1.5f, realCenter.Y * 1.5f, 100, default, 1.4f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].noLight = true;
                    Main.dust[d].velocity = realCenter;
                }
                JustSpawn = true;
            }
        }

        private void TrailingDust()
        {
            if (Main.rand.NextBool(16))
                Dust.NewDustDirect(Projectile.Center + Projectile.velocity, Projectile.width, Projectile.height, DustID.BlueFairy, Projectile.velocity.X * 0.05f, Projectile.velocity.Y * 0.05f);
        }
    }
}