using System;
using CalamityInheritance.Content.Items.Accessories;
using CalamityMod;
using CalamityMod.Items.Accessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using CalamityInheritance.CICooldowns;
using CalamityMod.CalPlayer;
using CalamityInheritance.Utilities;
using CalamityMod.Buffs.StatBuffs;
using Terraria.DataStructures;
using Mono.Cecil;
using CalamityInheritance.Content.Projectiles.Typeless;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.NPCs.Abyss;
using CalamityInheritance.Buffs;
using Terraria.ID;
using CalamityMod.Cooldowns;
using CalamityInheritance.Content.Items.Potions;
using CalamityInheritance.Buffs.Statbuffs;
using CalamityMod.Dusts;
using CalamityMod.Items.Armor.Silva;
using Terraria.Graphics.Shaders;
using CalamityInheritance.Content.Items.Weapons.Melee;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public override void PostUpdateMiscEffects()
        {
            CalamityInheritancePlayer modPlayer = Player.CalamityInheritance();
            // Update energy shields
            CIEnergyShields();

            // Misc effects, because I don't know what else to call it
            MiscEffects();

            OtherBuffEffects();

            // Standing still effects
            StandingStillEffects();

            ElysianAegisEffects();

            ShieldDurabilityMax = Player.statLifeMax2;
        }
        public void OtherBuffEffects()
        {
            CalamityPlayer modPlayer = Player.Calamity();
            var modplayer1 = Player.CalamityInheritance();
            if (armorShattering)
            {
                Player.GetDamage<ThrowingDamageClass>() += 0.08f;
                Player.GetDamage<MeleeDamageClass>() += 0.08f;
                Player.GetCritChance<RogueDamageClass>() += 8;
                Player.GetCritChance<MeleeDamageClass>() += 8;
            }

            if (cadence)
            {
                Player.lifeMagnet = true;
                Player.lifeRegen += 10;
                Player.statLifeMax2 += Player.statLifeMax / 5 / 20 * 25;
            }

            if (draconicSurge)
            {
                if (Player.wingTimeMax > 0)
                {
                    Player.wingTimeMax = (int)(Player.wingTimeMax * 1.25);
                }
                Player.statDefense += 16;
                Player.wingAccRunSpeed += 0.1f;
                Player.accRunSpeed += 0.1f;
                if(yharonLore)
                {
                    Player.GetDamage<GenericDamageClass>() += 0.15f;
                }

                if (Player.HasCooldown(DraconicElixirCooldown.ID))
                {
                    Player.statDefense -= 32;
                    Player.wingAccRunSpeed -= 0.2f;
                    Player.accRunSpeed -= 0.2f;
                    Player.GetDamage<GenericDamageClass>() -= 0.15f;
                }
            }

            if (penumbra)
            {
                modPlayer.stealthGenStandstill += 0.15f;
                modPlayer.stealthGenMoving += 0.1f;
            }

            if (profanedRage)
            {
                Player.GetCritChance<GenericDamageClass>() += ProfanedRagePotion.CritBoost;
            }

            if (holyWrath)
            {
                Player.GetDamage<GenericDamageClass>() += 0.12f;
            }

            if (tScale)
            {
                Player.endurance += 0.05f;
                Player.statDefense += 5;
                Player.kbBuff = true;
                if (titanBoost > 0)
                {
                    Player.statDefense += 20;
                    Player.endurance += 0.05f;
                    titanBoost--;
                }
            }
            else
            {
                titanBoost = 0;
            }

            if (yPower)
            {
                Player.endurance += 0.04f;
                Player.statDefense += 10;
                Player.pickSpeed -= 0.1f;
                Player.GetDamage<GenericDamageClass>() += 0.05f;
                Player.GetCritChance<GenericDamageClass>() += 2;
                Player.GetKnockback<SummonDamageClass>() += 1f;
                Player.moveSpeed += 0.075f;
            }

            if (YharimsInsignia)
            {
                Player.GetDamage<MeleeDamageClass>() += 0.15f;
                if (Player.statLife <= (int)(Player.statLifeMax2 * 0.5))
                    Player.GetDamage<GenericDamageClass>() += 0.1f;
            }

            if (darkSunRingold)
            {
                Player.maxMinions += 2;
                Player.GetDamage<GenericDamageClass>() += 0.12f;
                Player.GetKnockback<SummonDamageClass>() += 1.2f;
                Player.GetAttackSpeed<MeleeDamageClass>() += 0.12f;
                Player.pickSpeed -= 0.12f;
                if (Main.eclipse || !Main.dayTime)
                    Player.statDefense += Main.eclipse ? 20 : 20;
            }
            if (animusBoost > 1f)
            {
                if (Player.ActiveItem().type != ModContent.ItemType<Animus>())
                    animusBoost = 1f;
            }
            if (badgeofBravery) //如果启用
            {
                if(modPlayer.tarraMelee) //金源套不再能吃到勇气勋章的效果
                {
                    if(modPlayer.auricSet)
                    {
                        return;
                    }
                    Player.GetCritChance<MeleeDamageClass>() += 10;
                    Player.GetDamage<MeleeDamageClass>() += 0.10f;
                    Player.GetArmorPenetration<MeleeDamageClass>() += 15; 
                }
            }
            if (deificAmuletEffect)
            {
                Player.lifeRegen += 1; //生命恢复
            }
            if (RoDPaladianShieldActive) //如果佩戴壁垒
            {
                // 符合条件就启用圣骑士盾效果
                if (Player.statLife > Player.statLifeMax2 * 0.25f)
                {
                    Player.hasPaladinShield = true;
                    if (Player.whoAmI != Main.myPlayer && Player.miscCounter % 10 == 0)
                    {
                        int myPlayer = Main.myPlayer;
                        if (Main.player[myPlayer].team == Player.team && Player.team != 0)
                        {
                            float teamPlayerXDist = Player.position.X - Main.player[myPlayer].position.X;
                            float teamPlayerYDist = Player.position.Y - Main.player[myPlayer].position.Y;
                            if ((float)Math.Sqrt(teamPlayerXDist * teamPlayerXDist + teamPlayerYDist * teamPlayerYDist) < 800f)
                                Main.player[myPlayer].AddBuff(BuffID.PaladinsShield, 20);
                        }
                    }
                }
            }

            /*
             * 原动不封地把战士永恒套提供的“增加10%伤害”并不能契合当前版本的强度，因此此处直接进行了比较超量的数值加强
             * 但永恒套的怒气Buff本身只能通过受击获得，考虑到其触发条件我并不特别认为这会导致数值能多爆破（吧）
             * 速览: 永恒套的怒气buff现在触发不再有任何条件，但提供10点防御力与10%近战攻速与伤害，不提供暴击概率
             */
            if (reaverMeleeRage)
            {
                Player.GetDamage<MeleeDamageClass>() += 0.10f;
                Player.GetAttackSpeed<MeleeDamageClass>() += 10;
                Player.statDefense += 10;
            }

            /*
            同上，但法师永恒套的Buff属于击发式，提供一定量的暴击率加成与减魔耗
            暴击概率10%,魔力消耗20%
            */
            if (reaverMagePower)
            {
                Player.manaCost *= 0.80f;
                Player.GetCritChance<MagicDamageClass>() += 10;
            }

            if (badgeofBravery) //如果启用
            {
                if (modPlayer.tarraMelee)
                {
                    Player.GetCritChance<MeleeDamageClass>() += 10;
                    Player.GetDamage<MeleeDamageClass>() += 0.10f;
                    Player.GetArmorPenetration<MeleeDamageClass>() += 15;
                }
            }
        }
        #region Energy Shields
        private void CIEnergyShields()
        {
            // 因为较高等级的护盾更亮，所以这里从最高等级到最低等级处理护盾。
            bool shieldAddedLight = false;

            // 如果“海绵”装备没有装备，则消除其耐久冷却时间。
            // 故意保留充电冷却时间以防止快速切换来重新充电护盾。
            if (!CIsponge)
            {
                CalamityPlayer modPlayer = Player.Calamity();
                if (modPlayer.cooldowns.TryGetValue(CISpongeDurability.ID, out var cdDurability))
                    cdDurability.timeLeft = 0;

                // 由于“海绵”的护盾可能处于部分充电状态，这里是为了安全起见。
                // 如果玩家哪怕只有一帧没有装备这个配件，就会完全耗尽所有护盾。

                CISpongeShieldDurability = 0;

            }
            else
            {
                CalamityPlayer modPlayer = Player.Calamity();
                // 如果“海绵”的护盾已经耗尽且还没有开始其充电延迟，则开始充电延迟。
                if (CISpongeShieldDurability == 0 && !modPlayer.cooldowns.ContainsKey(CISpongeRecharge.ID))
                    Player.AddCooldown(CISpongeRecharge.ID, TheSpongetest.CIShieldRechargeDelay);

                // 如果护盾的耐久度大于0但耐久度冷却时间不在冷却时间字典中，则将其添加到冷却时间字典中。
                if (CISpongeShieldDurability > 0 && !modPlayer.cooldowns.ContainsKey(CISpongeDurability.ID))
                {
                    var durabilityCooldown = Player.AddCooldown(CISpongeDurability.ID, TheSpongetest.CIShieldRechargeDelay);
                    durabilityCooldown.timeLeft = CISpongeShieldDurability;
                }

                // 如果护盾的耐久度大于0且不在充电延迟中，则主动补充护盾点数。
                // 在第一次发生这种情况时播放声音。
                if (CISpongeShieldDurability > 0 && !modPlayer.cooldowns.ContainsKey(CISpongeRechargeRelay.ID))
                {
                    if (!CIplayedSpongeShieldSound)
                        SoundEngine.PlaySound(TheSpongetest.ActivationSound, Player.Center);
                    CIplayedSpongeShieldSound = true;

                    // 这个数不是一个整数，并存储了每帧的确切充电进度。
                    CIspongeShieldPartialRechargeProgress += TheSpongetest.CIShieldDurabilityMax / (float)TheSpongetest.CITotalShieldRechargeTime;

                    // 向下取整以获取本帧实际充电的护盾点数。
                    int pointsActuallyRecharged = (int)MathF.Floor(CIspongeShieldPartialRechargeProgress);

                    // 将这些点数加到真实的护盾耐久度上，并限制结果。然后从充电进度中减去这些点数。
                    CISpongeShieldDurability = Math.Min(CISpongeShieldDurability + pointsActuallyRecharged, TheSpongetest.CIShieldDurabilityMax);
                    CIspongeShieldPartialRechargeProgress -= pointsActuallyRecharged;

                    // 更新冷却时间字典中的耐久度指示器。
                    if (modPlayer.cooldowns.TryGetValue(CISpongeDurability.ID, out var cdDurability))
                        cdDurability.timeLeft = CISpongeShieldDurability;
                }

                // Add light if this shield is currently active
                if (CISpongeShieldDurability > 0 && !shieldAddedLight)
                {
                    // The Sponge is much brigher than other shields
                    Lighting.AddLight(Player.Center, Color.White.ToVector3() * 0.75f);
                    shieldAddedLight = true;
                }
            }
        }
        #endregion

        #region Misc Effects
        public void MiscEffects()
        {
            CalamityInheritancePlayer modPlayer = Player.CalamityInheritance();
            CalamityPlayer modPlayer1 = Player.Calamity();

            #region Lore
            if (modPlayer.ElementalQuiver)
                Player.magicQuiver = true;

            if (modPlayer.kingSlimeLore)
            {
                Player.moveSpeed += 0.05f;
                Player.jumpSpeedBoost += Player.autoJump ? 0f : 0.1f;
                Player.statDefense -= 3;
            }

            if (modPlayer.desertScourgeLore)
            {
                if (Player.ZoneDesert || Player.Calamity().ZoneSunkenSea)
                {
                    Player.statDefense += 5;
                    Player.GetDamage<GenericDamageClass>() -= 0.025f;
                }
            }

            if (modPlayer.crabulonLore)
            {
                if (Player.ZoneGlowshroom || Player.ZoneDirtLayerHeight || Player.ZoneRockLayerHeight)
                {
                    if (Main.myPlayer == Player.whoAmI)
                        Player.AddBuff(ModContent.BuffType<Mushy>(), 2);

                    Player.moveSpeed -= 0.1f;
                }
            }

            if (modPlayer.eaterOfWorldsLore)
            {
                int damage = (int)(15 * Player.GetBestClassDamage().ApplyTo(1));
                damage = Player.ApplyArmorAccDamageBonusesTo(damage);
                float knockBack = 1f;

                if (Main.rand.NextBool(15))
                {
                    int num = 0;

                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].owner == Player.whoAmI && Main.projectile[i].type == ModContent.ProjectileType<TheDeadlyMicrobeProjectile>())
                            num++;
                    }

                    if (Main.rand.Next(15) >= num && num < 6)
                    {
                        int num2 = 50;
                        int num3 = 24;
                        int num4 = 90;

                        for (int j = 0; j < num2; j++)
                        {
                            int num5 = Main.rand.Next(200 - j * 2, 400 + j * 2);
                            Vector2 center = Player.Center;
                            center.X += Main.rand.NextFloat(-num5, num5 + 1);
                            center.Y += Main.rand.NextFloat(-num5, num5 + 1);

                            if (!Collision.SolidCollision(center, num3, num3) && !Collision.WetCollision(center, num3, num3))
                            {
                                center.X += num3 / 2;
                                center.Y += num3 / 2;

                                if (Collision.CanHit(Player.Center, 1, 1, center, 1, 1) || Collision.CanHit(new Vector2(Player.Center.X, Player.position.Y - 50f), 1, 1, center, 1, 1))
                                {
                                    int num6 = (int)center.X / 16;
                                    int num7 = (int)center.Y / 16;
                                    bool flag = false;

                                    if (Main.rand.NextBool(3) && Main.tile[num6, num7] != null && Main.tile[num6, num7].WallType > 0)
                                        flag = true;
                                    else
                                    {
                                        center.X -= num4 / 2;
                                        center.Y -= num4 / 2;

                                        if (Collision.SolidCollision(center, num4, num4))
                                        {
                                            center.X += num4 / 2;
                                            center.Y += num4 / 2;
                                            flag = true;
                                        }
                                    }

                                    if (flag)
                                    {
                                        for (int k = 0; k < Main.maxProjectiles; k++)
                                        {
                                            if (Main.projectile[k].active && Main.projectile[k].owner == Player.whoAmI && Main.projectile[k].type == ModContent.ProjectileType<TheDeadlyMicrobeProjectile>() && (center - Main.projectile[k].Center).Length() < 48f)
                                            {
                                                flag = false;
                                                break;
                                            }
                                        }

                                        if (flag && Main.myPlayer == Player.whoAmI)
                                        {
                                            IEntitySource entitySource = Player.GetSource_ItemUse(Player.HeldItem);
                                            Projectile.NewProjectile(entitySource, center.X, center.Y, 0f, 0f, ModContent.ProjectileType<TheDeadlyMicrobeProjectile>(), damage, knockBack, Player.whoAmI, 0f, 0f);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (modPlayer.skeletronLore)
            {
                Player.GetDamage<GenericDamageClass>() += 0.1f;
                Player.GetCritChance<GenericDamageClass>() += 5;
            }

            if (modPlayer.destroyerLore)
            {
                Player.pickSpeed -= 0.05f;
            }

            if (modPlayer.aquaticScourgeLore)
            {
                if (Player.wellFed)
                {
                    Player.Calamity().decayEffigy = true;
                    Player.statDefense += 1;
                    Player.GetDamage(DamageClass.Generic) += 0.025f;
                    Player.GetCritChance<GenericDamageClass>() += 1;
                    Player.GetKnockback(DamageClass.Summon).Base += 0.25f;
                    Player.moveSpeed += 0.1f;
                }
                else
                {
                    Player.statDefense -= 1;
                    Player.GetDamage(DamageClass.Generic) -= 0.025f;
                    Player.GetCritChance<GenericDamageClass>() -= 1;
                    Player.GetKnockback(DamageClass.Summon).Base -= 0.25f;
                    Player.moveSpeed -= 0.1f;
                }
            }

            if (modPlayer.skeletronPrimeLore)
            {
                Player.GetArmorPenetration(DamageClass.Generic) += 10;
            }

            if (modPlayer.leviathanAndSirenLore)
            {
                CalamityPlayer modplayer = Player.Calamity();
                if (Player.IsUnderwater())
                {
                    if (modplayer.aquaticHeart || modplayer.aquaticHeartPrevious)
                        Player.statLifeMax2 += Player.statLifeMax2 / 20;
                }

                if (!Player.IsUnderwater())
                {
                    Player.statDefense -= 8;
                    Player.endurance -= 0.05f;
                }

                if (modPlayer1.sirenPet)
                {
                    Player.spelunkerTimer += 1;
                    if (Player.spelunkerTimer >= 10)
                    {
                        Player.spelunkerTimer = 0;
                        int num65 = 30;
                        int num66 = (int)Player.Center.X / 16;
                        int num67 = (int)Player.Center.Y / 16;

                        for (int num68 = num66 - num65; num68 <= num66 + num65; num68++)
                        {
                            for (int num69 = num67 - num65; num69 <= num67 + num65; num69++)
                            {
                                if (Main.rand.NextBool(4))
                                {
                                    Vector2 vector = new Vector2((float)(num66 - num68), (float)(num67 - num69));
                                    if (vector.Length() < (float)num65 && num68 > 0 && num68 < Main.maxTilesX - 1 && num69 > 0 && num69 < Main.maxTilesY - 1 && Main.tile[num68, num69] != null && Main.tile[num68, num69].HasTile)
                                    {
                                        bool flag7 = false;
                                        if (Main.tile[num68, num69].TileType == 185 && Main.tile[num68, num69].TileFrameY == 18)
                                        {
                                            if (Main.tile[num68, num69].TileFrameX >= 576 && Main.tile[num68, num69].TileFrameX <= 882)
                                                flag7 = true;
                                        }
                                        else if (Main.tile[num68, num69].TileType == 186 && Main.tile[num68, num69].TileFrameX >= 864 && Main.tile[num68, num69].TileFrameX <= 1170)
                                            flag7 = true;

                                        if (flag7 || Main.tileSpelunker[(int)Main.tile[num68, num69].TileType] || (Main.tileAlch[(int)Main.tile[num68, num69].TileType] && Main.tile[num68, num69].TileType != 82))
                                        {
                                            int num70 = Dust.NewDust(new Vector2((float)(num68 * 16), (float)(num69 * 16)), 16, 16, DustID.TreasureSparkle, 0f, 0f, 150, default, 0.3f);
                                            Main.dust[num70].fadeIn = 0.75f;
                                            Main.dust[num70].velocity *= 0.1f;
                                            Main.dust[num70].noLight = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (Player.ZoneSkyHeight)
            {
                if (modPlayer.astrumDeusLore)
                    Player.moveSpeed += 0.2f;
                if (modPlayer.astrumAureusLore)
                    Player.jumpSpeedBoost += 0.5f;
            }

            if (modPlayer.golemLore)
            {
                if (Math.Abs(Player.velocity.X) < 0.05f && Math.Abs(Player.velocity.Y) < 0.05f && Player.itemAnimation == 0)
                    Player.statDefense += 30;
            }

            if (modPlayer.dukeFishronLore)
            {
                if (Player.IsUnderwater())
                {
                    Player.GetDamage(DamageClass.Generic) += 0.05f;
                    Player.GetCritChance<GenericDamageClass>() += 5;
                    Player.moveSpeed += 0.1f;
                }
                else
                {
                    Player.GetDamage(DamageClass.Generic) -= 0.02f;
                    modPlayer.Player.GetCritChance<GenericDamageClass>() -= 2;
                    Player.moveSpeed -= 0.04f;
                }
            }

            if (modPlayer.lunaticCultistLore)
            {
                Player.blind = true;
                Player.endurance += 0.04f;
                Player.statDefense += 4;
                Player.GetDamage(DamageClass.Generic) += 0.04f;
                Player.GetCritChance<GenericDamageClass>() += 4;
                Player.GetKnockback(DamageClass.Summon).Base += 0.5f;
                Player.moveSpeed += 0.1f;
            }

            if (modPlayer.moonLordLore)
            {
                if (Player.gravDir == -1f && Player.gravControl2)
                {
                    Player.endurance += 0.05f;
                    Player.statDefense += 10;
                    Player.GetDamage(DamageClass.Generic) += 0.1f;
                    Player.GetCritChance<GenericDamageClass>() += 10;
                    Player.GetKnockback(DamageClass.Summon).Base += 1.5f;
                    Player.moveSpeed += 0.15f;
                }
                else
                    Player.slowFall = true;
            }

            if (modPlayer.twinsLore)
            {
                if (!Main.dayTime)
                {
                    Player.invis = true;
                    Player.GetCritChance<ThrowingDamageClass>() += 5;
                    Player.GetDamage<ThrowingDamageClass>() += 0.05f;
                }

                if (Player.statLife >= (int)(Player.statLifeMax2 * 0.5))
                    Player.statDefense -= 10;
            }

            if (modPlayer.wallOfFleshLore)
            {
                Player.GetDamage<GenericDamageClass>() -= 0.03f;
            }

            if (modPlayer.planteraLore)
            {
                if (Player.statLife >= (int)(Player.statLifeMax2 * 0.5))
                {
                    Player.GetDamage<GenericDamageClass>() -= 0.05f;
                    Player.statDefense += 10;
                }
                if (Player.statLife <= (int)(Player.statLifeMax2 * 0.5))
                {
                    Player.GetDamage<GenericDamageClass>() += 0.1f;
                    Player.statDefense -= 10;
                }
            }

            if (modPlayer.polterghastLore)
            {
                Player.GetDamage<GenericDamageClass>() -= 0.1f;
            }
            // Brimstone Elemental lore inferno potion boost
            if ((modPlayer.brimstoneElementalLore || modPlayer1.ataxiaBlaze) && Player.inferno)
            {
                const int FramesPerHit = 30;

                // Constantly increment the timer every frame.
                modPlayer1.brimLoreInfernoTimer = (modPlayer1.brimLoreInfernoTimer + 1) % FramesPerHit;

                // Only run this code for the client which is wearing the armor.
                // Brimstone flames is applied every single frame, but direct damage is only dealt twice per second.
                if (Player.whoAmI == Main.myPlayer)
                {
                    const int BaseDamage = 50;
                    int damage = (int)(BaseDamage * Player.GetBestClassDamage().ApplyTo(1));
                    damage = Player.ApplyArmorAccDamageBonusesTo(damage);
                    float range = 300f;
                    IEntitySource entitySource = Player.GetSource_Accessory(Player.HeldItem);
                    for (int i = 0; i < Main.maxNPCs; ++i)
                    {
                        NPC Npc = Main.npc[i];
                        if (!Npc.active || Npc.friendly || Npc.damage <= 0 || Npc.dontTakeDamage)
                            continue;

                        if (Vector2.Distance(Player.Center, Npc.Center) <= range)
                        {
                            Npc.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
                            if (modPlayer1.brimLoreInfernoTimer == 0)
                                Projectile.NewProjectileDirect(entitySource, Npc.Center, Vector2.Zero, ModContent.ProjectileType<DirectStrike>(), damage, 0f, Player.whoAmI, i);
                        }
                    }
                }
            }

            if (modPlayer.calamitasCloneLore)
            {
                Player.maxMinions += 2;
                Player.statLifeMax2 = (int)(Player.statLifeMax2 * 0.75);
            }
            if (modPlayer.plaguebringerGoliathLore)
            {
                if (Player.wingTimeMax > 0)
                    Player.wingTimeMax = (int)(Player.wingTimeMax * 1.25);
            }

            if (modPlayer.boomerDukeLore)
            {
                if (modPlayer1.ZoneAbyss || modPlayer1.ZoneSulphur)
                {
                    Player.breath = Player.breathMax + 91;
                    Player.endurance += 0.2f;
                    Player.statDefense += 30;
                    Player.Calamity().decayEffigy = true;
                    Player.buffImmune[ModContent.BuffType<SulphuricPoisoning>()] = true;
                    Player.buffImmune[ModContent.BuffType<CrushDepth>()] = true;
                    Player.lifeRegen += 3;
                }
                if (!modPlayer1.ZoneAbyss || !modPlayer1.ZoneSulphur)
                {
                    Player.endurance -= 0.1f;
                    Player.statDefense -= 15;
                    Player.accRunSpeed -= 0.5f;
                    Player.lifeRegen -= 3;
                }
            }

            if (modPlayer.ravagerLore)
            {
                if (Player.wingTimeMax > 0)
                    Player.wingTimeMax = (int)(Player.wingTimeMax * 0.5);
                Player.GetDamage<GenericDamageClass>() += 0.5f;
                Player.ClearBuff(BuffID.Featherfall);
            }

            if (modPlayer.providenceLore)
            {
                Player.statLifeMax2 = (int)(Player.statLifeMax2 * 0.8);
                Player.GetDamage<GenericDamageClass>() += 0.25f;
            }

            if (modPlayer.DoGLore)
            {
                Player.GetDamage<TrueMeleeDamageClass>() += 0.25f;
            }
            if (modPlayer.yharonLore)
            {
                modPlayer1.infiniteFlight = true;
                Player.GetDamage<GenericDamageClass>() -= 0.25f;
            }
            #endregion
            #region ArmorSet
            if (modPlayer.invincible)
            {
                foreach (int debuff in CalamityLists.debuffList)
                    Player.buffImmune[debuff] = true;
            }

            if (silvaMageCooldownold > 0)
                silvaMageCooldownold--;

            if (silvaStunCooldownold > 0)
                silvaStunCooldownold--;

            if (reaverBlastCooldown > 0)
                reaverBlastCooldown--; //战士永恒套cd

            if (reaverBurstCooldown > 0)
                reaverBurstCooldown--; //法师永恒套CD

            if (statisTimerOld > 0 && CIDashDelay >= 0)
                statisTimerOld = 0;//斯塔提斯CD

            if (modPlayer.silvaMageold && Player.HasCooldown(SilvaRevive.ID))
            {
                Player.GetDamage<MagicDamageClass>() += 0.60f;
            }

            if (modPlayer.silvaMelee && Player.HasCooldown(SilvaRevive.ID))
            {
                modPlayer1.contactDamageReduction += 0.2f;
            }

            if (silvaMelee)
            {
                double multiplier = (double)Player.statLife / (double)Player.statLifeMax2;
                Player.GetDamage<MeleeDamageClass>() += (float)(multiplier * 0.2);

                if (modPlayer1.auricSet && silvaMelee)
                {
                    double multiplier1 = (double)Player.statLife / (double)Player.statLifeMax2;
                    Player.GetDamage<MeleeDamageClass>() += (float)(multiplier1 * 0.2);
                }
            }

            if (modPlayer.silvaRanged && Player.HasCooldown(SilvaRevive.ID))
            {
                Player.GetDamage<RangedDamageClass>() += 0.40f;
            }

            if (modPlayer.silvaSummonEx && Player.HasCooldown(SilvaRevive.ID))
            {
                Player.GetCritChance<SummonDamageClass>() += 10;
                Player.maxMinions += 2;
            }

            if (modPlayer.silvaRogue && Player.HasCooldown(SilvaRevive.ID))
            {
                Player.GetDamage<RogueDamageClass>() += 0.40f;
            }

            if (modPlayer.AuricDebuffImmune)
            {
                foreach (int debuff in CalamityLists.debuffList)
                    Player.buffImmune[debuff] = true;
            }

            // Silva invincibility effects
            if (auricsilvaCountdown > 0 && aurichasSilvaEffect)
            {
                if(auricsilvaset && !silvaRebornMark)
                {
                    foreach (int debuff in CalamityLists.debuffList)
                        Player.buffImmune[debuff] = true;

                    auricsilvaCountdown -= 1;
                    if (auricsilvaCountdown <= 0)
                    {
                        SoundEngine.PlaySound(SilvaHeadSummon.DispelSound, Player.Center);
                        Player.AddCooldown(SilvaRevive.ID, CalamityUtils.SecondsToFrames(3 * 60));
                    }

                    for (int j = 0; j < 2; j++)
                    {
                        int green = Dust.NewDust(Player.position, Player.width, Player.height, DustID.ChlorophyteWeapon, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 2f);
                        Main.dust[green].position.X += (float)Main.rand.Next(-20, 21);
                        Main.dust[green].position.Y += (float)Main.rand.Next(-20, 21);
                        Main.dust[green].velocity *= 0.9f;
                        Main.dust[green].noGravity = true;
                        Main.dust[green].scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
                        Main.dust[green].shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
                        if (Main.rand.NextBool())
                            Main.dust[green].scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
                    }
                    if (!Player.HasCooldown(SilvaRevive.ID) && aurichasSilvaEffect && auricsilvaCountdown <= 0)
                    {
                        auricsilvaCountdown = 600;
                        aurichasSilvaEffect = false;
                    }
                }
            }

            if (CIsilvaCountdown > 0 && aurichasSilvaEffect)
            {
                if (auricsilvaset && silvaRebornMark)
                {
                    foreach (int debuff in CalamityLists.debuffList)
                        Player.buffImmune[debuff] = true;

                    CIsilvaCountdown -= 1;
                    if (CIsilvaCountdown <= 0)
                    {
                        SoundEngine.PlaySound(SilvaHeadSummon.DispelSound, Player.Center);
                        Player.AddCooldown(SilvaRevive.ID, CalamityUtils.SecondsToFrames(3 * 60));
                    }

                    for (int j = 0; j < 2; j++)
                    {
                        int green = Dust.NewDust(Player.position, Player.width, Player.height, DustID.ChlorophyteWeapon, 0f, 0f, 100, new Color(Main.DiscoR, 203, 103), 2f);
                        Main.dust[green].position.X += (float)Main.rand.Next(-20, 21);
                        Main.dust[green].position.Y += (float)Main.rand.Next(-20, 21);
                        Main.dust[green].velocity *= 0.9f;
                        Main.dust[green].noGravity = true;
                        Main.dust[green].scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
                        Main.dust[green].shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
                        if (Main.rand.NextBool())
                            Main.dust[green].scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
                    }
                    if (!Player.HasCooldown(SilvaRevive.ID) && aurichasSilvaEffect && CIsilvaCountdown <= 0)
                    {
                        CIsilvaCountdown = 900;
                        aurichasSilvaEffect = false;
                    }
                }
            }
            if (Player.whoAmI == Main.myPlayer && AncientXerocMadness)
            {
                Player.AddBuff(ModContent.BuffType<EmpyreanRage>(),  240);
                Player.AddBuff(ModContent.BuffType<EmpyreanWrath>(), 240);
            }
            #endregion
            if (Player.miscCounter % 150 == 0)
            {
                canFireReaverRangedRocket = true;
            }

            if (Player.whoAmI == Main.myPlayer && AncientXerocMadness)
            {
                Player.AddBuff(ModContent.BuffType<EmpyreanRage>(), 240);
                Player.AddBuff(ModContent.BuffType<EmpyreanWrath>(), 240);
            }
        }

        #endregion

        #region Standing Still Effects
        private void StandingStillEffects()
        {
            CalamityInheritancePlayer modPlayer = Player.CalamityInheritance();
            CalamityPlayer modPlayer1 = Player.Calamity();
            if (PsychoticAmulet)
            {
                if (Player.itemAnimation > 0)
                    modPlayer1.modStealthTimer = 5;

                if (Player.StandingStill(0.1f) && !Player.mount.Active)
                {
                    if (modPlayer1.modStealthTimer == 0 && modPlayer1.modStealth > 0f)
                    {
                        modPlayer1.modStealth -= 0.015f;
                        if (modPlayer1.modStealth <= 0f)
                        {
                            modPlayer1.modStealth = 0f;
                            if (Main.netMode == NetmodeID.MultiplayerClient)
                                NetMessage.SendData(MessageID.PlayerStealth, -1, -1, null, Player.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                        }
                    }
                }
                else
                {
                    float playerVel = Math.Abs(Player.velocity.X) + Math.Abs(Player.velocity.Y);
                    modPlayer1.modStealth += playerVel * 0.0075f;
                    if (modPlayer1.modStealth > 1f)
                        modPlayer1.modStealth = 1f;
                    if (Player.mount.Active)
                        modPlayer1.modStealth = 1f;
                }

                Player.GetDamage<ThrowingDamageClass>() += (1f - modPlayer1.modStealth) * 0.2f;
                Player.GetCritChance<ThrowingDamageClass>() += (int)((1f - modPlayer1.modStealth) * 10f);
                Player.aggro -= (int)((1f - modPlayer1.modStealth) * 750f);
                if (modPlayer1.modStealthTimer > 0)
                    modPlayer1.modStealthTimer--;
            }
            if (auricBoostold)
            {
                if (Player.itemAnimation > 0)
                    modPlayer1.modStealthTimer = 5;
                if (Player.StandingStill(0.1f) && !Player.mount.Active)
                {
                    if (modPlayer1.modStealthTimer == 0 && modPlayer1.modStealth > 0f)
                    {
                        modPlayer1.modStealth -= 0.015f;
                        if (modPlayer1.modStealth <= 0f)
                        {
                            modPlayer1.modStealth = 0f;
                            if (Main.netMode == NetmodeID.MultiplayerClient)
                                NetMessage.SendData(MessageID.PlayerStealth, -1, -1, null, Player.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                        }
                    }
                }
                else
                {
                    float playerVel = Math.Abs(Player.velocity.X) + Math.Abs(Player.velocity.Y);
                    modPlayer1.modStealth += playerVel * 0.0075f;
                    if (modPlayer1.modStealth > 1f)
                        modPlayer1.modStealth = 1f;
                    if (Player.mount.Active)
                        modPlayer1.modStealth = 1f;
                }
                float damageBoost = (1f - modPlayer1.modStealth) * 20f;
                Player.GetDamage<GenericDamageClass>() += damageBoost;
                int critBoost = (int)((1f - modPlayer1.modStealth) * 1000f);
                Player.GetCritChance<GenericDamageClass>() += critBoost;
                if (modPlayer1.modStealthTimer > 0)
                    modPlayer1.modStealthTimer--;
            }
            else
                modPlayer1.modStealth = 1f;
        }
        #endregion

        #region Elysian Aegis Effects
        public void ElysianAegisEffects()
        {
            if (elysianAegis)
            {
                bool spawnDust = false;

                // Activate buff
                if (elysianGuard)
                {
                    if (Player.whoAmI == Main.myPlayer)
                        Player.AddBuff(ModContent.BuffType<ElysianGuard>(), 2, false);

                    float shieldBoostInitial = shieldInvinc;
                    shieldInvinc -= 0.08f;
                    if (shieldInvinc < 0f)
                        shieldInvinc = 0f;
                    else
                        spawnDust = true;

                    if (shieldInvinc == 0f && shieldBoostInitial != shieldInvinc && Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.PlayerStealth, -1, -1, null, Player.whoAmI, 0f, 0f, 0f, 0, 0, 0);

                    float damageBoost = (5f - shieldInvinc) * 0.03f;
                    Player.GetDamage<GenericDamageClass>() += damageBoost;

                    int critBoost = (int)((5f - shieldInvinc) * 2f);
                    Player.GetCritChance<GenericDamageClass>() += critBoost;

                    Player.aggro += (int)((5f - shieldInvinc) * 220f);
                    Player.statDefense += (int)((5f - shieldInvinc) * 8f);
                    Player.moveSpeed *= 0.85f;

                    if (Player.mount.Active)
                    {
                        elysianGuard = false;
                    }
                }

                // Remove buff
                else
                {
                    float shieldBoostInitial = shieldInvinc;
                    shieldInvinc += 0.08f;
                    if (shieldInvinc > 5f)
                        shieldInvinc = 5f;
                    else
                        spawnDust = true;

                    if (shieldInvinc == 5f && shieldBoostInitial != shieldInvinc && Main.netMode == NetmodeID.MultiplayerClient)
                        NetMessage.SendData(MessageID.PlayerStealth, -1, -1, null, Player.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                }

                // Emit dust
                if (spawnDust)
                {
                    if (Main.rand.NextBool(2))
                    {
                        Vector2 vector = Vector2.UnitY.RotatedByRandom(Math.PI * 2D);
                        Dust dust = Main.dust[Dust.NewDust(Player.Center - vector * 30f, 0, 0, (int)CalamityDusts.ProfanedFire, 0f, 0f, 0, default, 1f)];
                        dust.noGravity = true;
                        dust.position = Player.Center - vector * (float)Main.rand.Next(5, 11);
                        dust.velocity = vector.RotatedBy(Math.PI / 2D, default) * 4f;
                        dust.scale = 0.5f + Main.rand.NextFloat();
                        dust.fadeIn = 0.5f;
                    }

                    if (Main.rand.NextBool(2))
                    {
                        Vector2 vector2 = Vector2.UnitY.RotatedByRandom(Math.PI * 2D);
                        Dust dust2 = Main.dust[Dust.NewDust(Player.Center - vector2 * 30f, 0, 0, DustID.GoldCoin, 0f, 0f, 0, default, 1f)];
                        dust2.noGravity = true;
                        dust2.position = Player.Center - vector2 * 12f;
                        dust2.velocity = vector2.RotatedBy(-Math.PI / 2D, default) * 2f;
                        dust2.scale = 0.5f + Main.rand.NextFloat();
                        dust2.fadeIn = 0.5f;
                    }
                }
            }
            else
                elysianGuard = false;
        }
        #endregion
    }
}
