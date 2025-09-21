using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Summon;
using CalamityMod.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Magic
{
    public class ManaOverloader : CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.Magic";
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:50,
            itemHeight:28,
            itemRare:ItemRarityID.Red,
            itemValue:CIShopValue.RarityPriceRed
        );
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var usPlayer = player.CIMod();
            player.GetCritChance<MagicDamageClass>() += 15;
            player.statManaMax2 += 75;
            usPlayer.OverloadManaPower = true;
            if (player.statMana > player.statManaMax2)
                //-3 HP/s
                player.lifeRegen -= 6;
        }
        public override void AddRecipes() =>
            CreateRecipe().
                AddIngredient<ManaPolarizer>().
                AddIngredient(ItemID.LunarBar, 10).
                AddIngredient<GalacticaSingularity>(5).
                AddTile(TileID.LunarCraftingStation).
                Register();
    }
}