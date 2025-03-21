using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class VictideSpearWaterRing: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.scale *= 0.75f;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 180;
        }
        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft <= 120;
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0f, Projectile.scale * 1.5f);
            int dCounts = 6;
            for (int i = 0; i < dCounts; i++)
            {
                Vector2 dPosition = Vector2.Normalize(Projectile.velocity) * new Vector2(Projectile.width / 2f, Projectile.height) * 0.75f;
                dPosition = dPosition.RotatedBy((i - (dCounts / 2 - 1)) * MathHelper.Pi/ (double)(float)dCounts, default) + Projectile.Center;
                Vector2 dVelocity = ((float)(Main.rand.NextDouble() * MathHelper.Pi) - MathHelper.PiOver2).ToRotationVector2() * Main.rand.Next(3, 8);
                int d = Dust.NewDust(dPosition + dVelocity, 0, 0, DustID.DungeonWater, dVelocity.X * 2f, dVelocity.Y * 2f, 100, default, Projectile.scale);
                Main.dust[d].noGravity = true;
                Main.dust[d].noLight = true;
                Main.dust[d].velocity /= 4f;
                Main.dust[d].velocity -= Projectile.velocity;
            }
            Projectile.ai[0] += 1f;
            //弱追踪
            if (Projectile.ai[0] > 40f)
            {
                Projectile.tileCollide = true;
                CIFunction.HomeInOnNPC(Projectile, false, 200f, 12f, 20f);
            }
            //发起追踪前这个玩意无视墙体
            else Projectile.tileCollide = false;
            CIFunction.FramesChanger(Projectile, 4, 2);
            Projectile.rotation += 0.08f ;
        }

        // public override bool PreDraw(ref Color lightColor)
        // {
        //     Texture2D texture2D13 = TextureAssets.Projectile[Projectile.type].Value;
        //     int p = TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
        //     int y6 = p * Projectile.frame;
        //     Main.spriteBatch.Draw(texture2D13, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle?(new Rectangle(0, y6, texture2D13.Width, p)), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture2D13.Width / 2f, p / 2f), Projectile.scale, SpriteEffects.None, 0f);
        //     return false;
        // }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath19, Projectile.Center);
            int dCounts = 36;
            for (int i = 0; i < dCounts; i++)
            {
                Vector2 dPos = Vector2.Normalize(Projectile.velocity) * new Vector2(Projectile.width / 2f, Projectile.height) * 0.75f;
                dPos = dPos.RotatedBy((double)((i - (dCounts / 2 - 1)) * MathHelper.TwoPi / dCounts), default) + Projectile.Center;
                Vector2 dVel = dPos - Projectile.Center;
                int d = Dust.NewDust(dPos + dVel, 0, 0, DustID.DungeonWater, dVel.X * 1.5f, dVel.Y * 1.5f, 100, default, 1.4f);
                Main.dust[d].noGravity = true;
                Main.dust[d].noLight = true;
                Main.dust[d].velocity = dVel / 2;
            }
        }
    }
}
