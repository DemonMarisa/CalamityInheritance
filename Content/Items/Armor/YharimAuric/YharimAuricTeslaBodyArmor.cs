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
using CalamityInheritance.Content.Items.Materials;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.Content.Items.Armor.AuricTesla;
using CalamityInheritance.Tiles.Furniture.CraftingStations;

namespace CalamityInheritance.Content.Items.Armor.YharimAuric
{
    [AutoloadEquip(EquipType.Body)]
    public class YharimAuricTeslaBodyArmor : ModItem, ILocalizedModType
    {
        public override void Load()
        {
            // All code below runs only if we're not loading on a server
            if (Main.netMode == NetmodeID.Server)
                return;
            // Add equip textures
            EquipLoader.AddEquipTexture(Mod, "CalamityInheritance/Content/Items/Armor/YharimAuric/YharimAuricTeslaBodyArmor_Back", EquipType.Back, this);
        }

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

        public override void AddRecipes()
        {
            if (CalamityInheritanceConfig.Instance.LegendaryitemsRecipes == true)
            {
                CreateRecipe().
                AddIngredient<AuricTeslaBodyArmorold>().
                AddIngredient<YharimsGiftLegacy>().
                AddIngredient<AuricBarold>(18).
                AddTile<DraedonsForgeold>().
                Register();
            }
        }
    }
}