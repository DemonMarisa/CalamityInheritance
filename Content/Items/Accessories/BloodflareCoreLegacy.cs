using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class BloodflareCoreLegacy : ModItem, ILocalizedModType
    {

        public new string LocalizationCategory => "Content.Items.Accessories";
        public override void SetStaticDefaults()
        {
            if(CIConfig.Instance.CustomShimmer == true)
             //开启微光转化后，灵魂边锋与虚空边锋可以用微光相互转化
            {
                 }
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = ModContent.RarityType<BlueGreen>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var usPlayer = player.CalamityInheritance();
            usPlayer.bloodflareCoreLegacy = true;
        }
    }
}
