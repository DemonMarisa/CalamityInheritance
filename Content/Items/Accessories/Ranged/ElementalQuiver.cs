﻿using CalamityInheritance.CIPlayer;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Ranged
{
    public class ElementalQuiver : CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.Ranged";
        public override void SetStaticDefaults()
        {
            if(CIServerConfig.Instance.CustomShimmer == true) //微光嬗变config启用时，肉后的天蓝石将会与本mod的天蓝石转化，关闭时则由沙虫正常掉落
            {
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<CalamityMod.Items.Accessories.ElementalQuiver>()] = ModContent.ItemType<ElementalQuiver>();
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<ElementalQuiver>()] = ModContent.ItemType<CalamityMod.Items.Accessories.ElementalQuiver>();
            }
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.accessory = true;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.defense = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer1 = player.Calamity();
            CalamityInheritancePlayer modplayer = player.GetModPlayer<CalamityInheritancePlayer>();
            if (CIConfig.Instance.ElementalQuiversplit == true)
            {
                modplayer.ElemQuiver= true;
            }
            else
            {
                modplayer.ElemQuiver= false;
            }
            player.GetDamage(DamageClass.Ranged) += 0.20f;
            player.GetCritChance(DamageClass.Ranged) += 15;
            player.ammoCost80 = true;
            player.lifeRegen += 4;
            player.pickSpeed -= 0.15f;
            if (!modPlayer1.deadshotBrooch)
                modplayer.DeadshotBroochCI = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MagicQuiver).
                AddIngredient<DaedalusEmblem>().
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register();

                CreateRecipe(). 
                    AddIngredient(ItemID.MagicQuiver).
                    AddIngredient<DeadshotBrooch>().
                    AddIngredient<AscendantSpiritEssence>(4).
                    AddTile<CosmicAnvil>().
                    Register(); 
        }
    }
}
