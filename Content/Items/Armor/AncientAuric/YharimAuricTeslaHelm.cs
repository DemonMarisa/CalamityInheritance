using Terraria.ModLoader;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using Terraria;
using CalamityMod.CalPlayer.Dashes;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using CalamityInheritance.Buffs.Statbuffs;
using Terraria.ID;
using System;
using CalamityMod.Buffs.DamageOverTime;

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
			Item.defense = 100; //150
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<YharimAuricTeslaBodyArmor>() && legs.type == ModContent.ItemType<YharimAuricTeslaCuisses>();
		public override void UpdateArmorSet(Player player)
		{
			var modPlayer = player.CIMod();
			var calPlayer = player.Calamity();
			modPlayer.ManaHealMutipler = 2.0f;
			player.setBonus = this.GetLocalizedValue("SetBonus");
			//标记为魔君金源甲
			modPlayer.YharimAuricSet = true;
			#region 灾厄的月后套通用效果
			player.Calamity().WearingPostMLSummonerSet = true;
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
			calPlayer.rogueStealthMax += 2.00f;

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
				if (modPlayer.AncinetGodSlayerDashReset)
				{
					calPlayer.rogueStealth = calPlayer.rogueStealthMax * 0.99f;
					//每次冲刺时，提供魔君之怒Buff。并刷新其消失前的CD。
					modPlayer.PerunofYharimCooldown = 1800;
					//魔君的弑神冲刺触发时，每次叠加一层魔君之怒的时候提供一个很难得到的短CD
					//因为出于某些原因弑神冲刺一旦触发会直接从0快速跳变至3.
					if (modPlayer.AncientAuricDashCounter < 3 && modPlayer.AncientAuricDashCache <= 0)
					{
						modPlayer.AncientAuricDashCounter += 1;
						modPlayer.AncientAuricDashCache = 30;
					}
					player.AddBuff(ModContent.BuffType<yharimOfPerun>(), 1800);

				}
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
			calPlayer.infiniteFlight = true;
			#region 龙蒿降防损, 林海强回血与血炎掉红心
			//远古龙蒿降防损
			calPlayer.defenseDamageRatio *= 0.5f;
			//远古血炎产红心, 远古林海强回血整合在这里面
			modPlayer.AncientSilvaForceRegen = true;
			modPlayer.AncientAuricSet = true;
			modPlayer.RefreshGodSlayerDash(calPlayer);
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
			ImmuneDebuffSoul(player);
			ImmuneDebuffSoulDLC(player);
			ImmnueDebuffCalamity(player);
			
    	}

        private void ImmnueDebuffCalamity(Player player)
        {
			foreach (int debuff in CalamityLists.debuffList)
				player.buffImmune[debuff] = true;
        }

        private void ImmuneDebuffSoulDLC(Player player)
        {
			if (!ModLoader.TryGetMod("FargowiltasCrossmod", out Mod DLC))
				return;
			player.ImmnueDebuff(DLC, "CalamitousPresenceBuff");
        }

		private void ImmuneDebuffSoul(Player player)
		{
			if (!ModLoader.TryGetMod("FargowiltasSouls", out Mod fargoSoul))
				return;
			player.ImmnueDebuff(fargoSoul, "MutantPresenceBuff");
			player.ImmnueDebuff(fargoSoul, "MutantFangBuff");
			player.ImmnueDebuff(fargoSoul, "AbomPresenceBuff");
			player.ImmnueDebuff(fargoSoul, "AbomFangBuff");
			player.ImmnueDebuff(fargoSoul, "MutantDesperationBuff");
			player.ImmnueDebuff(fargoSoul, "MoonFangBuff");
			player.ImmnueDebuff(fargoSoul, "OceanicMaulBuff");
			player.ImmnueDebuff(fargoSoul, "CurseoftheMoonBuff");
			player.ImmnueDebuff(fargoSoul, "AntisocialBuff");
			player.ImmnueDebuff(fargoSoul, "AtrophiedBuff");
			player.ImmnueDebuff(fargoSoul, "LightningRodBuff");
			player.ImmnueDebuff(fargoSoul, "FusedBuff");
			player.ImmnueDebuff(fargoSoul, "MutantNibbleBuff");
			player.ImmnueDebuff(fargoSoul, "HexedBuff");
			player.ImmnueDebuff(fargoSoul, "DefenselessBuff");
			player.ImmnueDebuff(fargoSoul, "HolyPriceBuff");
			player.ImmnueDebuff(fargoSoul, "ClippedWingsBuff");
			player.ImmnueDebuff(fargoSoul, "LethargicBuff");
			player.ImmnueDebuff(fargoSoul, "JammedBuff");
			player.ImmnueDebuff(fargoSoul, "UnstableBuff");
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            var mplr = player.CIMod();
            if(mplr.AncientAuricSet)
            {
                if (Main.keyState.IsKeyDown(Keys.LeftAlt))
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
	}
	public static class ImmnueMethods
	{
		internal static void ImmnueDebuff(this Player player, Mod otherMod, string buffName)
		{
			player.buffImmune[otherMod.Find<ModBuff>(buffName).Type] = true;
		}
	}
}