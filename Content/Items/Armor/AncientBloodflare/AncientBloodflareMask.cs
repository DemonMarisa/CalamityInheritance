using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Materials;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Rarity;
using Terraria.GameContent;
using CalamityMod;

namespace CalamityInheritance.Content.Items.Armor.AncientBloodflare
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientBloodflareMask : CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceBlueGreen;
            Item.rare = ModContent.RarityType<BlueGreen>();
            Item.defense = 20; //80
        }
        
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<AncientBloodflareBodyArmor>() && legs.type == ModContent.ItemType<AncientBloodflareCuisses>();
        public override void UpdateArmorSet(Player player)
        {
            CalamityInheritancePlayer usPlayer = player.CIMod();
            var calPlayer = player.Calamity();
            usPlayer.AncientBloodflareSet = true;
            calPlayer.rogueStealthMax += 1.20f;
            calPlayer.wearingRogueArmor = true;
            calPlayer.WearingPostMLSummonerSet = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");
            player.crimsonRegen = true;
            player.aggro += 900;
            //血炎数值
            calPlayer.healingPotionMultiplier += 0.35f; 
            player.lifeRegen += 10;
            if (player.statLife <= player.statLifeMax2 / 2)
                player.lifeRegen += 16;

        }
        
        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 4;
            player.maxTurrets += 3;
            player.statLifeMax2 += 100;
            player.GetDamage<GenericDamageClass>() += 0.15f;
            player.GetCritChance<GenericDamageClass>() += 15f;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BloodflareHeadMelee>().
                AddIngredient<BloodflareHeadRanged>().
                AddIngredient<BloodflareHeadMagic>().
                AddIngredient<BloodflareHeadSummon>().
                AddIngredient<BloodflareHeadRogue>().
                AddIngredient<BloodstoneCore>(15).
                AddIngredient<RuinousSoul>(15).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}