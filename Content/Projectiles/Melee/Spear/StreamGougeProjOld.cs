using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using CalamityMod.Projectiles.BaseProjectiles;
using Terraria.Audio;

namespace CalamityInheritance.Content.Projectiles.Melee.Spear
{
    public class StreamGougeProjOld : BaseSpearProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 40;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 90;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
        }

        public override float InitialSpeed => 5.5f;
        public override float ReelbackSpeed => 2.1f;
        public override float ForwardSpeed => 1f;
        public override Action<Projectile> EffectBeforeReelback => (proj) =>
        {
            int damage = (int)(Projectile.damage * 0.5f);
            float kb = Projectile.knockBack * 0.5f;
            Vector2 projPos = Projectile.Center + Projectile.velocity;
            Vector2 projVel = Projectile.velocity * 0.75f;
            if (Projectile.owner == Main.myPlayer)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), projPos, projVel, ModContent.ProjectileType<EssenceBeam>(), damage * 4, kb, Projectile.owner, 0f, 0f);

            SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);

            // Create a circle of purple dust where the projectile comes out, looking like the edge of a portal
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
        };

        public override void PostDraw(Color lightColor)
        {
            Vector2 origin = new Vector2(0f, 0f);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>("CalamityInheritance/Content/Projectiles/Melee/Spear/StreamGougeProjOldGlowProj").Value, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, origin, 1f, SpriteEffects.None, 0f);
        }
        public override void ExtraBehavior()
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
