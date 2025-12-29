using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs;
using Microsoft.Xna.Framework.Graphics;
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
using CalamityInheritance.Content.Items;
using CalamityMod.Dusts;
using CalamityMod.Particles;
using CalamityMod;
using CalamityInheritance.Buffs.StatDebuffs;

namespace CalamityInheritance.NPCs.Boss.SCAL.Proj
{
    public class BrimstoneWaveLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Boss.Projectiles";
        private int x;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 30;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200;
            Projectile.Opacity = 0f;
            CooldownSlot = ImmunityCooldownID.Bosses;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(x);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            x = reader.ReadInt32();
        }
        public override void AI()
        {

            Dust dust = Dust.NewDustPerfect(Projectile.Center, Main.rand.NextBool(3) ? 60 : 114);
            dust.noGravity = true;
            dust.velocity = Projectile.velocity * Main.rand.NextFloat(0.1f, 0.7f);
            dust.scale = Main.rand.NextFloat(0.9f, 1.8f);

            x++;
            Projectile.velocity.Y = (float)(5D * Math.Sin(x / 5D));

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 12)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
                Projectile.frame = 0;

            if (Projectile.timeLeft < 30)
                Projectile.Opacity = MathHelper.Clamp(Projectile.timeLeft / 30f, 0f, 1f);
            else
                Projectile.Opacity = MathHelper.Clamp(1f - ((Projectile.timeLeft - 1170) / 30f), 0f, 1f);

            Lighting.AddLight(Projectile.Center, 0.5f * Projectile.Opacity, 0f, 0f);

            if (Projectile.velocity.X < 0f)
                Projectile.spriteDirection = 1;
            else
                Projectile.spriteDirection = -1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;


            int framing = texture.Height / Main.projFrames[Projectile.type];
            int y6 = framing * Projectile.frame;

            Color DrawColor = Color.Red;
            if (CIGlobalNPC.LegacySCalLament != -1)
            {
                DrawColor = Color.White;
                texture = Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/Proj/BrimstoneWaveLegacy_Blue").Value;
            }
            DrawColor *= Projectile.Opacity;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, texture.Width, framing)), DrawColor, Projectile.rotation, new Vector2(texture.Width / 2f, framing / 2f), Projectile.scale, spriteEffects, 0);
            return false;
        }
        public override bool CanHitPlayer(Player player)
        {
            // 这一段还是复制的原灾爆改的，详情看原灾吧
            if (Projectile.Opacity != 1f)
                return false;

            bool cannotBeHurt = player.HasIFrames() || player.creativeGodMode;
            if (cannotBeHurt)
                return true;

            if (Colliding(Projectile.Hitbox, player.Hitbox) == false)
                return false;

            if (Projectile.Hitbox.Intersects(player.Hitbox))
            {
                player.AddBuff(BuffType<VulnerabilityHexLegacy>(), 360);
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

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage <= 0 || Projectile.Opacity != 1f)
                return;

            target.ScalDebuffs(240, 360, 0);
        }
    }
}
