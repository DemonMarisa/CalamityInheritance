using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class AncientCotBG: CIAccessories, ILocalizedModType
    {

        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:48,
            itemHeight:48,
            itemRare: RarityType<DeepBlue>(),
            itemValue:CIShopValue.RarityPriceDeepBlue
        );
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();
            usPlayer.LifeMaxPercentBoost += 0.1f;
            var calPlayer = player.Calamity();
            calPlayer.fleshTotem = true;
            player.endurance += 0.05f;
            player.GetDamage<GenericDamageClass>() += 0.05f;
            if (player.statLife <= (int)(player.statLifeMax2 * 0.5f))
            {
                player.endurance += 0.05f;
                player.GetDamage<GenericDamageClass>() += 0.1f;
                if (player.statLife <= (int)(player.statLifeMax2 * 0.15f))
                {
                    player.endurance += 0.10f;
                    player.GetDamage<GenericDamageClass>() += 0.20f;
                }
            }
            if (player.statDefense <= 100)
            {
                player.GetDamage<GenericDamageClass>() += 0.20f;
                player.GetAttackSpeed<GenericDamageClass>() += 0.1f;
            }
        }
        public override void AddRecipes()
        {   
            //Scarlet:旧血核与旧血契的加入已经没有必要微光转化了
            CreateRecipe().
                AddIngredient<BloodPactLegacy>().
                AddIngredient<BloodflareCoreLegacy>().
                AddIngredient<BloodyWormScarf>().
                AddIngredient<CosmiliteBar>(5).
                AddIngredient<Necroplasm>(5).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
