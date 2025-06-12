using CalamityInheritance.Content.Projectiles.Magic;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Sounds;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.NPCs.Boss.SCAL;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    public class DestroyerLegendary: CIMagic, ILocalizedModType
    {
        
        public static string TextRoute => $"{Generic.GetWeaponLocal}.Magic.DestroyerLegendary";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public static readonly int BaseDamage = 17;
        public override void SetDefaults()
        {
            Item.width = 96;
            Item.height = 34;
            Item.damage = BaseDamage;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 20;
            Item.useTime = Item.useAnimation = 7;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3.25f;
            Item.UseSound = SoundID.Item92;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<DestroyerLegendaryBomb>();
            Item.shootSpeed = 20f;
            Item.value = CIShopValue.RarityMaliceDrop;
            Item.rare = CIConfig.Instance.LegendaryRarity ? ModContent.RarityType<SHPCAqua>() : ModContent.RarityType<MaliceChallengeDrop>();
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // 必须手动转换，不然会按照int进行加成
            float Buff = (float)((float)(BaseDamage + LegendaryBuff() + Generic.GenericLegendBuffInt()) / (float)BaseDamage);
            damage *= Buff;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player p = Main.LocalPlayer;
            var mp = p.CIMod();
            //升级的Tooltip:
            string t1 = mp.DestroyerTier1? Language.GetTextValue($"{TextRoute}.TierOne")    : Language.GetTextValue($"{TextRoute}.TierOneTint");
            tooltips.FindAndReplace("[TIERONE]", t1);
            string t2 = mp.DestroyerTier2? Language.GetTextValue($"{TextRoute}.TierTwo")    : Language.GetTextValue($"{TextRoute}.TierTwoTint");
            tooltips.FindAndReplace("[TIERTWO]", t2);
            string t3 = mp.DestroyerTier3? Language.GetTextValue($"{TextRoute}.TierThree")  : Language.GetTextValue($"{TextRoute}.TierThreeTint");
            tooltips.FindAndReplace("[TIERTHREE]", t3);
            //用于发送传奇武器在至尊灾厄眼在场时得到数值增强的信息
            string t4 = null;
            if (NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()))
                t4 = Language.GetTextValue($"{Generic.GetWeaponLocal}.EmpoweredTooltip.Generic");
            //以下，用于比较复杂的计算
            int boostPercent = LegendaryBuff() + Generic.GenericLegendBuffInt();
            string update = this.GetLocalization("LegendaryScaling").Format(
                boostPercent.ToString()
            );
            tooltips.FindAndReplace("[SCALING]", update);
            if (t4 != null)
            tooltips.Add(new TooltipLine(Mod, "Buff", t4));
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = Item.useAnimation = 6;
                Item.UseSound = CommonCalamitySounds.LaserCannonSound;
            }
            else
            {
                Item.useTime = Item.useAnimation = 60;
                Item.UseSound = SoundID.Item92;
            }
            return base.CanUseItem(player);
        }
        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            mult *= player.CIMod().DestroyerTier3 ? 0f : 0.3f;
        }

        public override Vector2? HoldoutOffset()
        {
            // 在设置一点一点调的偏移量
            return new Vector2(-33, -8);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var p = player.CIMod();
            int bCounts = 3;
            int lCounts = 3;
            if (player.altFunctionUse == 2)
            {
                for (int i = 0; i < lCounts; i++)
                {
                    float velX = velocity.X + Main.rand.Next(-20, 21) * 0.05f;
                    float velY = velocity.Y + Main.rand.Next(-20, 21) * 0.05f;
                    // 向上偏移和手持偏移一样的-8
                    Projectile.NewProjectile(source, position + new Vector2(0f, -8f), new(velX,velY), ModContent.ProjectileType<DestroyerLegendaryLaser>(), damage, knockback * 0.5f, player.whoAmI, 0f, 0f);
                }
                return false;
            }
            else
            {
                for (int j = 0; j < bCounts; j++)
                {
                    float velX = velocity.X + Main.rand.Next(-40, 41) * 0.05f;
                    float velY = velocity.Y + Main.rand.Next(-40, 41) * 0.05f;
                    Projectile.NewProjectile(source, position.X, position.Y - 8, velX, velY, ModContent.ProjectileType<DestroyerLegendaryBomb>(), (int)(damage * 1.1), knockback, player.whoAmI, 0f, 0f);
                }
                
                return false;
            }
        }
        public static int LegendaryBuff()
        {
            //SHPC时期较早，因此具备时期较多的伤害增长. 同时，此处也采用与叶流类似的机制——即使用加算，而非乘算的增伤
            // 砍了一刀增幅，后期太超模了
            int dmgBuff = 0;
            bool DownCalclone = DownedBossSystem.downedCalamitasClone || CIDownedBossSystem.DownedCalClone;
            dmgBuff += DownCalclone ? 2 : 0;   //27 - 19
            dmgBuff += Condition.DownedPlantera.IsMet() ? 3: 0;         //32 - 22
            dmgBuff += DownedBossSystem.downedLeviathan ? 3 : 0;        //37 - 25
            dmgBuff += DownedBossSystem.downedAstrumAureus? 3 : 0;      //42 - 28
            dmgBuff += Condition.DownedGolem.IsMet() ? 4 : 0;           //50 - 32
            dmgBuff += Condition.DownedEmpressOfLight.IsMet() ? 4 : 0; //60 - 36
            dmgBuff += Condition.DownedDukeFishron.IsMet() ? 4 : 0;    //70 - 40
            dmgBuff += DownedBossSystem.downedRavager ? 4 : 0;         //80 - 44
            dmgBuff += DownedBossSystem.downedPlaguebringer ? 4 : 0;   //90 - 48
            dmgBuff += Condition.DownedCultist.IsMet() ? 5 : 0;        //100 - 53
            //没有星神游龙是故意的，我不希望有人说在冲线阶段浪费时间打这个玩意
            dmgBuff += Condition.DownedMoonLord.IsMet() ? 47: 0;        //120 - 90
            dmgBuff += DownedBossSystem.downedGuardians ? 20 : 0;       //170 - 110
            dmgBuff += DownedBossSystem.downedProvidence ? 110 : 0;      //220 - 220
            dmgBuff += DownedBossSystem.downedSignus ? 10 : 0;          //250 - 230
            dmgBuff += DownedBossSystem.downedCeaselessVoid ? 10 : 0;   //280 - 240
            dmgBuff += DownedBossSystem.downedStormWeaver ? 10 : 0;     //310 - 250
            dmgBuff += DownedBossSystem.downedPolterghast ? 150 : 0;     //370 - 400
            dmgBuff += DownedBossSystem.downedBoomerDuke ? 40 : 0;     //470 - 440
            //我tm又忘记金龙了，不管了，fuckyou
            dmgBuff += DownedBossSystem.downedDragonfolly? 10 : 0;      //490 - 450
            dmgBuff += DownedBossSystem.downedDoG ? 460 : 0;            //610 - 800
            dmgBuff += DownedBossSystem.downedYharon ? 1460 : 0;         //760 - 2060
            dmgBuff += DownedBossSystem.downedCalamitas ? 100 : 0;      //960 - 2160
            dmgBuff += DownedBossSystem.downedExoMechs ? 100 : 0;       //1060 - 2260
            dmgBuff += DownedBossSystem.downedExoMechs && DownedBossSystem.downedCalamitas && DownedBossSystem.downedPrimordialWyrm && CIDownedBossSystem.DownedLegacyScal ? 500 : 0; //1560 - 2760
            return dmgBuff;
        }
    }
}