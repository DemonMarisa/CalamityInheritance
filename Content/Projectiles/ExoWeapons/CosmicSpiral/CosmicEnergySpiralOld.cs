using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.SummonBuff.Weapons;
using CalamityInheritance.Content.Items.Weapons.ExoWeapons;
using CalamityInheritance.Core.Utils;
using CalamityMod.Projectiles.Ranged;
using LAP.Core.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.ExoWeapons.CosmicSpiral
{
    public class CosmicEnergySpiralOld : CIExoProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<CosmicImmaterializerOld>();
        public bool justSpawned = true;
        public bool exoLore;
        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 78;
            Projectile.height = 78;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 10f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(exoLore);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            exoLore = reader.ReadBoolean();
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.IsLocalPlayer())
                exoLore = Projectile.Owner().CI().LoreExo;
            Lighting.AddLight((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, Main.DiscoR / 255f, Main.DiscoG / 255f, Main.DiscoB / 255f);
            bool flag64 = Projectile.type == ProjectileType<CosmicEnergySpiralOld>();
            player.AddBuff(BuffType<CosmicEnergyOld>(), 2);
            if (flag64)
            {
                if (player.dead)
                {
                    Projectile.Kill();
                }
                else
                {
                    Projectile.timeLeft = 2;
                }
            }
            float num633 = 700f;
            float num634 = 800f;
            float num635 = 1200f;
            float num636 = 1600f;
            Projectile.rotation += Projectile.velocity.X * 0.1f;
            Vector2 vector46 = Projectile.position;
            bool flag25 = false;
            int target = 0;
            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                if (npc.CanBeChasedBy(Projectile, false))
                {
                    float num646 = Vector2.Distance(npc.Center, Projectile.Center);
                    if (!flag25 && num646 < num633)
                    {
                        vector46 = npc.Center;
                        flag25 = true;
                        target = npc.whoAmI;
                    }
                }
            }
            else
            {
                for (int num645 = 0; num645 < Main.maxNPCs; num645++)
                {
                    NPC nPC2 = Main.npc[num645];
                    if (nPC2.CanBeChasedBy(Projectile, false))
                    {
                        float num646 = Vector2.Distance(nPC2.Center, Projectile.Center);
                        if (!flag25 && num646 < num633)
                        {
                            num633 = num646;
                            vector46 = nPC2.Center;
                            flag25 = true;
                            target = num645;
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
                Projectile.ai[1] = 1f;
                Projectile.netUpdate = true;
            }
            if (flag25 && Projectile.ai[1] == 0f)
            {
                Vector2 vector47 = vector46 - Projectile.Center;
                float num648 = vector47.Length();
                vector47.Normalize();
                if (num648 > 400f)
                {
                    float scaleFactor2 = 6f; //6
                    vector47 *= scaleFactor2;
                    Projectile.velocity = (Projectile.velocity * 40f + vector47) / 41f;
                }
                else
                {
                    float num649 = 4f; //4
                    vector47 *= -num649;
                    Projectile.velocity = (Projectile.velocity * 40f + vector47) / 41f;
                }
            }
            else
            {
                bool flag26 = false;
                if (!flag26)
                {
                    flag26 = Projectile.ai[1] == 1f;
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
                    Projectile.ai[1] = 0f;
                    Projectile.netUpdate = true;
                }
                if (num651 > 2000f) //2000
                {
                    Projectile.position.X = Main.player[Projectile.owner].Center.X - Projectile.width / 2;
                    Projectile.position.Y = Main.player[Projectile.owner].Center.Y - Projectile.height / 2;
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
            float num395 = Main.mouseTextColor / 200f - 0.35f;
            num395 *= 0.2f;
            Projectile.scale = num395 + 0.95f;
            if (justSpawned)
            {
                justSpawned = false;
                Projectile.ai[0] = 100f;
            }
            if (Projectile.owner == Main.myPlayer)
            {
                if (Projectile.ai[0] != 0f)
                {
                    Projectile.ai[0] -= 1f;
                    return;
                }
                float tarX = Projectile.position.X;
                float tarY = Projectile.position.Y;
                float searchDist = 2400f;
                bool getTar = false;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    if (Main.npc[i].CanBeChasedBy(Projectile, false))
                    {
                        float npcX = Main.npc[i].Center.X;
                        float npcY = Main.npc[i].Center.Y;
                        float npcDist = Math.Abs(Projectile.Center.X - npcX) + Math.Abs(Projectile.Center.Y - npcY);
                        if (npcDist < searchDist)
                        {
                            searchDist = npcDist;
                            tarX = npcX;
                            tarY = npcY;
                            getTar = true;
                        }
                    }
                }
                if (getTar)
                {
                    if (exoLore)
                    {
                        SoundEngine.PlaySound(CISounds.CosmicSpiral2, Projectile.position);
                        int blastAmt = Main.rand.Next(12, 18);
                        for (int b = 0; b < blastAmt; b++)
                        {
                            Vector2 velocity = CIUtils.RandomVelocity(10f, 7f, 10f);
                            int p2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ProjectileType<CosmicBlastExoLore>(), Projectile.damage, 2f, Projectile.owner, target, 0f);
                            if (Main.projectile.IndexInRange(p2))
                                Main.projectile[p2].originalDamage = Projectile.originalDamage;
                        }
                        float speed = 15f;
                        float distX = tarX - Projectile.Center.X;
                        float distY = tarY - Projectile.Center.Y;
                        float dist = (float)Math.Sqrt((double)(distX * distX + distY * distY));
                        dist = speed / dist;
                        distX *= dist;
                        distY *= dist;
                        int bigDamage = Projectile.damage * 2;
                        Vector2 velocity1 = CIUtils.RandomVelocity(10f, 10f, 10f);
                        Vector2 adjustedVelocity = velocity1.RotatedBy(MathHelper.ToRadians(-120));
                        int p3 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, adjustedVelocity * 1.1f, ProjectileType<CosmicBlastBigExoLore>(), bigDamage, 2, Projectile.owner, 0f, 0f);
                        if (Main.projectile.IndexInRange(p3))
                            Main.projectile[p3].originalDamage = Projectile.originalDamage;

                        Vector2 adjustedVelocity2 = velocity1.RotatedBy(MathHelper.ToRadians(120));
                        int p4 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, adjustedVelocity2 * 1.1f, ProjectileType<CosmicBlastBigExoLore>(), bigDamage, 2, Projectile.owner, 0f, 0f);
                        if (Main.projectile.IndexInRange(p4))
                            Main.projectile[p4].originalDamage = Projectile.originalDamage;

                        int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, velocity1.X * 1.1f, velocity1.Y * 1.1f, ProjectileType<CosmicBlastBigExoLore>(), bigDamage, 3f, Projectile.owner, target, 0f);
                        if (Main.projectile.IndexInRange(p))
                            Main.projectile[p].originalDamage = Projectile.originalDamage;
                        Projectile.ai[0] = 120f;
                    }
                    else
                    {
                        SoundEngine.PlaySound(SoundID.Item105, Projectile.position);
                        int blastAmt = Main.rand.Next(5, 8);
                        int bigDamage = Projectile.damage * 2;
                        for (int b = 0; b < blastAmt; b++)
                        {
                            Vector2 velocity = CIUtils.RandomVelocity(10f, 7f, 10f);
                            int p2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ProjectileType<CosmicBlastOld>(), Projectile.damage, 2f, Projectile.owner, target, 0f);
                            if (Main.projectile.IndexInRange(p2))
                                Main.projectile[p2].originalDamage = Projectile.originalDamage;
                        }
                        float speed = 15f;
                        float distX = tarX - Projectile.Center.X;
                        float distY = tarY - Projectile.Center.Y;
                        float dist = (float)Math.Sqrt((double)(distX * distX + distY * distY));
                        dist = speed / dist;
                        distX *= dist;
                        distY *= dist;
                        int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, distX, distY, ProjectileType<CosmicBlastBigOld>(), bigDamage, 3f, Projectile.owner, target, 0f);
                        if (Main.projectile.IndexInRange(p))
                            Main.projectile[p].originalDamage = Projectile.originalDamage;
                        Projectile.ai[0] = 60f;
                    }
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            if (exoLore)
            {
                Projectile.netUpdate = true;
                return new Color(255, 255, 255, 255);
            }
            else
                return new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 255);
        }

        public override bool? CanDamage() => false;

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}