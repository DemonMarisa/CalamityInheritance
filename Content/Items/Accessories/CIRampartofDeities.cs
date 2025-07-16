using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;
using CalamityMod.Items.Accessories;

namespace CalamityInheritance.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class CIRampartofDeities : CIAccessories, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 62;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.defense = 18;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<CatalystViolet>();
        }
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            return Item.type.SetConflictMod<RampartofDeities>(equippedItem, incomingItem);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var usPlayer= player.CIMod();
            player.pStone = true;
            player.longInvince =true;
            usPlayer.RoDPaladianShieldActive = true; //启用帕拉丁盾
            player.lifeRegen += 4;
            player.GetArmorPenetration<GenericDamageClass>() += 50;
            if (player.statLife <= player.statLifeMax2 * 0.5)
                player.AddBuff(BuffID.IceBarrier, 5);
            player.noKnockback = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(). //
                AddIngredient(ItemID.FrozenShield).
                AddIngredient<CosmiliteBar>(10).
                AddRecipeGroup("CalamityInheritance:AnyDeificAmulet").
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
