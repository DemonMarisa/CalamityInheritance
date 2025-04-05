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
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class PlanteraLegendary: CIRanged, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Ranged";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public static readonly int BaseDamage = 25;
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 62;
            Item.damage = BaseDamage;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 2;
            Item.useAnimation = 2;
            Item.knockBack = 0.15f;
            Item.shootSpeed = 10f;
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
            damage *= (BaseDamage + LegendaryDamageBuff()) / BaseDamage;
            if (player.CIMod().PlanteraTier1)
                //叶流变成1攻速的时候面板会被下调90%
                damage *= 0.90f;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player p = Main.LocalPlayer;
            var mp = p.CIMod();
            //升级的Tooltip:
            if (mp.PlanteraTier1)
            {
                string t1 = Language.GetTextValue($"{Generic.GetWeaponLocal}.Ranged.PlanteraLegendary.TierOne");
                tooltips.Add(new TooltipLine(Mod, "TIERONE", t1));
            }
            if (mp.PlanteraTier2)
            {
                string t2 = Language.GetTextValue($"{Generic.GetWeaponLocal}.Ranged.PlanteraLegendary.TierTwo");
                tooltips.Add(new TooltipLine(Mod, "TIERTWO", t2));
            }
            if (mp.PlanteraTier1)
            {
                string t3 = Language.GetTextValue($"{Generic.GetWeaponLocal}.Ranged.PlanteraLegendary.TierThree");
                tooltips.Add(new TooltipLine(Mod, "TIERTHREE", t3));
            }
            //以下，用于比较复杂的计算
            int boostPercent = LegendaryDamageBuff();
            string update = this.GetLocalization("LegendaryScaling").Format(
                boostPercent.ToString()
            );
            tooltips.FindAndReplace("[SCALING]", update);
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
                Item.useAnimation = p.PlanteraTier1 ? 1 : 2;
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
                    const float offset = 0.48f; 
                    Vector2 summonP = velocity;
                    summonP.Normalize();
                    summonP *= 36f;
                    for (int i = 0; i < pCounts; i++)
                    {
                        float homeAi = player.CIMod().PlanteraTier3 && Main.rand.NextBool(2) ? 1f : 0f;
                        float piArrowOffset = i - (pCounts - 1) / 2;
                        Vector2 spawn = summonP.RotatedBy(offset * piArrowOffset, new Vector2());
                        Projectile.NewProjectile(source, position.X + spawn.X, position.Y + spawn.Y, velocity.X, velocity.Y, ModContent.ProjectileType<PlanteraLegendaryLeaf>(), damage, knockback, player.whoAmI, 0f, 0f, homeAi);
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
        public static int LegendaryDamageBuff()
        {
            int damageBuff = 0;
            damageBuff += Condition.DownedGolem.IsMet()         ? 5 : 0;        //5 (30)
            damageBuff += Condition.DownedDukeFishron.IsMet()   ? 5 : 0;       //10 (35)
            damageBuff += DownedBossSystem.downedPlaguebringer  ? 10 : 0;       //20 (45)
            damageBuff += Condition.DownedCultist.IsMet()       ? 10 : 0;       //30 (55)
            damageBuff += Condition.DownedMoonLord.IsMet()      ? 10 : 0;       //40 (65)
            damageBuff += DownedBossSystem.downedProvidence     ? 10 : 0;       //50 (75)
            damageBuff += DownedBossSystem.downedPolterghast    ? 10 : 0;       //60 (85)
            damageBuff += DownedBossSystem.downedDoG            ? 20 : 0;       //80 (105)
            //叶流成长在龙的时期停止
            damageBuff += DownedBossSystem.downedYharon         ? 20 : 0;       //100 (135)
            return damageBuff;
        }
    }
}