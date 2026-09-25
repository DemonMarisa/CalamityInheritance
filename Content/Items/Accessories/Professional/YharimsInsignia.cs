using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Professional
{
    public class YharimsInsignia : CIAccessories
    {
        public override int AccessoriesStyle => Professional;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 32;
            Item.rare = RarityType<BlueGreen>();
            Item.value = CIShopValue.RarityPricePurple;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CI().DealHolyFire = true;
            player.LAP().ExImmuneTime += 40;
            player.lavaMax = 600;
            player.GetDamage<MeleeDamageClass>() += 0.15f;
            player.BoostTrueMelee(0.15f);
            if (player.statLife <= (int)(player.statLifeMax2 * 0.5))
                player.GetDamage<GenericDamageClass>() += 0.1f;
        }
    }
}
