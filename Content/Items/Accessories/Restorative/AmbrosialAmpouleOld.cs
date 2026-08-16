using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Items.Accessories.Defense;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Accessories.Restorative
{
    public class AmbrosialAmpouleOld : CIAccessories
    {
        public override int AccessoriesStyle => Restorative;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.defense = 4;
            Item.width = Item.height = 20;
            Item.rare = ItemRarityID.Red;
            Item.value = CIShopValue.RarityPriceRed;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddDR(0.05f);
            player.pickSpeed *= 0.5f;
            player.CI().LifeRegen += 16;
            player.honey = true;
            player.CI().BeeFriendly = true;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Venom] = true;
            player.buffImmune[BuffID.Frozen] = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frostburn] = true;
            player.buffImmune[BuffID.Frostburn2] = true;
            if (!hideVisual)
                Lighting.AddLight(player.Center, new Vector3(1.2f, 1.2f, 0.72f));
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CorruptFlask>().
                AddIngredient<HoneyDewLegacy>().
                AddIngredient<CryoBar>(5). //修改为冰灵锭
                AddTile(TileID.Anvils).
                Register();

            CreateRecipe().
                AddIngredient<CrimsonFlask>().
                AddIngredient<HoneyDewLegacy>().
                AddIngredient<CryoBar>(5). //修改为冰灵锭
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
