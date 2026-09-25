using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.Debuffs;
using CalamityInheritance.Core.CIWorlds;
using CalamityInheritance.Core.Misc;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace CalamityInheritance.Content.NPCs.Boss.Scal.Proj
{
    public class SCalBrimstoneGigablastLegacy : CIBossProj
    {

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {

            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.Opacity = 0f;
            CooldownSlot = ImmunityCooldownID.Bosses;
        }
        public int dustType = CIBossAliveFlag.LegacySCalLament == -1 ? DustID.LifeDrain : CIDustID.DustMushroomSpray113;
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
                Projectile.Opacity = MathHelper.Clamp(1f - (Projectile.timeLeft - 60) / 60f, 0f, 1f);

            Lighting.AddLight(Projectile.Center, 0.9f * Projectile.Opacity, 0f, 0f);

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                SoundEngine.PlaySound(CISounds.HellblastFire, Projectile.position);
            }

            int target = Player.FindClosest(Projectile.Center, 1, 1);
            float projSpeed = Projectile.velocity.Length();
            Vector2 playerVec = Main.player[target].Center - Projectile.Center;
            playerVec.Normalize();
            playerVec *= projSpeed;
            Projectile.velocity = (Projectile.velocity * 24f + playerVec) / 25f;
            Projectile.velocity.Normalize();
            Projectile.velocity *= projSpeed;

            if (Projectile.ai[2] == 0f)
            {
                Projectile.ai[2] = 1f;
                for (int i = 0; i < 15; i++)
                {
                    Vector2 sparkVelocity = Projectile.velocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.8f, 1.5f);
                    Dust.NewDustPerfect(Projectile.Center + Projectile.velocity, dustType, sparkVelocity, 0, default, Main.rand.NextFloat(1.2f, 1.5f));
                }
            }

        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Request<Texture2D>(Texture).Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int drawStart = frameHeight * Projectile.frame;

            Color DrawColor = Color.Red;

            if (CIBossAliveFlag.LegacySCalLament != -1)
            {
                DrawColor = Color.White;
                texture = Request<Texture2D>($"{Texture}_Blue").Value;
            }

            DrawColor *= Projectile.Opacity;

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, drawStart, texture.Width, frameHeight)), DrawColor, Projectile.rotation, new Vector2(texture.Width / 2f, frameHeight / 2f), Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
        public override bool CanHitPlayer(Player player)
        {
            // 这一段还是复制的原灾爆改的，详情看原灾吧
            if (Projectile.Opacity != 1f)
                return false;
            return true;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage <= 0 || Projectile.Opacity != 1f)
                return;

            target.ScalDebuffs(240, 470, 90);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(CISounds.BrimstoneGigablastImpact, Projectile.Center);
            if (Projectile.owner == Main.myPlayer)
            {
                float spread = MathHelper.PiOver2 * 0.12f;
                double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 30f;
                double offsetAngle;
                for (int i = 0; i < 36; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 7f), (float)(Math.Cos(offsetAngle) * 7f), ProjectileType<BrimstoneBarrageLegacy>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 1f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 7f), (float)(-Math.Cos(offsetAngle) * 7f), ProjectileType<BrimstoneBarrageLegacy>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 1f);
                }
            }

            for (int j = 0; j < 2; j++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, CIDustID.DustLifeDrain, 0f, 0f, 50, default, 1f);
            }
            for (int k = 0; k < 20; k++)
            {
                int redFire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, CIDustID.DustLifeDrain, 0f, 0f, 0, default, 1.5f);
                Main.dust[redFire].noGravity = true;
                Main.dust[redFire].velocity *= 3f;
                redFire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, CIDustID.DustLifeDrain, 0f, 0f, 50, default, 1f);
                Main.dust[redFire].velocity *= 2f;
                Main.dust[redFire].noGravity = true;
            }
        }
    }
}
