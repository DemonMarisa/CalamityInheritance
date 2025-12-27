using CalamityMod.Items.Materials;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod;
using CalamityMod.Items.Placeables.Abyss;

namespace CalamityInheritance.Content.Items.Armor.Silva
{
    [AutoloadEquip(EquipType.Head)]
    public class SilvaHeadMelee : CIArmor, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 24;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 52; //96
            Item.rare = ModContent.RarityType<DeepBlue>();
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<SilvaArmorold>() && legs.type == ModContent.ItemType<SilvaLeggingsold>();
        public override void UpdateArmorSet(Player player)
        {
            var usPlayer = player.CIMod();
            usPlayer.SilvaFakeDeath = true;
            usPlayer.SilvaMeleeSetLegacy = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");

            player.Calamity().WearingPostMLSummonerSet = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage<MeleeDamageClass>() += 0.13f;
            player.GetCritChance<MeleeDamageClass>() += 13;
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
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
