using System;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.Accessories;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class GrandGelatinLegacy : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 15, 0, 0);
            Item.rare = 6;
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
            CreateRecipe()
            .AddIngredient<GrandGelatin>(1)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
