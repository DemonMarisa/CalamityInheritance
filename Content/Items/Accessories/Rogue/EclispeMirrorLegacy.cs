using System.Collections.Generic;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using ReLogic.Peripherals.RGB.SteelSeries;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Rogue
{
    public class EclispeMirrorLegacy : CIAccessories, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories.Rogue";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 38;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var usPlayer = player.CIMod();
            var calPlayer = player.Calamity();
            calPlayer.stealthGenStandstill += 0.30f;
            calPlayer.stealthGenMoving += 0.30f;
            calPlayer.rogueStealthMax += 0.30f;
            calPlayer.stealthStrikeHalfCost = true;
            calPlayer.wearingRogueArmor = true;
            player.GetCritChance<RogueDamageClass>() += 30;
            usPlayer.EMirror = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            float iShowSpeed = (player.GetTotalCritChance<RogueDamageClass>() + 4) - 100f;
            iShowSpeed /= 7f;
            string showStat = this.GetLocalization("ShowCritsBounes").Format(iShowSpeed.ToString("N1"));
            tooltips.FindAndReplace("[SHOW]", showStat);
            base.ModifyTooltips(tooltips);
        }
    }
}