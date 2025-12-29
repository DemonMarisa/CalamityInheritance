using CalamityInheritance.Buffs.Summon;
using CalamityInheritance.Texture;
using CalamityMod.NPCs;
using CalamityMod.Projectiles;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Summon.Worms
{
    public class Segment(Vector2 pos, float rot)
    {
        public Vector2 Pos = pos;
        public float Rot = rot;
    }
    public class DOGworm : ModProjectile, ILocalizedModType
    {
        public override string Texture => CITextureRegistry.DOGworm_Head.Path;
        public new string LocalizationCategory => "Content.Projectiles.Summon";
        public List<Segment> Segments = [];
        public int AttackStateTimer;
        public int AttackType;
        public Vector2 HoverPos;
        public ref float SpawnDust => ref Projectile.ai[0];
        public Player Owner => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.tileCollide = false;
            Projectile.minion = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.netUpdate = true;
            Projectile.minionSlots = 3f;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(ChargeBeginPos);
            writer.WriteVector2(HoverPos);
            writer.WriteVector2(PortalAttackBeginPos);
            for (int i = 0; i < Segments.Count; i++)
            {
                writer.WriteVector2(Segments[i].Pos);
            }
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            ChargeBeginPos = reader.ReadVector2();
            HoverPos = reader.ReadVector2();
            PortalAttackBeginPos = reader.ReadVector2();
            for (int i = 0; i < Segments.Count; i++)
            {
                Segments[i].Pos = reader.ReadVector2();
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            foreach (Segment segment in Segments)
            {
                if (targetHitbox.Intersects(Utils.CenteredRectangle(segment.Pos, new Vector2(50))))
                    return true;
            }
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void AI()
        {
            Projectile.netUpdate = true;
            Owner.AddBuff(BuffType<DOGSummonBuff>(), 2, true);
            if (!Owner.HasBuff(BuffType<DOGSummonBuff>()))
                Projectile.Kill();
            Projectile.Center += Projectile.velocity;
            if (Projectile.LAP().FirstFrame)
            {
                Projectile.velocity = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * 6;
                for (int i = 0; i < 14; i++)
                {
                    Segment segment = new(Projectile.Center, Projectile.rotation);
                    Segments.Add(segment);
                }
            }
            if (SpawnDust != 0)
            {
                for (int i = 0; i < 50; i++)
                {
                    Dust purpleElectricity = Dust.NewDustDirect(Projectile.position + Vector2.UnitY * 16f, Projectile.width, Projectile.height - 16, DustID.BoneTorch, 0f, 0f, 0, default, 1f);
                    purpleElectricity.velocity *= 2f;
                    purpleElectricity.scale *= 1.15f;
                }
                SpawnDust = 0;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            NPC target = LAPUtilities.FindClosestTarget(Projectile.Center, 3000, true);
            if (target is not null)
            {
                AttackStateTimer++;
                if (AttackType == 0)
                    LaserChargeAttack(target);
                else if (AttackType == 1)
                    ChangeToPortalAttack(target);
                else if (AttackType == 2)
                    PortalAttack(target);
            }
            else
            {
                PlayerFollowMovement(Owner);
            }
            for (int i = 0; i < Segments.Count; i++)
                UpdateSegment(i);
        }
        public void UpdateSegment(int segmentIndex)
        {
            float aheadSegmentRotation = segmentIndex > 0 ? Segments[segmentIndex - 1].Rot : Projectile.rotation;
            Vector2 aheadSegmentCenter = segmentIndex > 0 ? Segments[segmentIndex - 1].Pos : Projectile.Center;
            Vector2 offsetToAheadSegment = aheadSegmentCenter - Segments[segmentIndex].Pos;
            if (aheadSegmentRotation != Segments[segmentIndex].Rot)
            {
                float offsetAngle = MathHelper.WrapAngle(aheadSegmentRotation - Segments[segmentIndex].Rot);
                offsetToAheadSegment = offsetToAheadSegment.RotatedBy(offsetAngle * 0.075f);
            }
            Segments[segmentIndex].Rot = offsetToAheadSegment.ToRotation();
            if (offsetToAheadSegment != Vector2.Zero)
                Segments[segmentIndex].Pos = aheadSegmentCenter - offsetToAheadSegment.SafeNormalize(Vector2.Zero) * 20;
        }
        public int HoverFilp = 1;
        public void PlayerFollowMovement(Player owner)
        {
            AttackType = 0;
            Projectile.extraUpdates = 1;
            // If any attack was in use previously, send a net update now that attack mode is off.
            if (AttackStateTimer != 0)
            {
                AttackStateTimer = 0;
                Projectile.netUpdate = true;
            }
            if (Owner.miscCounter % 180 == 0)
            {
                HoverFilp = -HoverFilp;
                HoverPos = new Vector2(Main.rand.Next(200, 256) * HoverFilp, Main.rand.Next(-96, 96));
            }
            else if (Projectile.Center.Distance(Owner.Center) > 200)
            {
                Vector2 Target = Owner.Center + HoverPos;
                LAPUtilities.HomingTarget(Projectile, Target, -1, 24f, 100f);
            }
            else if (Projectile.Center.Distance(Owner.Center) > 500)
            {
                Vector2 Target = Owner.Center + HoverPos;
                LAPUtilities.HomingTarget(Projectile, Target, -1, 35f, 100f);
            }
            else if (Projectile.Center.Distance(Owner.Center) > 1920)
            {
                Projectile.extraUpdates = 2;
                Vector2 Target = Owner.Center;
                LAPUtilities.HomingTarget(Projectile, Target, -1, 35f, 35f);
            }
            else if (Projectile.Center.Distance(Owner.Center) > 3840)
            {
                Projectile.Center = Owner.Center + Vector2.UnitY * -200;
                for (int i = 0; i < Segments.Count; i++)
                {
                    Segments[i].Pos = Owner.Center + Vector2.UnitY * -200;
                }
            }
        }
        public int LaserAttackCount;
        public Vector2 ChargeBeginPos;
        public void LaserChargeAttack(NPC target)
        {
            Projectile.extraUpdates = 2;
            // 第一帧初始化随机目标位置
            if (AttackStateTimer == 1)
            {
                SoundEngine.PlaySound(SoundID.Item12, Projectile.Center);
                // 向目标发射激光
                for (int i = 0; i < 3; i++)
                {
                    Vector2 perturbedSpeed = LAPUtilities.GetVector2(Projectile.Center, target.Center).RotatedBy(MathHelper.Lerp(-0.15f, 0.15f, i / 2f)) * 18f;
                    if (LAPUtilities.IsLocalPlayer(Projectile.owner))
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ProjectileType<DOGLaser>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
                if (LAPUtilities.IsLocalPlayer(Projectile.owner))
                    ChargeBeginPos = target.Center + -Projectile.velocity.SafeNormalize(Vector2.UnitX) * 300;
            }
            // 有60帧的时间赶往目标
            if (AttackStateTimer > 0 && AttackStateTimer < 60)
            {
                float angularTurnSpeed = MathHelper.ToRadians(18f);
                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.AngleTo(ChargeBeginPos), angularTurnSpeed).ToRotationVector2() * 14f;
                if (Projectile.Center.Distance(ChargeBeginPos) < 24)// 如果离得太近，直接进入冲刺状态
                {
                    AttackStateTimer = 60 + Main.rand.Next(0, 10);// 添加一些随机偏移
                    HoverFilp = Main.rand.NextBool() ? 1 : -1;
                }
            }
            // 冲刺向目标，如果离得近则在敌人周围盘旋
            else if (AttackStateTimer >= 60 && AttackStateTimer < 90)
            {
                if (AttackStateTimer == 65)
                {
                    SoundEngine.PlaySound(SoundID.Item12, Projectile.Center);
                    // 向目标发射激光
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 perturbedSpeed = LAPUtilities.GetVector2(Projectile.Center, target.Center).RotatedBy(MathHelper.Lerp(-0.15f, 0.15f, i / 2f)) * 18f;
                        if (LAPUtilities.IsLocalPlayer(Projectile.owner))
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, perturbedSpeed, ProjectileType<DOGLaser>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                }
                if (Projectile.Center.Distance(target.Center) < 24)
                {
                    float angularTurnSpeed = MathHelper.ToRadians(9f);
                    Projectile.velocity = (Projectile.rotation + angularTurnSpeed).ToRotationVector2() * 14f;
                }
                else
                {
                    float angularTurnSpeed = MathHelper.ToRadians(18f);
                    Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.AngleTo(target.Center), angularTurnSpeed).ToRotationVector2() * 14f;
                }
            }
            // 脱离，不需要做任何事
            if (AttackStateTimer > 135)
            {
                LaserAttackCount++;
                AttackStateTimer = 0;
                if (LaserAttackCount >= 5)
                {
                    LaserAttackCount = 0;
                    AttackType = 1;
                }
            }
        }
        public void ChangeToPortalAttack(NPC target)
        {
            if (AttackStateTimer == 1)
            {
                if (LAPUtilities.IsLocalPlayer(Projectile.owner))
                    ChargeBeginPos = target.Center - Vector2.UnitY * 400;
            }
            if (AttackStateTimer < 120)
            {
                float angularTurnSpeed = MathHelper.ToRadians(18f);
                Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.AngleTo(ChargeBeginPos), angularTurnSpeed).ToRotationVector2() * 14f;
                if (Projectile.Center.Distance(ChargeBeginPos) < 64)
                {
                    AttackStateTimer = 120;
                }
            }
            else
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<TeleportRift>(), 0, 0f, Projectile.owner);
                AttackStateTimer = 0;
                AttackType = 2;
            }
        }
        public int ChargeCount;
        public Vector2 PortalAttackBeginPos;
        public void PortalAttack(NPC target)
        {
            if (AttackStateTimer == 1)
            {
                PortalAttackBeginPos = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * 300;
                if (LAPUtilities.IsLocalPlayer(Projectile.owner))
                    Projectile.Center = target.Center + PortalAttackBeginPos;
                for (int i = 0; i < Segments.Count; i++)
                {
                    Segments[i].Pos = Projectile.Center;
                }
                Projectile.velocity = LAPUtilities.GetVector2(Projectile.Center, target.Center) * 18f;
                if (LAPUtilities.IsLocalPlayer(Projectile.owner))
                    ChargeBeginPos = target.Center - PortalAttackBeginPos;
                if (LAPUtilities.IsLocalPlayer(Projectile.owner))
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<TeleportRift>(), 0, 0f, Projectile.owner);
            }
            if (AttackStateTimer <= 2)
            {
                for (int i = 0; i < Segments.Count; i++)
                {
                    Segments[i].Pos = Projectile.Center;
                }
            }
            if (Projectile.Center.Distance(ChargeBeginPos) < 48)
            {
                ChargeCount++;
                if (LAPUtilities.IsLocalPlayer(Projectile.owner))
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<TeleportRift>(), 0, 0f, Projectile.owner);
                AttackStateTimer = 0;
                if (ChargeCount > 12)
                {
                    AttackType = 0;
                    ChargeCount = 0;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tail = CITextureRegistry.DOGworm_Tail.Value;
            Vector2 TaildrawPos = Segments[^1].Pos - Main.screenPosition;
            Vector2 Tailorig = tail.Size() / 2;
            Main.spriteBatch.Draw(tail, TaildrawPos, null, Color.White * Projectile.Opacity, Segments[^1].Rot + MathHelper.PiOver2, Tailorig, 1f, 0, 0);
            for (int i = 1; i < Segments.Count - 1; i++)
            {
                Texture2D body = CITextureRegistry.DOGworm_Body.Value;
                Vector2 bodydrawPos = Segments[i].Pos - Main.screenPosition;
                Vector2 bodyorig = body.Size() / 2;
                Main.spriteBatch.Draw(body, bodydrawPos, null, Color.White * Projectile.Opacity, Segments[i].Rot + MathHelper.PiOver2, bodyorig, 1f, 0, 0);
            }
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawPos = Segments[0].Pos - Main.screenPosition;
            Vector2 orig = new Vector2(texture.Width / 2, texture.Height - 11);
            Main.spriteBatch.Draw(texture, drawPos, null, Color.White * Projectile.Opacity, Segments[0].Rot + MathHelper.PiOver2, orig, 1f, 0, 0);
            return false;
        }
    }
}
