using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.NPCs;
using Terraria.ID;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using CalamityMod.Buffs.Potions;
using CalamityMod.Cooldowns;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.CalPlayer;
using CalamityMod.Projectiles.Healing;
using System;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityMod.Projectiles;
using CalamityMod.Items.Armor.Vanity;
using CalamityMod.Projectiles.Magic;
using Mono.Cecil;
using Terraria.Audio;
using CalamityInheritance.Buffs.StatDebuffs;
using Terraria.WorldBuilding;
using CalamityMod.Balancing;
using CalamityInheritance.Content.Projectiles.Magic.Ray.ElementalBeamProj;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        #region On Hit NPC With Proj
        public override void OnHitNPCWithProj(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            if (Player.whoAmI != Main.myPlayer)
                return;

            CalamityGlobalNPC cgn = target.Calamity();
            #region Lore
            if (perforatorLore)
            {
                target.AddBuff(BuffID.Ichor, 90);
            }
            if (hiveMindLore)
            {
                target.AddBuff(BuffID.CursedInferno, 90);
            }
            if (providenceLore)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 180, false);
            }
            if (holyWrath)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 180, false);
            }
            if (YharimsInsignia)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 120, false);
            }
            #endregion

            #region GodSlayer
            if (Main.player[projectile.owner].CalamityInheritance().godSlayerMagic && projectile.DamageType == DamageClass.Magic)
            {
                if (hasFiredThisFrame)
                {
                    return;
                }
                hasFiredThisFrame = true;
                Player player = Main.player[projectile.owner];
                int weaponDamage = player.HeldItem.damage;
                int finalDamage = 200 + weaponDamage / 2;

                int[] array = new int[200];
                int num3 = 0;
                int num4 = 0;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].CanBeChasedBy(projectile, false))
                    {
                        float num5 = Math.Abs(Main.npc[i].position.X + (float)(Main.npc[i].width / 2) - projectile.position.X + (float)(projectile.width / 2)) + Math.Abs(Main.npc[i].position.Y + (float)(Main.npc[i].height / 2) - projectile.position.Y + (float)(projectile.height / 2));
                        if (num5 < 800f)
                        {
                            if (Collision.CanHit(projectile.position, 1, 1, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height) && num5 > 50f)
                            {
                                array[num4] = i;
                                num4++;
                            }
                            else if (num4 == 0)
                            {
                                array[num3] = i;
                                num3++;
                            }
                        }
                    }
                }
                if (num3 == 0 && num4 == 0)
                {
                    return;
                }
                int num6;
                if (num4 > 0)
                {
                    num6 = array[Main.rand.Next(num4)];
                }
                else
                {
                    num6 = array[Main.rand.Next(num3)];
                }
                float num7 = 20f;
                float num8 = (float)Main.rand.Next(-100, 101);
                float num9 = (float)Main.rand.Next(-100, 101);
                float num10 = (float)Math.Sqrt((double)(num8 * num8 + num9 * num9));
                num10 = num7 / num10;
                num8 *= num10;
                num9 *= num10;
                Projectile.NewProjectile(null, projectile.Center.X, projectile.Center.Y, (int)num8, (int)num9, ModContent.ProjectileType<GodSlayerOrb>(),
                    (int)((double)finalDamage * (Main.player[projectile.owner].Calamity().auricSet ? 2.0 : 1.5)), 0, projectile.owner, (float)num6, 0f);
                if (target.canGhostHeal)
                {
                    float num11 = Main.player[projectile.owner].Calamity().auricSet ? 0.03f : 0.06f; //0.2
                    num11 -= (float)projectile.numHits * 0.015f; //0.05
                    if (num11 <= 0f)
                    {
                        return;
                    }
                    float num12 = (float)projectile.damage * num11;
                    if ((int)num12 <= 0)
                    {
                        return;
                    }
                    if (Main.player[Main.myPlayer].lifeSteal <= 0f)
                    {
                        return;
                    }
                    Main.player[Main.myPlayer].lifeSteal -= num12 * 1.5f;
                    float num13 = 0f;
                    int num14 = projectile.owner;
                    for (int i = 0; i < 255; i++)
                    {
                        if (Main.player[i].active && !Main.player[i].dead && ((!Main.player[projectile.owner].hostile && !Main.player[i].hostile) || Main.player[projectile.owner].team == Main.player[i].team))
                        {
                            float num15 = Math.Abs(Main.player[i].position.X + (float)(Main.player[i].width / 2) - projectile.position.X + (float)(projectile.width / 2)) + Math.Abs(Main.player[i].position.Y + (float)(Main.player[i].height / 2) - projectile.position.Y + (float)(projectile.height / 2));
                            if (num15 < 1200f && (float)(Main.player[i].statLifeMax2 - Main.player[i].statLife) > num13)
                            {
                                num13 = (float)(Main.player[i].statLifeMax2 - Main.player[i].statLife);
                                num14 = i;
                            }
                        }
                    }
                    Projectile.NewProjectile(null, projectile.Center.X, projectile.Center.Y, 0, 0, ModContent.ProjectileType<GodSlayerHealOrb>(), 0, 0, projectile.owner, (float)num14, num12);
                }
            }

            if (Main.player[projectile.owner].CalamityInheritance().godSlayerSummonold && projectile.DamageType == DamageClass.Summon)
            {
                if (hasFiredThisFrame)
                {
                    return;
                }
                hasFiredThisFrame = true;
                Player player = Main.player[projectile.owner];
                int weaponDamage = player.HeldItem.damage;
                int finalDamage = 200 + weaponDamage / 2;

                int[] array = new int[200];
                int num3 = 0;
                int num4 = 0;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].CanBeChasedBy(projectile, false))
                    {
                        float num5 = Math.Abs(Main.npc[i].position.X + (float)(Main.npc[i].width / 2) - projectile.position.X + (float)(projectile.width / 2)) + Math.Abs(Main.npc[i].position.Y + (float)(Main.npc[i].height / 2) - projectile.position.Y + (float)(projectile.height / 2));
                        if (num5 < 800f)
                        {
                            if (Collision.CanHit(projectile.position, 1, 1, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height) && num5 > 50f)
                            {
                                array[num4] = i;
                                num4++;
                            }
                            else if (num4 == 0)
                            {
                                array[num3] = i;
                                num3++;
                            }
                        }
                    }
                }
                if (num3 == 0 && num4 == 0)
                {
                    return;
                }
                int num6;
                if (num4 > 0)
                {
                    num6 = array[Main.rand.Next(num4)];
                }
                else
                {
                    num6 = array[Main.rand.Next(num3)];
                }
                float num7 = 20f;
                float num8 = (float)Main.rand.Next(-100, 101);
                float num9 = (float)Main.rand.Next(-100, 101);
                float num10 = (float)Math.Sqrt((double)(num8 * num8 + num9 * num9));
                num10 = num7 / num10;
                num8 *= num10;
                num9 *= num10;
                Projectile.NewProjectile(null, projectile.Center.X, projectile.Center.Y, num8, num9, ModContent.ProjectileType<GodSlayerPhantom>(), (int)((double)finalDamage * 2.0), 0, projectile.owner, (float)num6, 0f);
            }
            #endregion
            var source = projectile.GetSource_FromThis();
            if (silvaMageold && silvaMageCooldownold <= 0 && (projectile.penetrate == 1 || projectile.timeLeft <= 5))
            {
                silvaMageCooldownold = 300;
                SoundEngine.PlaySound(SoundID.Zombie103, projectile.Center); //So scuffed, just because zombie sounds werent ported normally
                int silvaBurstDamage = Player.ApplyArmorAccDamageBonusesTo((float)(800.0 + 0.6 * projectile.damage));
                Projectile.NewProjectile(source, projectile.Center, Vector2.Zero, ModContent.ProjectileType<SilvaBurst>(), silvaBurstDamage, 8f, Player.whoAmI);
            }

            CalamityPlayer modPlayer = Player.Calamity();
            if (projectile.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>())
            {
                titanBoost = 600;
            }
            
        #region Xeroc
        // if (Main.player[(int)Player.FindClosest(projectile.position, projectile.width, projectile.height)].GetModPlayer<>().xerocSet)
        //     {
        //         int num = projectile.damage / 2;
        //         Main.player[(int)Player.FindClosest(projectile.position, projectile.width, projectile.height)].GetModPlayer<CalamityPlayer1Point2>().xerocDmg += (float)num;
        //         int[] array = new int[200];
        //         int num3 = 0;
        //         int num4 = 0;
        //         for (int i = 0; i < 200; i++)
        //         {
        //             if (Main.npc[i].CanBeChasedBy(projectile, false))
        //             {
        //                 float num5 = Math.Abs(Main.npc[i].position.X + (float)(Main.npc[i].width / 2) - projectile.position.X + (float)(projectile.width / 2)) + Math.Abs(Main.npc[i].position.Y + (float)(Main.npc[i].height / 2) - projectile.position.Y + (float)(projectile.height / 2));
        //                 if (num5 < 800f)
        //                 {
        //                     if (Collision.CanHit(projectile.position, 1, 1, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height) && num5 > 50f)
        //                     {
        //                         array[num4] = i;
        //                         num4++;
        //                     }
        //                     else if (num4 == 0)
        //                     {
        //                         array[num3] = i;
        //                         num3++;
        //                     }
        //                 }
        //             }
        //         }
        //         if (num3 == 0 && num4 == 0)
        //         {
        //             return;
        //         }
        //         int num6;
        //         if (num4 > 0)
        //         {
        //             num6 = array[Main.rand.Next(num4)];
        //         }
        //         else
        //         {
        //             num6 = array[Main.rand.Next(num3)];
        //         }
        //         float num7 = 30f;
        //         float num8 = (float)Main.rand.Next(-100, 101);
        //         float num9 = (float)Main.rand.Next(-100, 101);
        //         float num10 = (float)Math.Sqrt((double)(num8 * num8 + num9 * num9));
        //         num10 = num7 / num10;
        //         num8 *= num10;
        //         num9 *= num10;
        //         Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, num8, num9, Mod.Find<ModProjectile>("XerocOrb").Type, num, 0f, projectile.owner, (float)num6, 0f);
        //         float num11 = 0.02f;
        //         num11 -= (float)projectile.numHits * 0.01f;
        //         if (num11 <= 0f)
        //         {
        //             return;
        //         }
        //         float num12 = (float)projectile.damage * num11;
        //         if ((int)num12 <= 0)
        //         {
        //             return;
        //         }
        //         if (Main.player[Main.myPlayer].lifeSteal <= 0f)
        //         {
        //             return;
        //         }
        //         Main.player[Main.myPlayer].lifeSteal -= num12;
        //         float num13 = 0f;
        //         int num14 = projectile.owner;
        //         for (int i = 0; i < 255; i++)
        //         {
        //             if (Main.player[i].active && !Main.player[i].dead && ((!Main.player[projectile.owner].hostile && !Main.player[i].hostile) || Main.player[projectile.owner].team == Main.player[i].team))
        //             {
        //                 float num15 = Math.Abs(Main.player[i].position.X + (float)(Main.player[i].width / 2) - projectile.position.X + (float)(projectile.width / 2)) + Math.Abs(Main.player[i].position.Y + (float)(Main.player[i].height / 2) - projectile.position.Y + (float)(projectile.height / 2));
        //                 if (num15 < 1200f && (float)(Main.player[i].statLifeMax2 - Main.player[i].statLife) > num13)
        //                 {
        //                     num13 = (float)(Main.player[i].statLifeMax2 - Main.player[i].statLife);
        //                     num14 = i;
        //                 }
        //             }
        //         }
        //         Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, 0f, 0f, Mod.Find<ModProjectile>("XerocHealOrb").Type, 0, 0f, projectile.owner, (float)num14, num12);
        //     }
        #endregion
        }
        #endregion


        #region Debuffs
        public void NPCDebuffs(NPC target, bool melee, bool ranged, bool magic, bool summon, bool rogue, bool whip, bool proj = false, bool noFlask = false)
        {
            if ((melee || rogue || whip) && !noFlask)
            {
                if (armorShattering)
                {
                    CalamityUtils.Inflict246DebuffsNPC(target, ModContent.BuffType<Crumbling>());
                }
            }

        }
        #endregion
        public override void ModifyWeaponKnockback(Item item, ref StatModifier knockback)
        {
            if (yPower)
                knockback += item.knockBack * 0.25f;
        }
        #region Lifesteal
        public void ProjLifesteal(NPC target, Projectile proj, int damage, bool crit)
        {
            CalamityGlobalProjectile modProj = proj.Calamity();

            if (Main.player[Main.myPlayer].lifeSteal > 0f && !Player.moonLeech && target.lifeMax > 5)
            {
                if (auricsilvaset)
                {
                    double healMult = 0.1;
                    healMult -= proj.numHits * healMult * 0.5;
                    int heal = (int)Math.Round(damage * healMult);
                    if (heal > 100)
                        heal = 100;

                    if (CalamityGlobalProjectile.CanSpawnLifeStealProjectile(healMult, heal))
                        CalamityGlobalProjectile.SpawnLifeStealProjectile(proj, Player, heal, ModContent.ProjectileType<SilvaOrb>(), 3000f, 2f);
                }
            }
        }
        #endregion
    }
}
