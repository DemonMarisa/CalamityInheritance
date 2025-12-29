using System;
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
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.NPCs.Boss.SCAL;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    public class RavagerLegendary: LegendaryWeaponClass
    {
        public override ClassType GeneralWeaponClass => ClassType.Magic;
        public override Color DrawColor => Color.HotPink;
        public override int SetRarityColor => RarityType<BetsyPink>();
        public override void ExSSD()
        {
            Item.staff[Item.type] = true;
        }
        public int baseDamage = 60;
        public override void ExSD()
        {
            Item.width = 62;
            Item.height = 62;
            Item.damage = baseDamage;
            Item.mana = 9;
            Item.useAnimation = Item.useTime = 11;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item88;
            Item.autoReuse = true;
            Item.shootSpeed = 20f;
            Item.shoot = ProjectileType<RavagerLegendaryProjAlt>();
        }

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
            string t1 = mp.BetsyTier1 ? Language.GetTextValue($"{GeneralLegendItemTextPath}.TierOne") : Language.GetTextValue($"{GeneralLegendItemTextPath}.TierOneTint");
            tooltips.FindAndReplace("[TIERONE]", t1);
            string t2 = mp.BetsyTier2 ? Language.GetTextValue($"{GeneralLegendItemTextPath}.TierTwo") : Language.GetTextValue($"{GeneralLegendItemTextPath}.TierTwoTint");
            tooltips.FindAndReplace("[TIERTWO]", t2);
            string t3 = mp.BetsyTier3 ? Language.GetTextValue($"{GeneralLegendItemTextPath}.TierThree") : Language.GetTextValue($"{GeneralLegendItemTextPath}.TierThreeTint");
            tooltips.FindAndReplace("[TIERTHREE]", t3);
            //用于发送传奇武器在至尊灾厄眼在场时得到数值增强的信息
            string t4 = null;
            if (NPC.AnyNPCs(NPCType<SupremeCalamitasLegacy>()))
                t4 = Language.GetTextValue($"{Generic.WeaponTextPath}EmpoweredTooltip.Generic");
            // 以下，用于比较复杂的计算
            int boostPercent = (int)((LegendaryBuff() + Generic.GenericLegendBuff()));
            string update = this.GetLocalization("LegendaryScaling").Format(
                boostPercent.ToString()
            );
            tooltips.FindAndReplace("[SCALING]", update);
            if (t4 != null)
            tooltips.Add(new TooltipLine(Mod, "Buff", t4));
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // 必须手动转换，不然会按照int进行加成
            float Buff = (float)((float)(baseDamage + LegendaryBuff() + Generic.GenericLegendBuffInt()) / (float)baseDamage);
            damage *= Buff;
        }

        public static int LegendaryBuff()
        {
            int dmgBuff = 0;
            bool yharon = DownedBossSystem.downedYharon || CIDownedBossSystem.DownedLegacyYharonP2; ;
            dmgBuff += Condition.DownedEmpressOfLight.IsMet() ? 2 : 0;  // 47
            dmgBuff += Condition.DownedDukeFishron.IsMet() ? 2 : 0;     // 49
            dmgBuff += DownedBossSystem.downedPlaguebringer ? 2 : 0;    // 51
            dmgBuff += Condition.DownedCultist.IsMet() ? 9 : 0;         // 60
            dmgBuff += DownedBossSystem.downedAstrumDeus ? 5 : 0;       // 65
            dmgBuff += Condition.DownedMoonLord.IsMet() ? 30 : 0;       // 95
            dmgBuff += DownedBossSystem.downedGuardians ? 5 : 0;        // 100
            dmgBuff += DownedBossSystem.downedDragonfolly ? 5 : 0;      // 105
            dmgBuff += DownedBossSystem.downedProvidence ? 100 : 0;     // 155
            dmgBuff += DownedBossSystem.downedSignus ? 10 : 0;          // 165
            dmgBuff += DownedBossSystem.downedCeaselessVoid ? 10 : 0;   // 175
            dmgBuff += DownedBossSystem.downedStormWeaver ? 10 : 0;     // 185
            dmgBuff += DownedBossSystem.downedPolterghast ? 90 : 0;     // 275
            dmgBuff += DownedBossSystem.downedBoomerDuke ? 20 : 0;      // 295
            dmgBuff += DownedBossSystem.downedDoG ? 265 : 0;            // 480
            dmgBuff += yharon ? 620 : 0;                                // 1100
            dmgBuff += DownedBossSystem.downedCalamitas ? 50 : 0;       // 1150
            dmgBuff += DownedBossSystem.downedExoMechs ? 50 : 0;        // 1200
            dmgBuff += DownedBossSystem.downedExoMechs && DownedBossSystem.downedCalamitas && DownedBossSystem.downedPrimordialWyrm && CIDownedBossSystem.DownedLegacyScal ? 1200 : 0;
            return dmgBuff;
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
                        float pPosX = Main.MouseWorld.X + Main.rand.NextFloat(-200f, 201f);
                        float pPosY = Main.MouseWorld.Y + Main.rand.NextFloat(670f, 1080f);
                        Vector2 newPos = new (pPosX, pPosY);
                        //速度
                        Vector2 spd = Main.MouseWorld - newPos;
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
