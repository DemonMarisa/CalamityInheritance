using CalamityInheritance.Assets;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.DamageBuffs;
using LAP.Assets.TextureRegister;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoWeapons
{
    // Photoviscerator right click split projectile (attached flares to the flare cluster)
    public class ExoFlareold : CIExoProj
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;

        public float OffsetSpeed
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public float OffsetRotation
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public int Father
        {
            get => (int)Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
            Projectile.timeLeft = 160;
            Projectile.extraUpdates = 1;
            Projectile.MaxUpdates = 1;
            Projectile.CI().CanSplit = false;
        }
        public override void AI()
        {
            if (Projectile.localAI[0] == 0f)
            {
                OffsetRotation = Main.rand.NextFloat(MathHelper.TwoPi);
                OffsetSpeed = Main.rand.NextFloat(MathHelper.ToRadians(2.5f), MathHelper.ToRadians(4f)) * Main.rand.NextBool(2).ToDirectionInt();
                Projectile.localAI[0] = 1f;
            }

            // Ensure that the owner projectile index is a valid one.
            if (!Main.projectile.IndexInRange(Father))
            {
                Projectile.Kill();
                return;
            }

            Projectile owner = Main.projectile[Father];

            // Ensure the owner is the correct projectile.
            if (owner.type != ProjectileType<ExoFlareClusterold>() && Projectile.IsLocalPlayer())
                Projectile.Kill();

            // Movement around the owner.
            float orbitRadiusMultiplier = 2.3f;
            Projectile.Center = owner.Center + OffsetRotation.ToRotationVector2() * (float)Math.Cos(OffsetRotation * 0.3f) * owner.Size * 0.5f * orbitRadiusMultiplier;
            Projectile.rotation = (Projectile.position - Projectile.oldPos[1]).ToRotation();
            OffsetRotation += OffsetSpeed;

            if (Projectile.timeLeft <= 30)
            {
                Projectile.alpha += 15;
                if (Projectile.alpha >= 255)
                {
                    Projectile.alpha = 255;
                    return;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D lightTexture = CIExtraTexture.SmallGreyscaleCircle.Value;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float colorInterpolation = (float)Math.Cos(Projectile.timeLeft / 16f + Main.GlobalTimeWrappedHourly / 20f + i / (float)Projectile.oldPos.Length * MathHelper.Pi) * 0.5f + 0.5f;
                Color color = Color.Lerp(Color.LightGreen, Color.LightPink, colorInterpolation) * 0.4f;
                color.A = 0;
                Vector2 drawPosition = Projectile.oldPos[i] + lightTexture.Size() * 0.5f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + new Vector2(-0f, -Projectile.gfxOffY);
                Color outerColor = color;
                Color innerColor = color * 0.5f;
                float intensity = 0.9f + 0.15f * (float)Math.Cos(Main.GlobalTimeWrappedHourly % 60f * MathHelper.TwoPi);

                // Become smaller the futher along the old positions we are.
                intensity *= MathHelper.Lerp(0.15f, 1f, 1f - i / (float)Projectile.oldPos.Length);

                Vector2 outerScale = new Vector2(1.65f) * intensity;
                Vector2 innerScale = new Vector2(1.65f) * intensity * 0.7f;
                outerColor *= intensity;
                innerColor *= intensity;
                Main.EntitySpriteDraw(lightTexture, drawPosition, null, outerColor * Projectile.Opacity, 0f, lightTexture.Size() * 0.5f, outerScale * 0.6f, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(lightTexture, drawPosition, null, innerColor * Projectile.Opacity, 0f, lightTexture.Size() * 0.5f, innerScale * 0.6f, SpriteEffects.None, 0);
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIMiracleBlight>(), 300);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffType<CIMiracleBlight>(), 300);
    }
}
