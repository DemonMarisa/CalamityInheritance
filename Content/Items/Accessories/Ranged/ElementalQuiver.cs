using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Ranged
{
    public class ElementalQuiver : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Elemental Quiver");
            /* Tooltip.SetDefault("Ranged projectiles have a chance to split\n" +
                "Ranged weapons have a chance to instantly kill normal enemies\n" +
                "10% increased ranged damage, 5% increased ranged critical strike chance, and 20% reduced ammo usage\n" +
                "5 increased defense, 2 increased life regen, and 15% increased pick speed\n" +
                "Greatly increases arrow speed and grants a 20% chance to not consume arrows"); */
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            Item.rare = ModContent.RarityType<DarkBlue>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer modplayer = player.GetModPlayer<CalamityInheritancePlayer>();
            if (CalamityInheritanceConfig.Instance.ElementalQuiversplit == true)
            {
                modplayer.ElementalQuiver = true;
            }
            else
            {
                modplayer.ElementalQuiver = false;
            }
            player.GetDamage(DamageClass.Ranged) += 0.20f;
            player.GetCritChance(DamageClass.Ranged) += 15;
            player.ammoCost80 = true;
            player.lifeRegen += 4;
            player.statDefense += 30;
            player.pickSpeed -= 0.15f;
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.deadshotBrooch = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MagicQuiver);
            recipe.AddIngredient(ModContent.ItemType<DaedalusEmblem>());
            recipe.AddIngredient(ModContent.ItemType<AscendantSpiritEssence>(), 4);
            recipe.AddTile(ModContent.TileType<CosmicAnvil>());
            recipe.Register();
        }
    }
}
