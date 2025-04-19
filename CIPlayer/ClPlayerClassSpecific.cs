using System;
using System.Numerics;
using CalamityInheritance.Buffs.Mage;
using CalamityInheritance.Buffs.Statbuffs;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Projectiles.ArmorProj;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Projectiles.Typeless;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer 
    {
        public void MeleeOnHit(Projectile proj, NPC target, NPC.HitInfo hit, int dmgDone)
        {
            var calPlayer = Player.Calamity(); 
            var usPlayer = Player.CIMod();
            //真近战或者近战的简化判定
            bool ifTrueMelee = proj.CountsAsClass<TrueMeleeDamageClass>() || proj.CountsAsClass<TrueMeleeNoSpeedDamageClass>();
            bool ifMelee = proj.CountsAsClass<MeleeDamageClass>() || proj.CountsAsClass<MeleeNoSpeedDamageClass>();
            // 玩家手中武器伤害
            Player player = Main.player[proj.owner];
            int weaponDamage = hit.Damage;

            //弑神飞镖
            if (usPlayer.GodSlayerMelee && usPlayer.fireCD <= 0 && (ifMelee || ifTrueMelee))
            {
                int finalDamage = 500 + weaponDamage / 4;
                Vector2 velocity = CIFunction.GiveVelocity(200f);
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, velocity * 4f, ModContent.ProjectileType<GodSlayerDart>(), finalDamage, 0f, Player.whoAmI);
                usPlayer.fireCD = 60; 
            }
            //永恒套的近战爆炸攻击
            var meleeReaverSrc = proj.GetSource_FromThis();
            if (usPlayer.ReaverMeleeBlast && proj.DamageType == DamageClass.Melee)
            {
                int BlastDamage = (int)(proj.damage * 0.4);
                if (BlastDamage > 30)
                {
                    BlastDamage = 30;
                }
                if (usPlayer.ReaverBlastCooldown <= 0)
                {
                    SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, proj.Center);
                    Projectile.NewProjectile(meleeReaverSrc, proj.Center, Vector2.Zero, ModContent.ProjectileType<ReaverBlast>(),
                                            BlastDamage, 0.15f, Player.whoAmI);
                    usPlayer.ReaverBlastCooldown = 10;
                }
            }
            //这个应该是泰坦药水的真近战标记
            if (ifTrueMelee || proj.type == ModContent.ProjectileType<StepToolShadowChair>())
                BuffStatsTitanScaleTrueMelee = 600;
        }
        public void RangedOnHit(Projectile proj, NPC target, NPC.HitInfo hit, int dmgDone)
        {

        }
        public void MagicOnHit(Projectile proj, NPC target, NPC.HitInfo hit, int dmgDone)
        {
            
            Player player = Main.player[proj.owner];
            var calPlayer = player.Calamity(); 
            var usPlayer = player.CIMod();
            //弑神火
            if (GodSlayerMagicSet && proj.DamageType == DamageClass.Magic)
            {
                if (fireCD > 0)
                {
                    return;
                }
                fireCD = 2;
                int weaponDamage = hit.Damage;
                int finalDamage = 400 + weaponDamage / 4;

                int projectileTypes = ModContent.ProjectileType<GodSlayerOrb>();
                float randomAngleOffset = (float)(Main.rand.NextDouble() * 2 * MathHelper.Pi);
                Vector2 direction = new((float)Math.Cos(randomAngleOffset), (float)Math.Sin(randomAngleOffset));
                float randomSpeed = Main.rand.NextFloat(12f, 16f);
                Projectile.NewProjectile(proj.GetSource_FromThis(), proj.Center, direction * randomSpeed, projectileTypes, finalDamage * 5, proj.knockBack);
            }
            //林海
            var source = proj.GetSource_FromThis();
            if (SilvaMagicSetLegacy && SilvaMagicSetLegacyCooldown <= 0 && (proj.penetrate == 1 || proj.timeLeft <= 5) && proj.DamageType == DamageClass.Magic)
            {
                SilvaMagicSetLegacyCooldown = 300;
                SoundEngine.PlaySound(SoundID.Zombie103, proj.Center); //So scuffed, just because zombie sounds werent ported normally
                int silvaBurstDamage = Player.ApplyArmorAccDamageBonusesTo((float)(800.0 + 0.6 * proj.damage));
                Projectile.NewProjectile(source, proj.Center, Vector2.Zero, ModContent.ProjectileType<SilvaBurst>(), silvaBurstDamage, 8f, Player.whoAmI);
            }
            //永恒套
            if (Main.player[proj.owner].CIMod().ReaverMageBurst)
            {
                if (ReaverMageBurst) //击发时提供法术增强buff
                {
                    Player.AddBuff(ModContent.BuffType<ReaverMagePower>(), 180);
                }

                if (ReaverBurstCooldown <= 0)
                {
                    int[] projectileTypes = [ModContent.ProjectileType<CISporeGas>(), ModContent.ProjectileType<CISporeGas2>(), ModContent.ProjectileType<CISporeGas3>()];
                    float baseAngleIncrement = 2 * MathHelper.Pi / 16;
                    float randomAngleOffset = (float)(Main.rand.NextDouble() * MathHelper.Pi / 4 - MathHelper.Pi / 8);
                    //好像这样伤害还挺低的但我也不知道该不该调整了
                    int newDamage = Player.ApplyArmorAccDamageBonusesTo(CalamityUtils.DamageSoftCap(15 + 0.15 * proj.damage, 30));

                    for (int sporecounts = 0; sporecounts < 16; sporecounts++)
                    {
                        float angle = sporecounts * baseAngleIncrement + randomAngleOffset;
                        Vector2 direction = new((float)Math.Cos(angle), (float)Math.Sin(angle));
                        int randomProjectileType = projectileTypes[Main.rand.Next(projectileTypes.Length)];
                        float randomSpeed = Main.rand.NextFloat(2f, 4f);
                        Projectile.NewProjectile(Player.GetSource_FromThis(), target.Center, direction * randomSpeed, randomProjectileType, newDamage, proj.knockBack);
                    }
                    target.AddBuff(BuffID.Poisoned, 120);
                    ReaverBurstCooldown = 90;
                }
            }
        }
        public void SummonOnHit(Projectile proj, NPC target, NPC.HitInfo hit, int dmgDone)
        {
            Player player = Main.player[proj.owner];
            var calPlayer = player.Calamity(); 
            var usPlayer = player.CIMod();
            //核子
            if (usPlayer.summonProjCooldown <= 0)
            {
                if (usPlayer.NucleogenesisLegacy)
                {
                    Projectile.NewProjectile(proj.GetSource_FromThis(), proj.Center, Vector2.Zero, ModContent.ProjectileType<ApparatusExplosion>(), (int)(proj.damage * 0.25f), 4f, proj.owner);
                    usPlayer.summonProjCooldown = 25;
                }
            }

            if (usPlayer.GodSlayerSummonSet && proj.DamageType != DamageClass.Generic)
            {
                if (usPlayer.fireCD > 0)
                    return;

                usPlayer.fireCD = 3;
                player = Main.player[proj.owner];
                int weaponDamage = hit.Damage;
                int finalDamage = 400 + weaponDamage / 4;

                int projectileTypes = ModContent.ProjectileType<GodSlayerPhantom>();
                float randomAngleOffset = (float)(Main.rand.NextDouble() * 2 * MathHelper.Pi);
                Vector2 direction = new((float)Math.Cos(randomAngleOffset), (float)Math.Sin(randomAngleOffset));
                float randomSpeed = Main.rand.NextFloat(6f, 8f);
                Projectile.NewProjectile(proj.GetSource_FromThis(), proj.Center, direction * randomSpeed, projectileTypes, finalDamage, proj.knockBack);
            }
        }
        public void RogueOnHit(Projectile proj, NPC target, NPC.HitInfo hit, int dmgDone)
        {
            Player player = Main.player[proj.owner];
            var calPlayer = player.Calamity(); 
            var usPlayer = player.CIMod();
            //远古弑神套
            if (AncientGodSlayerSet && proj.Calamity().stealthStrike && proj.DamageType == ModContent.GetInstance<RogueDamageClass>() && PerunofYharimCooldown == 0)
            {
                //潜伏攻击成功时提供20%增伤
                player.GetDamage<GenericDamageClass>() += 0.2f;
                PerunofYharimCooldown = 2700;
            }
            //星幻套
            if (AncientAstralSet)
            {
                if (hit.Damage > 10 && hit.Crit && proj.DamageType == ModContent.GetInstance<RogueDamageClass>() && AncientAstralCritsCD == 0)
                {
                    Player.Heal(20);
                    AncientAstralCritsCount += 1;// 自增
                    if (AncientAstralCritsCount == RequireCrits)
                    {
                        SoundEngine.PlaySound(CISoundID.SoundFallenStar with { Volume = 0.7f }, Player.Center);
                        Player.AddBuff(ModContent.BuffType<AncientAstralBuff>(), 300); //5秒
                        CIFunction.DustCircle(Player.Center, 18f, 1.8f, DustID.HallowedWeapons, false, 8f);
                    }
                    AncientAstralCritsCD = 60; //一个非常微弱的CD
                }
                if (proj.DamageType == ModContent.GetInstance<RogueDamageClass>() && AncientAstralStealthCD == 0 && proj.Calamity().stealthStrike)
                {
                    AncientAstralStealthGap = 900; //15s
                    AncientAstralStealthCD = 60; //1秒间隔
                    AncientAstralStealth++;
                    //使其不超过12，即我们需要的上限
                    if (AncientAstralStealth > 12)
                        AncientAstralStealth = 12;
                    if (AncientAstralStealth < 13)
                    {
                        player.lifeRegen += AncientAstralStealth; //(6)
                    }
                }
            }
            //纳米技术
            Vector2 position = target.Center;
            if (proj.Calamity().stealthStrike && proj.Calamity().stealthStrikeHitCount < 3)
            {
                if (nanotechold)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Vector2 source = new Vector2(position.X + Main.rand.Next(-201, 201), Main.screenPosition.Y - 600f - Main.rand.Next(50));
                        Vector2 velocity = (position - source) / 40f;

                        Projectile.NewProjectile(proj.GetSource_FromThis(), source, velocity, ModContent.ProjectileType<NanoFlare>(), (int)(proj.damage * 0.05), 3f, proj.owner);
                    }
                }
            }
        }
    
        /// <summary>
        /// 全职业共享效果
        /// </summary>
        public void GenericOnhit(Projectile proj, NPC target, NPC.HitInfo hit, int dmgDone)
        {
            //远古血炎的红心生成
            if (AncientBloodflareSet && hit.Damage > 300 && target.IsAnEnemy(false) && target.lifeMax > 5 && AncientBloodflareHeartDropCD == 0) //大于300伤害才能产出红心与魔力星 
            {
                int amt = Main.rand.Next(2, 5); //2->4
                if (Main.rand.NextBool(6)) //每次攻击时1/6概率
                {
                    for (int i = 0; i < amt; i++)
                    {
                        Item.NewItem(target.GetSource_FromThis(), target.Hitbox, ItemID.Heart);
                        Item.NewItem(target.GetSource_FromThis(), target.Hitbox, ItemID.Star);
                    }
                    AncientBloodflareHeartDropCD = 180; //2s一次
                }
            }
            //魔君套的效果
            if (AncientAuricSet)
            {
                //红心CD
                if (hit.Damage > 300 && target.IsAnEnemy(false) && target.lifeMax > 5 && AncientBloodflareHeartDropCD == 0) //大于300伤害才能产出红心与魔力星 
                {
                    int amt = Main.rand.Next(3, 6); //3->5
                    if (Main.rand.NextBool(5)) //每次攻击时1/5概率
                    {
                        for (int i = 0; i < amt; i++)
                        {
                            Item.NewItem(target.GetSource_FromThis(), target.Hitbox, ItemID.Heart);
                            Item.NewItem(target.GetSource_FromThis(), target.Hitbox, ItemID.Star);
                        }
                        AncientBloodflareHeartDropCD = 90; //1.5秒一次
                    }
                }
                //魔君之怒
                if (proj.DamageType == ModContent.GetInstance<RogueDamageClass>()
                    && proj.Calamity().stealthStrike
                    && PerunofYharimCooldown == 0
                    )
                {
                    SoundEngine.PlaySound(CISoundMenu.YharimsThuner with { Volume = 0.5f });
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
            //花岗岩核心
            if (SMarnite && hit.Crit && SparkTimer == 0)
            {
                int sparksNum = 6;
                for (int i = 0; i < sparksNum; i++)
                {
                    //这里是为了绕过0的处理
                    Vector2 sparkSpeed = Main.rand.NextBool(2) ? new(Main.rand.NextFloat(-50f, 0f), Main.rand.NextFloat(-50f, 0f)) : new(Main.rand.NextFloat(1f, 51f), Main.rand.NextFloat(0f, 51f));
                    sparkSpeed.Normalize();
                    sparkSpeed *= Main.rand.NextFloat(30f, 61f) * 0.1f;
                    Projectile.NewProjectile(Player.GetSource_FromThis(), target.Center.X, target.Center.Y, sparkSpeed.X, sparkSpeed.Y, ModContent.ProjectileType<ShrineMarniteProj>(), (int)(hit.Damage * 0.15f), 0f, Player.whoAmI, 0f, 0f);
                }
                SparkTimer = 10;
            }
        }

        public void AddDebuff(Projectile p, NPC tar, ref NPC.HitInfo hit)
        {
        bool ifMelee = p.CountsAsClass<MeleeDamageClass>() || p.CountsAsClass<MeleeNoSpeedDamageClass>();
            bool ifTrueMelee = p.CountsAsClass<TrueMeleeDamageClass>() || p.CountsAsClass<TrueMeleeNoSpeedDamageClass>();
            bool ifRogue = p.CountsAsClass<RogueDamageClass>();
            bool ifSummon = p.CountsAsClass<SummonDamageClass>();
            if (ifMelee || ifTrueMelee || ifRogue || p.CountsAsClass<SummonMeleeSpeedDamageClass>())
            {
                if (BuffStatsArmorShatter)
                    CalamityUtils.Inflict246DebuffsNPC(tar, ModContent.BuffType<Crumbling>());
            }
            if (ifMelee || ifTrueMelee)
            {
                if (ElemGauntlet)
                {
                    tar.AddBuff(ModContent.BuffType<ElementalMix>(), 300, false);
                    tar.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300, false);
                    tar.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 300, false);
                    tar.AddBuff(BuffID.Frostburn2, 300);
                    tar.AddBuff(BuffID.CursedInferno, 300);
                    tar.AddBuff(BuffID.Inferno, 300);
                    tar.AddBuff(BuffID.Venom, 300);
                }
            }
            if (ifSummon)
            {
                if (NucleogenesisLegacy)
                {
                    tar.AddBuff(BuffID.Electrified, 120);
                    tar.AddBuff(ModContent.BuffType<HolyFlames>(), 300, false);
                    tar.AddBuff(ModContent.BuffType<AstralInfectionDebuff>(), 300, false);
                    tar.AddBuff(ModContent.BuffType<Irradiated>(), 300, false);
                    tar.AddBuff(ModContent.BuffType<Shadowflame>(), 300, false);
                }
            }
            //北辰鹦哥鱼的射弹计数器
            if (p.type == ModContent.ProjectileType<PolarStarLegacy>())
                PolarisBoostCounter += 1;

            #region Lore
            if (LorePerforator)
                tar.AddBuff(BuffID.Ichor, 90);
            if (LoreHive)
                tar.AddBuff(BuffID.CursedInferno, 90);
                
            if (LoreProvidence || PanelsLoreProvidence)
                tar.AddBuff(ModContent.BuffType<HolyInferno>(), 180, false);

            if (BuffStatsHolyWrath)
                tar.AddBuff(ModContent.BuffType<HolyFlames>(), 180, false);

            if (YharimsInsignia)
                tar.AddBuff(ModContent.BuffType<HolyFlames>(), 120, false);

            if (BuffStatsDraconicSurge && Main.zenithWorld)
                tar.AddBuff(ModContent.BuffType<Dragonfire>(), 360, false);
            #endregion
        }
    }

}