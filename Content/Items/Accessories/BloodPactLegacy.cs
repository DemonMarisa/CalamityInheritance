using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class BloodPactLegacy : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";
        public override void SetStaticDefaults()
        {
            
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.Yellow;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.CalamityInheritance();
            modPlayer.ancientBloodFact= true;
        }
    }
}
