using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.Buffs;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Restorative
{
    public class BloodPactLegacy : CIAccessories, ILocalizedModType
    {
        public override int AccessoriesStyle => Restorative;
        public override void SetDefaults()
        {
            Item.width = Item.height = 26;
            Item.rare = ItemRarityID.Yellow;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.LAP().MaxLifeMultiplier += 1f;
        }
        public static void ModfiyHurt(Player player, ref Player.HurtModifiers modifiers, ref float damageMult)
        {
            if (Main.rand.NextBool(4))
            {
                player.AddBuff(BuffType<BloodyBoost>(), 600);
                damageMult += 1.25f;
            }
        }
    }
}
