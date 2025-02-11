using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using CalamityInheritance.Content.Items.Materials;

namespace CalamityInheritance.Content.Items.Armor.Xeroc
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientXerocPlateMail : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceCyan;
            Item.rare = ItemRarityID.Cyan;
            Item.defense = 24;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<GenericDamageClass>() += 0.1f;
            player.GetCritChance<GenericDamageClass>() += 10;
            player.manaCost *= 0.8f;
            player.statLifeMax2 += 80;
            player.statManaMax2 += 60;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<NebulaBar>(12).
                AddIngredient<GalacticaSingularity>(8).
                AddTile(TileID.LunarCraftingStation).
                Register();
                
        }
    }
}