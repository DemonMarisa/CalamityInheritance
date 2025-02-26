using System;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueTypeScourgeoftheCosmosProjMiniClone : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        private int bounce = 3;
        private int minisAmt = 2;
        public static readonly float ChasingRange = 10000f;
        public static readonly float ChasingSpeed = 20f;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.alpha = 255;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 375;
            Projectile.extraUpdates = 4;
        }

        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < 270 && target.CanBeChasedBy(Projectile);

        public override void AI()
        {
            if (Projectile.alpha > 0)
                Projectile.alpha -= 50;
            else
                Projectile.extraUpdates = 1;

            if (Projectile.alpha < 0)
                Projectile.alpha = 0;

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 1)
                Projectile.frame = 0;

            for (int i = 0; i < 1; i++)
            {
                int dustType = Main.rand.NextBool(3) ? 56 : 242;
                float dustX = Projectile.velocity.X / 3f * i;
                float dustY = Projectile.velocity.Y / 3f * i;
                int scourgeDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 0, default, 1f);
                Dust dust = Main.dust[scourgeDust];
                dust.position.X = Projectile.Center.X - dustX;
                dust.position.Y = Projectile.Center.Y - dustY;
                dust.velocity *= 0f;
                dust.scale = 0.5f;
            }

            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) - MathHelper.PiOver2;

            float projX = Projectile.position.X;
            float projY = Projectile.position.Y;
            bool isHoming = false;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] > 30f)
            {
                Projectile.ai[0] = 30f;
                for (int enemy = 0; enemy < Main.maxNPCs; enemy++)
                {
                    if (Main.npc[enemy].CanBeChasedBy(Projectile, false))
                    {
                        float enemyX = Main.npc[enemy].position.X + Main.npc[enemy].width / 2;
                        float enemyY = Main.npc[enemy].position.Y + Main.npc[enemy].height / 2;
                        float enemyDistance = Math.Abs(Projectile.position.X + Projectile.width / 2 - enemyX) + Math.Abs(Projectile.position.Y + Projectile.height / 2 - enemyY);
                        if (enemyDistance < ChasingRange  && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[enemy].position, Main.npc[enemy].width, Main.npc[enemy].height))
                        {
                            CIFunction.HomeInOnNPC(Projectile, false, ChasingRange, ChasingSpeed, 16f);
                            isHoming = true;
                        }
                    }
                }
            }
            if (!isHoming)
            {
                projX = Projectile.position.X + Projectile.width / 2 + Projectile.velocity.X * 100f;
                projY = Projectile.position.Y + Projectile.height / 2 + Projectile.velocity.Y * 100f;
            }

            float projVelModifier = 0.16f;
            Vector2 projDirection = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
            float projDirectX = projX - projDirection.X;
            float projDirectY = projY - projDirection.Y;
            float projDistance = (float)Math.Sqrt(projDirectX * projDirectX + projDirectY * projDirectY);
            projDistance = 10f / projDistance;
            projDirectX *= projDistance;
            projDirectY *= projDistance;
            if (Projectile.velocity.X < projDirectX)
            {
                Projectile.velocity.X = Projectile.velocity.X + projVelModifier;
                if (Projectile.velocity.X < 0f && projDirectX > 0f)
                    Projectile.velocity.X = Projectile.velocity.X + projVelModifier * 2f;
            }
            else if (Projectile.velocity.X > projDirectX)
            {
                Projectile.velocity.X = Projectile.velocity.X - projVelModifier;
                if (Projectile.velocity.X > 0f && projDirectX < 0f)
                    Projectile.velocity.X = Projectile.velocity.X - projVelModifier * 2f;
            }
            if (Projectile.velocity.Y < projDirectY)
            {
                Projectile.velocity.Y = Projectile.velocity.Y + projVelModifier;
                if (Projectile.velocity.Y < 0f && projDirectY > 0f)
                    Projectile.velocity.Y = Projectile.velocity.Y + projVelModifier * 2f;
            }
            else if (Projectile.velocity.Y > projDirectY)
            {
                Projectile.velocity.Y = Projectile.velocity.Y - projVelModifier;
                if (Projectile.velocity.Y > 0f && projDirectY < 0f)
                    Projectile.velocity.Y = Projectile.velocity.Y - projVelModifier * 2f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            bounce--;
            if (bounce <= 0)
                Projectile.Kill();
            else
            {
                if (Projectile.velocity.X != oldVelocity.X)
                    Projectile.velocity.X = -oldVelocity.X;
                if (Projectile.velocity.Y != oldVelocity.Y)
                    Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int framing = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
            int y6 = framing * Projectile.frame;
            Vector2 origin = new(9f, 10f);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>("CalamityMod/Projectiles/Melee/ScourgeoftheCosmosMiniGlow").Value, Projectile.Center - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, texture2D13.Width, framing)), Color.White, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
        }

        public override void OnKill(int timeLeft)
        {
            int inc;
            for (int i = 0; i < 10; i = inc + 1)
            {
                int dustType = Main.rand.NextBool(3) ? 56 : 242;
                int killedDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 0, default, 1f);
                Dust dust = Main.dust[killedDust];
                dust.scale *= 1.1f;
                Main.dust[killedDust].noGravity = true;
                inc = i;
            }
            for (int j = 0; j < 15; j = inc + 1)
            {
                int dustType = Main.rand.NextBool(3) ? 56 : 242;
                int killedDust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 0, default, 1f);
                Dust dust = Main.dust[killedDust2];
                dust.velocity *= 2.5f;
                dust = Main.dust[killedDust2];
                dust.scale *= 0.8f;
                Main.dust[killedDust2].noGravity = true;
                inc = j;
            }
            for (int j = 0; j < minisAmt; j = inc + 1)
            {
                float randXDirect = Main.rand.Next(-35, 36) * 0.02f;
                float randYDirect = Main.rand.Next(-35, 36) * 0.02f;
                randXDirect *= 10f;
                randYDirect *= 10f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, randXDirect, randYDirect, ModContent.ProjectileType<RogueTypeScourgeoftheCosmosProjMini>(), (int)(Projectile.damage * 0.45f), Projectile.knockBack * 0.35f, Main.myPlayer, 10f);
                inc = j;
            }
            base.OnKill(timeLeft);
        }
    }
}
