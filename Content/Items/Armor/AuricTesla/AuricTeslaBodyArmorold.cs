﻿using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Armor.Silva;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories;
using CalamityInheritance.Content.Items.Materials;

namespace CalamityInheritance.Content.Items.Armor.AuricTesla
{
    [AutoloadEquip(EquipType.Body)]
    public class AuricTeslaBodyArmorold : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Armor.PostMoonLord";
        public override void Load()
        {
            // All code below runs only if we're not loading on a server
            if (Main.netMode != NetmodeID.Server)
            {
                // Add equip textures
                EquipLoader.AddEquipTexture(Mod, "CalamityInheritance/Content/Items/Armor/AuricTesla/AuricTeslaBodyArmorold_Back", EquipType.Back, this);
            }
        }

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 34;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.defense = 48;
            Item.rare = ModContent.RarityType<Violet>();
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = player.Calamity();
            CalamityInheritancePlayer modPlayer1 = player.CalamityInheritance();
            modPlayer1.godSlayerReflect = true;
            modPlayer1.GodSlayerDMGprotect = true;
            modPlayer.fBarrier = true;
            player.statLifeMax2 += 100;
            player.GetDamage<GenericDamageClass>() += 0.08f;
            player.GetCritChance<GenericDamageClass>() += 5;
            player.moveSpeed += 0.25f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GodSlayerChestplate>().
                AddIngredient<SilvaArmor>().
                AddIngredient<BloodflareBodyArmor>().
                AddIngredient<TarragonBreastplate>().
                AddIngredient<FrostBarrier>().
                AddIngredient<AuricBarold>(2).
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddIngredient<GodSlayerChestplate>().
                AddIngredient<SilvaArmor>().
                AddIngredient<BloodflareBodyArmor>().
                AddIngredient<TarragonBreastplate>().
                AddIngredient<FrostBarrier>().
                AddIngredient<AuricBar>(18).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
