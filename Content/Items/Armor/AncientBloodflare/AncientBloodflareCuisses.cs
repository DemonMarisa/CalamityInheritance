using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Armor.Bloodflare;

namespace CalamityInheritance.Content.Items.Armor.AncientBloodflare
{
[AutoloadEquip(EquipType.Legs)]
public class AncientBloodflareCuisses : CIArmor, ILocalizedModType
{
    
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }
    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 18;
        Item.value = CIShopValue.RarityPriceBlueGreen;
        Item.rare= RarityType<BlueGreen>();
        Item.defense = 20;
    }
    
    public override void UpdateEquip(Player player)
    {
    	player.moveSpeed += 0.3f;
        player.statLifeMax2 += 100;
    	player.lavaImmune = true;
    	player.ignoreWater = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe().
            AddIngredient<BloodflareCuisses>(1).
            AddIngredient<BloodstoneCore>(10).
            AddIngredient<RuinousSoul>(10).
            AddTile(TileID.LunarCraftingStation).
            Register();
    }
}}