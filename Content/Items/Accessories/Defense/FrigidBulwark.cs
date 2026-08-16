using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity.ShopValue;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Defense
{
    internal class FrigidBulwark : CIAccessories
    {
        public override int AccessoriesStyle => Defense;
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 40;
            Item.value = CIShopValue.RarityPriceLime;
            Item.rare = ItemRarityID.Lime;
            Item.defense = 12;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.resistCold = true;
            player.buffImmune[BuffID.Frostburn] = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frozen] = true;
            if (player.statLife >= player.statLifeMax2 * 0.25f)
            {
                player.hasPaladinShield = true;
                if (player.whoAmI != Main.myPlayer && player.miscCounter % 10 == 0)
                {
                    int myPlayer = Main.myPlayer;
                    if (Main.player[myPlayer].team == player.team && player.team != 0)
                    {
                        float teamPlayerXDist = player.position.X - Main.player[myPlayer].position.X;
                        float teamPlayerYDist = player.position.Y - Main.player[myPlayer].position.Y;
                        if ((float)Math.Sqrt(teamPlayerXDist * teamPlayerXDist + teamPlayerYDist * teamPlayerYDist) < 800f)
                            Main.player[myPlayer].AddBuff(BuffID.PaladinsShield, 20);
                    }
                }
            }
            player.noKnockback = true;
            if (player.statLife > player.statLifeMax2 * 0.5)
                player.GetDamage<GenericDamageClass>() += 0.1f;
            if (player.statLife <= player.statLifeMax2 * 0.5)
            {
                player.statDefense += 20;
                player.AddBuff(BuffID.IceBarrier, 5);
            }
            if (player.statLife <= player.statLifeMax2 * 0.15)
                player.AddDR(0.5f);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.FrozenShield).
                // AddIngredient(ItemType<FrostFlare>()).
                AddIngredient(ItemType<CoreofEleum>(), 5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
