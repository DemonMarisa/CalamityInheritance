using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using CalamityMod.Events;
using CalamityMod.Projectiles.Boss;
using CalamityMod.World;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.NPCs.Boss.SCAL.Proj
{
    public class SCalBrimstoneGigablastLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Boss.Projectiles";
        public static readonly SoundStyle ImpactSound = new("CalamityMod/Sounds/Custom/SCalSounds/BrimstoneGigablastImpact");
        public static readonly SoundStyle FireSound = new("CalamityInheritance/Sounds/Scal/HellblastFire");

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.Calamity().DealsDefenseDamage = true;
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.Opacity = 0f;
            CooldownSlot = ImmunityCooldownID.Bosses;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 5)
                Projectile.frame = 0;

            if (Projectile.ai[1] == 1f)
                Projectile.Opacity = MathHelper.Clamp(Projectile.timeLeft / 60f, 0f, 1f);
            else
                Projectile.Opacity = MathHelper.Clamp(1f - ((Projectile.timeLeft - 60) / 60f), 0f, 1f);

            Lighting.AddLight(Projectile.Center, 0.9f * Projectile.Opacity, 0f, 0f);

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                SoundEngine.PlaySound(FireSound, Projectile.position);
            }

            int target = Player.FindClosest(Projectile.Center, 1, 1);
            float projSpeed = Projectile.velocity.Length();
            Vector2 playerVec = Main.player[target].Center - Projectile.Center;
            playerVec.Normalize();
            playerVec *= projSpeed;
            Projectile.velocity = (Projectile.velocity * 24f + playerVec) / 25f;
            Projectile.velocity.Normalize();
            Projectile.velocity *= projSpeed;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            if (CIGlobalNPC.LegacySCalLament != -1)
                texture = ModContent.Request<Texture2D>("CalamityInheritance/NPCs/Boss/SCAL/Proj/SCalBrimstoneGigablastLegacy_Blue").Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int drawStart = frameHeight * Projectile.frame;
            lightColor.R = (byte)(255 * Projectile.Opacity);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, drawStart, texture.Width, frameHeight)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture.Width / 2f, frameHeight / 2f), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override bool CanHitPlayer(Player target) => Projectile.Opacity == 1f;

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage <= 0 || Projectile.Opacity != 1f)
                return;

            target.ScalDebuffs(240, 470, 90);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(ImpactSound, Projectile.Center);
            if (Projectile.owner == Main.myPlayer)
            {
                float spread = MathHelper.PiOver2 * 0.12f;
                double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 30f;
                double offsetAngle;
                for (int i = 0; i < 36; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 7f), (float)(Math.Cos(offsetAngle) * 7f), ModContent.ProjectileType<BrimstoneBarrageLegacy>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 1f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 7f), (float)(-Math.Cos(offsetAngle) * 7f), ModContent.ProjectileType<BrimstoneBarrageLegacy>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 1f);
                }
            }

            for (int j = 0; j < 2; j++)
            {
                Dust.NewDust( Projectile.position,  Projectile.width,  Projectile.height, (int)CalamityDusts.Brimstone, 0f, 0f, 50, default, 1f);
            }
            for (int k = 0; k < 20; k++)
            {
                int redFire = Dust.NewDust( Projectile.position,  Projectile.width,  Projectile.height, (int)CalamityDusts.Brimstone, 0f, 0f, 0, default, 1.5f);
                Main.dust[redFire].noGravity = true;
                Main.dust[redFire].velocity *= 3f;
                redFire = Dust.NewDust( Projectile.position,  Projectile.width,  Projectile.height, (int)CalamityDusts.Brimstone, 0f, 0f, 50, default, 1f);
                Main.dust[redFire].velocity *= 2f;
                Main.dust[redFire].noGravity = true;
            }
        }
    }
}
