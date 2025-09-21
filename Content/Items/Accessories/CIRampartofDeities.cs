﻿using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Accessories;
using CalamityMod;

namespace CalamityInheritance.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class CIRampartofDeities : CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:64,
            itemHeight:62,
            itemRare:ModContent.RarityType<CatalystViolet>(),
            itemValue:CIShopValue.RarityPriceCatalystViolet,
            itemDefense:18
        );
        public override void ExSSD() => Type.ShimmerEach<RampartofDeities>();
        public override bool CanEquipAccessory(Player player, int slot, bool modded) => !player.Calamity().rampartOfDeities;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var usPlayer= player.CIMod();
            player.pStone = true;
            player.longInvince =true;
            usPlayer.RoDPaladianShieldActive = true; //启用帕拉丁盾
            player.lifeRegen += 4;
            player.GetArmorPenetration<GenericDamageClass>() += 50;
            usPlayer.RampartOfDeitiesStar = true;
            if (player.statLife <= player.statLifeMax2 * 0.5)
                player.AddBuff(BuffID.IceBarrier, 5);
            player.noKnockback = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(). //
                AddIngredient(ItemID.FrozenShield).
                AddIngredient<CosmiliteBar>(10).
                AddRecipeGroup("CalamityInheritance:AnyDeificAmulet").
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
