using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class PlagueHive : CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:42,
            itemHeight:48,
            itemRare:ItemRarityID.Cyan,
            itemValue:CIShopValue.RarityPriceCyan
        );

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.toxicHeart = true;
            modPlayer.alchFlask = true;
            player.CIMod().PlagueHive = true;
            player.honeyCombItem = Item;
            player.strongBees = true;
            player.buffImmune[BuffType<Plague>()] = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ToxicHeart>().
                // AddIngredient<AlchemicalFlask>().
                AddIngredient(ItemID.HiveBackpack).
                AddIngredient(ItemID.HoneyComb).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
