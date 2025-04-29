using Terraria.ModLoader;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using Terraria;
using CalamityInheritance.Content.Items.Armor.AuricTesla;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Tiles.Furniture.CraftingStations;
using CalamityMod.CalPlayer.Dashes;
using CalamityInheritance.Content.Items.Armor.AncientTarragon;
using CalamityInheritance.Content.Items.Armor.AncientBloodflare;
using CalamityInheritance.Content.Items.Armor.AncientGodSlayer;
using CalamityInheritance.Content.Items.Armor.AncientSilva;
using CalamityInheritance.System.Configs;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;

namespace CalamityInheritance.Content.Items.Armor.AncientAuric
{
	[AutoloadEquip(EquipType.Head)]
	public class YharimAuricTeslaHelm : CIArmor, ILocalizedModType
	{
    	
		public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = CIShopValue.RarityMaliceDrop;
			Item.rare = ModContent.RarityType<MaliceChallengeDrop>();
			Item.defense = 20; //150
		}


		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<YharimAuricTeslaBodyArmor>() &&
				   legs.type == ModContent.ItemType<YharimAuricTeslaCuisses>();
		}

		public override void UpdateArmorSet(Player player)
		{
			var modPlayer = player.CIMod();
			var calPlayer = player.Calamity();
			modPlayer.ManaHealMutipler = 2.0f;
			player.setBonus = this.GetLocalizedValue("SetBonus");
			//标记为魔君金源甲
			modPlayer.YharimAuricSet = true;
			#region 灾厄的月后套通用效果
			calPlayer.tarraSet = true;
			calPlayer.bloodflareSet = true;
			calPlayer.godSlayer = true;
			// 去除了原灾金源套的判定，没啥用，而且还有巨难看的残影
			modPlayer.AuricSilvaFakeDeath = true; //林海自活
			#endregion
			#region 标记为盗贼套装 
			calPlayer.wearingRogueArmor = true;
			calPlayer.WearingPostMLSummonerSet = true;
			//继承制盗贼弑神盔甲
			calPlayer.rogueStealthMax += 1.50f;
			
			#endregion
			#region 弑神自活, 反伤, 弑神冲刺
			//弑神自活与反伤
			player.thorns += 10f;
			modPlayer.GodSlayerReborn = true;
			modPlayer.GodSlayerDMGprotect = true;
			modPlayer.GodSlayerReflect = true;
			if (calPlayer.godSlayerDashHotKeyPressed || player.dashDelay != 0 && calPlayer.LastUsedDashID == GodslayerArmorDash.ID)
			{
				calPlayer.DeferredDashID = GodslayerArmorDash.ID;
				player.dash = 0;
			}
			#endregion
			#region 血腥回血, 血炎岩浆泡澡
			player.crimsonRegen = true;
			player.ignoreWater = true;
			player.lavaImmune = true;
			if (player.lavaWet == true) //血炎岩浆泡澡
			{
				player.statDefense += 30;
				player.lifeRegen += 60;
			}
			#endregion
			#region 远古甲的弑神免伤, 龙蒿降防损, 林海强回血与血炎掉红心
			//弑神免伤
			calPlayer.contactDamageReduction +=0.25f;
			player.endurance += 0.15f;
			//远古龙蒿降防损
			calPlayer.defenseDamageRatio *= 0.5f;
			//远古血炎产红心, 远古林海强回血整合在这里面
			modPlayer.AncientSilvaForceRegen = true;
			modPlayer.AncientAuricSet = true;
			#endregion
			#region 五职业头盔套的各自套装效果
			modPlayer.auricBoostold = true;
			//战士
			calPlayer.tarraMelee = true;
			calPlayer.bloodflareMelee = true;
			modPlayer.GodSlayerMelee = true;
			modPlayer.SilvaMeleeSetLegacy = true;
			//射手
			calPlayer.tarraRanged = true;
			modPlayer.GodSlayerRangedSet = true;
			modPlayer.SilvaRangedSetLegacy = true;
			modPlayer.AuricbloodflareRangedSoul = true;
			if (player.HeldItem.useTime > 3 && player.HeldItem.DamageType == DamageClass.Ranged)
            player.GetAttackSpeed<RangedDamageClass>() += 0.2f;
			//法师
			calPlayer.tarraMage = true;
			calPlayer.bloodflareMage = true;
			modPlayer.SilvaMagicSetLegacy = true;
			modPlayer.GodSlayerMagicSet = true;
				//召唤
			modPlayer.GodSlayerSummonSet = true;
			//盗贼
			calPlayer.tarraThrowing = true;
			calPlayer.bloodflareThrowing = true;
			calPlayer.godSlayerThrowing = true;
			modPlayer.SilvaRougeSetLegacy = true;
			#endregion
    	}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            var mplr = player.CIMod();
            if(mplr.AncientAuricSet)
            {
                if (Main.keyState.IsKeyDown(Keys.LeftAlt))
                {
                    string Details = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Armor.YharimAuricTeslaHelm.Details");
                    tooltips.Add(new TooltipLine(Mod, "Details", Details));
                }
				else if (Main.keyState.IsKeyDown(Keys.LeftControl))
				{
                    string ExtraDetails = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Armor.YharimAuricTeslaHelm.ExtraDetails");
                    tooltips.Add(new TooltipLine(Mod, "Details", ExtraDetails));
				}
            }
        }
		public override void UpdateEquip(Player player)
		{
			player.maxMinions += 10;
			player.maxTurrets += 5;
			player.statLifeMax2 += 600;
			player.GetDamage<GenericDamageClass>() += 0.40f;
			player.GetCritChance<GenericDamageClass>() += 40;
		}

		public override void AddRecipes()
		{
			if (CIServerConfig.Instance.LegendaryitemsRecipes == true)
			{
				CreateRecipe().
				AddIngredient<AuricTeslaHeadMagic>().
				AddIngredient<AuricTeslaHeadMelee>().
				AddIngredient<AuricTeslaHeadRogue>().
				AddIngredient<AuricTeslaHeadRanged>().
				AddIngredient<AuricTeslaHeadSummon>().
				AddIngredient<AncientTarragonHelm>().
				AddIngredient<AncientBloodflareMask>().
				AddIngredient<AncientGodSlayerHelm>().
				AddIngredient<AncientSilvaHelm>().
				AddIngredient<AuricBarold>(12).
                AddIngredient<CalamitousEssence>(1).
                AddTile<DraedonsForgeold>().
				Register();
			}
   		}
	}
}