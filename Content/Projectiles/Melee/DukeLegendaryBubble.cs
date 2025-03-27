using System;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class DukeLegendaryBubble: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 255;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 5;
        }

        public override void AI()
        {
            if (Projectile.ai[1] > 0f)
            {
                int pTracking = (int)Projectile.ai[1] - 1;
                if (pTracking < 255)
                {
                    Projectile.localAI[0] += 1f;
                    if (Projectile.localAI[0] > 10f)
                    {
                        int dAmount = 6;
                        for (int i = 0; i < dAmount; i++)
                        {
                            Vector2 dRot = Vector2.Normalize(Projectile.velocity) * new Vector2(Projectile.width / 2f, Projectile.height) * 0.75f;
                            dRot = dRot.RotatedBy((i - (dAmount / 2 - 1)) * MathHelper.Pi/ (double)(float)dAmount, default) + Projectile.Center;
                            Vector2 faceDirection = ((float)(Main.rand.NextDouble() * MathHelper.Pi) - MathHelper.PiOver2).ToRotationVector2() * Main.rand.Next(3, 8);
                            int d = Dust.NewDust(dRot + faceDirection, 0, 0, DustID.Flare_Blue, faceDirection.X * 2f, faceDirection.Y * 2f, 100, new Color(53, Main.DiscoG, 255), 1.4f);
                            Main.dust[d].noGravity = true;
                            Main.dust[d].noLight = true;
                            Main.dust[d].velocity /= 4f;
                            Main.dust[d].velocity -= Projectile.velocity;
                        }
                        Projectile.alpha -= 5;
                        if (Projectile.alpha < 100)
                        {
                            Projectile.alpha = 100;
                        }
                        Projectile.rotation += Projectile.velocity.X * 0.1f;
                        Projectile.frame = (int)(Projectile.localAI[0] / 3f) % 3;
                    }
                    Vector2 pDir = Main.player[pTracking].Center - Projectile.Center;
                    float pVel = 4f;
                    pVel += Projectile.localAI[0] / 20f;
                    Projectile.velocity = Vector2.Normalize(pDir) * pVel;
                    if (pDir.Length() < 50f)
                    {
                        Projectile.Kill();
                    }
                }
            }
            else
            {
                float sAway = 0.21f;
                float sSpwan = (float)(Math.Cos((double)(sAway * Projectile.ai[0])) - 0.5) * 4f;
                Projectile.velocity.Y = Projectile.velocity.Y - sSpwan;
                Projectile.ai[0] += 1f;
                sSpwan = (float)(Math.Cos((double)(sAway * Projectile.ai[0])) - 0.5) * 4f;
                Projectile.velocity.Y = Projectile.velocity.Y + sSpwan;
                Projectile.localAI[0] += 1f;
                if (Projectile.localAI[0] > 10f)
                {
                    Projectile.alpha -= 5;
                    if (Projectile.alpha < 100)
                    {
                        Projectile.alpha = 100;
                    }
                    Projectile.rotation += Projectile.velocity.X * 0.1f;
                    Projectile.frame = (int)(Projectile.localAI[0] / 3f) % 3;
                }
            }
            if (Projectile.wet)
            {
                Projectile.position.Y = Projectile.position.Y - 16f;
                Projectile.Kill();
                return;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item96, Projectile.Center);
            int dMore = 36;
            for (int j = 0; j < dMore; j++)
            {
                Vector2 rot = Vector2.Normalize(Projectile.velocity) * new Vector2(Projectile.width / 2f, Projectile.height) * 0.75f;
                rot = rot.RotatedBy((double)((j - (dMore / 2 - 1)) * MathHelper.TwoPi / dMore), default) + Projectile.Center;
                Vector2 dir = rot - Projectile.Center;
                int kD = Dust.NewDust(rot + dir, 0, 0, DustID.Flare_Blue, dir.X * 2f, dir.Y * 2f, 100, new Color(53, Main.DiscoG, 255), 1.4f);
                Main.dust[kD].noGravity = true;
                Main.dust[kD].noLight = true;
                Main.dust[kD].velocity = dir;
            }
            if (Projectile.owner == Main.myPlayer)
            {
                int pTileX = (int)(Projectile.Center.Y / 16f);
                int pTileY = (int)(Projectile.Center.X / 16f);
                int pModify = 100;
                if (pTileY < 10)
                {
                    pTileY = 10;
                }
                if (pTileY > Main.maxTilesX - 10)
                {
                    pTileY = Main.maxTilesX - 10;
                }
                if (pTileX < 10)
                {
                    pTileX = 10;
                }
                if (pTileX > Main.maxTilesY - pModify - 10)
                {
                    pTileX = Main.maxTilesY - pModify - 10;
                }
                for (int k = pTileX; k < pTileX + pModify; k++)
                {
                    Tile t = Main.tile[pTileY, k];
                    if (t.HasTile && (Main.tileSolid[t.TileType] || t.LiquidAmount != 0))
                    {
                        pTileX = k;
                        break;
                    }
                }
                int s = Projectile.NewProjectile(Projectile.GetSource_FromThis(), pTileY * 16 + 8, pTileX * 16 - 32, 0f, 0f, ModContent.ProjectileType<BrinySpout>(), Projectile.damage / 3, 6f, Main.myPlayer, 3f, 7f); //First overload seems to deal with timing, second is segment amount
                Main.projectile[s].netUpdate = true;
            }
        }
    }
}
