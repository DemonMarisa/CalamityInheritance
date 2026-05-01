using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ModLoader;
using CalamityMod.Balancing;
using LAP.Core.Utilities;
using LAP.Content.Projectiles.LifeStealProj;

namespace CalamityInheritance.Content.Projectiles.Melee.Swords
{
    public class IridescentExcaliburSlash : GeneralDamageProj
    {
        public override ProjDamageType UseDamageClass => ProjDamageType.Melee;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.ownerHitCheck = true;
            Projectile.ownerHitCheckDistance = 300f;
            Projectile.usesOwnerMeleeHitCD = true;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            Player player = Main.player[Projectile.owner];
            float num = Projectile.localAI[0] / Projectile.ai[1];
            float num2 = Projectile.ai[0];
            float num3 = Projectile.velocity.ToRotation();
            float num4 = (float)Math.PI * num2 * num + num3 + num2 * (float)Math.PI + player.fullRotation;
            Projectile.rotation = num4;
            float num5 = 1f;
            float num6 = 1.2f;

            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) - Projectile.velocity;
            Projectile.scale = num6 + num * num5;

            float colorAIScale = Projectile.localAI[0] / Projectile.ai[1];
            float amount = Utils.Remap(colorAIScale, 0f, 0.6f, 0f, 1f) * Utils.Remap(colorAIScale, 0.6f, 1f, 1f, 0f);
            Color rainbow = Color.Lerp(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), new Color(255 - Main.DiscoR, 255 - Main.DiscoG, 255 - Main.DiscoB), amount);
            float num8 = Projectile.rotation + Main.rand.NextFloatDirection() * MathHelper.PiOver2 * 0.7f;
            Vector2 vector2 = Projectile.Center + num8.ToRotationVector2() * 84f * Projectile.scale;
            Vector2 vector3 = (num8 + Projectile.ai[0] * MathHelper.PiOver2).ToRotationVector2();
            if (Main.rand.NextFloat() * 2f < Projectile.Opacity)
            {
                Dust dust2 = Dust.NewDustPerfect(Projectile.Center + num8.ToRotationVector2() * (Main.rand.NextFloat() * 80f * Projectile.scale + 20f * Projectile.scale), 278, vector3 * 1f, 100, rainbow, 0.4f);
                dust2.fadeIn = 0.4f + Main.rand.NextFloat() * 0.15f;
                dust2.noGravity = true;
            }

            if (Main.rand.NextFloat() * 1.5f < Projectile.Opacity)
                Dust.NewDustPerfect(vector2, 43, vector3 * 1f, 100, rainbow * Projectile.Opacity, 1.2f * Projectile.Opacity);

            float num10 = Projectile.rotation + Main.rand.NextFloatDirection() * MathHelper.PiOver2 * 0.7f;
            Vector2 vector5 = Projectile.Center + num10.ToRotationVector2() * 84f * Projectile.scale;
            Vector2 vector6 = (num10 + Projectile.ai[0] * MathHelper.PiOver2).ToRotationVector2();
            if (Main.rand.NextFloat() < Projectile.Opacity)
            {
                Dust dust4 = Dust.NewDustPerfect(Projectile.Center + num10.ToRotationVector2() * (Main.rand.NextFloat() * 80f * Projectile.scale + 20f * Projectile.scale), 278, vector6 * 1f, 100, rainbow, 0.4f);
                dust4.fadeIn = 0.4f + Main.rand.NextFloat() * 0.15f;
                dust4.noGravity = true;
            }

            if (Main.rand.NextFloat() * 1.5f < Projectile.Opacity)
                Dust.NewDustPerfect(vector5, 43, vector6 * 1f, 100, rainbow * Projectile.Opacity, 1.2f * Projectile.Opacity);

            Projectile.scale *= Projectile.ai[2];
            if (Projectile.localAI[0] >= Projectile.ai[1])
                Projectile.Kill();

            // Enchantment visual bullshit
            Vector2 boxPosition = Projectile.position;
            int boxWidth = Projectile.width;
            int boxHeight = Projectile.height;
            for (float i = -MathHelper.PiOver4; i <= MathHelper.PiOver4; i += MathHelper.PiOver2)
            {
                Rectangle rect = Utils.CenteredRectangle(Projectile.Center + (Projectile.rotation + i).ToRotationVector2() * 70f * Projectile.scale, new Vector2(60f * Projectile.scale, 60f * Projectile.scale));
                Projectile.EmitEnchantmentVisualsAt(rect.TopLeft(), rect.Width, rect.Height);
            }
        }
        
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 vector = Projectile.Center - Main.screenPosition;
            Texture2D asset = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle rectangle = asset.Frame(1, 4);
            Vector2 origin = rectangle.Size() / 2f;
            float num = Projectile.scale * 1.1f;
            SpriteEffects effects = !(Projectile.ai[0] >= 0f) ? SpriteEffects.FlipVertically : SpriteEffects.None;
            float num2 = Projectile.localAI[0] / Projectile.ai[1];
            float num3 = Utils.Remap(num2, 0f, 0.6f, 0f, 1f) * Utils.Remap(num2, 0.6f, 1f, 1f, 0f);
            float num4 = 0.975f;
            float amount = num3;
            float fromValue = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3D);
            fromValue = Utils.Remap(fromValue, 0.2f, 1f, 0f, 1f);
            Color color = Color.Lerp(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), new Color(255 - Main.DiscoR, 255 - Main.DiscoG, 255 - Main.DiscoB), amount);
            Main.spriteBatch.Draw(asset, vector, rectangle, color * fromValue * num3, Projectile.rotation + Projectile.ai[0] * MathHelper.PiOver4 * -1f * (1f - num2), origin, num, effects, 0f);
            Color color2 = Color.Lerp(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), new Color(255 - Main.DiscoR, 255 - Main.DiscoG, 255 - Main.DiscoB), amount);
            Color color3 = Color.Lerp(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), new Color(255 - Main.DiscoR, 255 - Main.DiscoG, 255 - Main.DiscoB), amount);
            Color color4 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB) * num3 * 0.5f;
            Color color5 = color4 * fromValue * 0.5f;
            Main.spriteBatch.Draw(asset, vector, rectangle, color5 * 0.15f, Projectile.rotation + Projectile.ai[0] * 0.01f, origin, num, effects, 0f);
            Main.spriteBatch.Draw(asset, vector, rectangle, color3 * fromValue * num3 * 0.3f, Projectile.rotation, origin, num, effects, 0f);
            Main.spriteBatch.Draw(asset, vector, rectangle, color2 * fromValue * num3 * 0.5f, Projectile.rotation, origin, num * num4, effects, 0f);
            Main.spriteBatch.Draw(asset, vector, asset.Frame(1, 4, 0, 3), new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB) * 0.6f * num3, Projectile.rotation + Projectile.ai[0] * 0.01f, origin, num, effects, 0f);
            Main.spriteBatch.Draw(asset, vector, asset.Frame(1, 4, 0, 3), new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB) * 0.5f * num3, Projectile.rotation + Projectile.ai[0] * -0.05f, origin, num * 0.8f, effects, 0f);
            Main.spriteBatch.Draw(asset, vector, asset.Frame(1, 4, 0, 3), new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB) * 0.4f * num3, Projectile.rotation + Projectile.ai[0] * -0.1f, origin, num * 0.6f, effects, 0f);
            float num5 = num * 0.75f;
            Texture2D value = TextureAssets.Extra[98].Value;
            Color shineColor = color3 * Projectile.Opacity * 0.5f;
            Vector2 origin2 = value.Size() / 2f;
            float num9 = Utils.GetLerpValue(0f, 0.5f, num2, clamped: true) * Utils.GetLerpValue(1f, 0.5f, num2, clamped: true);
            Vector2 vector3 = new Vector2((Vector2.One * num5).X * 0.5f, (new Vector2(0f, Utils.Remap(num2, 0f, 1f, 3f, 0f)) * num5).X) * num9;
            Vector2 vector2 = new Vector2((Vector2.One * num5).Y * 0.5f, (new Vector2(0f, Utils.Remap(num2, 0f, 1f, 3f, 0f)) * num5).Y) * num9;
            shineColor *= num9;
            for (float num6 = 0f; num6 < 12f; num6 += 1f)
            {
                float num7 = Projectile.rotation + Projectile.ai[0] * num6 * -MathHelper.TwoPi * 0.025f + Utils.Remap(num2, 0f, 0.6f, 0f, 0.95504415f) * Projectile.ai[0];
                Vector2 drawpos = vector + num7.ToRotationVector2() * (asset.Width * 0.5f - 6f) * num;
                float num8 = num6 / 12f;

                Color color6 = new Color(255, 255, 255, 0) * num3 * num8 * 0.5f;
                color6 *= num9;
                Main.EntitySpriteDraw(value, drawpos, null, shineColor, MathHelper.PiOver2 + num7, origin2, vector3, SpriteEffects.None);
                Main.EntitySpriteDraw(value, drawpos, null, shineColor, 0f + num7, origin2, vector2, SpriteEffects.None);
                Main.EntitySpriteDraw(value, drawpos, null, color6, MathHelper.PiOver2 + num7, origin2, vector3 * 0.6f, SpriteEffects.None);
                Main.EntitySpriteDraw(value, drawpos, null, color6, 0f + num7, origin2, vector2 * 0.6f, SpriteEffects.None);
            }

            Vector2 drawpos2 = vector + (Projectile.rotation + Utils.Remap(num2, 0f, 0.6f, 0f, 0.95504415f) * Projectile.ai[0]).ToRotationVector2() * (asset.Width * 0.5f - 4f) * num;
            Color color7 = new Color(255, 255, 255, 0) * num3 * 0.5f;
            color7 *= num9;
            Main.EntitySpriteDraw(value, drawpos2, null, shineColor, MathHelper.PiOver2 + Projectile.rotation, origin2, vector3, SpriteEffects.None);
            Main.EntitySpriteDraw(value, drawpos2, null, shineColor, 0f + Projectile.rotation, origin2, vector2, SpriteEffects.None);
            Main.EntitySpriteDraw(value, drawpos2, null, color7, MathHelper.PiOver2 + Projectile.rotation, origin2, vector3 * 0.6f, SpriteEffects.None);
            Main.EntitySpriteDraw(value, drawpos2, null, color7, 0f + Projectile.rotation, origin2, vector2 * 0.6f, SpriteEffects.None);

            return false;
        }

        public override void CutTiles()
        {
            Vector2 startPoint = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 60f * Projectile.scale;
            Vector2 endPoint = (Projectile.rotation + MathHelper.PiOver4).ToRotationVector2() * 60f * Projectile.scale;
            float projectileSize = 60f * Projectile.scale;
            Utils.PlotTileLine(Projectile.Center + startPoint, Projectile.Center + endPoint, projectileSize, DelegateMethods.CutTiles);
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.HitDirectionOverride = Main.player[Projectile.owner].Center.X < target.Center.X ? 1 : -1;

            Vector2 positionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox);
            ParticleOrchestraSettings particleOrchestraSettings = default;
            particleOrchestraSettings.PositionInWorld = positionInWorld;
            ParticleOrchestraSettings settings = particleOrchestraSettings;
            switch (Main.rand.Next(5))
            {
                default:
                case 0:
                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.NightsEdge, settings, Projectile.owner);
                    break;

                case 1:
                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TrueNightsEdge, settings, Projectile.owner);
                    break;

                case 2:
                    settings.MovementVector = Projectile.velocity;
                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TerraBlade, settings, Projectile.owner);
                    break;

                case 3:
                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.Excalibur, settings, Projectile.owner);
                    break;

                case 4:
                    ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.TrueExcalibur, settings, Projectile.owner);
                    break;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float coneLength = 94f * Projectile.scale;
            float scale = MathHelper.TwoPi / 25f * Projectile.ai[0];
            float maximumAngle = MathHelper.PiOver4;
            float coneRotation = Projectile.rotation + scale;
            if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength, coneRotation, maximumAngle))
                return true;

            float rotation = Utils.Remap(Projectile.localAI[0], Projectile.ai[1] * 0.3f, Projectile.ai[1] * 0.5f, 1f, 0f);
            if (rotation > 0f)
            {
                float coneRotation2 = coneRotation - MathHelper.PiOver4 * Projectile.ai[0] * rotation;
                if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength, coneRotation2, maximumAngle))
                    return true;
            }

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int heal = Main.rand.Next(5, 12);
            if (heal > BalancingConstants.LifeStealCap)
                heal = BalancingConstants.LifeStealCap;

            if (Main.player[Main.myPlayer].lifeSteal <= 0f || heal <= 0 || target.lifeMax <= 5)
                return;

            Projectile.Owner().SpawnLifeStealProj(target, Projectile.GetSource_FromThis(), ProjectileType<StandardHealProj>(), target.Center, Vector2.Zero, heal);
        }
    }
}
