using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using Terraria;

namespace CalamityInheritance.Content.Items.Armor.YharimAuric
{
	[AutoloadEquip(EquipType.Head)]
	public class YharimAuricTeslaHelm : ModItem, ILocalizedModType
	{

        public new string LocalizationCategory => "Content.Items.Armor";
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = CIShopValue.RarityMaliceDrop;
			Item.rare = ModContent.RarityType<MaliceChallengeDrop>();
			Item.defense = 40; //132
		}


        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
			return body.type == ModContent.ItemType<YharimAuricTeslaBodyArmor>() &&
				   legs.type == ModContent.ItemType<YharimAuricTeslaCuisses>();
        }
		
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}

		public override void UpdateArmorSet(Player player)
		{
			var modPlayer = player.CalamityInheritance();
			var calPlayer = player.Calamity();
            player.setBonus = this.GetLocalizedValue("SetBonus");
			calPlayer.bloodflareSet = true;
			calPlayer.tarraSet = true;
			calPlayer.godSlayer = true;
			calPlayer.auricSet = true;
			calPlayer.wearingRogueArmor = true;
			calPlayer.rogueStealthMax += 1.2f;
			calPlayer.stealthStrikeHalfCost = true;
			modPlayer.GodSlayerReborn = true;
			modPlayer.auricsilvaset = true;
			modPlayer.GodSlayerDMGprotect = true;
			modPlayer.godSlayerReflect = true;
			modPlayer.auricBoostold = true;
			modPlayer.auricYharimSet = true;

			player.thorns += 10f;
			player.ignoreWater = true;
			player.crimsonRegen = true;

			player.lavaImmune = true;
			player.ignoreWater = true;
			if (player.lavaWet == true)
			{
				player.statDefense += 30;
				player.lifeRegen += 10;
			}
		}
		
		public override void UpdateEquip(Player player)
		{
			player.maxMinions += 3;
			player.maxTurrets += 6;
			player.GetDamage<GenericDamageClass>() += 0.3f;
			player.GetCritChance<GenericDamageClass>() += 30;
		}

		// public override void AddRecipes()
		// {
		// 	Recipe recipe = CreateRecipe();
		// 	recipe.AddIngredient(null, "SilvaHelm");
		// 	recipe.AddIngredient(null, "GodSlayerHelm");
		// 	recipe.AddIngredient(null, "BloodflareMask");
		// 	recipe.AddIngredient(null, "TarragonHelm");
		// 	recipe.AddIngredient(null, "EndothermicEnergy", 200);
		// 	recipe.AddIngredient(null, "NightmareFuel", 200);
		// 	recipe.AddIngredient(null, "Phantoplasm", 70);
		// 	recipe.AddIngredient(null, "DarksunFragment", 30);
		// 	recipe.AddIngredient(null, "BarofLife", 20);
		// 	recipe.AddIngredient(null, "CoreofCalamity", 15);
		// 	recipe.AddIngredient(null, "GalacticaSingularity", 10);
		// 	recipe.AddIngredient(null, "PsychoticAmulet");
		// 	recipe.AddTile(null, "DraedonsForge");
		// 	recipe.Register();
		// }
	}
}