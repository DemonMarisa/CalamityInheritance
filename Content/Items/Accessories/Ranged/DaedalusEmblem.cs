using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Ranged
{
    public class DaedalusEmblem : CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.Ranged";
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:46,
            itemHeight:40,
            itemRare:ItemRarityID.Lime,
            itemValue:CIShopValue.RarityPriceLime,
            itemDefense:10
        );
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            if (!modPlayer.deadshotBrooch)
                player.CIMod().DeadshotBroochCI = true;
            player.GetDamage<RangedDamageClass>() += 0.15f;
            player.GetCritChance<RangedDamageClass>() += 5;
            player.pickSpeed -= 0.15f;
            player.lifeRegen += 2;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.CelestialStone).
                AddIngredient<CoreofCalamity>(2).
                AddIngredient(ItemID.RangerEmblem).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
