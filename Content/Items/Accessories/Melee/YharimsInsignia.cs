﻿using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Accessories.Melee
{
    public class YharimsInsignia : CIAccessories, ILocalizedModType
    {

        public new string LocalizationCategory => "Content.Items.Accessories.Melee";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 38;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<BlueGreen>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer modPlayer = player.CIMod();
            modPlayer.YharimsInsignia = true;
            player.lavaMax = 600;
            player.GetDamage<MeleeDamageClass>() += 0.15f;
            player.GetDamage<TrueMeleeDamageClass>() += 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WarriorEmblem).
                AddIngredient<NecklaceofVexation>().
                AddRecipeGroup(CIRecipeGroup.BadgeofBravery).
                AddIngredient<CoreofSunlight>(5).
                AddIngredient<DivineGeode>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
