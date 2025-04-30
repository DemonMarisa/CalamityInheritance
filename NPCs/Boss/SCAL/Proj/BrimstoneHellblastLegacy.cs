using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using CalamityMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using CalamityInheritance.Content.Items;

namespace CalamityInheritance.NPCs.Boss.SCAL.Proj
{
    public class BrimstoneHellblastLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Boss.Projectiles";
        public static readonly SoundStyle FireSound = new("CalamityInheritance/Sounds/Scal/HellblastFire");
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 255;
            CooldownSlot = ImmunityCooldownID.Bosses;
        }
        public int dustType = CIGlobalNPC.LegacySCalLament == -1 ? (int)CalamityDusts.Brimstone : CIDustID.DustMushroomSpray113 ;
        public override void AI()
        {

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 10)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
                Projectile.frame = 0;

            Lighting.AddLight(Projectile.Center, 0.9f * Projectile.Opacity, 0f, 0f);

            Projectile.Opacity = MathHelper.Clamp(Projectile.timeLeft / 90f, 0f, 1f);

            if (Projectile.ai[1] == 0f)
            {
                for(int i = 0; i < 15; i++)
                {
                    Vector2 sparkVelocity = Projectile.velocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.4f, 0.8f);
                    Dust.NewDustPerfect(Projectile.Center + Projectile.velocity, dustType, sparkVelocity, 0, default, Main.rand.NextFloat(1.2f, 1.5f));
                }
                Projectile.ai[1] = 1f;
                SoundEngine.PlaySound(FireSound, Projectile.position);
            }
            // 普灾发射时速度更慢
            if (Projectile.ai[2] == 0f)
                Projectile.velocity *= 1.03f;
            else
                Projectile.velocity *= 1.08f;

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
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            if (CIGlobalNPC.LegacySCalLament != -1)
                texture = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/Proj/BrimstoneHellblastLegacy_Blue").Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int drawStart = frameHeight * Projectile.frame;
            lightColor.R = (byte)(255 * Projectile.Opacity);

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, drawStart, texture.Width, frameHeight)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture.Width / 2f, frameHeight / 2f), Projectile.scale, spriteEffects, 0);
            lightColor.R = (byte)(255 * Projectile.Opacity);
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1, texture);
            return false;
        }

        public override bool CanHitPlayer(Player target) => Projectile.timeLeft >= 51;

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage <= 0 || Projectile.Opacity != 1f)
                return;

            // 用于普灾的差分
            if (Projectile.ai[2] == 0f)
                target.ScalDebuffs(180, 240, 0);
            else
                target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 90);

        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(FireSound, Projectile.position);
            for (int dust = 0; dust <= 5; dust++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, (int)CalamityDusts.Brimstone, 0f, 0f);
            }
        }
    }
}
