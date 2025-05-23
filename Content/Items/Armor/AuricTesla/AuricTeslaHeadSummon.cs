﻿using CalamityMod.Buffs.Summon;
using CalamityMod.Items.Armor.Auric;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using System;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Projectiles.Summon;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityMod.CalPlayer.Dashes;
using CalamityInheritance.Content.Items.Armor.GodSlayerOld;
using CalamityInheritance.Content.Items.Accessories.Ranged;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;

namespace CalamityInheritance.Content.Items.Armor.AuricTesla
{
    [AutoloadEquip(EquipType.Head)]
    public class AuricTeslaHeadSummon : CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.defense = 12; //132
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            var usPlayer = player.CIMod();
            if (usPlayer.AuricSilvaFakeDeath)
            {
                if (Main.keyState.IsKeyDown(Keys.LeftAlt))
                {
                    string Details = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Armor.AuricTeslaHeadSummon.Details");
                    tooltips.Add(new TooltipLine(Mod, "Details", Details));
                }
            }
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<AuricTeslaBodyArmorold>() && legs.type == ModContent.ItemType<AuricTeslaCuissesold>();

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            CalamityInheritancePlayer CIPlayer = player.GetModPlayer<CalamityInheritancePlayer>();
            var modPlayer = player.Calamity();
            modPlayer.tarraSet = true;
            modPlayer.tarraSummon = true;
            modPlayer.bloodflareSet = true;
            modPlayer.bloodflareSummon = true;
            CIPlayer.AuricSilvaFakeDeath = true;
            modPlayer.silvaSummon = true;
            modPlayer.godSlayer = true;
            CIPlayer.AuricDebuffImmune = true;
            CIPlayer.GodSlayerSummonSet = true;

            player.setBonus = this.GetLocalizedValue("SetBonus");
            CIPlayer.GodSlayerReborn = true;
            if (modPlayer.godSlayerDashHotKeyPressed || player.dashDelay != 0 && modPlayer.LastUsedDashID == GodslayerArmorDash.ID)
            {
                modPlayer.DeferredDashID = GodslayerArmorDash.ID;
                player.dash = 0;
            }

            CIPlayer.SilvaSummonSetLegacy = true;
            modPlayer.WearingPostMLSummonerSet = true;

            player.thorns += 3f;
            player.ignoreWater = true;
            player.crimsonRegen = true;
            player.GetDamage<SummonDamageClass>() += 1.2f;
            player.maxMinions += 1;

            if (player.whoAmI == Main.myPlayer)
            {
                var source = player.GetSource_ItemUse(Item);
                if (player.FindBuffIndex(ModContent.BuffType<SilvaCrystalBuff>()) == -1)
                {
                    player.AddBuff(ModContent.BuffType<SilvaCrystalBuff>(), 3600, true);
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<SilvaCrystal>()] < 1)
                {
                    var damage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(1000);
                    var p = Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0f, -1f, ModContent.ProjectileType<SilvaCrystal>(), damage, 0f, Main.myPlayer, -20f, 0f);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 1000;
                }

                if (player.FindBuffIndex(ModContent.BuffType<Mechworm>()) == -1 && CIConfig.Instance.GodSlayerWorm)
                {
                    player.AddBuff(ModContent.BuffType<Mechworm>(), 3600, true);
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<MechwormHead>()] < 1 && CIConfig.Instance.GodSlayerWorm)
                {
                    int owner = player.whoAmI;
                    int typeHead = ModContent.ProjectileType<MechwormHead>();
                    int typeBody = ModContent.ProjectileType<MechwormBody>();
                    int typeBody2 = ModContent.ProjectileType<MechwormBody>();
                    int typeTail = ModContent.ProjectileType<MechwormTail>();
                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].owner == owner)
                        {
                            if (Main.projectile[i].type == typeHead || Main.projectile[i].type == typeTail || Main.projectile[i].type == typeBody ||
                                Main.projectile[i].type == typeBody2)
                            {
                                Main.projectile[i].Kill();
                            }
                        }
                    }
                    int maxMinionScale = player.maxMinions;
                    if (maxMinionScale > 10)
                    {
                        maxMinionScale = 10;
                    }
                    var Wormdamage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(CIServerConfig.Instance.CalStatInflationBACK ? 400 : 100);
                    Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
                    Vector2 value = Vector2.UnitX.RotatedBy(player.fullRotation, default);
                    Vector2 vector3 = Main.MouseWorld - vector2;
                    float velX = Main.mouseX + Main.screenPosition.X - vector2.X;
                    float velY = Main.mouseY + Main.screenPosition.Y - vector2.Y;
                    if (player.gravDir == -1f)
                    {
                        velY = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - vector2.Y;
                    }
                    float dist = (float)Math.Sqrt((double)(velX * velX + velY * velY));
                    if ((float.IsNaN(velX) && float.IsNaN(velY)) || (velX == 0f && velY == 0f))
                    {
                        velX = player.direction;
                        velY = 0f;
                        dist = 10f;
                    }
                    else
                    {
                        dist = 10f / dist;
                    }
                    velX *= dist;
                    velY *= dist;
                    int head = -1;
                    int tail = -1;
                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        if (Main.projectile[i].active && Main.projectile[i].owner == owner)
                        {
                            if (head == -1 && Main.projectile[i].type == typeHead)
                            {
                                head = i;
                            }
                            else if (tail == -1 && Main.projectile[i].type == typeTail)
                            {
                                tail = i;
                            }
                            if (head != -1 && tail != -1)
                            {
                                break;
                            }
                        }
                    }
                    if (head == -1 && tail == -1)
                    {
                        float num77 = Vector2.Dot(value, vector3);
                        if (num77 > 0f)
                        {
                            player.ChangeDir(1);
                        }
                        else
                        {
                            player.ChangeDir(-1);
                        }
                        velX = 0f;
                        velY = 0f;
                        vector2.X = Main.mouseX + Main.screenPosition.X;
                        vector2.Y = Main.mouseY + Main.screenPosition.Y;
                        int curr = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), vector2.X, vector2.Y, velX, velY, ModContent.ProjectileType<MechwormHead>(), Wormdamage, 1, owner);

                        int prev = curr;
                        curr = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), vector2.X, vector2.Y, velX, velY, ModContent.ProjectileType<MechwormBody>(), Wormdamage, 1, owner, prev);

                        prev = curr;
                        curr = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), vector2.X, vector2.Y, velX, velY, ModContent.ProjectileType<MechwormBody>(), Wormdamage, 1, owner, prev);
                        Main.projectile[prev].localAI[1] = curr;
                        Main.projectile[prev].netUpdate = true;

                        prev = curr;
                        curr = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), vector2.X, vector2.Y, velX, velY, ModContent.ProjectileType<MechwormTail>(), Wormdamage, 1, owner, prev);
                        Main.projectile[prev].localAI[1] = curr;
                        Main.projectile[prev].netUpdate = true;
                    }
                    else if (head != -1 && tail != -1)
                    {
                        int body = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), vector2.X, vector2.Y, velX, velY, ModContent.ProjectileType<MechwormBody>(), Wormdamage, 1, owner, Main.projectile[tail].ai[0]);
                        int back = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), vector2.X, vector2.Y, velX, velY, ModContent.ProjectileType<MechwormBody>(), Wormdamage, 1, owner, body);

                        Main.projectile[body].localAI[1] = back;
                        Main.projectile[body].ai[1] = 1f;
                        Main.projectile[body].minionSlots = 0f;
                        Main.projectile[body].netUpdate = true;

                        Main.projectile[back].localAI[1] = tail;
                        Main.projectile[back].netUpdate = true;
                        Main.projectile[back].minionSlots = 0f;
                        Main.projectile[back].ai[1] = 1f;

                        Main.projectile[tail].ai[0] = back;
                        Main.projectile[tail].netUpdate = true;
                        Main.projectile[tail].ai[1] = 1f;
                    }
                }
            }
        }
        public override void UpdateEquip(Player player)
        {
            var modPlayer1 = player.CIMod();
            modPlayer1.auricBoostold = true;
            player.maxMinions += 7;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup("CalamityInheritance:AnySilvaHeadSummon").
                AddIngredient<GodSlayerHeadSummonold>().
                AddIngredient<BloodflareHeadSummon>().
                AddIngredient<TarragonHeadSummon>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBarold>(1).
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddRecipeGroup("CalamityInheritance:AnySilvaHeadSummon").
                AddIngredient<GodSlayerHeadSummonold>().
                AddIngredient<BloodflareHeadSummon>().
                AddIngredient<TarragonHeadSummon>().
                AddIngredient<PsychoticAmulet>().
                AddIngredient<AuricBar>(12).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
