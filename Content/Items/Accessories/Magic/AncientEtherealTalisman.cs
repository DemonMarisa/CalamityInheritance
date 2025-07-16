using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Magic
{
    public class AncientEtherealTalisman : CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.Magic";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.accessory = true;
            Item.rare = ModContent.RarityType<DeepBlue>();
        }
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            return Item.type.SetConflictMod<EtherealTalisman>(equippedItem, incomingItem);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.eTalisman = true;

            player.manaMagnet = true;
            if (!hideVisual)
                player.manaFlower = true;

            player.statManaMax2 += 250;
            player.GetDamage<MagicDamageClass>() += 0.30f;
            player.manaCost -= 0.2f;
            player.GetCritChance<MagicDamageClass>() += 30;
            player.pStone = true;
            player.lifeRegen += 6;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SigilofCalamitas>().
                AddRecipeGroup("AnyManaFlower").
                AddIngredient(ItemID.CharmofMyths).
                AddIngredient(ItemID.LunarBar, 8).
                AddIngredient<GalacticaSingularity>(4).
                AddIngredient<AscendantSpiritEssence>(4).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
