using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod;
using System;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Buffs.Summon;
using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using System.Collections.Generic;
using CalamityInheritance.Content.Items.Weapons.Summon.Worms;
using CalamityInheritance.Content.Projectiles.Summon.Worms;
using CalamityInheritance.Buffs.Summon;
namespace CalamityInheritance.Content.Items.Armor.GodSlayerOld
{
    [AutoloadEquip(EquipType.Head)]
    public class GodSlayerHeadSummonold : CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 29; //96
            Item.rare = RarityType<DeepBlue>();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<GodSlayerChestplateold>() && legs.type == ItemType<GodSlayerLeggingsold>();

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
        public override void ModifyTooltips(List<TooltipLine> list) => list.IntegrateHotkey(CalamityKeybinds.GodSlayerDashHotKey);
        public override void UpdateArmorSet(Player player)
        {
            CalamityInheritancePlayer usPlayer = player.GetModPlayer<CalamityInheritancePlayer>();
            var calPlayer = player.Calamity();
            calPlayer.godSlayer = true;
            usPlayer.GodSlayerSummonSet = true;

            player.setBonus = this.GetLocalizedValue("SetBonus");
            usPlayer.GodSlayerReborn = true;

            calPlayer.WearingPostMLSummonerSet = true;
            usPlayer.CanUseLegacyGodSlayerDash = true;
            if (player.whoAmI == Main.myPlayer)
            {
                if (CIConfig.Instance.GodSlayerWorm)
                {
                    if (player.ownedProjectileCounts[ProjectileType<DOGworm_Auric>()] < 1)
                    {
                        ModItem item = ItemLoader.GetItem(ItemType<StaffofDOG>());
                        int p = Projectile.NewProjectile(player.GetSource_ItemUse(item.Item), player.Center + new Vector2(600, 300), Vector2.UnitX, ProjectileType<DOGworm_Auric>(), StaffofDOG.BaseDamage, 1, player.whoAmI);
                        Main.projectile[p].originalDamage = StaffofDOG.BaseDamage;
                    }
                }
            }
        }
 
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<SummonDamageClass>() += 0.65f;
            player.maxMinions += 3;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CosmiliteBar>(7).
                AddIngredient<AscendantSpiritEssence>(2).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
