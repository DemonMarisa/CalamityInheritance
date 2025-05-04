using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using CalamityMod.Particles;
using Terraria.Audio;
using CalamityInheritance.Content.Items.Weapons;
using XPT.Core.Audio.MP3Sharp.Decoding;
using CalamityMod.Projectiles.Rogue;
using CalamityInheritance.Sounds.Custom;
using System.IO;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class LumiStrikerProj : ModProjectile, ILocalizedModType
    {
        public int HitCounter = 0;
        public bool CanRotate = false;
        public float homingDist = 3600f;
        public float RotAngle = 0.5f;
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public static readonly SoundStyle Hitsound = new("CalamityMod/Sounds/Item/WulfrumKnifeTileHit2") { PitchVariance = 0.4f, Volume = 0.5f };
        public override string Texture => $"{Generic.WeaponRoute}/Rogue/LumiStriker";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 30;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 25;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 900;
            Projectile.extraUpdates = 5;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            Projectile.DoSyncHandlerWrite(ref writer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.DoSyncHandlerRead(ref reader);
        }
        public int Time = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            float rotOffset = MathHelper.PiOver4 + MathHelper.ToRadians(3.3f);

            Time++;
            Lighting.AddLight(Projectile.Center + Projectile.velocity * 0.6f, 0.6f, 0.2f, 0.9f);
            float radiusFactor = MathHelper.Lerp(0f, 1f, Utils.GetLerpValue(10f, 50f, Time, true));
            for (int i = 0; i < 9; i++)
            {
                float offsetRotationAngle = Projectile.velocity.ToRotation() + Time / 20f;
                float radius = (20f + (float)Math.Cos(Time / 3f) * 12f) * radiusFactor;
                Vector2 dustPosition = Projectile.Center;
                dustPosition += offsetRotationAngle.ToRotationVector2().RotatedBy(i / 5f * MathHelper.TwoPi) * radius;
                Dust dust = Dust.NewDustPerfect(dustPosition, Main.rand.NextBool() ? DustID.BubbleBurst_Blue : DustID.BubbleBurst_Pink);
                dust.noGravity = true;
                dust.velocity = Projectile.velocity * 0.1f;
                dust.scale = Main.rand.NextFloat(1.1f, 1.7f);
            }

            if (Projectile.Calamity().stealthStrike)
                StealthAI(rotOffset);
            else
                NorAI(rotOffset);
        }
        public void NorAI(float rotOffset)
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + rotOffset;
            //生成月明碎片
            if (Projectile.timeLeft % 20 == 0 && Projectile.owner == Main.myPlayer)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, -2f, ModContent.ProjectileType<LumiShard>(), (int)(Projectile.damage * 0.5), Projectile.knockBack * 0.25f, Projectile.owner);
        }

        public void StealthAI(float rotOffset)
        {
            // 开始时逐渐减速
            ref float Timer = ref Projectile.ai[0];
            ref float NPCType = ref Projectile.ai[1];
            ref float Timer2 = ref Projectile.ai[2];
            Timer++;
            if (Timer < 150)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + rotOffset;
                Projectile.velocity *= 0.985f;
            }
            else
            {
                NPC target = CIFunction.FindClosestTarget(Projectile, 2000);
                //这个方法可不会在返回null之后自动撤销程序的。
                if (target is null)
                    return;

                DoBehavior_Charge(target, ref Timer2, rotOffset);
            }

            if (Time > 10 && (Math.Abs(Projectile.velocity.X) >= 2f || Math.Abs(Projectile.velocity.Y) >= 2f))
            {
                SparkParticle spark3 = new SparkParticle(Projectile.Center - Projectile.velocity, Projectile.velocity * 0.01f, false, 18, 1.7f, Color.Plum * 0.6f);
                GeneralParticleHandler.SpawnParticle(spark3);
            }
        }
        #region 冲刺AI
        // 是的孩子们，我把NPC的冲刺AI搬过来了
        public bool canSlow = false;
        public bool hasCharge = false;
        public bool playercon = false;
        public int ChargeCount = 0;
        public void DoBehavior_Charge(NPC npc, ref float attacktimer, float rotOffset)
        {
            int totalCharge = 2;
            int slowCount = 80;
            // 不在慢下来的过程中时才会跟踪
            if (!canSlow && npc.Distance(Projectile.Center) < 2000f)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + rotOffset;
                CIFunction.HomingNPCBetter(Projectile, npc, 2000, 12f, 0f);
            }
            // 这里代表冲刺过程
            if (hasCharge == false)
            {
                if (!playercon)
                {
                    SoundEngine.PlaySound(CISoundMenu.LumiSpearAttackNor);
                    playercon = true;
                }
                // 击中敌人后可以减速，冲刺过程结束
                if (canSlow)
                    hasCharge = true;
                Projectile.netUpdate = true;
            }
            else if (canSlow)
            {
                // 会在80帧内慢下来
                attacktimer++;
                if (attacktimer < slowCount)
                {
                    Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.AngleTo(npc.Center) + rotOffset, 1f);
                    Projectile.velocity *= 0.985f;
                }
                // 随后开始新一轮冲刺，直到击中目标再减速
                else
                {
                    playercon = false;
                    canSlow = false;
                    hasCharge = false;
                    attacktimer = 0;
                    ChargeCount++;
                }
                Projectile.netUpdate = true;
            }

            if (ChargeCount > totalCharge - 1)
                Projectile.penetrate = 1;
        }
        #endregion
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            OnHitEffect(target);
            OnHitSparks();
            SoundEngine.PlaySound(Hitsound, target.Center);

            if(Projectile.ai[0] > 150)
                canSlow = true;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            OnHitEffect(target);
            SoundEngine.PlaySound(Hitsound, target.Center);
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.BubbleBurst_Blue, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
        }
        public void OnHitEffect(object target)
        {
            // 向后发射碎片，向前发射刀刃
            for (int i = 0; i < 7; i++)
            {
                Vector2 sVel = Projectile.velocity.RotatedByRandom(0.2f) * Main.rand.NextFloat(0.2f, 1.1f);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, -sVel, ModContent.ProjectileType<LumiShard>(), (int)(Projectile.damage * 0.5), Projectile.knockBack * 0.25f, Projectile.owner);
            }
            float rotation = MathHelper.ToRadians(15);
            for (int i = 0; i < 3; i++)
            {
                Vector2 perturbedSpeed = Projectile.velocity.RotatedBy(i == 0 ? -rotation : i == 2 ? rotation : 0);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ModContent.ProjectileType<RealityRuptureMini>(), (int)(Projectile.damage * 0.5), Projectile.knockBack * 0.25f, Projectile.owner);
            }
        }
        public void OnHitSparks()
        {
            int sparkCount = Main.rand.Next(20, 40);
            for (int i = 0; i < sparkCount; i++)
            {
                Vector2 sVel = Projectile.velocity.RotatedByRandom(0.2f) * Main.rand.NextFloat(0.6f, 1.7f);
                int sLife = Main.rand.Next(23, 60);
                float sScale = Main.rand.NextFloat(0.8f, 1f) * 0.955f;
                Color sColor = Color.Lerp(Color.DarkSlateBlue, Color.DarkBlue, Main.rand.NextFloat(0.7f));
                sColor = Color.Lerp(sColor, Color.Gold, Main.rand.NextFloat());
                SparkParticle spark = new(Projectile.Center, sVel, false, sLife, sScale, sColor);
                GeneralParticleHandler.SpawnParticle(spark);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}