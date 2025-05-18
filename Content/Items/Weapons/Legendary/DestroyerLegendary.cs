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
        public static readonly int BaseDamage = 22;
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
            damage *= (BaseDamage + LegendaryBuff() + Generic.GenericLegendBuffInt()) / BaseDamage;
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
            int dmgBuff = 0;
            dmgBuff += DownedBossSystem.downedCalamitasClone ? 5 : 0;   //27
            dmgBuff += Condition.DownedPlantera.IsMet() ? 5: 0;         //32
            dmgBuff += DownedBossSystem.downedLeviathan ? 5 : 0;        //37
            dmgBuff += DownedBossSystem.downedAstrumAureus? 5 : 0;      //42
            dmgBuff += Condition.DownedGolem.IsMet() ? 8 : 0;           //50
            dmgBuff += Condition.DownedEmpressOfLight.IsMet() ? 10 : 0; //60
            dmgBuff += Condition.DownedDukeFishron.IsMet() ? 10 : 0;    //70
            dmgBuff += DownedBossSystem.downedRavager ? 10 : 0;         //80
            dmgBuff += DownedBossSystem.downedPlaguebringer ? 10 : 0;   //90
            dmgBuff += Condition.DownedCultist.IsMet() ? 10 : 0;        //100
            //没有星神游龙是故意的，我不希望有人说在冲线阶段浪费时间打这个玩意
            dmgBuff += Condition.DownedMoonLord.IsMet() ? 20: 0;        //120
            dmgBuff += DownedBossSystem.downedGuardians ? 50 : 0;       //170
            dmgBuff += DownedBossSystem.downedProvidence ? 50 : 0;      //220
            dmgBuff += DownedBossSystem.downedSignus ? 30 : 0;          //250
            dmgBuff += DownedBossSystem.downedCeaselessVoid ? 30 : 0;   //280
            dmgBuff += DownedBossSystem.downedStormWeaver ? 30 : 0;     //310
            dmgBuff += DownedBossSystem.downedPolterghast ? 60 : 0;     //370
            dmgBuff += DownedBossSystem.downedBoomerDuke ? 100 : 0;     //470
            //我tm又忘记金龙了，不管了，fuckyou
            dmgBuff += DownedBossSystem.downedDragonfolly? 20 : 0;      //490
            dmgBuff += DownedBossSystem.downedDoG ? 120 : 0;            //610
            dmgBuff += DownedBossSystem.downedYharon ? 150 : 0;         //760
            dmgBuff += DownedBossSystem.downedCalamitas ? 200 : 0;      //960
            dmgBuff += DownedBossSystem.downedExoMechs ? 200 : 0;       //1060
            dmgBuff += DownedBossSystem.downedExoMechs && DownedBossSystem.downedCalamitas && DownedBossSystem.downedPrimordialWyrm ? 500 : 0; //1560
            dmgBuff += CIDownedBossSystem.DownedLegacyScal ? 500 : 0;
            return dmgBuff;
        }
    }
}