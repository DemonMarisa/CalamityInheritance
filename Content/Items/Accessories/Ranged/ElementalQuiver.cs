using CalamityInheritance.CIPlayer;
using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
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
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.accessory = true;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.defense = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
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
            player.pickSpeed -= 0.15f;
            if (!modPlayer.deadshotBrooch)
                modplayer.CIdeadshotBrooch = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MagicQuiver).
                AddIngredient<DaedalusEmblem>().
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe(). 
                AddIngredient(ItemID.MagicQuiver).
                AddIngredient<DeadshotBrooch>().
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register(); 
        }
    }
}
