using CalamityMod;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.NPCs;
using Terraria.ID;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.CalPlayer;
using CalamityMod.Projectiles.Healing;
using System;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityMod.Projectiles;
using CalamityMod.Projectiles.Magic;
using Terraria.Audio;
using CalamityInheritance.Buffs.Mage;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Buffs.Statbuffs;
using CalamityMod.Items.Accessories;

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
                        float num5 = Math.Abs(Main.npc[i].position.X + Main.npc[i].width / 2 - projectile.position.X + projectile.width / 2) + Math.Abs(Main.npc[i].position.Y + Main.npc[i].height / 2 - projectile.position.Y + projectile.height / 2);
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
                float num8 = Main.rand.Next(-100, 101);
                float num9 = Main.rand.Next(-100, 101);
                float num10 = (float)Math.Sqrt((double)(num8 * num8 + num9 * num9));
                num10 = num7 / num10;
                num8 *= num10;
                num9 *= num10;
                Projectile.NewProjectile(null, projectile.Center.X, projectile.Center.Y, (int)num8, (int)num9, ModContent.ProjectileType<GodSlayerOrb>(),
                    (int)(finalDamage * (Main.player[projectile.owner].Calamity().auricSet ? 2.0 : 1.5)), 0, projectile.owner, num6, 0f);
                if (target.canGhostHeal)
                {
                    float num11 = Main.player[projectile.owner].Calamity().auricSet ? 0.03f : 0.06f; //0.2
                    num11 -= projectile.numHits * 0.015f; //0.05
                    if (num11 <= 0f)
                    {
                        return;
                    }
                    float num12 = projectile.damage * num11;
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
                            float num15 = Math.Abs(Main.player[i].position.X + Main.player[i].width / 2 - projectile.position.X + projectile.width / 2) + Math.Abs(Main.player[i].position.Y + Main.player[i].height / 2 - projectile.position.Y + projectile.height / 2);
                            if (num15 < 1200f && Main.player[i].statLifeMax2 - Main.player[i].statLife > num13)
                            {
                                num13 = Main.player[i].statLifeMax2 - Main.player[i].statLife;
                                num14 = i;
                            }
                        }
                    }
                    Projectile.NewProjectile(null, projectile.Center.X, projectile.Center.Y, 0, 0, ModContent.ProjectileType<GodSlayerHealOrb>(), 0, 0, projectile.owner, num14, num12);
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
                        float num5 = Math.Abs(Main.npc[i].position.X + Main.npc[i].width / 2 - projectile.position.X + projectile.width / 2) + Math.Abs(Main.npc[i].position.Y + Main.npc[i].height / 2 - projectile.position.Y + projectile.height / 2);
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
                float num8 = Main.rand.Next(-100, 101);
                float num9 = Main.rand.Next(-100, 101);
                float num10 = (float)Math.Sqrt((double)(num8 * num8 + num9 * num9));
                num10 = num7 / num10;
                num8 *= num10;
                num9 *= num10;
                Projectile.NewProjectile(null, projectile.Center.X, projectile.Center.Y, num8, num9, ModContent.ProjectileType<GodSlayerPhantom>(), (int)(finalDamage * 2.0), 0, projectile.owner, num6, 0f);
            }
            #endregion
            var source = projectile.GetSource_FromThis();
            if (silvaMageold && silvaMageCooldownold <= 0 && (projectile.penetrate == 1 || projectile.timeLeft <= 5) && projectile.DamageType == DamageClass.Magic)
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
            #region ReaverSets
            #region ReaverMage
            //法师永恒套的套装效果

            CalamityInheritancePlayer reaverMagePlayer = Main.player[projectile.owner].GetModPlayer<CalamityInheritancePlayer>();
            var reaverMage = projectile.GetSource_FromThis();
            if (Main.player[projectile.owner].CalamityInheritance().reaverMageBurst)
            {
                if (reaverMageBurst) //击发时提供法术增强buff
                {
                    Player.AddBuff(ModContent.BuffType<ReaverMagePower>(), 180);
                }

                if (reaverBurstCooldown <= 0)
                {
                    int[] projectileTypes = { ModContent.ProjectileType<CISporeGas>(), ModContent.ProjectileType<CISporeGas2>(), ModContent.ProjectileType<CISporeGas3>() };
                    float baseAngleIncrement = 2 * MathHelper.Pi / 16;
                    float randomAngleOffset = (float)(Main.rand.NextDouble() * MathHelper.Pi / 4 - MathHelper.Pi / 8);
                    //好像这样伤害还挺低的但我也不知道该不该调整了
                    int newDamage = Player.ApplyArmorAccDamageBonusesTo(CalamityUtils.DamageSoftCap(15 + 0.15 * projectile.damage, 30));

                    for (int sporecounts = 0; sporecounts < 16; sporecounts++)
                    {
                        float angle = sporecounts * baseAngleIncrement + randomAngleOffset;
                        Vector2 direction = new((float)Math.Cos(angle), (float)Math.Sin(angle));
                        int randomProjectileType = projectileTypes[Main.rand.Next(projectileTypes.Length)];
                        float randomSpeed = Main.rand.NextFloat(2f, 4f);
                        Projectile.NewProjectile(source, target.Center, direction * randomSpeed, randomProjectileType, newDamage, projectile.knockBack);
                    }
                    target.AddBuff(BuffID.Poisoned, 120);
                    reaverBurstCooldown = 90;
                }
            }
            #endregion
            #region ReaverMelee
            //永恒套的近战爆炸攻击
            var meleeReaverSrc = projectile.GetSource_FromThis();
            if (Main.player[projectile.owner].CalamityInheritance().reaverMeleeBlast && projectile.DamageType == DamageClass.Melee)
            {
                int BlastDamage = (int)(projectile.damage * 0.4);
                if (BlastDamage > 30)
                {
                    BlastDamage = 30;
                }
                if (reaverBlastCooldown <= 0)
                {
                    SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, projectile.Center);
                    Projectile.NewProjectile(meleeReaverSrc, projectile.Center, Vector2.Zero, ModContent.ProjectileType<ReaverBlast>(),
                                            BlastDamage, 0.15f, Player.whoAmI);
                    reaverBlastCooldown = 10;
                }
            }
            #endregion
            #endregion
            if (!projectile.npcProj && !projectile.trap && projectile.friendly)
            {
                ProjLifesteal(target, projectile, damageDone, hit.Crit);
                ProjOnHit(projectile, target.Center, hit.Crit, target.IsAnEnemy(false));
            }
            #region AuricYharim
            if (auricYharimSet)
            {
                if (projectile.DamageType == ModContent.GetInstance<RogueDamageClass>()
                    && Main.projectile[projectile.owner].Calamity().stealthStrike
                    && yharimOfPerunStrikesCooldown == 0
                    ) 
                {
                        SoundEngine.PlaySound(SoundMenu.auricYharimDeadlyStrikes);
                        for (int j = 0; j < 50; j++)
                        {
                            int nebulousReviveDust = Dust.NewDust(Player.position, Player.width, Player.height, DustID.ShadowbeamStaff, 0f, 0f, 100, default, 2f);
                            Dust dust = Main.dust[nebulousReviveDust];
                            dust.position.X += Main.rand.Next(-20, 21);
                            dust.position.Y += Main.rand.Next(-20, 21);
                            dust.velocity *= 0.9f;
                            dust.scale *= 1f + Main.rand.Next(40) * 0.01f;
                            if (Main.rand.NextBool())
                                dust.scale *= 1f + Main.rand.Next(40) * 0.01f;
                        }
                        Player.AddBuff(ModContent.BuffType<yharimOfPerun>(), 1800);
                        yharimOfPerunStrikesCooldown = 1800;

                }
             }
        }
        #endregion
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
        private void ProjLifesteal(NPC target, Projectile proj, int damage, bool crit)
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
        #region Proj On Hit
        public void ProjOnHit(Projectile proj, Vector2 position, bool crit, bool npcCheck)
        {
            CalamityGlobalProjectile modProj = proj.Calamity();

            if (proj.CountsAsClass<ThrowingDamageClass>())
                RogueOnHit(proj, modProj, position, crit, npcCheck);
        }
        #endregion
        #region Rogue
        private void RogueOnHit(Projectile proj, CalamityGlobalProjectile modProj, Vector2 position, bool crit, bool npcCheck)
        {
            var spawnSource = proj.GetSource_FromThis();

            CalamityPlayer modPlayer = Player.Calamity();

            if (proj.Calamity().stealthStrike && proj.Calamity().stealthStrikeHitCount < 3)
            {
                if (nanotechold)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Vector2 source = new Vector2(position.X + Main.rand.Next(-201, 201), Main.screenPosition.Y - 600f - Main.rand.Next(50));
                        Vector2 velocity = (position - source) / 40f;

                        Projectile.NewProjectile(spawnSource, source, velocity, ModContent.ProjectileType<NanoFlare>(), (int)(proj.damage * 0.05), 3f, proj.owner);
                    }
                }
            }
        }
        #endregion
    }
}
