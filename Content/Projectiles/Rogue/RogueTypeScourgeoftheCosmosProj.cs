using System;
using System.IO;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class RogueTypeScourgeoftheCosmosProj : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";

        private int bounce = 3;
        private int stealthBounce = 9;
        private readonly int miniStealthDevouers = 9;
        private readonly int miniStealthDevouersTilesBounce = 3 ;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 36;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
        }
        public override void SendExtraAI(BinaryWriter writer) => Projectile.DoSyncHandlerWrite(ref writer);
        public override void ReceiveExtraAI(BinaryReader reader) => Projectile.DoSyncHandlerRead(ref reader);
        public override void AI()
        {
            if (Projectile.alpha <= 200)
            {
                int inc;
                for (int i = 0; i < 2; i = inc + 1)
                {
                    int dustType = Main.rand.NextBool(3) ? 56 : 242;
                    float shortXVel = Projectile.velocity.X / 4f * i;
                    float shortYVel = Projectile.velocity.Y / 4f * i;
                    int scourgeDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 0, default, 1f);
                    Main.dust[scourgeDust].position.X = Projectile.Center.X - shortXVel;
                    Main.dust[scourgeDust].position.Y = Projectile.Center.Y - shortYVel;
                    Dust dust = Main.dust[scourgeDust];
                    dust.velocity *= 0f;
                    Main.dust[scourgeDust].scale = 0.7f;
                    inc = i;
                }
            }
            Projectile.alpha -= 50;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver4;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 180f)
            {
                Projectile.velocity.Y = Projectile.velocity.Y + 0.4f;
                Projectile.velocity.X = Projectile.velocity.X * 0.97f;
            }
            if (Projectile.velocity.Y > 16f)
                Projectile.velocity.Y = 16f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            int projectileDamage = Projectile.Calamity().stealthStrike? (int)(Projectile.damage * 0.60f) : (int)(Projectile.damage * 1.10f);
            if(Projectile.Calamity().stealthStrike == false)
            {
                if (bounce <= 0)
                    Projectile.Kill();
                else
                {

                    bounce--;
                    SoundEngine.PlaySound(SoundID.NPCHit4, Projectile.position);
                    if (Projectile.velocity.X != oldVelocity.X)
                        Projectile.velocity.X = -oldVelocity.X;
                    if (Projectile.velocity.Y != oldVelocity.Y)
                        Projectile.velocity.Y = -oldVelocity.Y;
                    if (Projectile.owner == Main.myPlayer)
                    {
                        int minisAmt = 3;
                        int inc;
                        for (int j = 0; j < minisAmt; j = inc + 1)
                        {
                            float randXDirect = Main.rand.Next(-35, 36) * 0.02f;
                            float randYDirect = Main.rand.Next(-35, 36) * 0.02f;
                            randXDirect *= 10f;
                            randYDirect *= 10f;
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, randXDirect, randYDirect, ModContent.ProjectileType<RogueTypeScourgeoftheCosmosProjMini>(), projectileDamage, Projectile.knockBack * 0.35f, Main.myPlayer, 5);
                            inc = j;
                        }
                    }
                }
                return false;
            }
            else
            {
                if (stealthBounce<= 0)
                    Projectile.Kill();
                else
                {
                    stealthBounce--;
                    SoundEngine.PlaySound(SoundID.NPCHit4, Projectile.position);
                    if (Projectile.velocity.X != oldVelocity.X)
                        Projectile.velocity.X = -oldVelocity.X;
                    if (Projectile.velocity.Y != oldVelocity.Y)
                        Projectile.velocity.Y = -oldVelocity.Y;
                    if (Projectile.owner == Main.myPlayer)
                    {
                        int minisAmt = miniStealthDevouersTilesBounce;
                        if (Main.rand.NextBool(10))
                            minisAmt++;
                        int inc;
                        for (int j = 0; j < minisAmt; j = inc + 1)
                        {
                            float randXDirect = Main.rand.Next(-35, 36) * 0.02f;
                            float randYDirect = Main.rand.Next(-35, 36) * 0.02f;
                            randXDirect *= 10f;
                            randYDirect *= 10f;
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, randXDirect, randYDirect, ModContent.ProjectileType<RogueTypeScourgeoftheCosmosProjMiniClone>(), projectileDamage, Projectile.knockBack * 0.35f, Main.myPlayer);
                            inc = j;
                        }
                    }
                }
                return false;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCHit4, Projectile.position);
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
            if (Projectile.owner == Main.myPlayer)
            {
                int minisAmt = 3;
                int projectileDamage = Projectile.Calamity().stealthStrike? (int)(Projectile.damage * 0.55f): (int)(Projectile.damage * 1.45f);
                if (Projectile.Calamity().stealthStrike)
                    minisAmt = miniStealthDevouers;
                if (Main.rand.NextBool(10))
                    minisAmt++;
                for (int j = 0; j < minisAmt; j = inc + 1)
                {
                    float randXDirect = Main.rand.Next(-35, 36) * 0.02f;
                    float randYDirect = Main.rand.Next(-35, 36) * 0.02f;
                    randXDirect *= 10f;
                    randYDirect *= 10f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, randXDirect, randYDirect, ModContent.ProjectileType<RogueTypeScourgeoftheCosmosProjMini>(), projectileDamage, Projectile.knockBack * 0.35f, Main.myPlayer);
                    inc = j;
                }
            }
        }
    }
}
