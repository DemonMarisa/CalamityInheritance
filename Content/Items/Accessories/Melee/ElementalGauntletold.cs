using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Melee
{

    [AutoloadEquip([EquipType.HandsOn, EquipType.HandsOff])]
    public class ElementalGauntletold : CIAccessories, ILocalizedModType
    {

        public new string LocalizationCategory => "Content.Items.Accessories.Melee";
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:22,
            itemHeight:38,
            itemRare: RarityType<DeepBlue>(),
            itemValue:CIShopValue.RarityPriceDeepBlue,
            itemDefense:10
        );
        public override void ExSSD()
        {
            Type.ShimmerEach<ElementalGauntlet>(false);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer1 = player.CIMod();
            modPlayer1.ElemGauntlet = true;
            modPlayer1.YharimsInsignia = true;
            player.GetDamage<MeleeDamageClass>() += 0.30f;
            player.GetCritChance<MeleeDamageClass>() += 15;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.15f;
            player.kbGlove = true;
            player.autoReuseGlove = true;
            player.meleeScaleGlove = true;
            player.GetDamage<TrueMeleeDamageClass>() += 0.15f;
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
