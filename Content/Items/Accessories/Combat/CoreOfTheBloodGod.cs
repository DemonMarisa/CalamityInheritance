using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.CDs;
using CalamityInheritance.Content.Items.Accessories.Restorative;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Combat
{
    public class CoreOfTheBloodGod : CIAccessories, ILocalizedModType
    {
        public override int AccessoriesStyle => Combat;
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 48;
            Item.rare = RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<GenericDamageClass>() += 0.12f;
            player.AddDR(0.2f);
            player.LAP().MaxLifeMultiplier += 0.25f;
            player.LAP().healingPotionMult += 0.25f;
            player.CI().ContactDamageReduction *= 0.85f;
            player.CI().FleshTotem = true;
            player.CI().LifeRegen += 8;
            if (!player.HasCD<CotbgTotem>())
                player.CI().ContactDamageReduction *= 0.5f;
        }
        public static void FleshTotem_PreHurt(Player player)
        {
            if (!player.HasCD<CotbgTotem>())
                player.AddCD(LAPContent.CDType<CotbgTotem>(), SecondsToFrames(20));
        }
        public override void AddRecipes()
        {
            //CreateRecipe().
            //    AddIngredient(ItemType<BloodPactLegacy>()).
            //    AddIngredient<BloodyWormScarf>().
            //    AddIngredient<FleshTotem>().
            //    AddIngredient<CosmiliteBar>(5).
            //    AddIngredient<AscendantSpiritEssence>(4).
            //    AddTile<CosmicAnvil>().
            //    Register();
        }
    }
}
