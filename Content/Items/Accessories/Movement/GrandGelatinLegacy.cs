using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Movement
{
    public class GrandGelatinLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Movement;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 32;
            Item.rare = ItemRarityID.LightRed;
            Item.value = CIShopValue.RarityPriceLightRed;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed += 0.1f;
            player.jumpSpeedBoost += player.autoJump ? 0.5f : 2.0f;
            player.statLifeMax2 += 20;
            player.statManaMax2 += 20;
            if ((double)Math.Abs(player.velocity.X) < 0.05 && (double)Math.Abs(player.velocity.Y) < 0.05 && player.itemAnimation == 0)
            {
                player.lifeRegen += 2;
                player.manaRegenBonus += 2;
            }
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe()
                    .AddIngredient(CalAccessories.CleansingJelly)
                    .AddIngredient(CalAccessories.LifeJelly)
                    .AddIngredient(CalAccessories.VitalJelly)
                    .AddIngredient(ItemID.SoulofLight, 4)
                    .AddIngredient(ItemID.SoulofNight, 4)
                    .AddTile(TileID.Anvils)
                    .Register();
            }
            else
            {
                CreateRecipe()
                    .AddIngredient(ItemID.Gel, 15)
                    .AddIngredient(ItemID.SoulofLight, 4)
                    .AddIngredient(ItemID.SoulofNight, 4)
                    .AddTile(TileID.Anvils)
                    .Register();
            }
        }
    }
}
