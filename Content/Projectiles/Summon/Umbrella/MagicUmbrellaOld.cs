using CalamityMod.Projectiles.Summon.Umbrella;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace CalamityInheritance.Content.Projectiles.Summon.Umbrella
{
    public class MagicUmbrellaOld : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Summon";
        private int counter = 0;
        private bool canHome = false;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 0f;
            Projectile.timeLeft = 180;
            Projectile.penetrate = 10;
            Projectile.tileCollide = false;
            Projectile.minion = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation += 0.075f;
            Projectile.alpha -= 50;
            float num633 = MagicHat.Range;
            float num634 = 1600f;
            float num635 = 2200f;
            float num636 = 150f;
            float num637 = 0.05f;
            for (int num638 = 0; num638 < Main.projectile.Length; num638++)
            {
                bool flag23 = Main.projectile[num638].type == ModContent.ProjectileType<MagicUmbrella>();
                if (num638 != Projectile.whoAmI && Main.projectile[num638].active && Main.projectile[num638].owner == Projectile.owner &&
                    flag23 && Math.Abs(Projectile.position.X - Main.projectile[num638].position.X) + Math.Abs(Projectile.position.Y - Main.projectile[num638].position.Y) < (float)Projectile.width)
                {
                    if (Projectile.position.X < Main.projectile[num638].position.X)
                    {
                        Projectile.velocity.X = Projectile.velocity.X - num637;
                    }
                    else
                    {
                        Projectile.velocity.X = Projectile.velocity.X + num637;
                    }
                    if (Projectile.position.Y < Main.projectile[num638].position.Y)
                    {
                        Projectile.velocity.Y = Projectile.velocity.Y - num637;
                    }
                    else
                    {
                        Projectile.velocity.Y = Projectile.velocity.Y + num637;
                    }
                }
            }
            if (counter <= 30)
            {
                counter++;
                canHome = true;
            }
            else
            {
                canHome = true;
            }
            bool flag24 = false;
            if (Projectile.ai[0] == 2f)
            {
                Projectile.ai[1] += 1f;
                Projectile.extraUpdates = 1;
                if (Projectile.ai[1] > 40f)
                {
                    Projectile.ai[1] = 1f;
                    Projectile.ai[0] = 0f;
                    Projectile.extraUpdates = 0;
                    Projectile.numUpdates = 0;
                    Projectile.netUpdate = true;
                }
                else
                {
                    flag24 = true;
                }
            }
            if (flag24)
            {
                return;
            }
            if (!canHome)
            {
                return;
            }
            Vector2 vector46 = Projectile.position;
            bool flag25 = false;
            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                if (npc.CanBeChasedBy(Projectile, false) && canHome)
                {
                    float num646 = Vector2.Distance(npc.Center, Projectile.Center);
                    if (!flag25 && num646 < num633)
                    {
                        num633 = num646;
                        vector46 = npc.Center;
                        flag25 = true;
                    }
                }
            }
            else
            {
                for (int num645 = 0; num645 < Main.npc.Length; num645++)
                {
                    NPC nPC2 = Main.npc[num645];
                    if (nPC2.CanBeChasedBy(Projectile, false) && canHome)
                    {
                        float num646 = Vector2.Distance(nPC2.Center, Projectile.Center);
                        if (!flag25 && num646 < num633)
                        {
                            num633 = num646;
                            vector46 = nPC2.Center;
                            flag25 = true;
                        }
                    }
                }
            }
            float num647 = num634;
            if (flag25)
            {
                num647 = num635;
            }
            if (Vector2.Distance(player.Center, Projectile.Center) > num647)
            {
                Projectile.ai[0] = 1f;
                Projectile.netUpdate = true;
            }
            if (flag25 && Projectile.ai[0] == 0f)
            {
                Vector2 vector47 = vector46 - Projectile.Center;
                float num648 = vector47.Length();
                vector47.Normalize();
                if (num648 > 200f)
                {
                    float scaleFactor2 = 9f; //8
                    vector47 *= scaleFactor2;
                    Projectile.velocity = (Projectile.velocity * 40f + vector47) / 41f;
                }
                else
                {
                    float num649 = 4f;
                    vector47 *= -num649;
                    Projectile.velocity = (Projectile.velocity * 40f + vector47) / 41f;
                }
            }
            else
            {
                bool flag26 = false;
                if (!flag26)
                {
                    flag26 = Projectile.ai[0] == 1f;
                }
                float num650 = 6f;
                if (flag26)
                {
                    num650 = 15f;
                }
                Vector2 center2 = Projectile.Center;
                Vector2 vector48 = player.Center - center2 + new Vector2(0f, -60f);
                float num651 = vector48.Length();
                if (num651 > 200f && num650 < 8f)
                {
                    num650 = 8f;
                }
                if (num651 < num636 && flag26 && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    Projectile.ai[0] = 0f;
                    Projectile.netUpdate = true;
                }
                if (num651 > 2000f)
                {
                    Projectile.position.X = player.Center.X - (float)(Projectile.width / 2);
                    Projectile.position.Y = player.Center.Y - (float)(Projectile.height / 2);
                    Projectile.netUpdate = true;
                }
                if (num651 > 70f)
                {
                    vector48.Normalize();
                    vector48 *= num650;
                    Projectile.velocity = (Projectile.velocity * 40f + vector48) / 41f;
                }
                else if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
                {
                    Projectile.velocity.X = -0.15f;
                    Projectile.velocity.Y = -0.05f;
                }
            }
            if (Projectile.ai[1] > 0f)
            {
                Projectile.ai[1] += (float)Main.rand.Next(1, 4);
            }
            if (Projectile.ai[1] > 40f)
            {
                Projectile.ai[1] = 0f;
                Projectile.netUpdate = true;
            }
            if (Projectile.ai[0] == 0f)
            {
                if (Projectile.ai[1] == 0f && flag25 && num633 < 500f)
                {
                    Projectile.ai[1] += 1f;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Projectile.ai[0] = 2f;
                        Vector2 value20 = vector46 - Projectile.Center;
                        value20.Normalize();
                        Projectile.velocity = value20 * 9f; //8
                        Projectile.netUpdate = true;
                    }
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(75, 255, 255, Projectile.alpha);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 dspeed = new Vector2(Main.rand.NextFloat(-7f, 7f), Main.rand.NextFloat(-7f, 7f));
                int dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.RainbowTorch, dspeed.X, dspeed.Y, 160, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 0.75f);
                Main.dust[dust].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(4))
            {
                for (int n = 0; n < Main.rand.Next(1, 3); n++) //1 to 2 baseball bats
                {
                    float x = target.position.X + (float)Main.rand.Next(-400, 400);
                    float y = target.position.Y - (float)Main.rand.Next(500, 800);
                    Vector2 vector = new Vector2(x, y);
                    float num13 = target.position.X + (float)(target.width / 2) - vector.X;
                    float num14 = target.position.Y + (float)(target.height / 2) - vector.Y;
                    num13 += (float)Main.rand.Next(-100, 101);
                    int num15 = 29;
                    float num16 = (float)Math.Sqrt((double)(num13 * num13 + num14 * num14));
                    num16 = (float)num15 / num16;
                    num13 *= num16;
                    num14 *= num16;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), x, y, num13, num14, ModContent.ProjectileType<MagicBatOld>(), (int)(Projectile.damage * Main.rand.NextFloat(0.3f, 0.6f)), Projectile.knockBack * Main.rand.NextFloat(0.7f, 1f), Projectile.owner, 0f, 0f);
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.rand.NextBool(4))
            {
                for (int n = 0; n < Main.rand.Next(1, 3); n++) //1 to 2 baseball bats
                {
                    float x = target.position.X + (float)Main.rand.Next(-400, 400);
                    float y = target.position.Y - (float)Main.rand.Next(500, 800);
                    Vector2 vector = new Vector2(x, y);
                    float num13 = target.position.X + (float)(target.width / 2) - vector.X;
                    float num14 = target.position.Y + (float)(target.height / 2) - vector.Y;
                    num13 += (float)Main.rand.Next(-100, 101);
                    int num15 = 29;
                    float num16 = (float)Math.Sqrt((double)(num13 * num13 + num14 * num14));
                    num16 = (float)num15 / num16;
                    num13 *= num16;
                    num14 *= num16;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), x, y, num13, num14, ModContent.ProjectileType<MagicBatOld>(), (int)(Projectile.damage * Main.rand.NextFloat(0.3f, 0.6f)), Projectile.knockBack * Main.rand.NextFloat(0.7f, 1f), Projectile.owner, 0f, 0f);
                }
            }
        }
    }
}
