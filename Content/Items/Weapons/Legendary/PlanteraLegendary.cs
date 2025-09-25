using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Rarity;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;
using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.NPCs.Boss.SCAL;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    public class PlanteraLegendary: CIRanged, ILocalizedModType
    {
        
        public static string TextRoute => $"{Generic.WeaponTextPath}Ranged.PlanteraLegendary"; 
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public static readonly int BaseDamage = 8;
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 62;
            Item.damage = BaseDamage;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 2;
            Item.useAnimation = 2;
            Item.knockBack = 0.15f;
            Item.shootSpeed = 12f;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = CISoundID.SoundBow;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<PlanteraLegendaryLeaf>();
            Item.useAmmo = AmmoID.Arrow;
            Item.value = CIShopValue.RarityMaliceDrop;
            Item.rare = CIConfig.Instance.LegendaryRarity ? ModContent.RarityType<PlanteraGreen>() : ModContent.RarityType<MaliceChallengeDrop>();
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // 必须手动转换，不然会按照int进行加成
            float Buff = (float)((float)(BaseDamage + LegendaryBuff() + Generic.GenericLegendBuffInt()) / (float)BaseDamage);
            damage *= Buff;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player p = Main.LocalPlayer;
            var mp = p.CIMod();
            //升级的Tooltip:
            string t1 = mp.PlanteraTier1 ? Language.GetTextValue($"{TextRoute}.TierOne") : Language.GetTextValue($"{TextRoute}.TierOneTint");
            tooltips.FindAndReplace("[TIERONE]", t1);
            string t2 = mp.PlanteraTier2 ? Language.GetTextValue($"{TextRoute}.TierTwo") : Language.GetTextValue($"{TextRoute}.TierTwoTint");
            tooltips.FindAndReplace("[TIERTWO]", t2);
            string t3 = mp.PlanteraTier3 ? Language.GetTextValue($"{TextRoute}.TierThree") : Language.GetTextValue($"{TextRoute}.TierThreeTint");
            tooltips.FindAndReplace("[TIERTHREE]", t3);
            //用于发送传奇武器在至尊灾厄眼在场时得到数值增强的信息
            string t4 = null;
            if (NPC.AnyNPCs(ModContent.NPCType<SupremeCalamitasLegacy>()))
                t4 = Language.GetTextValue($"{Generic.WeaponTextPath}EmpoweredTooltip.Generic");
            //以下，用于比较复杂的计算
            int boostPercent = LegendaryBuff() + Generic.GenericLegendBuffInt();
            string update = this.GetLocalization("LegendaryScaling").Format(
                boostPercent.ToString()
            );
            tooltips.FindAndReplace("[SCALING]", update);
            if (t4 != null)
            tooltips.Add(new TooltipLine(Mod, "Buff", t4));
        }
        public override Vector2? HoldoutOffset()
        {
            // 偏移量
            return new Vector2(-10, 0);
        }
        public override bool CanUseItem(Player player)
        {
            var p = player.CIMod();
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 25;
                Item.useAnimation = 25;
                Item.UseSound = SoundID.Item77;
                Item.ArmorPenetration = p.PlanteraTier1 ? 50 : 0;
            }
            else
            {
                Item.useTime = p.PlanteraTier1 ? 1 : 2;
                Item.useAnimation = p.PlanteraTier1 ? 5 : 10;
                Item.UseSound = SoundID.Item5;
            }
            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                    //升级2启用时发射5份炸弹
                if (player.CIMod().PlanteraTier2)
                {
                    int pCounts = 5;
                    const float offset = 0.48f; 
                    Vector2 summonP = velocity;
                    summonP.Normalize();
                    summonP *= 36f;
                    for (int i = 0; i < pCounts; i++)
                    {
                        float homeAi = player.CIMod().PlanteraTier3 ? 1f : 0f;
                        float piArrowOffset = i - (pCounts - 1) / 2;
                        if (piArrowOffset == 1/2) piArrowOffset += 1/2;
                        Vector2 spawn = summonP.RotatedBy(offset * piArrowOffset, new Vector2());
                        Projectile.NewProjectile(source, position.X + spawn.X, position.Y + spawn.Y, velocity.X, velocity.Y, ModContent.ProjectileType<PlanteraLegendaryBomb>(), damage * 4, knockback * 60, player.whoAmI, 0f, 0f, homeAi);
                    }
                }
                else
                {
                    float homeAi = player.CIMod().PlanteraTier3 ? 1f : 0f;
                    Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<PlanteraLegendaryBomb>(), damage * 4, knockback * 60, player.whoAmI, 0f, 0f, homeAi);
                }
            }
            else
            {
                //升级2启用时发射两份树叶
                if (player.CIMod().PlanteraTier2)
                {
                    int pCounts = 2;
                    Vector2 summonP = velocity;
                    float rot = velocity.ToRotation();
                    Vector2 Offset = new Vector2(5, -5).RotatedBy(rot);
                    Vector2 Offset2 = new Vector2(5, 5).RotatedBy(rot);
                    summonP.Normalize();
                    summonP *= 36f;
                    for (int i = 0; i < pCounts; i++)
                    {
                        float homeAi = player.CIMod().PlanteraTier3 && Main.rand.NextBool(2) ? 1f : 0f;
                        Projectile.NewProjectile(source, position + (i == 0 ? Offset : Offset2), summonP, ModContent.ProjectileType<PlanteraLegendaryLeaf>(), damage, knockback, player.whoAmI, 0f, 0f, homeAi);
                    }
                }
                else 
                {
                    float homeAi = player.CIMod().PlanteraTier3 && Main.rand.NextBool(2) ? 1f : 0f;
                    Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<PlanteraLegendaryLeaf>(), damage, knockback, player.whoAmI ,0f, 0f, homeAi);
                }
            }
            return false;
        }
        public static int LegendaryBuff()
        {
            //SHPC时期较早，因此具备时期较多的伤害增长. 同时，此处也采用与叶流类似的机制——即使用加算，而非乘算的增伤
            // 砍了一刀增幅，后期太超模了
            int dmgBuff = 0;
            dmgBuff += DownedBossSystem.downedLeviathan ? 1 : 0;        // 9
            dmgBuff += DownedBossSystem.downedAstrumAureus ? 1 : 0;     // 10
            dmgBuff += Condition.DownedGolem.IsMet() ? 2 : 0;           // 12
            dmgBuff += Condition.DownedEmpressOfLight.IsMet() ? 2 : 0;  // 14
            dmgBuff += Condition.DownedDukeFishron.IsMet() ? 2 : 0;     // 16
            dmgBuff += DownedBossSystem.downedRavager ? 2 : 0;          // 18
            dmgBuff += DownedBossSystem.downedPlaguebringer ? 2 : 0;    // 20
            dmgBuff += Condition.DownedCultist.IsMet() ? 3 : 0;         // 23
            dmgBuff += DownedBossSystem.downedAstrumDeus ? 2 : 0;       // 25
            dmgBuff += Condition.DownedMoonLord.IsMet() ? 5 : 0;        // 30
            dmgBuff += DownedBossSystem.downedGuardians ? 2 : 0;        // 32
            dmgBuff += DownedBossSystem.downedDragonfolly ? 3 : 0;      // 35
            dmgBuff += DownedBossSystem.downedProvidence ? 25 : 0;      // 60
            dmgBuff += DownedBossSystem.downedSignus ? 2 : 0;           // 62
            dmgBuff += DownedBossSystem.downedCeaselessVoid ? 2 : 0;    // 64
            dmgBuff += DownedBossSystem.downedStormWeaver ? 1 : 0;      // 65
            dmgBuff += DownedBossSystem.downedPolterghast ? 30 : 0;     // 95
            dmgBuff += DownedBossSystem.downedBoomerDuke ? 5 : 0;       // 100
            dmgBuff += DownedBossSystem.downedDoG ? 136 : 0;            // 236
            dmgBuff += DownedBossSystem.downedYharon ? 476 : 0;         // 712
            dmgBuff += DownedBossSystem.downedCalamitas ? 25 : 0;       // 737
            dmgBuff += DownedBossSystem.downedExoMechs ? 25 : 0;        // 762
            dmgBuff += DownedBossSystem.downedExoMechs && DownedBossSystem.downedCalamitas && DownedBossSystem.downedPrimordialWyrm && CIDownedBossSystem.DownedLegacyScal ? 800 : 0;
            return dmgBuff;
        }
    }
}