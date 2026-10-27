using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Armor.ArmorItems.BloodflareLegacy;
using CalamityInheritance.Content.Items.Armor.ArmorItems.GodSlayerOld;
using CalamityInheritance.Content.Items.Armor.ArmorItems.Silva;
using CalamityInheritance.Content.Items.Armor.ArmorItems.TarragonLegacy;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.CraftingStations;
using CalamityInheritance.Core.Graphics.Insertlayer;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.AuricTeslaLegacy
{
    [AutoloadEquip(EquipType.Body)]
    public class AuricTeslaBodyArmorold : CIArmor
    {
        public override void Load()
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            EquipLoader.AddEquipTexture(Mod, $"{Texture}_Back", EquipType.Back, this);
        }

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 34;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.defense = 48;
            Item.rare = RarityType<CatalystViolet>();
        }
        public override void UpdateEquip(Player player)
        {
            player.back = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Back);
            player.CI().GodSlayerSet = true;
            player.statLifeMax2 += 100;
            player.GetDamage<GenericDamageClass>() += 0.08f;
            player.GetCritChance<GenericDamageClass>() += 5;
            player.moveSpeed += 0.25f;
        }
        public override void ArmorSetShadows(Player player)
        {
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GodSlayerChestplateold>().
                AddIngredient<SilvaArmorold>().
                AddIngredient<BloodflareBodyLegacy>().
                AddIngredient<TarragonBreastplateLegacy>().
                AddIngredient<AuricBarold>(2).
                AddTile<DraedonsForgeoldTile>().
                Register();
        }
    }
}
