using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using LAP.Core.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Combat
{
    public class DarkSunRingold : CIAccessories
    {
        public override int AccessoriesStyle => Combat;
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 6));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 32;
            Item.rare = RarityType<DeepBlue>();
            Item.defense = 10;
            Item.value = CIShopValue.RarityPriceDeepBlue;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<GenericDamageClass>() += 0.12f;
            player.GetAttackSpeed<MeleeDamageClass>() += 0.12f;
            player.LAP().LifeRegen += 2;
            player.maxMinions += 2;
            player.pickSpeed -= 0.12f;
            if (Main.dayTime)
                player.LAP().LifeRegen += 6;
            if (Main.eclipse || !Main.dayTime)
                player.statDefense += 30;
        }

        public override void AddRecipes()
        {
            if (HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.UelibloomBar, 10).
                    AddIngredient(CalamityMaterials.DarksunFragment, 100).
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
        }
    }
}
