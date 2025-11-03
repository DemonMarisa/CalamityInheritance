using CalamityMod.Items.Armor.Silva;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Items.Placeables;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Armor.Silva
{
    [AutoloadEquip(EquipType.Head)]
    public class SilvaHeadMagicold : CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 21; //110
            Item.rare = ModContent.RarityType<DeepBlue>();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<SilvaArmorold>() && legs.type == ModContent.ItemType<SilvaLeggingsold>();
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            var modPlayer1 = player.CIMod();
            modPlayer1.SilvaFakeDeath = true;
            modPlayer1.SilvaMagicSetLegacy = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");
        }

        public override void UpdateEquip(Player player)
        {
            player.manaCost *= 0.81f;
            player.GetDamage<MagicDamageClass>() += 0.13f;
            player.GetCritChance<MagicDamageClass>() += 13;
            player.statManaMax2 += 100;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PlantyMush>(6).
                AddIngredient<EffulgentFeather>(5).
                AddIngredient(ModContent.ItemType<DarksunFragment>(), 10).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
