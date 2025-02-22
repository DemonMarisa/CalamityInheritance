using Terraria.ModLoader;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using Terraria;
using CalamityInheritance.Content.Items.Armor.AuricTesla;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Tiles.Furniture.CraftingStations;
using CalamityMod.CalPlayer.Dashes;

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
			modPlayer.GodSlayerReborn = true;
			modPlayer.auricsilvaset = true;
			modPlayer.GodSlayerDMGprotect = true;
			modPlayer.godSlayerReflect = true;
			modPlayer.auricBoostold = true;
			modPlayer.auricYharimSet = true;

            calPlayer.WearingPostMLSummonerSet = true;

            player.thorns += 10f;
			player.ignoreWater = true;
			player.crimsonRegen = true;

			player.lavaImmune = true;
			player.ignoreWater = true;
			if (player.lavaWet == true)
			{
				player.statDefense += 30;
				player.lifeRegen += 60;
			}

            if (calPlayer.godSlayerDashHotKeyPressed || player.dashDelay != 0 && calPlayer.LastUsedDashID == GodslayerArmorDash.ID)
            {
                calPlayer.DeferredDashID = GodslayerArmorDash.ID;
                player.dash = 0;
            }

			//法师
            calPlayer.tarraMage = true;
            calPlayer.bloodflareMage = true;
            modPlayer.silvaMageold = true;
            modPlayer.godSlayerMagic = true;
			//战士
            calPlayer.tarraMelee = true;
            calPlayer.bloodflareMelee = true;
            calPlayer.godSlayerDamage = true;
            modPlayer.silvaMelee = true;
			//射手
            calPlayer.tarraRanged = true;
            modPlayer.godSlayerRangedold = true;
            modPlayer.silvaRanged = true;
            modPlayer.AuricbloodflareRangedSoul = true;
            if (player.HeldItem.useTime > 3 && player.HeldItem.DamageType == DamageClass.Ranged)
            {
                player.GetAttackSpeed<RangedDamageClass>() += 0.2f;
            }
            //盗贼
            calPlayer.tarraThrowing = true;
            calPlayer.bloodflareThrowing = true;
            calPlayer.godSlayerThrowing = true;
            modPlayer.silvaRogue = true;
            //召唤
            modPlayer.godSlayerSummonold = true;
        }
		
		public override void UpdateEquip(Player player)
		{
			player.maxMinions += 10;
			player.maxTurrets += 5;
			player.GetDamage<GenericDamageClass>() += 0.3f;
			player.GetCritChance<GenericDamageClass>() += 30;
		}

		public override void AddRecipes()
		{
			if (CalamityInheritanceConfig.Instance.LegendaryitemsRecipes == true)
			{
				CreateRecipe().
				AddIngredient<AuricTeslaHeadMagic>().
				AddIngredient<AuricTeslaHeadMelee>().
				AddIngredient<AuricTeslaHeadRogue>().
				AddIngredient<AuricTeslaHeadRanged>().
				AddIngredient<AuricTeslaHeadSummon>().
				AddIngredient<AuricBarold>(12).
				AddTile<DraedonsForgeold>().
				Register();
			}
        }
	}
}