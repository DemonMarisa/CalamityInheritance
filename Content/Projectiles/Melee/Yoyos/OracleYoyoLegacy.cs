using CalamityInheritance.Content.Items.Weapons.Melee.Yoyos;
using CalamityMod;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Projectiles.Typeless;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Yoyos
{
    public class OracleYoyoLegacy : ModProjectile
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<TheOracleLegacy>();
        public int HitBoxSize => (CurCharge - 60) < 0 ? 0 : (CurCharge - 60);
        public int OrigHitBoxSize = 60;
        public int UpdatesNum = 3;
        public int MaxCharge = 120;
        public int CurCharge;
        public int ChargeSpeed = 1;
        public int DisCharge = 0;
        public int DisChargeSpeed = 1;
        public int Time;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = -1f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 800f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 60f / UpdatesNum;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(CurCharge);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            CurCharge = reader.ReadInt32();
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = ProjAIStyleID.Yoyo;
            Projectile.width = Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = UpdatesNum;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 3 * UpdatesNum;
            Projectile.netImportant = true;
        }

        public override void AI()
        {
            if ((Projectile.position - Main.player[Projectile.owner].position).Length() > 3200f)
                Projectile.Kill();
            if (!LAPUtilities.FinalExtraUpdate(Projectile))
                return;
            Time++;
            if (Main.rand.NextBool())
            {
                int dustType = Main.rand.NextBool(3) ? 244 : 246;
                float scale = 0.8f + Main.rand.NextFloat(0.6f);
                int idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].velocity = Vector2.Zero;
                Main.dust[idx].scale = scale;
            }
            Vector2 vel = new Vector2(45, 45).RotatedByRandom(100);
            Dust dust = Dust.NewDustPerfect(Projectile.Center + vel, 213, Vector2.Zero, 0, default, Main.rand.NextFloat(2.2f, 2.4f));
            dust.noGravity = true;
            if (LAPUtilities.IsLocalPlayer(Projectile.owner))
            {
                // 逐渐递减结束充能
                if (DisCharge > 0)
                    DisCharge -= DisChargeSpeed;
                // 如果击中敌人会递增
                if (DisCharge > 2)
                {
                    if (CurCharge < MaxCharge)
                        CurCharge += ChargeSpeed;
                }
                else // 否则会降低
                {
                    if (CurCharge > 0)
                        CurCharge -= ChargeSpeed;
                }
                foreach (NPC target in Main.ActiveNPCs)
                {
                    if (target.dontTakeDamage || target.friendly)
                        continue;
                    if (Projectile.Center.Distance(target.Center) < CurCharge)
                    {
                        Projectile.Damage();
                    }
                }
                if (Time % 10 == 0 && CurCharge > 30)
                    FireAuricOrbs();
            }
            if (CurCharge > 10)
            {
                DrawRedLightningAura(CurCharge);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.Center.Distance(Projectile.Center) < OrigHitBoxSize)
                DisCharge = 180;
        }

        // Uses dust type 260, which lives for an extremely short amount of time
        private void DrawRedLightningAura(float radius)
        {
            // Light emits from the yoyo itself while the aura is active, eventually becoming insanely bright
            float brightness = radius * 0.03f;
            Lighting.AddLight(Projectile.Center, brightness, 0.2f * brightness, 0.1f * brightness);

            // Number of particles on the circumference scales directly with the circumference
            float dustDensity = 0.2f;
            int numDust = (int)(dustDensity * MathHelper.TwoPi * radius);
            float angleIncrement = MathHelper.TwoPi / numDust;

            // Incrementally rotate the vector as a ring of dust is drawn
            Vector2 dustOffset = new Vector2(radius, 0f);
            dustOffset = dustOffset.RotatedByRandom(MathHelper.TwoPi);
            for (int i = 0; i < numDust; ++i)
            {
                dustOffset = dustOffset.RotatedBy(angleIncrement);
                int dustType = 260;
                float scale = 1.6f + Main.rand.NextFloat(0.9f);
                int idx = Dust.NewDust(Projectile.Center, 1, 1, dustType);
                Main.dust[idx].position = Projectile.Center + dustOffset;
                Main.dust[idx].noGravity = true;
                Main.dust[idx].noLight = true;
                Main.dust[idx].velocity *= 0.5f;
                Main.dust[idx].scale = scale;
            }
            if (Main.rand.NextBool(15))
            {
                int numArcs = 3;
                float arcDustDensity = 0.6f;
                if (Main.rand.NextBool())
                    ++numArcs;
                if (Main.rand.NextBool())
                    ++numArcs;

                Vector2 radiusVec = new Vector2(radius, 0f);
                int dustPerArc = (int)(arcDustDensity * radius);
                for (int i = 0; i < numArcs; ++i)
                {
                    radiusVec = radiusVec.RotatedByRandom(MathHelper.TwoPi);
                    for (int j = 0; j < dustPerArc; ++j)
                    {
                        Vector2 partialRadius = (float)j / dustPerArc * radiusVec;
                        int dustType = 260;
                        float scale = 1.6f + Main.rand.NextFloat(0.9f);
                        int idx = Dust.NewDust(Projectile.Center, 1, 1, dustType);
                        Main.dust[idx].position = Projectile.Center + partialRadius;
                        Main.dust[idx].noGravity = true;
                        Main.dust[idx].noLight = true;
                        Main.dust[idx].velocity *= 0.3f;
                        Main.dust[idx].scale = scale;
                    }
                }
                SoundEngine.PlaySound(SoundID.NPCHit53 with { Volume = 0.2f }, Projectile.Center);
            }
        }
        public void FireAuricOrbs()
        {
            // Play a sound when orbs are fired
            SoundEngine.PlaySound(SoundID.Item92 with { Volume = 0.3f }, Projectile.Center);
            int numOrbs = 6;
            int orbID = ProjectileType<OracleYoyoOrb>();
            int orbDamage = Projectile.damage * 2;
            float orbKB = 8f;
            float angleVariance = MathHelper.TwoPi / numOrbs;
            float spinOffsetAngle = MathHelper.Pi / (2f * numOrbs);
            Vector2 posVec = new Vector2(2f, 0f).RotatedByRandom(MathHelper.TwoPi);
            for (int i = 0; i < numOrbs; ++i)
            {
                posVec = posVec.RotatedBy(angleVariance);
                Vector2 velocity = new Vector2(posVec.X, posVec.Y).RotatedBy(spinOffsetAngle);
                velocity.Normalize();
                velocity *= 18f;
                if (Projectile.owner == Main.myPlayer)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + posVec, velocity, orbID, orbDamage, orbKB, Main.myPlayer, 0.0f, 0.0f);
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => CalamityUtils.CircularHitboxCollision(Projectile.Center, OrigHitBoxSize + HitBoxSize, targetHitbox);
    }
}
