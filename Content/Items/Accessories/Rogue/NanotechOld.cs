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

namespace CalamityInheritance.Content.Items.Accessories.Rogue
{
    public class NanotechOld : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 46;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<DarkBlue>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.nanotech = true;
            modPlayer.raiderTalisman = true;
            modPlayer.electricianGlove = true;
            modPlayer.filthyGlove = true;
            modPlayer.bloodyGlove = true;
            player.GetDamage<ThrowingDamageClass>() += 0.15f;
            player.GetCritChance<ThrowingDamageClass>() += 0.10f;
            player.Calamity().rogueVelocity += 0.15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<RogueEmblem>().
                AddIngredient<RaidersTalisman>().
                AddIngredient<MoonstoneCrown>().
                AddIngredient<ElectriciansGlove>().
                AddIngredient(ItemID.LunarBar, 8).
                AddIngredient<GalacticaSingularity>(4).
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
