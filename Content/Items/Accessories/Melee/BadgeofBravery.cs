using CalamityInheritance.Content.Items.Armor.AncientTarragon;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Tarragon;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Melee
{
    public class BadgeofBravery : CIAccessories, ILocalizedModType
    {

        public new string LocalizationCategory => "Content.Items.Accessories.Melee";
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:30,
            itemHeight:30,
            itemRare:ModContent.RarityType<BlueGreen>(),
            itemValue:CIShopValue.RarityPriceBlueGreen
        );
        public override void ExSSD()
        {
            Type.ShimmerEach<BadgeofBravery>();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer1 = player.CIMod();
            player.GetAttackSpeed<MeleeDamageClass>() += 0.15f;
            modPlayer1.BraveBadge = true;
            bool helm =
                player.armor[0].type == ModContent.ItemType<TarragonHeadMelee>() ||
                player.armor[0].type == ModContent.ItemType<AncientTarragonHelm>();
            bool chest = 
                player.armor[1].type == ModContent.ItemType<TarragonBreastplate>() ||
                player.armor[1].type == ModContent.ItemType<AncientTarragonBreastplate>();
            bool legs = 
                player.armor[2].type == ModContent.ItemType<TarragonLeggings>() ||
                player.armor[2].type == ModContent.ItemType<AncientTarragonLeggings>();
            if (helm && chest && legs)
            {
                player.GetCritChance<MeleeDamageClass>() += 10;
                player.GetDamage<MeleeDamageClass>() += 0.10f;
                player.GetArmorPenetration<MeleeDamageClass>() += 15; 
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.FeralClaws).
                AddIngredient(ItemID.Leather, 3).
                AddIngredient<UelibloomBar>(2).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
