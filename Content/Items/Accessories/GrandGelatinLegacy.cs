using System;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.Accessories;
using Terraria.ID;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class GrandGelatinLegacy : CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:20,
            itemHeight:24,
            itemRare:ItemRarityID.LightRed,
            itemValue:CIShopValue.RarityPriceLightRed
        );
        public override void ExSSD()
        {
            Type.ShimmerEach<GrandGelatin>();
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
            CreateRecipe()
                .AddIngredient<CleansingJelly>()
                .AddIngredient<LifeJelly>()
                .AddIngredient<VitalJelly>()
                .AddIngredient(ItemID.SoulofLight,4)
                .AddIngredient(ItemID.SoulofNight,4)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
