﻿using System;
using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using CalamityMod;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.NPCs.Boss.SCAL;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    public class RavagerLegendary: CIMagic, ILocalizedModType
    {
        
        public static string TextRoute => $"{Generic.GetWeaponLocal}.Magic.RavagerLegendary";
        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 62;
            Item.damage = 100;
            Item.mana = 7;
            Item.DamageType = DamageClass.Magic;
            Item.useAnimation = Item.useTime = 11;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item88;
            Item.autoReuse = true;
            Item.shootSpeed = 20f;
            Item.shoot = ModContent.ProjectileType<RavagerLegendaryProjAlt>();

            Item.value = CIShopValue.RarityMaliceDrop;
            Item.rare = CIConfig.Instance.LegendaryRarity ? ModContent.RarityType<BetsyPink>() : ModContent.RarityType<MaliceChallengeDrop>();
        }

        public override bool AltFunctionUse(Player player) => true;

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            if (player.altFunctionUse == 2)
                mult *= 1.5f;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player p = Main.LocalPlayer;
            var mp = p.CIMod();
            //升级的Tooltip:
            string t1 = mp.BetsyTier1 ? Language.GetTextValue($"{TextRoute}.TierOne") : Language.GetTextValue($"{TextRoute}.TierOneTint");
            tooltips.FindAndReplace("[TIERONE]", t1);
            string t2 = mp.BetsyTier2 ? Language.GetTextValue($"{TextRoute}.TierTwo") : Language.GetTextValue($"{TextRoute}.TierTwoTint");
            tooltips.FindAndReplace("[TIERTWO]", t2);
            string t3 = mp.BetsyTier3 ? Language.GetTextValue($"{TextRoute}.TierThree") : Language.GetTextValue($"{TextRoute}.TierThreeTint");
            tooltips.FindAndReplace("[TIERTHREE]", t3);
            //用于发送传奇武器在至尊灾厄眼在场时得到数值增强的信息
            string t4 = null;
            if (NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()))
                t4 = Language.GetTextValue($"{Generic.GetWeaponLocal}.EmpoweredTooltip.Generic");
            // 以下，用于比较复杂的计算
            int boostPercent = (int)(LegendaryDamage() * 100);
            string update = this.GetLocalization("LegendaryScaling").Format(
                boostPercent.ToString()
            );
            tooltips.FindAndReplace("[SCALING]", update);
            if (t4 != null)
            tooltips.Add(new TooltipLine(Mod, "Buff", t4));
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage *= LegendaryDamage() + Generic.GenericLegendBuff();
        }
        public static float LegendaryDamage()
        {
            float newDamage = 0f;
            newDamage += DownedBossSystem.downedRavager ? 0.2f : 0f;
            newDamage += Condition.DownedEmpressOfLight.IsMet() ? 0.2f : 0f;
            newDamage += Condition.DownedDukeFishron.IsMet() ? 0.2f : 0f;
            newDamage += Condition.DownedCultist.IsMet() ? 0.4f : 0f;
            newDamage += Condition.DownedMoonLord.IsMet() ? 0.4f : 0f;
            newDamage += DownedBossSystem.downedGuardians ? 0.4f : 0f;
            newDamage += DownedBossSystem.downedProvidence  ? 0.6f : 0f;
            newDamage += DownedBossSystem.downedPolterghast ? 0.6f : 0f;
            newDamage += DownedBossSystem.downedDoG ? 0.8f : 0f;
            newDamage += DownedBossSystem.downedYharon ? 1.0f : 0f;
            newDamage += DownedBossSystem.downedExoMechs || DownedBossSystem.downedCalamitas ? 1.5f : 0f;
            newDamage += CIDownedBossSystem.DownedLegacyScal ? 5f : 0f;
            return 1f + newDamage;

        }
        public override float UseSpeedMultiplier(Player player)
        {
            if (player.altFunctionUse != 2)
                return 1.2f;
            return 1.50f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var usPlayer = player.CIMod();
            float pSpriteType = Main.rand.Next(6);
            if (player.altFunctionUse == 2)
            {
                int meteorAmt = Main.rand.Next(4, 6);
                //T2样式加强：左键与右键多2颗陨石
                if (usPlayer.BetsyTier2)
                    meteorAmt += 3;
                for (int i = 0; i < meteorAmt; ++i)
                {
                    float SpeedX = velocity.X + Main.rand.Next(-30, 31) * 0.05f;
                    float SpeedY = velocity.Y + Main.rand.Next(-30, 31) * 0.05f;
                    float ai0 = Main.rand.Next(6);
                    Projectile.NewProjectile(source, position.X, position.Y, SpeedX, SpeedY, type, damage, knockback, player.whoAmI, pSpriteType, 0.5f + (float)Main.rand.NextDouble() * 0.9f);
                }
                return false;
            }
            else
            //下面这段史不太好改看情况吧
            {
                float meteorSpeed = Item.shootSpeed;
                Vector2 realPlayerPos = player.RotatedRelativePoint(player.MountedCenter, true);
                float mouseXDist = Main.mouseX + Main.screenPosition.X - realPlayerPos.X;
                float mouseYDist = Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y;
                if (player.gravDir == -1f)
                {
                    mouseYDist = Main.screenPosition.Y + Main.screenHeight - Main.mouseY - realPlayerPos.Y;
                }
                float mouseDistance = (float)Math.Sqrt((double)(mouseXDist * mouseXDist + mouseYDist * mouseYDist));
                if (float.IsNaN(mouseXDist) && float.IsNaN(mouseYDist) || mouseXDist == 0f && mouseYDist == 0f)
                {
                    mouseXDist = player.direction;
                    mouseYDist = 0f;
                    mouseDistance = meteorSpeed;
                }
                else
                {
                    mouseDistance = meteorSpeed / mouseDistance;
                }
                int pAmt = 4;
                for (int i = 0; i < pAmt; i++)
                {
                    realPlayerPos = new Vector2(player.position.X + player.width * 0.5f + (float)(Main.rand.Next(201) * -(float)player.direction) + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 800f);
                    realPlayerPos.X = (realPlayerPos.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
                    realPlayerPos.Y += 100 * i;
                    mouseXDist = Main.mouseX + Main.screenPosition.X - realPlayerPos.X + Main.rand.Next(-40, 41) * 0.03f;
                    mouseYDist = Main.mouseY + Main.screenPosition.Y - realPlayerPos.Y;
                    if (mouseYDist < 0f)
                    {
                        mouseYDist *= -1f;
                    }
                    if (mouseYDist < 20f)
                    {
                        mouseYDist = 20f;
                    }
                    mouseDistance = (float)Math.Sqrt((double)(mouseXDist * mouseXDist + mouseYDist * mouseYDist));
                    mouseDistance = meteorSpeed / mouseDistance;
                    mouseXDist *= mouseDistance;
                    mouseYDist *= mouseDistance;
                    float meteorSpawnXOffset = mouseXDist;
                    float meteorSpawnYOffset = mouseYDist + Main.rand.Next(-40, 41) * 0.02f;
                    float ai0 = Main.rand.Next(6);
                    Projectile.NewProjectile(source, realPlayerPos.X, realPlayerPos.Y, meteorSpawnXOffset * 0.75f, meteorSpawnYOffset * 0.75f, type, damage, knockback, player.whoAmI, ai0, 0.5f + (float)Main.rand.NextDouble() * 0.9f); //0.3

                }
                //从地底下发射额外的射弹
                int j = 0;
                if (usPlayer.BetsyTier2)
                {
                    for ( ; j < 2; j++)
                    {
                        float pPosX = player.Center.X+ Main.rand.NextFloat(-200f, 201f);
                        float pPosY = player.Center.Y + Main.rand.NextFloat(670f, 1080f);
                        Vector2 newPos = new (pPosX, pPosY);
                        //速度
                        Vector2 spd= Main.MouseWorld - newPos;
                        //水平速度随机度
                        spd.X += Main.rand.NextFloat(-15f, 16f);
                        float pSpeed = 24f;
                        float tarDist =  spd.Length();
                        //?
                        tarDist = pSpeed / tarDist;
                        spd.X *= tarDist;
                        spd.Y *= tarDist;
                        float ai0 = Main.rand.Next(6);
                        Projectile.NewProjectile(source, newPos, spd, type, damage, knockback, player.whoAmI, ai0, 0.5f + (float)Main.rand.NextDouble() * 0.9f);
                    }
                }
                return false;
            }
        }
    
    }
}
