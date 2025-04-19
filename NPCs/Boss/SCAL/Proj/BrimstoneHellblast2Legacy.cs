using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Events;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.NPCs.Boss.SCAL.Proj
{
    public class BrimstoneHellblast2Legacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Boss.Projectiles";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.Calamity().DealsDefenseDamage = true;
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1500;
            Projectile.Opacity = 0f;
            CooldownSlot = ImmunityCooldownID.Bosses;
        }

        public override void AI()
        {
            // Cal Clone bullet hell projectiles accelerate after a certain time has passed
            if (Projectile.ai[0] == 2f && Main.expertMode && Projectile.timeLeft < 1260)
            {
                if (Projectile.velocity.Length() < (BossRushEvent.BossRushActive ? 15f : 10f))
                    Projectile.velocity *= 1.005f;
            }

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 10)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
                Projectile.frame = 0;

            if (Projectile.timeLeft < 60)
                Projectile.Opacity = MathHelper.Clamp(Projectile.timeLeft / 60f, 0f, 1f);
            else
                Projectile.Opacity = MathHelper.Clamp(1f - ((Projectile.timeLeft - 1440) / 60f), 0f, 1f);

            Lighting.AddLight(Projectile.Center, 0.9f * Projectile.Opacity, 0f, 0f);

            if (Projectile.velocity.X < 0f)
            {
                Projectile.spriteDirection = -1;
                Projectile.rotation = (float)Math.Atan2(-Projectile.velocity.Y, -Projectile.velocity.X);
            }
            else
            {
                Projectile.spriteDirection = 1;
                Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            if (CIGlobalNPC.LegacySCalLament != -1)
                texture = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/Proj/BrimstoneHellblast2Legacy_Blue").Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int drawStart = frameHeight * Projectile.frame;
            lightColor.R = (byte)(255 * Projectile.Opacity);

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, drawStart, texture.Width, frameHeight)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture.Width / 2f, frameHeight / 2f), Projectile.scale, spriteEffects, 0);
            return false;
        }

        public override bool CanHitPlayer(Player target) => Projectile.Opacity == 1f;

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage <= 0 || Projectile.Opacity != 1f)
                return;

            target.ScalDebuffs(180, 240, 0);
        }
    }
}
