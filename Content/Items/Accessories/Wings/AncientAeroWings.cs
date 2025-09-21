using CalamityInheritance.Content.Items.Armor.AncientAero;
using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    public class AncientAeroWings : CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.Wings";
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:22,
            itemHeight:32,
            itemRare:ItemRarityID.Orange,
            itemValue:CIShopValue.RarityPriceOrange
        );
        public override void ExSSD()
        {
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(80, 6.5f, 1f);
            Type.ShimmerEach<SkylineWings>(false);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noFallDmg = true;
            if (player.armor[0].type == ModContent.ItemType<AncientAeroHelm>() &&
                player.armor[1].type == ModContent.ItemType<AncientAeroArmor>() && 
                player.armor[2].type == ModContent.ItemType<AncientAeroLeggings>())
            {
                player.CIMod().AncientAeroWingsPower = true;
            }
        }
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.5f;
            ascentWhenRising = 0.1f;
            maxCanAscendMultiplier = 0.5f;
            maxAscentMultiplier = 1.5f;
            constantAscend = 0.1f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 6.25f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AerialiteBar>(10).
                AddIngredient(ItemID.SunplateBlock, 10).
                AddIngredient(ItemID.Feather, 10).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}