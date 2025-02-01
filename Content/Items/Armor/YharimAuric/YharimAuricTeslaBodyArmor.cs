using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Accessories;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Armor.YharimAuric
{
    [AutoloadEquip(EquipType.Body)]
    public class YharimAuricTeslaBodyArmor : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityMaliceDrop;
            Item.rare = ModContent.RarityType<MaliceChallengeDrop>();
            Item.defense = 48;
        }
        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += 600;
            player.statManaMax2 += 600;
            player.moveSpeed += 0.25f;
        }

        // public override void AddRecipes()
        // {
        //     Recipe recipe = CreateRecipe();
        //     recipe.AddIngredient(null, "SilvaArmor");
        //     recipe.AddIngredient(null, "GodSlayerChestplate");
        //     recipe.AddIngredient(null, "BloodflareBodyArmor");
        //     recipe.AddIngredient(null, "TarragonBreastplate");
        //     recipe.AddIngredient(null, "EndothermicEnergy", 400);
        //     recipe.AddIngredient(null, "NightmareFuel", 400);
        //     recipe.AddIngredient(null, "Phantoplasm", 140);
        //     recipe.AddIngredient(null, "DarksunFragment", 60);
        //     recipe.AddIngredient(null, "BarofLife", 40);
        //     recipe.AddIngredient(null, "CoreofCalamity", 30);
        //     recipe.AddIngredient(null, "GalacticaSingularity", 20);
        //     recipe.AddIngredient(null, "FrostBarrier");
        //     recipe.AddTile(null, "DraedonsForge");
        //     recipe.Register();
        // }
    }
}