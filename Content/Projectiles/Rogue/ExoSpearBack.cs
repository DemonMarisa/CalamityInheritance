using CalamityMod.Buffs.DamageOverTime;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using System;
using CalamityMod;
using CalamityInheritance.Content.Items;
using System.IO;
using LAP.Assets.TextureRegister;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class ExoSpearBack : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public bool ProducedAcceleration = false;

        public ref float Time => ref Projectile.ai[0];
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = GetInstance<RogueDamageClass>();
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 3;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false;
        }
        public override void SendExtraAI(BinaryWriter writer) => writer.Write(ProducedAcceleration);
        public override void ReceiveExtraAI(BinaryReader reader) => ProducedAcceleration = reader.ReadBoolean();
        public override void AI()
        {
            Time++;
            Lighting.AddLight(Projectile.Center + Projectile.velocity * 0.6f, 0.6f, 0.2f, 0.9f);
            float radiusFactor = MathHelper.Lerp(0f, 1f, Utils.GetLerpValue(10f, 50f, Time, true));
            for (int i = 0; i < 40; i++)
            {
                float offsetRotationAngle = Projectile.velocity.ToRotation() + Time / 20f;
                float radius = (25f + (float)Math.Cos(Time / 3f) * 12f) * radiusFactor;
                Vector2 dustPosition = Projectile.Center;
                dustPosition += offsetRotationAngle.ToRotationVector2().RotatedBy(i / 5f * MathHelper.TwoPi) * radius;
                Dust dust = Dust.NewDustPerfect(dustPosition, Main.rand.NextBool() ? CIDustID.DustSandnado : 107);
                dust.noGravity = true;
                dust.velocity = Projectile.velocity * 0.8f;
                dust.scale = Main.rand.NextFloat(1.1f, 1.7f);
            }
            Dust.NewDustPerfect(Projectile.Center, 247, (Vector2?)new Vector2(0f, 0f), 0, default(Color), 1f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.ExoDebuffs();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffType<MiracleBlight>(), 300);
    }
}
