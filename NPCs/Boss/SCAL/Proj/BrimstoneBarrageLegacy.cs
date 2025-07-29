using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Events;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Projectiles;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod.Particles;
using CalamityInheritance.Buffs.StatDebuffs;

namespace CalamityInheritance.NPCs.Boss.SCAL.Proj
{
    public class BrimstoneBarrageLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Boss.Projectiles";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 690;
            CooldownSlot = ImmunityCooldownID.Bosses;
        }

        public override void AI()
        {
            bool bossRush = BossRushEvent.BossRushActive;

            if (Projectile.velocity.Length() < (Projectile.ai[1] == 0f ? (bossRush ? 17.5f : 14f) : (bossRush ? 12.5f : 10f)))
                Projectile.velocity *= bossRush ? 1.0125f : 1.01f;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
                Projectile.frame = 0;

            if (Projectile.timeLeft < 60)
                Projectile.Opacity = MathHelper.Clamp(Projectile.timeLeft / 60f, 0f, 1f);

            if (Projectile.ai[0] == 2f)
            {
                if (Projectile.timeLeft > 570)
                {
                    int player = Player.FindClosest(Projectile.Center, 1, 1);
                    Vector2 vector = Main.player[player].Center - Projectile.Center;
                    float scaleFactor = Projectile.velocity.Length();
                    vector.Normalize();
                    vector *= scaleFactor;
                    Projectile.velocity = (Projectile.velocity * 15f + vector) / 16f;
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= scaleFactor;
                }
            }

            Lighting.AddLight(Projectile.Center, 0.75f * Projectile.Opacity, 0f, 0f);
        }
        public override bool CanHitPlayer(Player player)
        {
            // 这一段还是复制的原灾爆改的，详情看原灾吧
            if (Projectile.Opacity != 1f)
                return false;

            bool cannotBeHurt = player.HasIFrames() || player.creativeGodMode;
            if (cannotBeHurt)
                return true;

            if (Projectile.Hitbox.Intersects(player.Hitbox))
            {
                player.AddBuff(ModContent.BuffType<VulnerabilityHexLegacy>(), 240);
                Color orbcolor = Main.rand.NextBool() ? Color.Red : Color.Lerp(Color.Red, Color.Magenta, 0.5f);
                if (CIGlobalNPC.LegacySCalLament != -1)
                    orbcolor = Main.rand.NextBool() ? Color.Blue : Color.Lerp(Color.Blue, Color.DeepSkyBlue, 0.5f);

                GlowOrbParticle orb = new GlowOrbParticle(player.Center, new Vector2(6, 6).RotatedByRandom(360) * Main.rand.NextFloat(0.3f, 1.1f), false, 60, Main.rand.NextFloat(1.55f, 3.75f), orbcolor, true, true);
                GeneralParticleHandler.SpawnParticle(orb);
                if (Main.rand.NextBool())
                {
                    GlowOrbParticle orb2 = new GlowOrbParticle(player.Center, new Vector2(6, 6).RotatedByRandom(360) * Main.rand.NextFloat(0.3f, 1.1f), false, 60, Main.rand.NextFloat(1.55f, 3.75f), Color.Black, false, true, false);
                    GeneralParticleHandler.SpawnParticle(orb2);
                }
            }
            return true;
        }
        public override void OnHitPlayer(Player player, Player.HurtInfo hurtInfo)
        {
            if (Projectile.ai[2] == 0f)
                player.ScalDebuffs(180, 240, 0);
            else
                player.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 30);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int drawStart = frameHeight * Projectile.frame;
            Color DrawColor = Color.Red;

            if (CIGlobalNPC.LegacySCalLament != -1)
            {
                DrawColor = Color.White;
                texture = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/Proj/BrimstoneBarrageLegacy_blue").Value;
            }

            DrawColor *= Projectile.Opacity;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, drawStart, texture.Width, frameHeight)),
                DrawColor, Projectile.rotation, new Vector2(texture.Width / 2f, frameHeight / 2f), Projectile.scale, spriteEffects, 0);
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], DrawColor, 1, texture);
            return false;
        }
    }
}
