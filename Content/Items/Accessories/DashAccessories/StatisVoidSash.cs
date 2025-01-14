using CalamityInheritance.Content.Items.Materials;
using CalamityMod.CalPlayer;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Items.Materials;
using CalamityMod;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using Terraria.ID;
using Terraria.DataStructures;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer.Dash;

namespace CalamityInheritance.Content.Items.Accessories.DashAccessories
{
    public class StatisVoidSash : ModItem
    {
        public new string LocalizationCategory => "Items.Accessories";
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 3));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.accessory = true;
            Item.value= CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
        }
        
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer modPlayer1 = player.CalamityInheritance();
            CalamityPlayer modPlayer = player.Calamity();
            player.GetDamage<GenericDamageClass>() += 0.12f;
            player.GetCritChance<GenericDamageClass>() += 8;
            modPlayer.nucleogenesis = true;
            player.jumpSpeedBoost += 3.2f;
            player.moveSpeed += 0.10f;
            player.spikedBoots = 2;
            player.noFallDmg = true;
            player.blackBelt = true;
            player.autoJump = true;
            player.Calamity().DashID = string.Empty;
            player.dashType = 0;
            modPlayer1.CIDashID = StatisVoidSashDashOld.ID;   
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.EyeoftheGolem)
            .AddIngredient<Nucleogenesis>()
            .AddIngredient<StatisNinjaBeltLegacy>()
            .AddTile<CosmicAnvil>()
            .Register();

            CreateRecipe()
            .AddIngredient<CalamityMod.Items.Accessories.StatisVoidSash>()
            .AddIngredient<Nucleogenesis>()
            .AddIngredient(ItemID.DestroyerEmblem)
            .AddTile<CosmicAnvil>()
            .Register();
        }
    }
}
