using System;
using System.Collections.Generic;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientGodSlayer
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientGodSlayerHelm : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 40; //120
        }
        

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            bool isSet = body.type == ModContent.ItemType<AncientGodSlayerChestplate>() && legs.type == ModContent.ItemType<AncientGodSlayerLeggings>();
            return isSet;
        }
        
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            CalamityInheritancePlayer usPlayer = player.CalamityInheritance();
            CalamityPlayer calPlayer = player.Calamity();
            calPlayer.wearingRogueArmor = true;
            calPlayer.WearingPostMLSummonerSet = true;
            calPlayer.rogueStealthMax = 1.25f;
            usPlayer.AncientGodSlayerSet = true;
            usPlayer.AncientGodSlayerStat = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");
        }
        
        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 5;
            player.GetDamage<GenericDamageClass>() += 0.25f;
            player.GetCritChance<GenericDamageClass>() += 0.25f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GodSlayerHeadMelee>().
                AddIngredient<GodSlayerHeadRanged>().
                AddIngredient<GodSlayerHeadRogue>().
                AddIngredient<CosmiliteBar>(25).
                AddIngredient<AscendantSpiritEssence>(10).
                AddTile<CosmicAnvil>().
                Register();
                
        }
    }
}