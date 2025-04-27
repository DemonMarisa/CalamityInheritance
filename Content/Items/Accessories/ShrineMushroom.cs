using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityMod;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class ShrineMushroom: CIAccessories, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 36;
            Item.rare = ItemRarityID.Orange;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            player.GetDamage<TrueMeleeDamageClass>() += 0.25f;
            player.CIMod().SMushroom = true;
        }
    }
}