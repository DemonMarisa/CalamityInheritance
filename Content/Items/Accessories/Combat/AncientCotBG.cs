using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Combat
{
    public class AncientCotBG : CIAccessories
    {
        public override int AccessoriesStyle => Combat;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 26;
            Item.rare = RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.LAP().MaxLifeMultiplier += 0.1f;
            player.CI().FleshTotem = true;
            player.AddDR(0.05f);
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
        }
    }
}
