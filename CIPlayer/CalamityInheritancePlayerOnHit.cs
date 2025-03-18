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
using CalamityMod.Projectiles.Summon;
using System.Collections.Generic;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityMod.NPCs.Yharon;
using CalamityInheritance.Content.Items.MiscItem;
using CalamityInheritance.Content.Items.Potions;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        #region On Hit NPC With Item
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Item, consider using OnHitNPC instead */
        {
            if (Player.whoAmI != Main.myPlayer)
                return;
            NPCDebuffs(target, item.CountsAsClass<MeleeDamageClass>(), item.CountsAsClass<RangedDamageClass>(), item.CountsAsClass<MagicDamageClass>(), item.CountsAsClass<SummonDamageClass>(), item.CountsAsClass<ThrowingDamageClass>(), item.CountsAsClass<SummonMeleeSpeedDamageClass>());
        }
        #endregion

        public override void OnHitNPCWithProj(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            CalamityPlayer modPlayer = Player.Calamity();
            CalamityGlobalNPC cgn = target.Calamity();
            Player player = Main.player[projectile.owner];
            if (Player.whoAmI != Main.myPlayer)
                return;
            if (projectile.type == ModContent.ProjectileType<PolarStarLegacy>())
            {
                PolarisBoostCounter += 1;
            }
            #region Lore
            if (LorePerforator)
            {
                target.AddBuff(BuffID.Ichor, 90);
            }
            if (LoreHive)
            {
                target.AddBuff(BuffID.CursedInferno, 90);
            }
            if (LoreProvidence)
            {
                target.AddBuff(ModContent.BuffType<HolyInferno>(), 180, false);
            }
            if (BuffStatsHolyWrath)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 180, false);
            }
            if (YharimsInsignia)
            {
                target.AddBuff(ModContent.BuffType<HolyFlames>(), 120, false);
            }
            if (BuffStatsDraconicSurge && Main.zenithWorld)
            {
                target.AddBuff(ModContent.BuffType<Dragonfire>(), 360, false);
            }
            #endregion
            #region Armorset
            #region GodSlayer
            if (GodSlayerMagicSet && projectile.DamageType == DamageClass.Magic)
            {
                if (hasFiredThisFrame)
                {
                    return;
                }
                hasFiredThisFrame = true;
                int weaponDamage = player.HeldItem.damage;
                int finalDamage = 200 + weaponDamage / 2;

                int projectileTypes = ModContent.ProjectileType<GodSlayerOrb>();
                float randomAngleOffset = (float)(Main.rand.NextDouble() * 2 * MathHelper.Pi);
                Vector2 direction = new((float)Math.Cos(randomAngleOffset), (float)Math.Sin(randomAngleOffset));
                float randomSpeed = Main.rand.NextFloat(12f, 16f);
                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, direction * randomSpeed, projectileTypes, finalDamage, projectile.knockBack);
            }

            if (GodSlayerSummonSet && projectile.DamageType == DamageClass.Summon)
            {
                if (hasFiredThisFrame)
                {
                    return;
                }
                hasFiredThisFrame = true;
                player = Main.player[projectile.owner];
                int weaponDamage = player.HeldItem.damage;
                int finalDamage = 200 + weaponDamage / 2;

                int projectileTypes = ModContent.ProjectileType<GodSlayerPhantom>();
                float randomAngleOffset = (float)(Main.rand.NextDouble() * 2 * MathHelper.Pi);
                Vector2 direction = new((float)Math.Cos(randomAngleOffset), (float)Math.Sin(randomAngleOffset));
                float randomSpeed = Main.rand.NextFloat(6f, 8f);
                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, direction * randomSpeed, projectileTypes, finalDamage, projectile.knockBack);
            }
            #endregion

            var source = projectile.GetSource_FromThis();
            if (SilvaMagicSetLegacy && SilvaMagicSetLegacyCooldown <= 0 && (projectile.penetrate == 1 || projectile.timeLeft <= 5) && projectile.DamageType == DamageClass.Magic)
            {
                SilvaMagicSetLegacyCooldown = 300;
                SoundEngine.PlaySound(SoundID.Zombie103, projectile.Center); //So scuffed, just because zombie sounds werent ported normally
                int silvaBurstDamage = Player.ApplyArmorAccDamageBonusesTo((float)(800.0 + 0.6 * projectile.damage));
                Projectile.NewProjectile(source, projectile.Center, Vector2.Zero, ModContent.ProjectileType<SilvaBurst>(), silvaBurstDamage, 8f, Player.whoAmI);
            }

            if (projectile.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>() || projectile.type == ModContent.ProjectileType<StepToolShadowChair>())
            {
                BuffStatsTitanScaleTrueMelee = 600;
            }
            #region ReaverSets
            #region ReaverMage
            //法师永恒套的套装效果

            CalamityInheritancePlayer ReaverMagePlayer = Main.player[projectile.owner].GetModPlayer<CalamityInheritancePlayer>();
            var ReaverMage = projectile.GetSource_FromThis();
            if (Main.player[projectile.owner].CIMod().ReaverMageBurst)
            {
                if (ReaverMageBurst) //击发时提供法术增强buff
                {
                    Player.AddBuff(ModContent.BuffType<ReaverMagePower>(), 180);
                }

                if (ReaverBurstCooldown <= 0)
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
                    ReaverBurstCooldown = 90;
                }
            }
            #endregion
            #region ReaverMelee
            //永恒套的近战爆炸攻击
            var meleeReaverSrc = projectile.GetSource_FromThis();
            if (Main.player[projectile.owner].CIMod().ReaverMeleeBlast && projectile.DamageType == DamageClass.Melee)
            {
                int BlastDamage = (int)(projectile.damage * 0.4);
                if (BlastDamage > 30)
                {
                    BlastDamage = 30;
                }
                if (ReaverBlastCooldown <= 0)
                {
                    SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, projectile.Center);
                    Projectile.NewProjectile(meleeReaverSrc, projectile.Center, Vector2.Zero, ModContent.ProjectileType<ReaverBlast>(),
                                            BlastDamage, 0.15f, Player.whoAmI);
                    ReaverBlastCooldown = 10;
                }
            }
            #endregion
            #endregion
            if (!projectile.npcProj && !projectile.trap && projectile.friendly)
            {
                ProjLifesteal(target, projectile, damageDone, hit.Crit);
                ProjOnHit(projectile, target.Center, hit.Crit, target.IsAnEnemy(false));
            }
            #region AncientArmor
            if (AncientBloodflareSet && hit.Damage > 300 && target.IsAnEnemy(false) && target.lifeMax > 5 && AncientBloodflareHeartDropCD == 0) //大于300伤害才能产出红心与魔力星 
            {
                int amt = Main.rand.Next(2, 5); //2->4
                if(Main.rand.NextBool(6)) //每次攻击时1/6概率
                {
                    for(int i = 0; i < amt; i++)
                    {
                        Item.NewItem(target.GetSource_FromThis(), target.Hitbox, ItemID.Heart);
                        Item.NewItem(target.GetSource_FromThis(), target.Hitbox, ItemID.Star);
                    }
                    AncientBloodflareHeartDropCD = 180; //2s一次
                }
            }
            if (AncientGodSlayerSet && projectile.Calamity().stealthStrike && projectile.DamageType == ModContent.GetInstance<RogueDamageClass>() && PerunofYharimCooldown == 0)
            {
                //潜伏攻击成功时提供20%增伤
                player.GetDamage<GenericDamageClass>() += 0.2f;
                PerunofYharimCooldown = 2700;

            }
            #endregion
            
            #region AuricYharim
            if (AncientAuricSet)
            {
                if (hit.Damage > 300 && target.IsAnEnemy(false) && target.lifeMax > 5 && AncientBloodflareHeartDropCD == 0) //大于300伤害才能产出红心与魔力星 
                {
                    int amt = Main.rand.Next(3, 6); //3->5
                    if(Main.rand.NextBool(5)) //每次攻击时1/5概率
                    {
                        for(int i = 0; i < amt; i++)
                        {
                            Item.NewItem(target.GetSource_FromThis(), target.Hitbox, ItemID.Heart);
                            Item.NewItem(target.GetSource_FromThis(), target.Hitbox, ItemID.Star);
                        }
                        AncientBloodflareHeartDropCD = 90; //1.5秒一次
                    }
                }

                if (projectile.DamageType == ModContent.GetInstance<RogueDamageClass>()
                    && projectile.Calamity().stealthStrike
                    && PerunofYharimCooldown == 0
                    ) 
                {
                        SoundEngine.PlaySound(CISoundMenu.YharimsThuner with {Volume = 0.5f});
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
                        PerunofYharimCooldown = 1800;

                }
            }
            #endregion
            if(AncientAstralSet)
            {
                if(hit.Damage > 10 && hit.Crit && projectile.DamageType == ModContent.GetInstance<RogueDamageClass>() && AncientAstralCritsCD == 0)
                {
                    Player.Heal(20);
                    AncientAstralCritsCount += 1;// 自增
                    if(AncientAstralCritsCount == 10)
                    {
                        SoundEngine.PlaySound(SoundID.Item4, Player.Center);
                        Player.AddBuff(ModContent.BuffType<AncientAstralBuff>(), 480); //8秒
                        CIFunction.DustCircle(Player.Center, 12f, 1.2f, DustID.HallowedWeapons, false, 8f, 200);
                    }
                    AncientAstralCritsCD = 45; //一个非常微弱的CD
                }
                if(projectile.DamageType == ModContent.GetInstance<RogueDamageClass>() && AncientAstralStealthCD == 0 && projectile.Calamity().stealthStrike)
                {
                    AncientAstralStealthGap = 900; //15s
                    AncientAstralStealthCD = 60; //1秒间隔
                    if (AncientAstralStealth < 12)
                    {
                        player.lifeRegen += 2; //(24)
                        AncientAstralStealth++;
                    }
                }
            }
            #endregion
            NPCDebuffs(target, projectile.CountsAsClass<MeleeDamageClass>(), projectile.CountsAsClass<RangedDamageClass>(), projectile.CountsAsClass<MagicDamageClass>(), projectile.CountsAsClass<SummonDamageClass>(), projectile.CountsAsClass<ThrowingDamageClass>(), projectile.CountsAsClass<SummonMeleeSpeedDamageClass>());
        }
        #region Debuffs
        public void NPCDebuffs(NPC target, bool melee, bool ranged, bool magic, bool summon, bool rogue, bool whip, bool proj = false, bool noFlask = false)
        {
            if ((melee || rogue || whip) && !noFlask)
            {
                if (BuffStatsArmorShatter)
                {
                    CalamityUtils.Inflict246DebuffsNPC(target, ModContent.BuffType<Crumbling>());
                }
            }
            if (melee && !noFlask)
            {
                if (ElemGauntlet)
                {
                    target.AddBuff(ModContent.BuffType<ElementalMix>(), 300, false);
                    target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300, false);
                    target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 300, false);
                    target.AddBuff(BuffID.Frostburn2, 300);
                    target.AddBuff(BuffID.CursedInferno, 300);
                    target.AddBuff(BuffID.Inferno, 300);
                    target.AddBuff(BuffID.Venom, 300);
                }
            }
            if (summon)
            {
                if (NucleogenesisLegacy)
                {
                    target.AddBuff(BuffID.Electrified, 120);
                    target.AddBuff(ModContent.BuffType<HolyFlames>(), 300, false);
                    target.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 300, false);
                    target.AddBuff(ModContent.BuffType<Irradiated>(), 300, false);
                    target.AddBuff(ModContent.BuffType<Shadowflame>(), 300, false);
                }
            }
        }
        #endregion
        public override void ModifyWeaponKnockback(Item item, ref StatModifier knockback)
        {
            if (BuffStatsYharimsStin)
                knockback += item.knockBack * 0.25f;
        }
        public override void GetHealMana(Item item, bool quickHeal, ref int healValue)
        {
            healValue = (int)(healValue * ManaHealMutipler);
        }
        public override void OnConsumeMana(Item item, int manaConsumed)
        {
            if (AncientAuricSet && PerunofYharimStats)
            Player.Heal(manaConsumed);
        }
        #region Lifesteal
        private void ProjLifesteal(NPC target, Projectile proj, int damage, bool crit)
        {
            CalamityGlobalProjectile modProj = proj.Calamity();

            if (Main.player[Main.myPlayer].lifeSteal > 0f && !Player.moonLeech && target.lifeMax > 5)
            {
                if (AuricSilvaSet)
                {
                    double healMult = 0.1;
                    int heal = Main.rand.Next(5, 11);

                    if (CalamityGlobalProjectile.CanSpawnLifeStealProjectile(healMult, heal))
                        CalamityGlobalProjectile.SpawnLifeStealProjectile(proj, Player, heal, ModContent.ProjectileType<SilvaOrb>(), 3000f, 2f);
                }
                if (GodSlayerMagicSet)
                {
                    double healMult = 0.1;
                    int heal = Main.rand.Next(5, 11);

                    if (CalamityGlobalProjectile.CanSpawnLifeStealProjectile(healMult, heal))
                        CalamityGlobalProjectile.SpawnLifeStealProjectile(proj, Player, heal, ModContent.ProjectileType<GodSlayerHealOrb>(), 3000f, 2f);
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
            if (proj.CountsAsClass<SummonDamageClass>() && !proj.CountsAsClass<SummonMeleeSpeedDamageClass>())
                SummonOnHit(proj, modProj, position, crit, npcCheck);
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
        private static void SummonOnHit(Projectile proj, CalamityGlobalProjectile modProj, Vector2 position, bool crit, bool npcCheck)
        {
            Player player = Main.player[proj.owner];

            var source = proj.GetSource_FromThis();

            CalamityInheritancePlayer CIplayer = player.CIMod();

            List<int> summonExceptionList = new List<int>()
            {
                ModContent.ProjectileType<ApparatusExplosion>(),
            };

            if (summonExceptionList.TrueForAll(x => proj.type != x))
            {
                if (CIplayer.summonProjCooldown <= 0)
                {
                    if (CIplayer.NucleogenesisLegacy)
                    {
                        Projectile.NewProjectile(source, proj.Center, Vector2.Zero, ModContent.ProjectileType<ApparatusExplosion>(), (int)(proj.damage * 0.25f), 4f, proj.owner);
                        CIplayer.summonProjCooldown = 25;
                    }
                }
            }
        }
    }
}
