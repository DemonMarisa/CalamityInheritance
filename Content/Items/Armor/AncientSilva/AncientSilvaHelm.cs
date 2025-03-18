using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Armor.Silva;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Items.Armor.AncientSilva
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientSilvaHelm : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor"; 
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare =ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 20; //100
        }
        
       

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            bool isSet = body.type == ModContent.ItemType<AncientSilvaArmor>() && legs.type == ModContent.ItemType<AncientSilvaLeggings>();
            return isSet;
        }
        
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            var usPlayer = player.CIMod();
            CalamityPlayer calPlayer = player.Calamity();
            usPlayer.AncientSilvaSet = true;
            usPlayer.AncientSilvaStat = true;
            usPlayer.SilvaRebornMark = true;
            usPlayer.AuricSilvaSet = true;
            calPlayer.wearingRogueArmor = true;
            calPlayer.rogueStealthMax += 1.25f;
            calPlayer.WearingPostMLSummonerSet = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");
        }
        
        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 5;
            player.statLifeMax2 += 200;
            player.GetDamage<GenericDamageClass>() += 0.25f;
            player.GetCritChance<GenericDamageClass>() += 0.25f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SilvaHeadMagic>().
                AddIngredient<SilvaHeadSummon>().
                AddIngredient<EffulgentFeather>(25).
                AddIngredient<PlantyMush>(25).
                AddIngredient<AscendantSpiritEssence>(10).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}