using Microsoft.Xna.Framework;
using LAP.Assets.TextureRegister;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.DraedonsArsenal
{
    public class PulseRifleShotOld : ModProjectile, ILocalizedModType
    {
        public override string Texture => LAPTextureRegister.InvisibleTexturePath;
        public new string LocalizationCategory => "Content.Projectiles.DraedonsArsenal";

        private int dust1 = 27; //purple
        private int dust2 = 173; //shortlived purple
        private int dust3 = 234; //cyan and pink
        private bool hasHit = false;
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            bool noSplitProj = Projectile.ai[1] == 0f;

            Lighting.AddLight(Projectile.Center, 0.3f, 0f, 0.5f);

            float createDustVar = 10f;
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > createDustVar)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 pPos = Projectile.position;
                    pPos -= Projectile.velocity * (i * 0.25f);
                    int d = Dust.NewDust(pPos, 1, 1, dust1, 0f, 0f, 0, default, 1f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].position = pPos;
                    Main.dust[d].scale = Main.rand.Next(70, 110) * 0.013f;

                    int d2 = Dust.NewDust(pPos, 1, 1, dust3, 0f, 0f, 0, default, 1f);
                    Main.dust[d2].noGravity = true;
                    Main.dust[d2].position = pPos;
                    Main.dust[d2].scale = Main.rand.Next(70, 110) * 0.013f;
                }

                if (noSplitProj)
                {
                    Vector2 speed = new Vector2(5f, 10f);
                    Vector2 value8 = Vector2.UnitX * -12f;

                    for (int num41 = 0; num41 < 2; num41++)
                    {
                        value8 = -Vector2.UnitY.RotatedBy(24f * 0.1308997f + 0f * MathHelper.Pi) * speed;
                        int num42 = Dust.NewDust(Projectile.Center, 0, 0, dust2, 0f, 0f, 160, default, 1f);
                        Main.dust[num42].scale = 1.5f;
                        Main.dust[num42].noGravity = true;
                        Main.dust[num42].position = Projectile.Center + value8;
                    }

                    for (int num41 = 0; num41 < 2; num41++)
                    {
                        value8 = -Vector2.UnitY.RotatedBy(24f * 0.1308997f + 1f * MathHelper.Pi) * speed;
                        int num42 = Dust.NewDust(Projectile.Center, 0, 0, dust2, 0f, 0f, 160, default, 1f);
                        Main.dust[num42].scale = 1.5f;
                        Main.dust[num42].noGravity = true;
                        Main.dust[num42].position = Projectile.Center + value8;
                    }
                }
            }

            if (Projectile.localAI[0] == createDustVar && noSplitProj)
                PulseBurst(4f, 5f);
        }

        public override bool? CanHitNPC(NPC target) => Projectile.timeLeft < 280 && target.CanBeChasedBy(Projectile);

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Projectile.ai[1] < 5f && !hasHit && Main.myPlayer == Projectile.owner)
            {
                hasHit = true;

                int hasTar;
                if (Projectile.ai[1] > 0f)
                    hasTar = (int)Projectile.ai[0];
                else
                    hasTar = target.whoAmI;

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (!Main.npc[i].CanBeChasedBy(Projectile, false) || !Collision.CanHit(Projectile.Center, 1, 1, Main.npc[i].Center, 1, 1))
                        continue;

                    if (hasTar != Main.npc[i].whoAmI && Projectile.Center.ManhattanDistance(Main.npc[i].Center) < 600f)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.SafeDirectionTo(Main.npc[i].Center) * 5f, Projectile.type, Projectile.damage / 2, 0f, Projectile.owner, Main.npc[i].whoAmI, Projectile.ai[1] + 1f);
                        break;
                    }
                }
            }

            modifiers.SourceDamage.Flat += target.lifeMax / 250;
        }

        public override void OnKill(int timeLeft)
        {
            int timesSplit = (int)Projectile.ai[1];

            int height = 40 - timesSplit * 5;
            int totalDust = 400 - timesSplit * 50;
            float speed1 = 10f - timesSplit * 1.5f;
            /*这个管理简直灾难啊
            switch (timesSplit)
            {
                case 1:
                    height = 35;
                    totalDust = 350;
                    speed1 = 8.5f;
                    break;
                case 2:
                    height = 30;
                    totalDust = 300;
                    speed1 = 7f;
                    break;
                case 3:
                    height = 25;
                    totalDust = 250;
                    speed1 = 5.5f;
                    break;
                case 4:
                    height = 20;
                    totalDust = 200;
                    speed1 = 4f;
                    break;
                case 5:
                    height = 15;
                    totalDust = 150;
                    speed1 = 2.5f;
                    break;
                default:
                    break;
            }
            */
            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = height;
            Projectile.Center = Projectile.position;
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();

            SoundEngine.PlaySound(SoundID.Item93, Projectile.Center);

            int fourth = totalDust / 4;
            int half = totalDust / 2;
            int x = (int)(totalDust * 0.625f);

            for (int i = 0; i < totalDust; i++)
            {
                float dSpeed = 2f * (i / (float)fourth);
                int dustType = dust1;
                if (i > fourth)
                {
                    dSpeed = speed1;
                }
                if (i > x)
                {
                    dSpeed = speed1 * 1.3f;
                    dustType = dust3;
                }

                int d = Dust.NewDust(Projectile.Center, 6, 6, dustType, 0f, 0f, 100, default, 1f);
                float dX = Main.dust[d].velocity.X;
                float dY = Main.dust[d].velocity.Y;
                if (dX == 0f && dY == 0f)
                {
                    dX = 1f;
                }

                float dDist = (float)Math.Sqrt(dX * dX + dY * dY);
                dDist = dSpeed / dDist;
                if (i <= half)
                {
                    dX *= dDist;
                    dY *= dDist;
                }
                else
                {
                    dX = dX * dDist * 1.25f;
                    dY = dY * dDist * 0.75f;
                }

                Dust dust2 = Main.dust[d];
                dust2.velocity *= 0.5f;
                dust2.velocity.X = dust2.velocity.X + dX;
                dust2.velocity.Y = dust2.velocity.Y + dY;

                if (i > fourth)
                {
                    dust2.scale = 1.3f;
                }

                dust2.noGravity = true;
            }
        }

        private void PulseBurst(float speed1, float speed2)
        {
            float angleRandom = 0.05f;

            for (int i = 0; i < 50; i++)
            {
                float dustSpeed = Main.rand.NextFloat(speed1, speed2);
                Vector2 dustVel = new Vector2(dustSpeed, 0.0f).RotatedBy(Projectile.velocity.ToRotation());
                dustVel = dustVel.RotatedBy(-angleRandom);
                dustVel = dustVel.RotatedByRandom(2.0f * angleRandom);

                int d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dust3, dustVel.X, dustVel.Y, 200, default, 1.7f);
                Main.dust[d].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                Main.dust[d].noGravity = true;

                Dust dust = Main.dust[d];
                dust.velocity *= 3f;
                dust = Main.dust[d];

                d = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dust1, dustVel.X, dustVel.Y, 100, default, 1f);
                Main.dust[d].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * Projectile.width / 2f;

                dust = Main.dust[d];
                dust.velocity *= 2f;

                Main.dust[d].noGravity = true;
                Main.dust[d].fadeIn = 1f;
                Main.dust[d].color = Color.Green * 0.5f;

                dust = Main.dust[d];
            }
            for (int j = 0; j < 25; j++)
            {
                float dustSpeed = Main.rand.NextFloat(speed1, speed2);
                Vector2 dustVel = new Vector2(dustSpeed, 0.0f).RotatedBy(Projectile.velocity.ToRotation());
                dustVel = dustVel.RotatedBy(-angleRandom);
                dustVel = dustVel.RotatedByRandom(2.0f * angleRandom);

                int d2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dust2, dustVel.X, dustVel.Y, 0, default, 3f);
                Main.dust[d2].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(MathHelper.Pi).RotatedBy(Projectile.velocity.ToRotation()) * Projectile.width / 3f;
                Main.dust[d2].noGravity = true;

                Dust dust = Main.dust[d2];
                dust.velocity *= 0.5f;
                dust = Main.dust[d2];
            }
        }
    }
}
