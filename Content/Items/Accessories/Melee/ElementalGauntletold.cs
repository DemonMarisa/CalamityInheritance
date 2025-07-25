﻿using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Melee
{

    [AutoloadEquip([EquipType.HandsOn, EquipType.HandsOff])]
    public class ElementalGauntletold : CIAccessories, ILocalizedModType
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
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.accessory = true;
            Item.defense = 10;
            Item.rare = ModContent.RarityType<DeepBlue>();
        }
        public override bool CanEquipAccessory(Player player, int slot, bool modded) => !player.Calamity().eGauntlet;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer1 = player.CIMod();
            modPlayer1.ElemGauntlet = true;
            modPlayer1.YharimsInsignia = true;
            player.GetDamage<MeleeDamageClass>() += 0.30f;
            player.GetCritChance<MeleeDamageClass>() += 15;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.15f;
            player.kbGlove = true;
            player.autoReuseGlove = true;
            player.meleeScaleGlove = true;
            player.GetDamage<TrueMeleeDamageClass>() += 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.FireGauntlet).
                AddIngredient(ItemID.LunarBar, 8).
                AddIngredient<YharimsInsignia>().
                AddIngredient<GalacticaSingularity>(4).
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
