using CalamityInheritance.Content.Items.Armor.Silva;
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
    public class AncientSilvaHelm : CIArmor, ILocalizedModType
    {
         
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare =ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 20; //100
        }
        
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<AncientSilvaArmor>() && legs.type == ModContent.ItemType<AncientSilvaLeggings>();

        public override void UpdateArmorSet(Player player)
        {
            var usPlayer = player.CIMod();
            CalamityPlayer calPlayer = player.Calamity();
            usPlayer.AncientSilvaForceRegen = true;
            usPlayer.AncientSilvaStat = true;
            usPlayer.AncientSilvaFakeDeath = true;
            calPlayer.wearingRogueArmor = true;
            calPlayer.rogueStealthMax += 1.25f;
            calPlayer.WearingPostMLSummonerSet = true;
            calPlayer.healingPotionMultiplier += 0.30f;
            player.lifeRegen += 24; //+12HP/s
            player.lifeRegenTime = 2000;
            player.setBonus = this.GetLocalizedValue("SetBonus");
        }
        
        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 3;
            player.maxTurrets += 2;
            player.statLifeMax2 += 200;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SilvaHeadMagicold>().
                AddIngredient<SilvaHeadSummonold>().
                AddIngredient<SilvaHeadMelee>().
                AddIngredient<SilvaHeadRanged>().
                AddIngredient<SilvaHeadRogue>().
                AddIngredient<EffulgentFeather>(40).
                AddIngredient<PlantyMush>(75).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}