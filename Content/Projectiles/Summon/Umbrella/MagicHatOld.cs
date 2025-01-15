using CalamityMod.CalPlayer;
using CalamityMod.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.Buffs.Summon;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using SteelSeries.GameSense;

namespace CalamityInheritance.Content.Projectiles.Summon.Umbrella
{
    public class MagicHatOld : ModProjectile, ILocalizedModType 
    {
        public new string LocalizationCategory => "Mods.CalamityInheritance.Content.Content.Projectiles";
        public const float Range = 1500.0001f;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Magic Hat");
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 5f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            CalamityPlayer modPlayer = player.Calamity();
            CalamityGlobalProjectile modProj = Projectile.Calamity();
            CalamityInheritancePlayer modPlayer1 = player.CalamityInheritance();
            //set up minion buffs and bools
            bool hatExists = Projectile.type == ModContent.ProjectileType<MagicHatOld>();
            player.AddBuff(ModContent.BuffType<MagicHatBuffOld>(), 3600);
            if (hatExists)
            {
                if (player.dead)
                {
                    modPlayer1.MagicHatOld = false;
                }
                if (modPlayer1.MagicHatOld)
                {
                    Projectile.timeLeft = 2;
                }
            }

            //projectile movement
            Projectile.position.X = player.Center.X - (float)(Projectile.width / 2);
            Projectile.position.Y = player.Center.Y - (float)(Projectile.height / 2) + player.gfxOffY - 60f;
            if (player.gravDir == -1f)
            {
                Projectile.position.Y = Projectile.position.Y + 150f;
                Projectile.rotation = MathHelper.Pi;
            }
            else
            {
                Projectile.rotation = 0f;
            }
            Projectile.position.X = (float)(int)Projectile.position.X;
            Projectile.position.Y = (float)(int)Projectile.position.Y;

            //Change the summons scale size a little bit to make it pulse in and out
            float num395 = (float)Main.mouseTextColor / 200f - 0.35f;
            num395 *= 0.2f;
            Projectile.scale = num395 + 0.95f;

            //on summon dust and flexible damage
            if (Projectile.localAI[0] == 0f)
            {
                int dustAmt = 50;
                for (int dustIndex = 0; dustIndex < dustAmt; dustIndex++)
                {
                    int dustEffects = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 16f), Projectile.width, Projectile.height - 16, DustID.BoneTorch, 0f, 0f, 0, default, 1f);
                    Main.dust[dustEffects].velocity *= 2f;
                    Main.dust[dustEffects].scale *= 1.15f;
                }
                Projectile.localAI[0] += 1f;
            }

            //finding an enemy, then shooting projectiles at it
            if (Projectile.owner == Main.myPlayer)
            {
                float projPosX = Projectile.position.X;
                float projPosY = Projectile.position.Y;
                float detectionRange = Range;
                bool enemyDetected = false;
                if (player.HasMinionAttackTargetNPC)
                {
                    NPC npc = Main.npc[player.MinionAttackTargetNPC];
                    if (npc.CanBeChasedBy(Projectile, false))
                    {
                        float xDist = npc.position.X + (float)(npc.width / 2);
                        float yDist = npc.position.Y + (float)(npc.height / 2);
                        float enemyDist = Math.Abs(projPosX + (float)(Projectile.width / 2) - xDist) + Math.Abs(projPosY + (float)(Projectile.height / 2) - yDist);
                        if (enemyDist < detectionRange)
                        {
                            detectionRange = enemyDist;
                            projPosX = xDist;
                            projPosY = yDist;
                            enemyDetected = true;
                        }
                    }
                }
                else
                {
                    for (int index = 0; index < Main.npc.Length; index++)
                    {
                        NPC target = Main.npc[index];
                        if (target.CanBeChasedBy(Projectile, true))
                        {
                            float xDist = target.position.X + (float)(target.width / 2);
                            float yDist = target.position.Y + (float)(target.height / 2);
                            float enemyDist = Math.Abs(projPosX + (float)(Projectile.width / 2) - xDist) + Math.Abs(projPosY + (float)(Projectile.height / 2) - yDist);
                            if (enemyDist < detectionRange)
                            {
                                detectionRange = enemyDist;
                                projPosX = xDist;
                                projPosY = yDist;
                                enemyDetected = true;
                            }
                        }
                    }
                }
                if (enemyDetected)
                {
                    Projectile.ai[1] += 1f;
                    if ((Projectile.ai[1] % 5f) == 0f)
                    {
                        int amount = Main.rand.Next(1, 2);
                        for (int i = 0; i < amount; i++)
                        {
                            int projType = Utils.SelectRandom(Main.rand, new int[]
                            {
                                ModContent.ProjectileType<MagicUmbrellaOld>(),
                                ModContent.ProjectileType<MagicRifleOld>(),
                                ModContent.ProjectileType<MagicHammerOld>(),
                                ModContent.ProjectileType<MagicAxeOld>(),
                                ModContent.ProjectileType<MagicBirdOld>()
                            });
                            float velocityX = Main.rand.NextFloat(-10f, 10f);
                            float velocityY = Main.rand.NextFloat(-15f, -8f);
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.oldPosition.X + (float)(Projectile.width / 2), Projectile.oldPosition.Y + (float)(Projectile.height / 2), velocityX, velocityY, projType, Projectile.damage, 0f, Projectile.owner, 0f, 0f);
                        }
                    }
                }
            }
        }

        //glowmask effect
        public override Color? GetAlpha(Color lightColor) => new Color(200, 200, 200, 200);

        //no contact damage
        public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of true */ => false;
    }
}
