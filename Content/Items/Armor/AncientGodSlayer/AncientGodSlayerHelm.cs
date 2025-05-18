using CalamityInheritance.CIPlayer;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Items.Armor.GodSlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientGodSlayer
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientGodSlayerHelm : CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.defense = 30; //130
        }
        

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<AncientGodSlayerChestplate>() && legs.type == ModContent.ItemType<AncientGodSlayerLeggings>();

        public override void UpdateArmorSet(Player player)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();
            CalamityPlayer calPlayer = player.Calamity();
            calPlayer.wearingRogueArmor = true;
            calPlayer.WearingPostMLSummonerSet = true;
            calPlayer.godSlayer = true;
            calPlayer.rogueStealthMax += 1.25f;
            usPlayer.AncientGodSlayerSet = true;
            usPlayer.AncientGodSlayerStat = true;
            usPlayer.GodSlayerReborn = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");
            if (calPlayer.godSlayerDashHotKeyPressed || player.dashDelay != 0 && calPlayer.LastUsedDashID == GodslayerArmorDash.ID)
            {
                calPlayer.DeferredDashID = GodslayerArmorDash.ID;
                if (usPlayer.AncinetGodSlayerDashReset)
                    calPlayer.rogueStealth = calPlayer.rogueStealthMax / 4 * 3;
                
                player.dash = 0;
            }
        }
        
        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 5;
            player.maxTurrets += 3;
            player.statLifeMax2 += 200;
            player.GetDamage<GenericDamageClass>() += 0.25f;
            player.GetCritChance<GenericDamageClass>() += 0.25f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<GodSlayerHeadMelee>().
                AddIngredient<GodSlayerHeadRanged>().
                AddIngredient<GodSlayerHeadRogue>().
                AddIngredient<CosmiliteBar>(25).
                AddIngredient<AscendantSpiritEssence>(10).
                AddTile<CosmicAnvil>().
                Register();
                
        }
    }
}