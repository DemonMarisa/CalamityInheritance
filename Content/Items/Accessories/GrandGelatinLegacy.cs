using System;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.Accessories;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class GrandGelatinLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<GrandGelatin>()] = ModContent.ItemType<GrandGelatinLegacy>();
            ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<GrandGelatinLegacy>()] = ModContent.ItemType<GrandGelatin>();
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 24;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
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

            // if(CalamityInheritanceConfig.Instance.CustomShimmer == false) //微光嬗变config启用时，将会使原灾的血杯与这一速杀版本的血神核心微光相互转化
            // {
                CreateRecipe()
                    .AddIngredient<CleansingJelly>()
                    .AddIngredient<LifeJelly>()
                    .AddIngredient<VitalJelly>()
                    .AddIngredient(ItemID.SoulofLight,4)
                    .AddIngredient(ItemID.SoulofNight,4)
                    .AddTile(TileID.Anvils)
                    .Register();
            // }
        }
    }
}
