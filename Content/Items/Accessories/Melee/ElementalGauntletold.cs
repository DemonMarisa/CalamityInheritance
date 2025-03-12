using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Melee
{

    [AutoloadEquip([EquipType.HandsOn, EquipType.HandsOff])]
    public class ElementalGauntletold : ModItem, ILocalizedModType
    {

        public new string LocalizationCategory => "Content.Items.Accessories.Melee";
        public override void SetStaticDefaults()
        {
            
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 38;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<DeepBlue>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer1 = player.CalamityInheritance();
            modPlayer1.ElemGauntlet = true;
            player.GetDamage<MeleeDamageClass>() += 0.15f;
            player.GetCritChance<MeleeDamageClass>() += 15;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.15f;
            player.kbGlove = true;
            player.autoReuseGlove = true;
            player.meleeScaleGlove = true;
            player.GetDamage<TrueMeleeDamageClass>() += 0.25f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.FireGauntlet).
                AddIngredient(ItemID.LunarBar, 8).
                AddIngredient<YharimsInsignia>().
                AddIngredient<GalacticaSingularity>(4).
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
