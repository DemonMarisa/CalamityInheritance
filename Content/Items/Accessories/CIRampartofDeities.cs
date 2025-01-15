using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class CIRampartofDeities : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 62;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.defense = 18;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer1 = player.CalamityInheritance();
            player.pStone = true;
            player.longInvince =true;
            modPlayer1.deificAmuletEffect = true; //启用神圣护符的加成。
            modPlayer1.RoDPaladianShieldActive = true; //启用帕拉丁盾
            player.lifeRegen += 3;

            if (player.statLife <= player.statLifeMax2 * 0.5)
                player.AddBuff(BuffID.IceBarrier, 5);
            player.noKnockback = true;
            
        }

        public override void AddRecipes()
        {
            CreateRecipe(). //要考虑转移时期吗bro
                AddIngredient(ItemID.FrozenShield).
                AddIngredient<DeificAmulet>().
                AddIngredient<AuricBar>(5).
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddIngredient(ItemID.FrozenShield).
                AddIngredient<DeificAmuletLegacy>().
                AddIngredient<AuricBar>(5).
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register();

            CreateRecipe().
                AddIngredient<RampartofDeities>().
                Register();
        }
    }
}
