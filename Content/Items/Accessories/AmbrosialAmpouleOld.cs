using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class AmbrosialAmpouleOld : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.defense = 4;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            CalamityInheritancePlayer modPlayer1 = player.CalamityInheritance();
            // bool left in for abyss light purposes and life regen effects
            player.pickSpeed = 0.5f;
            player.endurance = 0.05f;
            // Inherits all effects of Honey Dew and Living Dew (except standing regen is not honey exclusive anymore)
            modPlayer.alwaysHoneyRegen = true;
            modPlayer.honeyDewHalveDebuffs = true;
            modPlayer.livingDewHalveDebuffs = true;
            modPlayer1.beeResist = true;
            modPlayer1.AmbrosialAmpouleOld = true;

            player.buffImmune[BuffID.Venom] = true;
            player.buffImmune[BuffID.Frozen] = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frostburn] = true;
            player.buffImmune[BuffID.Poisoned] = true;

            // Add light if the other accessories aren't equipped and visibility is turned on
            if (!(modPlayer.rOoze || modPlayer.purity) && !hideVisual)
                Lighting.AddLight(player.Center, new Vector3(1.2f, 1.2f, 0.72f));
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CrimsonFlask>().
                AddIngredient<ArchaicPowder>().
                AddIngredient<RadiantOoze>().
                AddIngredient<HoneyDew>().
                AddIngredient<StarblightSoot>(15).
                AddIngredient<CryonicBar>(5).
                AddTile(TileID.MythrilAnvil).
                Register();

            CreateRecipe().
                AddIngredient<CorruptFlask>().
                AddIngredient<ArchaicPowder>().
                AddIngredient<RadiantOoze>().
                AddIngredient<HoneyDew>().
                AddIngredient<StarblightSoot>(15).
                AddIngredient<CryonicBar>(5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
