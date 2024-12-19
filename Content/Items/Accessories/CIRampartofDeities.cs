using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;

namespace CalamityInheritance.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class CIRampartofDeities : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 62;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.defense = 18;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<Violet>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.dAmulet = true;
            player.longInvince = true;
            player.pStone = true;
            player.lifeRegen += 4;

            if (player.statLife <= player.statLifeMax2 * 0.5)
                player.AddBuff(BuffID.IceBarrier, 5);

            player.noKnockback = true;
            if (player.statLife > player.statLifeMax2 * 0.25f)
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
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.FrozenShield).
                AddIngredient<DeificAmulet>().
                AddIngredient<AuricBar>(5).
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
